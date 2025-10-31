using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.ExpressionTranslators.Internal;
using Newtonsoft.Json;
using DiagnosticoWeb.Code;
using DiagnosticoWeb.Database;
using DiagnosticoWeb.Models;
using DiagnosticoWeb.Validaciones;

namespace DiagnosticoWeb.Controllers
{
    /// <summary>
    /// Clase que permite la logica de negocio para la consulta, creacion, edicion de dependencia en el portal administrativo
    /// </summary>
    public class DependenciasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="context">Conexion a la base de datos</param>
        /// <param name="userManager">Objeto que permite saber elrol del usuario que esta usando el sistema</param>
        public DependenciasController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        /// <summary>
        /// Funcion que muestra la vista para la consulta de las dependencias registradas en la base de datos
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Authorize(Policy = "Catalogos")] 
        [RequireClaim("Permiso", Value = "dependencias.ver")]
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Funcion que consulta en la base de datos las dependencias registradas de acuerdo a los filtros seleccionados por el usuario
        /// </summary>
        /// <param name="request">Filtros seleccionados por el usuario</param>
        /// <returns>Cadena JSON con el listado de dependencias encontradas</returns>
        [HttpPost]
        [Authorize]
        public string GetDependencias([FromBody] DependenciasRequest request)
        {
            var response = new DependenciasResponse();
            var dependencias = _context.Dependencia.Where(v => v.Id != null);

            if (!string.IsNullOrEmpty(request.Nombre))
            {
                dependencias = dependencias.Where(v => v.Nombre.Contains(request.Nombre));
            }

            response.Total = dependencias.Count();
            response.Dependencias = dependencias.OrderBy(x => x.Nombre).Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize).ToList();
            return JsonConvert.SerializeObject(response);
        }

        /// <summary>
        /// Funcion que meustra la vista para la captura o modificacion en la informacion de una dependencia
        /// </summary>
        /// <param name="id">Identificador de la dependencia a modificar o null si es una nueva dependencia</param>
        /// <returns>Vista para la edicion o creacion de una dependencia</returns>
        [HttpGet]
        [Authorize]
        [RequireClaim("Permiso", Value = "dependencias.editar")]
        public IActionResult CreateEdit(string id = "")
        {
            var municipiosId = new List<string>();
            DependenciaCreateEditModel model;
            var municipios = _context.Municipio.ToDictionary(m => m.Id);

            if (string.IsNullOrEmpty(id))
            {
                ViewData["Title"] = "Crear dependencia";
                model = new DependenciaCreateEditModel()
                {
                    Id = "",
                    Nombre = "",
                    Zonas = new List<Zona>()
                };
            }
            else
            {
                ViewData["Title"] = "Editar dependencia";
                var dependencia = _context.Dependencia.Where(x => x.Id.Equals(id)).First();
                var zonas = _context.Zona.Where(z => z.DependenciaId.Equals(dependencia.Id) && z.DeletedAt == null)
                    .ToList();
                model = new DependenciaCreateEditModel()
                {
                    Id = dependencia.Id,
                    Nombre = dependencia.Nombre,
                    Zonas = zonas,
                };
                foreach (var zona in model.Zonas)
                {
                    var municipiosZona = _context.MunicipioZona
                        .Where(mz => mz.ZonaId.Equals(zona.Id) && mz.DeletedAt == null).ToList();
                    zona.Municipios = new List<MunicipiosZona>();
                    foreach (var municipioZona in municipiosZona)
                    {
                        var mz = new MunicipiosZona();
                        mz.Id = municipioZona.MunicipioId;
                        mz.Nombre = municipios[municipioZona.MunicipioId].Nombre;
                        municipiosId.Add(municipioZona.MunicipioId);
                        zona.Municipios.Add(mz);
                    }
                }
            }

            model.Municipios = municipios.Values.Where(m => !municipiosId.Contains(m.Id)).ToList();

            return View("CreateEdit", model);
        }

        /// <summary>
        /// Funcion que registra en la base de datos la informacion de la dependencia
        /// </summary>
        /// <param name="model">Datos de la dependencia</param>
        /// <returns>Estatus de registro en la base de datos</returns>
        [HttpPost]
        [Authorize]
        public string Guardar([FromBody] DependenciaCreateEditModel model)
        {
            var result = _context.Dependencia.Any(x => x.Nombre == model.Nombre && x.Id != model.Id);
            if (result) {
                ModelState.AddModelError("Nombre", "Esta dependencia ya fue creada.");
            }
            var izona = 0;
            foreach (var zonaModel in model.Zonas)
            {
                if (zonaModel.Municipios.Count == 0)
                {
                    ModelState.AddModelError("Zonas["+izona+"].Municipios", "La región no tiene municipios asociados.");    
                }

                izona++;
            }
            if (!ModelState.IsValid) {
                var errors = ModelState.Select(x => new { Key = x.Key, Error = x.Value.Errors.First().ErrorMessage }).ToList();

                return JsonConvert.SerializeObject(errors);
            }


            using (var transaction = _context.Database.BeginTransaction())
            {
                var now = DateTime.Now;

                Dependencia dep;

                if (string.IsNullOrEmpty(model.Id)) {
                    dep = new Dependencia() {
                        Nombre = model.Nombre,
                        CreatedAt = now,
                        UpdatedAt = now
                    };
                    _context.Dependencia.Add(dep);

                    _context.Bitacora.Add(new Bitacora()
                    {
                        UsuarioId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                        Accion = AccionBitacora.INSERCION,
                        Mensaje = "Se insertó la dependencia " + dep.Nombre + ".",
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    });
                } else {
                    dep = _context.Dependencia.Find(model.Id);
                    dep.Nombre = model.Nombre;
                    dep.UpdatedAt = now;

                    _context.Dependencia.Update(dep);

                    _context.Bitacora.Add(new Bitacora()
                    {
                        UsuarioId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                        Accion = AccionBitacora.EDICION,
                        Mensaje = "Se modificó la dependencia " + dep.Nombre + ".",
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    });
                }

                var zonasDb = _context.Zona.Where(z => z.DependenciaId.Equals(dep.Id)).ToDictionary(z => z.Id);
                var municipiosDic = new Dictionary<string, Dictionary<string, MunicipioZona>>();
                foreach (var zonadb in zonasDb) {
                    zonadb.Value.DeletedAt = now;
                    _context.Zona.Update(zonadb.Value);
                    municipiosDic.Add(zonadb.Key, _context.MunicipioZona.Where(mz => mz.ZonaId.Equals(zonadb.Key) && mz.DeletedAt==null)
                        .ToDictionary(mz=>mz.MunicipioId));
                    foreach (var zonaMunicipioDB in municipiosDic[zonadb.Key])
                    {
                        zonaMunicipioDB.Value.DeletedAt = now;
                        _context.MunicipioZona.Update(zonaMunicipioDB.Value);
                    }
                }

                foreach (var zonaModel in model.Zonas) {
                    var zona = new Zona();
                    if (zonasDb.ContainsKey(zonaModel.Id)) {
                        zona = zonasDb[zonaModel.Id];
                        zona.Clave = zonaModel.Clave;
                        zona.UpdatedAt = now;
                        zona.DeletedAt = null;
                        _context.Zona.Update(zona);
                    } else {
                        zona.Clave = zonaModel.Clave;
                        zona.DependenciaId = dep.Id;
                        zona.CreatedAt = now;
                        zona.UpdatedAt = now;
                        _context.Zona.Add(zona);
                    }
                    foreach (var municipio in zonaModel.Municipios)
                    {
                        var zonaMunicipio = new MunicipioZona();
                        if (municipiosDic.ContainsKey(zonaModel.Id) && municipiosDic[zonaModel.Id].ContainsKey(municipio.Id))
                        {
                            zonaMunicipio = municipiosDic[zonaModel.Id][municipio.Id];
                            zonaMunicipio.UpdatedAt = now;
                            zonaMunicipio.DeletedAt = null;
                            _context.MunicipioZona.Update(zonaMunicipio);
                        }
                        else
                        {
                            zonaMunicipio.CreatedAt = now;
                            zonaMunicipio.UpdatedAt = now;
                            zonaMunicipio.ZonaId = zona.Id;
                            zonaMunicipio.MunicipioId = municipio.Id;
                            _context.MunicipioZona.Add(zonaMunicipio);
                        }
                    }
                }

                _context.SaveChanges();
                transaction.Commit();
            }

            TempData["Alert"] = "Show";
            TempData.Keep("Alert");

            return "ok";
        }

        /// <summary>
        /// Funcion que consulta las regiones que tiene configurada la dependencia, recordar que cada dependencia puede asignar sus propias zonas del estado
        /// </summary>
        /// <param name="id">Identificador en base de datos de la dependencia</param>
        /// <returns>Cadena JSON con las regiones y sus municipios de la dependencia</returns>
        [HttpGet]
        [Authorize]
        public string GetRegiones(string id="")
        {
            var regiones = new List<Zona>();
            var zonas = _context.Zona.Where(z => z.DeletedAt == null && z.DependenciaId.Equals(id)).Include(z=>z.MunicipioZonas).ThenInclude(mz=>mz.Municipio);
            foreach (var zona in zonas)
            {
                var region = new Zona()
                {
                    Id = zona.Id,
                    Clave = zona.Clave,
                    MunicipioZonas = new List<MunicipioZona>()
                };
                foreach (var zonaMunicipioZona in zona.MunicipioZonas.OrderBy(mz=>mz.Municipio.Nombre))
                {
                    region.MunicipioZonas.Add(new MunicipioZona()
                    {
                        Municipio = new Municipio()
                        {
                            Id = zonaMunicipioZona.MunicipioId,
                            Nombre = zonaMunicipioZona.Municipio.Nombre
                        }
                    });
                }
                regiones.Add(region);
            }
            return JsonSedeshu.SerializeObject(regiones);
        }
    }
}