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
    /// Clase que contiene la logica de negocio para visualizar, crear, editar o importar el catalogo de niveles de estudio
    /// </summary>
    public class EstudioController : Controller
    {
        public readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="context">Conexion a la base de datos</param>
        /// <param name="userManager">Variable para saber que usuario esta usando el sistema</param>
        public EstudioController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        /// <summary>
        /// Funcion que muestra la vista con el listado de niveles de estudio registrados en la base de datos
        /// </summary>
        /// <returns>Vista con el listado de niveles de estudio</returns>
        [HttpGet]
        [Authorize]
        [RequireClaim("Permiso", Value = "catalogos.ver")]
        public IActionResult Index()
        {
            var request = new EstudioRequest();
            var importacion = _context.Importacion.Find("Estudios");
            if (importacion != null)
            {
                request.Usuario = _context.Users.Find(importacion.UsuarioId).Name;
                request.ImportedAt = importacion.ImportedAt.ToString();
            }

            return View("Index", request);
        }

        /// <summary>
        /// Funcion que consulta los niveles de estudio registrados en la base de datos de acuerdo a los filtros de
        /// busqueda seleccionados por el usuario
        /// </summary>
        /// <param name="request">Filtros de busqueda</param>
        /// <returns>Cadena JSON con el listado de niveles de estudio</returns>
        [HttpPost]
        [Authorize]
        public string GetEstudios([FromBody] EstudioRequest request)
        {
            var response = new EstudioResponse();
            var estudiosQuery = _context.Estudio.Where(e => e.DeletedAt == null);
            if (!string.IsNullOrEmpty(request.Nombre))
            {
                estudiosQuery = estudiosQuery.Where(e => e.Nombre.Contains(request.Nombre));
            }

            response.Total = estudiosQuery.Count();
            response.Estudios = estudiosQuery.OrderBy(e => e.Id).Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();
            return JsonSedeshu.SerializeObject(response);
        }

        /// <summary>
        /// Funcion que permite importar el catalogo de niveles de estudio desde un archivo de EXCEL, esta importacion
        /// sobreescribira los registros anteriores 
        /// </summary>
        /// <param name="model">Archivo de EXCEL con el catalogo de niveles de estudio</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public string Importar(EstudioArchivo model)
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
                                var importacion = _context.Importacion.Find("Estudios");
                                var pestana = archivoExcel.Worksheets.First();
                                var estudiosDB = _context.Estudio.ToDictionary(e => e.Id);
                                foreach (var estudio in estudiosDB)
                                {
                                    estudio.Value.UpdatedAt = now;
                                    estudio.Value.DeletedAt = now;
                                    _context.Estudio.Update(estudio.Value);
                                }

                                for (var i = 2; i <= pestana.Dimension.End.Row; i++)
                                {
                                    if (pestana.Cells[i,1].Value==null)
                                    {
                                        continue;
                                    }
                                    var id = pestana.Cells[i, 1].Value.ToString();
                                    Estudio estudio;
                                    if (estudiosDB.TryGetValue(id, out estudio))
                                    {
                                        estudio.Nombre = pestana.Cells[i, 2].Value.ToString();
                                        estudio.UpdatedAt = DateTime.Now;
                                        estudio.DeletedAt = null;
                                        _context.Estudio.Update(estudio);
                                    }
                                    else
                                    {
                                        estudio = new Estudio();
                                        estudio.Id = pestana.Cells[i, 1].Value.ToString();
                                        estudio.Nombre = pestana.Cells[i, 2].Value.ToString();
                                        estudio.CreatedAt = DateTime.Now;
                                        estudio.UpdatedAt = DateTime.Now;
                                        _context.Estudio.Add(estudio);
                                    }
                                }

                                if (importacion == null)
                                {
                                    importacion = new Importacion();
                                    importacion.Clave = "Estudios";
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
                                    Mensaje = "Se importó el catálogo de niveles de estudio.",
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
                    "Ocurrió un error importando los niveles de estudio. Revise el archivo de excel.");
                var errors = ModelState.Where(x => x.Value.Errors.Any())
                    .Select(x => new {Key = x.Key, Error = x.Value.Errors.First().ErrorMessage}).ToList();

                return JsonConvert.SerializeObject(errors);
            }

            TempData["Alert"] = "Show";
            TempData.Keep("Alert");

            return "ok";
        }

        /// <summary>
        /// Funcion que muestra la vista para la adicion o edicion en los datos del nivel de estudio
        /// </summary>
        /// <param name="id">Identificador en base de datos del nivel de estudio, null si el registro es nuevo</param>
        /// <returns>Vista con el formulario para registrar o modificar el nivel educativo</returns>
        [HttpGet]
        [Authorize]
        public IActionResult CreateEdit(string id = "")
        {
            var estudio = new EstudioCreateEditModel();
            var estudioDB = _context.Estudio.Find(id);
            if (string.IsNullOrEmpty(id))
            {
                ViewData["Title"] = "Crear nivel de estudio";
                estudio.Id = "";
                estudio.IdAnterior = "";
                estudio.Nombre = "";
            }
            else
            {
                ViewData["Title"] = "Editar nivel de estudio";
                estudio.Id = estudioDB.Id;
                estudio.IdAnterior = estudioDB.Id;
                estudio.Nombre = estudioDB.Nombre;
            }

            return View("Create", estudio);
        }

        /// <summary>
        /// Funcion para guardar en la base de datos la informacion del nivel educativo
        /// </summary>
        /// <param name="model">Datos del nivel educativo</param>
        /// <returns>Estatus del guardado; ok, error</returns>
        [HttpPost]
        [Authorize]
        [RequireClaim("Permiso", Value = "catalogos.editar")]
        public string Guardar([FromBody] EstudioCreateEditModel model)
        {
            if (_context.Estudio.Any(x => x.DeletedAt == null && x.Id == model.Id && x.Id != model.IdAnterior))
            {
                ModelState.AddModelError("Id", "Este Id ya fue creado.");
            }

            if (_context.Estudio.Any(x => x.Id != model.Id && model.Nombre != model.Nombre))
            {
                ModelState.AddModelError("Nombre", "Este nivel de estudios ya fue creado.");
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
                var estudioDB = _context.Estudio.Find(model.Id);
                if (estudioDB == null)
                {
                    estudioDB = new Estudio();
                    estudioDB.Id = model.Id;
                    estudioDB.Nombre = model.Nombre;
                    estudioDB.CreatedAt = now;
                    estudioDB.UpdatedAt = now;
                    _context.Estudio.Add(estudioDB);

                    _context.Bitacora.Add(new Bitacora()
                    {
                        UsuarioId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                        Accion = AccionBitacora.INSERCION,
                        Mensaje = "Se insertó el nivel de estudio " + model.Nombre + ".",
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    });
                }
                else
                {
                    estudioDB.Id = model.Id;
                    estudioDB.Nombre = model.Nombre;
                    estudioDB.UpdatedAt = now;
                    estudioDB.DeletedAt = null;
                    _context.Estudio.Update(estudioDB);

                    _context.Bitacora.Add(new Bitacora()
                    {
                        UsuarioId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                        Accion = AccionBitacora.EDICION,
                        Mensaje = "Se modificó el nivel de estudio " + model.Nombre + ".",
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
        /// Funcion para exportar a un archivo de EXCEL el listado de niveles educativos de acuerdo a los filtros de
        /// busqueda seleccionados por el usuario
        /// </summary>
        /// <param name="id">Filtros de busqueda</param>
        /// <returns>Archivo de EXCEL con el listado de niveles educativos</returns>
        [HttpPost]
        [Authorize]
        public IActionResult Exportar(EstudioRequest request)
        {
            using (var excel = new ExcelPackage())
            {
                var estudiosDB = _context.Estudio.Where(e => e.DeletedAt == null);                 
                if (!string.IsNullOrEmpty(request.Nombre))
                {
                    estudiosDB = estudiosDB.Where(o => o.Nombre.Contains(request.Nombre));
                }

                excel.Workbook.Worksheets.Add("Niveles de estudio");
                var pestana = excel.Workbook.Worksheets.First();
                pestana.Cells[1, 1].Value = "Id";
                pestana.Cells[1, 2].Value = "Nombre";
                var row = 2;
                foreach (var estudio in estudiosDB)
                {
                    pestana.Cells[row, 1].Value = estudio.Id;
                    pestana.Cells[row++, 2].Value = estudio.Nombre;
                }

                _context.Bitacora.Add(new Bitacora()
                {
                    UsuarioId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                    Accion = AccionBitacora.EXPORTACION,
                    Mensaje = "Se exporto el catálogo de niveles de estudio.",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                });
                _context.SaveChanges();

                return File(
                    fileContents: excel.GetAsByteArray(),
                    contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    fileDownloadName: "niveles de estudio.xlsx"
                );
            }
        }
    }
}