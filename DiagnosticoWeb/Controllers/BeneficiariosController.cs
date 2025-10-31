using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using moment.net;
using moment.net.Enums;
using Newtonsoft.Json;
using OfficeOpenXml;
using Rotativa.AspNetCore;
using DiagnosticoWeb.Controllers;
using DiagnosticoWeb.Code;
using DiagnosticoWeb.Database;
using DiagnosticoWeb.Models;
using DiagnosticoWeb.Validaciones;
using QRCoder;

namespace DiagnosticoWeb.Controllers
{
    /// <summary>
    /// Clase que permite consultar los beneficiarios registrados en la base de datos, así como ver y editar la información de sus datos personales
    /// </summary>
    public class BeneficiariosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpClientFactory _clientFactory;

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="context">Conexion a la base de datos</param>
        /// <param name="userManager">Objeto para identificar que usuario esta usando el sistema</param>
        public BeneficiariosController(ApplicationDbContext context, IConfiguration configuration,
            UserManager<ApplicationUser> userManager, IHttpClientFactory clientFactory)
        {
            _context = context;
            _configuration = configuration;
            _userManager = userManager;
            _clientFactory = clientFactory;
        }

        /// <summary>
        /// Funcion que muestra la vista con el listado de beneficiarios registrados en la base de datos
        /// </summary>
        /// <returns>Vista con el listado de beneficiarios registrados en la base de datos</returns>
        [HttpGet]
        [Authorize]
        [RequireClaim("Permiso", Value = "beneficiarios.ver")]
        public IActionResult Index()
        {
            var response = new ReponseBeneficiarioIndex();
            var date = DateTime.Now;
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            response.Inicio = firstDayOfMonth.ToString("yyyy-MM-dd");
            response.Fin = lastDayOfMonth.ToString("yyyy-MM-dd");
            return View("Index", response);
        }

        /// <summary>
        /// Funcion que devuelve los campos que puede utilizar el usuario para filtrar el listado de beneficiarios
        /// de acuerdo a los campos que tiene definidos la tabla de benficiarios y las tablas con las que se relaciona
        /// </summary>
        /// <returns>Cadena JSON el listado de filtros que puede utilizar el usuario</returns>
        [HttpGet]
        [Authorize]
        public string BuildFiltros(bool aplicacionesCompletas = true)
        {
            var response = new ResponseSolicitudIndex();
            var encuestaVersion = _context.EncuestaVersion.Where(ev => ev.Activa).Include(ev => ev.Preguntas)
                .ThenInclude(p => p.Respuestas).First();
            response.Filtros = new Dictionary<string, List<KeyValuePair<string, bool>>>
            {
                {"Beneficiario", new List<KeyValuePair<string, bool>>()},
                {"Domicilio", new List<KeyValuePair<string, bool>>()},
                {"Localidad", new List<KeyValuePair<string, bool>>()},
                {"ZonasImpulso", new List<KeyValuePair<string, bool>>()},
                {"Visita", new List<KeyValuePair<string, bool>>()},
                {"Encuesta", new List<KeyValuePair<string, bool>>()}
            };

            response.Filtros["Beneficiario"].Add(new KeyValuePair<string, bool>("Folio", false));
            response.Filtros["Beneficiario"].Add(new KeyValuePair<string, bool>("Nombre", false));
            response.Filtros["Beneficiario"].Add(new KeyValuePair<string, bool>("ApellidoPaterno", false));
            response.Filtros["Beneficiario"].Add(new KeyValuePair<string, bool>("ApellidoMaterno", false));
            response.Filtros["Beneficiario"].Add(new KeyValuePair<string, bool>("Sexo", true));
            response.Filtros["Beneficiario"].Add(new KeyValuePair<string, bool>("Curp", false));
            response.Filtros["Beneficiario"].Add(new KeyValuePair<string, bool>("Rfc", false));
            response.Filtros["Beneficiario"].Add(new KeyValuePair<string, bool>("EstatusInformacion", true));
            response.Filtros["Beneficiario"].Add(new KeyValuePair<string, bool>("Trabajador", true));
            response.Filtros["Domicilio"].Add(new KeyValuePair<string, bool>("Municipio", true));
            response.Filtros["Domicilio"].Add(new KeyValuePair<string, bool>("Ageb", true));
            response.Filtros["Domicilio"].Add(new KeyValuePair<string, bool>("CodigoPostal", false));
            response.Filtros["Domicilio"].Add(new KeyValuePair<string, bool>("DomicilioN", false));
            response.Filtros["Domicilio"].Add(new KeyValuePair<string, bool>("Zona", true));
            response.Filtros["Localidad"].Add(new KeyValuePair<string, bool>("Localidad", true));
            response.Filtros["Localidad"].Add(new KeyValuePair<string, bool>("ZonaIndigena", true));
            response.Filtros["ZonasImpulso"].Add(new KeyValuePair<string, bool>("ZonaImpulso", true));

            response.Filtros["Visita"].Add(new KeyValuePair<string, bool>("Educativa", true));
            response.Filtros["Visita"].Add(new KeyValuePair<string, bool>("Salud", true));
            response.Filtros["Visita"].Add(new KeyValuePair<string, bool>("SeguridadSocial", true));
            response.Filtros["Visita"].Add(new KeyValuePair<string, bool>("Vivienda", true));
            response.Filtros["Visita"].Add(new KeyValuePair<string, bool>("Servicios", true));
            response.Filtros["Visita"].Add(new KeyValuePair<string, bool>("Alimentaria", true));
            response.Filtros["Visita"].Add(new KeyValuePair<string, bool>("LineaBienestar", true));
            response.Filtros["Visita"].Add(new KeyValuePair<string, bool>("NivelPobreza", true));
            if (!aplicacionesCompletas)
            {
                response.Filtros["Visita"].Add(new KeyValuePair<string, bool>("Resultado", true));
            }

            foreach (var pregunta in encuestaVersion.Preguntas.Where(p => p.Activa && p.DeletedAt == null)
                .OrderBy(p => p.Numero))
            {
                var nombre = pregunta.Nombre.ToLower();
                nombre = char.ToUpper(nombre[0]) + nombre.Substring(1);
                response.Filtros["Encuesta"].Add(new KeyValuePair<string, bool>(
                    pregunta.Numero + ".-" + nombre, pregunta.Catalogo != null || pregunta.Respuestas.Any()));
            }

            return JsonSedeshu.SerializeObject(response);
        }

        /// <summary>
        /// Funcion que consulta el listado de beneficiarios de acuerdo a los filtros seleccionados por el usuario
        /// </summary>
        /// <param name="request">Filtros seleccionados por el usuario</param>
        /// <returns>Cadena JSON con el listado de beneficiarios</returns>
        [HttpPost]
        [Authorize]
        public string ObtenerBeneficiarios([FromBody] BeneficiariosRequest request)
        {
            var response = new BeneficiariosResponse();
            var now = DateTime.Now;
            var beneficiarioId = "";
            var inicio = new DateTime(now.Year, now.Month, 1).StartOf(DateTimeAnchor.Day)
                .ToString("yyyy-MM-dd HH:mm:ss");
            var fin = new DateTime(now.Year, now.Month, DateTime.DaysInMonth(now.Year, now.Month))
                .StartOf(DateTimeAnchor.Day).ToString("yyyy-MM-dd HH:mm:ss");
            var util = new BeneficiarioCode(_context);
            response.Beneficiarios = new List<BeneficiarioShortResponse>();
            if (!string.IsNullOrEmpty(request.Inicio))
            {
                inicio = DateTime.Parse(request.Inicio.Substring(0, 10)).StartOf(DateTimeAnchor.Day)
                    .ToString("yyyy-MM-dd HH:mm:ss");
            }

            if (!string.IsNullOrEmpty(request.Fin))
            {
                fin = DateTime.Parse(request.Fin.Substring(0, 10)).EndOf(DateTimeAnchor.Day)
                    .ToString("yyyy-MM-dd HH:mm:ss");
            }

            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandTimeout = 500;
                var query = util.BuildBeneficiariosQuery(request, TipoConsulta.Domicilios,
                    false, inicio, fin, false);
                command.CommandText = query.Key;
                if (request.PageSize > 0)
                {
                    command.CommandText += " OFFSET " + ((request.PageIndex - 1) * request.PageSize) +
                                           " ROWS FETCH NEXT " + request.PageSize + " ROWS ONLY";
                }

                var iParametro = 0;
                foreach (var parametro in query.Value)
                {
                    command.Parameters.Add(new SqlParameter("@" + (iParametro++), parametro));
                }

                _context.Database.OpenConnection();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var id = reader.GetString(0);
                            if (!beneficiarioId.Equals(id))
                            {
                                beneficiarioId = id;
                                var beneficiario = new BeneficiarioShortResponse()
                                {
                                    Id = beneficiarioId,
                                    Folio = reader.IsDBNull(24) ? "" : reader.GetString(24),
                                    Nombre = reader.GetString(1),
                                    ApellidoPaterno = reader.GetString(2),
                                    ApellidoMaterno = reader.GetString(3),
                                    ZonaImpulso = reader.IsDBNull(25) ? "" : reader.GetString(25),
                                    EstatusInformacion = reader.IsDBNull(21) ? "" : reader.GetString(21),
                                    FechaRegistro = reader.GetDateTime(22).ToString(),
                                };
                                if (!reader.IsDBNull(15))
                                {
                                    beneficiario.Municipio = reader.GetString(15);
                                    beneficiario.Localidad = reader.IsDBNull(16) ? "" : reader.GetString(16);
                                }

                                if (request.PageSize == 0 && !reader.IsDBNull(27))
                                {
                                    beneficiario.Latitud = reader.GetString(27);
                                    beneficiario.Longitud = reader.GetString(28);
                                }

                                response.Beneficiarios.Add(beneficiario);
                            }
                        }
                    }
                }
            }

            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                var query = util.BuildBeneficiariosQuery(request, TipoConsulta.Domicilios,
                    false, inicio, fin, true);
                _context.Database.OpenConnection();
                command.CommandText = query.Key;
                var iParametro = 0;
                foreach (var parametro in query.Value)
                {
                    command.Parameters.Add(new SqlParameter("@" + (iParametro++), parametro));
                }

                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            response.Total = reader.GetInt32(0);
                        }
                    }
                }
            }

            response.GranTotal =
                _context.Beneficiario.Count(b => b.DeletedAt == null && b.PadreId == null && b.HijoId == null);

            return JsonSedeshu.SerializeObject(response);
        }

        private DateTime StartOfDay(DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
        }

        private DateTime EndOfDay(DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);
        }

        /// <summary>
        /// Funcion que muestra la vista donde el usuario puede consultar en un mapa los domicilio registrados desde los dispositivos moviles
        /// </summary>
        /// <returns>Vista con el mapa</returns>
        [HttpGet]
        [Authorize]
        [RequireClaim("Permiso", Value = "beneficiarios.ver")]
        public IActionResult Mapa()
        {
            var response = new ReponseBeneficiarioIndex();
            var date = DateTime.Now;
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            response.Inicio = firstDayOfMonth.ToString("yyyy-MM-dd");
            response.Fin = lastDayOfMonth.ToString("yyyy-MM-dd");
            return View("Mapa", response);
        }

        /// <summary>
        /// Funcion que muestra la vista con la informacion detallada del beneficiario seleccionado por el usuario
        /// </summary>
        /// <param name="id">Identificador en base de datos del beneficiario seleccionado por el usuario</param>
        /// <returns>Vista con el detalle de la informacion del beneficiario, datos personales, domicilio, encuesta aplicada, solicitudes registradas</returns>
        [HttpGet]
        [Authorize]
        [RequireClaim("Permiso", Value = "beneficiarios.ver")]
        public async Task<IActionResult> Ver(string id = "")
        {
            try
            {
                var response = new BeneficiarioResponse
                {
                    Beneficiario = new BeneficiarioResponseModel(),
                    Carencias = new List<AplicacionCarencia>(),
                    Domicilio = new DomicilioResponse(),
                    Solicitudes = new List<SolicitudesListModel>(),
                    Preguntas = new List<AplicacionPregunta>(),
                    Documentos = new List<ImagenModel>(),
                    Fotos = new List<ImagenModel>(),
                    Huellas = new List<Huella>(),
                    Hijos = new List<BeneficiarioResponseModel>(),
                    FamiliaId = id,
                    Asiste = new List<AplicacionPregunta>()

                };

                var beneficiario = _context.Beneficiario.Where(b => b.Id.Equals(id)).Include(b => b.Estudio)
                    .Include(b => b.GradoEstudio)
                    .Include(b => b.EstadoCivil).Include(b => b.Estado)
                    .Include(b => b.Sexo).Include(b => b.Discapacidad)
                    .Include(b => b.DiscapacidadGrado).ThenInclude(dg => dg.Grade)
                    .Include(b => b.CausaDiscapacidad)
                    .Include(b => b.Domicilio).ThenInclude(d => d.Municipio)
                    .Include(b => b.Domicilio).ThenInclude(a => a.Ageb)
                    .Include(b => b.Domicilio).ThenInclude(d => d.Localidad)
                    .Include(b => b.Domicilio).ThenInclude(d => d.Colonia)
                    .Include(b => b.Domicilio).ThenInclude(d => d.Calle)
                    .Include(b => b.Domicilio).ThenInclude(d => d.ZonaImpulso)
                    .First();
                var trabajador = _context.Trabajador.Where(t => t.Id == beneficiario.TrabajadorId)
                    .Include(t => t.TrabajadorDependencias).ThenInclude(td => td.Dependencia).FirstOrDefault();
                response.Dependencia = trabajador.TrabajadorDependencias.FirstOrDefault().Dependencia.Nombre;
                if (beneficiario.Id == null) return View("Ver", response);
                {
                    var hijos = _context.Beneficiario.Include(x => x.Domicilio)
                        .Include(h => h.Parentesco)
                        .Include(h => h.Estudio)
                        .Include(h => h.GradoEstudio)
                        .Where(b => b.PadreId.Equals(beneficiario.Id) || b.Id.Equals(beneficiario.Id))
                        .OrderBy(h => h.NumIntegrante);
                    var aplicacion =
                        _context.Aplicacion.FirstOrDefault(a => a.BeneficiarioId == beneficiario.Id && a.Activa);
                    response.NumDomicilios =
                        _context.Domicilio.Count(d => d.Id.Equals(beneficiario.DomicilioId) && d.DeletedAt == null);
                    response.NumEncuestas =
                        _context.Aplicacion.Count(a => a.BeneficiarioId.Equals(id) && a.DeletedAt == null);
                    var preguntas = new List<AplicacionPregunta>();
                    var preguntasDB = _context.Pregunta.Where(p => p.DeletedAt == null && p.Activa)
                        .Include(p => p.Respuestas).ToDictionary(p => p.Id);
                    foreach (var pregunta in preguntasDB.Values)
                    {
                        pregunta.RespuestasMap = pregunta.Respuestas.ToDictionary(r => r.Id);
                    }

                    if (aplicacion != null)
                    {
                        preguntas = _context.AplicacionPregunta
                            .Where(ap => ap.AplicacionId.Equals(aplicacion.Id) && ap.DeletedAt == null)
                            .OrderBy(ap => ap.Pregunta.Numero)
                            .ThenBy(ap => ap.RespuestaIteracion).ToList();
                        response.Aplicacion = new Aplicacion
                        {
                            Id = aplicacion.Id,
                            EncuestaVersionId = aplicacion.EncuestaVersionId,
                            Estatus = aplicacion.Estatus,
                            CreatedAt = aplicacion.CreatedAt,
                            Piso = aplicacion.Piso,
                            Techo = aplicacion.Techo,
                            Muro = aplicacion.Muro,
                            Hacinamiento = aplicacion.Hacinamiento,
                            Agua = aplicacion.Agua,
                            Drenaje = aplicacion.Drenaje,
                            Electricidad = aplicacion.Electricidad,
                            Combustible = aplicacion.Combustible,
                            PropiedadVivienda = aplicacion.PropiedadVivienda,
                            Alimentaria = aplicacion.Alimentaria,
                            Trabajador = trabajador,
                            FechaInicio = aplicacion.FechaInicio
                        };
                    }

                    response.Beneficiario.Id = beneficiario.Id;
                    response.Beneficiario.Nombre = beneficiario.Nombre;
                    response.Beneficiario.ApellidoPaterno = beneficiario.ApellidoPaterno;
                    response.Beneficiario.ApellidoMaterno = beneficiario.ApellidoMaterno;
                    response.Beneficiario.FechaNacimiento = beneficiario.FechaNacimiento.Value.ToString("d/M/yy");
                    response.Beneficiario.Estado = beneficiario.EstadoId == null ? "" : beneficiario.Estado.Nombre;
                    response.Beneficiario.Curp = beneficiario.Curp;
                    response.Beneficiario.Rfc = beneficiario.Rfc;
                    response.Beneficiario.Estudio = beneficiario.EstudioId == null ? "" : beneficiario.Estudio.Nombre;
                    response.Beneficiario.GradoEstudio = beneficiario.GradoEstudioId == null
                        ? ""
                        : beneficiario.GradoEstudio != null
                            ? beneficiario.GradoEstudio.Nombre
                            : "Sin grado de estudio";
                    response.Beneficiario.EstadoCivil =
                        beneficiario.EstadoCivilId == null ? "" : beneficiario.EstadoCivil.Nombre;
                    response.Beneficiario.Discapacidad =
                        beneficiario.DiscapacidadId == null ? "" : beneficiario.Discapacidad.Nombre;
                    response.Beneficiario.GradoDiscapacidad = beneficiario.DiscapacidadGrado != null
                        ? beneficiario.DiscapacidadGrado.Grade.Nombre
                        : "Sin grado de discapacidad";
                    response.Beneficiario.CausaDiscapacidad = beneficiario.CausaDiscapacidad != null
                        ? beneficiario.CausaDiscapacidad.Nombre
                        : "Sin causa de discapacidad";
                    response.Beneficiario.Estatus = beneficiario.Estatus ? "Activo" : "Inactivo";
                    response.Beneficiario.Comentarios = beneficiario.Comentarios;
                    response.Beneficiario.Sexo = beneficiario.SexoId == null ? "" : beneficiario.Sexo.Nombre;
                    response.Beneficiario.Activa = true;
                    response.Beneficiario.EstatusInformacion = beneficiario.EstatusInformacion;

                    if (beneficiario.Domicilio != null)
                    {
                        var domicilio = beneficiario.Domicilio;
                        response.Domicilio.Id = domicilio.Id;
                        response.Domicilio.Domicilio = domicilio.DomicilioN;
                        response.Domicilio.Email = domicilio.Email;
                        response.Domicilio.Latitud = domicilio.Latitud;
                        response.Domicilio.Longitud = domicilio.Longitud;
                        response.Domicilio.LatitudAtm = domicilio.LatitudAtm;
                        response.Domicilio.LongitudAtm = domicilio.LongitudAtm;
                        response.Domicilio.Localidad = domicilio.Localidad.Nombre;
                        response.Domicilio.Municipio = domicilio.Localidad.Municipio.Nombre;
                        response.Domicilio.Ageb = domicilio.Ageb != null ? domicilio.Ageb.Clave : "";
                        response.Domicilio.Telefono = domicilio.Telefono;
                        response.Domicilio.TelefonoCasa = domicilio.TelefonoCasa;
                        response.Domicilio.CodigoPostal = domicilio.CodigoPostal;
                        response.Domicilio.EntreCalle1 = domicilio.EntreCalle1;
                        response.Domicilio.EntreCalle2 = domicilio.EntreCalle2;
                        response.Domicilio.Calle = domicilio.CalleId == null ? "" : domicilio.Calle.Nombre;
                        response.Domicilio.ZonaImpulso =
                            domicilio.ZonaImpulsoId == null ? "" : domicilio.ZonaImpulso.Nombre;
                        response.Domicilio.Activa = domicilio.Activa;
                        response.Domicilio.TipoAsentamientoId = domicilio.TipoAsentamientoId;
                        response.Domicilio.Indice = domicilio.Municipio.Indice;
                        response.Domicilio.NombreAsentamiento = domicilio.NombreAsentamiento;
                        response.Domicilio.CadenaOCR = !string.IsNullOrEmpty(domicilio.CadenaOCR)
                            ? domicilio.CadenaOCR
                            : "Texto no encontrado.";
                        response.Domicilio.Porcentaje =
                            !string.IsNullOrEmpty(domicilio.Porcentaje) ? domicilio.Porcentaje : "0";
                        response.Domicilio.IndiceDesarrolloHumano = domicilio.IndiceDesarrolloHumano ?? "";
                        response.Domicilio.MarginacionLocalidad = domicilio.MarginacionLocalidad ?? "";
                        response.Domicilio.MarginacionAgeb = domicilio.MarginacionAgeb ?? "";

                        var zona = _context.Zona.Join(
                            _context.MunicipioZona, z => z.Id, mz => mz.ZonaId, (z, mz) =>
                                new
                                {
                                    Z = z,
                                    MZ = mz
                                }).FirstOrDefault(z =>
                            z.MZ.MunicipioId == domicilio.MunicipioId && z.Z.DependenciaId ==
                            trabajador.TrabajadorDependencias.FirstOrDefault().DependenciaId);
                        if (zona != null)
                        {
                            response.Domicilio.Region = zona.Z.Clave;
                        }
                    }

                    foreach (var pregunta in preguntas)
                    {
                        var item = new AplicacionPregunta();
                        if (!preguntasDB.ContainsKey(pregunta.PreguntaId))
                        {
                            continue;
                        }

                        var preguntaDB = preguntasDB[pregunta.PreguntaId];
                        item.Id = preguntaDB.Numero +
                                  (preguntaDB.Iterable ? ("." + (pregunta.RespuestaIteracion + 1) + ".- ") : ".- ") +
                                  preguntaDB.Nombre;
                        switch (preguntaDB.TipoPregunta)
                        {
                            case TipoPreguntaString.Fecha:
                            case TipoPreguntaString.FechaPasada:
                            case TipoPreguntaString.FechaFutura:
                                item.Valor = pregunta.ValorFecha.HasValue
                                    ? pregunta.ValorFecha.Value.ToString("dd/MM/yyy", CultureInfo.InvariantCulture)
                                    : "";
                                break;
                            case TipoPreguntaString.Check:
                            case TipoPreguntaString.Radio:
                            case TipoPreguntaString.Listado:
                                if (preguntaDB.Catalogo == null)
                                {
                                    item.Valor = pregunta.RespuestaId == null
                                        ? pregunta.Valor
                                        : (preguntaDB.RespuestasMap.ContainsKey(pregunta.RespuestaId)
                                            ? preguntaDB.RespuestasMap[pregunta.RespuestaId].Nombre
                                            : "");
                                }
                                else
                                {
                                    var catalogo = Utils.BuildCatalogo(_context, preguntaDB.Catalogo)
                                        .ToDictionary(m => m.Id);
                                    item.Valor = catalogo[pregunta.ValorCatalogo].Nombre;
                                }

                                break;
                            case TipoPreguntaString.Abierta:
                            case TipoPreguntaString.Numerica:
                                item.Valor = pregunta.Valor;
                                break;
                        }

                        if (!string.IsNullOrEmpty(pregunta.Complemento))
                        {
                            item.Valor += " " + item.Complemento;
                        }

                        item.RespuestaValor = pregunta.RespuestaValor;
                        response.Preguntas.Add(item);
                    }

                    var archivosBeneficiario = _context.Archivo
                        .Where
                        (
                            x => x.DeletedAt == null &&
                                    (  /***     Cambio ALEX     ***/
                                        x.Nombre == ("firma" + beneficiario.Id) ||
                                        x.Nombre == (beneficiario.DomicilioId + "_Foto1") ||
                                        x.Nombre == (beneficiario.DomicilioId + "_Foto2") ||
                                        x.Nombre == (beneficiario.DomicilioId + "_Foto3") ||
                                        x.Nombre == (beneficiario.DomicilioId + "_Foto4") ||
                                        x.Nombre == (beneficiario.DomicilioId + "_Domicilio")
                                    )
                        /***     ORIGINAL     ***/
                        //x.Nombre.Contains(beneficiario.DomicilioId) &&
                        //!x.Nombre.Contains("dedo:")
                        )
                        .Select(a => new { a.Id, a.Nombre }).ToList();


                    foreach (var archivo in archivosBeneficiario)
                    {
                        response.Documentos.Add(new ImagenModel
                        {
                            Id = archivo.Id,
                            Nombre = archivo.Nombre.Replace(beneficiario.Id, "").Replace("_", " ").Trim(),
                            Show = false
                        });
                    }

                    var cesion = _context.Archivo
                        .FirstOrDefault(x => x.DeletedAt == null && x.Nombre.Equals(aplicacion.Id + "_Diagnostico"));
                    if (cesion != null)
                    {
                        response.Documentos.Add(new ImagenModel
                        {
                            Id = cesion.Id,
                            Nombre = "Cesión de datos",
                            Show = false
                        });
                    }

                    if (beneficiario.Huellas != null)
                    {
                        response.Huellas = JsonConvert.DeserializeObject<List<Huella>>(beneficiario.Huellas);
                        foreach (var huella in response.Huellas)
                        {
                            huella.nombre = BeneficiarioCode.GetNombreDedo(huella.dedo);
                        }
                    }

                    var conexion = _configuration.GetConnectionString("DefaultConnection");

                    foreach (var hijo in hijos)
                    {
                        var foto = _context.Archivo
                            .FirstOrDefault(x => x.DeletedAt == null && x.Nombre.Equals("foto_perfil" + hijo.Id));
                        if (foto != null)
                        {
                            response.Fotos.Add(new ImagenModel
                            {
                                Id = foto.Id,
                                Nombre = hijo.Nombre + " " + hijo.ApellidoPaterno + " " + hijo.ApellidoMaterno,
                                Show = false
                            });
                        }

                        response.Hijos.Add(new BeneficiarioResponseModel
                        {
                            Id = hijo.Id,
                            Nombre = hijo.Nombre,
                            NumIntegrante = hijo.NumIntegrante,
                            Curp = hijo.Curp,
                            ApellidoPaterno = hijo.ApellidoPaterno,
                            ApellidoMaterno = hijo.ApellidoMaterno,
                            MismoDomicilio = hijo.MismoDomicilio,
                            Estudio = hijo.Estudio?.Nombre,
                            GradoEstudio = hijo.GradoEstudio?.Nombre,
                            Parentesco = hijo.Parentesco?.Nombre,
                            DomicilioString = hijo.Domicilio != null
                                ? hijo.Domicilio.DomicilioN
                                : (hijo.MismoDomicilio ? "Mismo domicilio que el beneficiario" : "Sin domicilio")
                        });

                        if (string.IsNullOrEmpty(hijo.Curp))
                        {
                            var url = _configuration["WS_DATOS"];
                            var password = _configuration["CA_WS_CURP"];
                            var idApiCurp = _configuration["ID_WS_CURP"];
                            new Thread(async () =>
                            {
                                await ApiController.CompletarCurp(beneficiario.Id, conexion, url, password,
                                    _clientFactory.CreateClient(), idApiCurp);
                            }).Start();
                        }
                    }

                    if (aplicacion != null)
                    {
                        var numIntegrantesCarencia =
                            _context.AplicacionCarencia.Count(ac => ac.AplicacionId == aplicacion.Id);
                        if (aplicacion.Estatus.Equals(EstatusAplicacion.COMPLETO) &&
                            (aplicacion.NivelPobreza == null || numIntegrantesCarencia < hijos.Count()))
                        {
                            var api = new ApiController(_context, _configuration, _userManager, _clientFactory);
                            var respuestas = _context.AplicacionPregunta.Where(ap => ap.AplicacionId == aplicacion.Id &&
                                ap.PreguntaId.Equals("42") && ap.Grado != null && !ap.Grado.Equals("1")).ToList();
                            var respuestasApiModel = new List<AplicacionPreguntaApiModel>();
                            foreach (var respuesta in respuestas)
                            {
                                respuestasApiModel.Add(new AplicacionPreguntaApiModel
                                {
                                    Id = respuesta.Id,
                                    RespuestaIteracion = respuesta.RespuestaIteracion,
                                    RespuestaId = respuesta.RespuestaId,
                                    Grado = respuesta.Grado,
                                    Complemento = respuesta.Complemento,
                                    AplicacionId = respuesta.AplicacionId,
                                    PreguntaId = respuesta.PreguntaId,
                                    Valor = respuesta.Valor,
                                    CreatedAt = respuesta.CreatedAt.ToString("yyyy-MM-dd"),
                                    UpdatedAt = respuesta.UpdatedAt.ToString("yyyy-MM-dd")
                                });
                            }

                            api.RegistrarDiscapacidades(aplicacion.BeneficiarioId, respuestasApiModel);
                            if (aplicacion.NivelPobreza == null || numIntegrantesCarencia < hijos.Count())
                            {
                                //Vamos a calcular las carencias sociales desde el web ya que en la aplicacion movil no se hizo
                                new Coneval().CalcularCarencias(aplicacion.Id, response.Domicilio.TipoAsentamientoId,
                                    response.Domicilio.Indice,
                                    _context, null);
                            }

                            if (aplicacion.PreocupacionCovid == 0
                            ) //Vamos a complementar el calculo de carencias de acuerdo a las nuevas variables Preocupacion covid y perdida de empleo
                            {
                                new Coneval().CalcularCarenciasCOVID(response.Aplicacion, response.Carencias, _context);
                            }
                        }

                        var preguntas_ = _context.AplicacionPregunta.Where(p => p.AplicacionId == aplicacion.Id && p.PreguntaId == "29").ToList();

                        foreach (var ap in preguntas_)
                        {
                            response.Asiste.Add(new AplicacionPregunta
                            {
                                NumIntegrante = ap.NumIntegrante,
                                Id = ap.Id,
                                PreguntaId = ap.PreguntaId,
                                RespuestaId = ap.RespuestaId

                            });
                        }

                        var carencias = _context.AplicacionCarencia.Where(ap => ap.AplicacionId.Equals(aplicacion.Id))
                            .OrderBy(ap => ap.NumIntegrante).ToList();
                        foreach (var ac in carencias)
                        {
                            response.Carencias.Add(new AplicacionCarencia
                            {
                                NumIntegrante = ac.NumIntegrante,
                                Id = ac.Id,
                                Educativa = ac.Educativa,
                                ServicioSalud = ac.ServicioSalud,
                                SeguridadSocial = ac.SeguridadSocial,
                                Alimentaria = ac.Alimentaria,
                                Piso = ac.Piso,
                                Techo = ac.Techo,
                                Muro = ac.Muro,
                                Hacinamiento = ac.Hacinamiento,
                                Agua = ac.Agua,
                                Drenaje = ac.Drenaje,
                                Electricidad = ac.Electricidad,
                                Combustible = ac.Combustible,
                                Edad = ac.Edad,
                                NumCarencias = (ac.Educativa ? 1 : 0) + (ac.ServicioSalud ? 1 : 0)
                                                                      + (ac.SeguridadSocial ? 1 : 0)
                            });
                        }
                    }

                    if (string.IsNullOrEmpty(beneficiario.Domicilio.MunicipioCalculado))
                    {
                        var url = _configuration["WS_DIRECCION"];
                        var client = _clientFactory.CreateClient();
                        var domicilio = _context.Domicilio.FirstOrDefault(d => d.Id == beneficiario.DomicilioId);
                        new Thread(async () =>
                        {
                            await ImportacionDiagnosticos.CompletarDomicilio(domicilio, url, client);
                        }).Start();
                    }
                }
                response.VersionEncuestaActual =
                    _context.EncuestaVersion.FirstOrDefault(ev => ev.DeletedAt == null && ev.Activa).Id;
                return View("Ver", response);
            }
            catch (Exception ex)
            {
                return View("Ver");
            }
        }

        /// <summary>
        /// Funcion que exporta a Excel el listado de beneficiarios consultado por el usuario de acuerdo a los filtros seleccionados
        /// </summary>
        /// <param name="request">Filtros seleccionados por el usuario</param>
        /// <returns>Archivo de excel con el listado de beneficiarios encontrados en la base de datos</returns>
        [HttpPost]
        [Authorize]
        public IActionResult ExportarExcel(BeneficiariosRequest request)
        {
            var now = DateTime.Now;
            var inicio = new DateTime(now.Year, now.Month, 1).StartOf(DateTimeAnchor.Day)
                .ToString("yyyy-MM-dd HH:mm:ss");
            var fin = new DateTime(now.Year, now.Month, DateTime.DaysInMonth(now.Year, now.Month))
                .StartOf(DateTimeAnchor.Day).ToString("yyyy-MM-dd HH:mm:ss");
            if (!string.IsNullOrEmpty(request.Inicio))
            {
                inicio = DateTime.Parse(request.Inicio.Substring(0, 10)).StartOf(DateTimeAnchor.Day)
                    .ToString("yyyy-MM-dd HH:mm:ss");
            }

            if (!string.IsNullOrEmpty(request.Fin))
            {
                fin = DateTime.Parse(request.Fin.Substring(0, 10)).EndOf(DateTimeAnchor.Day)
                    .ToString("yyyy-MM-dd HH:mm:ss");
            }

            try
            {
                return ExportarBeneficiarios(request, inicio, fin);
            }
            catch (Exception e)
            {
                Excepcion.Registrar(e);
                throw;
            }
        }

        private IActionResult ExportarBeneficiarios(BeneficiariosRequest request, string inicio, string fin)
        {
            using (var excel = new ExcelPackage())
            {
                excel.Workbook.Worksheets.Add("beneficiarios");
                var pestana = excel.Workbook.Worksheets.First();
                var fila = 1;
                pestana.Cells[fila, 1].Value = "Familia";
                pestana.Cells[fila, 2].Value = "Fecha de registro";
                pestana.Cells[fila, 3].Value = "Folio";
                pestana.Cells[fila, 4].Value = "Numero de integrante";
                pestana.Cells[fila, 5].Value = "Nombre";
                pestana.Cells[fila, 6].Value = "Primer apellido";
                pestana.Cells[fila, 7].Value = "Segundo apellido";
                pestana.Cells[fila, 8].Value = "Parentesco";
                pestana.Cells[fila, 9].Value = "Fecha de nacimiento";
                pestana.Cells[fila, 10].Value = "Estado de nacimiento";
                pestana.Cells[fila, 11].Value = "Sexo";
                pestana.Cells[fila, 12].Value = "CURP";
                pestana.Cells[fila, 13].Value = "RFC";
                pestana.Cells[fila, 14].Value = "Nivel de estudios";
                pestana.Cells[fila, 15].Value = "Grado de estudios";
                pestana.Cells[fila, 16].Value = "Estado civil";
                pestana.Cells[fila, 17].Value = "Telefono";
                pestana.Cells[fila, 18].Value = "Telefono casa";
                pestana.Cells[fila, 19].Value = "Domicilio";
                pestana.Cells[fila, 20].Value = "Exterior";
                pestana.Cells[fila, 21].Value = "Interior";
                pestana.Cells[fila, 22].Value = "Entre calle 1";
                pestana.Cells[fila, 23].Value = "Entre calle 2";
                pestana.Cells[fila, 24].Value = "Calle posterior";
                pestana.Cells[fila, 25].Value = "Codigo postal";
                pestana.Cells[fila, 26].Value = "Municipio";
                pestana.Cells[fila, 27].Value = "Localidad";
                pestana.Cells[fila, 28].Value = "AGEB";
                pestana.Cells[fila, 29].Value = "Manzana";
                pestana.Cells[fila, 30].Value = "Colonia";
                pestana.Cells[fila, 31].Value = "Calle";
                pestana.Cells[fila, 32].Value = "Zona de impulso";
                pestana.Cells[fila, 33].Value = "Carretera";
                pestana.Cells[fila, 34].Value = "Camino";
                pestana.Cells[fila, 35].Value = "Tipo de asentamiento";
                pestana.Cells[fila, 36].Value = "Nombre de asentamiento";
                pestana.Cells[fila, 37].Value = "Latitud";
                pestana.Cells[fila, 38].Value = "Longitud";
                pestana.Cells[fila, 39].Value = "Zap";
                pestana.Cells[fila, 40].Value = "Tipo de zap";
                pestana.Cells[fila, 41].Value = "Cis cercano";
                pestana.Cells[fila, 42].Value = "ClaveMunicipioCisCalculado";
                pestana.Cells[fila, 43].Value = "DomicilioCisCalculado";
                pestana.Cells[fila, 44].Value = "TipoCalculo";
                pestana.Cells[fila, 45].Value = "IndiceDesarrolloHumano";
                pestana.Cells[fila, 46].Value = "MarginacionMunicipio";
                pestana.Cells[fila, 47].Value = "MarginacionLocalidad";
                pestana.Cells[fila, 48].Value = "MarginacionAgeb";
                pestana.Cells[fila, 49].Value = "Estatus";
                pestana.Cells[fila, 50].Value = "Encuestador";
                pestana.Cells[fila, 51].Value = "1116";
                pestana.Cells[fila, 52].Value = "1125";
                pestana.Cells[fila, 53].Value = "1126";
                pestana.Cells[fila, 54].Value = "1127";
                pestana.Cells[fila++, 55].Value = "1128";

                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    var util = new BeneficiarioCode(_context);
                    var query = util.BuildBeneficiariosQuery(request, TipoConsulta.Exportar,
                        request.Hijos, inicio, fin, false);
                    command.CommandText = query.Key;
                    var iParametro = 0;
                    foreach (var parametro in query.Value)
                    {
                        command.Parameters.Add(new SqlParameter("@" + (iParametro++), parametro));
                    }

                    var beneficiarioId = "";
                    var folio = "";
                    _context.Database.OpenConnection();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                if (reader.GetInt32(3) == 1)
                                {
                                    beneficiarioId = reader.GetString(0);
                                    folio = reader.GetString(2);
                                }

                                pestana.Cells[fila, 1].Value = beneficiarioId;
                                pestana.Cells[fila, 2].Value = reader.GetDateTime(1).ToString();
                                pestana.Cells[fila, 3].Value = folio;
                                pestana.Cells[fila, 4].Value = reader.GetInt32(3);
                                pestana.Cells[fila, 5].Value = reader.IsDBNull(4) ? "" : reader.GetString(4);
                                pestana.Cells[fila, 6].Value = reader.IsDBNull(5) ? "" : reader.GetString(5);
                                pestana.Cells[fila, 7].Value = reader.IsDBNull(6) ? "" : reader.GetString(6);
                                pestana.Cells[fila, 8].Value = reader.IsDBNull(7) ? "" : reader.GetString(7);
                                pestana.Cells[fila, 9].Value =
                                    reader.IsDBNull(8) ? "" : reader.GetDateTime(8).ToString();
                                pestana.Cells[fila, 10].Value = reader.IsDBNull(9) ? "" : reader.GetString(9);
                                pestana.Cells[fila, 11].Value = reader.IsDBNull(10) ? "" : reader.GetString(10);
                                pestana.Cells[fila, 12].Value = reader.IsDBNull(11) ? "" : reader.GetString(11);
                                pestana.Cells[fila, 13].Value = reader.IsDBNull(12) ? "" : reader.GetString(12);
                                pestana.Cells[fila, 14].Value = reader.IsDBNull(13) ? "" : reader.GetString(13);
                                pestana.Cells[fila, 15].Value = reader.IsDBNull(14) ? "" : reader.GetString(14);
                                pestana.Cells[fila, 16].Value = reader.IsDBNull(15) ? "" : reader.GetString(15);
                                pestana.Cells[fila, 17].Value = reader.IsDBNull(16) ? "" : reader.GetString(16);
                                pestana.Cells[fila, 18].Value = reader.IsDBNull(17) ? "" : reader.GetString(17);
                                pestana.Cells[fila, 19].Value = reader.IsDBNull(18) ? "" : reader.GetString(18);
                                pestana.Cells[fila, 20].Value = reader.IsDBNull(19) ? "" : reader.GetString(19);
                                pestana.Cells[fila, 21].Value = reader.IsDBNull(20) ? "" : reader.GetString(20);
                                pestana.Cells[fila, 22].Value = reader.IsDBNull(21) ? "" : reader.GetString(21);
                                pestana.Cells[fila, 23].Value = reader.IsDBNull(22) ? "" : reader.GetString(22);
                                pestana.Cells[fila, 24].Value = reader.IsDBNull(23) ? "" : reader.GetString(23);
                                pestana.Cells[fila, 25].Value = reader.IsDBNull(24) ? "" : reader.GetString(24);
                                pestana.Cells[fila, 26].Value = reader.IsDBNull(25) ? "" : reader.GetString(25);
                                pestana.Cells[fila, 27].Value = reader.IsDBNull(26) ? "" : reader.GetString(26);
                                pestana.Cells[fila, 28].Value = reader.IsDBNull(27) ? "" : reader.GetString(27);
                                pestana.Cells[fila, 29].Value = reader.IsDBNull(28) ? "" : reader.GetString(28);
                                pestana.Cells[fila, 30].Value = reader.IsDBNull(29) ? "" : reader.GetString(29);
                                pestana.Cells[fila, 31].Value = reader.IsDBNull(30) ? "" : reader.GetString(30);
                                pestana.Cells[fila, 32].Value = reader.IsDBNull(31) ? "" : reader.GetString(31);
                                pestana.Cells[fila, 33].Value = reader.IsDBNull(32) ? "" : reader.GetString(32);
                                pestana.Cells[fila, 34].Value = reader.IsDBNull(33) ? "" : reader.GetString(33);
                                pestana.Cells[fila, 35].Value = reader.IsDBNull(33) ? "" : reader.GetString(33);
                                pestana.Cells[fila, 36].Value = reader.IsDBNull(35) ? "" : reader.GetString(35);
                                pestana.Cells[fila, 37].Value = reader.IsDBNull(36) ? "" : reader.GetString(36);
                                pestana.Cells[fila, 38].Value = reader.IsDBNull(37) ? "" : reader.GetString(37);
                                pestana.Cells[fila, 39].Value = reader.IsDBNull(38) ? "" : reader.GetString(38);
                                pestana.Cells[fila, 40].Value = reader.IsDBNull(39) ? "" : reader.GetString(39);
                                pestana.Cells[fila, 41].Value = reader.IsDBNull(40) ? "" : reader.GetString(40);
                                pestana.Cells[fila, 42].Value = reader.IsDBNull(41) ? "" : reader.GetString(41);
                                pestana.Cells[fila, 43].Value = reader.IsDBNull(42) ? "" : reader.GetString(42);
                                pestana.Cells[fila, 44].Value = reader.IsDBNull(43) ? "" : reader.GetString(43);
                                pestana.Cells[fila, 45].Value = reader.IsDBNull(44) ? "" : reader.GetString(44);
                                pestana.Cells[fila, 46].Value = reader.IsDBNull(45) ? "" : reader.GetString(45);
                                pestana.Cells[fila, 47].Value = reader.IsDBNull(46) ? "" : reader.GetString(46);
                                pestana.Cells[fila, 48].Value = reader.IsDBNull(47) ? "" : reader.GetString(47);
                                pestana.Cells[fila, 49].Value = reader.IsDBNull(48) ? "" : reader.GetString(48);
                                pestana.Cells[fila, 50].Value = reader.IsDBNull(52) ? "" : reader.GetString(52);
                                pestana.Cells[fila, 51].Value = reader.IsDBNull(53) ? "" : reader.GetString(53);
                                pestana.Cells[fila, 52].Value = reader.IsDBNull(54) ? "" : reader.GetString(54);
                                pestana.Cells[fila, 53].Value = reader.IsDBNull(55) ? "" : reader.GetString(55);
                                pestana.Cells[fila, 54].Value = reader.IsDBNull(56) ? "" : reader.GetString(56);
                                pestana.Cells[fila++, 55].Value = reader.IsDBNull(57) ? "" : reader.GetString(57);
                            }
                        }
                    }
                }

                _context.Bitacora.Add(new Bitacora
                {
                    UsuarioId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                    Accion = AccionBitacora.EXPORTACION,
                    Mensaje = "Se exportó el listado de domicilios.",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                });
                _context.SaveChanges();

                return File(
                    fileContents: excel.GetAsByteArray(),
                    contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    fileDownloadName: "domicilios.xlsx"
                );
            }
        }

        /// <summary>
        /// Funcion que consulta en la base de datos el historico de cambios hechos en la informacion del beneficiario
        /// </summary>
        /// <param name="request">Objeto que contiene el numero de version a consultar en la base de datos</param>
        /// <returns>Cadena JSON con la informacion de los datos personales del beneficiario</returns>
        [HttpPost]
        [Authorize]
        public string VerDatos([FromBody] VerDatosRequest request)
        {
            var integrante = request.Numero == 1
                ? _context.Beneficiario.FirstOrDefault(d => d.Id == request.BeneficiarioId && d.DeletedAt == null)
                : _context.Beneficiario.FirstOrDefault(d =>
                    d.PadreId == request.BeneficiarioId && d.DeletedAt == null && d.NumIntegrante == request.Numero);

            var response = new BeneficiarioEditModel
            {
                Id = integrante.Id,
                Nombre = integrante.Nombre,
                ApellidoPaterno = integrante.ApellidoPaterno,
                ApellidoMaterno = integrante.ApellidoMaterno,
                FechaNacimiento = integrante.FechaNacimiento.Value,
                EstadoId = integrante.EstadoId,
                Curp = integrante.Curp ?? "",
                Rfc = integrante.Rfc ?? "",
                EstudioId = integrante.EstudioId,
                GradoEstudioId = integrante.GradoEstudioId,
                EstadoCivilId = integrante.EstadoCivilId,
                SexoId = integrante.SexoId,
                DiscapacidadId = integrante.DiscapacidadId,
                DiscapacidadGradoId = integrante.DiscapacidadGradoId,
                CausaDiscapacidadId = integrante.CausaDiscapacidadId,
                Comentarios = integrante.Comentarios,
                Estatus = integrante.Estatus
            };
            return JsonSedeshu.SerializeObject(response);
        }

        /// <summary>
        /// Funcion que muestra las respuestas de la encuesta coneval en le domicilio del beneficiario seleccionado por el usuario
        /// </summary>
        /// <param name="request">Numero de encuesta aplicada</param>
        /// <returns>Cadena JSON con las respuestas contestadas por el beneficiario</returns>
        [HttpPost]
        [Authorize]
        public string VerEncuesta([FromBody] VerDatosRequest request)
        {
            var response = new BeneficiarioResponse
            {
                Preguntas = new List<AplicacionPregunta>(),
                Aplicacion = new Aplicacion()
            };
            var aplicaciones = _context.Aplicacion.Where(a =>
                    a.BeneficiarioId.Equals(request.BeneficiarioId) && a.DeletedAt == null)
                .OrderByDescending(a => a.Activa)
                .ThenByDescending(a => a.CreatedAt).ToList();
            var aplicacion = aplicaciones[request.Numero];
            var preguntas = _context.AplicacionPregunta.Where(ap => ap.AplicacionId.Equals(aplicacion.Id))
                .Include(ap => ap.Pregunta).OrderBy(ap => ap.Pregunta.Numero).ToList();

            response.Aplicacion.Id = aplicacion.Id;
            response.Aplicacion.Estatus = aplicacion.Estatus;

            foreach (var pregunta in preguntas)
            {
                var item = new AplicacionPregunta
                {
                    Id = pregunta.Pregunta.Numero + " " + pregunta.Pregunta.Nombre,
                    Valor = pregunta.Pregunta.TipoPregunta == "Fecha"
                        ? DateTime.ParseExact(pregunta.Valor, "yyyy/MM/dd", CultureInfo.InvariantCulture)
                            .ToString("dd/MM/yyy", CultureInfo.InvariantCulture)
                        : pregunta.Valor,
                    RespuestaValor = pregunta.RespuestaValor
                };
                response.Preguntas.Add(item);
            }

            return JsonSedeshu.SerializeObject(response);
        }

        /// <summary>
        /// Funcion que muestra las carencias de la encuesta contestada por el beneficiario
        /// </summary>
        /// <param name="request">Objeto que contiene la encuesta aplicada al beneficiario</param>
        /// <returns>Cadena JSON con las carencias calculadas por en jar coneval.jar</returns>
        [HttpPost]
        [Authorize]
        public string VerCarencias([FromBody] VerDatosRequest request)
        {
            var response = new BeneficiarioResponse
            {
                Carencias = new List<AplicacionCarencia>()
            };
            var carenciasDb = _context.Carencia.ToDictionary(c => c.Clave);
            var aplicacion = _context.Aplicacion.Find(request.BeneficiarioId);
            // if (aplicacion.Carencias != null)
            // {
            //     var carencias = JsonConvert.DeserializeObject<CarenciasJson>(aplicacion.Carencias);
            //     foreach (var carencia in carenciasDb)
            //     {
            //         var property = carencias.GetType().GetProperty(carencia.Value.Clave);
            //         bool r = (bool) property.GetValue(carencias);
            //         response.Carencias.Add(item: new Carencia()
            //         {
            //             Clave = carencia.Value.Clave,
            //             Nombre = carencia.Value.Nombre,
            //             Carente = r
            //         });
            //     }
            // }

            return JsonSedeshu.SerializeObject(response);
        }

        /// <summary>
        /// Funcion que muestra la vista para la edicion de los datos personales del beneficiario, esto generara una
        /// nueva version en el historico de los datos del beneficiario
        /// </summary>
        /// <param name="id">Identificador del beneficiario</param>
        /// <returns>Vista para la edicion de los datos del beneficiario</returns>
        [HttpGet]
        [Route("Beneficiarios/Edit/{id}/{numintegrante}")]
        [Authorize]
        [RequireClaim("Permiso", Value = "beneficiarios.editar")]
        public IActionResult Edit(string id, int numintegrante = 1)
        {
            var response = new BeneficiarioEdit
            {
                Beneficiario = new BeneficiarioEditModel(),
                Estados = new List<Estado>(),
                Estudios = new List<Estudio>(),
                GradosEstudio = new List<GradosEstudio>(),
                EstadosCiviles = new List<EstadoCivil>(),
                Discapacidades = new List<Discapacidad>(),
                CausaDiscapacidad = new List<CausaDiscapacidad>(),
                Sexos = new List<Sexo>(),
                FamiliaId = id
            };
            _context.Estado.OrderBy(e => e.Clave).ToList().ForEach(e =>
                response.Estados.Add(new Estado() {Id = e.Id, Nombre = e.Nombre, Abreviacion = e.Abreviacion}));
            _context.Estudio.OrderBy(e => e.Nombre).ToList()
                .ForEach(e => response.Estudios.Add(new Estudio() {Id = e.Id, Nombre = e.Nombre}));
            _context.GradosEstudio.OrderBy(e => e.Nombre).ToList()
                .ForEach(e => response.GradosEstudio.Add(new GradosEstudio() {Id = e.Id, Nombre = e.Nombre}));
            _context.EstadoCivil.OrderBy(ec => ec.Nombre).ToList().ForEach(e =>
                response.EstadosCiviles.Add(new EstadoCivil() {Id = e.Id, Nombre = e.Nombre}));
            _context.Discapacidad.OrderBy(d => d.Nombre).ToList().ForEach(e =>
                response.Discapacidades.Add(new Discapacidad() {Id = e.Id, Nombre = e.Nombre}));
            _context.CausaDiscapacidad.OrderBy(d => d.Nombre).ToList().ForEach(e =>
                response.CausaDiscapacidad.Add(new CausaDiscapacidad() {Id = e.Id, Nombre = e.Nombre}));
            _context.Sexo.OrderBy(s => s.Nombre).ToList()
                .ForEach(e => response.Sexos.Add(new Sexo() {Id = e.Id, Nombre = e.Nombre}));

            var beneficiario = numintegrante == 1
                ? _context.Beneficiario.Find(id)
                : _context.Beneficiario.FirstOrDefault(b =>
                    b.DeletedAt == null && b.PadreId == id && b.NumIntegrante == numintegrante);

            response.Beneficiario.Id = beneficiario.Id;
            response.Beneficiario.Nombre = beneficiario.Nombre;
            response.Beneficiario.ApellidoPaterno = beneficiario.ApellidoPaterno;
            response.Beneficiario.ApellidoMaterno = beneficiario.ApellidoMaterno;
            response.Beneficiario.FechaNacimiento = beneficiario.FechaNacimiento.Value;
            response.Beneficiario.EstadoId = beneficiario.EstadoId;
            response.Beneficiario.Curp = beneficiario.Curp ?? "";
            response.Beneficiario.Rfc = beneficiario.Rfc ?? "";
            response.Beneficiario.EstudioId = beneficiario.EstudioId;
            response.Beneficiario.GradoEstudioId = beneficiario.GradoEstudioId;
            response.Beneficiario.EstadoCivilId = beneficiario.EstadoCivilId;
            response.Beneficiario.SexoId = beneficiario.SexoId;
            response.Beneficiario.DiscapacidadId = beneficiario.DiscapacidadId;
            response.Beneficiario.DiscapacidadGradoId = beneficiario.DiscapacidadGradoId;
            response.Beneficiario.CausaDiscapacidadId = beneficiario.CausaDiscapacidadId;
            response.Beneficiario.Comentarios = beneficiario.Comentarios;
            response.Beneficiario.Estatus = beneficiario.Estatus;
            response.Beneficiario.PadreId = beneficiario.PadreId;
            response.Beneficiario.NumIntegrante = beneficiario.NumIntegrante;
            response.Beneficiario.NumDatos =
                _context.BeneficiarioHistorico.Count(bh =>
                    bh.DeletedAt == null && bh.BeneficiarioId == beneficiario.Id);

            return View("Edit", response);
        }

        [HttpPost]
        [Authorize]
        public string ObtenerGradoDiscapacidad([FromBody] string id = "")
        {
            var gradosResponse = new List<GradosModel>();
            var Grados = _context.DiscapacidadGrado.Where(x => x.DiscapacidadId.Equals(id)).Include(x => x.Grade)
                .ToList();
            foreach (var grado in Grados)
            {
                gradosResponse.Add(new GradosModel()
                {
                    DiscapacidadGradoId = grado.Id,
                    GradoId = grado.GradoId,
                    Nombre = grado.Grade.Nombre
                });
            }

            return JsonConvert.SerializeObject(gradosResponse);
        }

        /// <summary>
        /// Funcion que guarda en la base de datos la informacion del beneficiario modificada por el usuario admnistrador
        /// </summary>
        /// <param name="request">Datos nuevos del beneficiario</param>
        /// <returns>Estatus del guardado en la base de datos</returns>
        [HttpPost]
        [Authorize]
        public string Guardar([FromBody] BeneficiarioEditModel request)
        {
            var beneficiario = _context.Beneficiario.Find(request.Id);
            if (beneficiario == null)
            {
                var beneficiarioHistorico = _context.BeneficiarioHistorico.Find(request.Id);
                beneficiario = _context.Beneficiario.Find(beneficiarioHistorico.BeneficiarioId);
            }

            if (!ModelState.IsValid)
            {
                var errores = ModelState.Select(x => new {x.Key, Error = x.Value.Errors.First().ErrorMessage})
                    .ToList();

                return JsonConvert.SerializeObject(errores);
            }

            using (var transaction = _context.Database.BeginTransaction())
            {
                var now = DateTime.Now;
                var historico = new BeneficiarioHistorico
                {
                    Curp = beneficiario.Curp,
                    ApellidoPaterno = beneficiario.ApellidoPaterno,
                    ApellidoMaterno = beneficiario.ApellidoMaterno,
                    Nombre = beneficiario.Nombre,
                    FechaNacimiento = StartOfDay(beneficiario.FechaNacimiento ?? DateTime.Now),
                    Rfc = beneficiario.Rfc,
                    Comentarios = beneficiario.Comentarios,
                    BeneficiarioId = beneficiario.Id,
                    EstadoId = beneficiario.EstadoId,
                    GradoEstudioId = beneficiario.GradoEstudioId,
                    EstudioId = beneficiario.EstudioId,
                    EstadoCivilId = beneficiario.EstadoCivilId,
                    DiscapacidadId = beneficiario.DiscapacidadId,
                    DiscapacidadGradoId = beneficiario.DiscapacidadGradoId,
                    CausaDiscapacidadId = beneficiario.CausaDiscapacidadId,
                    EstatusInformacion = beneficiario.EstatusInformacion,
                    SexoId = beneficiario.SexoId,
                    TrabajadorId = beneficiario.TrabajadorId,
                    Estatus = beneficiario.Estatus,
                    CreatedAt = now,
                    UpdatedAt = now
                };
                _context.BeneficiarioHistorico.Add(historico);

                beneficiario.Curp = request.Curp;
                beneficiario.ApellidoPaterno = request.ApellidoPaterno;
                beneficiario.ApellidoMaterno = request.ApellidoMaterno;
                beneficiario.Nombre = request.Nombre;
                beneficiario.FechaNacimiento = StartOfDay(request.FechaNacimiento);
                beneficiario.Rfc = request.Rfc;
                beneficiario.EstadoId = request.EstadoId;
                beneficiario.GradoEstudioId = request.GradoEstudioId;
                beneficiario.EstudioId = request.EstudioId;
                beneficiario.EstadoCivilId = request.EstadoCivilId;
                beneficiario.DiscapacidadId = request.DiscapacidadId;
                beneficiario.DiscapacidadId = request.DiscapacidadId;
                beneficiario.DiscapacidadGradoId = request.DiscapacidadGradoId;
                beneficiario.CausaDiscapacidadId = request.CausaDiscapacidadId;
                beneficiario.SexoId = request.SexoId;
                beneficiario.Estatus = request.Estatus;
                beneficiario.Comentarios = request.Comentarios;
                beneficiario.UpdatedAt = now;

                _context.Beneficiario.Update(beneficiario);

                _context.Bitacora.Add(new Bitacora()
                {
                    UsuarioId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                    Accion = AccionBitacora.EDICION,
                    Mensaje = "Se modificó el beneficiario " + beneficiario.Nombre + ".",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                });

                _context.SaveChanges();
                transaction.Commit();
            }

            return "ok";
        }

        /// <summary>
        /// Funcion que permite marcar como activa una version anterior en los datos del beneficiario para que esta
        /// informacion sea enviada a los dispositivos moviles 
        /// </summary>
        /// <param name="id">Identificador del beneficiario</param>
        /// <returns>Estatus de la actualizacion en la base de datos </returns>
        [HttpPost]
        [Authorize]
        public string Activar(string id)
        {
            var now = DateTime.Now;
            var Id = id.Split(".")[0];
            var Numero = Int32.Parse(id.Split(".")[1]);
            using (var transaction = _context.Database.BeginTransaction())
            {
                var beneficiario = _context.Beneficiario.Find(Id);
                if (beneficiario != null)
                {
                    var historicos = _context.BeneficiarioHistorico
                        .Where(d => d.BeneficiarioId.Equals(Id) && d.DeletedAt == null)
                        .OrderByDescending(d => d.CreatedAt).ToList();
                    var historico = historicos[Numero - 1];
                    var historicoNombre = historico.Nombre;
                    var historicoApellidoPaterno = historico.ApellidoPaterno;
                    var historicoApellidoMaterno = historico.ApellidoMaterno;
                    var historicoFechaNacimiento = historico.FechaNacimiento;
                    var historicoCurp = historico.Curp;
                    var historicoRfc = historico.Rfc;
                    var historicoEstadoId = historico.EstadoId;
                    var historicoEstudioId = historico.EstudioId;
                    var historicoGradoEstudioId = historico.GradoEstudioId;
                    var historicoEstadoCivilId = historico.EstadoCivilId;
                    var historicoDiscapacidadId = historico.DiscapacidadId;
                    var historicoDiscapacidadGradoId = historico.DiscapacidadGradoId;
                    var historicoCausaDiscapacidadId = historico.CausaDiscapacidadId;
                    var historicoSexoId = historico.SexoId;
                    var historicoComentarios = historico.Comentarios;
                    var historicoEstatusInformacion = historico.EstatusInformacion;
                    var historicoEstatusUpdatedAt = historico.EstatusUpdatedAt;

                    var beneficiarioNombre = beneficiario.Nombre;
                    var beneficiarioApellidoPaterno = beneficiario.ApellidoPaterno;
                    var beneficiarioApellidoMaterno = beneficiario.ApellidoMaterno;
                    var beneficiarioFechaNacimiento = beneficiario.FechaNacimiento;
                    var beneficiarioCurp = beneficiario.Curp;
                    var beneficiarioRfc = beneficiario.Rfc;
                    var beneficiarioEstadoId = beneficiario.EstadoId;
                    var beneficiarioEstudioId = beneficiario.EstudioId;
                    var beneficiarioGradoEstudioId = beneficiario.GradoEstudioId;
                    var beneficiarioEstadoCivilId = beneficiario.EstadoCivilId;
                    var beneficiarioDiscapacidadId = beneficiario.DiscapacidadId;
                    var beneficiarioDiscapacidadGradoId = beneficiario.DiscapacidadGradoId;
                    var beneficiarioCausaDiscapacidadId = beneficiario.CausaDiscapacidadId;
                    var beneficiarioSexoId = beneficiario.SexoId;
                    var beneficiarioComentarios = beneficiario.Comentarios;
                    var beneficiarioEstatusInformacion = beneficiario.EstatusInformacion;
                    var beneficiarioEstatusUpdatedAt = beneficiario.EstatusUpdatedAt;

                    beneficiario.Nombre = historicoNombre;
                    beneficiario.ApellidoPaterno = historicoApellidoPaterno;
                    beneficiario.ApellidoMaterno = historicoApellidoMaterno;
                    beneficiario.FechaNacimiento = historicoFechaNacimiento;
                    beneficiario.Curp = historicoCurp;
                    beneficiario.Rfc = historicoRfc;
                    beneficiario.EstadoId = historicoEstadoId;
                    beneficiario.GradoEstudioId = historicoGradoEstudioId;
                    beneficiario.EstudioId = historicoEstudioId;
                    beneficiario.EstadoCivilId = historicoEstadoCivilId;
                    beneficiario.DiscapacidadId = historicoDiscapacidadId;
                    beneficiario.DiscapacidadGradoId = historicoDiscapacidadGradoId;
                    beneficiario.CausaDiscapacidadId = historicoCausaDiscapacidadId;
                    beneficiario.SexoId = historicoSexoId;
                    beneficiario.Comentarios = historicoComentarios;
                    beneficiario.EstatusInformacion = historicoEstatusInformacion;
                    beneficiario.EstatusUpdatedAt = historicoEstatusUpdatedAt;
                    beneficiario.UpdatedAt = now;

                    historico.Nombre = beneficiarioNombre;
                    historico.ApellidoPaterno = beneficiarioApellidoPaterno;
                    historico.ApellidoMaterno = beneficiarioApellidoMaterno;
                    historico.FechaNacimiento = beneficiarioFechaNacimiento;
                    historico.Curp = beneficiarioCurp;
                    historico.Rfc = beneficiarioRfc;
                    historico.EstadoId = beneficiarioEstadoId;
                    historico.EstudioId = beneficiarioEstudioId;
                    historico.GradoEstudioId = beneficiarioGradoEstudioId;
                    historico.EstadoCivilId = beneficiarioEstadoCivilId;
                    historico.DiscapacidadId = beneficiarioDiscapacidadId;
                    historico.DiscapacidadGradoId = beneficiarioDiscapacidadGradoId;
                    historico.CausaDiscapacidadId = beneficiarioCausaDiscapacidadId;
                    historico.SexoId = beneficiarioSexoId;
                    historico.Comentarios = beneficiarioComentarios;
                    historico.EstatusInformacion = beneficiarioEstatusInformacion;
                    historico.EstatusUpdatedAt = beneficiarioEstatusUpdatedAt;
                    historico.UpdatedAt = now;

                    _context.Beneficiario.Update(beneficiario);
                    _context.BeneficiarioHistorico.Update(historico);

                    _context.Bitacora.Add(new Bitacora()
                    {
                        UsuarioId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                        Accion = AccionBitacora.EDICION,
                        Mensaje = "Se modificó el beneficiario " + beneficiario.Nombre + ".",
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    });

                    _context.SaveChanges();
                }

                transaction.Commit();
            }

            return "ok";
        }

        /// <summary>
        /// View pdf generation as ActionResult
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetPdf (string id)
        {
            try
            {
                var response = new BeneficiarioResponse
                {
                    Beneficiario = new BeneficiarioResponseModel(),
                    Carencias = new List<AplicacionCarencia>(),
                    Discapacidades = new List<DiscapacidadIntegrante>(),
                    Domicilio = new DomicilioResponse(),
                    Solicitudes = new List<SolicitudesListModel>(),
                    Preguntas = new List<AplicacionPregunta>(),
                    Documentos = new List<ImagenModel>(),
                    Huellas = new List<Huella>(),
                    Hojas = new List<List<BeneficiarioResponseModel>>(),
                    Ruta = _configuration["SITE_URL"]
                };

                var beneficiario = _context.Beneficiario.Where(b => b.Id == id).Include(b => b.Estudio)
                    .Include(b => b.GradoEstudio)
                    .Include(b => b.EstadoCivil).Include(b => b.Estado)
                    .Include(b => b.Sexo).Include(b => b.Discapacidad)
                    .Include(b => b.DiscapacidadGrado).ThenInclude(dg => dg.Grade)
                    .Include(b => b.CausaDiscapacidad).First();

                var domicilio = _context.Domicilio.Where(d => d.Id == beneficiario.DomicilioId)
                    .Include(a => a.Ageb)
                    .Include(d => d.Localidad).Include(l => l.Municipio)
                    .Include(b => b.Colonia)
                    .Include(b => b.Calle)
                    .Include(b => b.ZonaImpulso).FirstOrDefault();

                var hijos = _context.Beneficiario.Include(x => x.Domicilio)
                    .Include(h => h.Parentesco)
                    .Include(h => h.Estudio)
                    .Include(h => h.GradoEstudio)
                    .Where(b => b.PadreId.Equals(beneficiario.Id) || b.Id.Equals(beneficiario.Id))
                    .OrderBy(h => h.NumIntegrante);

                var trabajador = _context.Trabajador.Where(t => t.Id == domicilio.TrabajadorId)
                    .Include(t => t.TrabajadorDependencias).ThenInclude(td => td.Dependencia).FirstOrDefault();
                var aplicacion =
                    _context.Aplicacion.FirstOrDefault(a => a.BeneficiarioId == beneficiario.Id && a.Activa);

                if (aplicacion != null)
                {
                    var discapacidades = new Dictionary<string, DiscapacidadIntegrante>();
                    response.Aplicacion = new Aplicacion
                    {
                        Id = aplicacion.Id,
                        Estatus = aplicacion.Estatus,
                        CreatedAt = aplicacion.CreatedAt,
                        Piso = aplicacion.Piso,
                        Techo = aplicacion.Techo,
                        Muro = aplicacion.Muro,
                        Hacinamiento = aplicacion.Hacinamiento,
                        Agua = aplicacion.Agua,
                        Drenaje = aplicacion.Drenaje,
                        Electricidad = aplicacion.Electricidad,
                        Combustible = aplicacion.Combustible,
                        PropiedadVivienda = aplicacion.PropiedadVivienda,
                        Trabajador = trabajador,
                        FechaInicio = aplicacion.FechaInicio,
                        PerdidaEmpleo = aplicacion.PerdidaEmpleo,
                        PreocupacionCovid = aplicacion.PreocupacionCovid,
                        Alimentaria = aplicacion.Alimentaria
                    };
                    foreach (var hijo in hijos)
                    {
                        discapacidades.Add(hijo.Id, new DiscapacidadIntegrante
                        {
                            NumIntegrante = hijo.NumIntegrante
                        });
                    }
                    var preguntas = _context.AplicacionPregunta.Where(p => p.AplicacionId == aplicacion.Id && p.PreguntaId == "29").ToList();
                    var carencias = _context.AplicacionCarencia.Where(ap => ap.AplicacionId.Equals(aplicacion.Id))
                        .OrderBy(ap => ap.NumIntegrante).ToList();
                    var integrantesId = carencias.Select(c => c.BeneficiarioId);
                    var discapacidadesDB = _context.BeneficiarioDiscapacidades
                        .Where(bd => integrantesId.Contains(bd.BeneficiarioId)).ToList();

                    foreach (var ap in preguntas)
                    {
                        response.Preguntas.Add(new AplicacionPregunta
                        {
                            NumIntegrante = ap.NumIntegrante,
                            Id = ap.Id,
                            PreguntaId = ap.PreguntaId,
                            RespuestaId = ap.RespuestaId
                 
                        });
                    }
                    foreach (var ac in carencias)
                    {
                        response.Carencias.Add(new AplicacionCarencia
                        {
                            NumIntegrante = ac.NumIntegrante,
                            Id = ac.Id,
                            Educativa = ac.Educativa,
                            ServicioSalud = ac.ServicioSalud,
                            SeguridadSocial = ac.SeguridadSocial,
                            Alimentaria = ac.Alimentaria,
                            Piso = ac.Piso,
                            Techo = ac.Techo,
                            Muro = ac.Muro,
                            Hacinamiento = ac.Hacinamiento,
                            Agua = ac.Agua,
                            Drenaje = ac.Drenaje,
                            Electricidad = ac.Electricidad,
                            Combustible = ac.Combustible,
                            Edad = ac.Edad,
                            PerdidaEmpleo = ac.PerdidaEmpleo,
                            NumCarencias = (ac.Educativa ? 1 : 0) + (ac.ServicioSalud ? 1 : 0)
                                                                  + (ac.SeguridadSocial ? 1 : 0) + (ac.Alimentaria ? 1 : 0) 
                                                                  + ( ac.Piso || ac.Techo || ac.Muro || ac.Hacinamiento ? 1: 0) 
                                                                  + (ac.Agua || ac.Drenaje || ac.Electricidad || ac.Combustible ? 1 : 0)
                        });
                    }
                     if(response.Carencias.Count() > response.Preguntas.Count())
                    {
                        var car = response.Carencias.Count();
                        var p = response.Preguntas.Count();
                        var dif = car - p;

                        for (int i = 0; i <= dif; i ++ )
                        {
                            response.Preguntas.Add(new AplicacionPregunta
                            {
                                NumIntegrante = -1,
                                Id = null,
                                PreguntaId = null,
                                RespuestaId = null

                            });

                        }
                    }

                    foreach (var discapacidad in discapacidadesDB)
                    {
                        switch (discapacidad.DiscapacidadId)
                        {
                            case "117":
                                discapacidades[discapacidad.BeneficiarioId].Caminar = true;
                                break;
                            case "118":
                                discapacidades[discapacidad.BeneficiarioId].Ver = true;
                                break;
                            case "119":
                                discapacidades[discapacidad.BeneficiarioId].Hablar = true;
                                break;
                            case "120":
                                discapacidades[discapacidad.BeneficiarioId].Oir = true;
                                break;
                            case "121":
                                discapacidades[discapacidad.BeneficiarioId].Vestirse = true;
                                break;
                            case "122":
                                discapacidades[discapacidad.BeneficiarioId].Atencion = true;
                                break;
                            case "123":
                                discapacidades[discapacidad.BeneficiarioId].Mental = true;
                                break;
                        }
                    }

                    response.Discapacidades = discapacidades.Values.ToList();
                }

                response.Beneficiario.Id = beneficiario.Id;
                response.Beneficiario.Nombre = beneficiario.Nombre;
                response.Beneficiario.ApellidoPaterno = beneficiario.ApellidoPaterno;
                response.Beneficiario.ApellidoMaterno = beneficiario.ApellidoMaterno;
                response.Beneficiario.FechaNacimiento = beneficiario.FechaNacimiento.Value.ToString("d/M/yy");
                response.Beneficiario.Estado = beneficiario.Estado.Nombre;
                response.Beneficiario.Curp = beneficiario.Curp;
                response.Beneficiario.Rfc = beneficiario.Rfc;
                response.Beneficiario.Estudio = beneficiario.Estudio.Nombre;
                response.Beneficiario.GradoEstudio = beneficiario.GradoEstudio != null
                    ? beneficiario.GradoEstudio.Nombre
                    : "Sin grado de estudio";
                response.Beneficiario.EstadoCivil = beneficiario.EstadoCivil.Nombre;
                response.Beneficiario.Discapacidad =
                    beneficiario.DiscapacidadId == null ? "" : beneficiario.Discapacidad.Nombre;
                response.Beneficiario.GradoDiscapacidad = beneficiario.DiscapacidadGrado != null
                    ? beneficiario.DiscapacidadGrado.Grade.Nombre
                    : "Sin grado de discapacidad";
                response.Beneficiario.CausaDiscapacidad = beneficiario.CausaDiscapacidad != null
                    ? beneficiario.CausaDiscapacidad.Nombre
                    : "Sin causa de discapacidad";
                response.Beneficiario.Estatus = beneficiario.Estatus ? "Activo" : "Inactivo";
                response.Beneficiario.Comentarios = beneficiario.Comentarios;
                response.Beneficiario.Sexo = beneficiario.Sexo.Nombre;
                response.Beneficiario.Activa = true;
                response.Beneficiario.EstatusInformacion = beneficiario.EstatusInformacion;
                response.Beneficiario.Folio = beneficiario.Folio;

                response.Domicilio.Id = domicilio.Id;
                response.Domicilio.Domicilio = domicilio.DomicilioN;
                response.Domicilio.NombreAsentamiento = domicilio.NombreAsentamiento;
                response.Domicilio.Email = domicilio.Email;
                response.Domicilio.Latitud = domicilio.LatitudAtm;
                response.Domicilio.Longitud = domicilio.LongitudAtm;
                response.Domicilio.Localidad = domicilio.Localidad.Nombre;
                response.Domicilio.Municipio = domicilio.Localidad.Municipio.Nombre;
                response.Domicilio.IndiceDesarrolloHumano = domicilio.IndiceDesarrolloHumano != null ? domicilio.IndiceDesarrolloHumano.Split('@')[0] : " ";
                response.Domicilio.IndiceDesarrolloHumano = response.Domicilio.IndiceDesarrolloHumano.Replace("_2015","").Replace("_2010","");
                response.Domicilio.MarginacionLocalidad = domicilio.MarginacionLocalidad.Contains("ND") ? " ": domicilio.MarginacionLocalidad.Split('@')[0].Replace("_2010", "").Replace("_2015", " ").Replace("[\"ND\"]", "");
                response.Domicilio.MarginacionAgeb = domicilio.MarginacionAgeb.Contains("ND") ? " " : domicilio.MarginacionAgeb.Split('@')[0].Replace("_2010","").Replace("_2015"," ");
                response.Domicilio.Telefono = domicilio.Telefono;
                response.Domicilio.TelefonoCasa = domicilio.TelefonoCasa;
                response.Domicilio.CodigoPostal = domicilio.CodigoPostal;
                response.Domicilio.EntreCalle1 = domicilio.EntreCalle1;
                response.Domicilio.EntreCalle2 = domicilio.EntreCalle2;
                response.Domicilio.ZonaImpulso = domicilio.ZonaImpulsoId == null ? "" : domicilio.ZonaImpulso.Nombre;
                response.Domicilio.AgebCalculado = domicilio.AgebCalculado.Contains("ND") ? "" : domicilio.AgebCalculado.Split('@')[0];
                response.Domicilio.NumExterior = domicilio.NumExterior;
                response.Dependencia = trabajador.TrabajadorDependencias.FirstOrDefault().Dependencia.Nombre;

                var zona = _context.Zona.Join(
                    _context.MunicipioZona, z => z.Id, mz => mz.ZonaId, (z, mz) =>
                        new
                        {
                            Z = z,
                            MZ = mz
                        }).FirstOrDefault(z => z.MZ.MunicipioId == domicilio.MunicipioId && z.Z.DependenciaId ==
                    trabajador.TrabajadorDependencias.FirstOrDefault().DependenciaId);
                if (zona != null)
                {
                    response.Domicilio.Region = zona.Z.Clave;
                }

                foreach (var hijosData in hijos.ChunkData(6))
                {
                    var hoja = new List<BeneficiarioResponseModel>();
                    foreach (var hijo in hijosData)
                    {
                        hoja.Add(new BeneficiarioResponseModel
                        {
                            Id = hijo.Id,
                            Nombre = hijo.Nombre,
                            NumIntegrante = hijo.NumIntegrante,
                            Curp = hijo.Curp,
                            ApellidoPaterno = hijo.ApellidoPaterno,
                            ApellidoMaterno = hijo.ApellidoMaterno,
                            MismoDomicilio = hijo.MismoDomicilio,
                            Parentesco = hijo.Parentesco?.Nombre,
                            Estudio = hijo.Estudio?.Nombre,
                            GradoEstudio = hijo.GradoEstudio?.Nombre,
                            DomicilioString = hijo.Domicilio != null
                                ? hijo.Domicilio.DomicilioN
                                : (hijo.MismoDomicilio ? "Mismo domicilio que el beneficiario" : "Sin domicilio")
                        });
                    }

                    response.Hojas.Add(hoja);
                }

                var qrGenerator = new QRCodeGenerator();
                var qrCodeData = qrGenerator.CreateQrCode(_configuration["MAPS_URL"] + "?q=" +
                                                          response.Domicilio.Latitud + ","
                                                          + response.Domicilio.Longitud, QRCodeGenerator.ECCLevel.Q);
                var qrCode = new QRCode(qrCodeData);
                var qrCodeImage = qrCode.GetGraphic(20);
                response.Qr = Convert.ToBase64String(BitmapToBytesCode(qrCodeImage));
                response.NumIntegrantes = response.Hojas.Sum(h => h.Count);
                return new ViewAsPdf("~/Views/Beneficiarios/FichaFamiliar.cshtml", response)
                {
                    FileName = "Ficha familiar.pdf",
                    PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape,
                };
            }
            catch (Exception e)
            {
                Excepcion.Registrar(e);
            }

            return new EmptyResult();
        }

        private static byte[] BitmapToBytesCode(Bitmap image)
        {
            using (var stream = new MemoryStream())
            {
                image.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
                return stream.ToArray();
            }
        }

        [HttpPost]
        [Authorize]
        public string VerDatosHistoricos([FromBody] VerDatosRequest request)
        {
            var integrantes = _context.BeneficiarioHistorico.Where(b => b.DeletedAt == null
                                                                        && b.BeneficiarioId == request.BeneficiarioId)
                .OrderByDescending(bh => bh.CreatedAt).ToList();
            var integrante = integrantes[request.Numero - 2];
            var response = new BeneficiarioEditModel
            {
                Id = integrante.Id,
                Nombre = integrante.Nombre,
                ApellidoPaterno = integrante.ApellidoPaterno,
                ApellidoMaterno = integrante.ApellidoMaterno,
                FechaNacimiento = integrante.FechaNacimiento.Value,
                EstadoId = integrante.EstadoId,
                Curp = integrante.Curp ?? "",
                Rfc = integrante.Rfc ?? "",
                EstudioId = integrante.EstudioId,
                GradoEstudioId = integrante.GradoEstudioId,
                EstadoCivilId = integrante.EstadoCivilId,
                SexoId = integrante.SexoId,
                DiscapacidadId = integrante.DiscapacidadId,
                DiscapacidadGradoId = integrante.DiscapacidadGradoId,
                CausaDiscapacidadId = integrante.CausaDiscapacidadId,
                Comentarios = integrante.Comentarios,
                Estatus = integrante.Estatus
            };
            return JsonSedeshu.SerializeObject(response);
        }
    }
}