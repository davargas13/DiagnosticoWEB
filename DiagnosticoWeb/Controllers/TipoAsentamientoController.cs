using System;
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

namespace DiagnosticoWeb.Controllers
{
    /// <summary>
    /// Clase con la logica de negocio para la visualizacion, adicion, edicion, importacion y exportacion del catalogo de tipos de asentamientos
    /// </summary>
    public class TipoAsentamientoController : Controller
    {
        public readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="context">Conexion a la base de datos</param>
        /// <param name="userManager">Variable para identificar que usuario esta en sesion</param>
        public TipoAsentamientoController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        /// <summary>
        /// Funcion que muestra la vista con el listado de tipos de asentamientos
        /// </summary>
        /// <returns>Vista con el listado de tipos de asentamientos</returns>
        [HttpGet]
        [Authorize]
        [RequireClaim("Permiso", Value = "catalogos.ver")]
        public IActionResult Index()
        {
            var request = new TipoAsentamientoRequest();
            var importacion = _context.Importacion.Find("TiposAsentamiento");
            if (importacion != null)
            {
                request.Usuario = _context.Users.Find(importacion.UsuarioId).Name;
                request.ImportedAt = importacion.ImportedAt.ToString();
            }

            return View("Index", request);
        }

        /// <summary>
        /// Funcion que consulta en la base de datos el listado de tipos de asentamientos registrados de acuerdo a los
        /// filtros de busqueda seleccionados por el usuario
        /// </summary>
        /// <param name="request">Filtros de busqeda</param>
        /// <returns>Cadena JSON con el listado de tipos de asentamientos</returns>
        [HttpPost]
        [Authorize]
        public string GetTipoAsentamientos([FromBody] TipoAsentamientoRequest request)
        {
            var response = new TipoAsentamientoResponse();
            var tipoAsentamientoQuery = _context.TipoAsentamiento.Where(ta=>ta.DeletedAt==null);
            if (!string.IsNullOrEmpty(request.Nombre))
            {
                tipoAsentamientoQuery = tipoAsentamientoQuery.Where(e => e.Nombre.Contains(request.Nombre));
            }
            response.Total = tipoAsentamientoQuery .Count();
            response.TipoAsentamientos = tipoAsentamientoQuery.OrderBy(ta=>ta.Id).Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize)
                .ToList();
            return JsonSedeshu.SerializeObject(response);
        }

        /// <summary>
        /// Funcion que importa el catalogo de tipos de asentamientos desde un archivo de EXCEL, la importacion
        /// sobreescribira los registros previos
        /// </summary>
        /// <param name="model">Archivo de EXCEL</param>
        /// <returns>Estatus de la importacion: ok, error</returns>
        [HttpPost]
        [Authorize]
        public string Importar(TipoAsentamientoArchivo model)
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
                                var importacion = _context.Importacion.Find("TiposAsentamiento");
                                var pestana = archivoExcel.Worksheets.First();
                                var tipoAsentamientosDB = _context.TipoAsentamiento.ToDictionary(e => e.Id);
                                foreach (var tipoAsentamiento in tipoAsentamientosDB)
                                {
                                    tipoAsentamiento.Value.UpdatedAt = now;
                                    tipoAsentamiento.Value.DeletedAt = now;
                                    _context.TipoAsentamiento.Update(tipoAsentamiento.Value);
                                }
                                for (var i = 2; i <= pestana.Dimension.End.Row; i++)
                                {
                                    if (pestana.Cells[i,1].Value==null)
                                    {
                                        continue;
                                    }
                                    var id = pestana.Cells[i, 1].Value.ToString();
                                    TipoAsentamiento tipoAsentamiento;
                                    if (tipoAsentamientosDB.TryGetValue(id, out tipoAsentamiento)) {
                                        tipoAsentamiento.Nombre = pestana.Cells[i, 2].Value.ToString();
                                        tipoAsentamiento.UpdatedAt = now;
                                        tipoAsentamiento.DeletedAt = null;
                                        _context.TipoAsentamiento.Update(tipoAsentamiento);
                                    } else {
                                        tipoAsentamiento = new TipoAsentamiento();
                                        tipoAsentamiento.Id = pestana.Cells[i, 1].Value.ToString();
                                        tipoAsentamiento.Nombre = pestana.Cells[i, 2].Value.ToString();
                                        tipoAsentamiento.CreatedAt = now;
                                        tipoAsentamiento.UpdatedAt = now;
                                        _context.TipoAsentamiento.Add(tipoAsentamiento);
                                    }
                                }
                                
                                if (importacion == null)
                                {
                                    importacion = new Importacion();
                                    importacion.Clave = "TiposAsentamiento";
                                    importacion.ImportedAt = now;
                                    importacion.UsuarioId = _userManager.GetUserId(HttpContext.User);
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
                                    Mensaje = "Se importó el catálogo de tipos de asentamientos.",
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
                ModelState.AddModelError("File", "Ocurrió un error importando los tipos de asentamientos. Revise el archivo de excel.");
                var errors = ModelState.Where(x => x.Value.Errors.Any()).Select(x => new { Key = x.Key, Error = x.Value.Errors.First().ErrorMessage }).ToList();

                return JsonConvert.SerializeObject(errors);
            }

            TempData["Alert"] = "Show";
            TempData.Keep("Alert");

            return "ok";
        }

        /// <summary>
        /// Funcion que muestra la vista para la adicion, edicion de la informacion de un tipo de asentamiento
        /// </summary>
        /// <param name="id">Identificar en base de datos del tipo de asentamiento a modificar, null si el registro es nuevo</param>
        /// <returns>Vista con el formulario para los datos del tipo de asentamiento</returns>
        [HttpGet]
        [Authorize]
        public IActionResult CreateEdit(string id = "")
        {
            var tipoAsentamiento = new TipoAsentamientoCreateEditModel();
            var tipoAsentamientoDB = _context.TipoAsentamiento.Find(id);
            if (string.IsNullOrEmpty(id))
            {
                ViewData["Title"] = "Crear tipo de asentamiento";
                tipoAsentamiento.Id = "";
                tipoAsentamiento.IdAnterior = "";
                tipoAsentamiento.Nombre = "";
            }
            else
            {
                ViewData["Title"] = "Editar tipo de asentamiento";
                tipoAsentamiento.Id = tipoAsentamientoDB.Id;
                tipoAsentamiento.IdAnterior = tipoAsentamientoDB.Id;
                tipoAsentamiento.Nombre = tipoAsentamientoDB.Nombre;
            }

            return View("Create", tipoAsentamiento);
        }

        /// <summary>
        /// Funcion que guarda en la base de datos la informacion del tipo de asentamiento
        /// </summary>
        /// <param name="model">Datos del tipo de asentamiento</param>
        /// <returns>Estatus del guardo: ok, error</returns>
        [HttpPost]
        [Authorize]
        [RequireClaim("Permiso", Value = "catalogos.editar")]
        public string Guardar([FromBody] TipoAsentamientoCreateEditModel model)
        {
            if (_context.TipoAsentamiento.Any(x => x.DeletedAt == null && x.Id == model.Id && x.Id != model.IdAnterior))
            {
                ModelState.AddModelError("Id", "Este Id ya fue creado.");
            }
            if (_context.TipoAsentamiento.Any(x => x.Id != model.Id && model.Nombre != model.Nombre))
            {
                ModelState.AddModelError("Nombre", "Este tipo de asentamiento ya fue creado.");
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
                var tipoAsentamientoDB = _context.TipoAsentamiento.Find(model.Id);
                if (tipoAsentamientoDB == null)
                {
                    tipoAsentamientoDB = new TipoAsentamiento();
                    tipoAsentamientoDB.Id = model.Id;
                    tipoAsentamientoDB.Nombre = model.Nombre;
                    tipoAsentamientoDB.CreatedAt = now;
                    tipoAsentamientoDB.UpdatedAt = now;
                    _context.TipoAsentamiento.Add(tipoAsentamientoDB);

                    _context.Bitacora.Add(new Bitacora()
                    {
                        UsuarioId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                        Accion = AccionBitacora.INSERCION,
                        Mensaje = "Se insertó el tipo de asentamiento " + model.Nombre + ".",
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    });
                }
                else
                {
                    tipoAsentamientoDB.Id = model.Id;
                    tipoAsentamientoDB.Nombre = model.Nombre;
                    tipoAsentamientoDB.UpdatedAt = now;
                    tipoAsentamientoDB.DeletedAt = null;
                    _context.TipoAsentamiento.Update(tipoAsentamientoDB);

                    _context.Bitacora.Add(new Bitacora()
                    {
                        UsuarioId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                        Accion = AccionBitacora.EDICION,
                        Mensaje = "Se modificó el tipo de asentamiento " + model.Nombre + ".",
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
        /// Funcion que exporta a un archivo de EXCEL los tipos de asentamientos registrados en la base de datos de
        /// acuerdo con los filtros de busqueda seleccionados por el usuario
        /// </summary>
        /// <param name="id">Filtros de busqueda</param>
        /// <returns>Archivo de EXCEL con el listado de tipos de asentamientos</returns>
        [HttpPost]
        [Authorize]
        public IActionResult Exportar(TipoAsentamientoRequest request)
        {
            using (var excel = new ExcelPackage())
            {                 
                var tipoAsentamientosDb = _context.TipoAsentamiento.Where(e => e.DeletedAt == null);
                if (!string.IsNullOrEmpty(request.Nombre))
                {
                    tipoAsentamientosDb= tipoAsentamientosDb.Where(z => z.Nombre.Contains(request.Nombre));
                }
                excel.Workbook.Worksheets.Add("Tipos de asentamientos");
                var pestana = excel.Workbook.Worksheets.First();
                pestana.Cells[1, 1].Value = "Id";
                pestana.Cells[1, 2].Value = "Nombre";
                var row = 2;
                foreach (var tipoAsentamiento in tipoAsentamientosDb)
                {
                    pestana.Cells[row, 1].Value = tipoAsentamiento.Id;
                    pestana.Cells[row++, 2].Value = tipoAsentamiento.Nombre;
                }

                _context.Bitacora.Add(new Bitacora()
                {
                    UsuarioId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                    Accion = AccionBitacora.EXPORTACION,
                    Mensaje = "Se exportó el catálogo de tipos de asentamientos.",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                });
                _context.SaveChanges();

                return File(
                    fileContents: excel.GetAsByteArray(),
                    contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    fileDownloadName: "tipos de asentamientos.xlsx"
                );
            }
        }
    }
}