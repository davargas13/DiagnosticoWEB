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
using EFCore.BulkExtensions;
using Microsoft.Extensions.Configuration;

namespace DiagnosticoWeb.Controllers
{
    /// <summary>
    /// Controlador que contiene la logica de negocio para listar, crear, editar e importar agebs al catalogo
    /// </summary>
    public class AgebController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="context">Conexion a la base de datos</param>
        /// <param name="userManager">Administrador de la sesion para identificar que usuario esta usando el sistema</param>
        public AgebController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _configuration = configuration;
        }

        /// <summary>
        /// Funcion que muestra el listado de agebs registrados en la base de datos
        /// </summary>
        /// <returns>Vista con las agebs</returns>
        [HttpGet]
        [Authorize]
        [RequireClaim("Permiso", Value = "catalogos.ver")]
        public IActionResult Index()
        {
            var request = new AgebRequest();
            var importacion = _context.Importacion.Find("Agebs");
            if (importacion != null)
            {
                request.Usuario = _context.Users.Find(importacion.UsuarioId).Name;
                request.ImportedAt = importacion.ImportedAt.ToString();
            }

            return View("Index", request);
        }

        /// <summary>
        /// Funcion que filtra las agebs de la base de datos de acuerdo a los filtros seleccionados por el usuario
        /// </summary>
        /// <param name="request">Filtros seleccionados por el usuario</param>
        /// <returns>JSON con las agebs encontradas en la base de datos</returns>
        [HttpPost]
        [Authorize]
        public string GetAgebs([FromBody] AgebRequest request)
        {
            var response = new AgebResponse();
            var agebsQuery = _context.Ageb.Where(a => a.DeletedAt == null);
            if (!string.IsNullOrEmpty(request.Clave))
            {
                agebsQuery = agebsQuery.Where(e => e.Clave.Equals(request.Clave));
            }   
            if (!string.IsNullOrEmpty(request.MunicipioId))
            {
                agebsQuery = agebsQuery.Where(e => e.MunicipioId == request.MunicipioId);
            } 
            if (!string.IsNullOrEmpty(request.LocalidadId))
            {
                agebsQuery = agebsQuery.Where(e => e.LocalidadId == request.LocalidadId);
            }

            response.Total = agebsQuery.Count();
            response.Agebs = agebsQuery.OrderBy(a=>a.Clave).Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize)
                .Include(a => a.Localidad).ThenInclude(l => l.Municipio).ToList();
            return JsonSedeshu.SerializeObject(response);
        }

        /// <summary>
        /// Funcion que importa desde un excel las agebs a la base de datos del sistema
        /// </summary>
        /// <param name="model">Formulario que contiene el archivo de excel</param>
        /// <returns>Estatus de importacion</returns>
        [HttpPost]
        [Authorize]
        public string Importar(AgebArchivo model)
        {
            if (!ModelState.IsValid)
            {
                var errores = ModelState.Select(x => new {Key = x.Key, Error = x.Value.Errors.First().ErrorMessage})
                    .ToList();

                return JsonConvert.SerializeObject(errores);
            }

            var now = DateTime.Now;
            var importacion = _context.Importacion.FirstOrDefault(i => i.Clave.Equals("Agebs"));
            var nuevaImportacion = importacion == null;
            if (nuevaImportacion)
            {
                importacion = new Importacion();
            }
            importacion.Clave = "Agebs";
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
                Mensaje = "Se importó el catálogo de agebs.",
                CreatedAt = now,
                UpdatedAt = now
            });

            _context.SaveChanges();
            var status = BulkInsert(now, model.File);
            TempData["Alert"] = "Show";
            TempData.Keep("Alert");
            return status;
        }
        
        /// <summary>
        /// Función que inserta o actualiza las agebs del archivo de excel utilizando bulkinsert
        /// </summary>
        /// <param name="now">Fecha actual</param>
        /// <param name="archivo">Archivo de excel</param>
        /// <returns>Cadena con estatus de importacion</returns>
        public string BulkInsert(DateTime now, IFormFile archivo)
        {
            try
            {
                using (var package = new ExcelPackage(archivo.OpenReadStream()))
                {
                    var archivoExcel = package.Workbook;
                    if (archivoExcel != null)
                    {
                        if (archivoExcel.Worksheets.Count > 0)
                        {
                          using (var transaction = _context.Database.BeginTransaction()) 
                          { 
                              var agebsNuevas = new Dictionary<string, Ageb>(); 
                              var pestana = archivoExcel.Worksheets.First();
                              var agebsAnteriores = _context.Ageb.ToDictionary(e => e.Id);
                              foreach (var ageb in agebsAnteriores)
                              {
                                  ageb.Value.UpdatedAt = now;
                                  ageb.Value.DeletedAt = now;
                              }
                              for (var i = 2; i <= pestana.Dimension.End.Row; i++)
                              {
                                  if (pestana.Cells[i,1].Value==null)
                                  {
                                      continue;
                                      
                                  }
                                  var id = pestana.Cells[i, 1].Value.ToString();
                                  Ageb ageb;
                                  if (!agebsAnteriores.ContainsKey(id))
                                  {
                                      ageb = new Ageb
                                      {
                                          CreatedAt = now
                                          
                                      };
                                      agebsNuevas.Add(id, ageb);
                                  }
                                  else
                                  {
                                      ageb = agebsAnteriores[id];
                                  }
                                  ageb.Id = id;
                                  ageb.Clave = pestana.Cells[i, 2].Value.ToString();
                                  ageb.Nombre = pestana.Cells[i, 2].Value.ToString();
                                  ageb.EstadoId = pestana.Cells[i, 3].Value.ToString();
                                  ageb.MunicipioId = pestana.Cells[i, 4].Value.ToString();
                                  ageb.LocalidadId = pestana.Cells[i, 5].Value.ToString();
                                  ageb.UpdatedAt = now;
                                  ageb.DeletedAt = null;
                              }
                              var bulkConfig = new BulkConfig {SetOutputIdentity = true, BatchSize = 4000};
                              _context.BulkUpdate(agebsAnteriores.Values.ToList(), bulkConfig);
                              _context.BulkInsert(agebsNuevas.Values.ToList(), bulkConfig);
                              transaction.Commit();
                          }
                        }
                    }
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("File",
                    "Ocurrió un error importando las agebs. Revise el archivo de excel.");
                var errors = ModelState.Select(x => new {Key = x.Key, Error = x.Value.Errors.First().ErrorMessage})
                    .ToList();

                return JsonConvert.SerializeObject(errors);
            }
            return "ok";
        }

        /// <summary>
        /// Funcion que devuelve la vista para crear o editar los datos de una AGEB
        /// </summary>
        /// <param name="id">Id de la ageb a editar, null si la ageb es nueva</param>
        /// <returns>Vista para editar o crear la ageb</returns>
        [HttpGet]
        [Authorize]
        public IActionResult CreateEdit(string id = "")
        {
            var ageb = new AgebCreateEditModel();
            var agebDB = _context.Ageb.Find(id);
            var nueva = string.IsNullOrEmpty(id); 
            ViewData["Title"] = nueva ? "Crear Ageb" : "Editar Ageb";
            
            ageb.Id = nueva ? "" : agebDB.Id;
            ageb.IdAnterior = nueva ? "" :agebDB.Id;
            ageb.Clave = nueva ? "" :agebDB.Clave;
            ageb.MunicipioId = nueva ? "" :agebDB.MunicipioId;
            ageb.LocalidadId = nueva ? "" :agebDB.LocalidadId;

            ageb.Municipios = _context.Municipio.ToList();
            ageb.Localidades = new List<Localidad>();
            return View("Create", ageb);
        }


        /// <summary>
        /// Funcion que guarda en la base de datos la informacion de la ageb
        /// </summary>
        /// <param name="model">Datos de la ageb</param>
        /// <returns>Estatus de guardado</returns>
        [HttpPost]
        [Authorize]
        [RequireClaim("Permiso", Value = "catalogos.editar")]
        public string Guardar([FromBody] AgebCreateEditModel model)
        {
            if (_context.Ageb.Any(x => x.DeletedAt == null && x.Id == model.Id && x.Id != model.IdAnterior))
            {
                ModelState.AddModelError("Id", "Este Id ya fue creado.");
            }
            if (_context.Ageb.Any(x => x.Clave == model.Clave && x.Id != model.Id && model.MunicipioId != model.MunicipioId))
            {
                ModelState.AddModelError("Nombre", "Esta AGEB ya fue creada.");
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
                var agebDB = _context.Ageb.Find(model.Id);
                if (agebDB == null)
                {
                    nueva = true;
                    agebDB = new Ageb
                    {
                        CreatedAt = now
                    };
                    
                }
                
                agebDB.Id = model.Id;
                agebDB.Clave = model.Clave;
                agebDB.Nombre = model.Clave;
                agebDB.MunicipioId = model.MunicipioId;
                agebDB.LocalidadId = model.LocalidadId;
                agebDB.EstadoId = _configuration["GUANAJUATO"];
                agebDB.UpdatedAt = now;
                
                if (nueva)
                {
                    _context.Ageb.Add(agebDB);
                }
                else
                {
                    _context.Ageb.Update(agebDB);
                }
                
                _context.Bitacora.Add(new Bitacora
                {
                    UsuarioId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                    Accion = nueva ? AccionBitacora.INSERCION : AccionBitacora.EDICION,
                    Mensaje = "Se "+(nueva ? "insertó" : "modificó")+" la ageb " + model.Clave + ".",
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
        /// Funcion que exporta el listado de agebs de acuerdo a los filtros seleccionados por el usuario
        /// </summary>
        /// <param name="id">Filtros seleccionados por el usuario</param>
        /// <returns>Archivo de excel generado</returns>
        [HttpPost]
        [Authorize]
        public IActionResult Exportar(AgebRequest request)
        {
            using (var excel = new ExcelPackage())
            {                
                var agebsQuery = _context.Ageb.Where(a => a.DeletedAt == null);
                if (!string.IsNullOrEmpty(request.Clave))
                {
                    agebsQuery = agebsQuery.Where(e => e.Clave.Equals(request.Clave));
                }   
                if (!string.IsNullOrEmpty(request.MunicipioId))
                {
                    agebsQuery = agebsQuery.Where(e => e.MunicipioId == request.MunicipioId);
                } 
                if (!string.IsNullOrEmpty(request.LocalidadId))
                {
                    agebsQuery = agebsQuery.Where(e => e.LocalidadId == request.LocalidadId);
                }

                excel.Workbook.Worksheets.Add("Agebs");
                var pestana = excel.Workbook.Worksheets.First();
                pestana.Cells[1, 1].Value = "Id";
                pestana.Cells[1, 2].Value = "Clave";
                pestana.Cells[1, 3].Value = "Estado";
                pestana.Cells[1, 4].Value = "Municipio";
                pestana.Cells[1, 5].Value = "Localidad";
                var agebsDB = agebsQuery.ToList();
                var row = 2;
                foreach (var ageb in agebsDB)
                {
                    pestana.Cells[row, 1].Value = ageb.Id;
                    pestana.Cells[row, 2].Value = ageb.Clave;
                    pestana.Cells[row, 3].Value = ageb.EstadoId;
                    pestana.Cells[row, 4].Value = ageb.MunicipioId;
                    pestana.Cells[row++, 5].Value = ageb.LocalidadId;
                }

                _context.Bitacora.Add(new Bitacora()
                {
                    UsuarioId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                    Accion = AccionBitacora.EXPORTACION,
                    Mensaje = "Se exportó el catálogo de agebs.",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                });
                _context.SaveChanges();

                return File(
                    fileContents: excel.GetAsByteArray(),
                    contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    fileDownloadName: "agebs.xlsx"
                );
            }
        }
        
        [HttpGet("/Ageb/ByLocalidades/{localidadesIds}")]
        [Authorize]
        public string ByLocalidades(string localidadesIds = "")
        {
            string[] arrLocalidadesIds = localidadesIds.Split(new char[] { ',' });
            var localidades = _context.Localidad
                .Include(l => l.Agebs)
                .Where(l => arrLocalidadesIds.Contains(l.Id)).ToList();
            var agebList = new Dictionary<string, List<Ageb>>();
            foreach (var localidad in localidades)
            {
                agebList.Add(localidad.Nombre, localidad.Agebs);
            }
            return JsonSedeshu.SerializeObject(agebList);
        }
        
        [HttpGet("/Ageb/ByLocalidad/{localidadId}")]
        [Authorize]
        public string ByLocalidad(string localidadId = "")
        {
            var agebs = _context.Ageb.Where(a => a.LocalidadId==localidadId).OrderBy(a=>a.Clave);
            var agebList = agebs.Select(ageb => new Model {Id = ageb.Id, Nombre = ageb.Clave}).ToList();
            return JsonSedeshu.SerializeObject(agebList);
        }
    }
}