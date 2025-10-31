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
    public class UnidadController:Controller
    {
        public readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        
        public UnidadController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        
        [HttpGet]
        [Authorize]
        [RequireClaim("Permiso", Value = "catalogos.ver")]
        public IActionResult Index()
        {
            var request = new EstudioRequest();
            var importacion = _context.Importacion.Find("Unidades");
            if (importacion != null)
            {
                request.Usuario = _context.Users.Find(importacion.UsuarioId).Name;
                request.ImportedAt = importacion.ImportedAt.ToString();
            }

            return View("Index", request);
        }
        
        [HttpPost]
        [Authorize]
        public string GetUnidades([FromBody] UnidadRequest request)
        {
            var response = new UnidadResponse();
            var unidadesQuery = _context.Unidad.Where(e => e.DeletedAt == null);
            if (!string.IsNullOrEmpty(request.Nombre))
            {
                unidadesQuery = unidadesQuery.Where(e => e.Nombre.Contains(request.Nombre));
            }

            response.Total = unidadesQuery.Count();
            response.Unidades = unidadesQuery.OrderBy(e => e.Id).Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();
            return JsonSedeshu.SerializeObject(response);
        }
        
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
                                var importacion = _context.Importacion.Find("Unidades");
                                var pestana = archivoExcel.Worksheets.First();
                                var unidadesDB = _context.Unidad.ToDictionary(e => e.Id);
                                foreach (var unidad in unidadesDB)
                                {
                                    unidad.Value.UpdatedAt = now;
                                    unidad.Value.DeletedAt = now;
                                    _context.Unidad.Update(unidad.Value);
                                }

                                for (var i = 2; i <= pestana.Dimension.End.Row; i++)
                                {
                                    string id = pestana.Cells[i, 1].Value.ToString();
                                    Unidad unidad;
                                    if (unidadesDB.TryGetValue(id, out unidad))
                                    {
                                        unidad.Nombre = pestana.Cells[i, 2].Value.ToString();
                                        unidad.UpdatedAt = DateTime.Now;
                                        unidad.DeletedAt = null;
                                        _context.Unidad.Update(unidad);
                                    }
                                    else
                                    {
                                        unidad = new Unidad();
                                        unidad.Id = pestana.Cells[i, 1].Value.ToString();
                                        unidad.Nombre = pestana.Cells[i, 2].Value.ToString();
                                        unidad.CreatedAt = DateTime.Now;
                                        unidad.UpdatedAt = DateTime.Now;
                                        _context.Unidad.Add(unidad);
                                    }
                                }

                                if (importacion == null)
                                {
                                    importacion = new Importacion();
                                    importacion.Clave = "Unidades";
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
                                    Mensaje = "Se importó el catálogo de unidades.",
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
                    "Ocurrió un error importando las unidades. Revise el archivo de excel.");
                var errors = ModelState.Where(x => x.Value.Errors.Any())
                    .Select(x => new {Key = x.Key, Error = x.Value.Errors.First().ErrorMessage}).ToList();

                return JsonConvert.SerializeObject(errors);
            }

            TempData["Alert"] = "Show";
            TempData.Keep("Alert");

            return "ok";
        }
        
        [HttpGet]
        [Authorize]
        [RequireClaim("Permiso", Value = "catalogos.editar")]
        public IActionResult CreateEdit(string id = "")
        {
            var unidad = new UnidadModel();
            var unidadDB = _context.Unidad.Find(id);
            if (string.IsNullOrEmpty(id))
            {
                ViewData["Title"] = "Crear unidad";
                unidad.Id = "";
                unidad.IdAnterior = "";
                unidad.Nombre = "";
            }
            else
            {
                ViewData["Title"] = "Editar unidad";
                unidad.Id = unidadDB.Id;
                unidad.IdAnterior = unidadDB.Id;
                unidad.Nombre = unidadDB.Nombre;
            }

            return View("Create", unidad);
        }
        
        [HttpPost]
        [Authorize]
        public string Guardar([FromBody] UnidadModel model)
        {
            if (_context.Unidad.Any(x => x.DeletedAt == null && x.Id == model.Id && x.Id != model.IdAnterior))
            {
                ModelState.AddModelError("Id", "Este Id ya fue creado.");
            }

            if (_context.Unidad.Any(x => x.Id != model.Id && model.Nombre != model.Nombre))
            {
                ModelState.AddModelError("Nombre", "Esta unidad ya fue creada.");
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
                var unidadDB = _context.Unidad.Find(model.Id);
                if (unidadDB == null)
                {
                    unidadDB = new Unidad();
                    unidadDB.Id = model.Id;
                    unidadDB.Nombre = model.Nombre;
                    unidadDB.CreatedAt = now;
                    unidadDB.UpdatedAt = now;
                    _context.Unidad.Add(unidadDB);

                    _context.Bitacora.Add(new Bitacora()
                    {
                        UsuarioId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                        Accion = AccionBitacora.INSERCION,
                        Mensaje = "Se insertó la unidad " + model.Nombre + ".",
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    });
                }
                else
                {
                    unidadDB.Id = model.Id;
                    unidadDB.Nombre = model.Nombre;
                    unidadDB.UpdatedAt = now;
                    unidadDB.DeletedAt = null;
                    _context.Unidad.Update(unidadDB);

                    _context.Bitacora.Add(new Bitacora()
                    {
                        UsuarioId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                        Accion = AccionBitacora.EDICION,
                        Mensaje = "Se modificó la unidad " + model.Nombre + ".",
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
        
        [HttpGet]
        [Authorize]
        public IActionResult Exportar(string id)
        {
            using (var excel = new ExcelPackage())
            {
                var unidadesDB = _context.Unidad.Where(e => e.DeletedAt == null);
                UnidadRequest request = JsonConvert.DeserializeObject<UnidadRequest>(id);
                if (!string.IsNullOrEmpty(request.Nombre))
                {
                    unidadesDB= unidadesDB.Where(o => o.Nombre.Contains(request.Nombre));
                }

                excel.Workbook.Worksheets.Add("Unidades");
                var pestana = excel.Workbook.Worksheets.First();
                pestana.Cells[1, 1].Value = "Id";
                pestana.Cells[1, 2].Value = "Nombre";
                var row = 2;
                foreach (var unidad in unidadesDB)
                {
                    pestana.Cells[row, 1].Value = unidad.Id;
                    pestana.Cells[row++, 2].Value = unidad.Nombre;
                }

                _context.Bitacora.Add(new Bitacora()
                {
                    UsuarioId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                    Accion = AccionBitacora.EXPORTACION,
                    Mensaje = "Se exportó el catálogo de unidades.",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                });
                _context.SaveChanges();

                return File(
                    fileContents: excel.GetAsByteArray(),
                    contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    fileDownloadName: "unidades.xlsx"
                );
            }
        }
    }
}