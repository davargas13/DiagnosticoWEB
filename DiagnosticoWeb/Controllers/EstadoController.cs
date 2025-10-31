using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query.ExpressionVisitors.Internal;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using DiagnosticoWeb.Code;
using DiagnosticoWeb.Database;
using DiagnosticoWeb.Models;
using DiagnosticoWeb.Validaciones;

namespace DiagnosticoWeb.Controllers
{
    /// <summary>
    /// Clase que permite la visualizacion, adicion, edicion e importacion del catalogo de estados del pais
    /// </summary>
    public class EstadoController : Controller
    {
        public readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="context">Conexion a la base de datos</param>
        /// <param name="userManager">Variable que nos permite saber que usuario esta usando el sistema</param>
        public EstadoController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        /// <summary>
        /// Funcion que muestra la vista para la consulta de los estados registrados en la base de datos
        /// </summary>
        /// <returns>Vista con el listado de estados</returns>
        [HttpGet]
        [Authorize]
        [RequireClaim("Permiso", Value = "catalogos.ver")]
        public IActionResult Index()
        {
            var request = new EstadoRequest();
            var importacion = _context.Importacion.Find("Estados");
            if (importacion != null)
            {
                request.Usuario = _context.Users.Find(importacion.UsuarioId).Name;
                request.ImportedAt = importacion.ImportedAt.ToString();
            }

            return View("Index", request);
        }

        /// <summary>
        /// Funcion que consulta en la base de datos los estados registrados de acuerdo a los filtros de busqueda
        /// especificados por el usuario
        /// </summary>
        /// <param name="request">Filtros de busqueda</param>
        /// <returns>Cadena JSON con el listado de estados</returns>
        [HttpPost]
        [Authorize]
        public string GetEstados([FromBody] EstadoRequest request)
        {
            var response = new EstadoResponse();
            var estadosQuery = _context.Estado.Where(x => x.DeletedAt == null);
            if (!string.IsNullOrEmpty(request.Clave))
            {
                estadosQuery = estadosQuery.Where(e => e.Clave.Equals(request.Clave));
            }

            if (!string.IsNullOrEmpty(request.Nombre))
            {
                estadosQuery = estadosQuery.Where(e => e.Nombre.Contains(request.Nombre));
            }
            response.Total = estadosQuery.Count();
            response.Estados = estadosQuery.OrderBy(x=>x.Clave).Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize).ToList();
            return JsonConvert.SerializeObject(response);
        }

        /// <summary>
        /// Funcion que importa el archivo de EXCEL donde se encuentran los estados, la importacion de esta informacion
        /// sobreescribira los registros almacenados previamente
        /// </summary>
        /// <param name="model">Archivo de EXCEL</param>
        /// <returns>Estatus de la importacion: ok, error</returns>
        [HttpPost]
        [Authorize]
        public string Importar(EstadoArchivo model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Select(x => new {Key = x.Key, Error = x.Value.Errors.First().ErrorMessage})
                    .ToList();

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
                                var importacion = _context.Importacion
                                    .FirstOrDefault(i => i.Clave.Equals("Estados"));
                                var pestana = archivoExcel.Worksheets.First();
                                var estadosDB = _context.Estado.ToDictionary(e => e.Id);
                                foreach (var estado in estadosDB)
                                {
                                    estado.Value.UpdatedAt = now;
                                    estado.Value.DeletedAt = now;
                                    _context.Estado.Update(estado.Value);
                                }
                                for (var i = 2; i <= pestana.Dimension.End.Row; i++)
                                {
                                    if (pestana.Cells[i,1].Value==null)
                                    {
                                        continue;
                                    }
                                    var id = pestana.Cells[i, 2].Value.ToString();
                                    Estado estado;
                                    if (estadosDB.TryGetValue(id, out estado))
                                    {
                                        estado.Clave = pestana.Cells[i, 1].Value.ToString();
                                        estado.Abreviacion = pestana.Cells[i, 4].Value.ToString();
                                        estado.Nombre = pestana.Cells[i, 3].Value.ToString();
                                        estado.UpdatedAt = now;
                                        estado.DeletedAt = null;
                                        _context.Estado.Update(estado);
                                    }
                                    else
                                    {
                                        estado = new Estado();
                                        estado.Id = pestana.Cells[i, 2].Value.ToString();
                                        estado.Clave = pestana.Cells[i, 1].Value.ToString();
                                        estado.Abreviacion = pestana.Cells[i, 4].Value.ToString();
                                        estado.Nombre = pestana.Cells[i, 3].Value.ToString();
                                        estado.CreatedAt = now;
                                        estado.UpdatedAt = now;
                                        _context.Estado.Add(estado);
                                    }
                                }

                                if (importacion == null)
                                {
                                    importacion = new Importacion();
                                    importacion.Clave = "Estados";
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
                                    Mensaje = "Se importó el catálogo de estados.",
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
                    "Ocurrió un error importando los estados. Revise el archivo de excel.");
                var errors = ModelState.Select(x => new {Key = x.Key, Error = x.Value.Errors.First().ErrorMessage})
                    .ToList();

                return JsonConvert.SerializeObject(errors);
            }
        }

        /// <summary>
        /// Funcion que muestra la vista para la adicion o edicion en la informacion de un estado
        /// </summary>
        /// <param name="id">Identificador en base de datos del estado a modificar, null si es un nuevo registro</param>
        /// <returns>Vista con el formulario para configurar el registro</returns>
        [HttpGet]
        [Authorize]
        public IActionResult CreateEdit(string id = "")
        {
            var estado = new EstadoModel();
            var estadoDB = _context.Estado.Find(id);
            if (string.IsNullOrEmpty(id))
            {
                ViewData["Title"] = "Crear estado";
                estado.Id = "";
                estado.IdAnterior = "";
                estado.Clave = "";
                estado.Abreviacion = "";
                estado.Nombre = "";
            }
            else
            {
                ViewData["Title"] = "Editar estado";
                estado.Id = estadoDB.Id;
                estado.IdAnterior = estadoDB.Id;
                estado.Clave = estadoDB.Clave;
                estado.Abreviacion = estadoDB.Abreviacion;
                estado.Nombre = estadoDB.Nombre;
            }

            return View("Create", estado);
        }
        
        /// <summary>
        /// Funcion que guarda en la base de datos la informacion del estado 
        /// </summary>
        /// <param name="model">Datos del estado</param>
        /// <returns>Estatus de guardado: ok, error</returns>
        [HttpPost]
        [Authorize]
        [RequireClaim("Permiso", Value = "catalogos.editar")]
        public string Guardar([FromBody] EstadoModel model)
        {
            if (_context.Estado.Any(x => x.Id == model.Id && x.Id != model.IdAnterior))
            {
                ModelState.AddModelError("Id", "Este Id ya fue creado.");
            }
            if (_context.Estado.Any(x => x.Nombre == model.Nombre.Trim() && x.Id != model.Id))
            {
                ModelState.AddModelError("Nombre", "Este estado ya fue creado.");
            }
            if (_context.Estado.Any(x => x.Clave== model.Clave.Trim() && x.Id != model.Id))
            {
                ModelState.AddModelError("Clave", "Este estado ya fue creado.");
            }
            if (_context.Estado.Any(x => x.Abreviacion== model.Abreviacion.Trim() && x.Id != model.Id))
            {
                ModelState.AddModelError("Abreviacion", "Este estado ya fue creado.");
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
                var estadoDB = _context.Estado.Find(model.Id);
                if (estadoDB == null)
                {
                    estadoDB = new Estado();
                    estadoDB.Id = model.Id;
                    estadoDB.Clave = model.Clave;
                    estadoDB.Abreviacion = model.Abreviacion;
                    estadoDB.Nombre = model.Nombre;
                    estadoDB.CreatedAt = now;
                    estadoDB.UpdatedAt = now;
                    _context.Estado.Add(estadoDB);

                    _context.Bitacora.Add(new Bitacora()
                    {
                        UsuarioId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                        Accion = AccionBitacora.INSERCION,
                        Mensaje = "Se insertó el estado " + estadoDB.Nombre + ".",
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    });

                }
                else
                {
                    estadoDB.Id = model.Id;
                    estadoDB.Clave = model.Clave;
                    estadoDB.Abreviacion = model.Abreviacion;
                    estadoDB.Nombre = model.Nombre;
                    estadoDB.UpdatedAt = now;
                    estadoDB.DeletedAt = null;
                    _context.Estado.Update(estadoDB);

                    _context.Bitacora.Add(new Bitacora()
                    {
                        UsuarioId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                        Accion = AccionBitacora.EDICION,
                        Mensaje = "Se modificó el estado " + estadoDB.Nombre + ".",
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
        /// Funcion que exporta a EXCEL los estados registrados en la base de datos de acuerdo a los filtros de busqueda
        /// especificados por el usuario
        /// </summary>
        /// <param name="id">Filtros seleccionados por el usuario</param>
        /// <returns>Archivo EXCEL con el listado de estados encontrados en la base de datos</returns>
        [HttpPost]
        [Authorize]
        public IActionResult Exportar(EstadoRequest request)
        {            
            using (var excel = new ExcelPackage()){
                try
                {                    
                    var estadosQuery = _context.Estado.Where(x => x.DeletedAt == null);
                    if (!string.IsNullOrEmpty(request.Clave))
                    {
                        estadosQuery = estadosQuery.Where(e => e.Clave.Equals(request.Clave));
                    }

                    if (!string.IsNullOrEmpty(request.Nombre))
                    {
                        estadosQuery = estadosQuery.Where(e => e.Nombre.Contains(request.Nombre));
                    }
                    excel.Workbook.Worksheets.Add("Estados");
                    var pestana = excel.Workbook.Worksheets.First();
                    pestana.Cells[1, 1].Value = "Clave";
                    pestana.Cells[1, 2].Value = "Id";
                    pestana.Cells[1, 3].Value = "Nombre";
                    pestana.Cells[1, 4].Value = "Abreviación";
                    var estadosDB = estadosQuery.ToList();
                    var row = 2;
                    foreach (var estado in estadosDB)
                    {
                        pestana.Cells[row, 1].Value = estado.Clave;
                        pestana.Cells[row, 2].Value = estado.Id;
                        pestana.Cells[row, 3].Value = estado.Nombre;
                        pestana.Cells[row++, 4].Value = estado.Abreviacion;
                    }

                    _context.Bitacora.Add(new Bitacora()
                    {
                        UsuarioId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                        Accion = AccionBitacora.EXPORTACION,
                        Mensaje = "Se exportó el catálogo de estados.",
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    });
                    _context.SaveChanges();
                }
                        
                catch (Exception e)
                {
                    Excepcion.Registrar(e);
                }
                return File(
                    fileContents: excel.GetAsByteArray(),
                    contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    fileDownloadName: "estados.xlsx"
                );
            }
        }
        
        /// <summary>
        /// Funcion para consultar los estados
        /// </summary>
        /// <returns>Cadena JSON con el listado de estados</returns>
        [HttpGet]
        [Authorize]
        public string Get()
        {
            var estados = _context.Estado.Where(l => l.DeletedAt == null).ToList();
            return JsonSedeshu.SerializeObject(estados);
        }
    }
}