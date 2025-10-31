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
    /// Clase con la logica de negocio para la visualizacion, adicion, edicion, importacion y exportacion del catalogo de zonas de impulso
    /// </summary>
    public class ZonaController : Controller
    {
        public readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="context">Conexion a la base de datos</param>
        /// <param name="userManager">Variable para identificar que usuario esta usando el sistema</param>
        public ZonaController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        /// <summary>
        /// Funcion que muestra la vista con el listado de zonas de impulso social registradas en la base de datos
        /// </summary>
        /// <returns>Vista con el listado de zonas de impulso social registradas en la base de datos</returns>
        [HttpGet]
        [Authorize]
        [RequireClaim("Permiso", Value = "catalogos.ver")]
        public IActionResult Index()
        {
            var request = new ZonaImpulsoRequest();
            var importacion = _context.Importacion.Find("ZonasImpulso");
            if (importacion != null)
            {
                request.Usuario = _context.Users.Find(importacion.UsuarioId).Name;
                request.ImportedAt = importacion.ImportedAt.ToString();
            }

            return View("Index", request);
        }

        /// <summary>
        /// Funcion que consulta en la base de datos las zonas de impulso social registradas de acuerdo a los filtros de
        /// busqueda seleccionados por el usuario
        /// </summary>
        /// <param name="request">Filtros de busqueda</param>
        /// <returns>Cadena JSON con el listado de zonas de impulso social</returns>
        [HttpPost]
        [Authorize]
        public string GetZonas([FromBody] ZonaImpulsoRequest request)
        {
            var response = new ZonaResponse();
            var zonasQuery = _context.ZonaImpulso.Where(z => z.DeletedAt == null);
            if (!string.IsNullOrEmpty(request.Nombre))
            {
                zonasQuery = zonasQuery.Where(e => e.Nombre.Contains(request.Nombre));
            }

            if (!string.IsNullOrEmpty(request.MunicipioId))
            {
                zonasQuery = zonasQuery.Where(e => e.MunicipioId == request.MunicipioId);
            }
            response.Total = zonasQuery.Count();
            response.Zonas = zonasQuery.Include(z => z.Municipio).OrderBy(z => z.Id)
                .Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize).ToList();
            return JsonConvert.SerializeObject(response);
        }

        /// <summary>
        /// Funcion que importa desde un archivo de EXCEL el catalogo de zonas de impulso social, la importacion sobreescribira
        /// los registros previos
        /// </summary>
        /// <param name="model">Archivo de EXCEL</param>
        /// <returns>Estatus de importacion: ok, error</returns>
        [HttpPost]
        [Authorize]
        public string Importar(ZonasImpulsoArchivo model)
        {
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
                                var importacion = _context.Importacion.FirstOrDefault(i => i.Clave.Equals("ZonasImpulso"));
                                var pestana = archivoExcel.Worksheets.First();
                                var zonasDB = _context.ZonaImpulso.ToDictionary(e => e.Id);
                                var nuevaImportacion = importacion == null;
                                foreach (var impulso in zonasDB)
                                {
                                    impulso.Value.UpdatedAt = now;
                                    impulso.Value.DeletedAt = now;
                                    _context.ZonaImpulso.Update(impulso.Value);
                                }
                                
                                for (var i = 2; i <= pestana.Dimension.End.Row; i++)
                                {
                                    if (pestana.Cells[i,1].Value==null)
                                    {
                                        continue;
                                    }

                                    var nueva = false;
                                    var id = pestana.Cells[i, 1].Value.ToString();
                                    ZonaImpulso zonaImpulso;
                                    if (!zonasDB.ContainsKey(id))
                                    {
                                        nueva = true;
                                        zonaImpulso = new ZonaImpulso
                                        {
                                            CreatedAt = now
                                        };
                                    }
                                    else
                                    {
                                        zonaImpulso = zonasDB[id];
                                    }
                                    zonaImpulso.Id = pestana.Cells[i, 1].Value.ToString();
                                    zonaImpulso.Nombre = pestana.Cells[i, 2].Value.ToString();
                                    zonaImpulso.MunicipioId = pestana.Cells[i, 3].Value.ToString();
                                    zonaImpulso.CreatedAt = now;
                                    zonaImpulso.UpdatedAt = now;
                                    zonaImpulso.DeletedAt = null;
                                    if (nueva)
                                    {
                                        _context.ZonaImpulso.Add(zonaImpulso);
                                    }
                                    else
                                    {
                                        _context.ZonaImpulso.Update(zonaImpulso);
                                    }
                                }
                                
                                if (nuevaImportacion)
                                {
                                    importacion = new Importacion();
                                }
                                importacion.Clave = "ZonasImpulso";
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
                                    Mensaje = "Se importó el catálogo de zonas de impulso.",
                                    CreatedAt = now,
                                    UpdatedAt = now
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
                    "Ocurrió un error importando las zonas de impulso. Revise el archivo de excel.");
                var errors = ModelState.Where(x => x.Value.Errors.Any())
                    .Select(x => new {Key = x.Key, Error = x.Value.Errors.First().ErrorMessage}).ToList();

                return JsonConvert.SerializeObject(errors);
            }

            return "ok";
        }

        /// <summary>
        /// Funcion que muestra la vista para la adicion, edicion en la informacion de una zona de impulso social
        /// </summary>
        /// <param name="id">Identificador en base de datos de la zona de impulso social, null si el registro es nuevo</param>
        /// <returns>Vista con el formulario para la configuracion de la zona de impulso social</returns>
        [HttpGet]
        [Authorize]
        public IActionResult CreateEdit(string id = "")
        {
            var zona = new ZonaCreateEditModel();
            var zonaDB = _context.ZonaImpulso.Find(id);
            if (string.IsNullOrEmpty(id))
            {
                ViewData["Title"] = "Crear zona de impulso";
                zona.Id = "";
                zona.IdAnterior = "";
                zona.Nombre = "";
                zona.MunicipioId = null;
            }
            else
            {
                ViewData["Title"] = "Editar zona de impulso";
                zona.Id = zonaDB.Id;
                zona.IdAnterior = zonaDB.Id;
                zona.Nombre = zonaDB.Nombre;
                zona.MunicipioId = zonaDB.MunicipioId;
            }

            zona.Municipios = _context.Municipio.ToList();
            return View("Create", zona);
        }

        /// <summary>
        /// Funcion para guardar en la base de datos la informacion de la zona de impulso social
        /// </summary>
        /// <param name="model">Datos de la zona de impulso social</param>
        /// <returns>Estatus del guardado: ok, error</returns>
        [HttpPost]
        [Authorize]
        [RequireClaim("Permiso", Value = "catalogos.editar")]
        public string Guardar([FromBody] ZonaCreateEditModel model)
        {
            if (_context.ZonaImpulso.Any(x => x.DeletedAt == null && x.Id == model.Id && x.Id != model.IdAnterior))
            {
                ModelState.AddModelError("Id", "Este Id ya fue creado.");
            }

            if (_context.ZonaImpulso.Any(x => x.Id != model.Id && model.Nombre != model.Nombre))
            {
                ModelState.AddModelError("Nombre", "Esta zona de impulso ya fue creada.");
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
                var zonaDB = _context.ZonaImpulso.Find(model.Id);
                if (zonaDB == null)
                {
                    zonaDB = new ZonaImpulso();
                    zonaDB.Id = model.Id;
                    zonaDB.Nombre = model.Nombre;
                    zonaDB.MunicipioId = model.MunicipioId;
                    zonaDB.CreatedAt = now;
                    zonaDB.UpdatedAt = now;
                    _context.ZonaImpulso.Add(zonaDB);

                    _context.Bitacora.Add(new Bitacora()
                    {
                        UsuarioId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                        Accion = AccionBitacora.INSERCION,
                        Mensaje = "Se insertó la zona de impulso " + model.Nombre + ".",
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    });
                }
                else
                {
                    zonaDB.Id = model.Id;
                    zonaDB.Nombre = model.Nombre;
                    zonaDB.MunicipioId = model.MunicipioId;
                    zonaDB.UpdatedAt = now;
                    zonaDB.DeletedAt = null;
                    _context.ZonaImpulso.Update(zonaDB);

                    _context.Bitacora.Add(new Bitacora()
                    {
                        UsuarioId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                        Accion = AccionBitacora.EDICION,
                        Mensaje = "Se modificó la zona de impulso " + model.Nombre + ".",
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
        /// Funcion que exporta el catalogo de zonas de impulso social a un archivo de EXCEL de acuerdo a los filtros de
        /// busqueda seleccionados por el usuario
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public IActionResult Exportar(ZonaImpulsoRequest request)
        {
            using (var excel = new ExcelPackage())
            {                 
                var zonasQuery = _context.ZonaImpulso.Where(e => e.DeletedAt == null);
                if (!string.IsNullOrEmpty(request.Nombre))
                {
                    zonasQuery = zonasQuery.Where(e => e.Nombre.Contains(request.Nombre));
                }

                if (!string.IsNullOrEmpty(request.MunicipioId))
                {
                    zonasQuery = zonasQuery.Where(e => e.MunicipioId == request.MunicipioId);
                }

                excel.Workbook.Worksheets.Add("Zonas de impulso");
                var pestana = excel.Workbook.Worksheets.First();
                pestana.Cells[1, 1].Value = "Id";
                pestana.Cells[1, 2].Value = "Nombre";
                pestana.Cells[1, 3].Value = "Municipio";
                var row = 2;
                foreach (var zona in zonasQuery)
                {
                    pestana.Cells[row, 1].Value = zona.Id;
                    pestana.Cells[row, 2].Value = zona.Nombre;
                    pestana.Cells[row++, 3].Value = zona.MunicipioId;
                }

                _context.Bitacora.Add(new Bitacora()
                {
                    UsuarioId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                    Accion = AccionBitacora.EXPORTACION,
                    Mensaje = "Se exportó el catálogo de zonas de impulso.",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                });
                _context.SaveChanges();

                return File(
                    fileContents: excel.GetAsByteArray(),
                    contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    fileDownloadName: "zonas de impulso.xlsx"
                );
            }
        }
        
        /// <summary>
        /// Funcion para consultar las zonas de impulso del municipio seleccionado
        /// </summary>
        /// <param name="id">Identificador en base de datos del municipio seleccionado</param>
        /// <returns>Cadena JSON con el listado de zonas de impulso pertenecientes al municipio</returns>
        [HttpGet]
        [Authorize]
        public string ByMunicipio(string id = "")
        {
            var localidades = _context.ZonaImpulso.Where(l => l.MunicipioId.Equals(id)).OrderBy(z=>z.Nombre).ToList();
            return JsonSedeshu.SerializeObject(localidades);
        }
    }
}