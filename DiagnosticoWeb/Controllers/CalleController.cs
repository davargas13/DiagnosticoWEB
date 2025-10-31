using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OfficeOpenXml;
using DiagnosticoWeb.Code;
using DiagnosticoWeb.Database;
using DiagnosticoWeb.Models;
using DiagnosticoWeb.Validaciones;
using EFCore.BulkExtensions;
using Microsoft.AspNetCore.Http;

namespace DiagnosticoWeb.Controllers
{
    public class CalleController : Controller
    {
        public readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="context">Conexion a la base de datos</param>
        /// <param name="userManager">Administrador de la sesion para identificar que usuario esta usando el sistema</param>
        public CalleController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        
        /// <summary>
        /// Funcion que muestra la vista con el listado de localidades registradas en la base de datos
        /// </summary>
        /// <returns>Vista con el listado de localidades</returns>
        [HttpGet]
        [Authorize]
        [RequireClaim("Permiso", Value = "catalogos.ver")]
        public IActionResult Index()
        {
            var request = new CalleRequest();
            var importacion = _context.Importacion.Find("Calles");
            if (importacion != null)
            {
                request.Usuario = _context.Users.Find(importacion.UsuarioId).Name;
                request.ImportedAt = importacion.ImportedAt.ToString();
            }

            return View("Index", request);
        }
        
        /// <summary>
        /// Funcion que busca en la base de datos las localidades registradas de acuerdo a los filtros de busqueda
        /// seleccionados por el usuario
        /// </summary>
        /// <param name="request">Filtros de busqueda</param>
        /// <returns>Cadena JSON con el listado de localidades</returns>
        [HttpPost]
        [Authorize]
        public string GetCalles([FromBody] CalleRequest request)
        {
            var response = new CalleResponse();
            var callesQuery = _context.Calle.Where(l => l.DeletedAt == null);
            if (!string.IsNullOrEmpty(request.Nombre))
            {
                callesQuery = callesQuery.Where(e => e.Nombre.Contains(request.Nombre));
            }
            if (!string.IsNullOrEmpty(request.MunicipioId))
            {
                callesQuery = callesQuery.Where(e => e.MunicipioId == request.MunicipioId);
            }
            if (!string.IsNullOrEmpty(request.LocalidadId))
            {
                callesQuery = callesQuery.Where(e => e.LocalidadId == request.LocalidadId);
            }

            response.Total = callesQuery.Count();
            response.Calles = callesQuery.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize).Include(l => l.Municipio)
                .Include(l=>l.Localidad)
                .ToList();
            return JsonSedeshu.SerializeObject(response);
        }
        
        /// <summary>
        /// Funcion que importa el catalogo de localidades desde un archivo de EXCEL
        /// </summary>
        /// <param name="request">Archivo de EXCEL</param>
        /// <returns>Estatus de la importacion: ok, error</returns>
        [HttpPost]
        [Authorize]
        [DisableRequestSizeLimit]
        public string Importar(ImportarCalleRequest model)
        {
            if (model.Calles == null)
            {
                ModelState.AddModelError("Error", "Se debe seleccionar al menos un archivo");
            }

            if (!ModelState.IsValid)
            {
                var errores = ModelState.Select(x => new {Key = x.Key, Error = x.Value.Errors.First().ErrorMessage})
                    .ToList();

                return JsonConvert.SerializeObject(errores);
            }
            
            var now = DateTime.Now;
            var importacion = _context.Importacion.FirstOrDefault(i => i.Clave.Equals("Calles"));
            var nuevaImportacion = importacion == null;
            if (nuevaImportacion)
            {
                importacion = new Importacion();
            }
            importacion.Clave = "Calles";
            importacion.ImportedAt = now;
            importacion.UsuarioId = _userManager.GetUserId(HttpContext.User);
            if (nuevaImportacion)
            {
                _context.Importacion.Add(importacion);
            }
            else
            {
                _context.Importacion.Update(importacion);
            }

            _context.Bitacora.Add(new Bitacora
            {
                UsuarioId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                Accion = AccionBitacora.IMPORTACION,
                Mensaje = "Se importó el catálogo de calles.",
                CreatedAt = now,
                UpdatedAt = now
            });

            _context.SaveChanges();
            var status = BulkInsert(now, model.Calles);
            TempData["Alert"] = "Show";
            TempData.Keep("Alert");
            return status;
        }
        
        /// <summary>
        /// Función que inserta o actualiza las calles del archivo de excel utilizando bulkinsert
        /// </summary>
        /// <param name="now">Fecha actual</param>
        /// <param name="archivo">Archivo de excel</param>
        /// <returns>Cadena con estatus de importacion</returns>
        public string BulkInsert(DateTime now, IFormFile archivo)
        {
            try
            {
                using (var package = new ExcelPackage(archivo.OpenReadStream()))
                {
                    var archivoExcel = package.Workbook;
                    if (archivoExcel != null)
                    {
                        if (archivoExcel.Worksheets.Count > 0)
                        {
                          using (var transaction = _context.Database.BeginTransaction()) 
                          { 
                              var callesNuevas = new Dictionary<string, Calle>(); 
                              var callesAnteriores = new Dictionary<string, Calle>(); 
                              var pestana = archivoExcel.Worksheets.First();
                              var callesDB = _context.Calle.ToDictionary(e => e.Id);
                              var localidadesAnteriores = _context.Localidad.ToDictionary(e => e.Id);
                              foreach (var calle in callesDB)
                              {
                                  if (!localidadesAnteriores.ContainsKey(calle.Value.LocalidadId))
                                  {
                                      continue;
                                  }
                                  calle.Value.UpdatedAt = now;
                                  calle.Value.DeletedAt = now;
                                  callesAnteriores.Add(calle.Key, calle.Value);
                              }
                              for (var i = 2; i <= pestana.Dimension.End.Row; i++)
                              {
                                  if (pestana.Cells[i,1].Value==null)
                                  {
                                      continue;
                                      
                                  }
                                  var id = pestana.Cells[i, 1].Value.ToString();
                                  Calle calle;
                                  if (!callesAnteriores.ContainsKey(id))
                                  {
                                      calle = new Calle
                                      {
                                          CreatedAt = now
                                          
                                      };
                                      callesNuevas.Add(id, calle);
                                  }
                                  else
                                  {
                                      calle = callesAnteriores[id];
                                  }
                                  calle.Id = id;
                                  calle.Nombre = pestana.Cells[i, 2].Value.ToString();
                                  calle.MunicipioId = pestana.Cells[i, 3].Value.ToString();
                                  calle.LocalidadId = pestana.Cells[i, 4].Value.ToString();
                                  calle.UpdatedAt = now;
                                  calle.DeletedAt = null;
                              }
                              var bulkConfig = new BulkConfig {SetOutputIdentity = true, BatchSize = 4000};
                              _context.BulkUpdate(callesAnteriores.Values.ToList(), bulkConfig);
                              _context.BulkInsert(callesNuevas.Values.ToList(), bulkConfig);
                              transaction.Commit();
                          }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Excepcion.Registrar(e);
                ModelState.AddModelError("File",
                    "Ocurrió un error importando las calles. Revise el archivo de excel.");
                var errors = ModelState.Select(x => new {Key = x.Key, Error = x.Value.Errors.First().ErrorMessage})
                    .ToList();

                return JsonConvert.SerializeObject(errors);
            }
            return "ok";
        }
        
        /// <summary>
        /// Funcion que muestra la vista para la adicion o edicion de una localidad
        /// </summary>
        /// <param name="id">Identificador en base de datos de la localidad, null si es nueva</param>
        /// <returns>Vista con el formulario para editar los datos de la localidad</returns>
        [HttpGet]
        [Authorize]
        public IActionResult CreateEdit(string id = "")
        {
            var calle = new CalleCreateEditModel();
            if (string.IsNullOrEmpty(id))
            {
                ViewData["Title"] = "Crear calle";
                calle.Id = "";
                calle.IdAnterior = "";
                calle.Nombre = "";
                calle.MunicipioId = null;
                calle.LocalidadId = null;
            }
            else
            {
                var calleDB = _context.Calle.Find(id);
                ViewData["Title"] = "Editar calle";
                calle.Id = calleDB.Id;
                calle.IdAnterior = calleDB.Id;
                calle.Nombre = calleDB.Nombre;
                calle.MunicipioId = calleDB.MunicipioId;
                calle.LocalidadId = calleDB.LocalidadId;
            }

            calle.Municipios = _context.Municipio.ToList();
            calle.Localidades = new List<Localidad>();
            return View("Create", calle);
        }
        
        /// <summary>
        /// Funcion para guardar en la base de datos la informacion de la localidad
        /// </summary>
        /// <param name="model">Datos de la localidad</param>
        /// <returns>Estatus del guardado: ok, error</returns>
        [HttpPost]
        [Authorize]
        [RequireClaim("Permiso", Value = "catalogos.editar")]
        public string Guardar([FromBody] CalleCreateEditModel model)
        {
            if (_context.Calle.Any(x => x.DeletedAt == null && x.Id == model.Id && x.Id != model.IdAnterior))
            {
                ModelState.AddModelError("Id", "Este Id ya fue creado.");
            }

            if (_context.Calle.Any(x =>
                x.Nombre == model.Nombre && x.Id != model.Id && x.MunicipioId.Equals(model.MunicipioId)))
            {
                ModelState.AddModelError("Nombre", "Esta calle ya fue creada.");
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Select(x => new {Key = x.Key, Error = x.Value.Errors.First().ErrorMessage})
                    .ToList();

                return JsonConvert.SerializeObject(errors);
            }

            using (var transaction = _context.Database.BeginTransaction())
            {
                var now = DateTime.Now;
                var calleDB = _context.Calle.Find(model.Id);
                if (calleDB == null)
                {
                    calleDB = new Calle
                    {
                        Id = model.Id,
                        Nombre = model.Nombre,
                        MunicipioId = model.MunicipioId,
                        LocalidadId = model.LocalidadId,
                        CreatedAt = now,
                        UpdatedAt = now
                    };
                    _context.Calle.Add(calleDB);

                    _context.Bitacora.Add(new Bitacora()
                    {
                        UsuarioId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                        Accion = AccionBitacora.INSERCION,
                        Mensaje = "Se insertó la calle " + model.Nombre + ".",
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    });
                }
                else
                {
                    calleDB.Id = model.Id;
                    calleDB.Nombre = model.Nombre;
                    calleDB.MunicipioId = model.MunicipioId;
                    calleDB.LocalidadId = model.LocalidadId;
                    calleDB.UpdatedAt = now;
                    calleDB.DeletedAt = null;
                    _context.Calle.Update(calleDB);

                    _context.Bitacora.Add(new Bitacora()
                    {
                        UsuarioId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                        Accion = AccionBitacora.EDICION,
                        Mensaje = "Se modificó la calle " + model.Nombre + ".",
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    });
                }

                _context.SaveChanges();
                transaction.Commit();
            }

            TempData["Alert"] = "Show";
            TempData.Keep("Alert");
            return "ok";
        }

        [HttpPost]
        [Authorize]
        public IActionResult Exportar(CalleRequest request)
        {
            using (var excel = new ExcelPackage())
            {
                var callesQuery = _context.Calle.Where(a => a.DeletedAt == null);
                if (!string.IsNullOrEmpty(request.Nombre))
                {
                    callesQuery = callesQuery.Where(e => e.Nombre.Contains(request.Nombre));
                }
                if (!string.IsNullOrEmpty(request.MunicipioId))
                {
                    callesQuery = callesQuery.Where(e => e.MunicipioId == request.MunicipioId);
                }
                if (!string.IsNullOrEmpty(request.LocalidadId))
                {
                    callesQuery = callesQuery.Where(e => e.LocalidadId == request.LocalidadId);
                }

                excel.Workbook.Worksheets.Add("Agebs");
                var pestana = excel.Workbook.Worksheets.First();
                pestana.Cells[1, 1].Value = "Id";
                pestana.Cells[1, 2].Value = "Nombre";
                pestana.Cells[1, 3].Value = "MunicipioId";                
                pestana.Cells[1, 4].Value = "LocalidadId";
                var callesDB = callesQuery.ToList();
                var row = 2;
                foreach (var calle in callesDB)
                {
                    pestana.Cells[row, 1].Value = calle.Id;
                    pestana.Cells[row, 2].Value = calle.Nombre;
                    pestana.Cells[row, 3].Value = calle.MunicipioId;                                        
                    pestana.Cells[row++, 4].Value = calle.LocalidadId;
                }

                _context.Bitacora.Add(new Bitacora()
                {
                    UsuarioId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                    Accion = AccionBitacora.EXPORTACION,
                    Mensaje = "Se exportó el catálogo de calles.",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                });
                _context.SaveChanges();

                return File(
                    fileContents: excel.GetAsByteArray(),
                    contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    fileDownloadName: "calles.xlsx"
                );
            }
        }
    }
}