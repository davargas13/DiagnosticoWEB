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
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;

namespace DiagnosticoWeb.Controllers
{
    /// <summary>
    /// Controlador que contiene la logica de negocio para listar, crear, editar e importar manzanas al catalogo
    /// </summary>
    public class ManzanaController : Controller
    {
        public readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="context">Conexion a la base de datos</param>
        /// <param name="userManager">Administrador de la sesion para identificar que usuario esta usando el sistema</param>
        public ManzanaController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        
        /// <summary>
        /// Funcion que muestra la vista con el listado de manzanas registrados en la base de datos
        /// </summary>
        /// <returns>Vista con el lsitado de manzanas</returns>
        [HttpGet]
        [Authorize]
        [RequireClaim("Permiso", Value = "catalogos.ver")]
        public IActionResult Index()
        {
            var response = new ManzanaRequest();
            var importacion = _context.Importacion.Find("Manzanas");
            if (importacion != null)
            {
                response.Usuario = _context.Users.Find(importacion.UsuarioId).Name;
                response.ImportedAt = importacion.ImportedAt.ToString();
            }
            return View("Index", response);
        }

        /// <summary>
        /// Funcion que consulta en la base de datos las manzanas registrados de acuerdo a los filtros de busqueda
        /// seleccionados por el usuario
        /// </summary>
        /// <param name="request">Filtros de busqueda</param>
        /// <returns>Cadena JSON con el listado de manzanas</returns>
        [HttpPost]
        [Authorize]
        public string ObtenerManzanas([FromBody] ManzanaRequest request)
        {
            var response = new ManzanaResponse();
            var manzanasQuery = _context.Manzana
                .Include(m=>m.Municipio).Include(m=>m.Localidad)
                .Include(m=>m.Ageb).Where(x => x.DeletedAt == null);

            if (!string.IsNullOrEmpty(request.Nombre))
            {
                manzanasQuery = manzanasQuery.Where(x => x.Nombre.Contains(request.Nombre));
            }  
            
            if (!string.IsNullOrEmpty(request.MunicipioId))
            {
                manzanasQuery = manzanasQuery.Where(x => x.MunicipioId == request.MunicipioId);
            }
            
            if (!string.IsNullOrEmpty(request.LocalidadId))
            {
                manzanasQuery = manzanasQuery.Where(x => x.LocalidadId == request.LocalidadId);
            }
            
            if (!string.IsNullOrEmpty(request.AgebId))
            {
                manzanasQuery = manzanasQuery.Where(x => x.AgebId == request.AgebId);
            }

            response.Total = manzanasQuery.Count();
            var manzanasList = manzanasQuery.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize)
                .ToList();
            response.Manzanas = manzanasList;
            return JsonSedeshu.SerializeObject(response);
        }
        
        /// <summary>
        /// Funcion que muestra la vista para la adicion o edicion en los datos de una manzana
        /// </summary>
        /// <param name="id">Identificador en base de datos de la manzana a modificar, null si el registro es nuevo</param>
        /// <returns>Vista con el formulario con los campos de la manzana</returns>
        [HttpGet]
        [Authorize]
        public IActionResult CreateEdit(string id = "")
        {
            var nuevo = string.IsNullOrEmpty(id);
            ViewData["Title"] = nuevo ? "Crear municipio" : "Editar municipio";
            var manzana = _context.Manzana.FirstOrDefault(x => x.Id.Equals(id));
            var model = new ManzanaCreateEditModel
            {
                Id = nuevo ? "" : manzana.Id,
                IdAnterior = nuevo ? "" :manzana.Id,
                Nombre = nuevo ? "" :manzana.Nombre,
                MunicipioId = nuevo ? "" :manzana.MunicipioId,
                LocalidadId = nuevo ? "" :manzana.LocalidadId,
                AgebId = nuevo ? "" :manzana.AgebId,
            };
            return View(model);
        }

        [HttpGet("/Manzana/ByAgeb/{agebId}")]
        [Authorize]
        public string ByLocalidad(string agebId = "")
        {
            var manzanas = _context.Manzana.Where(m => m.AgebId==agebId).OrderBy(m=>m.Nombre);
            var agebList = new List<Model>();
            foreach (var manzana in manzanas)
            {
                agebList.Add(new Model
                {
                    Id = manzana.Id,
                    Nombre = manzana.Nombre
                });
            }
            return JsonSedeshu.SerializeObject(agebList);
        }
        
        /// <summary>
        /// Funcion para guardar en la base de datos la informacion de la manzana
        /// </summary>
        /// <param name="model">Datos de la manzana</param>
        /// <returns>Estatus de guardado: ok, error</returns>
        [HttpPost]
        [Authorize]
        [RequireClaim("Permiso", Value = "catalogos.editar")]
        public string Guardar([FromBody] ManzanaCreateEditModel model)
        {
            if (_context.Manzana.Any(x => x.DeletedAt == null && x.Id == model.Id && x.Id != model.IdAnterior))
            {
                ModelState.AddModelError("Id", "Este Id ya fue creado.");
            }

            if (!ModelState.IsValid)
            {
                var errores = ModelState.Select(x => new {Key = x.Key, Error = x.Value.Errors.First().ErrorMessage}).ToList();
                return JsonConvert.SerializeObject(errores);
            }
            
            var manzana = _context.Manzana.Find(model.Id);
            var nuevo = false;
            if (manzana == null)
            {
                manzana = new Manzana
                {
                    CreatedAt = DateTime.Now,
                };
                nuevo = true;
            }
            manzana.Id = model.Id;
            manzana.Nombre = model.Nombre;
            manzana.MunicipioId = model.MunicipioId;
            manzana.LocalidadId = model.LocalidadId;
            manzana.AgebId = model.AgebId;
            manzana.UpdatedAt = DateTime.Now;

            if (nuevo)
            {
                _context.Manzana.Add(manzana);
            }
            else
            {
                _context.Manzana.Update(manzana);
            }

            _context.Bitacora.Add(new Bitacora()
            {
                UsuarioId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                Accion = nuevo ? AccionBitacora.INSERCION : AccionBitacora.EDICION,
                Mensaje = nuevo ?"Se insertó la manzana " + model.Nombre + ".": "Se modificó la manzana " + manzana.Nombre + ".",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            });

            _context.SaveChanges();

            TempData["Alert"] = "Show";
            TempData.Keep("Alert");

            return "ok";
        }

        
        /// <summary>
        /// Funcion para exportar el catalogo de manzanas a un archivo de EXCEL de acuerdo a los filtros de busqueda
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
                var manzanasQuery = _context.Manzana.Where(x => x.DeletedAt == null);
                if (!string.IsNullOrEmpty(request.Nombre))
                {
                    manzanasQuery = manzanasQuery.Where(e => e.Nombre.Contains(request.Nombre));
                }
                excel.Workbook.Worksheets.Add("Manzanas");
                var pestana = excel.Workbook.Worksheets.First();
                pestana.Cells[1, 1].Value = "Id";
                pestana.Cells[1, 2].Value = "Nombre";
                pestana.Cells[1, 3].Value = "MunicipioId";
                pestana.Cells[1, 4].Value = "LocalidadId";
                pestana.Cells[1, 5].Value = "AgebId";
                var manzanas = manzanasQuery.ToList();
                var row = 2;
                foreach (var manzana in manzanas)
                {
                    pestana.Cells[row, 1].Value = manzana.Id;
                    pestana.Cells[row, 2].Value = manzana.Nombre;
                    pestana.Cells[row, 3].Value = manzana.MunicipioId;
                    pestana.Cells[row, 4].Value = manzana.LocalidadId;
                    pestana.Cells[row++, 5].Value = manzana.AgebId;
                }

                _context.Bitacora.Add(new Bitacora()
                {
                    UsuarioId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                    Accion = AccionBitacora.EXPORTACION,
                    Mensaje = "Se exportó el catálogo de manzanas.",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                });
                _context.SaveChanges();

                return File(
                    fileContents: excel.GetAsByteArray(),
                    contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    fileDownloadName: "manzanas.xlsx"
                );
            }
        }
        
        /// <summary>
        /// Funcion para importar el catalogo de manzanas desde un archivo de EXCEL, la importacion de estos registros
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
            
            var now = DateTime.Now;
            var importacion = _context.Importacion.FirstOrDefault(i => i.Clave.Equals("Manzanas"));
            var nuevaImportacion = importacion == null;
            
            if (nuevaImportacion)
            {
                importacion = new Importacion();
            }
            importacion.Clave = "Manzanas";
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
                Mensaje = "Se importó el catálogo de manzanas.",
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
        /// Función que inserta o actualiza las manzanas del archivo de excel utilizando bulkinsert
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
                              var manzanasNuevas = new Dictionary<string, Manzana>(); 
                              var pestana = archivoExcel.Worksheets.First();
                              var manzanasAnteriores = _context.Manzana.ToDictionary(e => e.Id);
                              foreach (var manzana in manzanasAnteriores)
                              {
                                  manzana.Value.UpdatedAt = now;
                                  manzana.Value.DeletedAt = now;
                              }
                              for (var i = 2; i <= pestana.Dimension.End.Row; i++)
                              {
                                  if (pestana.Cells[i,1].Value==null)
                                  {
                                      continue;
                                      
                                  }
                                  var id = pestana.Cells[i, 1].Value.ToString();
                                  Manzana manzana;
                                  if (!manzanasAnteriores.ContainsKey(id))
                                  {
                                      manzana = new Manzana
                                      {
                                          CreatedAt = now
                                          
                                      };
                                      manzanasNuevas.Add(id, manzana);
                                  }
                                  else
                                  {
                                      manzana = manzanasAnteriores[id];
                                  }
                                  manzana.Id = pestana.Cells[i, 1].Value.ToString();
                                  manzana.Nombre = pestana.Cells[i, 2].Value.ToString();
                                  manzana.MunicipioId = pestana.Cells[i, 3].Value.ToString();
                                  manzana.LocalidadId = pestana.Cells[i, 4].Value.ToString();
                                  manzana.AgebId = pestana.Cells[i, 5].Value.ToString();
                                  manzana.UpdatedAt = now;
                                  manzana.DeletedAt = null;
                              }
                              var bulkConfig = new BulkConfig {SetOutputIdentity = true, BatchSize = 4000};
                              _context.BulkUpdate(manzanasAnteriores.Values.ToList(), bulkConfig);
                              _context.BulkInsert(manzanasNuevas.Values.ToList(), bulkConfig);
                              transaction.Commit();
                          }
                        }
                    }
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("File",
                    "Ocurrió un error importando las manzanas. Revise el archivo de excel.");
                var errors = ModelState.Select(x => new {Key = x.Key, Error = x.Value.Errors.First().ErrorMessage})
                    .ToList();

                return JsonConvert.SerializeObject(errors);
            }
            return "ok";
        }
    }
}