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
    /// Clase que tiene la logica de negocio para listar, crear, editar o importar el catalogo de estados civiles
    /// </summary>
    public class EstadoCivilController : Controller
    {
        public readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="context">Conexion a la base de datos</param>
        /// <param name="userManager">Variable para identificar que usuario esta usando el sistema</param>
        public EstadoCivilController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        /// <summary>
        /// Funcion que muestra la vista con el listado de estdos civiles registrados en la base de datos
        /// </summary>
        /// <returns>Vista con el listado de estados civiles</returns>
        [HttpGet]
        [Authorize]
        [RequireClaim("Permiso", Value = "catalogos.ver")]
        public IActionResult Index()
        {
            var request = new EstadoCivilRequest();
            var importacion = _context.Importacion.Find("EstadosCiviles");
            if (importacion != null)
            {
                request.Usuario = _context.Users.Find(importacion.UsuarioId).Name;
                request.ImportedAt = importacion.ImportedAt.ToString();
            }

            return View("Index", request);
        }

        /// <summary>
        /// Funcion que busca en la base de datos los estados civiles registrados y los filtra de acuerdo a los campos seleccinonados por el usuario
        /// </summary>
        /// <param name="request">Filtros de busqueda</param>
        /// <returns>Cadena JSON con el listado de estaod civiles</returns>
        [HttpPost]
        [Authorize]
        public string GetEstadosCiviles([FromBody] EstadoCivilRequest request)
        {
            var response = new EstadoCivilResponse();
            var estadocivilQuery = _context.EstadoCivil.Where(ec => ec.DeletedAt == null);
            if (!string.IsNullOrEmpty(request.Nombre))
            {
                estadocivilQuery = estadocivilQuery.Where(e => e.Nombre.Contains(request.Nombre));
            }

            response.Total = estadocivilQuery.Count();
            response.EstadosCiviles = estadocivilQuery.OrderBy(ec=>ec.Id).Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize).ToList();
            return JsonSedeshu.SerializeObject(response);
        }

        /// <summary>
        /// Funcion que importa desde un archivo de EXCEL el catalogo de estados civiles, la importacion de estos
        /// registros sobreescribe los datos almacenados previamente en la base de datos
        /// </summary>
        /// <param name="model">Archivo de excel</param>
        /// <returns>Estatus de importacion: ok, error</returns>
        [HttpPost]
        [Authorize]
        public string Importar(EstadoCivilArchivo model)
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
                                    .FirstOrDefault(i => i.Clave.Equals("EstadosCiviles"));
                                var pestana = archivoExcel.Worksheets.First();
                                var estadosCivilesDB = _context.EstadoCivil.ToDictionary(e => e.Id);
                                foreach (var estadoCivil in estadosCivilesDB)
                                {
                                    estadoCivil.Value.UpdatedAt = now;
                                    estadoCivil.Value.DeletedAt = now;
                                    _context.EstadoCivil.Update(estadoCivil.Value);
                                }
                                for (var i = 2; i <= pestana.Dimension.End.Row; i++)
                                {
                                    if (pestana.Cells[i,1].Value==null)
                                    {
                                        continue;
                                    }
                                    var id = pestana.Cells[i, 1].Value.ToString();
                                    EstadoCivil estadoCivil;
                                    if (estadosCivilesDB.TryGetValue(id, out estadoCivil))
                                    {
                                        estadoCivil.Nombre = pestana.Cells[i, 2].Value.ToString();
                                        estadoCivil.UpdatedAt = now;
                                        estadoCivil.DeletedAt = null;
                                        _context.EstadoCivil.Update(estadoCivil);
                                    }
                                    else
                                    {
                                        estadoCivil = new EstadoCivil();
                                        estadoCivil.Id = pestana.Cells[i, 1].Value.ToString();
                                        estadoCivil.Nombre = pestana.Cells[i, 2].Value.ToString();
                                        estadoCivil.CreatedAt = now;
                                        estadoCivil.UpdatedAt = now;
                                        _context.EstadoCivil.Add(estadoCivil);
                                    }
                                }
                                
                                if (importacion == null)
                                {
                                    importacion = new Importacion();
                                    importacion.Clave = "EstadosCiviles";
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
                                    Mensaje = "Se importó el catálogo de estados civiles.",
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
                    "Ocurrió un error importando los estados civiles. Revise el archivo de excel.");
                var errors = ModelState.Where(x => x.Value.Errors.Any())
                    .Select(x => new {Key = x.Key, Error = x.Value.Errors.First().ErrorMessage}).ToList();

                return JsonConvert.SerializeObject(errors);
            }

            TempData["Alert"] = "Show";
            TempData.Keep("Alert");

            return "ok";
        }

        /// <summary>
        /// Funcion que muestra la vista para la creacion o edicion de un estado civil
        /// </summary>
        /// <param name="id">Identificador en base de datos del estado civil, null si es nuevo registro</param>
        /// <returns>Vista con el formulario para la captura o modificacon del estado civil</returns>
        [HttpGet]
        [Authorize]
        public IActionResult CreateEdit(string id = "")
        {
            var estadoCivil = new EstadoCivilCreateEditModel();
            var estadoCivilDB = _context.EstadoCivil.Find(id);
            if (string.IsNullOrEmpty(id))
            {
                ViewData["Title"] = "Crear estado civil";
                estadoCivil.Id = "";
                estadoCivil.IdAnterior = "";
                estadoCivil.Nombre = "";
            }
            else
            {
                ViewData["Title"] = "Editar estado civil";
                estadoCivil.Id = estadoCivilDB.Id;
                estadoCivil.IdAnterior = estadoCivilDB.Id;
                estadoCivil.Nombre = estadoCivilDB.Nombre;
            }

            return View("Create", estadoCivil);
        }

        /// <summary>
        /// Funcion que guarda en la base de datos los datos capturados por el usuario 
        /// </summary>
        /// <param name="model">Datos del estado civil</param>
        /// <returns>Estatus de guardado: ok, error</returns>
        [HttpPost]
        [Authorize]
        [RequireClaim("Permiso", Value = "catalogos.editar")]
        public string Guardar([FromBody] EstadoCivilCreateEditModel model)
        {
            if (_context.EstadoCivil.Any(x => x.DeletedAt == null && x.Id == model.Id && x.Id != model.IdAnterior))
            {
                ModelState.AddModelError("Id", "Este Id ya fue creado.");
            }
            if (_context.EstadoCivil.Any(x => x.Id != model.Id && model.Nombre != model.Nombre))
            {
                ModelState.AddModelError("Nombre", "Este estado civil ya fue creado.");
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
                var estadoCivilDB = _context.EstadoCivil.Find(model.Id);
                if (estadoCivilDB == null)
                {
                    estadoCivilDB = new EstadoCivil();
                    estadoCivilDB.Id = model.Id;
                    estadoCivilDB.Nombre = model.Nombre;
                    estadoCivilDB.CreatedAt = now;
                    estadoCivilDB.UpdatedAt = now;
                    _context.EstadoCivil.Add(estadoCivilDB);

                    _context.Bitacora.Add(new Bitacora()
                    {
                        UsuarioId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                        Accion = AccionBitacora.INSERCION,
                        Mensaje = "Se insertó el estado civil " + model.Nombre + ".",
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    });
                }
                else
                {
                    estadoCivilDB.Id = model.Id;
                    estadoCivilDB.Nombre = model.Nombre;
                    estadoCivilDB.UpdatedAt = now;
                    estadoCivilDB.DeletedAt = null;
                    _context.EstadoCivil.Update(estadoCivilDB);

                    _context.Bitacora.Add(new Bitacora()
                    {
                        UsuarioId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                        Accion = AccionBitacora.EDICION,
                        Mensaje = "Se modificó el estado civil " + model.Nombre + ".",
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
        /// Funcion que exporta a EXCEL el catalogo de estados civiles de acuerdo a los filtros seleccionados por el usuario
        /// </summary>
        /// <param name="id">Filtros de busqueda</param>
        /// <returns>Archivo de EXCEL con el listado de estados civiles</returns>
        [HttpPost]
        [Authorize]
        public IActionResult Exportar(EstadoCivilRequest request)
        {
            using (var excel = new ExcelPackage())
            {
                var estadoCivilesDB = _context.EstadoCivil.Where(e => e.DeletedAt == null);                
                if (!string.IsNullOrEmpty(request.Nombre))
                {
                    estadoCivilesDB = estadoCivilesDB.Where(o => o.Nombre.Contains(request.Nombre));
                }
                excel.Workbook.Worksheets.Add("Estados civiles");
                var pestana = excel.Workbook.Worksheets.First();
                pestana.Cells[1, 1].Value = "Id";
                pestana.Cells[1, 2].Value = "Nombre";
                var row = 2;
                foreach (var estadoCivil in estadoCivilesDB)
                {
                    pestana.Cells[row, 1].Value = estadoCivil.Id;
                    pestana.Cells[row++, 2].Value = estadoCivil.Nombre;
                }

                _context.Bitacora.Add(new Bitacora()
                {
                    UsuarioId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                    Accion = AccionBitacora.EXPORTACION,
                    Mensaje = "Se exportó el catálogo de estados civiles.",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                });
                _context.SaveChanges();

                return File(
                    fileContents: excel.GetAsByteArray(),
                    contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    fileDownloadName: "estados civiles.xlsx"
                );
            }
        }
    }
}