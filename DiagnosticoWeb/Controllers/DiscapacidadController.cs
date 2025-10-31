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

namespace DiagnosticoWeb.Controllers
{
    /// <summary>
    /// Clase que contiene la logica de negocio para la consulta, creacion y edicion del catalogo de discapacidades
    /// </summary>
    public class DiscapacidadController : Controller
    {
        public readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="context">Conexion a la base de datos</param>
        /// <param name="userManager">Objeto que permite saber que usuario esta usando el sistema</param>
        public DiscapacidadController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        /// <summary>
        /// Funcion que muestra la vista del listado de discapacidades registradas en la base de datos
        /// </summary>
        /// <returns>Vista con el listado de discapacidades</returns>
        [HttpGet]
        [Authorize]
        [RequireClaim("Permiso", Value = "catalogos.ver")]
        public IActionResult Index()
        {
            var request = new DiscapacidadRequest();
            var importacion = _context.Importacion.Find("Discapacidades");
            if (importacion != null)
            {
                request.Usuario = _context.Users.Find(importacion.UsuarioId).Name;
                request.ImportedAt = importacion.ImportedAt.ToString();
            }

            return View("Index", request);
        }

        /// <summary>
        /// Funcion que consulta en la base de datos las discapacidades registradas de acuerdo a filtros seleccionados por el usuario
        /// </summary>
        /// <param name="request">Filtros seleccionados por el usuario</param>
        /// <returns>Cadena JSON con el listado de discapacidades</returns>
        [HttpPost]
        [Authorize]
        public string GetDiscapacidades([FromBody] DiscapacidadRequest request)
        {
            var response = new DiscapacidadResponse();
            var discapacidadesQuery = _context.Discapacidad.Where(d => d.DeletedAt == null);
            if (!string.IsNullOrEmpty(request.Nombre))
            {
                discapacidadesQuery = discapacidadesQuery.Where(e => e.Nombre.Contains(request.Nombre));
            }

            response.Total = discapacidadesQuery.Count();
            response.Discapacidades = discapacidadesQuery.OrderBy(d => d.Id)
                .Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();
            return JsonSedeshu.SerializeObject(response);
        }

        /// <summary>
        /// Funcion que importa desde un archivo de Excel el catalogo de discapacidades, la importacion de estos registros reemplazara a los registros existentes en la base de datos
        /// </summary>
        /// <param name="model">Archivo Excel con el listado de discapacidades</param>
        /// <returns>Estatus de la importacion</returns>
        [HttpPost]
        [Authorize]
        public string Importar(DiscapacidadArchivo model)
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
                                var importacion = _context.Importacion.Find("Discapacidades");
                                var pestana = archivoExcel.Worksheets.First();
                                var discapacidadesDB = _context.Discapacidad.ToDictionary(e => e.Id);
                                foreach (var discapacidad in discapacidadesDB)
                                {
                                    discapacidad.Value.UpdatedAt = now;
                                    discapacidad.Value.DeletedAt = now;
                                    _context.Discapacidad.Update(discapacidad.Value);
                                }

                                for (var i = 2; i <= pestana.Dimension.End.Row; i++)
                                {
                                    if (pestana.Cells[i,1].Value==null)
                                    {
                                        continue;
                                    }
                                    var id = pestana.Cells[i, 1].Value.ToString();
                                    Discapacidad discapacidad;
                                    if (discapacidadesDB.TryGetValue(id, out discapacidad))
                                    {
                                        discapacidad.Nombre = pestana.Cells[i, 2].Value.ToString();
                                        discapacidad.UpdatedAt = now;
                                        discapacidad.DeletedAt = null;
                                        _context.Discapacidad.Update(discapacidad);
                                    }
                                    else
                                    {
                                        discapacidad = new Discapacidad();
                                        discapacidad.Id = pestana.Cells[i, 1].Value.ToString();
                                        discapacidad.Nombre = pestana.Cells[i, 2].Value.ToString();
                                        discapacidad.CreatedAt = now;
                                        discapacidad.UpdatedAt = now;
                                        _context.Discapacidad.Add(discapacidad);
                                    }
                                }

                                if (importacion == null)
                                {
                                    importacion = new Importacion();
                                    importacion.Clave = "Discapacidades";
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
                                    Mensaje = "Se importó el catálogo de discapacidades.",
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
        /// Funcion que muestra la pantalla para crear o editar la informacion de una discapacidad
        /// </summary>
        /// <param name="id">Identificar de la discapaicdad a modificar o null si es nueva</param>
        /// <returns>Vista para crear o editar la discapacidad</returns>
        [HttpGet]
        [Authorize]
        public IActionResult CreateEdit(string id = "")
        {
            var discapacidad = new DiscapacidadCreateEditModel();
            var discapacidadDB = _context.Discapacidad.Find(id);
            if (string.IsNullOrEmpty(id))
            {
                ViewData["Title"] = "Crear discapacidad";
                discapacidad.Id = "";
                discapacidad.IdAnterior = "";
                discapacidad.Nombre = "";
            }
            else
            {
                ViewData["Title"] = "Editar discapacidad";
                discapacidad.Id = discapacidadDB.Id;
                discapacidad.IdAnterior = discapacidadDB.Id;
                discapacidad.Nombre = discapacidadDB.Nombre;
            }

            discapacidad.GradosId = _context.DiscapacidadGrado
                .Where(dg => dg.DiscapacidadId.Equals(discapacidad.Id) && dg.DeletedAt == null).Select(dg => dg.GradoId)
                .ToList();
            discapacidad.Grados = new List<Grado>();
            foreach (var grado in _context.Grado.Where(g => g.DeletedAt == null))
            {
                discapacidad.Grados.Add(new Grado()
                {
                    Id = grado.Id,
                    Nombre = grado.Nombre
                });
            }

            return View("Create", discapacidad);
        }


        /// <summary>
        /// Funcion que guarda en la base de datos la informacion de la discapacidad
        /// </summary>
        /// <param name="model">Datos de la discapacidad</param>
        /// <returns>Estatus de guardadon en la base de datos</returns>
        [HttpPost]
        [Authorize]
        [RequireClaim("Permiso", Value = "catalogos.editar")]
        public string Guardar([FromBody] DiscapacidadCreateEditModel model)
        {
            if (_context.Discapacidad.Any(x => x.DeletedAt == null && x.Id == model.Id && x.Id != model.IdAnterior))
            {
                ModelState.AddModelError("Id", "Este Id ya fue creado.");
            }

            if (_context.Discapacidad.Any(x => x.Id != model.Id && model.Nombre != model.Nombre))
            {
                ModelState.AddModelError("Nombre", "Esta discapacidad ya fue creada.");
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
                var discapacidadDB = _context.Discapacidad.Find(model.Id);
                var grados = _context.DiscapacidadGrado.Where(dg => dg.DiscapacidadId.Equals(model.Id)).ToDictionary(dg=>dg.GradoId);
                foreach (var grado in grados)
                {
                    grado.Value.DeletedAt = now;
                    _context.DiscapacidadGrado.Update(grado.Value);
                }
                
                if (discapacidadDB == null)
                {
                    nueva = true;
                    discapacidadDB = new Discapacidad
                    {
                        CreatedAt = now
                    };
                }

                discapacidadDB.Id = model.Id;
                discapacidadDB.Nombre = model.Nombre;
                discapacidadDB.UpdatedAt = now;
                discapacidadDB.DeletedAt = null;
                
                if (nueva)
                {
                    _context.Discapacidad.Add(discapacidadDB);
                }
                else
                {
                    _context.Discapacidad.Update(discapacidadDB);
                }
                _context.SaveChanges();

                foreach (var gradoId in model.GradosId)
                {
                    if (grados.ContainsKey(gradoId))
                    {
                        var grado = grados[gradoId];
                        grado.UpdatedAt = now;
                        grado.DeletedAt = null;
                        _context.DiscapacidadGrado.Update(grado);
                    }
                    else
                    {
                        var grado = new DiscapacidadGrado
                        {
                            GradoId = gradoId, DiscapacidadId = discapacidadDB.Id, CreatedAt = now, UpdatedAt = now
                        };
                        _context.DiscapacidadGrado.Add(grado);
                    }
                }
                _context.Bitacora.Add(new Bitacora
                {
                    UsuarioId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                    Accion = nueva ? AccionBitacora.INSERCION : AccionBitacora.EDICION,
                    Mensaje = "Se "+(nueva ? "insertó":"modificó")+" la discapacidad " + model.Nombre + ".",
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
        /// Funcion que exporta a Excel el listado de discapacidades de acuerdo a los filtros seleccionados por el usuario 
        /// </summary>
        /// <param name="id">Filtros seleccionados por el usuario</param>
        /// <returns>Archivo de Excel con el listado de dispacidades</returns>
        [HttpPost]
        [Authorize]
        public IActionResult Exportar(DiscapacidadRequest request)
        {
            using (var excel = new ExcelPackage())
            {                
                var discapacidadesDB = _context.Discapacidad.Where(e => e.DeletedAt == null);
                if (!string.IsNullOrEmpty(request.Nombre))
                {
                    discapacidadesDB = discapacidadesDB.Where(z => z.Nombre.Contains(request.Nombre));
                }

                excel.Workbook.Worksheets.Add("Discapacidades");
                var pestana = excel.Workbook.Worksheets.First();
                pestana.Cells[1, 1].Value = "Id";
                pestana.Cells[1, 2].Value = "Nombre";
                var row = 2;
                foreach (var discapacidad in discapacidadesDB)
                {
                    pestana.Cells[row, 1].Value = discapacidad.Id;
                    pestana.Cells[row++, 2].Value = discapacidad.Nombre;
                }

                _context.Bitacora.Add(new Bitacora()
                {
                    UsuarioId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                    Accion = AccionBitacora.EXPORTACION,
                    Mensaje = "Se exportó el catálogo de discapacidades.",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                });
                _context.SaveChanges();

                return File(
                    fileContents: excel.GetAsByteArray(),
                    contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    fileDownloadName: "discapacidades.xlsx"
                );
            }
        }
    }
}