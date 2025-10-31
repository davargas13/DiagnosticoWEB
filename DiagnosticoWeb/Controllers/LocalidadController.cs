using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
using Microsoft.Extensions.Configuration;

namespace DiagnosticoWeb.Controllers
{
    /// <summary>
    /// Clase con la logica de negocio para la visualizacion,adicion, edicion o importacion del catalogo de localidades
    /// </summary>
    public class LocalidadController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="context">Conexion a la base de datos</param>
        /// <param name="userManager">Variable para identificar que usuario esta usando el sistema</param>
        public LocalidadController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, 
            IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _configuration = configuration;
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
            var request = new LocalidadRequest();
            var importacion = _context.Importacion.Find("Localidades");
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
        public string GetLocalidades([FromBody] LocalidadRequest request)
        {
            var response = new LocalidadResponse();
            var localidadesQuery = _context.Localidad.Where(l => l.DeletedAt == null);
            if (!string.IsNullOrEmpty(request.Clave))
            {
                localidadesQuery = localidadesQuery.Where(e => e.Clave.Equals(request.Clave));
            }

            if (!string.IsNullOrEmpty(request.Nombre))
            {
                localidadesQuery = localidadesQuery.Where(e => e.Nombre.Contains(request.Nombre));
            } 
            
            if (!string.IsNullOrEmpty(request.MunicipioId))
            {
                localidadesQuery = localidadesQuery.Where(e => e.MunicipioId == request.MunicipioId);
            }

            response.Total = localidadesQuery.Count();
            response.Localidades = localidadesQuery.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize).Include(l => l.Municipio).ToList();
            return JsonSedeshu.SerializeObject(response);
        }

        /// <summary>
        /// Funcion que importa el catalogo de localidades desde un archivo de EXCEL
        /// </summary>
        /// <param name="request">Archivo de EXCEL</param>
        /// <returns>Estatus de la importacion: ok, error</returns>
        [HttpPost]
        [Authorize]
        public string Importar(ImportarLocalidadRequest model)
        {
            if (!ModelState.IsValid)
            {
                var errores = ModelState.Select(x => new {Key = x.Key, Error = x.Value.Errors.First().ErrorMessage})
                    .ToList();

                return JsonConvert.SerializeObject(errores);
            }
            
            var now = DateTime.Now;
            var importacion = _context.Importacion.FirstOrDefault(i => i.Clave.Equals("Localidades"));
            var nuevaImportacion = importacion == null;
            if (nuevaImportacion)
            {
                importacion = new Importacion();
            }
            importacion.Clave = "Localidades";
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
                Mensaje = "Se importó el catálogo de localidades.",
                CreatedAt = now,
                UpdatedAt = now
            });

            _context.SaveChanges();
            var status = BulkInsert(now, model.Localidades);
            TempData["Alert"] = "Show";
            TempData.Keep("Alert");
            return status;
        }
        
        /// <summary>
        /// Función que inserta o actualiza las localidades del archivo de excel utilizando bulkinsert
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
                              var localidadesNuevas = new Dictionary<string, Localidad>(); 
                              var pestana = archivoExcel.Worksheets.First();
                              var localidadesAnteriores = _context.Localidad.ToDictionary(e => e.Id);
                              foreach (var localidad in localidadesAnteriores)
                              {
                                  localidad.Value.UpdatedAt = now;
                                  localidad.Value.DeletedAt = now;
                              }
                              for (var i = 2; i <= pestana.Dimension.End.Row; i++)
                              {
                                  if (pestana.Cells[i,1].Value==null)
                                  {
                                      continue;
                                      
                                  }
                                  var id = pestana.Cells[i, 1].Value.ToString();
                                  Localidad localidad;
                                  if (!localidadesAnteriores.ContainsKey(id))
                                  {
                                      localidad = new Localidad
                                      {
                                          CreatedAt = now
                                          
                                      };
                                      localidadesNuevas.Add(id, localidad);
                                  }
                                  else
                                  {
                                      localidad = localidadesAnteriores[id];
                                  }
                                  localidad.Id = pestana.Cells[i, 1].Value.ToString();
                                  localidad.Clave = pestana.Cells[i, 4].Value.ToString();
                                  localidad.Nombre = pestana.Cells[i, 5].Value.ToString();
                                  localidad.MunicipioId = pestana.Cells[i, 3].Value.ToString();
                                  localidad.EstadoId = pestana.Cells[i, 2].Value.ToString();
                                  localidad.UpdatedAt = now;
                                  localidad.DeletedAt = null;
                              }
                              var bulkConfig = new BulkConfig {SetOutputIdentity = true, BatchSize = 4000};
                              _context.BulkUpdate(localidadesAnteriores.Values.ToList(), bulkConfig);
                              _context.BulkInsert(localidadesNuevas.Values.ToList(), bulkConfig);
                              transaction.Commit();
                          }
                        }
                    }
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("File",
                    "Ocurrió un error importando las localidades. Revise el archivo de excel.");
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
            var localidad = new LocalidadCreateEditModel();
            var localidadDB = _context.Localidad.Find(id);
            var nueva = localidadDB == null;
            ViewData["Title"] = nueva ? "Crear localidad":"Editar localidad";
            
            localidad.Id = nueva ? "" : localidadDB.Id;
            localidad.IdAnterior = nueva ? "" : localidadDB.Id;
            localidad.Nombre = nueva ? "" :localidadDB.Nombre;
            localidad.Clave = nueva ? "" :localidadDB.Clave;
            localidad.MunicipioId = nueva ? "" :localidadDB.MunicipioId;
            localidad.EstadoId = _configuration["GUANAJUATO"];
            return View("Create", localidad);
        }


        /// <summary>
        /// Funcion para guardar en la base de datos la informacion de la localidad
        /// </summary>
        /// <param name="model">Datos de la localidad</param>
        /// <returns>Estatus del guardado: ok, error</returns>
        [HttpPost]
        [Authorize]
        [RequireClaim("Permiso", Value = "catalogos.editar")]
        public string Guardar([FromBody] LocalidadCreateEditModel model)
        {
            if (_context.Localidad.Any(x => x.DeletedAt == null && x.Id == model.Id && x.Id != model.IdAnterior))
            {
                ModelState.AddModelError("Id", "Este Id ya fue creado.");
            }

            if (_context.Localidad.Any(x =>
                x.Nombre == model.Nombre && x.Id != model.Id && x.MunicipioId.Equals(model.MunicipioId)))
            {
                ModelState.AddModelError("Nombre", "Esta localidad ya fue creada.");
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Select(x => new {Key = x.Key, Error = x.Value.Errors.First().ErrorMessage})
                    .ToList();

                return JsonConvert.SerializeObject(errors);
            }

            using (var transaction = _context.Database.BeginTransaction())
            {
                var nueva = false;
                var now = DateTime.Now;
                var localidadDB = _context.Localidad.Find(model.Id);
                if (localidadDB == null)
                {
                    nueva = true;
                    localidadDB = new Localidad
                    {
                        CreatedAt = now
                    };
                }

                localidadDB.Id = model.Id;
                localidadDB.Nombre = model.Nombre;
                localidadDB.Clave = model.Clave;
                localidadDB.MunicipioId = model.MunicipioId;
                localidadDB.EstadoId = _configuration["GUANAJUATO"];
                localidadDB.CreatedAt = now;
                localidadDB.UpdatedAt = now;
                localidadDB.DeletedAt = null;
                
                if (nueva)
                {
                    _context.Localidad.Add(localidadDB);
                }
                else
                {
                    _context.Localidad.Update(localidadDB);
                }

                _context.Bitacora.Add(new Bitacora()
                {
                    UsuarioId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                    Accion = nueva ? AccionBitacora.INSERCION : AccionBitacora.EDICION,
                    Mensaje = nueva ? "Se insertó la localidad " + model.Nombre + ".":"Se modificó la localidad " + model.Nombre + ".",
                    CreatedAt = now,
                    UpdatedAt = now
                });
                _context.SaveChanges();
                transaction.Commit();
            }

            TempData["Alert"] = "Show";
            TempData.Keep("Alert");
            return "ok";
        }

        /// <summary>
        /// Funcion para consultar las localidades del municipio seleccionado
        /// </summary>
        /// <param name="id">Identificador en base de datos del municipio seleccionado</param>
        /// <returns>Cadena JSON con el listado de localidades pertenecientes al municipio</returns>
        [HttpGet]
        [Authorize]
        public string ByMunicipio(string id = "")
        {
            var localidades = _context.Localidad.Where(l => l.MunicipioId.Equals(id)).OrderBy(l=>l.Nombre).ToList();
            return JsonSedeshu.SerializeObject(localidades);
        }
        
        [HttpGet("/Localidad/ByMunicipios/{municipioIds}")]
        [Authorize]
        public string ByMunicipios(string municipioIds = "")
        {
            string[] arrMunicipioIds = municipioIds.Split(new char[] { ',' });
            var municipios = _context.Municipio
                .Include(m => m.Localidades)
                .Where(m => arrMunicipioIds.Contains(m.Id)).ToList();
            var locList = new Dictionary<string, List<Localidad>>();
            foreach (var municipio in municipios)
            {
                locList.Add(municipio.Nombre, municipio.Localidades);
            }
            return JsonSedeshu.SerializeObject(locList);
        }

        /// <summary>
        /// Funcion que exporta a un archivo de EXCEL las localidades registradas en la base de datos de acuerdo a los
        /// filtros de busqueda seleccionados por el usuario
        /// </summary>
        /// <param name="id">Filtros de busqueda</param>
        /// <returns>Archivo de EXCEL con el listado de localidades</returns>
        [HttpPost]
        [Authorize]
        public IActionResult Exportar(LocalidadRequest request)
        {             
            using (var excel = new ExcelPackage())
            {
                var localidadesQuery = _context.Localidad.Where(x => x.DeletedAt == null);
                if (!string.IsNullOrEmpty(request.Clave))
                {
                    localidadesQuery = localidadesQuery.Where(e => e.Clave.Equals(request.Clave));
                }

                if (!string.IsNullOrEmpty(request.Nombre))
                {
                    localidadesQuery = localidadesQuery.Where(e => e.Nombre.Contains(request.Nombre));
                } 
            
                if (!string.IsNullOrEmpty(request.MunicipioId))
                {
                    localidadesQuery = localidadesQuery.Where(e => e.MunicipioId == request.MunicipioId);
                }

                excel.Workbook.Worksheets.Add("Localidades");
                var pestana = excel.Workbook.Worksheets.First();
                pestana.Cells[1, 1].Value = "Id";
                pestana.Cells[1, 2].Value = "Estado";
                pestana.Cells[1, 3].Value = "Municipio";
                pestana.Cells[1, 4].Value = "Clave";
                pestana.Cells[1, 5].Value = "Nombre";
                var localidades = localidadesQuery.ToList();
                var row = 2;
                foreach (var localidad in localidades)
                {
                    pestana.Cells[row, 1].Value = localidad.Id;
                    pestana.Cells[row, 2].Value = localidad.EstadoId;
                    pestana.Cells[row, 3].Value = localidad.MunicipioId;
                    pestana.Cells[row, 4].Value = localidad.Clave;
                    pestana.Cells[row++, 5].Value = localidad.Nombre;
                }

                _context.Bitacora.Add(new Bitacora()
                {
                    UsuarioId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                    Accion = AccionBitacora.EXPORTACION,
                    Mensaje = "Se exportó el catálogo de localidades.",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                });
                _context.SaveChanges();

                return File(
                    fileContents: excel.GetAsByteArray(),
                    contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    fileDownloadName: "localidades.xlsx"
                );
            }
        }
    }
}