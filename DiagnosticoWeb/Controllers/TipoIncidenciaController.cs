using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OfficeOpenXml;
using DiagnosticoWeb.Code;
using DiagnosticoWeb.Database;
using DiagnosticoWeb.Models;
using DiagnosticoWeb.Validaciones;

namespace DiagnosticoWeb.Controllers
{
    public class TipoIncidenciaController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="context">Conexion a la base de datos</param>
        /// <param name="userManager">Variable para identificar que usuario esta en sesion</param>
        public TipoIncidenciaController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        
        /// <summary>
        /// Funcion que muestra la vista con el listado de tipos de incidencias
        /// </summary>
        /// <returns>Vista con el listado de tipos de incidencias</returns>
        [HttpGet]
        [Authorize]
        [RequireClaim("Permiso", Value = "catalogos.ver")]
        public IActionResult Index()
        {
            var request = new TipoIncidenciaRequest();
            var importacion = _context.Importacion.Find("TiposIncidencia");
            if (importacion != null)
            {
                request.Usuario = _context.Users.Find(importacion.UsuarioId).Name;
                request.ImportedAt = importacion.ImportedAt.ToString();
            }

            return View("Index", request);
        }
        
        /// <summary>
        /// Funcion que consulta en la base de datos el listado de tipos de incidencias registrados de acuerdo a los
        /// filtros de busqueda seleccionados por el usuario
        /// </summary>
        /// <param name="request">Filtros de busqeda</param>
        /// <returns>Cadena JSON con el listado de tipos de incidencias</returns>
        [HttpPost]
        [Authorize]
        public string GetTipoIncidencias([FromBody] TipoIncidenciaRequest request)
        {
            var response = new TipoIncidenciaResponse();
            var tipoIncidenciaQuery = _context.TipoIncidencia.Where(ta=>ta.DeletedAt==null);
            if (!string.IsNullOrEmpty(request.Nombre))
            {
                tipoIncidenciaQuery = tipoIncidenciaQuery.Where(e => e.Nombre.Contains(request.Nombre));
            }
            response.Total = tipoIncidenciaQuery .Count();
            response.Tipos = tipoIncidenciaQuery.OrderBy(ta=>ta.Id).Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize)
                .ToList();
            return JsonSedeshu.SerializeObject(response);
        }
        
        /// <summary>
        /// Funcion que importa el catalogo de tipos de incidencias desde un archivo de EXCEL, la importacion
        /// sobreescribira los registros previos
        /// </summary>
        /// <param name="model">Archivo de EXCEL</param>
        /// <returns>Estatus de la importacion: ok, error</returns>
        [HttpPost]
        [Authorize]
        public string Importar(TipoIncidenciaArchivo model)
        {
            if (!ModelState.IsValid) {
                var errores = ModelState.Select(x => new { Key = x.Key, Error = x.Value.Errors.First().ErrorMessage }).ToList();

                return JsonConvert.SerializeObject(errores);
            }

            try { 
                using (var package = new ExcelPackage(model.File.OpenReadStream()))
                {
                    var archivoExcel = package.Workbook;
                    if (archivoExcel != null)
                    {
                        if (archivoExcel.Worksheets.Count > 0)
                        {
                            using (var transaction = _context.Database.BeginTransaction())
                            {
                                var now = DateTime.Now;
                                var importacion = _context.Importacion.Find("TiposIncidencia");
                                var pestana = archivoExcel.Worksheets.First();
                                var tipoIncidenciasDB = _context.TipoIncidencia.ToDictionary(e => e.Id);
                                foreach (var tipoIncidencia in tipoIncidenciasDB)
                                {
                                    tipoIncidencia.Value.UpdatedAt = now;
                                    tipoIncidencia.Value.DeletedAt = now;
                                    _context.TipoIncidencia.Update(tipoIncidencia.Value);
                                }
                                for (var i = 2; i <= pestana.Dimension.End.Row; i++)
                                {
                                    var id = pestana.Cells[i, 1].Value.ToString();
                                    TipoIncidencia tipoIncidencia;
                                    if (tipoIncidenciasDB.TryGetValue(id, out tipoIncidencia)) {
                                        tipoIncidencia.Nombre = pestana.Cells[i, 2].Value.ToString();
                                        tipoIncidencia.UpdatedAt = now;
                                        tipoIncidencia.DeletedAt = null;
                                        _context.TipoIncidencia.Update(tipoIncidencia);
                                    } else {
                                        tipoIncidencia = new TipoIncidencia
                                        {
                                            Id = pestana.Cells[i, 1].Value.ToString(),
                                            Nombre = pestana.Cells[i, 2].Value.ToString(),
                                            CreatedAt = now,
                                            UpdatedAt = now
                                        };
                                        _context.TipoIncidencia.Add(tipoIncidencia);
                                    }
                                }
                                
                                if (importacion == null)
                                {
                                    importacion = new Importacion
                                    {
                                        Clave = "TiposIncidencia",
                                        ImportedAt = now,
                                        UsuarioId = _userManager.GetUserId(HttpContext.User)
                                    };
                                    _context.Importacion.Add(importacion);
                                }
                                else
                                {
                                    importacion.ImportedAt = now;
                                    importacion.UsuarioId = _userManager.GetUserId(HttpContext.User);
                                    _context.Importacion.Update(importacion);
                                }

                                _context.Bitacora.Add(new Bitacora()
                                {
                                    UsuarioId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                                    Accion = AccionBitacora.IMPORTACION,
                                    Mensaje = "Se importó el catálogo de tipos de incidencias.",
                                    CreatedAt = DateTime.Now,
                                    UpdatedAt = DateTime.Now
                                });

                                _context.SaveChanges();
                                transaction.Commit();
                            }
                        }
                    }
                }
            } catch (Exception) {
                ModelState.AddModelError("File", "Ocurrió un error importando los tipos de incidencias. Revise el archivo de excel.");
                var errors = ModelState.Where(x => x.Value.Errors.Any()).Select(x => new { Key = x.Key, Error = x.Value.Errors.First().ErrorMessage }).ToList();

                return JsonConvert.SerializeObject(errors);
            }

            TempData["Alert"] = "Show";
            TempData.Keep("Alert");

            return "ok";
        }
        
        /// <summary>
        /// Funcion que muestra la vista para la adicion, edicion de la informacion de un tipo de incidencia
        /// </summary>
        /// <param name="id">Identificar en base de datos del tipo de incidencia a modificar, null si el registro es nuevo</param>
        /// <returns>Vista con el formulario para los datos del tipo de incidencia</returns>
        [HttpGet]
        [Authorize]
        public IActionResult CreateEdit(string id = "")
        {
            var tipoIncidencia = new TipoIncidenciaCreateEditModel();
            var tipoIncidenciaDB = _context.TipoIncidencia.Find(id);
            if (string.IsNullOrEmpty(id))
            {
                ViewData["Title"] = "Crear tipo de incidencia";
                tipoIncidencia.Id = "";
                tipoIncidencia.IdAnterior = "";
                tipoIncidencia.Nombre = "";
            }
            else
            {
                ViewData["Title"] = "Editar tipo de incidencia";
                tipoIncidencia.Id = tipoIncidenciaDB.Id;
                tipoIncidencia.IdAnterior = tipoIncidenciaDB.Id;
                tipoIncidencia.Nombre = tipoIncidenciaDB.Nombre;
            }

            return View("Create", tipoIncidencia);
        }
        
        /// <summary>
        /// Funcion que guarda en la base de datos la informacion del tipo de incidencia
        /// </summary>
        /// <param name="model">Datos del tipo de incidencia</param>
        /// <returns>Estatus del guardo: ok, error</returns>
        [HttpPost]
        [Authorize]
        [RequireClaim("Permiso", Value = "catalogos.editar")]
        public string Guardar([FromBody] TipoIncidenciaCreateEditModel model)
        {
            if (_context.TipoIncidencia.Any(x => x.DeletedAt == null && x.Id == model.Id && x.Id != model.IdAnterior))
            {
                ModelState.AddModelError("Id", "Este Id ya fue creado.");
            }
            if (_context.TipoIncidencia.Any(x => x.Id != model.Id && model.Nombre != model.Nombre))
            {
                ModelState.AddModelError("Nombre", "Este tipo de incidencia ya fue creado.");
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
                var tipoIncidenciaDB = _context.TipoIncidencia.Find(model.Id);
                if (tipoIncidenciaDB == null)
                {
                    tipoIncidenciaDB = new TipoIncidencia
                    {
                        Id = model.Id, Nombre = model.Nombre, CreatedAt = now, UpdatedAt = now
                    };
                    _context.TipoIncidencia.Add(tipoIncidenciaDB);

                    _context.Bitacora.Add(new Bitacora()
                    {
                        UsuarioId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                        Accion = AccionBitacora.INSERCION,
                        Mensaje = "Se insertó el tipo de incidencia " + model.Nombre + ".",
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    });
                }
                else
                {
                    tipoIncidenciaDB.Id = model.Id;
                    tipoIncidenciaDB.Nombre = model.Nombre;
                    tipoIncidenciaDB.UpdatedAt = now;
                    tipoIncidenciaDB.DeletedAt = null;
                    _context.TipoIncidencia.Update(tipoIncidenciaDB);

                    _context.Bitacora.Add(new Bitacora()
                    {
                        UsuarioId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                        Accion = AccionBitacora.EDICION,
                        Mensaje = "Se modificó el tipo de incidencia " + model.Nombre + ".",
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

        /// <summary>
        /// Funcion que exporta a un archivo de EXCEL los tipos de incidencias registrados en la base de datos de
        /// acuerdo con los filtros de busqueda seleccionados por el usuario
        /// </summary>
        /// <param name="id">Filtros de busqueda</param>
        /// <returns>Archivo de EXCEL con el listado de tipos de incidencias</returns>
        [HttpPost]
        [Authorize]
        public IActionResult Exportar(TipoIncidenciaRequest request)
        {
            using (var excel = new ExcelPackage())
            {                
                var tipoIncidenciasDB = _context.TipoIncidencia.Where(e => e.DeletedAt == null);
                if (!string.IsNullOrEmpty(request.Nombre))
                {
                    tipoIncidenciasDB = tipoIncidenciasDB.Where(z => z.Nombre.Contains(request.Nombre));
                }
                excel.Workbook.Worksheets.Add("Tipos de incidencias");
                var pestana = excel.Workbook.Worksheets.First();
                pestana.Cells[1, 1].Value = "Id";
                pestana.Cells[1, 2].Value = "Nombre";
                var row = 2;
                foreach (var tipoIncidencia in tipoIncidenciasDB)
                {
                    pestana.Cells[row, 1].Value = tipoIncidencia.Id;
                    pestana.Cells[row++, 2].Value = tipoIncidencia.Nombre;
                }

                _context.Bitacora.Add(new Bitacora()
                {
                    UsuarioId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                    Accion = AccionBitacora.EXPORTACION,
                    Mensaje = "Se exportó el catálogo de tipos de incidencias.",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                });
                _context.SaveChanges();

                return File(
                    fileContents: excel.GetAsByteArray(),
                    contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    fileDownloadName: "tipos de incidencias.xlsx"
                );
            }
        }
    }
}