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

namespace DiagnosticoWeb.Controllers
{
    /// <summary>
    /// Controlador que contiene la logica de negocio para listar, crear, editar e importar agebs al catalogo
    /// </summary>
    public class ColoniaController : Controller
    {
        public readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="context">Conexion a la base de datos</param>
        /// <param name="userManager">Administrador de la sesion para identificar que usuario esta usando el sistema</param>
        public ColoniaController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        
        /// <summary>
        /// Funcion que muestra la vista con el listado de colonias registradas en la base de datos
        /// </summary>
        /// <returns>Vista con el listado de colonias</returns>
        [HttpGet]
        [Authorize]
        [RequireClaim("Permiso", Value = "catalogos.ver")]
        public IActionResult Index()
        {
            var request = new ColoniaRequest();
            var importacion = _context.Importacion.Find("Colonias");
            if (importacion != null)
            {
                request.Usuario = _context.Users.Find(importacion.UsuarioId).Name;
                request.ImportedAt = importacion.ImportedAt.ToString();
            }

            return View("Index", request);
        }
        
        /// <summary>
        /// Funcion que busca en la base de datos las colonias registradas de acuerdo a los filtros de busqueda
        /// seleccionados por el usuario
        /// </summary>
        /// <param name="request">Filtros de busqueda</param>
        /// <returns>Cadena JSON con el listado de colonias</returns>
        [HttpPost]
        [Authorize]
        public string GetColonias([FromBody] ColoniaRequest request)
        {
            var response = new ColoniaResponse();
            var coloniasQuery = _context.Colonia.Where(l => l.DeletedAt == null);
            if (!string.IsNullOrEmpty(request.Nombre))
            {
                coloniasQuery = coloniasQuery.Where(e => e.Nombre.Contains(request.Nombre));
            }

            if (!string.IsNullOrEmpty(request.CodigoPostal))
            {
                coloniasQuery = coloniasQuery.Where(e => e.CodigoPostal.Contains(request.CodigoPostal));
            }
            
            if (!string.IsNullOrEmpty(request.MunicipioId))
            {
                coloniasQuery = coloniasQuery.Where(e => e.MunicipioId==request.MunicipioId);
            }

            response.Total = coloniasQuery.Count();
            response.Colonias = coloniasQuery.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize).Include(l => l.Municipio).ToList();
            return JsonSedeshu.SerializeObject(response);
        }
        
        /// <summary>
        /// Funcion que importa el catalogo de colonias desde un archivo de EXCEL
        /// </summary>
        /// <param name="request">Archivo de EXCEL</param>
        /// <returns>Estatus de la importacion: ok, error</returns>
        [HttpPost]
        [Authorize]
        public string Importar(ImportarColoniaRequest model)
        {
            if (model.Colonias == null)
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
            var importacion = _context.Importacion.FirstOrDefault(i => i.Clave.Equals("Colonias"));
            var nuevaImportacion = importacion == null;
            if (nuevaImportacion)
            {
                importacion = new Importacion();
            }
            importacion.Clave = "Colonias";
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
                Mensaje = "Se importó el catálogo de colonias.",
                CreatedAt = now,
                UpdatedAt = now
            });

            _context.SaveChanges();
            var status = BulkInsert(now, model.Colonias);
            TempData["Alert"] = "Show";
            TempData.Keep("Alert");
            return status;
        }
        
        /// <summary>
        /// Función que inserta o actualiza las colonias del archivo de excel utilizando bulkinsert
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
                              var coloniasNuevas = new Dictionary<string, Colonia>(); 
                              var pestana = archivoExcel.Worksheets.First();
                              var coloniasAnteriores = _context.Colonia.ToDictionary(e => e.Id);
                              foreach (var colonia in coloniasAnteriores)
                              {
                                  colonia.Value.UpdatedAt = now;
                                  colonia.Value.DeletedAt = now;
                              }
                              for (var i = 2; i <= pestana.Dimension.End.Row; i++)
                              {
                                  if (pestana.Cells[i,1].Value==null)
                                  {
                                      continue;
                                      
                                  }
                                  var id = pestana.Cells[i, 1].Value.ToString();
                                  Colonia colonia;
                                  if (!coloniasAnteriores.ContainsKey(id))
                                  {
                                      colonia = new Colonia
                                      {
                                          CreatedAt = now
                                          
                                      };
                                      coloniasNuevas.Add(id, colonia);
                                  }
                                  else
                                  {
                                      colonia = coloniasAnteriores[id];
                                  }
                                  colonia.Nombre = pestana.Cells[i, 2].Value.ToString();
                                  colonia.CodigoPostal = pestana.Cells[i, 3].Value.ToString();
                                  colonia.MunicipioId = pestana.Cells[i, 4].Value.ToString();
                                  colonia.UpdatedAt = now;
                                  colonia.DeletedAt = null;
                              }
                              var bulkConfig = new BulkConfig {SetOutputIdentity = true, BatchSize = 4000};
                              _context.BulkUpdate(coloniasAnteriores.Values.ToList(), bulkConfig);
                              _context.BulkInsert(coloniasNuevas.Values.ToList(), bulkConfig);
                              transaction.Commit();
                          }
                        }
                    }
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("File",
                    "Ocurrió un error importando las colonias. Revise el archivo de excel.");
                var errors = ModelState.Select(x => new {Key = x.Key, Error = x.Value.Errors.First().ErrorMessage})
                    .ToList();

                return JsonConvert.SerializeObject(errors);
            }
            return "ok";
        }
         
         /// <summary>
        /// Funcion que muestra la vista para la adicion o edicion de una colonia
        /// </summary>
        /// <param name="id">Identificador en base de datos de la colonia, null si es nueva</param>
        /// <returns>Vista con el formulario para editar los datos de la colonia</returns>
        [HttpGet]
        [Authorize]
        public IActionResult CreateEdit(string id = "")
        {
            var colonia = new ColoniaCreateEditModel();
            if (string.IsNullOrEmpty(id))
            {
                ViewData["Title"] = "Crear colonia";
                colonia.Id = "";
                colonia.IdAnterior = "";
                colonia.Nombre = "";
                colonia.CodigoPostal = "";
                colonia.MunicipioId = null;
            }
            else
            {
                var coloniaDB = _context.Colonia.Find(id);
                ViewData["Title"] = "Editar colonia";
                colonia.Id = coloniaDB.Id;
                colonia.IdAnterior = coloniaDB.Id;
                colonia.Nombre = coloniaDB.Nombre;
                colonia.CodigoPostal = coloniaDB.CodigoPostal;
                colonia.MunicipioId = coloniaDB.MunicipioId;
            }

            colonia.Municipios = _context.Municipio.ToList();
            return View("Create", colonia);
        }


        /// <summary>
        /// Funcion para guardar en la base de datos la informacion de la colonia
        /// </summary>
        /// <param name="model">Datos de la colonia</param>
        /// <returns>Estatus del guardado: ok, error</returns>
        [HttpPost]
        [Authorize]
        [RequireClaim("Permiso", Value = "catalogos.editar")]
        public string Guardar([FromBody] ColoniaCreateEditModel model)
        {
            if (_context.Colonia.Any(x => x.DeletedAt == null && x.Id == model.Id && x.Id != model.IdAnterior))
            {
                ModelState.AddModelError("Id", "Este Id ya fue creado.");
            }

            if (_context.Colonia.Any(x =>
                x.Nombre == model.Nombre && x.Id != model.Id && x.MunicipioId.Equals(model.MunicipioId)))
            {
                ModelState.AddModelError("Nombre", "Esta colonia ya fue creada.");
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
                var coloniaDB = _context.Colonia.Find(model.Id);
                if (coloniaDB == null)
                {
                    coloniaDB = new Colonia();
                    coloniaDB.Id = model.Id;
                    coloniaDB.Nombre = model.Nombre;
                    coloniaDB.CodigoPostal = model.CodigoPostal;
                    coloniaDB.MunicipioId = model.MunicipioId;
                    coloniaDB.CreatedAt = now;
                    coloniaDB.UpdatedAt = now;
                    _context.Colonia.Add(coloniaDB);

                    _context.Bitacora.Add(new Bitacora()
                    {
                        UsuarioId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                        Accion = AccionBitacora.INSERCION,
                        Mensaje = "Se insertó la colonia " + model.Nombre + ".",
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    });
                }
                else
                {
                    coloniaDB.Id = model.Id;
                    coloniaDB.Nombre = model.Nombre;
                    coloniaDB.CodigoPostal = model.CodigoPostal;
                    coloniaDB.MunicipioId = model.MunicipioId;
                    coloniaDB.UpdatedAt = now;
                    coloniaDB.DeletedAt = null;
                    _context.Colonia.Update(coloniaDB);

                    _context.Bitacora.Add(new Bitacora()
                    {
                        UsuarioId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                        Accion = AccionBitacora.EDICION,
                        Mensaje = "Se modificó la colonia " + model.Nombre + ".",
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
        public IActionResult Exportar(ColoniaRequest request)
        {
            using (var excel = new ExcelPackage())
            {
                var coloniasQuery = _context.Colonia.Where(a => a.DeletedAt == null);
                if (!string.IsNullOrEmpty(request.Nombre))
                {
                    coloniasQuery = coloniasQuery.Where(e => e.Nombre.Contains(request.Nombre));
                }

                if (!string.IsNullOrEmpty(request.CodigoPostal))
                {
                    coloniasQuery = coloniasQuery.Where(e => e.CodigoPostal.Contains(request.CodigoPostal));
                }
            
                if (!string.IsNullOrEmpty(request.MunicipioId))
                {
                    coloniasQuery = coloniasQuery.Where(e => e.MunicipioId==request.MunicipioId);
                }

                excel.Workbook.Worksheets.Add("Agebs");
                var pestana = excel.Workbook.Worksheets.First();
                pestana.Cells[1, 1].Value = "Id";
                pestana.Cells[1, 2].Value = "Nombre";
                pestana.Cells[1, 3].Value = "Codigo postal";
                pestana.Cells[1, 4].Value = "MunicipioId";
                var coloniasDB = coloniasQuery.ToList();
                var row = 2;

                foreach (var colonia in coloniasDB)
                {
                    pestana.Cells[row, 1].Value = colonia.Id;
                    pestana.Cells[row, 2].Value = colonia.Nombre;
                    pestana.Cells[row, 3].Value = colonia.CodigoPostal;
                    pestana.Cells[row++, 4].Value = colonia.MunicipioId;
                }

                _context.Bitacora.Add(new Bitacora()
                {
                    UsuarioId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                    Accion = AccionBitacora.EXPORTACION,
                    Mensaje = "Se exportó el catálogo de colonias.",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                });
                _context.SaveChanges();

                return File(
                    fileContents: excel.GetAsByteArray(),
                    contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    fileDownloadName: "colonias.xlsx"
                );
            }
        }
    }
}