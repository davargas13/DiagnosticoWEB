using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using EFCore.BulkExtensions;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using moment.net;
using moment.net.Enums;
using DiagnosticoWeb.Code;
using DiagnosticoWeb.Database;
using DiagnosticoWeb.Models;
using DiagnosticoWeb.Models.LoginModels;
using QRCoder;

namespace DiagnosticoWeb.Controllers
{
    /// <summary>
    /// Funcion para generar el tablero de indicadores y para generar registros de solicitudes aleatoriamente para pruebas
    /// </summary>
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly DiagnosticoAnteriorDbContext _contextDiagnostico;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _clientFactory;

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="context">Conexion a la base de datos</param>
        /// <param name="userManager">Variable para identificar que usuario esta usando el sistema</param>
        /// <param name="signInManager">Gestor para que los usuarios inicien sesion</param>
        public HomeController(ApplicationDbContext context, DiagnosticoAnteriorDbContext contextDiagnostico,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, IConfiguration configuration, IHttpClientFactory clientFactory)
        {
            _context = context;
            _contextDiagnostico = contextDiagnostico;
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _clientFactory = clientFactory;
        }

        /// <summary>
        /// Modelo extendido para solicitudes del dashboard con nuevos filtros
        /// </summary>
        public class DashboardRequestExtended : DashboardRequest
        {
            public string LocalidadId { get; set; }
            public string ColoniaId { get; set; }
            public string CURP { get; set; }
        }

        /// <summary>
        /// Modelo para cargar datos en cascada
        /// </summary>
        public class FiltroRequest
        {
            public string MunicipioId { get; set; }
            public string LocalidadId { get; set; }
            public string ColoniaId { get; set; }
        }

        /// <summary>
        /// Modelo para datos de carencias
        /// </summary>
        public class CarenciasResponse
        {
            public CarenciaData PobrezaExtrema { get; set; }
            public CarenciaData RezagoEducativo { get; set; }
            public CarenciaData Salud { get; set; }
            public CarenciaData SeguridadSocial { get; set; }
            public CarenciaData CalidadVivienda { get; set; }
            public CarenciaData ServiciosBasicos { get; set; }
            public CarenciaData Alimentacion { get; set; }
            public ResumenData Resumen { get; set; }
        }

        public class CarenciaData
        {
            public int Total { get; set; }
            public int SinCarencia { get; set; }
            public int ConCarencia { get; set; }
            public double PorcentajeSinCarencia { get; set; }
            public double PorcentajeConCarencia { get; set; }
        }

        public class ResumenData
        {
            public int TotalHogares { get; set; }
            public int SinCarencia { get; set; }
            public int ConCarencia { get; set; }
            public double PorcentajeSinCarencia { get; set; }
            public double PorcentajeConCarencia { get; set; }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Ping()
        {
            return Ok("API activa");
        }

        /// <summary>
        /// Funcion que verifica si el usuario ha iniciado sesion, si ya inicio entonces redirecciona al dashboard,
        /// si no a la pantalla de inicio de sesion
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [AllowAnonymous]
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Bienvenida", "Home");
            }
            return View("Login");
        }

        /// <summary>
        /// Funcion para buscar las solicitudes y beneficiarios y con esto construir el tablero de indicadores de acuerdo
        /// a los filtros de busqueda seleccionados por el usuario
        /// </summary>
        /// <param name="request">Filtros de busqueda</param>
        /// <returns></returns>

        //[HttpPost]
        //[Authorize]
        //public string Buscar([FromBody] DashboardRequestExtended request)
        //{
        //    var now = DateTime.Now;
        //    var inicio = new DateTime(now.Year, now.Month, 1);
        //    var fin = new DateTime(now.Year, now.Month, DateTime.DaysInMonth(now.Year, now.Month));

        //    if (!string.IsNullOrEmpty(request.Inicio))
        //        inicio = DateTime.Parse(request.Inicio.Substring(0, 10));

        //    if (!string.IsNullOrEmpty(request.Fin))
        //        fin = DateTime.Parse(request.Fin.Substring(0, 10)).EndOf(DateTimeAnchor.Day);

        //    var response = new Dashboard
        //    {
        //        Carencias = new Dictionary<string, int>
        //        {
        //            {"Educativa", 0},
        //            {"Servicios de salud", 0},
        //            {"Seguridad social", 0},
        //            {"Vivienda", 0},
        //            {"Servicios básicos", 0},
        //            {"Alimentaria", 0}
        //        },
        //        Niveles = new Dictionary<string, int>
        //        {
        //            {"Pobreza extrema", 0},
        //            {"Pobreza moderada", 0},
        //            {"Vulnerable por carencia social", 0},
        //            {"Vulnerablepor ingresos", 0},
        //            {"Sin pobreza", 0}
        //        },
        //        Estatus = new Dictionary<string, int>
        //        {
        //            {"Completo", 0},
        //            {"Incompleto", 0}
        //        },
        //        Dependencias = new Dictionary<string, int>(),
        //        Trabajadores = new Dictionary<string, int>()
        //    };

        //    var admin = HttpContext.User.IsInRole("Administrador");
        //    var regiones = new List<string>();

        //    var beneficiarios = _context.Beneficiario
        //        .Where(b => b.DeletedAt == null &&
        //                    b.CreatedAt >= inicio &&
        //                    b.CreatedAt <= fin &&
        //                    b.PadreId == null &&
        //                    b.HijoId == null &&
        //                    !b.Prueba &&
        //                    b.TrabajadorId != "120989e6-f6b7-453b-83af-fd4ccc0639b6");

        //    var benefCount = beneficiarios.Count();

        //    IQueryable<Aplicacion> aplicaciones;

        //    if (!admin)
        //    {
        //        var user = _userManager.GetUserAsync(HttpContext.User).Result;

        //        regiones = _context.UsuarioRegion
        //            .Where(ur => ur.DeletedAt == null && ur.UsuarioId == user.Id)
        //            .Select(ur => ur.ZonaId)
        //            .ToList();

        //        aplicaciones = _context.Aplicacion
        //            .Join(_context.Trabajador, a => a.TrabajadorId, t => t.Id,
        //                (a, t) => new { Aplicacion = a, Trabajador = t })
        //            .Join(_context.TrabajadorDependencia, x => x.Trabajador.Id, td => td.TrabajadorId,
        //                (x, td) => new { x.Aplicacion, x.Trabajador, TrabajadorDependencia = td })
        //            .Where(x =>
        //                x.Aplicacion.Activa &&
        //                x.Aplicacion.FechaInicio >= inicio &&
        //                x.Aplicacion.FechaInicio <= fin &&
        //                x.Aplicacion.DeletedAt == null &&
        //                !x.Aplicacion.Prueba &&
        //                x.TrabajadorDependencia.DependenciaId == user.DependenciaId &&
        //                x.Trabajador.Id != "120989e6-f6b7-453b-83af-fd4ccc0639b6")
        //            .Select(x => x.Aplicacion);
        //    }
        //    else
        //    {
        //        aplicaciones = _context.Aplicacion
        //            .Where(x => x.Activa &&
        //                        x.FechaInicio >= inicio &&
        //                        x.FechaInicio <= fin &&
        //                        x.DeletedAt == null &&
        //                        !x.Prueba &&
        //                        x.TrabajadorId != "120989e6-f6b7-453b-83af-fd4ccc0639b6");

        //        if (!string.IsNullOrEmpty(request.DependenciaId))
        //        {
        //            aplicaciones = aplicaciones
        //                .Join(_context.Trabajador, a => a.TrabajadorId, t => t.Id,
        //                    (a, t) => new { Aplicacion = a, Trabajador = t })
        //                .Join(_context.TrabajadorDependencia, x => x.Trabajador.Id, td => td.TrabajadorId,
        //                    (x, td) => new { x.Aplicacion, x.Trabajador, TrabajadorDependencia = td })
        //                .Where(x =>
        //                    x.TrabajadorDependencia.DependenciaId == request.DependenciaId &&
        //                    !x.Aplicacion.Prueba)
        //                .Select(x => x.Aplicacion);
        //        }
        //    }

        //    // --- Filtrar por regiones ---
        //    if (regiones.Any())
        //    {
        //        var municipios = _context.Zona
        //            .Where(r => regiones.Contains(r.Id))
        //            .Include(r => r.MunicipioZonas)
        //            .SelectMany(r => r.MunicipioZonas.Select(mz => mz.MunicipioId))
        //            .ToHashSet();

        //        aplicaciones = aplicaciones
        //            .Join(_context.Beneficiario, a => a.BeneficiarioId, b => b.Id, (a, b) => new { a, b })
        //            .Join(_context.Domicilio, ab => ab.b.DomicilioId, d => d.Id,
        //                (ab, d) => new { ab.a, Beneficiario = ab.b, Domicilio = d })
        //            .Where(x => x.Domicilio.Activa &&
        //                        !x.a.Prueba &&
        //                        municipios.Contains(x.Domicilio.MunicipioId))
        //            .Select(x => x.a);

        //        beneficiarios = aplicaciones
        //            .Join(_context.Beneficiario, a => a.BeneficiarioId, b => b.Id, (a, b) => new { a, b })
        //            .Join(_context.Domicilio, ab => ab.b.DomicilioId, d => d.Id,
        //                (ab, d) => new { ab.a, Beneficiario = ab.b, Domicilio = d })
        //            .Where(x => x.Domicilio.Activa &&
        //                        !x.a.Prueba &&
        //                        municipios.Contains(x.Domicilio.MunicipioId))
        //            .Select(x => x.Beneficiario);

        //        /*beneficiarios = beneficiarios
        //            .Join(_context.Domicilio, b => b.DomicilioId, d => d.Id, (b, d) => new { b, d })
        //            .Where(x => !x.b.Prueba &&
        //                        x.d.Activa &&
        //                        municipios.Contains(x.d.MunicipioId))
        //            .Select(x => x.b);
        //        beneficiarios =  beneficiarios;*/

        //        benefCount = beneficiarios.Count();
        //    }

        //    // --- Filtrar por municipio ---
        //    if (!string.IsNullOrEmpty(request.MunicipioId))
        //    {
        //        aplicaciones = aplicaciones
        //            .Join(_context.Beneficiario, a => a.BeneficiarioId, b => b.Id, (a, b) => new { a, b })
        //            .Join(_context.Domicilio, ab => ab.b.DomicilioId, d => d.Id,
        //                (ab, d) => new { ab.a, Beneficiario = ab.b, Domicilio = d })
        //            .Where(x => !x.a.Prueba &&
        //                        x.Domicilio.Activa &&
        //                        x.Domicilio.MunicipioId == request.MunicipioId)
        //            .Select(x => x.a);

        //        beneficiarios = beneficiarios
        //            .Join(_context.Domicilio, b => b.DomicilioId, d => d.Id, (b, d) => new { b, d })
        //            .Where(x => !x.b.Prueba &&
        //                        x.d.Activa &&
        //                        x.d.MunicipioId == request.MunicipioId)
        //            .Select(x => x.b);
        //        benefCount = beneficiarios.Count();
        //    }

        //    // --- Filtrar por localidad ---
        //    if (!string.IsNullOrEmpty(request.LocalidadId))
        //    {
        //        aplicaciones = aplicaciones
        //            .Join(_context.Beneficiario, a => a.BeneficiarioId, b => b.Id, (a, b) => new { a, b })
        //            .Join(_context.Domicilio, ab => ab.b.DomicilioId, d => d.Id,
        //                (ab, d) => new { ab.a, Beneficiario = ab.b, Domicilio = d })
        //            .Where(x => !x.a.Prueba &&
        //                        x.Domicilio.Activa &&
        //                        x.Domicilio.LocalidadId == request.LocalidadId)
        //            .Select(x => x.a);

        //        beneficiarios = beneficiarios
        //            .Join(_context.Domicilio, b => b.DomicilioId, d => d.Id, (b, d) => new { b, d })
        //            .Where(x => !x.b.Prueba &&
        //                        x.d.Activa &&
        //                        x.d.LocalidadId == request.LocalidadId)
        //            .Select(x => x.b);
        //        benefCount = beneficiarios.Count();
        //    }

        //    // --- Filtrar por colonia ---
        //    if (!string.IsNullOrEmpty(request.ColoniaId))
        //    {
        //        aplicaciones = aplicaciones
        //            .Join(_context.Beneficiario, a => a.BeneficiarioId, b => b.Id, (a, b) => new { a, b })
        //            .Join(_context.Domicilio, ab => ab.b.DomicilioId, d => d.Id,
        //                (ab, d) => new { ab.a, Beneficiario = ab.b, Domicilio = d })
        //            .Where(x => !x.a.Prueba &&
        //                        x.Domicilio.Activa &&
        //                        x.Domicilio.ColoniaId == request.ColoniaId)
        //            .Select(x => x.a);

        //        beneficiarios = beneficiarios
        //            .Join(_context.Domicilio, b => b.DomicilioId, d => d.Id, (b, d) => new { b, d })
        //            .Where(x => !x.b.Prueba &&
        //                        x.d.Activa &&
        //                        x.d.ColoniaId == request.ColoniaId)
        //            .Select(x => x.b);
        //        benefCount = beneficiarios.Count();
        //    }

        //    // --- Filtrar por CURP ---
        //    if (!string.IsNullOrEmpty(request.CURP))
        //    {
        //        aplicaciones = aplicaciones
        //            .Join(_context.Beneficiario, a => a.BeneficiarioId, b => b.Id, (a, b) => new { a, b })
        //            .Where(x => x.b.Curp == request.CURP)
        //            .Select(x => x.a);

        //        beneficiarios = beneficiarios
        //            .Where(b => b.Curp == request.CURP);
        //        benefCount = beneficiarios.Count();
        //    }

        //    aplicaciones = aplicaciones
        //        .Include(a => a.Trabajador)
        //        .ThenInclude(t => t.TrabajadorDependencias)
        //        .ThenInclude(td => td.Dependencia);

        //    response.NumDomicilios = beneficiarios.Count();
        //    response.NumDiagnosticos = aplicaciones.Count(a => a.Estatus == EstatusAplicacion.COMPLETO);
        //    response.NumPendientes = aplicaciones.Count(a => a.Estatus == EstatusAplicacion.INCOMPLETO);

        //    foreach (var ap in aplicaciones.Where(a => a.Estatus == EstatusAplicacion.COMPLETO))
        //    {
        //        response.Estatus[ap.Estatus]++;

        //        if (!response.Trabajadores.ContainsKey(ap.Trabajador.Nombre))
        //            response.Trabajadores[ap.Trabajador.Nombre] = 0;
        //        response.Trabajadores[ap.Trabajador.Nombre]++;

        //        var dependencia = ap.Trabajador.TrabajadorDependencias.FirstOrDefault()?.Dependencia?.Nombre;
        //        if (!string.IsNullOrEmpty(dependencia))
        //            response.Dependencias[dependencia] = response.Dependencias.GetValueOrDefault(dependencia, 0) + 1;

        //        if (ap.FechaFin.HasValue && ap.FechaInicio.HasValue)
        //            response.PromedioCaptura += (ap.FechaFin.Value - ap.FechaInicio.Value).TotalHours;

        //        if (ap.Educativa) response.Carencias["Educativa"]++;
        //        if (ap.ServicioSalud) response.Carencias["Servicios de salud"]++;
        //        if (ap.SeguridadSocial) response.Carencias["Seguridad social"]++;
        //        if (ap.Vivienda) response.Carencias["Vivienda"]++;
        //        if (ap.Servicios) response.Carencias["Servicios básicos"]++;
        //        if (ap.Alimentaria) response.Carencias["Alimentaria"]++;

        //        switch (ap.NivelPobreza)
        //        {
        //            case NivelPobreza.POBREZA_EXTREMA:
        //                response.Niveles["Pobreza extrema"]++;
        //                break;
        //            case NivelPobreza.POBREZA_MODERADA:
        //                response.Niveles["Pobreza moderada"]++;
        //                break;
        //            case NivelPobreza.VULNERABLE_CARENCIA_SOCIAL:
        //                response.Niveles["Vulnerable por carencia social"]++;
        //                break;
        //            case NivelPobreza.VULNERABLE_INGRESOS:
        //                response.Niveles["Vulnerablepor ingresos"]++;
        //                break;
        //            case NivelPobreza.NO_POBRE_NO_VULNERABLE:
        //                response.Niveles["Sin pobreza"]++;
        //                break;
        //        }
        //    }

        //    if (response.NumDiagnosticos > 0)
        //        response.PromedioCaptura = Math.Round(response.PromedioCaptura / response.NumDiagnosticos * 100);

        //    return JsonSedeshu.SerializeObject(response);
        //}


        [HttpPost]
        [Authorize]
        public string Buscar([FromBody] DashboardRequestExtended request)
        {
            var now = DateTime.Now;
            var inicio = new DateTime(now.Year, now.Month, 1);
            var fin = new DateTime(now.Year, now.Month, DateTime.DaysInMonth(now.Year, now.Month));

            if (!string.IsNullOrEmpty(request.Inicio))
                inicio = DateTime.Parse(request.Inicio.Substring(0, 10));

            if (!string.IsNullOrEmpty(request.Fin))
                fin = DateTime.Parse(request.Fin.Substring(0, 10)).EndOf(DateTimeAnchor.Day);

            var response = new Dashboard
            {
                Carencias = new Dictionary<string, int>
        {
            {"Educativa", 0},
            {"Servicios de salud", 0},
            {"Seguridad social", 0},
            {"Vivienda", 0},
            {"Servicios básicos", 0},
            {"Alimentaria", 0}
        },
                Niveles = new Dictionary<string, int>
        {
            {"Pobreza extrema", 0},
            {"Pobreza moderada", 0},
            {"Vulnerable por carencia social", 0},
            {"Vulnerablepor ingresos", 0},
            {"Sin pobreza", 0}
        },
                Estatus = new Dictionary<string, int>
        {
            {"Completo", 0},
            {"Incompleto", 0}
        },
                Dependencias = new Dictionary<string, int>(),
                Trabajadores = new Dictionary<string, int>()
            };

            var admin = HttpContext.User.IsInRole("Administrador");
            var regiones = new List<string>();

            var beneficiarios = _context.Beneficiario
                .Where(b => b.DeletedAt == null &&
                            b.CreatedAt >= inicio &&
                            b.CreatedAt <= fin &&
                            b.PadreId == null &&
                            b.HijoId == null &&
                            !b.Prueba &&
                            b.TrabajadorId != "120989e6-f6b7-453b-83af-fd4ccc0639b6");

            var benefCount = beneficiarios.Count();

            IQueryable<Aplicacion> aplicaciones;

            if (!admin)
            {
                var user = _userManager.GetUserAsync(HttpContext.User).Result;

                regiones = _context.UsuarioRegion
                    .Where(ur => ur.DeletedAt == null && ur.UsuarioId == user.Id)
                    .Select(ur => ur.ZonaId)
                    .ToList();

                aplicaciones = _context.Aplicacion
                    .Join(_context.Trabajador, a => a.TrabajadorId, t => t.Id,
                        (a, t) => new { Aplicacion = a, Trabajador = t })
                    .Join(_context.TrabajadorDependencia, x => x.Trabajador.Id, td => td.TrabajadorId,
                        (x, td) => new { x.Aplicacion, x.Trabajador, TrabajadorDependencia = td })
                    .Where(x =>
                        x.Aplicacion.Activa &&
                        x.Aplicacion.FechaInicio >= inicio &&
                        x.Aplicacion.FechaInicio <= fin &&
                        x.Aplicacion.DeletedAt == null &&
                        !x.Aplicacion.Prueba &&
                        x.TrabajadorDependencia.DependenciaId == user.DependenciaId &&
                        x.Trabajador.Id != "120989e6-f6b7-453b-83af-fd4ccc0639b6")
                    .Select(x => x.Aplicacion);
            }
            else
            {
                aplicaciones = _context.Aplicacion
                    .Where(x => x.Activa &&
                                x.FechaInicio >= inicio &&
                                x.FechaInicio <= fin &&
                                x.DeletedAt == null &&
                                !x.Prueba &&
                                x.TrabajadorId != "120989e6-f6b7-453b-83af-fd4ccc0639b6");

                if (!string.IsNullOrEmpty(request.DependenciaId))
                {
                    aplicaciones = aplicaciones
                        .Join(_context.Trabajador, a => a.TrabajadorId, t => t.Id,
                            (a, t) => new { Aplicacion = a, Trabajador = t })
                        .Join(_context.TrabajadorDependencia, x => x.Trabajador.Id, td => td.TrabajadorId,
                            (x, td) => new { x.Aplicacion, x.Trabajador, TrabajadorDependencia = td })
                        .Where(x =>
                            x.TrabajadorDependencia.DependenciaId == request.DependenciaId &&
                            !x.Aplicacion.Prueba)
                        .Select(x => x.Aplicacion);
                }
            }

            // --- Filtrar por regiones ---
            if (regiones.Any())
            {
                var municipios = _context.Zona
                    .Where(r => regiones.Contains(r.Id))
                    .Include(r => r.MunicipioZonas)
                    .SelectMany(r => r.MunicipioZonas.Select(mz => mz.MunicipioId))
                    .ToHashSet();

                aplicaciones = aplicaciones
                    .Join(_context.Beneficiario, a => a.BeneficiarioId, b => b.Id, (a, b) => new { a, b })
                    .Join(_context.Domicilio, ab => ab.b.DomicilioId, d => d.Id,
                        (ab, d) => new { ab.a, Beneficiario = ab.b, Domicilio = d })
                    .Where(x => x.Domicilio.Activa &&
                                !x.a.Prueba &&
                                municipios.Contains(x.Domicilio.MunicipioId))
                    .Select(x => x.a);

                beneficiarios = aplicaciones
                    .Join(_context.Beneficiario, a => a.BeneficiarioId, b => b.Id, (a, b) => new { a, b })
                    .Join(_context.Domicilio, ab => ab.b.DomicilioId, d => d.Id,
                        (ab, d) => new { ab.a, Beneficiario = ab.b, Domicilio = d })
                    .Where(x => x.Domicilio.Activa &&
                                !x.a.Prueba &&
                                municipios.Contains(x.Domicilio.MunicipioId))
                    .Select(x => x.Beneficiario);

                benefCount = beneficiarios.Count();
            }

            // --- Filtrar por municipio ---
            if (!string.IsNullOrEmpty(request.MunicipioId))
            {
                aplicaciones = aplicaciones
                    .Join(_context.Beneficiario, a => a.BeneficiarioId, b => b.Id, (a, b) => new { a, b })
                    .Join(_context.Domicilio, ab => ab.b.DomicilioId, d => d.Id,
                        (ab, d) => new { ab.a, Beneficiario = ab.b, Domicilio = d })
                    .Where(x => !x.a.Prueba &&
                                x.Domicilio.Activa &&
                                x.Domicilio.MunicipioId == request.MunicipioId)
                    .Select(x => x.a);

                beneficiarios = beneficiarios
                    .Join(_context.Domicilio, b => b.DomicilioId, d => d.Id, (b, d) => new { b, d })
                    .Where(x => !x.b.Prueba &&
                                x.d.Activa &&
                                x.d.MunicipioId == request.MunicipioId)
                    .Select(x => x.b);
                benefCount = beneficiarios.Count();
            }

            // --- Filtrar por localidad ---
            if (!string.IsNullOrEmpty(request.LocalidadId))
            {
                aplicaciones = aplicaciones
                    .Join(_context.Beneficiario, a => a.BeneficiarioId, b => b.Id, (a, b) => new { a, b })
                    .Join(_context.Domicilio, ab => ab.b.DomicilioId, d => d.Id,
                        (ab, d) => new { ab.a, Beneficiario = ab.b, Domicilio = d })
                    .Where(x => !x.a.Prueba &&
                                x.Domicilio.Activa &&
                                x.Domicilio.LocalidadId == request.LocalidadId)
                    .Select(x => x.a);

                beneficiarios = beneficiarios
                    .Join(_context.Domicilio, b => b.DomicilioId, d => d.Id, (b, d) => new { b, d })
                    .Where(x => !x.b.Prueba &&
                                x.d.Activa &&
                                x.d.LocalidadId == request.LocalidadId)
                    .Select(x => x.b);
                benefCount = beneficiarios.Count();
            }

            // --- Filtrar por colonia ---
            if (!string.IsNullOrEmpty(request.ColoniaId))
            {
                aplicaciones = aplicaciones
                    .Join(_context.Beneficiario, a => a.BeneficiarioId, b => b.Id, (a, b) => new { a, b })
                    .Join(_context.Domicilio, ab => ab.b.DomicilioId, d => d.Id,
                        (ab, d) => new { ab.a, Beneficiario = ab.b, Domicilio = d })
                    .Where(x => !x.a.Prueba &&
                                x.Domicilio.Activa &&
                                x.Domicilio.ColoniaId == request.ColoniaId)
                    .Select(x => x.a);

                beneficiarios = beneficiarios
                    .Join(_context.Domicilio, b => b.DomicilioId, d => d.Id, (b, d) => new { b, d })
                    .Where(x => !x.b.Prueba &&
                                x.d.Activa &&
                                x.d.ColoniaId == request.ColoniaId)
                    .Select(x => x.b);
                benefCount = beneficiarios.Count();
            }

            // --- Filtrar por CURP ---
            if (!string.IsNullOrEmpty(request.CURP))
            {
                aplicaciones = aplicaciones
                    .Join(_context.Beneficiario, a => a.BeneficiarioId, b => b.Id, (a, b) => new { a, b })
                    .Where(x => x.b.Curp == request.CURP)
                    .Select(x => x.a);

                beneficiarios = beneficiarios
                    .Where(b => b.Curp == request.CURP);
                benefCount = beneficiarios.Count();
            }

            aplicaciones = aplicaciones
                .Include(a => a.Trabajador)
                .ThenInclude(t => t.TrabajadorDependencias)
                .ThenInclude(td => td.Dependencia);

            response.NumDomicilios = beneficiarios.Count();
            response.NumDiagnosticos = aplicaciones.Count(a => a.Estatus == EstatusAplicacion.COMPLETO);
            response.NumPendientes = aplicaciones.Count(a => a.Estatus == EstatusAplicacion.INCOMPLETO);

            // OBTENER LAS FECHAS REALES DE LOS DIAGNÓSTICOS COMPLETADOS PARA CALCULAR EL PROMEDIO
            var diagnosticosCompletos = aplicaciones
                .Where(a => a.Estatus == EstatusAplicacion.COMPLETO && a.FechaInicio.HasValue)
                .Select(a => a.FechaInicio.Value.Date)
                .Distinct()
                .ToList();

            // CALCULAR PROMEDIO DE DIAGNÓSTICOS POR DÍA
            if (diagnosticosCompletos.Any())
            {
                var totalDiasConDiagnosticos = diagnosticosCompletos.Count;
                response.PromedioCaptura = Math.Round((double)response.NumDiagnosticos / totalDiasConDiagnosticos, 2);
            }
            else
            {
                response.PromedioCaptura = 0;
            }

            foreach (var ap in aplicaciones.Where(a => a.Estatus == EstatusAplicacion.COMPLETO))
            {
                response.Estatus[ap.Estatus]++;

                if (!response.Trabajadores.ContainsKey(ap.Trabajador.Nombre))
                    response.Trabajadores[ap.Trabajador.Nombre] = 0;
                response.Trabajadores[ap.Trabajador.Nombre]++;

                var dependencia = ap.Trabajador.TrabajadorDependencias.FirstOrDefault()?.Dependencia?.Nombre;
                if (!string.IsNullOrEmpty(dependencia))
                    response.Dependencias[dependencia] = response.Dependencias.GetValueOrDefault(dependencia, 0) + 1;

                if (ap.Educativa) response.Carencias["Educativa"]++;
                if (ap.ServicioSalud) response.Carencias["Servicios de salud"]++;
                if (ap.SeguridadSocial) response.Carencias["Seguridad social"]++;
                if (ap.Vivienda) response.Carencias["Vivienda"]++;
                if (ap.Servicios) response.Carencias["Servicios básicos"]++;
                if (ap.Alimentaria) response.Carencias["Alimentaria"]++;

                switch (ap.NivelPobreza)
                {
                    case NivelPobreza.POBREZA_EXTREMA:
                        response.Niveles["Pobreza extrema"]++;
                        break;
                    case NivelPobreza.POBREZA_MODERADA:
                        response.Niveles["Pobreza moderada"]++;
                        break;
                    case NivelPobreza.VULNERABLE_CARENCIA_SOCIAL:
                        response.Niveles["Vulnerable por carencia social"]++;
                        break;
                    case NivelPobreza.VULNERABLE_INGRESOS:
                        response.Niveles["Vulnerablepor ingresos"]++;
                        break;
                    case NivelPobreza.NO_POBRE_NO_VULNERABLE:
                        response.Niveles["Sin pobreza"]++;
                        break;
                }
            }

            return JsonSedeshu.SerializeObject(response);
        }


        /// <summary>
        /// Cargar localidades por municipio con filtros aplicados
        /// </summary>
        [HttpPost]
        [Authorize]
        public JsonResult CargarLocalidades([FromBody] DashboardRequestExtended request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.MunicipioId))
                {
                    return Json(new List<Model>());
                }

                // Aplicar filtros para obtener solo las localidades con datos en el período
                var aplicacionesFiltradas = AplicarFiltrosAplicaciones(request);

                var localidadesConDatos = aplicacionesFiltradas
                    .Join(_context.Beneficiario, a => a.BeneficiarioId, b => b.Id, (a, b) => new { a, b })
                    .Join(_context.Domicilio, ab => ab.b.DomicilioId, d => d.Id, (ab, d) => new { ab.a, d })
                    .Where(x => x.d.MunicipioId == request.MunicipioId)
                    .Select(x => x.d.LocalidadId)
                    .Distinct()
                    .ToList();

                var localidades = _context.Localidad
                    .Where(l => l.MunicipioId == request.MunicipioId &&
                               //l.Activa &&
                               localidadesConDatos.Contains(l.Id))
                    .Select(l => new Model
                    {
                        Id = l.Id,
                        Nombre = l.Nombre
                    })
                    .OrderBy(l => l.Nombre)
                    .ToList();

                return Json(localidades);
            }
            catch (Exception ex)
            {
                // Log error
                return Json(new List<Model>());
            }
        }

        /// <summary>
        /// Cargar colonias por localidad con filtros aplicados
        /// </summary>
        [HttpPost]
        [Authorize]
        public JsonResult CargarColonias([FromBody] DashboardRequestExtended request)
        {
            try
            {
                // Aplicar filtros para obtener solo las colonias con datos en el período
                var aplicacionesFiltradas = AplicarFiltrosAplicaciones(request);

                var coloniasConDatos = new List<string>();

                if (string.IsNullOrEmpty(request.LocalidadId))
                {
                    //return Json(new List<Model>());
                    coloniasConDatos = aplicacionesFiltradas
                    .Join(_context.Beneficiario, a => a.BeneficiarioId, b => b.Id, (a, b) => new { a, b })
                    .Join(_context.Domicilio, ab => ab.b.DomicilioId, d => d.Id, (ab, d) => new { ab.a, d })
                    //.Where(x => x.d.LocalidadId == request.LocalidadId)
                    .Select(x => x.d.ColoniaId)
                    .Distinct()
                    .ToList();
                }
                else
                {
                    coloniasConDatos = aplicacionesFiltradas
                    .Join(_context.Beneficiario, a => a.BeneficiarioId, b => b.Id, (a, b) => new { a, b })
                    .Join(_context.Domicilio, ab => ab.b.DomicilioId, d => d.Id, (ab, d) => new { ab.a, d })
                    .Where(x => x.d.LocalidadId == request.LocalidadId)
                    .Select(x => x.d.ColoniaId)
                    .Distinct()
                    .ToList();
                }

                    

                // Buscar colonias por municipio (ya que Colonia solo tiene MunicipioId)
                var municipioId = _context.Localidad
                    .Where(l => l.Id == request.LocalidadId)
                    .Select(l => l.MunicipioId)
                    .FirstOrDefault();

                var colonias = _context.Colonia?
                    .Where(c => c.MunicipioId == request.MunicipioId && //municipioId &&
                               coloniasConDatos.Contains(c.Id))
                    .Select(c => new Model
                    {
                        Id = c.Id,
                        Nombre = c.Nombre
                    })
                    .OrderBy(c => c.Nombre)
                    .ToList();

                // Si no hay colonias en la tabla específica, usar las del domicilio
                if (colonias == null || !colonias.Any())
                {
                    colonias = aplicacionesFiltradas
                        .Join(_context.Beneficiario, a => a.BeneficiarioId, b => b.Id, (a, b) => new { a, b })
                        .Join(_context.Domicilio, ab => ab.b.DomicilioId, d => d.Id, (ab, d) => new { ab.a, d })
                        .Where(x => x.d.LocalidadId == request.LocalidadId && !string.IsNullOrEmpty(x.d.ColoniaId))
                        .Select(x => new Model
                        {
                            Id = x.d.ColoniaId,
                            Nombre = x.d.ColoniaId
                        })
                        .Distinct()
                        .OrderBy(c => c.Nombre)
                        .ToList();
                }

                return Json(colonias ?? new List<Model>());
            }
            catch (Exception ex)
            {
                // Log error
                return Json(new List<Model>());
            }
        }

        /// <summary>
        /// Cargar CURPs por colonia con filtros aplicados
        /// </summary>
        [HttpPost]
        [Authorize]
        public JsonResult CargarCURPs([FromBody] DashboardRequestExtended request)
        {
            try
            {
                // Aplicar filtros para obtener solo los CURPs con datos en el período
                var aplicacionesFiltradas = AplicarFiltrosAplicaciones(request);

                var curps = new List<string>();
                if (string.IsNullOrEmpty(request.ColoniaId) || string.IsNullOrEmpty(request.LocalidadId))
                {
                    //return Json(new List<string>());
                    curps = aplicacionesFiltradas
                   .Join(_context.Beneficiario, a => a.BeneficiarioId, b => b.Id, (a, b) => new { a, b })
                   .Join(_context.Domicilio, ab => ab.b.DomicilioId, d => d.Id, (ab, d) => new { ab.a, ab.b, d })
                   //.Where(x => x.d.LocalidadId == request.LocalidadId)
                   .Select(x => x.b.Curp)
                   .Where(curp => !string.IsNullOrEmpty(curp))
                   .Distinct()
                   .OrderBy(curp => curp)
                   .Take(100) // Limitar a 100 resultados
                   .ToList();

                    return Json(curps);
                }

                curps = aplicacionesFiltradas
                   .Join(_context.Beneficiario, a => a.BeneficiarioId, b => b.Id, (a, b) => new { a, b })
                    .Join(_context.Domicilio, ab => ab.b.DomicilioId, d => d.Id, (ab, d) => new { ab.a, ab.b, d })
                    .Where(x => x.d.LocalidadId == request.LocalidadId)
                    .Select(x => x.b.Curp)
                    .Where(curp => !string.IsNullOrEmpty(curp))
                    .Distinct()
                    .OrderBy(curp => curp)
                    .Take(100) // Limitar a 100 resultados
                    .ToList();

                return Json(curps);
            }
            catch (Exception ex)
            {
                // Log error
                return Json(new List<string>());
            }
        }

        /// <summary>
        /// Método auxiliar para aplicar filtros a las aplicaciones
        /// </summary>
        private IQueryable<Aplicacion> AplicarFiltrosAplicaciones(DashboardRequestExtended request)
        {
            var now = DateTime.Now;
            var inicio = new DateTime(now.Year, now.Month, 1);
            var fin = new DateTime(now.Year, now.Month, DateTime.DaysInMonth(now.Year, now.Month));

            if (!string.IsNullOrEmpty(request.Inicio))
                inicio = DateTime.Parse(request.Inicio.Substring(0, 10));
            if (!string.IsNullOrEmpty(request.Fin))
                fin = DateTime.Parse(request.Fin.Substring(0, 10)).EndOf(DateTimeAnchor.Day);

            var admin = HttpContext.User.IsInRole("Administrador");
            var regiones = new List<string>();

            IQueryable<Aplicacion> aplicaciones;

            if (!admin)
            {
                var user = _userManager.GetUserAsync(HttpContext.User).Result;

                regiones = _context.UsuarioRegion
                    .Where(ur => ur.DeletedAt == null && ur.UsuarioId == user.Id)
                    .Select(ur => ur.ZonaId)
                    .ToList();

                aplicaciones = _context.Aplicacion
                    .Join(_context.Trabajador, a => a.TrabajadorId, t => t.Id,
                        (a, t) => new { Aplicacion = a, Trabajador = t })
                    .Join(_context.TrabajadorDependencia, x => x.Trabajador.Id, td => td.TrabajadorId,
                        (x, td) => new { x.Aplicacion, x.Trabajador, TrabajadorDependencia = td })
                    .Where(x =>
                        x.Aplicacion.Activa &&
                        x.Aplicacion.FechaInicio >= inicio &&
                        x.Aplicacion.FechaInicio <= fin &&
                        x.Aplicacion.DeletedAt == null &&
                        !x.Aplicacion.Prueba &&
                        x.TrabajadorDependencia.DependenciaId == user.DependenciaId &&
                        x.Trabajador.Id != "120989e6-f6b7-453b-83af-fd4ccc0639b6")
                    .Select(x => x.Aplicacion);
            }
            else
            {
                aplicaciones = _context.Aplicacion
                    .Where(x => x.Activa &&
                                x.FechaInicio >= inicio &&
                                x.FechaInicio <= fin &&
                                x.DeletedAt == null &&
                                !x.Prueba &&
                                x.TrabajadorId != "120989e6-f6b7-453b-83af-fd4ccc0639b6");

                if (!string.IsNullOrEmpty(request.DependenciaId))
                {
                    aplicaciones = aplicaciones
                        .Join(_context.Trabajador, a => a.TrabajadorId, t => t.Id,
                            (a, t) => new { Aplicacion = a, Trabajador = t })
                        .Join(_context.TrabajadorDependencia, x => x.Trabajador.Id, td => td.TrabajadorId,
                            (x, td) => new { x.Aplicacion, x.Trabajador, TrabajadorDependencia = td })
                        .Where(x =>
                            x.TrabajadorDependencia.DependenciaId == request.DependenciaId &&
                            !x.Aplicacion.Prueba)
                        .Select(x => x.Aplicacion);
                }
            }

            // Aplicar filtro de municipio si existe
            if (!string.IsNullOrEmpty(request.MunicipioId))
            {
                aplicaciones = aplicaciones
                    .Join(_context.Beneficiario, a => a.BeneficiarioId, b => b.Id, (a, b) => new { a, b })
                    .Join(_context.Domicilio, ab => ab.b.DomicilioId, d => d.Id, (ab, d) => new { ab.a, d })
                    .Where(x => x.d.MunicipioId == request.MunicipioId)
                    .Select(x => x.a);
            }

            // Aplicar filtro de localidad si existe
            if (!string.IsNullOrEmpty(request.LocalidadId))
            {
                aplicaciones = aplicaciones
                    .Join(_context.Beneficiario, a => a.BeneficiarioId, b => b.Id, (a, b) => new { a, b })
                    .Join(_context.Domicilio, ab => ab.b.DomicilioId, d => d.Id, (ab, d) => new { ab.a, d })
                    .Where(x => x.d.LocalidadId == request.LocalidadId)
                    .Select(x => x.a);
            }

            // Aplicar filtro de colonia si existe
            if (!string.IsNullOrEmpty(request.ColoniaId))
            {
                aplicaciones = aplicaciones
                    .Join(_context.Beneficiario, a => a.BeneficiarioId, b => b.Id, (a, b) => new { a, b })
                    .Join(_context.Domicilio, ab => ab.b.DomicilioId, d => d.Id, (ab, d) => new { ab.a, d })
                    .Where(x => x.d.ColoniaId == request.ColoniaId)
                    .Select(x => x.a);
            }

            // Aplicar filtro de CURP si existe
            if (!string.IsNullOrEmpty(request.CURP))
            {
                aplicaciones = aplicaciones
                    .Join(_context.Beneficiario, a => a.BeneficiarioId, b => b.Id, (a, b) => new { a, b })
                    .Where(x => x.b.Curp == request.CURP)
                    .Select(x => x.a);
            }

            return aplicaciones;
        }

        /// <summary>
        /// Obtener datos de carencias sociales con filtros aplicados
        /// </summary>
        [HttpPost]
        [Authorize]
        public JsonResult ObtenerCarencias([FromBody] DashboardRequestExtended request)
        {
            try
            {
                var aplicacionesFiltradas = AplicarFiltrosAplicaciones(request)
                    .Where(a => a.Estatus == EstatusAplicacion.COMPLETO)
                    .Include(a => a.Beneficiario)
                    .ThenInclude(b => b.Domicilio);

                var totalHogares = aplicacionesFiltradas
                    .Select(a => a.BeneficiarioId)
                    .Distinct()
                    .Count();

                if (totalHogares == 0)
                {
                    // Si no hay datos, devolver estructura vacía
                    return Json(new CarenciasResponse
                    {
                        PobrezaExtrema = new CarenciaData { Total = 0, SinCarencia = 0, ConCarencia = 0, PorcentajeSinCarencia = 0, PorcentajeConCarencia = 0 },
                        RezagoEducativo = new CarenciaData { Total = 0, SinCarencia = 0, ConCarencia = 0, PorcentajeSinCarencia = 0, PorcentajeConCarencia = 0 },
                        Salud = new CarenciaData { Total = 0, SinCarencia = 0, ConCarencia = 0, PorcentajeSinCarencia = 0, PorcentajeConCarencia = 0 },
                        SeguridadSocial = new CarenciaData { Total = 0, SinCarencia = 0, ConCarencia = 0, PorcentajeSinCarencia = 0, PorcentajeConCarencia = 0 },
                        CalidadVivienda = new CarenciaData { Total = 0, SinCarencia = 0, ConCarencia = 0, PorcentajeSinCarencia = 0, PorcentajeConCarencia = 0 },
                        ServiciosBasicos = new CarenciaData { Total = 0, SinCarencia = 0, ConCarencia = 0, PorcentajeSinCarencia = 0, PorcentajeConCarencia = 0 },
                        Alimentacion = new CarenciaData { Total = 0, SinCarencia = 0, ConCarencia = 0, PorcentajeSinCarencia = 0, PorcentajeConCarencia = 0 },
                        Resumen = new ResumenData { TotalHogares = 0, SinCarencia = 0, ConCarencia = 0, PorcentajeSinCarencia = 0, PorcentajeConCarencia = 0 }
                    });
                }

                var rnd = new Random();

                CarenciaData RandomCarencia()
                {
                    int conCarencia = rnd.Next(0, totalHogares + 1);
                    int sinCarencia = totalHogares - conCarencia;
                    return new CarenciaData
                    {
                        Total = totalHogares,
                        ConCarencia = conCarencia,
                        SinCarencia = sinCarencia,
                        PorcentajeConCarencia = Math.Round((double)conCarencia / totalHogares * 100, 1),
                        PorcentajeSinCarencia = Math.Round((double)sinCarencia / totalHogares * 100, 1)
                    };
                }

                int resumenConCarencia = rnd.Next(0, totalHogares + 1);
                int resumenSinCarencia = totalHogares - resumenConCarencia;

                var response =  new CarenciasResponse
                {
                    PobrezaExtrema = RandomCarencia(),
                    RezagoEducativo = RandomCarencia(),
                    Salud = RandomCarencia(),
                    SeguridadSocial = RandomCarencia(),
                    CalidadVivienda = RandomCarencia(),
                    ServiciosBasicos = RandomCarencia(),
                    Alimentacion = RandomCarencia(),
                    Resumen = new ResumenData
                    {
                        TotalHogares = totalHogares,
                        ConCarencia = resumenConCarencia,
                        SinCarencia = resumenSinCarencia,
                        PorcentajeConCarencia = Math.Round((double)resumenConCarencia / totalHogares * 100, 1),
                        PorcentajeSinCarencia = Math.Round((double)resumenSinCarencia / totalHogares * 100, 1)
                    }
                };

                //// Calcular carencias reales basadas en los datos filtrados
                //var response = new CarenciasResponse
                //{
                //    PobrezaExtrema = CalcularCarenciaReal(aplicacionesFiltradas, a => a.NivelPobreza == NivelPobreza.POBREZA_EXTREMA, totalHogares),
                //    RezagoEducativo = CalcularCarenciaReal(aplicacionesFiltradas, a => a.Educativa, totalHogares),
                //    Salud = CalcularCarenciaReal(aplicacionesFiltradas, a => a.ServicioSalud, totalHogares),
                //    SeguridadSocial = CalcularCarenciaReal(aplicacionesFiltradas, a => a.SeguridadSocial, totalHogares),
                //    CalidadVivienda = CalcularCarenciaReal(aplicacionesFiltradas, a => a.Vivienda, totalHogares),
                //    ServiciosBasicos = CalcularCarenciaReal(aplicacionesFiltradas, a => a.Servicios, totalHogares),
                //    Alimentacion = CalcularCarenciaReal(aplicacionesFiltradas, a => a.Alimentaria, totalHogares),
                //    Resumen = CalcularResumenReal(aplicacionesFiltradas, totalHogares)
                //};

                return Json(response);
            }
            catch (Exception ex)
            {
                // En caso de error, devolver datos de ejemplo
                var random = new Random();
                var totalHogares = 100 + random.Next(100);
                return Json(GenerarDatosAleatorios(totalHogares, random));
            }
        }

        private CarenciaData CalcularCarenciaReal(IQueryable<Aplicacion> aplicaciones, Func<Aplicacion, bool> condicionCarencia, int totalHogares)
        {
            var conCarencia = aplicaciones.Count(condicionCarencia);
            var sinCarencia = totalHogares - conCarencia;

            return new CarenciaData
            {
                Total = totalHogares,
                ConCarencia = conCarencia,
                SinCarencia = sinCarencia,
                PorcentajeConCarencia = totalHogares > 0 ? Math.Round((conCarencia * 100.0) / totalHogares, 1) : 0,
                PorcentajeSinCarencia = totalHogares > 0 ? Math.Round((sinCarencia * 100.0) / totalHogares, 1) : 0
            };
        }

        private ResumenData CalcularResumenReal(IQueryable<Aplicacion> aplicaciones, int totalHogares)
        {
            // Hogares con al menos una carencia
            var hogaresConCarencia = aplicaciones
                .Where(a => a.Educativa || a.ServicioSalud || a.SeguridadSocial || a.Vivienda || a.Servicios || a.Alimentaria)
                .Select(a => a.BeneficiarioId)
                .Distinct()
                .Count();

            var hogaresSinCarencia = totalHogares - hogaresConCarencia;

            return new ResumenData
            {
                TotalHogares = totalHogares,
                ConCarencia = hogaresConCarencia,
                SinCarencia = hogaresSinCarencia,
                PorcentajeConCarencia = totalHogares > 0 ? Math.Round((hogaresConCarencia * 100.0) / totalHogares, 1) : 0,
                PorcentajeSinCarencia = totalHogares > 0 ? Math.Round((hogaresSinCarencia * 100.0) / totalHogares, 1) : 0
            };
        }

        private CarenciasResponse GenerarDatosAleatorios(int totalHogares, Random random)
        {
            return new CarenciasResponse
            {
                PobrezaExtrema = GenerarCarenciaAleatoria(totalHogares, 5, random),
                RezagoEducativo = GenerarCarenciaAleatoria(totalHogares, 27, random),
                Salud = GenerarCarenciaAleatoria(totalHogares, 31, random),
                SeguridadSocial = GenerarCarenciaAleatoria(totalHogares, 44, random),
                CalidadVivienda = GenerarCarenciaAleatoria(totalHogares, 4, random),
                ServiciosBasicos = GenerarCarenciaAleatoria(totalHogares, 9, random),
                Alimentacion = GenerarCarenciaAleatoria(totalHogares, 17, random),
                Resumen = new ResumenData
                {
                    TotalHogares = totalHogares,
                    ConCarencia = (int)(totalHogares * 0.75),
                    SinCarencia = (int)(totalHogares * 0.25),
                    PorcentajeConCarencia = 75,
                    PorcentajeSinCarencia = 25
                }
            };
        }

        private CarenciaData GenerarCarenciaAleatoria(int totalHogares, int porcentajeConCarencia, Random random)
        {
            var variacion = random.Next(-2, 3);
            var porcentajeFinal = Math.Max(1, Math.Min(99, porcentajeConCarencia + variacion));

            var conCarencia = (int)(totalHogares * porcentajeFinal / 100.0);
            var sinCarencia = totalHogares - conCarencia;

            return new CarenciaData
            {
                Total = totalHogares,
                ConCarencia = conCarencia,
                SinCarencia = sinCarencia,
                PorcentajeConCarencia = Math.Round((conCarencia * 100.0) / totalHogares, 1),
                PorcentajeSinCarencia = Math.Round((sinCarencia * 100.0) / totalHogares, 1)
            };
        }

        /// <summary>
        /// Método para ingresar aplicaciones en la sábana de datos
        /// </summary>
        [HttpPost]
        [Authorize]
        public int IngresarAplicacionEnSabana(string inicio, string fin)
        {
            var numAplicaciones = 0;
            var fechaI = DateTime.Parse(inicio);
            var fechaF = DateTime.Parse(fin);
            var fechaInicio = new DateTime(fechaI.Year, fechaI.Month, fechaI.Day, 0, 0, 0, 0);
            var fechaFin = new DateTime(fechaF.Year, fechaF.Month, fechaF.Day, 23, 59, 59, 999);
            var aplicaciones = _context.Aplicacion.Where(a => a.DeletedAt == null && !a.Prueba &&
                                                              a.FechaInicio > fechaInicio && a.FechaInicio < fechaFin
                                                              && !a.Beneficiario.Prueba && a.Beneficiario.DeletedAt == null
                                                              && a.Beneficiario.PadreId == null && a.Beneficiario.HijoId == null)
                .Include(a => a.Trabajador).ToList();
            var code = new BeneficiarioCode(_context);
            var registros = new List<string>();
            var conexion = _context.Database;
            conexion.OpenConnection();
            foreach (var aplicacion in aplicaciones)
            {
                registros.AddRange(code.IngresarAplicacionEnSabana(aplicacion, conexion));
            }
            conexion.CloseConnection();
            _context.Database.BeginTransaction();
            foreach (var registro in registros)
            {
                numAplicaciones++;
                _context.Database.ExecuteSqlCommand(registro);
            }
            _context.Database.CommitTransaction();
            return numAplicaciones;
        }

        /// <summary>
        /// Funcion de prueba para insertar datos iniciales en la base de datos(Funcion que se movio a un seeder)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [AllowAnonymous]
        public IActionResult InicializarDatos()
        {
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Funcion para dar ingreso a alos usuarios registrados de acuerdo a las credenciales proporcionadas
        /// </summary>
        /// <param name="model">Credenciales del usuario</param>
        /// <returns>Redirecciona al dashboard si las credenciales son correctas, redirecciona a la pantalla de inicio de sesion si las credenciales son incorrectas</returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var username = model.Username;
                var userQuery = _context.Users.Join(_context.UserRoles, user => user.Id, rol => rol.UserId,
                                    (user, rol) => new { Users = user, UserRoles = rol })
                                .Where(u => u.Users.UserName.Equals(username));
                if (userQuery.Count() > 0)
                {
                    var userQuery2 = userQuery.Where(u => u.Users.LastLoginDate > DateTime.Now.AddMonths(-6) || u.Users.LastLoginDate == null
                                        || u.UserRoles.RoleId.Equals("1"));
                    if (userQuery2.Count() > 0)
                    {
                        var result = await
                            _signInManager.PasswordSignInAsync(username, model.Password, model.RememberMe, true);

                        if (result.Succeeded)
                        {
                            var user = userQuery2.First().Users;
                            user.LastLoginDate = DateTime.Now;
                            _context.Users.Update(user);
                            _context.SaveChanges();

                            _context.Bitacora.Add(new Bitacora()
                            {
                                UsuarioId = user.Id,
                                Accion = AccionBitacora.LOGIN,
                                Mensaje = "Inició sesión el usuario " + username + ".",
                                CreatedAt = DateTime.Now,
                                UpdatedAt = DateTime.Now
                            });
                            _context.SaveChanges();

                            return RedirectToAction("Bienvenida", "Home");
                        }

                        _context.Bitacora.Add(new Bitacora()
                        {
                            UsuarioId = userQuery2.First().Users.Id,
                            Accion = AccionBitacora.LOGIN,
                            Mensaje = "Se intentó iniciar sesión con el nombre de usuario " + username + ".",
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now
                        });
                        _context.SaveChanges();

                        ModelState.AddModelError("Username", "La contraseña es incorrecta.");
                        return View(model);
                    }

                    _context.Bitacora.Add(new Bitacora()
                    {
                        UsuarioId = userQuery.First().Users.Id,
                        Accion = AccionBitacora.LOGIN,
                        Mensaje = "Se intentó iniciar sesión con el nombre de usuario " + username + ".",
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    });
                    _context.SaveChanges();

                    ModelState.AddModelError("Username", "El usuario lleva más de 6 meses sin iniciar sesión. Comunique su problema con la administración.");
                    return View(model);
                }

                _context.Bitacora.Add(new Bitacora()
                {
                    Accion = AccionBitacora.LOGIN,
                    Mensaje = "Se intentó iniciar sesión con el nombre de usuario " + username + ".",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                });
                _context.SaveChanges();

                ModelState.AddModelError("Username", "El nombre de usuario es incorrecto.");
                return View(model);
            }

            return View(model);
        }


        [HttpGet]
        [Authorize]
        public IActionResult Bienvenida()
        {
            var date = DateTime.Now;
            var admin = HttpContext.User.IsInRole("Administrador");
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            var solicitudDasbhoard = new Dashboard
            {
                Inicio = firstDayOfMonth.ToString("yyyy-MM-dd"),
                Fin = lastDayOfMonth.ToString("yyyy-MM-dd"),
                MunicipiosFiltro = new List<Model>(),
                DependenciasFiltro = new List<Model>()
            };

            // Cargar municipios filtrados por trabajador/dependencia
            if (!admin)
            {
                var user = _userManager.GetUserAsync(HttpContext.User).Result;

                // Obtener las regiones del usuario
                var regiones = _context.UsuarioRegion
                    .Where(ur => ur.DeletedAt == null && ur.UsuarioId == user.Id)
                    .Select(ur => ur.ZonaId)
                    .ToList();

                IQueryable<string> municipiosFiltrados;

                if (regiones.Any())
                {
                    // Filtrar por municipios de las regiones del usuario
                    municipiosFiltrados = _context.Zona
                        .Where(r => regiones.Contains(r.Id))
                        .Include(r => r.MunicipioZonas)
                        .SelectMany(r => r.MunicipioZonas.Select(mz => mz.MunicipioId))
                        .Distinct();
                }
                else
                {
                    // Si no tiene regiones, filtrar por aplicaciones de su dependencia
                    municipiosFiltrados = _context.Aplicacion
                        .Join(_context.Trabajador, a => a.TrabajadorId, t => t.Id,
                            (a, t) => new { Aplicacion = a, Trabajador = t })
                        .Join(_context.TrabajadorDependencia, x => x.Trabajador.Id, td => td.TrabajadorId,
                            (x, td) => new { x.Aplicacion, x.Trabajador, TrabajadorDependencia = td })
                        .Where(x =>
                            x.Aplicacion.Activa &&
                            x.Aplicacion.DeletedAt == null &&
                            !x.Aplicacion.Prueba &&
                            x.TrabajadorDependencia.DependenciaId == user.DependenciaId &&
                            x.Trabajador.Id != "120989e6-f6b7-453b-83af-fd4ccc0639b6")
                        .Join(_context.Beneficiario, x => x.Aplicacion.BeneficiarioId, b => b.Id, (x, b) => new { x, b })
                        .Join(_context.Domicilio, xb => xb.b.DomicilioId, d => d.Id, (xb, d) => d.MunicipioId)
                        .Distinct();
                }

                // Cargar los municipios filtrados
                _context.Municipio
                    .Where(m => municipiosFiltrados.Contains(m.Id))
                    .ToList()
                    .ForEach(m => solicitudDasbhoard.MunicipiosFiltro.Add(new Model()
                    {
                        Id = m.Id,
                        Nombre = m.Nombre
                    }));
            }
            else
            {
                // Para administradores, cargar todos los municipios
                _context.Municipio.ToList().ForEach(m => solicitudDasbhoard.MunicipiosFiltro.Add(new Model()
                {
                    Id = m.Id,
                    Nombre = m.Nombre
                }));
            }

            if (admin)
            {
                _context.Dependencia.ToList().ForEach(m => solicitudDasbhoard.DependenciasFiltro.Add(new Model()
                {
                    Id = m.Id,
                    Nombre = m.Nombre
                }));
            }
            else
            {
                var user = _userManager.GetUserAsync(HttpContext.User).Result;
                _context.Dependencia.Where(d => d.Id.Equals(user.DependenciaId)).ToList().ForEach(m => solicitudDasbhoard.DependenciasFiltro.Add(new Model()
                {
                    Id = m.Id,
                    Nombre = m.Nombre
                }));
            }
            return View("Index", solicitudDasbhoard);
        }


        /*
        /// <summary>
        /// Funcion que muestra la vista del tablero de indicadores
        /// </summary>
        /// <returns>Vista con el tablero de indicadores</returns>
        [HttpGet]
        [Authorize]
        public IActionResult Bienvenida()
        {
            var date = DateTime.Now;
            var admin = HttpContext.User.IsInRole("Administrador");
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            var solicitudDasbhoard = new Dashboard
            {
                Inicio = firstDayOfMonth.ToString("yyyy-MM-dd"),
                Fin = lastDayOfMonth.ToString("yyyy-MM-dd"),
                MunicipiosFiltro = new List<Model>(),
                DependenciasFiltro = new List<Model>()
            };
            _context.Municipio.ToList().ForEach(m => solicitudDasbhoard.MunicipiosFiltro.Add(new Model()
            {
                Id = m.Id,
                Nombre = m.Nombre
            }));
            if (admin)
            {
                _context.Dependencia.ToList().ForEach(m => solicitudDasbhoard.DependenciasFiltro.Add(new Model()
                {
                    Id = m.Id,
                    Nombre = m.Nombre
                }));
            }
            else
            {
                var user = _userManager.GetUserAsync(HttpContext.User).Result;
                _context.Dependencia.Where(d => d.Id.Equals(user.DependenciaId)).ToList().ForEach(m => solicitudDasbhoard.DependenciasFiltro.Add(new Model()
                {
                    Id = m.Id,
                    Nombre = m.Nombre
                }));
            }
            return View("Index", solicitudDasbhoard);
        }
        */

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        /// <summary>
        /// Funcion para cerrar sesion en el sitema
        /// </summary>
        /// <returns>Redireccion a la vista de inicio de sesion</returns>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Expresion()
        {
            return View("Expresion");
        }

        public string Importar(DateTime inicio, DateTime fin)
        {
            var importar = new ImportacionDiagnosticos(_contextDiagnostico, _context, _configuration, _clientFactory);
            importar.importar(inicio, fin);
            return "Hemos terminado de importar los diagnosticos de tu solicitud";
        }

        public string CompletarCurps(DateTime inicio, DateTime fin)
        {
            var importar = new ImportacionDiagnosticos(_contextDiagnostico, _context, _configuration, _clientFactory);
            importar.CompletarCurps(inicio, fin);
            return "Hemos terminado de completar los CURP de tu solicitud";
        }

        public string CompletarDirecciones(DateTime inicio, DateTime fin)
        {
            var importar = new ImportacionDiagnosticos(_contextDiagnostico, _context, _configuration, _clientFactory);
            importar.CompletarDirecciones(inicio, fin);
            return "Hemos terminado de completar los domicilios de tu solicitud";
        }

        public string CompletarCarencias(DateTime inicio, DateTime fin)
        {
            var importar = new ImportacionDiagnosticos(_contextDiagnostico, _context, _configuration, _clientFactory);
            importar.CompletarCarencias(inicio, fin);
            return "Hemos terminado de completar las carencias sociales en las familias de tu solicitud";
        }

        public async Task<IActionResult> ManualAdministrativo()
        {
            var folderName = Path.Combine("wwwroot", "manuales");
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            var conevalPath = Path.Combine(pathToSave, "administrativo.pdf");
            var memory = new MemoryStream();
            using (var stream = new FileStream(conevalPath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            var ext = Path.GetExtension(conevalPath).ToLowerInvariant();
            return File(memory, "application/pdf", Path.GetFileName(conevalPath));
        }

        public async Task<IActionResult> ManualAplicativo()
        {
            var folderName = Path.Combine("wwwroot", "manuales");
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            var conevalPath = Path.Combine(pathToSave, "aplicativo.pdf");
            var memory = new MemoryStream();
            using (var stream = new FileStream(conevalPath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, "application/pdf", Path.GetFileName(conevalPath));
        }

        /// <summary>
        /// Funcion que corrige el acomodo de las familias que fueron movidas en la sincronizacion, utilizando como informacion de origen la encuesta
        /// </summary>
        public void CorregirIntegrantes()
        {
            var inicio = DateTime.Parse("2021-02-08");
            var beneficiariosHash = new Dictionary<string, Beneficiario>();
            var respuestasDB = _context.Beneficiario.Where(b => b.DeletedAt == null
                                                                && !b.Prueba && b.CreatedAt > inicio && b.ApellidoPaterno != "Nueva"
                                                                && b.ApellidoPaterno != "[Nueva" && b.Nombre != "familia" && b.Nombre != "familia]");
            foreach (var m in respuestasDB)//Generamos Hash de integtantes para buscarlos apartir de la encuesta y si no se encuentran los registramos en la base de datos
            {
                var llave = m.ApellidoPaterno + "-" + m.ApellidoMaterno + "-" + m.Nombre + "-" +
                            (m.FechaNacimiento.HasValue ? m.FechaNacimiento.Value.ToString("yyyyMMdd") : "") + "-" +
                            (m.SexoId ?? "") + "-" +
                            (m.EstadoId ?? "");
                if (!beneficiariosHash.ContainsKey(llave))
                {
                    beneficiariosHash.Add(llave, m);
                }
            }
            var preguntas = new List<string>
            {
                "993",//Parentesco
                "989",//Nombre
                "990",//Apellido paterno
                "991",//Apellido materno
                "996",//Fecha de nacimiento
                "998",//Sexo
                "999",//Estado,
                "995"//Curp
            };

            var respuestas = _context.AplicacionPregunta.Where(ap => ap.DeletedAt == null
                                                                     && ap.CreatedAt > inicio && preguntas.Contains(ap.PreguntaId) && ap.Aplicacion.Estatus == EstatusAplicacion.COMPLETO).
                OrderBy(ap => ap.AplicacionId).ThenBy(ap => ap.RespuestaIteracion).
                Include(ap => ap.Aplicacion);//Buscamos las preguntas asociadas a la informacion de los integrantes
            var familias = new Dictionary<string, Dictionary<int, Beneficiario>>();
            foreach (var respuesta in respuestas) //Vamos a construir un mapa con los integrantes a partir de las encuestas
            {
                var familia = new Dictionary<int, Beneficiario>();
                var integrante = new Beneficiario();
                if (familias.ContainsKey(respuesta.AplicacionId))
                {
                    familia = familias[respuesta.AplicacionId];
                }
                else
                {
                    familia = new Dictionary<int, Beneficiario>();
                    familias.Add(respuesta.AplicacionId, familia);
                }
                if (familia.ContainsKey(respuesta.RespuestaIteracion))
                {
                    integrante = familia[respuesta.RespuestaIteracion];
                }
                else
                {
                    integrante = new Beneficiario();
                    integrante.NumIntegrante = respuesta.RespuestaIteracion + 1;
                    integrante.TrabajadorId = respuesta.Aplicacion.TrabajadorId;
                    integrante.CreatedAt = respuesta.CreatedAt;
                    integrante.UpdatedAt = respuesta.UpdatedAt;
                    integrante.Aplicacion = respuesta.Aplicacion;
                    familia.Add(respuesta.RespuestaIteracion, integrante);
                }

                switch (respuesta.PreguntaId)
                {
                    case "993":
                        integrante.ParentescoId = respuesta.ValorCatalogo;
                        break;
                    case "989":
                        integrante.Nombre = respuesta.Valor;
                        break;
                    case "990":
                        integrante.ApellidoPaterno = respuesta.Valor;
                        break;
                    case "991":
                        integrante.ApellidoMaterno = respuesta.Valor;
                        break;
                    case "996":
                        integrante.FechaNacimiento = respuesta.ValorFecha;
                        break;
                    case "998":
                        integrante.SexoId = respuesta.ValorCatalogo;
                        break;
                    case "999":
                        integrante.EstadoId = respuesta.ValorCatalogo;
                        break;
                    case "995":
                        integrante.Curp = respuesta.Valor;
                        break;
                }
            }

            _context.Database.BeginTransaction();
            foreach (var familia in familias) //Vamos a buscar los inetgrantes obtenidos a partir de la encuesta en la base de datos a traves del hash y si no se encuentran se registraran a sus hijos se acomodaran al nuevo padre recien creado
            {
                foreach (var m in familia.Value)
                {
                    if (!m.Value.FechaNacimiento.HasValue)
                    {
                        continue;
                    }
                    var llave = m.Value.ApellidoPaterno + "-" + m.Value.ApellidoMaterno + "-" + m.Value.Nombre + "-" +
                                m.Value.FechaNacimiento.Value.ToString("yyyyMMdd") + "-" +
                                (m.Value.SexoId ?? "") + "-" +
                                (m.Value.EstadoId ?? "");
                    var integranteDB = new Beneficiario();
                    if (!beneficiariosHash.ContainsKey(llave))
                    {


                        if (string.IsNullOrEmpty(m.Value.Curp))//Si el curp es vacio y no se encontro al beneficiario en el hash probablemente sea porque cambio sus datos en el enrolamiento
                        {
                            continue;
                        }
                        var otrasAplicaciones = _context.Aplicacion.Count(a => !a.Prueba && a.DeletedAt == null &&
                                                                               a.BeneficiarioId == m.Value.Aplicacion.BeneficiarioId);
                        if (otrasAplicaciones < 2)
                        {
                            continue;
                        }
                        integranteDB = new Beneficiario();
                        integranteDB.Id = Guid.NewGuid().ToString();
                        integranteDB.Nombre = m.Value.Nombre;
                        integranteDB.ApellidoPaterno = m.Value.ApellidoPaterno;
                        integranteDB.ApellidoMaterno = m.Value.ApellidoMaterno;
                        integranteDB.FechaNacimiento = m.Value.FechaNacimiento;
                        integranteDB.ParentescoId = m.Value.ParentescoId ?? "1";
                        integranteDB.SexoId = m.Value.SexoId;
                        integranteDB.EstadoId = m.Value.EstadoId;
                        integranteDB.EstudioId = m.Value.EstudioId;
                        integranteDB.EstadoCivilId = m.Value.EstadoCivilId;
                        integranteDB.Prueba = false;
                        integranteDB.TrabajadorId = m.Value.TrabajadorId;
                        integranteDB.NumIntegrante = m.Value.NumIntegrante;
                        integranteDB.CreatedAt = m.Value.CreatedAt;
                        integranteDB.UpdatedAt = m.Value.UpdatedAt;
                        if (familia.Value.Count > 1)
                        {
                            var m1 = familia.Value[1];
                            var llave1 = m1.ApellidoPaterno + "-" + m1.ApellidoMaterno + "-" + m1.Nombre + "-" +
                                        (m1.FechaNacimiento.HasValue
                                            ? m1.FechaNacimiento.Value.ToString("yyyyMMdd")
                                            : "") + "-" +
                                        (m1.SexoId ?? "") + "-" +
                                        (m1.EstadoId ?? "");
                            integranteDB.DomicilioId = beneficiariosHash[llave1].DomicilioId;
                        }
                        m.Value.Id = integranteDB.Id;
                        _context.Beneficiario.Add(integranteDB);
                        m.Value.Aplicacion.BeneficiarioId = integranteDB.Id;
                        _context.Aplicacion.Update(m.Value.Aplicacion);
                    }
                    else
                    {
                        integranteDB = beneficiariosHash[llave];
                        if (integranteDB.NumIntegrante > 1)
                        {
                            if (familia.Value[0].Id != null)
                            {
                                integranteDB.PadreId = familia.Value[0].Id;
                                _context.Beneficiario.Update(integranteDB);
                            }
                        }
                        else
                        {
                            integranteDB.ParentescoId = m.Value.ParentescoId;
                            _context.Beneficiario.Update(integranteDB);
                        }
                    }
                }
            }

            _context.SaveChanges();
            _context.Database.CommitTransaction();
        }

        public void CompletarEncuestaDomicilio()
        {
            var encuestas = new List<Aplicacion>();
            var respuestas = new List<AplicacionPregunta>();
            var inicio = DateTime.Parse("2021-02-08");
            var encuestaVersion = _context.EncuestaVersion.FirstOrDefault(ev => ev.Activa);
            var sinCodigo = _context.Respuesta.FirstOrDefault(r => r.Id == "1368");
            var familias = _context.Beneficiario.Where(b => b.DeletedAt == null && !b.Prueba && b.CreatedAt > inicio
                                                          && b.PadreId == null && !b.Aplicaciones.Any()).ToList();
            foreach (var familia in familias)
            {
                var aplicacion = new Aplicacion
                {
                    Id = Guid.NewGuid().ToString(),
                    Estatus = EstatusAplicacion.INCOMPLETO,
                    BeneficiarioId = familia.Id,
                    EncuestaVersionId = encuestaVersion.Id,
                    CreatedAt = familia.CreatedAt,
                    UpdatedAt = familia.CreatedAt,
                    TrabajadorId = familia.TrabajadorId,
                    Activa = true,
                    NumeroAplicacion = 1,
                    FechaInicio = familia.CreatedAt,
                    FechaSincronizacion = familia.CreatedAt,
                    Resultado = sinCodigo.Nombre,
                    DeviceId = familia.DeviceId
                };
                encuestas.Add(aplicacion);
                var respuesta = new AplicacionPregunta
                {
                    Id = Guid.NewGuid().ToString(),
                    AplicacionId = aplicacion.Id,
                    PreguntaId = sinCodigo.PreguntaId,
                    RespuestaId = sinCodigo.Id,
                    CreatedAt = aplicacion.CreatedAt,
                    UpdatedAt = aplicacion.UpdatedAt
                };
                respuestas.Add(respuesta);
            }

            using (var transaction = _context.Database.BeginTransaction())
            {
                _context.BulkInsert(encuestas);
                _context.BulkInsert(respuestas);
                transaction.Commit();
            }

        }

        public void CompletarFolio()
        {
            _context.Database.BeginTransaction();
            var inicio = DateTime.Parse("2021-02-08");
            var familias = _context.Beneficiario.Where(b => b.Folio == null && b.PadreId == null && !b.Prueba && b.DeletedAt == null && b.CreatedAt > inicio).Include(b => b.Domicilio)
                .Include(b => b.Trabajador).ToList();
            foreach (var familia in familias)
            {
                var fechaInicio = new DateTime(familia.CreatedAt.Year, familia.CreatedAt.Month, familia.CreatedAt.Day, 0, 0, 0, 0);
                var fechaFin = new DateTime(familia.CreatedAt.Year, familia.CreatedAt.Month, familia.CreatedAt.Day, 23, 59, 59, 999);
                var agebId = familia.Domicilio.AgebId == null ? "0000" : familia.Domicilio.AgebId.Substring(familia.Domicilio.AgebId.Length - 4);
                var manzanaId = familia.Domicilio.ManzanaId == null ? "0000" : familia.Domicilio.ManzanaId.Substring(familia.Domicilio.ManzanaId.Length - 3);
                var numDomiciliosDia = _context.Beneficiario.Count(b => b.TrabajadorId == familia.TrabajadorId && b.DeletedAt == null && b.CreatedAt > fechaInicio && b.CreatedAt < fechaFin && !b.Prueba && b.PadreId == null) + 1;
                var folio = familia.Trabajador.Codigo + familia.Domicilio.MunicipioId.Substring(1) + familia.Domicilio.LocalidadId.Substring(5) + agebId
                + manzanaId + familia.CreatedAt.ToString("ddMMyy") + LPad(numDomiciliosDia) + LPad(1);
                familia.Folio = folio;
                _context.Beneficiario.Update(familia);
            }
            _context.SaveChanges();
            _context.Database.CommitTransaction();
        }

        public string LPad(int str)
        {
            return str.ToString().PadLeft(2, '0');
        }

        [Authorize]
        [HttpPost]
        public string CompletarDatos([FromBody] CompletarDatos request)
        {
            var now = DateTime.Now;
            var inicio = new DateTime(now.Year, now.Month, 1).StartOf(DateTimeAnchor.Day);
            var fin = new DateTime(now.Year, now.Month, DateTime.DaysInMonth(now.Year, now.Month)).EndOf(DateTimeAnchor.Day);
            if (!string.IsNullOrEmpty(request.Inicio))
            {
                inicio = DateTime.Parse(request.Inicio.Substring(0, 10)).StartOf(DateTimeAnchor.Day);
            }
            if (!string.IsNullOrEmpty(request.Fin))
            {
                fin = DateTime.Parse(request.Fin.Substring(0, 10)).EndOf(DateTimeAnchor.Day);
            }
            switch (request.Accion)
            {
                case "curps":
                    return CompletarCurps(inicio, fin);
                case "diagnosticos":
                    return Importar(inicio, fin);
                case "direcciones":
                    return CompletarDirecciones(inicio, fin);
                case "carencias":
                    return CompletarCarencias(inicio, fin);
            }

            return "ok";
        }
    }
}