using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using DiagnosticoWeb.Database;
using DiagnosticoWeb.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Internal;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using DiagnosticoWeb.Code;
using DiagnosticoWeb.Validaciones;
using System.Security.Claims;
using System.IO;
using System.Drawing;

namespace DiagnosticoWeb.Controllers
{
    /// <summary>
    /// Clase con la logica de negocio para la visualizacion de las solicitudes registradas en los dispositivos moviles
    /// </summary>
    public class SolicitudesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="context">Conexion a la base de datos</param>
        /// <param name="userManager">Variable para conocer que usuario esta usando el sistema</param>
        public SolicitudesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        /// <summary>
        /// Funcion que muetra la vista con el listado de solicitudes registradas en la base de datos
        /// </summary>
        /// <returns>Vista con el listado de solicitudes</returns>
        [HttpGet]
        [Authorize]
        [RequireClaim("Permiso", Value = "solicitudes.ver")]
        public IActionResult Index()
        {
            var date = DateTime.Now;
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            var response = new ResponseSolicitudIndex()
            {
                Inicio = firstDayOfMonth.ToString("yyyy-MM-dd"),
                Fin = lastDayOfMonth.ToString("yyyy-MM-dd")
            };
            return View("ListadoSolicitudes", response);
        }

        /// <summary>
        /// Funcion para construir los filtros de busqueda del listado de solicitudes de acuerdo a los campos de la
        /// tabla solicitudes y las tablas con las que se relaciona
        /// </summary>
        /// <returns>Cadena JSON con los filtros que puede usar el usuario</returns>
        [HttpGet]
        [Authorize]
        public string BuildFiltros()
        {
            var response = new ResponseSolicitudIndex();
            var encuestaVersion = _context.EncuestaVersion.Where(ev => ev.Activa).Include(ev => ev.Preguntas)
                .ThenInclude(p => p.Respuestas).First();
            response.Filtros = new Dictionary<string, List<KeyValuePair<string, bool>>>();
            response.Filtros.Add("Beneficiario", new List<KeyValuePair<string, bool>>());
            response.Filtros.Add("Domicilio", new List<KeyValuePair<string, bool>>());
            response.Filtros.Add("Localidad", new List<KeyValuePair<string, bool>>());
            response.Filtros.Add("ZonasImpulso", new List<KeyValuePair<string, bool>>());
            response.Filtros.Add("Solicitud", new List<KeyValuePair<string, bool>>());
            response.Filtros.Add("Dependencia", new List<KeyValuePair<string, bool>>());
            response.Filtros.Add("Encuesta", new List<KeyValuePair<string, bool>>());

            response.Filtros["Beneficiario"].Add(new KeyValuePair<string, bool>("Folio", false));
            response.Filtros["Beneficiario"].Add(new KeyValuePair<string, bool>("Nombre", false));
            response.Filtros["Beneficiario"].Add(new KeyValuePair<string, bool>("ApellidoPaterno", false));
            response.Filtros["Beneficiario"].Add(new KeyValuePair<string, bool>("ApellidoMaterno", false));
            response.Filtros["Beneficiario"].Add(new KeyValuePair<string, bool>("Sexo", true));
            response.Filtros["Beneficiario"].Add(new KeyValuePair<string, bool>("Curp", false));
            response.Filtros["Beneficiario"].Add(new KeyValuePair<string, bool>("Rfc", false));
            response.Filtros["Domicilio"].Add(new KeyValuePair<string, bool>("Municipio", true));
            response.Filtros["Domicilio"].Add(new KeyValuePair<string, bool>("Ageb", true));
            response.Filtros["Domicilio"].Add(new KeyValuePair<string, bool>("CodigoPostal", false));
            response.Filtros["Domicilio"].Add(new KeyValuePair<string, bool>("DomicilioN", false));
            response.Filtros["Domicilio"].Add(new KeyValuePair<string, bool>("Zona", true));
            response.Filtros["Localidad"].Add(new KeyValuePair<string, bool>("Localidad", true));
            response.Filtros["Localidad"].Add(new KeyValuePair<string, bool>("ZonaIndigena", true));
            response.Filtros["ZonasImpulso"].Add(new KeyValuePair<string, bool>("ZonaImpulso", true));
            response.Filtros["Solicitud"].Add(new KeyValuePair<string, bool>("ProgramaSocial", true));
            response.Filtros["Solicitud"].Add(new KeyValuePair<string, bool>("Vertiente", true));
            response.Filtros["Solicitud"].Add(new KeyValuePair<string, bool>("Estatus", true));
            response.Filtros["Dependencia"].Add(new KeyValuePair<string, bool>("Dependencia", true));

            foreach (var pregunta in encuestaVersion.Preguntas.OrderBy(p => p.Numero))
            {
                response.Filtros["Encuesta"].Add(new KeyValuePair<string, bool>(
                    pregunta.Numero + ".-" + pregunta.Nombre.ToLower(), pregunta.Catalogo != null || pregunta.Respuestas.Any()));
            }

            return JsonSedeshu.SerializeObject(response);
        }

        /// <summary>
        /// Funcion que devuelve el valor de los filtros que son de tipo listado para que el usuario pueda filtrar el listado de solicitudes
        /// </summary>
        /// <param name="id">Nombre del catalogo del cual se mostraran sus valores</param>
        /// <param name="municipioId">Nombre del municipio para el caso en el que el catalogo es localidad o ageb</param>
        /// <returns>Cadena JSON con los valores del filtro</returns>
        [HttpGet("/Solicitudes/BuildCatalogos/{id}/{municipioId}")]
        [Authorize]
        public string BuildCatalogos(string id = "", string municipioId = "")
        {
            var admin = HttpContext.User.IsInRole("Administrador");
            var user = _userManager.GetUserAsync(HttpContext.User).Result;
            return JsonSedeshu.SerializeObject(BuildCatalogosList(id, municipioId, admin, user));
        }

        public List<Model> BuildCatalogosList(string id, string municipioId, bool admin, ApplicationUser user)
        {
            var response = new List<Model>();
            var encuestaVersion = _context.EncuestaVersion.Where(ev => ev.Activa).First();
            var preguntas = _context.Pregunta.Where(p => p.Activa && p.EncuestaVersionId.Equals(encuestaVersion.Id))
                .Include(p => p.Respuestas).ToDictionary(p => p.Numero);
            var dependenciaId = user.DependenciaId;
            switch (id)
            {
                case "CausaDiscapacidad":
                    _context.CausaDiscapacidad.Where(s => s.DeletedAt == null).ToList()
                        .ForEach(a => response.Add(new Model {Id = a.Id, Nombre = a.Nombre}));
                    break;
                case "Sexo":
                    _context.Sexo.Where(s => s.DeletedAt == null).ToList()
                        .ForEach(a => response.Add(new Model {Id = a.Id, Nombre = a.Nombre}));
                    break;
                case "Ageb":
                    _context.Ageb.Where(a => a.MunicipioId.Equals(municipioId)).ToList()
                        .ForEach(a => response.Add(new Model {Id = a.Id, Nombre = a.Clave}));
                    break;
                case "Municipio":
                    _context.Municipio.Where(m => m.DeletedAt == null).ToList().ForEach(a =>
                        response.Add(new Model {Id = a.Id, Nombre = a.Nombre}));
                    break;
                case "Localidad":
                    _context.Localidad.Where(l => l.DeletedAt == null && l.MunicipioId.Equals(municipioId)).ToList()
                        .ForEach(a =>
                            response.Add(new Model {Id = a.Id, Nombre = a.Nombre}));
                    break;
                case "ZonaImpulso":
                    _context.ZonaImpulso.Where(z => z.DeletedAt == null).ToList().ForEach(a =>
                        response.Add(new Model {Id = a.Id, Nombre = a.Nombre}));
                    break;
                case "ZonaIndigena":
                    _context.Localidad.Where(l => l.DeletedAt == null).Where(l => l.ZonaIndigenaId != null).ToList()
                        .ForEach(
                            a => response.Add(new Model {Id = a.Id, Nombre = a.Nombre}));
                    break;
                case "Trabajador":
                    if (admin)
                    {
                        _context.Trabajador.Where(t => t.DeletedAt == null).ToList().ForEach(a =>
                            response.Add(new Model {Id = a.Id, Nombre = a.Nombre}));
                    }
                    else
                    {
                        _context.Trabajador.Join(_context.TrabajadorDependencia, t => t.Id, td => td.TrabajadorId,
                            (t, td) =>
                                new
                                {
                                    Trabajador = t,
                                    TrabajadorDependencia = td
                                }).Where(t =>
                            t.Trabajador.DeletedAt == null && t.TrabajadorDependencia.DeletedAt == null &&
                            t.TrabajadorDependencia.DependenciaId.Equals(dependenciaId)).ToList().ForEach(a =>
                            response.Add(new Model {Id = a.Trabajador.Id, Nombre = a.Trabajador.Nombre}));
                    }

                    break;
                case "Dependencia":
                    if (admin)
                    {
                        _context.Dependencia.Where(d => d.DeletedAt == null).ToList().ForEach(a =>
                            response.Add(new Model {Id = a.Id, Nombre = a.Nombre}));
                    }
                    else
                    {
                        _context.Dependencia.Where(d => d.DeletedAt == null).Where(d => d.Id.Equals(dependenciaId))
                            .ToList()
                            .ForEach(a =>
                                response.Add(new Model {Id = a.Id, Nombre = a.Nombre}));
                    }

                    break;
                case "Zona":
                    if (admin)
                    {
                        _context.Zona.Where(z => z.DeletedAt == null).ToList().ForEach(a =>
                            response.Add(new Model {Id = a.Id, Nombre = a.Clave}));
                    }
                    else
                    {
                        _context.Zona.Where(z => z.DeletedAt == null).Where(z => z.DependenciaId.Equals(dependenciaId))
                            .ToList().ForEach(a => response.Add(new Model {Id = a.Id, Nombre = a.Clave}));
                    }

                    break;
                case "Estatus":
                    typeof(EstatusSolicitud).GetFields().ToList().ForEach(e => response.Add(new Model()
                    {
                        Id = e.Name,
                        Nombre = e.GetValue(null).ToString(),
                    }));
                    break;
                case "EstatusInformacion":
                    typeof(EstatusInformacion).GetFields().ToList().ForEach(e => response.Add(new Model()
                    {
                        Id = e.Name,
                        Nombre = e.GetValue(null).ToString(),
                    }));
                    break;
                case "Educativa" :
                case "Salud" :
                case "SeguridadSocial" :
                case "Servicios" :
                case "Vivienda" :
                case "Alimentaria" :
                    typeof(EstatusCarencia).GetFields().ToList().ForEach(e => response.Add(new Model()
                    {
                        Id = e.Name,
                        Nombre = e.GetValue(null).ToString(),
                    }));
                    break;
                case "LineaBienestar":
                    typeof(LineasBienestar).GetFields().ToList().ForEach(e => response.Add(new Model()
                    {
                        Id = e.Name,
                        Nombre = e.GetValue(null).ToString(),
                    }));
                    break;
                case "NivelPobreza":
                    typeof(NivelPobreza).GetFields().ToList().ForEach(e => response.Add(new Model()
                    {
                        Id = e.Name,
                        Nombre = e.GetValue(null).ToString(),
                    }));
                    break;
                case "Resultado":
                    _context.Respuesta.Where(r=>r.PreguntaId.Equals("138")).ToList().ForEach(m =>
                        response.Add(new Model() {Id = m.Nombre, Nombre = m.Nombre}));
                    break;
                default:
                    var pregunta = preguntas[Int32.Parse(id)];
                    if (pregunta != null)
                    {
                        if (pregunta.Catalogo == null)
                        {
                            foreach (var respuesta in pregunta.Respuestas)
                            {
                                response.Add(new Model()
                                {
                                    Id = respuesta.Id,
                                    Nombre = respuesta.Nombre
                                });
                            }
                        }
                        else
                        {
                            response.AddRange(Utils.BuildCatalogo(_context, pregunta.Catalogo));
                        }
                    }
                    break;
            }

            return response;
        }

        /// <summary>
        /// Funcion que consulta las solicitudes registradas en la base de datos de acuerdo a los filtros de busqueda
        /// seleccionados por el usuario
        /// </summary>
        /// <param name="request">Filtros de busqueda</param>
        /// <returns>Cadena JSON con el listado de solicitudes</returns>
        [HttpPost]
        [Authorize]
        public string ObtenerSolicitudes([FromBody] SolicitudesRequest request)
        {
            DateTime inicio;
            DateTime fin;
            string dependenciaId = null;
            DateTime.TryParseExact(request.Inicio.Substring(0, 10), "yyyy-MM-dd", CultureInfo.InvariantCulture,
                DateTimeStyles.None, out inicio);
            DateTime.TryParseExact(request.Fin.Substring(0, 10), "yyyy-MM-dd", CultureInfo.InvariantCulture,
                DateTimeStyles.None, out fin);
            var response = new SolicitudesResponse();
            var admin = HttpContext.User.IsInRole("Administrador");
            var regiones = new List<string>();
            if (!admin)
            {
                var user = _userManager.GetUserAsync(HttpContext.User).Result;
                dependenciaId = user.DependenciaId;
                _context.UsuarioRegion.Where(ur => ur.DeletedAt == null && ur.UsuarioId.Equals(user.Id)).ToList()
                    .ForEach(
                        ur => regiones.Add(ur.ZonaId));
            }

            var query = BuildSolicitudesQuery(request, false, dependenciaId, regiones);
            var SolQuery = _context.Solicitud
                .FromSql(query.Key, query.Value.ToArray())
                .Include(x => x.Vertiente).ThenInclude(x => x.ProgramaSocial)
                .Where(x => x.DeletedAt == null && !x.Estatus.Equals("Incompleta"))
                .Where(s => s.CreatedAt >= inicio).Where(s => s.CreatedAt <= fin.AddDays(1).AddTicks(-1))
                .OrderByDescending(x => x.CreatedAt);
            if (dependenciaId != null)
            {
                response.GranTotal = _context.Solicitud
                    .Where(s => s.DeletedAt == null && !s.Estatus.Equals(EstatusSolicitud.INCOMPLETA))
                    .Join(_context.Vertiente, s => s.VertienteId, v => v.Id, (s, v) => new
                    {
                        Vertiente = v,
                        Solicitud = s
                    }).Join(_context.ProgramaSocial, v => v.Vertiente.ProgramaSocialId, ps => ps.Id, (v, ps) => new
                    {
                        v.Solicitud,
                        ProgramaSocial = ps
                    }).Count(ps => ps.ProgramaSocial.DependenciaId.Equals(dependenciaId));
            }
            else
            {
                response.GranTotal = _context.Solicitud.Count(s =>
                    s.DeletedAt == null && !s.Estatus.Equals(EstatusSolicitud.INCOMPLETA));
            }

            response.Total = SolQuery.Count();
            var SolicitudesList = new List<SolicitudesListModel>();
            var Solicitudes = new List<Solicitud>();

            foreach (var Solicitud in Solicitudes)
            {
                var solicitudModel = new SolicitudesListModel()
                {
                    SolicitudId = Solicitud.Id,
                    Beneficiario = NombreCompleto(Solicitud.Beneficiario),
                    ProgramaSocial = Solicitud.Vertiente.ProgramaSocial.Nombre,
                    Vertiente = Solicitud.Vertiente.Nombre,
                    Estatus = Solicitud.Estatus,
                    CreatedAt = Solicitud.CreatedAt,
                };
                SolicitudesList.Add(solicitudModel);
            }

            response.Solicitudes = SolicitudesList;

            return JsonSedeshu.SerializeObject(response);
        }

        /// <summary>
        /// Funcion que exporta a un archivo de EXCEL el listado de solicitudes junto con la informacion de su
        /// beneficiario, respuestas de la encuesta CONEVAL de acuerdo a los filtros de busqueda seleccionados por el
        /// usuario
        /// </summary>
        /// <param name="request">Filtros de busqueda</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public IActionResult exportarExcel(SolicitudesRequest request)
        {
            DateTime inicio;
            DateTime fin;
            DateTime.TryParseExact(request.Inicio.Substring(0, 10), "yyyy-MM-dd", CultureInfo.InvariantCulture,
                DateTimeStyles.None, out inicio);
            DateTime.TryParseExact(request.Fin.Substring(0, 10), "yyyy-MM-dd", CultureInfo.InvariantCulture,
                DateTimeStyles.None, out fin);
            string dependenciaId = null;
            var admin = HttpContext.User.IsInRole("Administrador");
            var regiones = new List<string>();
            if (!admin)
            {
                var user = _userManager.GetUserAsync(HttpContext.User).Result;
                dependenciaId = user.DependenciaId;
                _context.UsuarioRegion.Where(ur => ur.DeletedAt == null && ur.UsuarioId.Equals(user.Id)).ToList()
                    .ForEach(
                        ur => regiones.Add(ur.ZonaId));
            }

            var encuestaVersion = _context.EncuestaVersion.Where(ev => ev.Activa).First();
            var query = BuildSolicitudesQuery(request, true, dependenciaId, regiones);
            var solicitudes = _context.Solicitud
                .FromSql(query.Key, query.Value.ToArray()).Include(x => x.Vertiente).ThenInclude(x => x.ProgramaSocial)
                .Include(x => x.Beneficiario).ThenInclude(b=>b.Sexo)
                .Include(s => s.Aplicacion)
                .ThenInclude(a => a.AplicacionPreguntas)
                .Include(x => x.Vertiente).ThenInclude(v => v.Unidad)
                .Where(x => x.DeletedAt == null && !x.Estatus.Equals("Incompleta"))
                .Where(s => s.CreatedAt >= inicio).Where(s => s.CreatedAt <= fin.AddDays(1).AddTicks(-1))
                .ToList();
            var maxRespuestaIteracion = Query.RawSqlQuery("select max(ap.RespuestaIteracion) from AplicacionPreguntas ap" +
                                           " inner join Aplicaciones a on ap.AplicacionId = a.Id inner join EncuestaVersiones EV on a.EncuestaVersionId = EV.Id" +
                                           " where ev.Activa = 1 and ap.RespuestaIteracion > 0 and ap.DeletedAt is null",
                x => x[0] is DBNull ? 0 : (int)x[0], _context);
            var maximo = maxRespuestaIteracion.First();
            var columnas = new Dictionary<string, int>();
            using (var excel = new ExcelPackage())
            {
                excel.Workbook.Worksheets.Add("solicitudes");
                var pestana = excel.Workbook.Worksheets.First();
                var fila = 2;
                pestana.Cells[1, 1].Value = "Nombre";
                pestana.Cells[1, 2].Value = "Apellidos";
                pestana.Cells[1, 3].Value = "Fecha de nacimiento";
                pestana.Cells[1, 4].Value = "Sexo";
                pestana.Cells[1, 5].Value = "CURP";
                pestana.Cells[1, 6].Value = "RFC";
                pestana.Cells[1, 7].Value = "AGEB";
                pestana.Cells[1, 8].Value = "Telefono";
                pestana.Cells[1, 9].Value = "Telefono casa";
                pestana.Cells[1, 10].Value = "Domicilio";
                pestana.Cells[1, 11].Value = "Latitud";
                pestana.Cells[1, 12].Value = "Longitud";
                pestana.Cells[1, 13].Value = "Codigo postal";
                pestana.Cells[1, 14].Value = "Programa";
                pestana.Cells[1, 15].Value = "Proyecto de inversion";
                pestana.Cells[1, 16].Value = "Vertiente";
                pestana.Cells[1, 17].Value = "Ejercicio";
                pestana.Cells[1, 18].Value = "Ciclo";
                pestana.Cells[1, 19].Value = "Unidad";
                pestana.Cells[1, 20].Value = "Cantidad";
                pestana.Cells[1, 21].Value = "Costo";
                pestana.Cells[1, 22].Value = "Economico";
                pestana.Cells[1, 23].Value = "Fecha";
                var numPregunta = 24;
                var preguntasNoItarables = _context.Pregunta
                    .Where(p => p.DeletedAt == null && p.EncuestaVersionId.Equals(encuestaVersion.Id))
                    .Where(r => r.DeletedAt == null).Where(p => !p.Iterable).OrderBy(p => p.Numero);
                var preguntasItarables = _context.Pregunta
                    .Where(p => p.DeletedAt == null && p.EncuestaVersionId.Equals(encuestaVersion.Id))
                    .Where(r => r.DeletedAt == null).Where(p => p.Iterable).OrderBy(p => p.Numero);
                foreach (var pregunta in preguntasNoItarables)
                {
                    columnas.Add(pregunta.Id, numPregunta);
                    pestana.Cells[1, numPregunta].Value = pregunta.Nombre;
                    pestana.Cells[1, numPregunta + 1].Value = "Id";
                    numPregunta += 2;
                }

                for (int j = 0; j <= maximo; j++)
                {
                    foreach (var pregunta in preguntasItarables)
                    {
                        columnas.Add(pregunta.Id + "-" + j, numPregunta);
                        pestana.Cells[1, numPregunta].Value = pregunta.Nombre;
                        pestana.Cells[1, numPregunta + 1].Value = "Id";
                        numPregunta += 2;
                    }
                }

                var id = "";
                try
                {
                    foreach (var solicitud in solicitudes)
                    {
                        id = solicitud.Id;
                        var domicilio = solicitud.Beneficiario.Domicilio;
                        pestana.Cells[fila, 1].Value = solicitud.Beneficiario.Nombre;
                        pestana.Cells[fila, 2].Value = solicitud.Beneficiario.ApellidoPaterno + " " +
                                                       solicitud.Beneficiario.ApellidoMaterno;
                        pestana.Cells[fila, 3].Value = solicitud.Beneficiario.FechaNacimiento.HasValue?solicitud.Beneficiario.FechaNacimiento.Value.ToString("dd/MM/yyyy"):"";
                        pestana.Cells[fila, 4].Value = solicitud.Beneficiario.Sexo.Nombre;
                        pestana.Cells[fila, 5].Value = solicitud.Beneficiario.Curp;
                        pestana.Cells[fila, 6].Value = solicitud.Beneficiario.Rfc;
                        pestana.Cells[fila, 7].Value = domicilio.AgebId;
                        pestana.Cells[fila, 8].Value = domicilio.Telefono;
                        pestana.Cells[fila, 9].Value = domicilio.TelefonoCasa;
                        pestana.Cells[fila, 10].Value = domicilio.DomicilioN;
                        pestana.Cells[fila, 11].Value = domicilio.Latitud;
                        pestana.Cells[fila, 12].Value = domicilio.Longitud;
                        pestana.Cells[fila, 13].Value = domicilio.CodigoPostal;
                        pestana.Cells[fila, 14].Value = solicitud.Vertiente.ProgramaSocial.Nombre;
                        pestana.Cells[fila, 15].Value = solicitud.Vertiente.ProgramaSocial.Proyecto;
                        pestana.Cells[fila, 16].Value = solicitud.Vertiente.Nombre;
                        pestana.Cells[fila, 17].Value = solicitud.Vertiente.Ejercicio;
                        pestana.Cells[fila, 18].Value = solicitud.Vertiente.Ciclo;
                        pestana.Cells[fila, 19].Value = solicitud.Vertiente.UnidadId == null
                            ? "" : solicitud.Vertiente.Unidad.Nombre;
                        pestana.Cells[fila, 20].Value = solicitud.Cantidad;
                        pestana.Cells[fila, 21].Value = solicitud.Costo;
                        pestana.Cells[fila, 22].Value = solicitud.Economico;
                        pestana.Cells[fila, 23].Value = solicitud.CreatedAt.ToString("dd/MM/yyyy");
                        numPregunta = 24;
                        foreach (var pregunta in solicitud.Aplicacion.AplicacionPreguntas
                            .Where(ap => !ap.Pregunta.Iterable)
                            .OrderBy(ap => ap.Pregunta.Numero))
                        {
                            if (pregunta.DeletedAt != null)
                            {
                                var columna = columnas[pregunta.PreguntaId];
                                pestana.Cells[fila, columna].Value =
                                    pregunta.Valor == "" ? pregunta.RespuestaValor.ToString() : pregunta.Valor;
                                pestana.Cells[fila, columna + 1].Value = pregunta.RespuestaId;
                                numPregunta += 2;
                            }
                        }

                        foreach (var pregunta in solicitud.Aplicacion.AplicacionPreguntas
                            .Where(ap => ap.Pregunta.Iterable).OrderBy(ap => ap.Pregunta.Numero))
                        {
                            if (pregunta.DeletedAt != null)
                            {
                                var key = pregunta.PreguntaId + "-" + pregunta.RespuestaIteracion;
                                var hasKey = columnas.ContainsKey(key);
                                if (hasKey)
                                {
                                    var columna = columnas[key];
                                    if (pestana.Cells[fila, columna].GetValue<string>() == null)
                                    {
                                        pestana.Cells[fila, columna].Value =
                                            pregunta.Valor == "" ? pregunta.RespuestaValor.ToString() : pregunta.Valor;
                                        pestana.Cells[fila, columna + 1].Value = pregunta.RespuestaId;
                                    }
                                    else
                                    {
                                        pestana.Cells[fila, columna].Value += "," + (pregunta.Valor == ""
                                                                                  ? pregunta.RespuestaValor.ToString()
                                                                                  : pregunta.Valor);
                                        pestana.Cells[fila, columna + 1].Value += "," + (pregunta.RespuestaId);
                                    }

                                    if (pregunta.Grado != null)
                                    {
                                        pestana.Cells[fila, columna].Value += "=>" + (pregunta.Grado);
                                        pestana.Cells[fila, columna + 1].Value += "=>" + (pregunta.Grado);
                                    }
                                }
                            }

                            numPregunta += 2;
                        }

                        fila++;
                    }
                }
                catch (Exception e)
                {
                    Excepcion.Registrar(e);
                    throw;
                }

                _context.Bitacora.Add(new Bitacora()
                {
                    UsuarioId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                    Accion = AccionBitacora.EXPORTACION,
                    Mensaje = "Se exportó el catálogo de solicitudes.",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                });
                _context.SaveChanges();

                return File(
                    fileContents: excel.GetAsByteArray(),
                    contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    fileDownloadName: "solicitudes.xlsx"
                );
            }
        }

        /// <summary>
        /// Funcion que constuye la consulta SQL que obtendrá el listado de solicitudes de acuerdo a los filtros de
        /// busqueda seleccionados por el usuario
        /// </summary>
        /// <param name="request">Filtros de busqueda</param>
        /// <param name="exportar">Variable que indica si esta consulta se exportará a EXCEL ya que de ser asi se consulta la informacion del beneficiario, su domicilio y las respuestas de la encuesta CONEVAL</param>
        /// <param name="dependenciaId">Identificador en base de datos de la dependencia del usuario en sesion en el caso de que el usuario no sea administrador y las solicitudes solo sean de programas pertenecientes a la dependencia</param>
        /// <param name="regiones"></param>
        /// <returns></returns>
        private KeyValuePair<string, List<string>> BuildSolicitudesQuery(SolicitudesRequest request, bool exportar,
            string dependenciaId, List<string> regiones)
        {
            var i = 0;
            string inners = "";
            string query = "select DISTINCT(S.Id) as unico, S.* from Solicitudes S ";
            var valores = new List<string>();
            var tablas = new Dictionary<string, string>();
            var where = " where S.Estatus !='{" + (i++) + "}' ";
            valores.Add(EstatusSolicitud.INCOMPLETA);
            if (dependenciaId != null)
            {
                tablas.Add("V", " inner join Vertientes V on V.Id=S.VertienteId and V.DeletedAt is null");
                tablas.Add("PS",
                    " inner join ProgramasSociales PS on PS.Id=V.ProgramaSocialId and PS.DeletedAt is null");
                where += " AND PS.DependenciaId={" + (i++) + "} ";
                valores.Add(dependenciaId);
            }

            if (regiones.Count > 0)
            {
                var municipios = new HashSet<string>();
                var regionesDB = _context.Zona.Where(r => regiones.Contains(r.Id)).Include(r => r.MunicipioZonas);
                foreach (var regionDB in regionesDB)
                {
                    foreach (MunicipioZona municipioZona in regionDB.MunicipioZonas)
                    {
                        municipios.Add(municipioZona.MunicipioId);
                    }
                }

                tablas.Add("D", " inner join Domicilio D on S.DomicilioId=D.Id ");
                tablas.Add("L", " inner join Localidades L on L.Id=D.LocalidadId ");
                where += " AND L.MunicipioId in(" + string.Join(",", municipios) + ")";
            }

            var encuesta = _context.Encuesta.Where(e => e.Activo).Include(e => e.EncuestaVersiones)
                .ThenInclude(ev => ev.Preguntas).ThenInclude(p => p.Respuestas).First();
            var json = JsonConvert.DeserializeObject(request.Valores);
            var solicitudProperties = typeof(Solicitud).GetProperties();
            var beneficiarioProperties = typeof(Beneficiario).GetProperties();
            var domicilioProperties = typeof(Domicilio).GetProperties();
            var localidadProperties = typeof(Localidad).GetProperties();
            var programaSocialProperties = typeof(ProgramaSocial).GetProperties();
            var vertienteProperties = typeof(Vertiente).GetProperties();
            var municipioProperties = typeof(Municipio).GetProperties();

            var solicitudDictionary = Query.BuildPropertyDictionary(solicitudProperties);
            var beneficiarioDictionary = Query.BuildPropertyDictionary(beneficiarioProperties);
            var domicilioDictionary = Query.BuildPropertyDictionary(domicilioProperties);
            var localidadDictionary = Query.BuildPropertyDictionary(localidadProperties);
            var vertienteDictionary = Query.BuildPropertyDictionary(vertienteProperties);
            var programaSocialDictionary = Query.BuildPropertyDictionary(programaSocialProperties);
            var municipioDictionary = Query.BuildPropertyDictionary(municipioProperties);

            var preguntasDictionary = new Dictionary<string, Pregunta>();
            if (exportar)
            {
                tablas.Add("A", " inner join Aplicaciones A on A.Id = S.AplicacionId ");
                tablas.Add("AP", " inner join AplicacionPreguntas AP on AP.AplicacionId=A.Id ");
            }

            foreach (var pregunta in encuesta.EncuestaVersiones.Where(ev => ev.Activa).First().Preguntas)
            {
                preguntasDictionary.Add(pregunta.Numero + ".-" + pregunta.Nombre.Replace(" ", "").ToLower(), pregunta);
            }

            foreach (var valor in (JObject) json)
            {
                if (valor.Key.ToString().Length > 0 && valor.Value.ToString() != "")
                {
                    if (preguntasDictionary.ContainsKey(valor.Key.ToLower()))
                    {
                        var campo = preguntasDictionary[valor.Key.ToLower()];
                        var esId = campo.Catalogo != null || campo.Respuestas.Any();
                        where += " and AP.preguntaId = {" + i + "} and AP."
                                 + (esId ? "RespuestaId = {" + (i + 1) + "}" : "Valor like {" + (i + 1) + "}");
                        valores.Add(campo.Id);
                        valores.Add(esId ? valor.Value.ToString() : "%" + valor.Value.ToString() + "%");
                        i += 2;
                        if (!tablas.ContainsKey("A"))
                        {
                            tablas.Add("A", " inner join Aplicaciones A on A.Id = S.AplicacionId ");
                            tablas.Add("AP", " inner join AplicacionPreguntas AP on AP.AplicacionId=A.Id ");
                        }
                    }
                    else
                    {
                        if (Query.FiltrarSolicitudes(ref where, ref i, solicitudDictionary, valor.Key,
                            valor.Value.ToString(), valores, "S"))
                        {
                            if (!tablas.ContainsKey("T"))
                            {
                                tablas.Add("T", " inner join Trabajadores T on T.Id=S.TrabajadorId ");
                            }
                        }
                        else
                        {
                            if (Query.FiltrarSolicitudes(ref where, ref i, beneficiarioDictionary, valor.Key,
                                valor.Value.ToString(), valores, "B"))
                            {
                                if (!tablas.ContainsKey("B"))
                                {
                                    tablas.Add("B", " inner join Beneficiarios B on B.Id=S.BeneficiarioId ");
                                }
                            }
                            else
                            {
                                if (Query.FiltrarSolicitudes(ref where, ref i, domicilioDictionary, valor.Key,
                                    valor.Value.ToString(), valores, "D"))
                                {
                                    if (!tablas.ContainsKey("D"))
                                    {
                                        tablas.Add("D", " inner join Domicilio D on S.DomicilioId=D.Id ");
                                    }
                                }
                                else
                                {
                                    if (Query.FiltrarSolicitudes(ref where, ref i, localidadDictionary, valor.Key,
                                        valor.Value.ToString(), valores, "L"))
                                    {
                                        if (!tablas.ContainsKey("D"))
                                        {
                                            tablas.Add("D", " inner join Domicilio D on S.DomicilioId=S.Id ");
                                        }

                                        if (!tablas.ContainsKey("L"))
                                        {
                                            tablas.Add("L", " inner join Localidades L on L.Id=D.LocalidadId ");
                                        }
                                    }
                                    else
                                    {
                                        if (Query.FiltrarSolicitudes(ref where, ref i, programaSocialDictionary,
                                            valor.Key, valor.Value.ToString(), valores, "PS"))
                                        {
                                            if (!tablas.ContainsKey("PS"))
                                            {
                                                tablas.Add("V",
                                                    " inner join Vertientes V on V.Id=S.VertienteId and V.DeletedAt is null");
                                                tablas.Add("PS",
                                                    " inner join ProgramasSociales PS on PS.Id=V.ProgramaSocialId and PS.DeletedAt is null");
                                            }
                                        }
                                        else
                                        {
                                            if (Query.FiltrarSolicitudes(ref where, ref i, municipioDictionary,
                                                valor.Key,
                                                valor.Value.ToString(), valores, "MZ"))
                                            {
                                                if (!tablas.ContainsKey("D"))
                                                {
                                                    tablas.Add("D", " inner join Domicilio D on S.DomicilioId=S.Id ");
                                                }

                                                if (!tablas.ContainsKey("MZ"))
                                                {
                                                    tablas.Add("MZ",
                                                        " inner join MunicipiosZonas MZ on MZ.MunicipioId=D.MunicipioId");
                                                }
                                            }
                                            else
                                            {
                                                if (Query.FiltrarSolicitudes(ref where, ref i, vertienteDictionary,
                                                    valor.Key, valor.Value.ToString(), valores, "V"))
                                                {
                                                    if (!tablas.ContainsKey("V"))
                                                    {
                                                        tablas.Add("V",
                                                            " inner join Vertientes V on V.Id=S.VertienteId and V.DeletedAt is null");
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            foreach (var tabla in tablas)
            {
                inners += tabla.Value;
            }

            return new KeyValuePair<string, List<string>>(query + inners + where, valores);
        }

        /// <summary>
        /// Funcion que genera el nombre completo del beneficiario a partir de sus apeliidos y nombres
        /// </summary>
        /// <param name="beneficiario">Modelo del beneficiario</param>
        /// <returns>Cadena con el nombre completo del beneficiario</returns>
        public string NombreCompleto(Beneficiario beneficiario)
        {
            return beneficiario.ApellidoPaterno + " " + beneficiario.ApellidoMaterno + " " + beneficiario.Nombre;
        }

        /// <summary>
        /// Funcion que muestra el detalle de la solicitud seleccionada por el usuario(datos personales del beneficiario,
        /// fotografias, datos del comicilio, respuestas de la encuesta coneval y carencias encontradas)
        /// </summary>
        /// <param name="id">Identificador en base de datos de la solicitud seleccionada por el usuario</param>
        /// <returns>Vista con la visualizacion de la informacion</returns>
        [HttpGet]
        [Authorize]
        [RequireClaim("Permiso", Value = "solicitudes.ver")]
        public IActionResult VerSolicitud(string id = "")
        {
            var SolicitudResponse = new VerSolicitudModel();
            var carenciasDb = _context.Carencia.ToDictionary(c => c.Clave);
            var Sol = _context.Solicitud.Include(x => x.Vertiente).ThenInclude(v => v.ProgramaSocial).Include(v => v.Vertiente).ThenInclude(x => x.Unidad)
                .Include(x => x.Beneficiario).ThenInclude(b => b.Domicilio)
                .Include(s => s.Aplicacion)
                .First(x => x.DeletedAt == null && x.Id.Equals(id));

            SolicitudResponse.Solicitud = new SolicitudesListModel()
            {
                SolicitudId = Sol.Id,
                Beneficiario = NombreCompleto(Sol.Beneficiario),
                ProgramaSocial = Sol.Vertiente.ProgramaSocial.Nombre,
                Vertiente = Sol.Vertiente.Nombre,
                Unidad = Sol.Vertiente.Unidad.Nombre,
                Cantidad = Sol.Cantidad,
                Costo = Sol.Costo,
                Economico = Sol.Economico,
                Estatus = Sol.Estatus,
                CreatedAt = Sol.CreatedAt
            };
            SolicitudResponse.Preguntas = GenerarEncuesta(Sol.AplicacionId);
            var domicilio = Sol.Beneficiario.Domicilio;
            var ageb = _context.Ageb.Find(domicilio.AgebId);
            var localidad = _context.Localidad.Where(l => l.Id.Equals(domicilio.LocalidadId)).Include(l => l.Municipio)
                .First();
            SolicitudResponse.Solicitud.Domicilio = new DomicilioResponse()
            {
                Domicilio = domicilio.DomicilioN,
                Telefono = domicilio.Telefono,
                Email = domicilio.Email,
                Ageb = ageb == null ? "" : ageb.Clave,
                Localidad = localidad.Nombre,
                Municipio = localidad.Municipio.Nombre,
                Latitud = domicilio.Latitud,
                Longitud = domicilio.Longitud,
            };

            SolicitudResponse.Carencias = new List<Carencia>();

            SolicitudResponse.Imagenes = new List<ImagenModel>();
            SolicitudResponse.Documentos = new List<ImagenModel>();
            var vertientesArchivos = _context.VertienteArchivo.ToDictionary(x => x.Id);
            var archivos = _context.Archivo.Where(x => x.DeletedAt == null && x.Nombre.Contains(id))
                .Select(a => new {a.Id, a.Nombre}).ToList();
            var archivosBeneficiario = _context.Archivo
                .Where(x => x.DeletedAt == null && x.Nombre.Contains(Sol.BeneficiarioId))
                .Select(a => new {a.Id, a.Nombre}).ToList();
            foreach (var archivo in archivos)
            {
                SolicitudResponse.Imagenes.Add(new ImagenModel()
                {
                    Id = archivo.Id,
                    Nombre = ObtenerNombreFoto(archivo.Nombre, vertientesArchivos),
                    Show = false
                });
            }

            foreach (var archivo in archivosBeneficiario)
            {
                SolicitudResponse.Documentos.Add(new ImagenModel()
                {
                    Id = archivo.Id,
                    Nombre = archivo.Nombre.Replace(Sol.BeneficiarioId, "").Replace("_", " ").Trim(),
                    Show = false
                });
            }

            return View(SolicitudResponse);
        }

        /// <summary>
        /// Funcion que consulta en la base de datos la imagen del beneficiario solicitada por el usuario
        /// </summary>
        /// <param name="id">Identificador en base de datos de la imagen</param>
        /// <returns></returns>
        public string GetImagen(string id = "")
        {
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), "archivos");
            //pathToSave = @"\\139.70.80.12\c$\inetpub\wwwroot\diagnostico\archivos";
            using (var image = Image.FromFile(Path.Combine(pathToSave, id + ".jpg")))
            {
                using (MemoryStream m = new MemoryStream())
                {
                    image.Save(m, image.RawFormat);
                    var imageBytes = m.ToArray();
                    return Convert.ToBase64String(imageBytes);
                }
            }
        }

        /// <summary>
        /// Funcion que construye el nombre del documento de acuerdo al id del registro en la base de datos
        /// </summary>
        /// <param name="Nombre">Nombre del registro</param>
        /// <param name="vertientesArchivos">Diccionario con los archivos del beneficiario</param>
        /// <returns>Cadena con el nombre del archivo</returns>
        public string ObtenerNombreFoto(string Nombre, Dictionary<string, VertienteArchivo> vertientesArchivos)
        {
            var ids = Nombre.Split("_");
            if (ids.Length > 1)
            {
                if (vertientesArchivos.ContainsKey(ids[1]))
                {
                    var vertienteArchivo = vertientesArchivos[ids[1]];

                    return vertienteArchivo.TipoArchivo;
                }

                return ids[1];
            }

            return Nombre;
        }

        /// <summary>
        /// Funcion que consulta las respuestas de la encuesta CONEVAL en la solicitud seleccionada por el usuario
        /// </summary>
        /// <param name="id">Identificador en base de datos de la solicitud seleccionada por el usuario</param>
        /// <returns>Listado de preguntas respondidas en el hogar visitado</returns>
        public List<EncuestaSolicitudModel> GenerarEncuesta(string id = "")
        {
            var aplicaciones = _context.Aplicacion.Where(x => x.Id.Equals(id)).Join(
                    _context.AplicacionPregunta.Where(x => x.DeletedAt == null),
                    App => App.Id, AppPreg => AppPreg.AplicacionId,
                    (App, AppPreg) => new {Aplicacion = App, AplicacionPregunta = AppPreg})
                .Join(_context.Pregunta, AppPreg => AppPreg.AplicacionPregunta.PreguntaId, Preg => Preg.Id,
                    (AppPreg, Preg) => new {AplicacionPregunta = AppPreg, Pregunta = Preg})
                .OrderBy(x => x.Pregunta.Numero).ToList();

            var Preguntas = new List<Pregunta>();
            var encuestaVersion = _context.EncuestaVersion.Where(x => x.Activa)
                .Include(x => x.Preguntas)
                .ThenInclude(x => x.Respuestas).First();

            foreach (var Pregunta in encuestaVersion.Preguntas.OrderBy(x => x.Numero))
            {
                Preguntas.Add(Pregunta);
            }

            var preguntas = new List<EncuestaSolicitudModel>();
            var index = 1;
            var numeroPregunta = 0;
            foreach (var Pregunta in Preguntas)
            {
                if (Pregunta.Numero > numeroPregunta)
                {
                    if (Pregunta.Iterable)
                    {
                        var respuestaIteracion =
                            EncontrarRespuesta(Pregunta.CondicionIterable.Substring(1), aplicaciones);
                        if(respuestaIteracion!=null)
                        {
                            var PreguntasIterables = ObtenerRespuestasSeguidas(Pregunta, Preguntas);
                            for (var i = 0; i < Int64.Parse(respuestaIteracion); i++)
                            {
                                foreach (var PregIt in PreguntasIterables)
                                {
                                    var respuesta = EncontrarRespuesta(PregIt.Id, aplicaciones, i);
                                    if (!string.IsNullOrEmpty(respuesta))
                                    {
                                        preguntas.Add(new EncuestaSolicitudModel()
                                        {
                                            Pregunta = index++ + "." + (i + 1) + ".- " + PregIt.Nombre,
                                            Respuesta = respuesta
                                        });
                                    }

                                    numeroPregunta = PregIt.Numero > numeroPregunta ? PregIt.Numero : numeroPregunta;
                                }
                            }
                        }
                    }
                    else
                    {
                        var respuesta = EncontrarRespuesta(Pregunta.Id, aplicaciones);
                        if (!string.IsNullOrEmpty(respuesta))
                        {
                            preguntas.Add(new EncuestaSolicitudModel()
                            {
                                Pregunta = index++ + ".- " + Pregunta.Nombre,
                                Respuesta = respuesta
                            });
                        }
                    }
                }

                numeroPregunta = Pregunta.Numero > numeroPregunta ? Pregunta.Numero : numeroPregunta;
            }

            return preguntas;
        }

        /// <summary>
        /// Funcion que devuelve la respuesta del beneficiario en la pregunta seleccionada por el usuario, esta busqueda depende si la pregunta es de tipo catalogo o de tipo respuesta
        /// </summary>
        /// <param name="Id">Identificador en base de datos de la pregunta a buscar</param>
        /// <param name="List">Listado de pregutnas respondidas por el beneficiario</param>
        /// <param name="iteracion">Numero de integrente preguntado</param>
        /// <returns>Cadena con el texto de la respuesta</returns>
        public string EncontrarRespuesta(string Id, IEnumerable<dynamic> List, int iteracion = 0)
        {
            foreach (var Element in List)
            {
                var AplicacionPregunta = Element.AplicacionPregunta.AplicacionPregunta;
                if (Id.Equals(Element.Pregunta.Id) && Int64.Parse(AplicacionPregunta.RespuestaIteracion) == iteracion)
                {
                    if (AplicacionPregunta.Respuesta != null)
                    {
                        return AplicacionPregunta.Respuesta.Nombre;
                    }

                    return !string.IsNullOrEmpty(AplicacionPregunta.Valor)
                        ? AplicacionPregunta.Valor
                        : "" + AplicacionPregunta.RespuestaValor;
                }
            }

            return null;
        }

        public EncuestaValores ObtenerRespuestas(List<EncuestaSolicitudModel> encuesta, int index)
        {
            return new EncuestaValores() {Index = 1, Preguntas = null};
        }

        /// <summary>
        /// Funcion que devuelve las preguntas que son iterables de la encuesta CONEVAL
        /// </summary>
        /// <param name="pregunta">Modelo de la pregunta para ver si es iterable</param>
        /// <param name="preguntas">Listado de preguntas de la encuesta CONEVAL</param>
        /// <returns>Listado de preguntas iterables</returns>
        public List<Pregunta> ObtenerRespuestasSeguidas(Pregunta pregunta, List<Pregunta> preguntas)
        {
            var pregs = new List<Pregunta>();
            foreach (var p in preguntas)
            {
                if (p.Numero >= pregunta.Numero)
                {
                    if (p.Iterable && pregunta.CondicionIterable.Equals(p.CondicionIterable))
                    {
                        pregs.Add(p);
                    }
                    else
                    {
                        return pregs;
                    }
                }
            }

            return pregs;
        }

        /// <summary>
        /// Funcion que dicatmina como positiva la entrega de la solicitud seleccionada por el usuario
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public string Aceptar([FromBody] DictaminacionModel model)
        {
            var solicitud = _context.Solicitud.Find(model.Id);
            var requ = Request;
            solicitud.Estatus = EstatusSolicitud.ACEPTADO;
            solicitud.UpdatedAt = DateTime.Now;
            _context.Solicitud.Update(solicitud);

            _context.Bitacora.Add(new Bitacora()
            {
                UsuarioId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                Accion = AccionBitacora.EDICION,
                Mensaje = "Se modificó el estatus a Aceptado de una solicitud con Id " + solicitud.Id.Substring(0, 8) + "...",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            });

            _context.SaveChanges();

            TempData["Alert"] = "Show";
            TempData.Keep("Alert");

            return "ok";
        }

        /// <summary>
        /// Funcion que dictamina como negativa la entrega de la solicitud seleccionada por el usuario
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public string Rechazar([FromBody] DictaminacionModel model)
        {
            var solicitud = _context.Solicitud.Find(model.Id);
            solicitud.Estatus = EstatusSolicitud.RECHAZADO;
            solicitud.UpdatedAt = DateTime.Now;
            _context.Solicitud.Update(solicitud);

            _context.Bitacora.Add(new Bitacora()
            {
                UsuarioId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                Accion = AccionBitacora.EDICION,
                Mensaje = "Se modificó el estatus a Rechazado de una solicitud con Id " + solicitud.Id.Substring(0, 8) + "...",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            });

            _context.SaveChanges();

            TempData["Alert"] = "Show";
            TempData.Keep("Alert");

            return "ok";
        }

        /// <summary>
        /// Funcion que muestra la vista con un mapa donde se marcaran los domicilios que tienen solicitudes
        /// </summary>
        /// <returns>Vista con el mapa que marcara los domicilios visitados por los promotores de la dependencia</returns>
        [HttpGet]
        [Authorize]
        [RequireClaim("Permiso", Value = "solicitudes.ver")]
        public IActionResult Mapa()
        {
            var date = DateTime.Now;
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            var response = new ResponseSolicitudIndex()
            {
                Inicio = firstDayOfMonth.ToString("yyyy-MM-dd"),
                Fin = lastDayOfMonth.ToString("yyyy-MM-dd")
            };
            return View("Mapa", response);
        }
    }
}