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
    /// Controlador que contiene la logica de negocio para listar, crear, editar e importar agebs al catalogo
    /// </summary>
    public class CaminoController : Controller
    {
        public readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="context">Conexion a la base de datos</param>
        /// <param name="userManager">Administrador de la sesion para identificar que usuario esta usando el sistema</param>
        public CaminoController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        
        /// <summary>
        /// Funcion que muestra la vista con el listado de caminos registrados en la base de datos
        /// </summary>
        /// <returns>Vista con el lsitado de caminos</returns>
        [HttpGet]
        [Authorize]
        [RequireClaim("Permiso", Value = "catalogos.ver")]
        public IActionResult Index()
        {
            var response = new CaminoRequest();
            var importacion = _context.Importacion.Find("Caminos");
            if (importacion != null)
            {
                response.Usuario = _context.Users.Find(importacion.UsuarioId).Name;
                response.ImportedAt = importacion.ImportedAt.ToString();
            }

            return View("Index", response);
        }

        /// <summary>
        /// Funcion que consulta en la base de datos los caminos registrados de acuerdo a los filtros de busqueda
        /// seleccionados por el usuario
        /// </summary>
        /// <param name="request">Filtros de busqueda</param>
        /// <returns>Cadena JSON con el listado de caminos</returns>
        [HttpPost]
        [Authorize]
        public string ObtenerCaminos([FromBody] CaminoRequest request)
        {
            var response = new CaminoResponse();
            var caminosQuery = _context.Camino.Where(x => x.DeletedAt == null);

            if (!string.IsNullOrEmpty(request.Nombre))
            {
                caminosQuery = caminosQuery.Where(x => x.Nombre.Contains(request.Nombre));
            }

            response.Total = caminosQuery.Count();
            var caminoslist = caminosQuery.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize)
                .ToList();
            response.Caminos = caminoslist;
            return JsonSedeshu.SerializeObject(response);
        }
        
        /// <summary>
        /// Funcion que muestra la vista para la adicion o edicion en los datos de un camino
        /// </summary>
        /// <param name="id">Identificador en base de datos del camino a modificar, null si el registro es nuevo</param>
        /// <returns>Vista con el formulario con los campos del camino</returns>
        [HttpGet]
        [Authorize]
        public IActionResult CreateEdit(string id = "")
        {
            var nuevo = string.IsNullOrEmpty(id);
            ViewData["Title"] = nuevo ? "Crear camino" : "Editar camino";
            var camino = _context.Camino.FirstOrDefault(x => x.Id.Equals(id));
            var model = new CaminoCreateEditModel
            {
                Id = nuevo ? "" : camino.Id,
                IdAnterior = nuevo ? "" :camino.Id,
                Nombre = nuevo ? "" :camino.Nombre,
            };
            return View(model);
        }

        /// <summary>
        /// Funcion para guardar en la base de datos la informacion del camino
        /// </summary>
        /// <param name="model">Datos del camino</param>
        /// <returns>Estatus de guardado: ok, error</returns>
        [HttpPost]
        [Authorize]
        [RequireClaim("Permiso", Value = "catalogos.editar")]
        public string Guardar([FromBody] CaminoCreateEditModel model)
        {
            if (_context.Camino.Any(x => x.DeletedAt == null && x.Id == model.Id && x.Id != model.IdAnterior))
            {
                ModelState.AddModelError("Id", "Este Id ya fue creado.");
            }
            if (_context.Camino.Any(x =>
                x.DeletedAt == null && !x.Id.Equals(model.Id) && x.Nombre.Equals(model.Nombre.Trim())))
            {
                ModelState.AddModelError("Nombre", "Este camino ya ha sido creado.");
            }

            if (!ModelState.IsValid)
            {
                var errores = ModelState.Select(x => new {Key = x.Key, Error = x.Value.Errors.First().ErrorMessage}).ToList();
                return JsonConvert.SerializeObject(errores);
            }
            
            var camino = _context.Camino.Find(model.Id);
            var nuevo = false;
            if (camino == null)
            {
                camino = new Camino
                {
                    CreatedAt = DateTime.Now,
                };
                nuevo = true;
            }
            camino.Id = model.Id;
            camino.Nombre = model.Nombre;
            camino.UpdatedAt = DateTime.Now;

            if (nuevo)
            {
                _context.Camino.Add(camino);
            }
            else
            {
                _context.Camino.Update(camino);
            }

            _context.Bitacora.Add(new Bitacora()
            {
                UsuarioId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                Accion = nuevo ? AccionBitacora.INSERCION : AccionBitacora.EDICION,
                Mensaje = nuevo ? "Se insertó el camino " + model.Nombre + "." : "Se modificó el camino " + camino.Nombre + ".",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            });

            _context.SaveChanges();

            TempData["Alert"] = "Show";
            TempData.Keep("Alert");

            return "ok";
        }

        
        /// <summary>
        /// Funcion para exportar el catalogo de municipios a un archivo de EXCEL de acuerdo a los filtros de busqueda
        /// seleccionados por el usuario
        /// </summary>
        /// <param name="id">Filtros de busqueda</param>
        /// <returns>Archivo de EXCEL</returns>
        [HttpPost]
        [Authorize]
        public IActionResult Exportar(MunicipioRequest request)
        {             
            using (var excel = new ExcelPackage())
            {
                var caminosquery = _context.Camino.Where(x => x.DeletedAt == null);
                if (!string.IsNullOrEmpty(request.Nombre))
                {
                    caminosquery = caminosquery.Where(e => e.Nombre.Contains(request.Nombre));
                }
                excel.Workbook.Worksheets.Add("Caminos");
                var pestana = excel.Workbook.Worksheets.First();
                pestana.Cells[1, 1].Value = "Id";
                pestana.Cells[1, 2].Value = "Nombre";
                
                var caminos = caminosquery.ToList();
                var row = 2;
                foreach (var camino in caminos)
                {
                    pestana.Cells[row, 1].Value = camino.Id;
                    pestana.Cells[row++, 2].Value = camino.Nombre;
                }

                _context.Bitacora.Add(new Bitacora()
                {
                    UsuarioId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                    Accion = AccionBitacora.EXPORTACION,
                    Mensaje = "Se exportó el catálogo de caminos.",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                });
                _context.SaveChanges();

                return File(
                    fileContents: excel.GetAsByteArray(),
                    contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    fileDownloadName: "caminos.xlsx"
                );
            }
        }
        
        /// <summary>
        /// Funcion para importar el catalogo de caminos desde un archivo de EXCEL, la importacion de estos registros
        /// sobreescribira los registros anteriores
        /// </summary>
        /// <param name="model">Archivo de EXCEL</param>
        /// <returns>Estatus de la importacion: ok, error</returns>
        [HttpPost]
        [Authorize]
        public string Importar(MunicipioArchivo model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Select(x => new {Key = x.Key, Error = x.Value.Errors.First().ErrorMessage});

                return JsonConvert.SerializeObject(errors);
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
                                var importacion = _context.Importacion.FirstOrDefault(i => i.Clave.Equals("Caminos"));
                                var pestana = archivoExcel.Worksheets.First();
                                var caminosDB = _context.Camino.ToDictionary(e => e.Id);
                                var nuevaImportacion = importacion == null;
                                foreach (var camino in caminosDB)
                                {
                                    camino.Value.UpdatedAt = now;
                                    camino.Value.DeletedAt = now;
                                    _context.Camino.Update(camino.Value);
                                }

                                for (var i = 2; i <= pestana.Dimension.End.Row; i++)
                                {
                                    if (pestana.Cells[i,1].Value==null)
                                    {
                                        continue;
                                    }
                                    var nueva = false;
                                    var id = pestana.Cells[i, 1].Value.ToString();
                                    Camino camino;
                                    if (!caminosDB.ContainsKey(id))
                                    {
                                        nueva = true;
                                        camino = new Camino
                                        {
                                            CreatedAt = now
                                        };
                                    }
                                    else
                                    {
                                        camino = caminosDB[id];
                                    }

                                    camino.Id = pestana.Cells[i, 1].Value.ToString();
                                    camino.Nombre = pestana.Cells[i, 2].Value.ToString();
                                    camino.UpdatedAt = now;
                                    camino.DeletedAt = null;
                                    if (nueva)
                                    {
                                        _context.Camino.Add(camino);
                                    }
                                    else
                                    {
                                        _context.Camino.Update(camino);
                                    }
                                }

                                if (nuevaImportacion)
                                {
                                    importacion = new Importacion();
                                }
                                importacion.Clave = "Caminos";
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
                                    Mensaje = "Se importó el catálogo de caminos.",
                                    CreatedAt = DateTime.Now,
                                    UpdatedAt = DateTime.Now
                                });

                                _context.SaveChanges();
                                transaction.Commit();
                            }
                        }
                    }
                }

                TempData["Alert"] = "Show";
                TempData.Keep("Alert");

                return "ok";
            }
            catch (Exception)
            {
                ModelState.AddModelError("File",
                    "Ocurrió un error importando los caminos. Revise el archivo de excel.");
                var errors = ModelState.Select(x => new {Key = x.Key, Error = x.Value.Errors.First().ErrorMessage})
                    .ToList();

                return JsonConvert.SerializeObject(errors);
            }
        }
    }
}