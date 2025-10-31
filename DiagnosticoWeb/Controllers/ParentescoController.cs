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
    /// Clase con la logica de negocio para la visualizacion, adicion, edicion e importacion del catalogo de parentescos
    /// </summary>
    public class ParentescoController : Controller
    {
        public readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        /// <summary>
        /// Costructor de la clase
        /// </summary>
        /// <param name="context">Conexion a la base de datos</param>
        /// <param name="userManager">Variable para identi</param>
        public ParentescoController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        /// <summary>
        /// Funcion que muestra la vista para la visualizacion del listado de parentescos registradas en la base de datos
        /// </summary>
        /// <returns>Vista con el listado de parentescos</returns>
        [HttpGet]
        [Authorize]
        [RequireClaim("Permiso", Value = "catalogos.ver")]
        public IActionResult Index()
        {
            var request = new ParentescoRequest();
            var importacion = _context.Importacion.Find("Parentescos");
            if (importacion != null)
            {
                request.Usuario = _context.Users.Find(importacion.UsuarioId).Name;
                request.ImportedAt = importacion.ImportedAt.ToString();
            }

            return View("Index", request);
        }

        /// <summary>
        /// Funcion que busca en la base de datos los parentescos registrados previamente de acuerdo a los filtros de
        /// busqueda seleccionados por el usuario
        /// </summary>
        /// <param name="request">Filtros de busqueda</param>
        /// <returns>Cadena JSON con el listado de parentescos</returns>
        [HttpPost]
        [Authorize]
        public string GetParentescos([FromBody] ParentescoRequest request)
        {
            var response = new ParentescoResponse();
            var parentescoQuery = _context.Parentesco.Where(p=>p.DeletedAt==null);
            if (!string.IsNullOrEmpty(request.Nombre))
            {
                parentescoQuery = parentescoQuery.Where(e => e.Nombre.Contains(request.Nombre));
            }
            response.Total = parentescoQuery.Count();
            response.Parentescos = parentescoQuery.OrderBy(p=>p.Id).Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize)
                .ToList();
            return JsonSedeshu.SerializeObject(response);
        }

        /// <summary>
        /// Funcion para importar a partir de un archivo de EXCEL el catalogo de parentescos, esta importacion
        /// sobreescribira los registros anteriores
        /// </summary>
        /// <param name="model">Archivo de EXCEL</param>
        /// <returns>Estatus de importacion: ok, error</returns>
        [HttpPost]
        [Authorize]
        public string Importar(ParentescoArchivo model)
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
                                var importacion = _context.Importacion.Find("Parentescos");
                                var pestana = archivoExcel.Worksheets.First();
                                var parentescosDB = _context.Parentesco.ToDictionary(e => e.Id);
                                foreach (var parentesco in parentescosDB)
                                {
                                    parentesco.Value.UpdatedAt = now;
                                    parentesco.Value.DeletedAt = now;
                                    _context.Parentesco.Update(parentesco.Value);
                                }
                                for (var i = 2; i <= pestana.Dimension.End.Row; i++)
                                {
                                    if (pestana.Cells[i,1].Value==null)
                                    {
                                        continue;
                                    }
                                    var id = pestana.Cells[i, 1].Value.ToString();
                                    Parentesco parentesco;
                                    if (parentescosDB.TryGetValue(id, out parentesco)) {
                                        parentesco.Nombre = pestana.Cells[i, 2].Value.ToString();
                                        parentesco.UpdatedAt = now;
                                        parentesco.DeletedAt = null;
                                        _context.Parentesco.Update(parentesco);
                                    } else {
                                        parentesco = new Parentesco();
                                        parentesco.Id = pestana.Cells[i, 1].Value.ToString();
                                        parentesco.Nombre = pestana.Cells[i, 2].Value.ToString();
                                        parentesco.CreatedAt = now;
                                        parentesco.UpdatedAt = now;
                                        _context.Parentesco.Add(parentesco);
                                    }
                                }
                                
                                if (importacion == null)
                                {
                                    importacion = new Importacion();
                                    importacion.Clave = "Parentescos";
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
                                    Mensaje = "Se importó el catálogo de parentescos.",
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
                ModelState.AddModelError("File", "Ocurrió un error importando los parentescos. Revise el archivo de excel.");
                var errors = ModelState.Where(x => x.Value.Errors.Any()).Select(x => new { Key = x.Key, Error = x.Value.Errors.First().ErrorMessage }).ToList();

                return JsonConvert.SerializeObject(errors);
            }

            TempData["Alert"] = "Show";
            TempData.Keep("Alert");

            return "ok";
        }

        /// <summary>
        /// Funcion que muestra la vista para la adicion o edicion en los datos de un parentesco
        /// </summary>
        /// <param name="id">Identificador en base de datos del parentesco a editar, null si el registro es nuevo</param>
        /// <returns>Vista con el formulario con los datos del parentesco</returns>
        [HttpGet]
        [Authorize]
        public IActionResult CreateEdit(string id = "")
        {
            var parentesco = new ParentescoCreateEditModel();
            var parentescoDB = _context.Parentesco.Find(id);
            if (string.IsNullOrEmpty(id))
            {
                ViewData["Title"] = "Crear parentesco";
                parentesco.Id = "";
                parentesco.IdAnterior = "";
                parentesco.Nombre = "";
            }
            else
            {
                ViewData["Title"] = "Editar ocupación";
                parentesco.Id = parentescoDB.Id;
                parentesco.IdAnterior = parentescoDB.Id;
                parentesco.Nombre = parentescoDB.Nombre;
            }

            return View("Create", parentesco);
        }


        /// <summary>
        /// Funcion para registrar en la base de datos los campos de la ocupacion
        /// </summary>
        /// <param name="model">Datos del parentesco</param>
        /// <returns>Estatus del guardado: ok, error</returns>
        [HttpPost]
        [Authorize]
        [RequireClaim("Permiso", Value = "catalogos.editar")]
        public string Guardar([FromBody] OcupacionCreateEditModel model)
        {
            if (_context.Parentesco.Any(x => x.DeletedAt == null && x.Id == model.Id && x.Id != model.IdAnterior))
            {
                ModelState.AddModelError("Id", "Este Id ya fue creado.");
            }
            if (_context.Parentesco.Any(x => x.Id != model.Id && model.Nombre != model.Nombre))
            {
                ModelState.AddModelError("Nombre", "Este parentesco ya fue creado.");
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
                var parentescoDB = _context.Parentesco.Find(model.Id);
                if (parentescoDB == null)
                {
                    parentescoDB = new Parentesco();
                    parentescoDB.Id = model.Id;
                    parentescoDB.Nombre = model.Nombre;
                    parentescoDB.CreatedAt = now;
                    parentescoDB.UpdatedAt = now;
                    _context.Parentesco.Add(parentescoDB);

                    _context.Bitacora.Add(new Bitacora()
                    {
                        UsuarioId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                        Accion = AccionBitacora.INSERCION,
                        Mensaje = "Se insertó el parentesco " + model.Nombre + ".",
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    });
                }
                else
                {
                    parentescoDB.Id = model.Id;
                    parentescoDB.Nombre = model.Nombre;
                    parentescoDB.UpdatedAt = now;
                    parentescoDB.DeletedAt = null;
                    _context.Parentesco.Update(parentescoDB);

                    _context.Bitacora.Add(new Bitacora()
                    {
                        UsuarioId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                        Accion = AccionBitacora.EDICION,
                        Mensaje = "Se modificó el parentesco " + model.Nombre + ".",
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
        /// Funcion para exportar a EXCEL el catalogo de parentescos registrado en la base de datos segun los filtros de
        /// busqueda seleccionados por el usuario
        /// </summary>
        /// <param name="id">Filtros de busqueda</param>
        /// <returns>Archivo de EXCEL con el catalodo de parentescos</returns>
        [HttpPost]
        [Authorize]
        public IActionResult Exportar(ParentescoRequest request)
        {
            using (var excel = new ExcelPackage())
            {                 
                var parentescosDB = _context.Parentesco.Where(e => e.DeletedAt == null);
                if (!string.IsNullOrEmpty(request.Nombre))
                {
                    parentescosDB= parentescosDB.Where(z => z.Nombre.Contains(request.Nombre));
                }
                excel.Workbook.Worksheets.Add("Parentescos");
                var pestana = excel.Workbook.Worksheets.First();
                pestana.Cells[1, 1].Value = "Id";
                pestana.Cells[1, 2].Value = "Nombre";
                var row = 2;
                foreach (var parentesco in parentescosDB)
                {
                    pestana.Cells[row, 1].Value = parentesco.Id;
                    pestana.Cells[row++, 2].Value = parentesco.Nombre;
                }

                _context.Bitacora.Add(new Bitacora()
                {
                    UsuarioId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                    Accion = AccionBitacora.EXPORTACION,
                    Mensaje = "Se exportó el catálogo de parentescos.",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                });
                _context.SaveChanges();

                return File(
                    fileContents: excel.GetAsByteArray(),
                    contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    fileDownloadName: "parentescos.xlsx"
                );
            }
        }

    }
}