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
    /// Clase con la logica de negocio para la visualizacion, adicion, edicion e importacion del catalogo de ocupaciones
    /// </summary>
    public class OcupacionController : Controller
    {
        public readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="context">Conexion a la base de datos</param>
        /// <param name="userManager">Variable para identificar que usuario esta usando el sistema</param>
        public OcupacionController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        /// <summary>
        /// Funcion para mostrar la vista con el listado de ocupaciones registradas en la base de datos
        /// </summary>
        /// <returns>Vista con el listado de ocupaciones</returns>
        [HttpGet]
        [Authorize]
        [RequireClaim("Permiso", Value = "catalogos.ver")]
        public IActionResult Index()
        {
            var request = new OcupacionRequest();
            var importacion = _context.Importacion.Find("Ocupaciones");
            if (importacion != null)
            {
                request.Usuario = _context.Users.Find(importacion.UsuarioId).Name;
                request.ImportedAt = importacion.ImportedAt.ToString();
            }

            return View("Index", request);
        }

        /// <summary>
        /// Funcion que consulta en la base de datos las ocupaciones registradas de acuerdo a los filtros de busqueda
        /// seleccionados por el usuario
        /// </summary>
        /// <param name="request">Filtros de busqueda</param>
        /// <returns>Cadena JSON con el listado de ocupaciones</returns>
        [HttpPost]
        [Authorize]
        public string GetOcupaciones([FromBody] OcupacionRequest request)
        {
            var response = new OcupacionResponse();
            var ocupacionesQuery = _context.Ocupacion.Where(o => o.DeletedAt == null);
            if (!string.IsNullOrEmpty(request.Nombre))
            {
                ocupacionesQuery = ocupacionesQuery.Where(e => e.Nombre.Contains(request.Nombre));
            }

            response.Total = ocupacionesQuery.Count();
            response.Ocupaciones = ocupacionesQuery.OrderBy(o=>o.Id).Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();
            return JsonSedeshu.SerializeObject(response);
        }

        /// <summary>
        /// Funcion para importar desde un archivo de EXCEL el catalogo de ocupaciones, la importacion sobreescribira los registros anteriores
        /// </summary>
        /// <param name="model">Archivo de EXCEL</param>
        /// <returns>Estatus de importacion: ok, error</returns>
        [HttpPost]
        [Authorize]
        public string Importar(OcupacionArchivo model)
        {
            if (!ModelState.IsValid)
            {
                var errores = ModelState.Select(x => new {Key = x.Key, Error = x.Value.Errors.First().ErrorMessage})
                    .ToList();

                return JsonConvert.SerializeObject(errores);
            }

            try
            {
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
                                var importacion = _context.Importacion
                                    .FirstOrDefault(i => i.Clave.Equals("Ocupaciones"));
                                var pestana = archivoExcel.Worksheets.First();
                                var ocupacionesDB = _context.Ocupacion.ToDictionary(e => e.Id);
                                foreach (var ocupacion in ocupacionesDB)
                                {
                                    ocupacion.Value.UpdatedAt = now;
                                    ocupacion.Value.DeletedAt = now;
                                    _context.Ocupacion.Update(ocupacion.Value);
                                }
                                for (var i = 2; i <= pestana.Dimension.End.Row; i++)
                                {
                                    if (pestana.Cells[i,1].Value==null)
                                    {
                                        continue;
                                    }
                                    var id = pestana.Cells[i, 1].Value.ToString();
                                    Ocupacion ocupacion;
                                    if (ocupacionesDB.TryGetValue(id, out ocupacion))
                                    {
                                        ocupacion.Nombre = pestana.Cells[i, 2].Value.ToString();
                                        ocupacion.UpdatedAt = now;
                                        ocupacion.DeletedAt = null;
                                        _context.Ocupacion.Update(ocupacion);
                                    }
                                    else
                                    {
                                        ocupacion = new Ocupacion();
                                        ocupacion.Id = pestana.Cells[i, 1].Value.ToString();
                                        ocupacion.Nombre = pestana.Cells[i, 2].Value.ToString();
                                        ocupacion.CreatedAt = now;
                                        ocupacion.UpdatedAt = now;
                                        _context.Ocupacion.Add(ocupacion);
                                    }
                                }

                                if (importacion == null)
                                {
                                    importacion = new Importacion();
                                    importacion.Clave = "Ocupaciones";
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
                                    Mensaje = "Se importó el catálogo de ocupaciones.",
                                    CreatedAt = DateTime.Now,
                                    UpdatedAt = DateTime.Now
                                });

                                _context.SaveChanges();
                                transaction.Commit();
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("File",
                    "Ocurrió un error importando las ocupaciones. Revise el archivo de excel.");
                var errors = ModelState.Where(x => x.Value.Errors.Any())
                    .Select(x => new {Key = x.Key, Error = x.Value.Errors.First().ErrorMessage}).ToList();

                return JsonConvert.SerializeObject(errors);
            }

            TempData["Alert"] = "Show";
            TempData.Keep("Alert");

            return "ok";
        }

        /// <summary>
        /// Funcion que muestra la vista para la adicion o edicion de los datos de una ocupacion
        /// </summary>
        /// <param name="id">Identificador en base de datos de la ocupacion a modificar, null si el registro es nuevo</param>
        /// <returns>Vista con el formulario para registrar los datos de la ocupacion</returns>
        [HttpGet]
        [Authorize]
        public IActionResult CreateEdit(string id = "")
        {
            var ocupacion = new OcupacionCreateEditModel();
            var ocupacionDB = _context.Ocupacion.Find(id);
            if (string.IsNullOrEmpty(id))
            {
                ViewData["Title"] = "Crear ocupación";
                ocupacion.Id = "";
                ocupacion.IdAnterior = "";
                ocupacion.Nombre = "";
            }
            else
            {
                ViewData["Title"] = "Editar ocupación";
                ocupacion.Id = ocupacionDB.Id;
                ocupacion.IdAnterior = ocupacionDB.Id;
                ocupacion.Nombre = ocupacionDB.Nombre;
            }

            return View("Create", ocupacion);
        }
        
        /// <summary>
        /// Funcion para guardar la informacion de la ocupacion en la base de datos 
        /// </summary>
        /// <param name="model">Datos de la ocupacion</param>
        /// <returns>Estatus del guardado: ok, error</returns>
        [HttpPost]
        [Authorize]
        [RequireClaim("Permiso", Value = "catalogos.editar")]
        public string Guardar([FromBody] OcupacionCreateEditModel model)
        {
            if (_context.Ocupacion.Any(x => x.DeletedAt == null && x.Id == model.Id && x.Id != model.IdAnterior))
            {
                ModelState.AddModelError("Id", "Este Id ya fue creado.");
            }
            if (_context.Ocupacion.Any(x => x.Id != model.Id && model.Nombre != model.Nombre))
            {
                ModelState.AddModelError("Nombre", "Esta ocupación ya fue creada.");
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
                var ocupacionDB = _context.Ocupacion.Find(model.Id);
                if (ocupacionDB == null)
                {
                    ocupacionDB = new Ocupacion();
                    ocupacionDB.Id = model.Id;
                    ocupacionDB.Nombre = model.Nombre;
                    ocupacionDB.CreatedAt = now;
                    ocupacionDB.UpdatedAt = now;
                    _context.Ocupacion.Add(ocupacionDB);

                    _context.Bitacora.Add(new Bitacora()
                    {
                        UsuarioId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                        Accion = AccionBitacora.INSERCION,
                        Mensaje = "Se insertó la ocupación " + model.Nombre + ".",
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    });
                }
                else
                {
                    ocupacionDB.Id = model.Id;
                    ocupacionDB.Nombre = model.Nombre;
                    ocupacionDB.UpdatedAt = now;
                    ocupacionDB.DeletedAt = null;
                    _context.Ocupacion.Update(ocupacionDB);

                    _context.Bitacora.Add(new Bitacora()
                    {
                        UsuarioId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                        Accion = AccionBitacora.EDICION,
                        Mensaje = "Se modificó la ocupación " + model.Nombre + ".",
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
        /// Funcion para exportar a EXCEL el listado de ocupaciones de acuerdo a los filtros de busqueda seleccionados
        /// por el usuario
        /// </summary>
        /// <param name="id">Filtros de busqueda</param>
        /// <returns>Archivo de EXCEL con el listado de ocupaciones</returns>
        [HttpPost]
        [Authorize]
        public IActionResult Exportar(OcupacionRequest request)
        {
            using (var excel = new ExcelPackage())
            {                 
                var ocupacionesDB = _context.Ocupacion.Where(e => e.DeletedAt == null);
                if (!string.IsNullOrEmpty(request.Nombre))
                {
                    ocupacionesDB = ocupacionesDB.Where(o => o.Nombre.Contains(request.Nombre));
                }
                excel.Workbook.Worksheets.Add("Ocupaciones");
                var pestana = excel.Workbook.Worksheets.First();
                pestana.Cells[1, 1].Value = "Id";
                pestana.Cells[1, 2].Value = "Nombre";
                var row = 2;
                foreach (var ocupacion in ocupacionesDB)
                {
                    pestana.Cells[row, 1].Value = ocupacion.Id;
                    pestana.Cells[row++, 2].Value = ocupacion.Nombre;
                }

                _context.Bitacora.Add(new Bitacora()
                {
                    UsuarioId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                    Accion = AccionBitacora.EXPORTACION,
                    Mensaje = "Se exportó el catálogo de ocupaciones.",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                });
                _context.SaveChanges();

                return File(
                    fileContents: excel.GetAsByteArray(),
                    contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    fileDownloadName: "ocupaciones.xlsx"
                );
            }
        }
    }
}
