using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using DiagnosticoWeb.Code;
using DiagnosticoWeb.Database;
using DiagnosticoWeb.Models;
using DiagnosticoWeb.Validaciones;

namespace DiagnosticoWeb.Controllers
{
    /// <summary>
    /// Clase con la logica de negocio para la visualizacion, adicion o edicion de los programas sociales
    /// </summary>
    public class ProgramasSocialesController : Controller
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="context">Conexion a la base de datos</param>
        public ProgramasSocialesController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Funcion que muestra la vista con el listado de programas sociales 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [RequireClaim("Permiso", Value = "programasocial.ver")]
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Funcion que muestra la vista para la adicion, edicion de un programa social
        /// </summary>
        /// <param name="id">Identificador en base de datos del programa social a editar, null para un registro nuevo</param>
        /// <returns>Vista con el formulario para la captura de un programa social</returns>
        [HttpGet]
        [Authorize]
        [RequireClaim("Permiso", Value = "programasocial.editar")]
        public IActionResult CreateEdit(string id = "")
        {
            ProgramaSocialModel model;
            if (string.IsNullOrEmpty(id))
            {
                ViewData["Title"] = "Crear programa social";
                var vertientes = new List<VertienteCreateEditModel>();
                vertientes.Add(new VertienteCreateEditModel()
                {
                    Id = "",
                    Nombre = "",
                    Ejercicio = "",
                    Ciclo = "",
                    Costo = 0,
                    TipoEntrega = "Validación",
                    Carencias = new List<string>(),
                    UnidadId = ""
                });

                model = new ProgramaSocialModel()
                {
                    Id = "",
                    Nombre = "",
                    Proyecto = "",
                    DependenciaId = "",
                    Vertientes = vertientes,
                    Carencias = _context.Carencia.ToList()
                };
            }
            else
            {
                ViewData["Title"] = "Editar programa social";
                ProgramaSocial programaSocial = _context.ProgramaSocial.Find(id);
                var vertientes = new List<VertienteCreateEditModel>();
                var vertientesPS = _context.Vertiente
                    .Where(x => x.ProgramaSocialId.Equals(programaSocial.Id) && x.DeletedAt == null)
                    .OrderBy(x => x.UpdatedAt).ToList();
                foreach (var vertiente in vertientesPS)
                {
                    vertientes.Add(new VertienteCreateEditModel()
                    {
                        Id = vertiente.Id,
                        Nombre = vertiente.Nombre,
                        Ejercicio = vertiente.Ejercicio,
                        Ciclo = vertiente.Ciclo,
                        Costo = vertiente.Costo,
                        TipoEntrega = vertiente.TipoEntrega,
                        Vigencia = vertiente.Vigencia,
                        UnidadId = vertiente.UnidadId
                    });
                }

                model = new ProgramaSocialModel()
                {
                    Id = programaSocial.Id,
                    Nombre = programaSocial.Nombre,
                    Proyecto = programaSocial.Proyecto,
                    DependenciaId = programaSocial.DependenciaId,
                    Vertientes = vertientes,
                    Carencias = _context.Carencia.ToList()
                };
            }

            foreach (var vertiente in model.Vertientes)
            {
                vertiente.Evidencias =
                    _context.VertienteArchivo.Where(va => va.VertienteId.Equals(vertiente.Id) && va.DeletedAt == null)
                        .ToList();
                vertiente.Carencias = _context.VertienteCarencia
                    .Where(va => va.VertienteId.Equals(vertiente.Id) && va.DeletedAt == null)
                    .Select(vc => vc.CarenciaId).ToList();
            }

            return View("CreateEdit", model);
        }

        /// <summary>
        /// Funcion que consulta los programas sociales registrados en la base de datos de acuerdo a los filtros de
        /// busqueda seleccionados por el usuario
        /// </summary>
        /// <param name="request">Filtros de busqueda</param>
        /// <returns>Cadena JSON con el listado de programas sociales</returns>
        [HttpPost]
        [Authorize]
        public string GetProgramasSociales([FromBody] ProgramaSocialRequest request)
        {
            var response = new ProgramaSocialResponse();
            var programas = _context.ProgramaSocial.Include(x => x.Vertientes).Where(v => v.Id != null);

            if (!string.IsNullOrEmpty(request.Nombre))
            {
                programas = programas.Where(x => x.Nombre.Contains(request.Nombre));
            }

            if (!string.IsNullOrEmpty(request.DependenciaId))
            {
                programas = programas.Where(x => x.DependenciaId.Equals(request.DependenciaId));
            }

            response.Total = programas.Count();
            var progSociales = programas.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize)
                .ToList();
            var programasListado = new List<ListadoProgramaSocial>();

            foreach (var p in progSociales)
            {
                programasListado.Add(new ListadoProgramaSocial()
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    Dependencia = _context.Dependencia.Find(p.DependenciaId),
                    Vertientes = p.Vertientes.Where(x => x.DeletedAt == null).Count().ToString()
                });
            }

            response.ProgramasSociales =
                programasListado.OrderBy(x => x.Dependencia.Nombre).ThenBy(x => x.Nombre).ToList();

            return JsonConvert.SerializeObject(response);
        }

        /// <summary>
        /// Funcion que consulta las dependencias registradas en la base de datos para que el usuario selecciona la dependencia del programa social
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public string GetDependencias()
        {
            var response = new DependenciasResponseSelect();
            var dependencias = _context.Dependencia.Where(v => v.Id != null).ToList();

            response.Dependencias = dependencias;

            return JsonConvert.SerializeObject(response);
        }

        /// <summary>
        /// Funcion para guardar la informacion del programa social en la base de datos
        /// </summary>
        /// <param name="model">Datos del programa social</param>
        /// <returns>Estatus del guardado: ok, error</returns>
        [HttpPost]
        [Authorize]
        public string Guardar([FromBody] ProgramaSocialModel model)
        {
            var vertientesEliminar = new Dictionary<string, Vertiente>();
            var now = DateTime.Now;
            var result = _context.ProgramaSocial.Any(x =>
                x.Id != model.Id && x.Nombre == model.Nombre && x.DependenciaId == model.DependenciaId);
            if (result)
            {
                ModelState.AddModelError("Nombre", "Este programa social ya fue creado.");
            }

            var ievidencia = 0;
            foreach (var vertiente in model.Vertientes)
            {
                if (vertiente.Evidencias.Count == 0)
                {
                    ModelState.AddModelError("Vertientes[" + ievidencia + "].Documentos",
                        "Debe agregar por lo menos un tipo de documento a la vertiente.");
                }

                ievidencia++;
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Select(x => new {Key = x.Key, Error = x.Value.Errors.First().ErrorMessage})
                    .ToList();

                return JsonConvert.SerializeObject(errors);
            }

            ProgramaSocial programa;
            var carenciasDic = new Dictionary<string, Dictionary<string, VertienteCarencia>>();
            var documentosDic = new Dictionary<string, Dictionary<string, VertienteArchivo>>();
            if (string.IsNullOrEmpty(model.Id))
            {
                programa = new ProgramaSocial()
                {
                    Nombre = model.Nombre,
                    Proyecto = model.Proyecto,
                    DependenciaId = model.DependenciaId,
                    CreatedAt = now,
                    UpdatedAt = now
                };
                _context.ProgramaSocial.Add(programa);

                _context.Bitacora.Add(new Bitacora()
                {
                    UsuarioId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                    Accion = AccionBitacora.INSERCION,
                    Mensaje = "Se insertó el programa social " + programa.Nombre + ".",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                });
            }
            else
            {
                vertientesEliminar = _context.Vertiente
                    .Where(x => x.ProgramaSocialId.Equals(model.Id) && x.DeletedAt == null).ToDictionary(x => x.Id);
                foreach (var vertEl in vertientesEliminar.Values)
                {
                    vertEl.UpdatedAt = now;
                    vertEl.DeletedAt = now;
                    _context.Vertiente.Update(vertEl);
                    carenciasDic.Add(vertEl.Id,
                        _context.VertienteCarencia.Where(vc => vc.VertienteId.Equals(vertEl.Id) && vc.DeletedAt == null)
                            .ToDictionary(vc => vc.CarenciaId));
                    documentosDic.Add(vertEl.Id,
                        _context.VertienteArchivo.Where(vc => vc.VertienteId.Equals(vertEl.Id) && vc.DeletedAt == null)
                            .ToDictionary(vc => vc.Id));
                    foreach (var vertienteCarencia in carenciasDic[vertEl.Id])
                    {
                        vertienteCarencia.Value.DeletedAt = now;
                        _context.VertienteCarencia.Update(vertienteCarencia.Value);
                    }

                    foreach (var vertienteDocumento in documentosDic[vertEl.Id])
                    {
                        vertienteDocumento.Value.DeletedAt = now;
                        _context.VertienteArchivo.Update(vertienteDocumento.Value);
                    }
                }


                programa = _context.ProgramaSocial.Find(model.Id);
                programa.Nombre = model.Nombre;
                programa.Proyecto = model.Proyecto;
                programa.UpdatedAt = now;
                _context.ProgramaSocial.Update(programa);

                _context.Bitacora.Add(new Bitacora() {
                    UsuarioId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                    Accion = AccionBitacora.EDICION,
                    Mensaje = "Se modificó el programa social " + programa.Nombre + ".",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                });
            }

            foreach (var vertiente in model.Vertientes)
            {
                Vertiente vert;
                if (vertientesEliminar.TryGetValue(vertiente.Id, out vert))
                {
                    vert.Nombre = vertiente.Nombre;
                    vert.Ciclo = vertiente.Ciclo;
                    vert.Ejercicio = vertiente.Ejercicio;
                    vert.Costo = vertiente.Costo;
                    vert.UpdatedAt = now;
                    vert.DeletedAt = null;
                    vert.ProgramaSocial = programa;
                    vert.Vigencia = vertiente.Vigencia;
                    vert.TipoEntrega = vertiente.TipoEntrega;
                    vert.UnidadId = vertiente.UnidadId;
                    _context.Vertiente.Update(vert);
                }
                else
                {
                    vert = new Vertiente()
                    {
                        Nombre = vertiente.Nombre,
                        Ejercicio = vertiente.Ejercicio,
                        Ciclo = vertiente.Ciclo,
                        Costo = vertiente.Costo,
                        ProgramaSocialId = programa.Id,
                        CreatedAt = now,
                        UpdatedAt = now,
                        Vigencia = vertiente.Vigencia,
                        TipoEntrega = vertiente.TipoEntrega,
                        UnidadId = vertiente.UnidadId,
                        ProgramaSocial = programa
                    };

                    _context.Vertiente.Add(vert);
                }

                foreach (var evidencia in vertiente.Evidencias)
                {
                    var evi = new VertienteArchivo();
                    if (documentosDic.ContainsKey(vertiente.Id) &&
                        documentosDic[vertiente.Id].ContainsKey(evidencia.Id))
                    {
                        evi = documentosDic[vertiente.Id][evidencia.Id];
                        evi.UpdatedAt = now;
                        evi.DeletedAt = null;
                        evi.TipoArchivo = evidencia.TipoArchivo;
                        _context.VertienteArchivo.Update(evi);
                    }
                    else
                    {
                        evi.TipoArchivo = evidencia.TipoArchivo;
                        evi.CreatedAt = now;
                        evi.UpdatedAt = now;
                        evi.VertienteId = vert.Id;
                        _context.VertienteArchivo.Add(evi);
                    }
                }

                foreach (var carencia in vertiente.Carencias)
                {
                    var vertienteCarencia = new VertienteCarencia();
                    if (carenciasDic.ContainsKey(vertiente.Id) && carenciasDic[vertiente.Id].ContainsKey(carencia))
                    {
                        vertienteCarencia = carenciasDic[vertiente.Id][carencia];
                        vertienteCarencia.UpdatedAt = now;
                        vertienteCarencia.DeletedAt = null;
                        _context.VertienteCarencia.Update(vertienteCarencia);
                    }
                    else
                    {
                        vertienteCarencia.VertienteId = vert.Id;
                        vertienteCarencia.CarenciaId = carencia;
                        vertienteCarencia.UpdatedAt = now;
                        vertienteCarencia.CreatedAt = now;
                        _context.VertienteCarencia.Add(vertienteCarencia);
                    }
                }
            }

            _context.SaveChanges();

            TempData["Alert"] = "Show";
            TempData.Keep("Alert");
            return "ok";
        }

        [HttpPost]
        [Authorize]
        public string GetUnidades()
        {
            var unidades = new List<Unidad>();
            var unidadesDB = _context.Unidad.Where(u => u.DeletedAt == null);
            foreach (var unidad in unidadesDB)
            {
                unidades.Add(new Unidad()
                {
                    Id = unidad.Id,
                    Nombre = unidad.Nombre
                });
            }

            return JsonConvert.SerializeObject(unidades);
        }
    }
}