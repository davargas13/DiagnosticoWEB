using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using DiagnosticoWeb.Database;
using DiagnosticoWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using OfficeOpenXml;
using DiagnosticoWeb.Code;
using System.Security.Claims;
using DiagnosticoWeb.Validaciones;
using Microsoft.Extensions.Configuration;

namespace DiagnosticoWeb.Controllers
{
    /// <summary>
    /// Clase con la logica de negocio para la visualizacion, adicion, edicion o importacion del catalogo de municipios
    /// </summary>
    public class MunicipiosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="context">Conexion a la base de datos</param>
        /// <param name="userManager">Variable para identificar que usuario esta usando el sistema</param>
        public MunicipiosController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _configuration = configuration;
        }

        /// <summary>
        /// Funcion que muestra la vista con el listado de municipios registrados en la base de datos
        /// </summary>
        /// <returns>Vista con el lsitado de municipios</returns>
        [HttpGet]
        [Authorize]
        [RequireClaim("Permiso", Value = "catalogos.ver")]
        public IActionResult Index()
        {
            var response = new MunicipioRequest();
            var importacion = _context.Importacion.Find("Municipios");
            if (importacion != null)
            {
                response.Usuario = _context.Users.Find(importacion.UsuarioId).Name;
                response.ImportedAt = importacion.ImportedAt.ToString();
            }

            return View("Index", response);
        }

        /// <summary>
        /// Funcion que consulta en la base de datos los municipios registrados de acuerdo a los filtros de busqueda
        /// seleccionados por el usuario
        /// </summary>
        /// <param name="request">Filtros de busqueda</param>
        /// <returns>Cadena JSON con el listado de municipios</returns>
        [HttpPost]
        [Authorize]
        public string ObtenerMunicipios([FromBody] MunicipioRequest request)
        {
            var response = new MunicipioResponse();
            var municipiosQuery = _context.Municipio.Include(x => x.Localidades).Where(x => x.DeletedAt == null);

            if (!string.IsNullOrEmpty(request.Nombre))
            {
                municipiosQuery = municipiosQuery.Where(x => x.Nombre.Contains(request.Nombre));
            }

            response.Total = municipiosQuery.Count();
            var municipiosList = municipiosQuery.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize)
                .ToList();

            var municipios = new List<MunicipioListModel>();
            foreach (var municipio in municipiosList)
            {
                var localidades = municipio.Localidades.Where(x => x.DeletedAt == null).Select(x => x.Nombre);

                municipios.Add(new MunicipioListModel()
                {
                    Id = municipio.Id,
                    Municipio = municipio.Nombre,
                    Localidades = localidades.Count().ToString()
                });
            }

            response.Municipios = municipios;

            return JsonConvert.SerializeObject(response);
        }

        /// <summary>
        /// Funcion que muestra la vista para la adicion o edicion en los datos de un municipio
        /// </summary>
        /// <param name="id">Identificador en base de datos del municipio a modificar, null si el registro es nuevo</param>
        /// <returns>Vista con el formulario con los campos del municipio</returns>
        [HttpGet]
        [Authorize]
        public IActionResult CreateEdit(string id = "")
        {            
            var municipio = _context.Municipio.FirstOrDefault(x => x.Id.Equals(id));
            var nueva = municipio == null;
            ViewData["Title"] = nueva ? "Crear municipio" : "Editar municipio";
            var model = new MunicipioCreateEditModel()
            {
                Id = nueva ? "" : municipio.Id,
                IdAnterior = nueva ? "" : municipio.Id,
                Nombre = nueva ? "" : municipio.Nombre,
                Clave = nueva ? "" : municipio.Clave
            };                     
            model.EstadoId = _configuration["GUANAJUATO"];
            return View(model);
        }

        /// <summary>
        /// Funcion para guardar en la base de datos la informacion del municipio
        /// </summary>
        /// <param name="model">Datos del municipio</param>
        /// <returns>Estatus de guardado: ok, error</returns>
        [HttpPost]
        [Authorize]
        [RequireClaim("Permiso", Value = "catalogos.editar")]
        public string Guardar([FromBody] MunicipioCreateEditModel model)
        {
            if (_context.Municipio.Any(x =>
                x.DeletedAt == null && x.Id == model.Id && x.Id != model.IdAnterior))
            {
                ModelState.AddModelError("Id", "Este Id ya fue creado.");
            }
            if (_context.Municipio.Any(x =>
                x.DeletedAt == null && !x.Id.Equals(model.Id) && x.Nombre.Equals(model.Nombre)))
            {
                ModelState.AddModelError("Nombre", "Este municipio ya ha sido creado.");
            }
            if (_context.Municipio.Any(x =>
                x.DeletedAt == null && !x.Id.Equals(model.Id) && x.Clave.Equals(model.Clave)))
            {
                ModelState.AddModelError("Clave", "Ya existe un municipio con esta clave.");
            }

            if (!ModelState.IsValid)
            {
                var errores = ModelState.Select(x => new {Key = x.Key, Error = x.Value.Errors.First().ErrorMessage})
                    .ToList();

                return JsonConvert.SerializeObject(errores);
            }
            var municipio = _context.Municipio.Find(model.Id);
            if (municipio == null)
            {
                _context.Municipio.Add(new Municipio()
                {
                    Id = model.Id,
                    Nombre = model.Nombre,
                    Clave = model.Clave,
                    EstadoId = model.EstadoId,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    Localidades = new List<Localidad>()
                });

                _context.Bitacora.Add(new Bitacora()
                {
                    UsuarioId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                    Accion = AccionBitacora.INSERCION,
                    Mensaje = "Se insertó el municipio " + model.Nombre + ".",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                });
            }
            else
            {
                municipio.Nombre = model.Nombre;
                municipio.Clave = model.Clave;
                municipio.EstadoId = model.EstadoId;
                municipio.UpdatedAt = DateTime.Now;
                _context.Municipio.Update(municipio);

                _context.Bitacora.Add(new Bitacora()
                {
                    UsuarioId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                    Accion = AccionBitacora.EDICION,
                    Mensaje = "Se modificó el municipio " + municipio.Nombre + ".",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                });
            }

            _context.SaveChanges();

            TempData["Alert"] = "Show";
            TempData.Keep("Alert");

            return "ok";
        }

        /// <summary>
        /// Funcion para importar el catalogo de municipios desde un archivo de EXCEL, la importacion de estos registros
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
                                var importacion = _context.Importacion.FirstOrDefault(i => i.Clave.Equals("Municipios"));
                                var nuevaImportacion = importacion == null;
                                var pestana = archivoExcel.Worksheets.First();
                                var municipiosDB = _context.Municipio.ToDictionary(e => e.Id);
                                foreach (var municipio in municipiosDB)
                                {
                                    municipio.Value.UpdatedAt = now;
                                    municipio.Value.DeletedAt = now;
                                    _context.Municipio.Update(municipio.Value);
                                }

                                for (var i = 2; i <= pestana.Dimension.End.Row; i++)
                                {
                                    if (pestana.Cells[i,1].Value==null)
                                    {
                                        continue;
                                    }
                                    var nueva = false;
                                    var id = pestana.Cells[i, 1].Value.ToString();
                                    Municipio municipio;
                                    if (!municipiosDB.ContainsKey(id))
                                    {
                                        nueva = true;
                                        municipio = new Municipio
                                        {
                                            CreatedAt = now
                                        };
                                    }
                                    else
                                    {
                                        municipio = municipiosDB[id];
                                    }

                                    municipio.Id = id;
                                    municipio.Clave = pestana.Cells[i, 2].Value.ToString();
                                    municipio.Nombre = pestana.Cells[i, 3].Value.ToString();
                                    municipio.EstadoId = pestana.Cells[i, 4].Value.ToString();
                                    municipio.UpdatedAt = now;
                                    municipio.DeletedAt = null;
                                    if (nueva)
                                    {
                                        _context.Municipio.Add(municipio);
                                    }
                                    else
                                    {
                                        _context.Municipio.Update(municipio);
                                    }
                                }

                                if (nuevaImportacion)
                                {
                                    importacion = new Importacion();
                                }
                                importacion.Clave = "Municipios";
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

                                _context.Bitacora.Add(new Bitacora()
                                {
                                    UsuarioId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                                    Accion = AccionBitacora.IMPORTACION,
                                    Mensaje = "Se importó el catálogo de municipios.",
                                    CreatedAt = now,
                                    UpdatedAt = now
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
                    "Ocurrió un error importando los municipios. Revise el archivo de excel.");
                var errors = ModelState.Select(x => new {Key = x.Key, Error = x.Value.Errors.First().ErrorMessage})
                    .ToList();

                return JsonConvert.SerializeObject(errors);
            }
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
                var municipiosQuery = _context.Municipio.Where(x => x.DeletedAt == null);
                if (!string.IsNullOrEmpty(request.Nombre))
                {
                    municipiosQuery = municipiosQuery.Where(e => e.Nombre.Contains(request.Nombre));
                }
                excel.Workbook.Worksheets.Add("Municipios");
                var pestana = excel.Workbook.Worksheets.First();
                pestana.Cells[1, 1].Value = "Id";
                pestana.Cells[1, 2].Value = "Clave";
                pestana.Cells[1, 3].Value = "Nombre";
                pestana.Cells[1, 4].Value = "EstadoId";
                var municipios = municipiosQuery.ToList();
                var row = 2;
                foreach (var municipio in municipios)
                {
                    pestana.Cells[row, 1].Value = municipio.Id;
                    pestana.Cells[row, 2].Value = municipio.Clave;
                    pestana.Cells[row, 3].Value = municipio.Nombre;
                    pestana.Cells[row++, 4].Value = municipio.EstadoId;
                }

                _context.Bitacora.Add(new Bitacora()
                {
                    UsuarioId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                    Accion = AccionBitacora.EXPORTACION,
                    Mensaje = "Se exportó el catálogo de municipios.",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                });
                _context.SaveChanges();

                return File(
                    fileContents: excel.GetAsByteArray(),
                    contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    fileDownloadName: "municipios.xlsx"
                );
            }
        }
        
        /// <summary>
        /// Funcion para consultar los municipios
        /// </summary>
        /// <returns>Cadena JSON con el listado de municipios</returns>
        [HttpGet]
        [Authorize]
        public string Get()
        {
            var municipios = _context.Municipio.Where(l => l.DeletedAt == null).ToList();
            return JsonSedeshu.SerializeObject(municipios);
        }
    }
}