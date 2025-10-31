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
    /// Controlador que contiene la logica de negocio para listar, crear, editar e importar carreteras al catalogo
    /// </summary>
    public class CarreteraController : Controller
    {
        public readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="context">Conexion a la base de datos</param>
        /// <param name="userManager">Administrador de la sesion para identificar que usuario esta usando el sistema</param>
        public CarreteraController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        
        /// <summary>
        /// Funcion que muestra la vista con el listado de carreteras registrados en la base de datos
        /// </summary>
        /// <returns>Vista con el lsitado de carreteras</returns>
        [HttpGet]
        [Authorize]
        [RequireClaim("Permiso", Value = "catalogos.ver")]
        public IActionResult Index()
        {
            var response = new CarreteraRequest();
            var importacion = _context.Importacion.Find("Carreteras");
            if (importacion != null)
            {
                response.Usuario = _context.Users.Find(importacion.UsuarioId).Name;
                response.ImportedAt = importacion.ImportedAt.ToString();
            }

            return View("Index", response);
        }

        /// <summary>
        /// Funcion que consulta en la base de datos las carreteras registrados de acuerdo a los filtros de busqueda
        /// seleccionados por el usuario
        /// </summary>
        /// <param name="request">Filtros de busqueda</param>
        /// <returns>Cadena JSON con el listado de carreteras</returns>
        [HttpPost]
        [Authorize]
        public string ObtenerCarreteras([FromBody] CarreteraRequest request)
        {
            var response = new CarreteraResponse();
            var carreterasQuery = _context.Carretera.Where(x => x.DeletedAt == null);

            if (!string.IsNullOrEmpty(request.Nombre))
            {
                carreterasQuery = carreterasQuery.Where(x => x.Nombre.Contains(request.Nombre));
            }
            
            response.Total = carreterasQuery.Count();
            var carreterasList = carreterasQuery.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize)
                .ToList();
            response.Carreteras = carreterasList;
            return JsonSedeshu.SerializeObject(response);
        }
        
        /// <summary>
        /// Funcion que muestra la vista para la adicion o edicion en los datos de una carretera
        /// </summary>
        /// <param name="id">Identificador en base de datos de la carretera a modificar, null si el registro es nuevo</param>
        /// <returns>Vista con el formulario con los campos de la carretera</returns>
        [HttpGet]
        [Authorize]
        public IActionResult CreateEdit(string id = "")
        {
            var nuevo = string.IsNullOrEmpty(id);
            ViewData["Title"] = nuevo ? "Crear carretera" : "Editar carretera";
            var carretera = _context.Carretera.FirstOrDefault(x => x.Id.Equals(id));
            var model = new CarreteraCreateEditModel
            {
                Id = nuevo ? "" : carretera.Id,
                IdAnterior = nuevo ? "" :carretera.Id,
                Nombre = nuevo ? "" :carretera.Nombre
            };
            return View(model);
        }

        /// <summary>
        /// Funcion para guardar en la base de datos la informacion de la carretera
        /// </summary>
        /// <param name="model">Datos de la carretera</param>
        /// <returns>Estatus de guardado: ok, error</returns>
        [HttpPost]
        [Authorize]
        [RequireClaim("Permiso", Value = "catalogos.editar")]
        public string Guardar([FromBody] CarreteraCreateEditModel model)
        {
            if (_context.Carretera.Any(x => x.DeletedAt == null && x.Id == model.Id && x.Id != model.IdAnterior))
            {
                ModelState.AddModelError("Id", "Este Id ya fue creado.");
            }
            if (_context.Carretera.Any(x =>
                x.DeletedAt == null && !x.Id.Equals(model.Id) && x.Nombre.Equals(model.Nombre.Trim())))
            {
                ModelState.AddModelError("Nombre", "Este tipo de carretera ya ha sido creado.");
            }

            if (!ModelState.IsValid)
            {
                var errores = ModelState.Select(x => new {Key = x.Key, Error = x.Value.Errors.First().ErrorMessage}).ToList();
                return JsonConvert.SerializeObject(errores);
            }
            
            var carretera = _context.Carretera.Find(model.Id);
            var nuevo = false;
            if (carretera == null)
            {
                carretera = new Carretera
                {
                    CreatedAt = DateTime.Now,
                };
                nuevo = true;
            }
            carretera.Id = model.Id;
            carretera.Nombre = model.Nombre;
            carretera.UpdatedAt = DateTime.Now;

            if (nuevo)
            {
                _context.Carretera.Add(carretera);
            }
            else
            {
                _context.Carretera.Update(carretera);
            }

            _context.Bitacora.Add(new Bitacora()
            {
                UsuarioId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                Accion = nuevo ? AccionBitacora.INSERCION : AccionBitacora.EDICION,
                Mensaje = nuevo ?"Se insertó el tipo de carretera " + model.Nombre + ".": "Se modificó el tipo de carretera " + carretera.Nombre + ".",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            });

            _context.SaveChanges();

            TempData["Alert"] = "Show";
            TempData.Keep("Alert");

            return "ok";
        }

        
        /// <summary>
        /// Funcion para exportar el catalogo de carreteras a un archivo de EXCEL de acuerdo a los filtros de busqueda
        /// seleccionados por el usuario
        /// </summary>
        /// <param name="id">Filtros de busqueda</param>
        /// <returns>Archivo de EXCEL</returns>
        [HttpPost]
        [Authorize]
        public IActionResult Exportar(CarreteraRequest request)
        {             
            using (var excel = new ExcelPackage())
            {
                var carreterasQuery = _context.Carretera.Where(x => x.DeletedAt == null);
                if (!string.IsNullOrEmpty(request.Nombre))
                {
                    carreterasQuery = carreterasQuery.Where(e => e.Nombre.Contains(request.Nombre));
                }
                excel.Workbook.Worksheets.Add("Carreteras");
                var pestana = excel.Workbook.Worksheets.First();
                pestana.Cells[1, 1].Value = "Id";
                pestana.Cells[1, 2].Value = "Nombre";
                var carreteras = carreterasQuery.ToList();
                var row = 2;
                foreach (var carretera in carreteras)
                {
                    pestana.Cells[row, 1].Value = carretera.Id;
                    pestana.Cells[row++, 2].Value = carretera.Nombre;
                }

                _context.Bitacora.Add(new Bitacora()
                {
                    UsuarioId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                    Accion = AccionBitacora.EXPORTACION,
                    Mensaje = "Se exportó el catálogo de tipos de carreteras.",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                });
                _context.SaveChanges();

                return File(
                    fileContents: excel.GetAsByteArray(),
                    contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    fileDownloadName: "carreteras.xlsx"
                );
            }
        }
        
        /// <summary>
        /// Funcion para importar el catalogo de carreteras desde un archivo de EXCEL, la importacion de estos registros
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
                                var importacion = _context.Importacion.FirstOrDefault(i => i.Clave.Equals("Carreteras"));
                                var pestana = archivoExcel.Worksheets.First();
                                var carreterasDB = _context.Carretera.ToDictionary(e => e.Id);
                                var nuevaImportacion = importacion == null;
                                foreach (var carretera in carreterasDB)
                                {
                                    carretera.Value.UpdatedAt = now;
                                    carretera.Value.DeletedAt = now;
                                    _context.Carretera.Update(carretera.Value);
                                }

                                for (var i = 2; i <= pestana.Dimension.End.Row; i++)
                                {
                                    if (pestana.Cells[i,1].Value==null)
                                    {
                                        continue;
                                    }
                                    var nueva = false;
                                    var id = pestana.Cells[i, 1].Value.ToString();
                                    Carretera carretera;
                                    if (!carreterasDB.ContainsKey(id))
                                    {
                                        nueva = true;
                                        carretera = new Carretera
                                        {
                                            CreatedAt = now
                                        };
                                    }
                                    else
                                    {
                                        carretera = carreterasDB[id];
                                    }

                                    carretera.Id = pestana.Cells[i, 1].Value.ToString();
                                    carretera.Nombre = pestana.Cells[i, 2].Value.ToString();
                                    carretera.UpdatedAt = now;
                                    carretera.DeletedAt = null;
                                    if (nueva)
                                    {
                                        _context.Carretera.Add(carretera);
                                    }
                                    else
                                    {
                                        _context.Carretera.Update(carretera);
                                    }
                                }

                                if (nuevaImportacion)
                                {
                                    importacion = new Importacion();
                                }
                                importacion.Clave = "Carreteras";
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
                                    Mensaje = "Se importó el catálogo de carreteras.",
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
                    "Ocurrió un error importando los tipos de carreteras. Revise el archivo de excel.");
                var errors = ModelState.Select(x => new {Key = x.Key, Error = x.Value.Errors.First().ErrorMessage})
                    .ToList();

                return JsonConvert.SerializeObject(errors);
            }
        }
    }
}