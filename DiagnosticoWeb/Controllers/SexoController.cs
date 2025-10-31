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
    /// Clase con la logica de negocio para la visualcion, adicion, edicio, importacion y exportacion del catalogo de sexos
    /// </summary>
    public class SexoController : Controller
    {
        public readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="context">Conexion a la base de datos</param>
        /// <param name="userManager">Variable para saber que usuario esta usando el sistema</param>
        public SexoController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        /// <summary>
        /// Funcion que muestra la vista con el listado de sexos registrados en la base de datos
        /// </summary>
        /// <returns>Vista con el listado de sexos</returns>
        [HttpGet]
        [Authorize]
        [RequireClaim("Permiso", Value = "catalogos.ver")]
        public IActionResult Index()
        {
            var request = new SexoRequest();
            var importacion = _context.Importacion.Find("Sexos");
            if (importacion != null)
            {
                request.Usuario = _context.Users.Find(importacion.UsuarioId).Name;
                request.ImportedAt = importacion.ImportedAt.ToString();
            }

            return View("Index", request);
        }

        /// <summary>
        /// Funcion que consulta el catalogo de sexos registrados en la base de datos de acuerdo a los filtros de
        /// busqueda seleccionados por el usuario
        /// </summary>
        /// <param name="request">Filtros de busqueda</param>
        /// <returns>Cadena JSON con el listado de sexos</returns>
        [HttpPost]
        [Authorize]
        public string GetSexos([FromBody] SexoRequest request)
        {
            var response = new SexoResponse();
            var sexosQuery = _context.Sexo.Where(s => s.DeletedAt == null);
            if (!string.IsNullOrEmpty(request.Nombre))
            {
                sexosQuery = sexosQuery.Where(e => e.Nombre.Contains(request.Nombre));
            }

            response.Total = sexosQuery.Count();
            response.Sexos = sexosQuery.OrderBy(s=>s.Id).Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize)
                .ToList();
            return JsonSedeshu.SerializeObject(response);
        }

        /// <summary>
        /// Funcion para importar el catalogo de sexos desde un archivo de EXCEL, la importacion sobreescribira los
        /// registros anteriores
        /// </summary>
        /// <param name="model">Archivo de EXCEL</param>
        /// <returns>Estatus de la importacion: ok, error</returns>
        [HttpPost]
        [Authorize]
        public string Importar(SexoArchivo model)
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
                                var importacion = _context.Importacion.Find("Sexos");
                                var pestana = archivoExcel.Worksheets.First();
                                var sexosDB = _context.Sexo.ToDictionary(e => e.Id);
                                foreach (var sexo in sexosDB)
                                {
                                    sexo.Value.UpdatedAt = now;
                                    sexo.Value.DeletedAt = now;
                                    _context.Sexo.Update(sexo.Value);
                                }
                                for (var i = 2; i <= pestana.Dimension.End.Row; i++)
                                {
                                    if (pestana.Cells[i,1].Value==null)
                                    {
                                        continue;
                                    }
                                    var id = pestana.Cells[i, 1].Value.ToString();
                                    Sexo sexo;
                                    if (sexosDB.TryGetValue(id, out sexo))
                                    {
                                        sexo.Nombre = pestana.Cells[i, 2].Value.ToString();
                                        sexo.UpdatedAt = now;
                                        sexo.DeletedAt = null;
                                        _context.Sexo.Update(sexo);
                                    }
                                    else
                                    {
                                        sexo = new Sexo();
                                        sexo.Id = pestana.Cells[i, 1].Value.ToString();
                                        sexo.Nombre = pestana.Cells[i, 2].Value.ToString();
                                        sexo.CreatedAt = now;
                                        sexo.UpdatedAt = now;
                                        _context.Sexo.Add(sexo);
                                    }
                                }
                                if (importacion == null)
                                {
                                    importacion = new Importacion();
                                    importacion.Clave = "Sexos";
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
                                    Mensaje = "Se importó el catálogo de géneros.",
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
        /// Funcion que muestra la vista para el registro en la informacion de un sexo
        /// </summary>
        /// <param name="id">Identificador en base de datos del sexo a modificar, null si el registro es nuevo</param>
        /// <returns>Vista con el formulario para la captura del sexo</returns>
        [HttpGet]
        [Authorize]
        public IActionResult CreateEdit(string id = "")
        {
            var sexo = new SexoCreateEditModel();
            var sexoDB = _context.Sexo.Find(id);
            if (string.IsNullOrEmpty(id))
            {
                ViewData["Title"] = "Crear género";
                sexo.Id = "";
                sexo.IdAnterior = "";
                sexo.Nombre = "";
            }
            else
            {
                ViewData["Title"] = "Editar género";
                sexo.Id = sexoDB.Id;
                sexo.IdAnterior = sexoDB.Id;
                sexo.Nombre = sexoDB.Nombre;
            }

            return View("Create", sexo);
        }

        /// <summary>
        /// Funcion para guardar en la base de datos la informacion del sexo
        /// </summary>
        /// <param name="model">Datos del sexo</param>
        /// <returns>Estatus del guardado: ok, error</returns>
        [HttpPost]
        [Authorize]
        [RequireClaim("Permiso", Value = "catalogos.editar")]
        public string Guardar([FromBody] SexoCreateEditModel model)
        {
            if (_context.Sexo.Any(x => x.DeletedAt == null && x.Id == model.Id && x.Id != model.IdAnterior))
            {
                ModelState.AddModelError("Id", "Este Id ya fue creado.");
            }
            if (_context.Sexo.Any(x => x.Id != model.Id && model.Nombre != model.Nombre))
            {
                ModelState.AddModelError("Nombre", "Este género ya fue creado.");
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
                var sexoDB = _context.Sexo.Find(model.Id);
                if (sexoDB == null)
                {
                    sexoDB = new Sexo();
                    sexoDB.Id = model.Id;
                    sexoDB.Nombre = model.Nombre;
                    sexoDB.CreatedAt = now;
                    sexoDB.UpdatedAt = now;
                    _context.Sexo.Add(sexoDB);

                    _context.Bitacora.Add(new Bitacora()
                    {
                        UsuarioId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                        Accion = AccionBitacora.INSERCION,
                        Mensaje = "Se insertó el género " + model.Nombre + ".",
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    });
                }
                else
                {
                    sexoDB.Id = model.Id;
                    sexoDB.Nombre = model.Nombre;
                    sexoDB.UpdatedAt = now;
                    sexoDB.DeletedAt = null;
                    _context.Sexo.Update(sexoDB);

                    _context.Bitacora.Add(new Bitacora()
                    {
                        UsuarioId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                        Accion = AccionBitacora.EDICION,
                        Mensaje = "Se modificó el género " + model.Nombre + ".",
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
        /// Funcion para exportar a un archivo de EXCEL el catalogo de sexos de acuerdo a los filtros de busqueda
        /// seleccionados por el usuario
        /// </summary>
        /// <param name="id">Filtros de busqueda</param>
        /// <returns>Archivo de EXCEL con el listado de sexos</returns>
        [HttpPost]
        [Authorize]
        public IActionResult Exportar(SexoRequest request)
        {
            using (var excel = new ExcelPackage())
            {                 
                var sexosDB = _context.Sexo.Where(e => e.DeletedAt == null);
                if (!string.IsNullOrEmpty(request.Nombre))
                {
                    sexosDB= sexosDB.Where(z => z.Nombre.Contains(request.Nombre));
                }
                excel.Workbook.Worksheets.Add("Generos");
                var pestana = excel.Workbook.Worksheets.First();
                pestana.Cells[1, 1].Value = "Id";
                pestana.Cells[1, 2].Value = "Nombre";
                var row = 2;
                foreach (var sexo in sexosDB)
                {
                    pestana.Cells[row, 1].Value = sexo.Id;
                    pestana.Cells[row++, 2].Value = sexo.Nombre;
                }

                _context.Bitacora.Add(new Bitacora()
                {
                    UsuarioId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                    Accion = AccionBitacora.EXPORTACION,
                    Mensaje = "Se exportó el catálogo de géneros.",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                });
                _context.SaveChanges();

                return File(
                    fileContents: excel.GetAsByteArray(),
                    contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    fileDownloadName: "generos.xlsx"
                );
            }
        }
    }
}