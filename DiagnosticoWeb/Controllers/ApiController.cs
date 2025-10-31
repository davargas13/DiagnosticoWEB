using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using DiagnosticoWeb.Code;
using DiagnosticoWeb.Database;
using DiagnosticoWeb.Models;
using System.Data.SqlClient;
using System.Data;
using System.Security.Claims;

namespace DiagnosticoWeb.Controllers
{
    /// <summary>
    /// Clase que sincroniza la información de la base de datos a la aplicacion movil
    /// </summary>
    [Route("api/[action]")]
    [ApiController]
    public class ApiController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpClientFactory _clientFactory;

        private const string Llave = "tresfact";

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="context">Conexion a la base de datos</param>
        public ApiController(ApplicationDbContext context, IConfiguration configuration,
            UserManager<ApplicationUser> userManager, IHttpClientFactory clientFactory)
        {
            _context = context;
            _configuration = configuration;
            _userManager = userManager;
            _clientFactory = clientFactory;
        }

        /// <summary>
        /// Funcion que devuelve la informacion modificada de los catalogos de acuerdo a la fecha que especifica el dispositivo movil
        /// </summary>
        /// <param name="request">Objeto que contiene la fecha de ultima sincronizacion</param>
        /// <returns>JSON con la informacion modificada o nueva de los catalogos de acuerdo a la fecha solicitada por el dispositivo movil</returns>
        [HttpPost]
        public string Sync([FromForm] RequestSync request)
        {
            var api = new ApiModel();
            var first = request.LastSyncDate.Equals("1990-02-01 00:00:00");
            var localidadesIds = JsonConvert.DeserializeObject<List<string>>(request.LocalidadesId);
            var municipiosIds = JsonConvert.DeserializeObject<List<string>>(request.MunicipiosId);
            var trabajadorId = request.TrabajadorId;
            var lastSync = Convert.ToDateTime(request.LastSyncDate);

            var usuarios = first
                ? _context.Trabajador.Where(x => DateTime.Compare(x.UpdatedAt, lastSync) >= 0 && x.DeletedAt == null)
                    .ToList()
                : _context.Trabajador.Where(x => DateTime.Compare(x.UpdatedAt, lastSync) >= 0).ToList();
            api.Usuarios = usuarios.Select(usuario => new TrabajadorApiModel
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Username = usuario.Username,
                Email = usuario.Email,
                Password = Utils.DecryptString(Llave, usuario.Password, true),
                Codigo = usuario.Codigo,
                CreatedAt = usuario.CreatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                UpdatedAt = usuario.UpdatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                DeletedAt = usuario.DeletedAt?.ToString("yyyy-MM-ddTHH:mm:sszzz")
            }).ToList();

            var usuariosDep = first
                ? _context.TrabajadorDependencia
                    .Where(x => DateTime.Compare(x.UpdatedAt, lastSync) >= 0 && x.DeletedAt == null).ToList()
                : _context.TrabajadorDependencia.Where(x => DateTime.Compare(x.UpdatedAt, lastSync) >= 0).ToList();
            api.UsuarioDependencias = usuariosDep.Select(dependencia => new TrabajadorDependenciaApiModel
            {
                Id = dependencia.Id,
                UsuarioId = dependencia.TrabajadorId,
                DependenciaId = dependencia.DependenciaId,
                CreatedAt = dependencia.CreatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                UpdatedAt = dependencia.UpdatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                DeletedAt = dependencia.DeletedAt?.ToString("yyyy-MM-ddTHH:mm:sszzz")
            }).ToList();

            var trabajadorRegiones = first
                ? _context.TrabajadorRegion
                    .Where(x => DateTime.Compare(x.UpdatedAt, lastSync) >= 0 && x.DeletedAt == null).ToList()
                : _context.TrabajadorRegion.Where(x => DateTime.Compare(x.UpdatedAt, lastSync) >= 0).ToList();
            api.TrabajadorRegiones = trabajadorRegiones.Select(region => new TrabajadorRegionApiModel
            {
                Id = region.Id,
                TrabajadorId = region.TrabajadorId,
                ZonaId = region.ZonaId,
                CreatedAt = region.CreatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                UpdatedAt = region.UpdatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                DeletedAt = region.DeletedAt?.ToString("yyyy-MM-ddTHH:mm:sszzz")
            }).ToList();

            var causasDiscapacidades = first
                ? _context.CausaDiscapacidad
                    .Where(x => DateTime.Compare(x.UpdatedAt, lastSync) >= 0 && x.DeletedAt == null).ToList()
                : _context.CausaDiscapacidad.Where(x => DateTime.Compare(x.UpdatedAt, lastSync) >= 0).ToList();
            api.CausasDiscapacidades = causasDiscapacidades.Select(causa => new CausaDiscapacidadApiModel
            {
                Id = causa.Id,
                Nombre = causa.Nombre,
                CreatedAt = causa.CreatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                UpdatedAt = causa.UpdatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                DeletedAt = causa.DeletedAt?.ToString("yyyy-MM-ddTHH:mm:sszzz")
            }).ToList();

            var discapacidadesGrado = first
                ? _context.DiscapacidadGrado
                    .Where(x => DateTime.Compare(x.UpdatedAt, lastSync) >= 0 && x.DeletedAt == null).ToList()
                : _context.DiscapacidadGrado.Where(x => DateTime.Compare(x.UpdatedAt, lastSync) >= 0).ToList();
            api.DiscapacidadesGrados = discapacidadesGrado.Select(grado => new DiscapacidadGradoApiModel
            {
                Id = grado.Id,
                DiscapacidadId = grado.DiscapacidadId,
                GradoId = grado.GradoId,
                Grado = grado.Grado,
                CreatedAt = grado.CreatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                UpdatedAt = grado.UpdatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                DeletedAt = grado.DeletedAt?.ToString("yyyy-MM-ddTHH:mm:sszzz")
            }).ToList();

            var gradosEstudio = first
                ? _context.GradosEstudio.Where(x => DateTime.Compare(x.UpdatedAt, lastSync) >= 0 && x.DeletedAt == null)
                    .ToList()
                : _context.GradosEstudio.Where(x => DateTime.Compare(x.UpdatedAt, lastSync) >= 0).ToList();
            api.GradosEstudio = gradosEstudio.Select(grado => new GradosEstudioApiModel
            {
                Id = grado.Id,
                Nombre = grado.Nombre,
                CreatedAt = grado.CreatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                UpdatedAt = grado.UpdatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                DeletedAt = grado.DeletedAt?.ToString("yyyy-MM-ddTHH:mm:sszzz")
            }).ToList();

            var grados = first
                ? _context.Grado.Where(x => DateTime.Compare(x.UpdatedAt, lastSync) >= 0 && x.DeletedAt == null)
                    .ToList()
                : _context.Grado.Where(x => DateTime.Compare(x.UpdatedAt, lastSync) >= 0).ToList();
            api.Grados = grados.Select(grado => new GradoApiModel
            {
                Id = grado.Id,
                Nombre = grado.Nombre,
                CreatedAt = grado.CreatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                UpdatedAt = grado.UpdatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                DeletedAt = grado.DeletedAt?.ToString("yyyy-MM-ddTHH:mm:sszzz")
            }).ToList();

            var encuestas = first
                ? _context.Encuesta.Where(x => DateTime.Compare(x.UpdatedAt, lastSync) >= 0 && x.DeletedAt == null)
                    .ToList()
                : _context.Encuesta.Where(x => DateTime.Compare(x.UpdatedAt, lastSync) >= 0).ToList();
            api.Encuestas = encuestas.Select(encuesta => new EncuestaApiModel
            {
                Id = encuesta.Id,
                Nombre = encuesta.Nombre,
                Mensaje = encuesta.Mensaje,
                Activo = encuesta.Activo,
                Vigencia = encuesta.Vigencia,
                CreatedAt = encuesta.CreatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                UpdatedAt = encuesta.UpdatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                DeletedAt = encuesta.DeletedAt?.ToString("yyyy-MM-ddTHH:mm:sszzz")
            }).ToList();

            var versiones = first
                ? _context.EncuestaVersion
                    .Where(x => DateTime.Compare(x.UpdatedAt, lastSync) >= 0 && x.DeletedAt == null).ToList()
                : _context.EncuestaVersion.Where(x => DateTime.Compare(x.UpdatedAt, lastSync) >= 0).ToList();
            api.Versiones = versiones.Select(version => new EncuestaVersionApiModel()
            {
                Id = version.Id,
                Activa = version.Activa,
                Numero = version.Numero,
                EncuestaId = version.EncuestaId,
                CreatedAt = version.CreatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                UpdatedAt = version.UpdatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                DeletedAt = version.DeletedAt?.ToString("yyyy-MM-ddTHH:mm:sszzz")
            }).ToList();

            var preguntas = first
                ? _context.Pregunta.Where(x => DateTime.Compare(x.UpdatedAt, lastSync) >= 0 && x.DeletedAt == null)
                    .ToList()
                : _context.Pregunta.Where(x => DateTime.Compare(x.UpdatedAt, lastSync) >= 0).ToList();
            api.Preguntas = preguntas.Select(pregunta => new PreguntaApiModel
            {
                Id = pregunta.Id,
                Nombre = pregunta.Nombre,
                Numero = pregunta.Numero,
                Iterable = pregunta.Iterable,
                Condicion = pregunta.Condicion,
                CondicionIterable = pregunta.CondicionIterable,
                CondicionLista = pregunta.CondicionLista,
                EncuestaVersionId = pregunta.EncuestaVersionId,
                TipoPregunta = pregunta.TipoPregunta,
                Editable = pregunta.Editable,
                Catalogo = pregunta.Catalogo,
                Gradual = pregunta.Gradual,
                Expresion = pregunta.Expresion,
                ExpresionEjemplo = pregunta.ExpresionEjemplo,
                CarenciaId = pregunta.CarenciaId,
                EsNombre = pregunta.EsNombre,
                Complemento = pregunta.Complemento,
                TipoComplemento = pregunta.TipoComplemento,
                Activa = pregunta.Activa,
                Obligatoria = pregunta.Obligatoria,
                SeleccionarRespuestas = pregunta.SeleccionarRespuestas,
                Maximo = pregunta.Maximo,
                CreatedAt = pregunta.CreatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                UpdatedAt = pregunta.UpdatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                DeletedAt = pregunta.DeletedAt?.ToString("yyyy-MM-ddTHH:mm:sszzz")
            }).ToList();

            var respuestas = first
                ? _context.Respuesta.Where(x => DateTime.Compare(x.UpdatedAt, lastSync) >= 0 && x.DeletedAt == null)
                    .ToList()
                : _context.Respuesta.Where(x => DateTime.Compare(x.UpdatedAt, lastSync) >= 0).ToList();
            api.Respuestas = respuestas.Select(respuesta => new RespuestaApiModel
            {
                Id = respuesta.Id,
                Nombre = respuesta.Nombre,
                Numero = respuesta.Numero,
                PreguntaId = respuesta.PreguntaId,
                Negativa = respuesta.Negativa,
                Complemento = respuesta.Complemento,
                TipoComplemento = respuesta.TipoComplemento,
                CreatedAt = respuesta.CreatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                UpdatedAt = respuesta.UpdatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                DeletedAt = respuesta.DeletedAt?.ToString("yyyy-MM-ddTHH:mm:sszzz")
            }).ToList();

            var preguntasGrado = first
                ? _context.PreguntaGrado.Where(x => DateTime.Compare(x.UpdatedAt, lastSync) >= 0 && x.DeletedAt == null)
                    .ToList()
                : _context.PreguntaGrado.Where(x => DateTime.Compare(x.UpdatedAt, lastSync) >= 0).ToList();
            api.PreguntasGrados = preguntasGrado.Select(grado => new PreguntaGradoApiModel
            {
                Id = grado.Id,
                Grado = grado.Grado,
                PreguntaId = grado.PreguntaId,
                CreatedAt = grado.CreatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                UpdatedAt = grado.UpdatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                DeletedAt = grado.DeletedAt?.ToString("yyyy-MM-ddTHH:mm:sszzz")
            }).ToList();

            var respuestasGrados = first
                ? _context.RespuestaGrado
                    .Where(x => DateTime.Compare(x.UpdatedAt, lastSync) >= 0 && x.DeletedAt == null).ToList()
                : _context.RespuestaGrado.Where(x => DateTime.Compare(x.UpdatedAt, lastSync) >= 0).ToList();
            api.RespuestasGrados = respuestasGrados.Select(grado => new RespuestaGradoApiModel()
            {
                Id = grado.Id,
                PreguntaGradoId = grado.PreguntaGradoId,
                RespuestaId = grado.RespuestaId,
                Grado = int.Parse(grado.Grado),
                CreatedAt = grado.CreatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                UpdatedAt = grado.UpdatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                DeletedAt = grado.DeletedAt?.ToString("yyyy-MM-ddTHH:mm:sszzz")
            }).ToList();


            var carreteras = first
                ? _context.Carretera.Where(x => DateTime.Compare(x.UpdatedAt, lastSync) >= 0 && x.DeletedAt == null)
                    .ToList()
                : _context.Carretera.Where(x => DateTime.Compare(x.UpdatedAt, lastSync) >= 0).ToList();
            api.Carreteras = carreteras.Select(carretera => new CarreteraApiModel
            {
                Id = carretera.Id,
                Nombre = carretera.Nombre,
                CreatedAt = carretera.CreatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                UpdatedAt = carretera.UpdatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                DeletedAt = carretera.DeletedAt?.ToString("yyyy-MM-ddTHH:mm:sszzz")
            }).ToList();


            var caminos = first
                ? _context.Camino.Where(x => DateTime.Compare(x.UpdatedAt, lastSync) >= 0 && x.DeletedAt == null)
                    .ToList()
                : _context.Camino.Where(x => DateTime.Compare(x.UpdatedAt, lastSync) >= 0).ToList();
            api.Caminos = caminos.Select(camino => new CaminoApiModel
            {
                Id = camino.Id,
                Nombre = camino.Nombre,
                CreatedAt = camino.CreatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                UpdatedAt = camino.UpdatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                DeletedAt = camino.DeletedAt?.ToString("yyyy-MM-ddTHH:mm:sszzz")
            }).ToList();


            var unidades = first
                ? _context.Unidad.Where(x => DateTime.Compare(x.UpdatedAt, lastSync) >= 0 && x.DeletedAt == null)
                    .ToList()
                : _context.Unidad.Where(x => DateTime.Compare(x.UpdatedAt, lastSync) >= 0).ToList();
            api.Unidades = unidades.Select(unidad => new UnidadApiModel
            {
                Id = unidad.Id,
                Nombre = unidad.Nombre,
                CreatedAt = unidad.CreatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                UpdatedAt = unidad.UpdatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                DeletedAt = unidad.DeletedAt?.ToString("yyyy-MM-ddTHH:mm:sszzz")
            }).ToList();

            var discapacidades = first
                ? _context.Discapacidad.Where(x => DateTime.Compare(x.UpdatedAt, lastSync) >= 0 && x.DeletedAt == null)
                    .ToList()
                : _context.Discapacidad.Where(x => DateTime.Compare(x.UpdatedAt, lastSync) >= 0).ToList();
            api.Discapacidades = discapacidades.Select(discapacidad => new DiscapacidadApiModel
            {
                Id = discapacidad.Id,
                Nombre = discapacidad.Nombre,
                CreatedAt = discapacidad.CreatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                UpdatedAt = discapacidad.UpdatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                DeletedAt = discapacidad.DeletedAt?.ToString("yyyy-MM-ddTHH:mm:sszzz")
            }).ToList();

            var estadosCiviles = first
                ? _context.EstadoCivil.Where(x => DateTime.Compare(x.UpdatedAt, lastSync) >= 0 && x.DeletedAt == null)
                    .ToList()
                : _context.EstadoCivil.Where(x => DateTime.Compare(x.UpdatedAt, lastSync) >= 0).ToList();
            api.EstadosCiviles = estadosCiviles.Select(civil => new EstadoCivilApiModel
            {
                Id = civil.Id,
                Nombre = civil.Nombre,
                CreatedAt = civil.CreatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                UpdatedAt = civil.UpdatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                DeletedAt = civil.DeletedAt?.ToString("yyyy-MM-ddTHH:mm:sszzz")
            }).ToList();

            var estudios = first
                ? _context.Estudio.Where(x => DateTime.Compare(x.UpdatedAt, lastSync) >= 0 && x.DeletedAt == null)
                    .ToList()
                : _context.Estudio.Where(x => DateTime.Compare(x.UpdatedAt, lastSync) >= 0).ToList();
            api.Estudios = estudios.Select(estudio => new EstudioApiModel
            {
                Id = estudio.Id,
                Nombre = estudio.Nombre,
                CreatedAt = estudio.CreatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                UpdatedAt = estudio.UpdatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                DeletedAt = estudio.DeletedAt?.ToString("yyyy-MM-ddTHH:mm:sszzz")
            }).ToList();

            var ocupaciones = first
                ? _context.Ocupacion.Where(x => DateTime.Compare(x.UpdatedAt, lastSync) >= 0 && x.DeletedAt == null)
                    .ToList()
                : _context.Ocupacion.Where(x => DateTime.Compare(x.UpdatedAt, lastSync) >= 0).ToList();
            api.Ocupaciones = ocupaciones.Select(ocupacion => new OcupacionApiModel
            {
                Id = ocupacion.Id,
                Nombre = ocupacion.Nombre,
                CreatedAt = ocupacion.CreatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                UpdatedAt = ocupacion.UpdatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                DeletedAt = ocupacion.DeletedAt?.ToString("yyyy-MM-ddTHH:mm:sszzz"),
            }).ToList();

            var parentescos = first
                ? _context.Parentesco.Where(x => DateTime.Compare(x.UpdatedAt, lastSync) >= 0 && x.DeletedAt == null)
                    .ToList()
                : _context.Parentesco.Where(x => DateTime.Compare(x.UpdatedAt, lastSync) >= 0).ToList();
            api.Parentescos = parentescos.Select(parentesco => new ParentescoApiModel
            {
                Id = parentesco.Id,
                Nombre = parentesco.Nombre,
                CreatedAt = parentesco.CreatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                UpdatedAt = parentesco.UpdatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                DeletedAt = parentesco.DeletedAt?.ToString("yyyy-MM-ddTHH:mm:sszzz")
            }).ToList();

            var sexos = first
                ? _context.Sexo.Where(x => DateTime.Compare(x.UpdatedAt, lastSync) >= 0 && x.DeletedAt == null).ToList()
                : _context.Sexo.Where(x => DateTime.Compare(x.UpdatedAt, lastSync) >= 0).ToList();
            api.Sexos = sexos.Select(sexo => new SexoApiModel
            {
                Id = sexo.Id,
                Nombre = sexo.Nombre,
                CreatedAt = sexo.CreatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                UpdatedAt = sexo.UpdatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                DeletedAt = sexo.DeletedAt?.ToString("yyyy-MM-ddTHH:mm:sszzz")
            }).ToList();

            var zonasImpulso = first
                ? _context.ZonaImpulso.Where(x => DateTime.Compare(x.UpdatedAt, lastSync) >= 0 && x.DeletedAt == null)
                    .ToList()
                : _context.ZonaImpulso.Where(x => DateTime.Compare(x.UpdatedAt, lastSync) >= 0).ToList();
            api.ZonasImpulso = zonasImpulso.Select(impulso => new ZonaImpulsoApiModel
            {
                Id = impulso.Id,
                Nombre = impulso.Nombre,
                MunicipioId = impulso.MunicipioId,
                CreatedAt = impulso.CreatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                UpdatedAt = impulso.UpdatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                DeletedAt = impulso.DeletedAt?.ToString("yyyy-MM-ddTHH:mm:sszzz")
            }).ToList();

            var tiposAsentamientos = first
                ? _context.TipoAsentamiento
                    .Where(x => DateTime.Compare(x.UpdatedAt, lastSync) >= 0 && x.DeletedAt == null).ToList()
                : _context.TipoAsentamiento.Where(x => DateTime.Compare(x.UpdatedAt, lastSync) >= 0).ToList();
            api.TiposAsentamientos = tiposAsentamientos.Select(asentamiento => new TipoAsentamientoApiModel
            {
                Id = asentamiento.Id,
                Nombre = asentamiento.Nombre,
                CreatedAt = asentamiento.CreatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                UpdatedAt = asentamiento.UpdatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                DeletedAt = asentamiento.DeletedAt?.ToString("yyyy-MM-ddTHH:mm:sszzz")
            }).ToList();

            var carencias = first
                ? _context.Carencia.Where(x => DateTime.Compare(x.UpdatedAt, lastSync) >= 0 && x.DeletedAt == null)
                    .ToList()
                : _context.Carencia.Where(x => DateTime.Compare(x.UpdatedAt, lastSync) >= 0).ToList();
            api.Carencias = carencias.Select(carencia => new CarenciaApiModel
            {
                Id = carencia.Id,
                Nombre = carencia.Nombre,
                Clave = carencia.Clave,
                Color = carencia.Color,
                PadreId = carencia.PadreId,
                CreatedAt = carencia.CreatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                UpdatedAt = carencia.UpdatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                DeletedAt = carencia.DeletedAt?.ToString("yyyy-MM-ddTHH:mm:sszzz")
            }).ToList();

            var tipoIncidencias = first
                ? _context.TipoIncidencia
                    .Where(x => DateTime.Compare(x.UpdatedAt, lastSync) >= 0 && x.DeletedAt == null)
                    .ToList()
                : _context.TipoIncidencia.Where(x => DateTime.Compare(x.UpdatedAt, lastSync) >= 0).ToList();
            api.TiposIncidencias = tipoIncidencias.Select(tipo => new TipoIncidenciaApiModel()
            {
                Id = tipo.Id,
                Nombre = tipo.Nombre,
                CreatedAt = tipo.CreatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                UpdatedAt = tipo.UpdatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                DeletedAt = tipo.DeletedAt?.ToString("yyyy-MM-ddTHH:mm:sszzz")
            }).ToList();

            var lineasBienestar = _context.LineaBienestar.Where(x => DateTime.Compare(x.UpdatedAt, lastSync) >= 0)
                .ToList();
            api.LineasBienestar = lineasBienestar.Select(linea => new LineaBienestarApiModel
            {
                Id = linea.Id,
                MinimaRural = linea.MinimaRural,
                Rural = linea.Rural,
                MinimaUrbana = linea.MinimaUrbana,
                Urbana = linea.Urbana,
                CreatedAt = linea.CreatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                UpdatedAt = linea.UpdatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz"),
            }).ToList();

            var configuraciones = first
                ? _context.Configuracion.Where(x => DateTime.Compare(x.UpdatedAt, lastSync) >= 0).ToList()
                : _context.Configuracion.Where(x => DateTime.Compare(x.UpdatedAt, lastSync) >= 0).ToList();
            api.Configuraciones = configuraciones.Select(configuracion => new ConfiguracionApiModel
            {
                Id = configuracion.Id,
                Nombre = configuracion.Nombre,
                Valor = configuracion.Valor,
                CreatedAt = configuracion.CreatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                UpdatedAt = configuracion.UpdatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz"),
            }).ToList();

            if (municipiosIds.Any())
            {
                var benefQuery = _context.Beneficiario.Where(x => x.PadreId == null && x.HijoId == null)
                    .GroupJoin(_context.Domicilio, ben => ben.DomicilioId, dom => dom.Id,
                        (ben, dom) => new {Beneficiario = ben, Domicilio = dom});
                if (trabajadorId != null)
                {
                    benefQuery = benefQuery.Where(x => x.Beneficiario.TrabajadorId == trabajadorId);
                }

                if (localidadesIds.Any())
                {
                    benefQuery = benefQuery.Where(x => !x.Domicilio.Any() || x.Domicilio.Any(y =>
                        municipiosIds.Contains(y.MunicipioId) &&
                        localidadesIds.Contains(y.LocalidadId) && y.Activa));
                }
                else
                {
                    benefQuery = benefQuery.Where(x =>
                        !x.Domicilio.Any() || x.Domicilio.Any(y => municipiosIds.Contains(y.MunicipioId) && y.Activa));
                }

                var benef = benefQuery.ToList();

                var hijosTodos = _context.Beneficiario
                    .Where(x => x.PadreId != null || x.HijoId != null)
                    .Include(x => x.Domicilio).ToList();

                var beneficiarios = new List<BeneficiarioApiModel>();
                var domicilios = new List<DomicilioApiModel>();

                foreach (var Data in benef)
                {
                    beneficiarios.Add(new BeneficiarioApiModel()
                    {
                        Id = Data.Beneficiario.Id,
                        ApellidoPaterno = Data.Beneficiario.ApellidoPaterno,
                        ApellidoMaterno = Data.Beneficiario.ApellidoMaterno,
                        Nombre = Data.Beneficiario.Nombre,
                        NombreCompleto = ObtenerNombreCompleto(Data.Beneficiario),
                        FechaNacimiento = Data.Beneficiario.FechaNacimiento?.AddHours(12.0)
                            .ToString("yyyy-MM-ddTHH:mm:sszzz"),
                        EstadoId = Data.Beneficiario.EstadoId,
                        Rfc = Data.Beneficiario.Rfc,
                        Curp = Data.Beneficiario.Curp,
                        SexoId = Data.Beneficiario.SexoId,
                        Comentarios = Data.Beneficiario.Comentarios,
                        EstudioId = Data.Beneficiario.EstudioId,
                        DomicilioId = Data.Beneficiario.DomicilioId,
                        EstatusVisita = Data.Beneficiario.EstatusVisita,
                        EstadoCivilId = Data.Beneficiario.EstadoCivilId,
                        DiscapacidadId = Data.Beneficiario.DiscapacidadId,
                        TrabajadorId = Data.Beneficiario.TrabajadorId,
                        EstatusInformacion = Data.Beneficiario.EstatusInformacion,
                        EstatusUpdatedAt = Data.Beneficiario.EstatusUpdatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                        PadreId = Data.Beneficiario.PadreId,
                        MismoDomicilio = Data.Beneficiario.MismoDomicilio,
                        ParentescoId = Data.Beneficiario.ParentescoId,
                        DiscapacidadGradoId = Data.Beneficiario.DiscapacidadGradoId,
                        CausaDiscapacidadId = Data.Beneficiario.CausaDiscapacidadId,
                        GradoEstudioId = Data.Beneficiario.GradoEstudioId,
                        Estatus = Data.Beneficiario.Estatus,
                        Huellas = Data.Beneficiario.Huellas,
                        Folio = Data.Beneficiario.Folio,
                        NumIntegrante = Data.Beneficiario.NumIntegrante,
                        NumFamilia = Data.Beneficiario.NumFamilia,
                        CreatedAt = DateToMiddleDay(Data.Beneficiario.CreatedAt),
                        UpdatedAt = Data.Beneficiario.UpdatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                        DeletedAt = Data.Beneficiario.DeletedAt?.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                        VersionAplicacion = Data.Beneficiario.VersionAplicacion
                    });

                    domicilios.AddRange(Data.Domicilio.Where(x => x.Activa).Select(Dom => new DomicilioApiModel()
                    {
                        Id = Dom.Id,
                        Email = Dom.Email,
                        Telefono = Dom.Telefono,
                        TelefonoCasa = Dom.TelefonoCasa,
                        DomicilioN = Dom.DomicilioN,
                        NumExterior = Dom.NumExterior,
                        NumInterior = Dom.NumInterior,
                        NumFamilia = Dom.NumFamilia,
                        NumFamiliasRegistradas = Dom.NumFamiliasRegistradas,
                        EntreCalle1 = Dom.EntreCalle1,
                        EntreCalle2 = Dom.EntreCalle2,
                        CallePosterior = Dom.CallePosterior,
                        CodigoPostal = Dom.CodigoPostal,
                        AgebId = Dom.AgebId,
                        Latitud = Dom.Latitud,
                        Longitud = Dom.Longitud,
                        LatitudAtm = Dom.LatitudAtm,
                        LongitudAtm = Dom.LongitudAtm,
                        LocalidadId = Dom.LocalidadId,
                        ManzanaId = Dom.ManzanaId,
                        ColoniaId = Dom.ColoniaId,
                        CalleId = Dom.CalleId,
                        ZonaImpulsoId = Dom.ZonaImpulsoId,
                        CarreteraId = Dom.CarreteraId,
                        CaminoId = Dom.CaminoId,
                        TipoAsentamientoId = Dom.TipoAsentamientoId,
                        NombreAsentamiento = Dom.NombreAsentamiento,
                        MunicipioId = Dom.MunicipioId,
                        TrabajadorId = Dom.TrabajadorId,
                        CadenaOCR = Dom.CadenaOCR,
                        Porcentaje = Dom.Porcentaje,
                        CreatedAt = Dom.CreatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                        UpdatedAt = Dom.UpdatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                        DeletedAt = Dom.DeletedAt?.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                        VersionAplicacion = Dom.VersionAplicacion,
                        AutorizaFotos = Dom.AutorizaFotos
                    }));

                    var Hijos = hijosTodos
                        .Where(x => x.PadreId != null && x.PadreId.Equals(Data.Beneficiario.Id))
                        .ToList();

                    foreach (var Hijo in Hijos)
                    {
                        beneficiarios.Add(new BeneficiarioApiModel()
                        {
                            Id = Hijo.Id,
                            ApellidoPaterno = Hijo.ApellidoPaterno,
                            ApellidoMaterno = Hijo.ApellidoMaterno,
                            Nombre = Hijo.Nombre,
                            NombreCompleto = ObtenerNombreCompleto(Hijo),
                            FechaNacimiento = Hijo.FechaNacimiento?.AddHours(12.0).ToString("yyyy-MM-ddTHH:mm:sszzz"),
                            EstadoId = Hijo.EstadoId,
                            Rfc = Hijo.Rfc,
                            Curp = Hijo.Curp,
                            SexoId = Hijo.SexoId,
                            Comentarios = Hijo.Comentarios,
                            EstudioId = Hijo.EstudioId,
                            DomicilioId = Hijo.DomicilioId,
                            EstatusVisita = Hijo.EstatusVisita,
                            EstadoCivilId = Hijo.EstadoCivilId,
                            DiscapacidadId = Hijo.DiscapacidadId,
                            TrabajadorId = Hijo.TrabajadorId,
                            EstatusInformacion = Hijo.EstatusInformacion,
                            EstatusUpdatedAt = Hijo.EstatusUpdatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                            PadreId = Hijo.PadreId,
                            HijoId = Hijo.HijoId,
                            MismoDomicilio = Hijo.MismoDomicilio,
                            ParentescoId = Hijo.ParentescoId,
                            DiscapacidadGradoId = Hijo.DiscapacidadGradoId,
                            CausaDiscapacidadId = Hijo.CausaDiscapacidadId,
                            GradoEstudioId = Hijo.GradoEstudioId,
                            Estatus = Hijo.Estatus,
                            CreatedAt = DateToMiddleDay(Hijo.CreatedAt),
                            Huellas = Hijo.Huellas,
                            Folio = Hijo.Folio,
                            NumIntegrante = Hijo.NumIntegrante,
                            NumFamilia = Hijo.NumFamilia,
                            UpdatedAt = Hijo.UpdatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                            DeletedAt = Hijo.DeletedAt?.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                            VersionAplicacion = Hijo.VersionAplicacion
                        });

                        if (Hijo.DomicilioId != null)
                        {
                            domicilios.Add(new DomicilioApiModel()
                            {
                                Id = Hijo.Domicilio.Id,
                                Email = Hijo.Domicilio.Email,
                                Telefono = Hijo.Domicilio.Telefono,
                                TelefonoCasa = Hijo.Domicilio.TelefonoCasa,
                                DomicilioN = Hijo.Domicilio.DomicilioN,
                                NumExterior = Hijo.Domicilio.NumExterior,
                                NumInterior = Hijo.Domicilio.NumInterior,
                                NumFamilia = Hijo.Domicilio.NumFamilia,
                                NumFamiliasRegistradas = Hijo.Domicilio.NumFamiliasRegistradas,
                                EntreCalle1 = Hijo.Domicilio.EntreCalle1,
                                EntreCalle2 = Hijo.Domicilio.EntreCalle2,
                                CallePosterior = Hijo.Domicilio.CallePosterior,
                                CodigoPostal = Hijo.Domicilio.CodigoPostal,
                                AgebId = Hijo.Domicilio.AgebId,
                                Latitud = Hijo.Domicilio.Latitud,
                                Longitud = Hijo.Domicilio.Longitud,
                                LatitudAtm = Hijo.Domicilio.LatitudAtm,
                                LongitudAtm = Hijo.Domicilio.LongitudAtm,
                                LocalidadId = Hijo.Domicilio.LocalidadId,
                                ManzanaId = Hijo.Domicilio.ManzanaId,
                                ColoniaId = Hijo.Domicilio.ColoniaId,
                                CalleId = Hijo.Domicilio.CalleId,
                                ZonaImpulsoId = Hijo.Domicilio.ZonaImpulsoId,
                                CarreteraId = Hijo.Domicilio.CarreteraId,
                                CaminoId = Hijo.Domicilio.CaminoId,
                                TipoAsentamientoId = Hijo.Domicilio.TipoAsentamientoId,
                                NombreAsentamiento = Hijo.Domicilio.NombreAsentamiento,
                                MunicipioId = Hijo.Domicilio.MunicipioId,
                                TrabajadorId = Hijo.Domicilio.TrabajadorId,
                                CadenaOCR = Hijo.Domicilio.CadenaOCR,
                                Porcentaje = Hijo.Domicilio.Porcentaje,
                                CreatedAt = Hijo.Domicilio.CreatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                                UpdatedAt = Hijo.Domicilio.UpdatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                                DeletedAt = Hijo.Domicilio.DeletedAt?.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                                VersionAplicacion = Hijo.Domicilio.VersionAplicacion,
                                AutorizaFotos = Hijo.Domicilio.AutorizaFotos
                            });
                        }

                        var Tutores = hijosTodos
                            .Where(x => x.HijoId != null && x.HijoId.Equals(Hijo.Id))
                            .ToList();

                        foreach (var Tutor in Tutores)
                        {
                            beneficiarios.Add(new BeneficiarioApiModel()
                            {
                                Id = Tutor.Id,
                                ApellidoPaterno = Tutor.ApellidoPaterno,
                                ApellidoMaterno = Tutor.ApellidoMaterno,
                                Nombre = Tutor.Nombre,
                                NombreCompleto = ObtenerNombreCompleto(Tutor),
                                FechaNacimiento = Tutor.FechaNacimiento?.AddHours(12.0)
                                    .ToString("yyyy-MM-ddTHH:mm:sszzz"),
                                EstadoId = Tutor.EstadoId,
                                Rfc = Tutor.Rfc,
                                Curp = Tutor.Curp,
                                SexoId = Tutor.SexoId,
                                Comentarios = Tutor.Comentarios,
                                EstudioId = Tutor.EstudioId,
                                DomicilioId = Tutor.DomicilioId,
                                EstatusVisita = Tutor.EstatusVisita,
                                EstadoCivilId = Tutor.EstadoCivilId,
                                DiscapacidadId = Tutor.DiscapacidadId,
                                TrabajadorId = Tutor.TrabajadorId,
                                EstatusInformacion = Tutor.EstatusInformacion,
                                EstatusUpdatedAt = Tutor.EstatusUpdatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                                PadreId = Tutor.PadreId,
                                HijoId = Tutor.HijoId,
                                MismoDomicilio = Tutor.MismoDomicilio,
                                ParentescoId = Tutor.ParentescoId,
                                DiscapacidadGradoId = Tutor.DiscapacidadGradoId,
                                CausaDiscapacidadId = Tutor.CausaDiscapacidadId,
                                GradoEstudioId = Tutor.GradoEstudioId,
                                Estatus = Tutor.Estatus,
                                Huellas = Tutor.Huellas,
                                Folio = Tutor.Folio,
                                NumIntegrante = Tutor.NumIntegrante,
                                NumFamilia = Tutor.NumFamilia,
                                CreatedAt = DateToMiddleDay(Tutor.CreatedAt),
                                UpdatedAt = Tutor.UpdatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                                DeletedAt = Tutor.DeletedAt?.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                                VersionAplicacion = Tutor.VersionAplicacion
                            });

                            if (Tutor.DomicilioId != null)
                            {
                                domicilios.Add(new DomicilioApiModel()
                                {
                                    Id = Tutor.Domicilio.Id,
                                    Email = Tutor.Domicilio.Email,
                                    Telefono = Tutor.Domicilio.Telefono,
                                    TelefonoCasa = Tutor.Domicilio.TelefonoCasa,
                                    DomicilioN = Tutor.Domicilio.DomicilioN,
                                    NumExterior = Tutor.Domicilio.NumExterior,
                                    NumInterior = Tutor.Domicilio.NumInterior,
                                    NumFamilia = Tutor.Domicilio.NumFamilia,
                                    NumFamiliasRegistradas = Tutor.Domicilio.NumFamiliasRegistradas,
                                    EntreCalle1 = Tutor.Domicilio.EntreCalle1,
                                    EntreCalle2 = Tutor.Domicilio.EntreCalle2,
                                    CallePosterior = Tutor.Domicilio.CallePosterior,
                                    CodigoPostal = Tutor.Domicilio.CodigoPostal,
                                    AgebId = Tutor.Domicilio.AgebId,
                                    Latitud = Tutor.Domicilio.Latitud,
                                    Longitud = Tutor.Domicilio.Longitud,
                                    LatitudAtm = Tutor.Domicilio.LatitudAtm,
                                    LongitudAtm = Tutor.Domicilio.LongitudAtm,
                                    LocalidadId = Tutor.Domicilio.LocalidadId,
                                    ManzanaId = Tutor.Domicilio.ManzanaId,
                                    ColoniaId = Tutor.Domicilio.ColoniaId,
                                    CalleId = Tutor.Domicilio.CalleId,
                                    ZonaImpulsoId = Tutor.Domicilio.ZonaImpulsoId,
                                    CarreteraId = Tutor.Domicilio.CarreteraId,
                                    CaminoId = Tutor.Domicilio.CaminoId,
                                    TipoAsentamientoId = Tutor.Domicilio.TipoAsentamientoId,
                                    NombreAsentamiento = Tutor.Domicilio.NombreAsentamiento,
                                    MunicipioId = Tutor.Domicilio.MunicipioId,
                                    TrabajadorId = Tutor.Domicilio.TrabajadorId,
                                    CadenaOCR = Tutor.Domicilio.CadenaOCR,
                                    Porcentaje = Tutor.Domicilio.Porcentaje,
                                    CreatedAt = Tutor.Domicilio.CreatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                                    UpdatedAt = Tutor.Domicilio.UpdatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                                    DeletedAt = Tutor.Domicilio.DeletedAt?.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                                    VersionAplicacion = Tutor.Domicilio.VersionAplicacion,
                                    AutorizaFotos = Tutor.Domicilio.AutorizaFotos
                                });
                            }
                        }
                    }
                }

                api.Beneficiarios = beneficiarios;
                api.Domicilios = domicilios;
                api.Aplicaciones = new List<AplicacionApiModel>();
                api.Incidencias = new List<IncidenciaApiModel>();

                var grupoBeneficiarios =
                    Lista.ChunkData(beneficiarios.Where(b => b.PadreId == null).Select(x => x.Id).ToList(), 2000);
                foreach (var ids in grupoBeneficiarios)
                {
                    var aplicaciones = _context.Aplicacion.Where(a => ids.Contains(a.BeneficiarioId) && a.Activa)
                        .ToList();
                    foreach (var aplicacion in aplicaciones)
                    {
                        var apiAplicacion = new AplicacionApiModel
                        {
                            Id = aplicacion.Id,
                            Estatus = aplicacion.Estatus,
                            Resultado = aplicacion.Resultado,
                            BeneficiarioId = aplicacion.BeneficiarioId,
                            EncuestaVersionId = aplicacion.EncuestaVersionId,
                            TrabajadorId = aplicacion.TrabajadorId,
                            NumeroAplicacion = aplicacion.NumeroAplicacion,
                            NumeroIntegrantes = aplicacion.NumeroIntegrantes,
                            NumeroEducativas = aplicacion.NumeroEducativas,
                            NumeroSalud = aplicacion.NumeroSalud,
                            NumeroSocial = aplicacion.NumeroSocial,
                            NumeroCarencias = aplicacion.NumeroCarencias,
                            FechaInicio = aplicacion.FechaInicio?.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                            FechaFin = aplicacion.FechaFin?.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                            Tiempo = aplicacion.Tiempo,
                            Activa = aplicacion.Activa,
                            CreatedAt = aplicacion.CreatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                            UpdatedAt = aplicacion.UpdatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                            DeletedAt = aplicacion.DeletedAt?.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                            Educativa = aplicacion.Educativa,
                            Analfabetismo = aplicacion.Analfabetismo,
                            Inasistencia = aplicacion.Inasistencia,
                            PrimariaIncompleta = aplicacion.PrimariaIncompleta,
                            SecundariaIncompleta = aplicacion.SecundariaIncompleta,
                            ServicioSalud = aplicacion.ServicioSalud,
                            SeguridadSocial = aplicacion.SeguridadSocial,
                            Vivienda = aplicacion.Vivienda,
                            Piso = aplicacion.Piso,
                            Techo = aplicacion.Techo,
                            Muro = aplicacion.Muro,
                            Hacinamiento = aplicacion.Hacinamiento,
                            Servicios = aplicacion.Servicios,
                            Agua = aplicacion.Agua,
                            Drenaje = aplicacion.Drenaje,
                            Electricidad = aplicacion.Electricidad,
                            Combustible = aplicacion.Combustible,
                            Alimentaria = aplicacion.Alimentaria,
                            GradoAlimentaria = aplicacion.GradoAlimentaria,
                            Pea = aplicacion.Pea,
                            Ingreso = aplicacion.Ingreso,
                            LineaBienestar = aplicacion.LineaBienestar,
                            VersionAlgoritmo = aplicacion.VersionAlgoritmo,
                            NivelPobreza = aplicacion.NivelPobreza,
                            TieneActa = aplicacion.TieneActa,
                            SeguridadPublica = aplicacion.SeguridadPublica,
                            SeguridadParque = aplicacion.SeguridadParque,
                            ConfianzaLiderez = aplicacion.ConfianzaLiderez,
                            ConfianzaInstituciones = aplicacion.ConfianzaInstituciones,
                            Movilidad = aplicacion.Movilidad,
                            RedesSociales = aplicacion.RedesSociales,
                            Tejido = aplicacion.Tejido,
                            Satisfaccion = aplicacion.Satisfaccion,
                            PropiedadVivienda = aplicacion.PropiedadVivienda,
                            AplicacionPreguntas = new List<AplicacionPreguntaApiModel>(),
                            AplicacionCarencias = new List<AplicacionCarenciaApiModel>(),
                            VersionAplicacion = aplicacion.VersionAplicacion
                        };
                        if (aplicacion.Estatus == EstatusAplicacion.INCOMPLETO)
                        {
                            var aplicacionPreguntas = _context.AplicacionPregunta
                                .Where(a => a.AplicacionId.Equals(aplicacion.Id)).ToList();
                            foreach (var ap in aplicacionPreguntas)
                            {
                                apiAplicacion.AplicacionPreguntas.Add((new AplicacionPreguntaApiModel()
                                {
                                    Id = ap.Id,
                                    Valor = ap.Valor,
                                    AplicacionId = ap.AplicacionId,
                                    PreguntaId = ap.PreguntaId,
                                    RespuestaId = ap.RespuestaId,
                                    RespuestaValor = ap.RespuestaValor,
                                    RespuestaIteracion = ap.RespuestaIteracion,
                                    Grado = ap.Grado,
                                    Complemento = ap.Complemento,
                                    ValorNumerico = ap.ValorNumerico,
                                    ValorFecha = ap.ValorFecha?.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                                    ValorCatalogo = ap.ValorCatalogo,
                                    CreatedAt = ap.CreatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                                    UpdatedAt = ap.UpdatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                                    DeletedAt = ap.DeletedAt?.ToString("yyyy-MM-ddTHH:mm:sszzz")
                                }));
                            }
                        }
                        else
                        {
                            var aplicacionCarencias = _context.AplicacionCarencia
                                .Where(a => a.AplicacionId.Equals(aplicacion.Id)).ToList();
                            foreach (var ac in aplicacionCarencias)
                            {
                                apiAplicacion.AplicacionCarencias.Add(new AplicacionCarenciaApiModel
                                {
                                    Id = ac.Id,
                                    BeneficiarioId = ac.BeneficiarioId,
                                    AplicacionId = ac.AplicacionId,
                                    Educativa = ac.Educativa,
                                    Analfabetismo = ac.Analfabetismo,
                                    Inasistencia = ac.Inasistencia,
                                    PrimariaIncompleta = ac.PrimariaIncompleta,
                                    SecundariaIncompleta = ac.SecundariaIncompleta,
                                    ServicioSalud = ac.ServicioSalud,
                                    SeguridadSocial = ac.SeguridadSocial,
                                    Vivienda = ac.Vivienda,
                                    Piso = ac.Piso,
                                    Techo = ac.Techo,
                                    Muro = ac.Muro,
                                    Hacinamiento = ac.Hacinamiento,
                                    Servicios = ac.Servicios,
                                    Agua = ac.Agua,
                                    Drenaje = ac.Drenaje,
                                    Electricidad = ac.Electricidad,
                                    Combustible = ac.Combustible,
                                    Alimentaria = ac.Alimentaria,
                                    GradoAlimentaria = ac.GradoAlimentaria,
                                    Pea = ac.Pea,
                                    ParentescoId = ac.ParentescoId,
                                    SexoId = ac.SexoId,
                                    Discapacidad = ac.Discapacidad,
                                    GradoEducativa = ac.GradoEducativa,
                                    Edad = ac.Edad,
                                    Ingreso = ac.Ingreso,
                                    LineaBienestar = ac.LineaBienestar,
                                    VersionAlgoritmo = ac.VersionAlgoritmo,
                                    NumIntegrante = ac.NumIntegrante,
                                    NivelPobreza = ac.NivelPobreza,
                                    TieneActa = ac.TieneActa,
                                    CreatedAt = ac.CreatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                                    UpdatedAt = ac.UpdatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                                    DeletedAt = ac.DeletedAt?.ToString("yyyy-MM-ddTHH:mm:sszzz")
                                });
                            }
                        }

                        api.Aplicaciones.Add(apiAplicacion);
                    }

                    var incidencias = _context.Incidencias.Where(a => ids.Contains(a.BeneficiarioId)).ToList();
                    foreach (var incidencia in incidencias)
                    {
                        api.Incidencias.Add(new IncidenciaApiModel
                        {
                            Id = incidencia.Id,
                            Observaciones = incidencia.Observaciones,
                            CreatedAt = incidencia.CreatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                            UpdatedAt = incidencia.UpdatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                            DeletedAt = incidencia.DeletedAt?.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                            TrabajadorId = incidencia.TrabajadorId,
                            TipoIncidenciaId = incidencia.TipoIncidenciaId,
                            BeneficiarioId = incidencia.BeneficiarioId,
                        });
                    }
                }
            }
            else
            {
                api.Beneficiarios = new List<BeneficiarioApiModel>();
                api.Domicilios = new List<DomicilioApiModel>();
                api.Aplicaciones = new List<AplicacionApiModel>();
            }

            api.Estados = first ? new List<EstadoApiModel>() : GetEstados(lastSync, first);
            api.Municipios = first ? new List<MunicipioApiModel>() : GetMunicipios(lastSync, first);
            api.Localidades = first ? new List<LocalidadApiModel>() : GetLocalidades(lastSync, first);
            api.Dependencias = first ? new List<DependenciaApiModel>() : GetDependencias(lastSync, first);
            api.Zonas = first ? new List<ZonaApiModel>() : GetZonas(lastSync, first);
            api.MunicipiosZonas = first ? new List<MunicipioZonaApiModel>() : GetMunicipiosZonas(lastSync, first);
            api.Agebs = first ? new List<AgebApiModel>() : GetAgebs(lastSync, null);
            api.Manzanas = first ? new List<ManzanaApiModel>() : GetManzanas(lastSync, null);
            api.Colonias = first ? new List<ColoniaApiModel>() : GetColonias(lastSync, null);
            api.Calles = first ? new List<CalleApiModel>() : GetCalles(lastSync, null);

            api.SyncedAt = DateTime.Now;
            return JsonConvert.SerializeObject(api);
        }

        /// <summary>
        /// Funcion que construye el nombre completo del beneficiario de acuerdo a su nombre, primer apellido y segundo apellido
        /// </summary>
        /// <param name="beneficiario">Objeto de base de datos del beneficiario</param>
        /// <returns>Cadena con el nombre completo del beneficiario</returns>
        private string ObtenerNombreCompleto(Beneficiario beneficiario)
        {
            return beneficiario.ApellidoPaterno + " " +
                   (!string.IsNullOrEmpty(beneficiario.ApellidoMaterno) ? beneficiario.ApellidoMaterno + " " : "") +
                   beneficiario.Nombre;
        }

        /// <summary>
        /// Funcion para obtener una fecha con hora del medio día
        /// </summary>
        /// <param name="date">Fecha de creación del módelo</param>
        /// <returns>Fecha con hora del medio día</returns>
        private string DateToMiddleDay(DateTime date)
        {
            date = new DateTime(date.Year, date.Month, date.Day, 12, 0, 0);

            return date.ToString("yyyy-MM-ddTHH:mm:sszzz");
        }

        /// <summary>
        /// Funcion para obtener una fecha sin horas
        /// </summary>
        /// <param name="date">Fecha de creación del módelo</param>
        /// <returns>Fecha con hora del medio día</returns>
        private DateTime StartOfDay(DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
        }

        /// <summary>
        /// Funcion que convierte a radianes el grado especificado
        /// </summary>
        /// <param name="val">Grado</param>
        /// <returns>Radianes</returns>
        private static double ToRadians(double val)
        {
            return (Math.PI / 180) * val;
        }

        /// <summary>
        /// Función que ingresa o acutaliza en la base de datos la información capturada por los promotores en los dispositivos móviles
        /// </summary>
        /// <param name="request">Objeto que contiene la información del beneficiario</param>
        /// <returns>Estatus de registro en la base de datos</returns>
        [DisableRequestSizeLimit]
        [HttpPost]
        public async Task<string> Beneficiarios([FromForm] RequestBeneficiarios request)
        {
            var response = new ResponseBeneficiarios();
            try
            {
                var beneficiario = JsonConvert.DeserializeObject<BeneficiarioApiModel>(request.Beneficiario);
                var hijos = JsonConvert.DeserializeObject<List<BeneficiarioApiModel>>(request.Hijos);
                var domicilio = JsonConvert.DeserializeObject<DomicilioApiModel>(request.Domicilio);
                var domiciliosHijos = JsonConvert.DeserializeObject<List<DomicilioApiModel>>(request.DomiciliosHijos);
                var aplicaciones = JsonConvert.DeserializeObject<List<AplicacionApiModel>>(request.Aplicaciones);
                var incidencias = JsonConvert.DeserializeObject<List<IncidenciaApiModel>>(request.Incidencias);
                var aplicacionesTerminadas =
                    new List<string>(); //Listado que almacena los ids de la aplicaciones concluidas para poder notificar a los administradores

                var aplicacionesHash = _context.Aplicacion.Where(a => a.BeneficiarioId.Equals(beneficiario.Id))
                    .ToDictionary(x => x.Id);
                var aplicacionesPreguntasHash = _context.AplicacionPregunta
                    .Where(ap => ap.Aplicacion.BeneficiarioId.Equals(beneficiario.Id)).ToDictionary(x => x.Id);
                var AplicacionesCarenciasHash = _context.AplicacionCarencia
                    .Where(ap => ap.Aplicacion.BeneficiarioId.Equals(beneficiario.Id)).ToDictionary(x => x.Id);

                using (var transaction = _context.Database.BeginTransaction())
                {
                    var domicilioDB = _context.Domicilio.FirstOrDefault(x => x.Activa && x.Id.Equals(domicilio.Id));
                    var nuevoDomicilio = false;
                    if (domicilioDB == null)
                    {
                        domicilioDB = new Domicilio();
                        nuevoDomicilio = true;
                    }

                    domicilioDB.Id = domicilio.Id;
                    domicilioDB.Email = domicilio.Email;
                    domicilioDB.Telefono = domicilio.Telefono;
                    domicilioDB.TelefonoCasa = domicilio.TelefonoCasa;
                    domicilioDB.DomicilioN = domicilio.DomicilioN;
                    domicilioDB.NumExterior = domicilio.NumExterior;
                    domicilioDB.NumInterior = domicilio.NumInterior;
                    domicilioDB.NumFamilia = domicilio.NumFamilia;
                    domicilioDB.NumFamiliasRegistradas = domicilio.NumFamiliasRegistradas;
                    domicilioDB.EntreCalle1 = domicilio.EntreCalle1;
                    domicilioDB.EntreCalle2 = domicilio.EntreCalle2;
                    domicilioDB.CallePosterior = domicilio.CallePosterior;
                    domicilioDB.CodigoPostal = domicilio.CodigoPostal;
                    domicilioDB.AgebId = domicilio.AgebId;
                    domicilioDB.Latitud = domicilio.Latitud;
                    domicilioDB.Longitud = domicilio.Longitud;
                    domicilioDB.LatitudAtm = domicilio.LatitudAtm;
                    domicilioDB.LongitudAtm = domicilio.LongitudAtm;
                    domicilioDB.MunicipioId = domicilio.MunicipioId;
                    domicilioDB.LocalidadId = domicilio.LocalidadId;
                    domicilioDB.TrabajadorId = domicilio.TrabajadorId;
                    domicilioDB.CadenaOCR = domicilio.CadenaOCR;
                    domicilioDB.Porcentaje = domicilio.Porcentaje;
                    domicilioDB.ManzanaId = domicilio.ManzanaId;
                    domicilioDB.ColoniaId = domicilio.ColoniaId;
                    domicilioDB.CalleId = domicilio.CalleId;
                    domicilioDB.ZonaImpulsoId = domicilio.ZonaImpulsoId;
                    domicilioDB.CarreteraId = domicilio.CarreteraId;
                    domicilioDB.CaminoId = domicilio.CaminoId;
                    domicilioDB.TipoAsentamientoId = domicilio.TipoAsentamientoId;
                    domicilioDB.NombreAsentamiento = domicilio.NombreAsentamiento;
                    domicilioDB.Activa = true;
                    domicilioDB.UpdatedAt = DateTime.Parse(domicilio.UpdatedAt);
                    domicilioDB.CreatedAt = DateTime.Parse(domicilio.CreatedAt);
                    domicilioDB.DeviceId = domicilio.DeviceId;
                    domicilioDB.VersionAplicacion = domicilio.VersionAplicacion;
                    domicilioDB.AutorizaFotos = domicilio.AutorizaFotos;

                    if (nuevoDomicilio)
                    {
                        _context.Domicilio.Add(domicilioDB);
                    }
                    else
                    {
                        _context.Domicilio.Update(domicilioDB);
                    }

                    _context.SaveChanges();

                    var beneficiarioDB =
                        _context.Beneficiario.FirstOrDefault(b => b.DeletedAt == null && b.Id == beneficiario.Id);
                    var nuevoBeneficiario = false;
                    if (beneficiarioDB == null)
                    {
                        beneficiarioDB = new Beneficiario();
                        nuevoBeneficiario = true;
                    }

                    beneficiarioDB.Id = beneficiario.Id;
                    beneficiarioDB.ApellidoPaterno = beneficiario.ApellidoPaterno;
                    beneficiarioDB.ApellidoMaterno = beneficiario.ApellidoMaterno;
                    beneficiarioDB.Nombre = beneficiario.Nombre;
                    beneficiarioDB.FechaNacimiento = StartOfDay(DateTime.Parse(beneficiario.FechaNacimiento));
                    beneficiarioDB.EstadoId = beneficiario.EstadoId;
                    beneficiarioDB.Rfc = beneficiario.Rfc;
                    beneficiarioDB.Curp = beneficiario.Curp;
                    beneficiarioDB.SexoId = beneficiario.SexoId;
                    beneficiarioDB.Comentarios = beneficiario.Comentarios;
                    beneficiarioDB.EstudioId = beneficiario.EstudioId;
                    beneficiarioDB.DomicilioId = beneficiario.DomicilioId;
                    beneficiarioDB.EstatusVisita = beneficiario.EstatusVisita;
                    beneficiarioDB.EstadoCivilId = beneficiario.EstadoCivilId;
                    beneficiarioDB.DiscapacidadId = beneficiario.DiscapacidadId;
                    beneficiarioDB.GradoEstudioId = beneficiario.GradoEstudioId;
                    beneficiarioDB.DiscapacidadGradoId = beneficiario.DiscapacidadGradoId;
                    beneficiarioDB.CausaDiscapacidadId = beneficiario.CausaDiscapacidadId;
                    beneficiarioDB.TrabajadorId = beneficiario.TrabajadorId;
                    beneficiarioDB.EstatusInformacion = beneficiario.EstatusInformacion;
                    beneficiarioDB.EstatusUpdatedAt = DateTime.Parse(beneficiario.EstatusUpdatedAt);
                    beneficiarioDB.Estatus = beneficiario.Estatus;
                    beneficiarioDB.Huellas = beneficiario.Huellas;
                    beneficiarioDB.Folio = beneficiario.Folio;
                    beneficiarioDB.NumIntegrante = beneficiario.NumIntegrante;
                    beneficiarioDB.NumFamilia = beneficiario.NumFamilia;
                    beneficiarioDB.CreatedAt = DateTime.Parse(beneficiario.CreatedAt);
                    beneficiarioDB.UpdatedAt = DateTime.Parse(beneficiario.UpdatedAt);
                    beneficiarioDB.DeviceId = beneficiario.DeviceId;
                    beneficiarioDB.VersionAplicacion = beneficiario.VersionAplicacion;
                    beneficiarioDB.MismoDomicilio = beneficiario.MismoDomicilio;
                    beneficiarioDB.ParentescoId = beneficiario.ParentescoId;

                    if (nuevoBeneficiario)
                    {
                        _context.Beneficiario.Add(beneficiarioDB);
                    }
                    else
                    {
                        _context.Beneficiario.Update(beneficiarioDB);
                    }

                    _context.SaveChanges();

                    foreach (var Hijo in hijos.OrderByDescending(x => x.PadreId))
                    {
                        var hijoDB =
                            _context.Beneficiario.FirstOrDefault(b => b.DeletedAt == null && b.Id.Equals(Hijo.Id));
                        var nuevoHijo = false;
                        if (hijoDB == null)
                        {
                            nuevoHijo = true;
                            hijoDB = new Beneficiario();
                        }

                        hijoDB.Id = Hijo.Id;
                        hijoDB.ApellidoPaterno = Hijo.ApellidoPaterno;
                        hijoDB.ApellidoMaterno = Hijo.ApellidoMaterno;
                        hijoDB.Nombre = Hijo.Nombre;
                        hijoDB.FechaNacimiento = StartOfDay(DateTime.Parse(Hijo.FechaNacimiento));
                        hijoDB.EstadoId = Hijo.EstadoId;
                        hijoDB.Rfc = Hijo.Rfc;
                        hijoDB.Curp = Hijo.Curp;
                        hijoDB.SexoId = Hijo.SexoId;
                        hijoDB.Comentarios = Hijo.Comentarios;
                        hijoDB.EstudioId = Hijo.EstudioId;
                        hijoDB.DomicilioId = Hijo.DomicilioId;
                        hijoDB.EstatusVisita = Hijo.EstatusVisita;
                        hijoDB.GradoEstudioId = Hijo.GradoEstudioId;
                        hijoDB.EstadoCivilId = Hijo.EstadoCivilId;
                        hijoDB.DiscapacidadId = Hijo.DiscapacidadId;
                        hijoDB.DiscapacidadGradoId = Hijo.DiscapacidadGradoId;
                        hijoDB.CausaDiscapacidadId = Hijo.CausaDiscapacidadId;
                        hijoDB.TrabajadorId = Hijo.TrabajadorId;
                        hijoDB.PadreId = Hijo.PadreId;
                        hijoDB.HijoId = Hijo.HijoId;
                        hijoDB.MismoDomicilio = Hijo.MismoDomicilio;
                        hijoDB.ParentescoId = Hijo.ParentescoId;
                        hijoDB.EstatusInformacion = Hijo.EstatusInformacion;
                        hijoDB.EstatusUpdatedAt = DateTime.Parse(Hijo.EstatusUpdatedAt);
                        hijoDB.Estatus = Hijo.Estatus;
                        hijoDB.CreatedAt = DateTime.Parse(Hijo.CreatedAt);
                        hijoDB.UpdatedAt = DateTime.Now;
                        hijoDB.Huellas = Hijo.Huellas;
                        hijoDB.Folio = Hijo.Folio;
                        hijoDB.NumIntegrante = Hijo.NumIntegrante;
                        hijoDB.NumFamilia = Hijo.NumFamilia;
                        hijoDB.DeviceId = Hijo.DeviceId;
                        hijoDB.VersionAplicacion = Hijo.VersionAplicacion;

                        if (nuevoHijo)
                        {
                            _context.Beneficiario.Add(hijoDB);
                        }
                        else
                        {
                            _context.Beneficiario.Update(hijoDB);
                        }

                        _context.SaveChanges();
                    }

                    foreach (var domicilioHijo in domiciliosHijos)
                    {
                        var domicilioHijoDB =
                            _context.Domicilio.FirstOrDefault(d => d.DeletedAt == null && d.Id == domicilioHijo.Id);
                        var nuevoDomicilioHijo = false;
                        if (domicilioHijoDB == null)
                        {
                            domicilioHijoDB = new Domicilio();
                            nuevoDomicilioHijo = false;
                        }

                        domicilioHijoDB.Id = domicilioHijo.Id;
                        domicilioHijoDB.Email = domicilioHijo.Email;
                        domicilioHijoDB.Telefono = domicilioHijo.Telefono;
                        domicilioHijoDB.TelefonoCasa = domicilioHijo.TelefonoCasa;
                        domicilioHijoDB.DomicilioN = domicilioHijo.DomicilioN;
                        domicilioHijoDB.NumExterior = domicilioHijo.NumExterior;
                        domicilioHijoDB.NumInterior = domicilioHijo.NumInterior;
                        domicilioHijoDB.NumFamilia = domicilioHijo.NumFamilia;
                        domicilioHijoDB.NumFamiliasRegistradas = domicilioHijo.NumFamiliasRegistradas;
                        domicilioHijoDB.EntreCalle1 = domicilioHijo.EntreCalle1;
                        domicilioHijoDB.EntreCalle2 = domicilioHijo.EntreCalle2;
                        domicilioHijoDB.CallePosterior = domicilioHijo.CallePosterior;
                        domicilioHijoDB.CodigoPostal = domicilioHijo.CodigoPostal;
                        domicilioHijoDB.AgebId = domicilioHijo.AgebId;
                        domicilioHijoDB.Latitud = domicilioHijo.Latitud;
                        domicilioHijoDB.Longitud = domicilioHijo.Longitud;
                        domicilioHijoDB.LatitudAtm = domicilioHijo.LatitudAtm;
                        domicilioHijoDB.LongitudAtm = domicilioHijo.LongitudAtm;
                        domicilioHijoDB.MunicipioId = domicilioHijo.MunicipioId;
                        domicilioHijoDB.LocalidadId = domicilioHijo.LocalidadId;
                        domicilioHijoDB.TrabajadorId = domicilioHijo.TrabajadorId;
                        domicilioHijoDB.CadenaOCR = domicilioHijo.CadenaOCR;
                        domicilioHijoDB.Porcentaje = domicilioHijo.Porcentaje;
                        domicilioHijoDB.ManzanaId = domicilioHijo.ManzanaId;
                        domicilioHijoDB.ColoniaId = domicilioHijo.ColoniaId;
                        domicilioHijoDB.CalleId = domicilioHijo.CalleId;
                        domicilioHijoDB.ZonaImpulsoId = domicilioHijo.ZonaImpulsoId;
                        domicilioHijoDB.CarreteraId = domicilioHijo.CarreteraId;
                        domicilioHijoDB.CaminoId = domicilioHijo.CaminoId;
                        domicilioHijoDB.TipoAsentamientoId = domicilioHijo.TipoAsentamientoId;
                        domicilioHijoDB.NombreAsentamiento = domicilioHijo.NombreAsentamiento;
                        domicilioHijoDB.Activa = true;
                        domicilioHijoDB.CreatedAt = DateTime.Parse(domicilioHijo.CreatedAt);
                        domicilioHijoDB.UpdatedAt = DateTime.Now;
                        domicilioHijoDB.DeletedAt = domicilioHijo.DeletedAt != null
                            ? DateTime.Parse(domicilioHijo.DeletedAt)
                            : (DateTime?) null;
                        domicilioHijoDB.DeviceId = domicilioHijo.DeviceId;
                        domicilioHijoDB.VersionAplicacion = domicilioHijo.VersionAplicacion;
                        domicilioHijoDB.AutorizaFotos = domicilioHijo.AutorizaFotos;

                        if (nuevoDomicilioHijo)
                        {
                            _context.Domicilio.Add(domicilioHijoDB);
                        }
                        else
                        {
                            _context.Domicilio.Update(domicilioHijoDB);
                        }
                    }

                    _context.SaveChanges();

                    var now = DateTime.Now;
                    foreach (var aplicacion in aplicaciones)
                    {
                        Aplicacion aplicacionDB;
                        var nuevaAplicacion = true;
                        if (aplicacionesHash.TryGetValue(aplicacion.Id, out aplicacionDB))
                        {
                            nuevaAplicacion = false;
                            if (aplicacion.Estatus == EstatusAplicacion.COMPLETO &&
                                aplicacionDB.Estatus != EstatusAplicacion.COMPLETO)
                            {
                                aplicacionesTerminadas.Add(aplicacion.Id);
                            }
                        }
                        else
                        {
                            aplicacionDB = new Aplicacion();
                        }

                        aplicacionDB.Id = aplicacion.Id;
                        aplicacionDB.Estatus = aplicacion.Estatus;
                        aplicacionDB.Resultado = aplicacion.Resultado;
                        aplicacionDB.Activa = aplicacion.Activa;
                        aplicacionDB.BeneficiarioId = aplicacion.BeneficiarioId;
                        aplicacionDB.EncuestaVersionId = aplicacion.EncuestaVersionId;
                        aplicacionDB.TrabajadorId = aplicacion.TrabajadorId;
                        aplicacionDB.NumeroAplicacion = aplicacion.NumeroAplicacion;
                        aplicacionDB.NumeroIntegrantes = aplicacion.NumeroIntegrantes;
                        aplicacionDB.NumeroEducativas = aplicacion.NumeroEducativas;
                        aplicacionDB.NumeroSalud = aplicacion.NumeroSalud;
                        aplicacionDB.NumeroSocial = aplicacion.NumeroSocial;
                        aplicacionDB.NumeroCarencias = aplicacion.NumeroCarencias;
                        aplicacionDB.FechaInicio = aplicacion.FechaInicio != null
                            ? DateTime.Parse(aplicacion.FechaInicio)
                            : (DateTime?) null;
                        aplicacionDB.FechaFin = aplicacion.FechaFin != null
                            ? DateTime.Parse(aplicacion.FechaFin)
                            : (DateTime?) null;
                        aplicacionDB.Tiempo = aplicacion.Tiempo;
                        aplicacionDB.CreatedAt = DateTime.Parse(aplicacion.CreatedAt);
                        aplicacionDB.UpdatedAt = DateTime.Parse(aplicacion.UpdatedAt);
                        aplicacionDB.DeletedAt = aplicacion.DeletedAt != null
                            ? DateTime.Parse(aplicacion.DeletedAt)
                            : (DateTime?) null;
                        aplicacionDB.FechaSincronizacion = now;
                        aplicacionDB.Educativa = aplicacion.Educativa;
                        aplicacionDB.Analfabetismo = aplicacion.Analfabetismo;
                        aplicacionDB.Inasistencia = aplicacion.Inasistencia;
                        aplicacionDB.PrimariaIncompleta = aplicacion.PrimariaIncompleta;
                        aplicacionDB.SecundariaIncompleta = aplicacion.SecundariaIncompleta;
                        aplicacionDB.ServicioSalud = aplicacion.ServicioSalud;
                        aplicacionDB.SeguridadSocial = aplicacion.SeguridadSocial;
                        aplicacionDB.Vivienda = aplicacion.Vivienda;
                        aplicacionDB.Piso = aplicacion.Piso;
                        aplicacionDB.Techo = aplicacion.Techo;
                        aplicacionDB.Muro = aplicacion.Muro;
                        aplicacionDB.Hacinamiento = aplicacion.Hacinamiento;
                        aplicacionDB.Servicios = aplicacion.Servicios;
                        aplicacionDB.Agua = aplicacion.Agua;
                        aplicacionDB.Drenaje = aplicacion.Drenaje;
                        aplicacionDB.Electricidad = aplicacion.Electricidad;
                        aplicacionDB.Combustible = aplicacion.Combustible;
                        aplicacionDB.Alimentaria = aplicacion.Alimentaria;
                        aplicacionDB.GradoAlimentaria = aplicacion.GradoAlimentaria;
                        aplicacionDB.Pea = aplicacion.Pea;
                        aplicacionDB.Ingreso = aplicacion.Ingreso;
                        aplicacionDB.LineaBienestar = aplicacion.LineaBienestar;
                        aplicacionDB.VersionAlgoritmo = aplicacion.VersionAlgoritmo;
                        aplicacionDB.NivelPobreza = aplicacion.NivelPobreza;
                        aplicacionDB.TieneActa = aplicacion.TieneActa;
                        aplicacionDB.SeguridadPublica = aplicacion.SeguridadPublica;
                        aplicacionDB.SeguridadParque = aplicacion.SeguridadParque;
                        aplicacionDB.ConfianzaLiderez = aplicacion.ConfianzaLiderez;
                        aplicacionDB.ConfianzaInstituciones = aplicacion.ConfianzaInstituciones;
                        aplicacionDB.Movilidad = aplicacion.Movilidad;
                        aplicacionDB.RedesSociales = aplicacion.RedesSociales;
                        aplicacionDB.Tejido = aplicacion.Tejido;
                        aplicacionDB.Satisfaccion = aplicacion.Satisfaccion;
                        aplicacionDB.PropiedadVivienda = aplicacion.PropiedadVivienda;
                        aplicacionDB.DeviceId = aplicacion.DeviceId;
                        aplicacionDB.Tiempo = aplicacion.Tiempo;
                        aplicacionDB.VersionAplicacion = aplicacion.VersionAplicacion;

                        if (nuevaAplicacion)
                        {
                            if (aplicacion.Estatus == EstatusAplicacion.COMPLETO)
                            {
                                aplicacionesTerminadas.Add(aplicacion.Id);
                            }

                            _context.Aplicacion.Add(aplicacionDB);
                        }
                        else
                        {
                            _context.Aplicacion.Update(aplicacionDB);
                        }

                        _context.SaveChanges();

                        foreach (var aplicacionPregunta in aplicacion.AplicacionPreguntas)
                        {
                            AplicacionPregunta aplicacionPreguntaDB;
                            nuevaAplicacion = true;
                            if (aplicacionesPreguntasHash.TryGetValue(aplicacionPregunta.Id, out aplicacionPreguntaDB))
                            {
                                nuevaAplicacion = false;
                            }
                            else
                            {
                                aplicacionPreguntaDB = new AplicacionPregunta();
                            }

                            aplicacionPreguntaDB.Id = aplicacionPregunta.Id;
                            aplicacionPreguntaDB.AplicacionId = aplicacionPregunta.AplicacionId;
                            aplicacionPreguntaDB.PreguntaId = aplicacionPregunta.PreguntaId;
                            aplicacionPreguntaDB.RespuestaId = aplicacionPregunta.RespuestaId;
                            aplicacionPreguntaDB.RespuestaValor = aplicacionPregunta.RespuestaValor;
                            aplicacionPreguntaDB.RespuestaIteracion = aplicacionPregunta.RespuestaIteracion;
                            aplicacionPreguntaDB.Grado = aplicacionPregunta.Grado;
                            aplicacionPreguntaDB.Complemento = aplicacionPregunta.Complemento;
                            aplicacionPreguntaDB.Valor = aplicacionPregunta.Valor;
                            aplicacionPreguntaDB.ValorNumerico = aplicacionPregunta.ValorNumerico;
                            aplicacionPreguntaDB.ValorFecha = aplicacionPregunta.ValorFecha == null
                                ? (DateTime?) null
                                : DateTime.Parse(aplicacionPregunta.ValorFecha);
                            aplicacionPreguntaDB.ValorCatalogo = aplicacionPregunta.ValorCatalogo;
                            aplicacionPreguntaDB.CreatedAt = DateTime.Parse(aplicacionPregunta.CreatedAt);
                            aplicacionPreguntaDB.UpdatedAt = DateTime.Parse(aplicacionPregunta.UpdatedAt);
                            aplicacionPreguntaDB.DeletedAt = aplicacionPregunta.DeletedAt != null
                                ? DateTime.Parse(aplicacionPregunta.DeletedAt)
                                : (DateTime?) null;
                            if (nuevaAplicacion)
                            {
                                _context.AplicacionPregunta.Add(aplicacionPreguntaDB);
                            }
                            else
                            {
                                _context.AplicacionPregunta.Update(aplicacionPreguntaDB);
                            }
                        }

                        _context.SaveChanges();

                        foreach (var aplicacionCarencia in aplicacion.AplicacionCarencias)
                        {
                            AplicacionCarencia aplicacionCarenciaDB;
                            nuevaAplicacion = true;
                            if (AplicacionesCarenciasHash.TryGetValue(aplicacionCarencia.Id, out aplicacionCarenciaDB))
                            {
                                nuevaAplicacion = false;
                            }
                            else
                            {
                                aplicacionCarenciaDB = new AplicacionCarencia();
                            }

                            aplicacionCarenciaDB.Id = aplicacionCarencia.Id;
                            aplicacionCarenciaDB.BeneficiarioId = aplicacionCarencia.BeneficiarioId;
                            aplicacionCarenciaDB.AplicacionId = aplicacionCarencia.AplicacionId;
                            aplicacionCarenciaDB.Educativa = aplicacionCarencia.Educativa;
                            aplicacionCarenciaDB.TieneActa = aplicacionCarencia.TieneActa;
                            aplicacionCarenciaDB.Analfabetismo = aplicacionCarencia.Analfabetismo;
                            aplicacionCarenciaDB.Inasistencia = aplicacionCarencia.Inasistencia;
                            aplicacionCarenciaDB.PrimariaIncompleta = aplicacionCarencia.PrimariaIncompleta;
                            aplicacionCarenciaDB.SecundariaIncompleta = aplicacionCarencia.SecundariaIncompleta;
                            aplicacionCarenciaDB.ServicioSalud = aplicacionCarencia.ServicioSalud;
                            aplicacionCarenciaDB.SeguridadSocial = aplicacionCarencia.SeguridadSocial;
                            aplicacionCarenciaDB.Vivienda = aplicacionCarencia.Vivienda;
                            aplicacionCarenciaDB.Piso = aplicacionCarencia.Piso;
                            aplicacionCarenciaDB.Techo = aplicacionCarencia.Techo;
                            aplicacionCarenciaDB.Muro = aplicacionCarencia.Muro;
                            aplicacionCarenciaDB.Hacinamiento = aplicacionCarencia.Hacinamiento;
                            aplicacionCarenciaDB.Servicios = aplicacionCarencia.Servicios;
                            aplicacionCarenciaDB.Agua = aplicacionCarencia.Agua;
                            aplicacionCarenciaDB.Drenaje = aplicacionCarencia.Drenaje;
                            aplicacionCarenciaDB.Electricidad = aplicacionCarencia.Electricidad;
                            aplicacionCarenciaDB.Combustible = aplicacionCarencia.Combustible;
                            aplicacionCarenciaDB.Alimentaria = aplicacionCarencia.Alimentaria;
                            aplicacionCarenciaDB.GradoAlimentaria = aplicacionCarencia.GradoAlimentaria;
                            aplicacionCarenciaDB.Pea = aplicacionCarencia.Pea;
                            aplicacionCarenciaDB.ParentescoId = aplicacionCarencia.ParentescoId;
                            aplicacionCarenciaDB.SexoId = aplicacionCarencia.SexoId;
                            aplicacionCarenciaDB.Edad = aplicacionCarencia.Edad;
                            aplicacionCarenciaDB.Ingreso = aplicacionCarencia.Ingreso;
                            aplicacionCarenciaDB.LineaBienestar = aplicacionCarencia.LineaBienestar;
                            aplicacionCarenciaDB.VersionAlgoritmo = aplicacionCarencia.VersionAlgoritmo;
                            aplicacionCarenciaDB.NumIntegrante = aplicacionCarencia.NumIntegrante;
                            aplicacionCarenciaDB.NivelPobreza = aplicacionCarencia.NivelPobreza;
                            aplicacionCarenciaDB.CreatedAt = DateTime.Parse(aplicacionCarencia.CreatedAt);
                            aplicacionCarenciaDB.UpdatedAt = DateTime.Parse(aplicacionCarencia.UpdatedAt);
                            aplicacionCarenciaDB.DeletedAt = aplicacionCarencia.DeletedAt != null
                                ? DateTime.Parse(aplicacionCarencia.DeletedAt)
                                : (DateTime?) null;
                            aplicacionCarenciaDB.Discapacidad = aplicacionCarencia.Discapacidad;
                            aplicacionCarenciaDB.GradoEducativa = aplicacionCarencia.GradoEducativa;
                            if (nuevaAplicacion)
                            {
                                _context.AplicacionCarencia.Add(aplicacionCarenciaDB);
                            }
                            else
                            {
                                _context.AplicacionCarencia.Update(aplicacionCarenciaDB);
                            }
                        }

                        _context.SaveChanges();
                        if (aplicacion.Estatus == EstatusAplicacion.COMPLETO)
                        {
                            RegistrarDiscapacidades(aplicacion.BeneficiarioId, aplicacion.AplicacionPreguntas.Where(
                                ap => ap.PreguntaId.Equals("42")
                                      && ap.Grado != null && !ap.Grado.Equals("1")).ToList());
                            _context.SaveChanges();
                        }
                    }

                    var ultimaAplicacion =
                        _context.Aplicacion.FirstOrDefault(a => a.BeneficiarioId.Equals(beneficiario.Id) && a.Activa);
                    if (
                        ultimaAplicacion !=
                        null) // Se quitan de la base de datos central las respuestas que fueron eliminadas logicamente de los dispositivos
                    {
                        var eliminadas = _context.AplicacionPregunta.Where(ap =>
                            ap.AplicacionId.Equals(ultimaAplicacion.Id) && ap.DeletedAt != null);
                        _context.AplicacionPregunta.RemoveRange(eliminadas);
                        _context.SaveChanges();
                    }

                    if (incidencias.Any())
                    {
                        var incidenciasDB = _context.Incidencias.Where(i => i.BeneficiarioId.Equals(beneficiario.Id))
                            .ToDictionary(i => i.Id);
                        foreach (var incidencia in incidencias)
                        {
                            if (!incidenciasDB.ContainsKey(incidencia.Id))
                            {
                                var incidenciaDB = new Incidencia
                                {
                                    Id = incidencia.Id,
                                    Observaciones = incidencia.Observaciones,
                                    CreatedAt = DateTime.Parse(incidencia.CreatedAt),
                                    UpdatedAt = DateTime.Parse(incidencia.UpdatedAt),
                                    DeletedAt = incidencia.DeletedAt == null
                                        ? (DateTime?) null
                                        : DateTime.Parse(incidencia.DeletedAt),
                                    TrabajadorId = incidencia.TrabajadorId,
                                    TipoIncidenciaId = incidencia.TipoIncidenciaId,
                                    BeneficiarioId = incidencia.BeneficiarioId,
                                    DeviceId = incidencia.DeviceId,
                                };
                                _context.Incidencias.Add(incidenciaDB);
                            }
                        }

                        _context.SaveChanges();
                    }

                    transaction.Commit();
                }

                var conexion = _configuration.GetConnectionString("DefaultConnection");
                var url = _configuration["SITE_URL"];
                var mailgun = _configuration["MAILGUN_URI"];
                var mailgunKey = _configuration["MAILGUN_KEY"];
                var from = _configuration["MAILGUN_FROM"];
                var mailgunRequest = _configuration["MAILGUN_REQUEST"];

                new Thread(() =>
                {
                    //Se manda correo en otro hilo en los diagnosticos completos
                    SendEmailAsync(aplicacionesTerminadas, conexion, url, mailgun, mailgunKey, from, mailgunRequest);
                }).Start();

                if (domicilio.Id != null && domicilio.LatitudAtm.Any() && domicilio.LongitudAtm.Any())
                {
                    url = _configuration["WS_DIRECCION"];
                    new Thread(async () =>
                    {
                        //Se actualizan las variables calculables del domicilio con la API
                        await CompletarDireccionAsync(domicilio.Id, conexion, url, _clientFactory.CreateClient());
                    }).Start();
                }

                foreach (var terminada in aplicacionesTerminadas)
                {
                    url = _configuration["WS_DATOS"];
                    var password = _configuration["CA_WS_CURP"];
                    var idApiCurp = _configuration["ID_WS_CURP"];
                    var aplicacionDB = _context.Aplicacion.FirstOrDefault(a => a.Id == terminada);
                    var jefeFamilia = _context.Beneficiario.FirstOrDefault(b => b.Id == aplicacionDB.BeneficiarioId).Id;
                    var integrantes = _context.Beneficiario
                        .Where(b => b.DeletedAt == null && b.HijoId == null && b.PadreId == jefeFamilia)
                        .Select(i => i.Id).ToList();
                    new Thread(async () =>
                    {
                        await CompletarCurp(jefeFamilia, conexion, url, password, _clientFactory.CreateClient(), idApiCurp);
                        foreach (var integrante in integrantes)
                        {
                            await CompletarCurp(integrante, conexion, url, password, _clientFactory.CreateClient(), idApiCurp);
                        }
                    }).Start();
                }

                response.Curp = beneficiario.Curp;
            }
            catch (Exception e)
            {
                Excepcion.Registrar(e);
                response.Curp = "";
                response.Result =
                    "Error en el proceso de ingresar la información recibida en el portal administrativo, puede compartir este mensaje con el equipo técnico : " +
                    e.Message + " ";
                return JsonSedeshu.SerializeObject(response);
            }

            response.Result = "ok";
            response.BeneficiarioId = null;
            response.Aplicaciones = new List<Aplicacion>();
            response.AplicacionesPreguntas = new List<AplicacionPregunta>();
            response.AplicacionesCarencias = new List<AplicacionCarencia>();
            return JsonSedeshu.SerializeObject(response);
        }

        public void RegistrarDiscapacidades(string beneficiarioId, List<AplicacionPreguntaApiModel> respuestas)
        {
            var now = DateTime.Now;
            var integrantes = _context.Beneficiario
                .Where(b => b.DeletedAt == null &&
                            (b.Id.Equals(beneficiarioId) || b.PadreId.Equals(beneficiarioId)))
                .OrderBy(b => b.NumIntegrante).ToList();
            var integrantesIds = integrantes.Select(i => i.Id);
            var discapacidades = _context.BeneficiarioDiscapacidades
                .Where(b => b.DeletedAt == null && integrantesIds.Contains(b.BeneficiarioId))
                .ToDictionary(b => b.BeneficiarioId + "-" + b.DiscapacidadId);
            var respuestasDiscapacidad = _context.Respuesta.Where(r => r.PreguntaId.Equals("42")).Select(r => r.Id);
            var gradosDB = _context.RespuestaGrado.Where(rg => respuestasDiscapacidad.Contains(rg.RespuestaId))
                .ToDictionary(rg => rg.RespuestaId + "-" + rg.Grado);
            foreach (var respuesta in respuestas)
            {
                var integrante = integrantes.FirstOrDefault(i => i.NumIntegrante == respuesta.RespuestaIteracion + 1);
                if (integrante == null)
                {
                    continue;
                }

                var nuevoRegistro = true;
                var beneficiarioDiscapacidad = new BeneficiarioDiscapacidad();
                if (discapacidades.ContainsKey(integrante.Id + "-" + respuesta.RespuestaId))
                {
                    beneficiarioDiscapacidad = discapacidades[integrante.Id + "-" + respuesta.RespuestaId];
                    nuevoRegistro = false;
                }

                beneficiarioDiscapacidad.BeneficiarioId = integrante.Id;
                beneficiarioDiscapacidad.DiscapacidadId = respuesta.RespuestaId;
                beneficiarioDiscapacidad.GradoId = gradosDB[respuesta.RespuestaId + "-" + respuesta.Grado].Id;
                beneficiarioDiscapacidad.CausaId = respuesta.Complemento;
                beneficiarioDiscapacidad.DeletedAt = null;
                beneficiarioDiscapacidad.UpdatedAt = now;
                if (nuevoRegistro)
                {
                    beneficiarioDiscapacidad.CreatedAt = now;
                    _context.BeneficiarioDiscapacidades.Add(beneficiarioDiscapacidad);
                }
                else
                {
                    _context.BeneficiarioDiscapacidades.Update(beneficiarioDiscapacidad);
                }
            }
        }

        /// <summary>
        /// Funcion que sincroniza los archivos del registro de un beneficiario, comprobante de domicilio, foto del hogar, copia del aviso de privacidad, firma, etc
        /// </summary>
        /// <param name="request">Informacion de la imagen</param>
        /// <returns>Estatus de la sincronizacion</returns>
        [HttpPost]
        [DisableRequestSizeLimit]
        public string Archivos([FromForm] ArchivoRequest request)
        {
            var ArchivosHash = _context.Archivo.ToDictionary(x => x.Id);
            var Archivo = JsonConvert.DeserializeObject<ArchivoApiModel>(request.Archivo);

            try
            {
                Archivo arc;
                if (ArchivosHash.TryGetValue(Archivo.Id, out arc))
                {
                    arc.Id = Archivo.Id;
                    arc.Nombre = Archivo.Nombre;
                    arc.CreatedAt = DateTime.Parse(Archivo.CreatedAt);
                    arc.UpdatedAt = DateTime.Parse(Archivo.UpdatedAt);
                    arc.DeletedAt = Archivo.DeletedAt != null ? DateTime.Parse(Archivo.DeletedAt) : (DateTime?) null;

                    _context.Archivo.Update(arc);
                }
                else
                {
                    arc = new Archivo
                    {
                        Id = Archivo.Id,
                        Nombre = Archivo.Nombre,
                        CreatedAt = DateTime.Parse(Archivo.CreatedAt),
                        UpdatedAt = DateTime.Parse(Archivo.UpdatedAt),
                        DeletedAt = Archivo.DeletedAt != null ? DateTime.Parse(Archivo.DeletedAt) : (DateTime?) null,
                    };

                    _context.Archivo.Add(arc);
                }

                Image image = GetImage(Convert.FromBase64String(Archivo.Base64));
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), "archivos");
                image.Save(Path.Combine(pathToSave, Archivo.Id + ".jpg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                Excepcion.Registrar(e);
                return "Error: " + e.Message;
            }

            return "ok";
        }

        private Image GetImage(byte[] bytes)
        {
            var stream = new MemoryStream(bytes);
            return Image.FromStream(stream);
        }


        /// <summary>
        /// Funcion que verifica que las credenciales proporcionadas por un promotor para acceder a la aplicacion movil
        /// </summary>
        /// <param name="model">Usuario y contraseña del promotor</param>
        /// <returns>Resultado de la validacion</returns>
        [HttpPost]
        public string LoginMovil([FromForm] LoginMovilModel model)
        {
            var response = new LoginMovilResponse();
            var user = _context.Trabajador.First(x => x.DeletedAt == null && x.Username.Equals(model.Username)
                                                                          && x.Tipo == TipoTrabajador.ENCUESTADOR);
            if (user != null && Utils.DecryptString(Llave, user.Password, true).Equals(model.Password))
            {
                response.Trabajador = user;
                response.Result = "ok";

                return JsonConvert.SerializeObject(response);
            }

            response.Result = "error";

            return JsonConvert.SerializeObject(response);
        }

        /// <summary>
        /// Funcion que obtiene las imagenes del registro de un beneficiario
        /// </summary>
        /// <param name="model">Datos del beneficiario</param>
        /// <returns>Imagen obtenida de la base de datos</returns>
        [HttpPost]
        public string ObtenerImagenes([FromForm] DescargaImagenApiModel model)
        {
            var api = new ImagesModel();
            var ArchivosHash = _context.Archivo.ToDictionary(x => x.Id);
            var Archivos = JsonConvert.DeserializeObject<List<ArchivoApiModel>>(model.Archivos);
            api.Archivos = new List<ArchivoApiModel>();
            try
            {
                if (Archivos.Any())
                {
                    foreach (var Archivo in Archivos)
                    {
                        Archivo arc;
                        if (ArchivosHash.TryGetValue(Archivo.Id, out arc))
                        {
                            arc.Id = Archivo.Id;
                            arc.Nombre = Archivo.Nombre;
                            arc.CreatedAt = DateTime.Parse(Archivo.CreatedAt);
                            arc.UpdatedAt = DateTime.Parse(Archivo.UpdatedAt);
                            arc.DeletedAt = Archivo.DeletedAt != null
                                ? DateTime.Parse(Archivo.DeletedAt)
                                : (DateTime?) null;

                            _context.Archivo.Update(arc);
                        }
                        else
                        {
                            arc = new Archivo()
                            {
                                Id = Archivo.Id,
                                Nombre = Archivo.Nombre,
                                CreatedAt = DateTime.Parse(Archivo.CreatedAt),
                                UpdatedAt = DateTime.Parse(Archivo.UpdatedAt),
                                DeletedAt =
                                    Archivo.DeletedAt != null ? DateTime.Parse(Archivo.DeletedAt) : (DateTime?) null,
                            };

                            _context.Archivo.Add(arc);
                        }

                        var fromBase64String = Convert.FromBase64String(Archivo.Base64);
                        Image image;
                        using (var ms = new MemoryStream(fromBase64String))
                        {
                            image = Image.FromStream(ms);
                        }

                        var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), "archivos");
                        image.Save(Path.Combine(pathToSave, Archivo.Id), System.Drawing.Imaging.ImageFormat.Png);
                    }

                    _context.SaveChanges();
                }

                var files = _context.Archivo.Where(x => x.Nombre.Contains(model.Id) && x.DeletedAt == null).ToList();
                foreach (var file in files)
                {
                    var archivo = new ArchivoApiModel()
                    {
                        Id = file.Id,
                        Nombre = file.Nombre,
                        Enviado = true,
                        CreatedAt = file.CreatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                        UpdatedAt = file.UpdatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                        DeletedAt = file.DeletedAt?.ToString("yyyy-MM-ddTHH:mm:sszzz")
                    };
                    var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), "archivos");
                    using (var image = Image.FromFile(Path.Combine(pathToSave, archivo.Id + ".jpg")))
                    {
                        using (MemoryStream m = new MemoryStream())
                        {
                            image.Save(m, image.RawFormat);
                            var imageBytes = m.ToArray();
                            archivo.Base64 = Convert.ToBase64String(imageBytes);
                        }
                    }

                    api.Archivos.Add(archivo);
                }
            }
            catch (Exception e)
            {
                Excepcion.Registrar(e);
                return JsonConvert.SerializeObject(api);
            }

            return JsonConvert.SerializeObject(api);
        }

        /// <summary>
        /// Funcion que compara si la contraseña introducida por el promotor es la misma que el hash que se encuentra almacenado en la base de datos
        /// </summary>
        /// <param name="password">Contraseña ingresada</param>
        /// <param name="hashedPassword">Contraseña encriptada de la base de datos</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Excepcion devuelta si la contraseña a comparar es vacia</exception>
        public static bool VerifyHashedPassword(string password, string hashedPassword)
        {
            byte[] buffer4;
            if (hashedPassword == null)
            {
                return false;
            }

            if (password == null)
            {
                throw new ArgumentNullException("password");
            }

            var src = Convert.FromBase64String(hashedPassword);
            if ((src.Length != 0x31) || (src[0] != 0))
            {
                return false;
            }

            var dst = new byte[0x10];
            Buffer.BlockCopy(src, 1, dst, 0, 0x10);
            var buffer3 = new byte[0x20];
            Buffer.BlockCopy(src, 0x11, buffer3, 0, 0x20);
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, dst, 0x3e8))
            {
                buffer4 = bytes.GetBytes(0x20);
            }

            return buffer3.SequenceEqual(buffer4);
        }

        /// <summary>
        /// Funcion que registra las carencias calculadas por el JAR coneval.jar en la aplicacion que lo mando llamar
        /// </summary>
        /// <param name="respuestaCarencia">Objeto que contiene el id de la aplicacion y el json de las carencias</param>
        /// <returns>Estatus de la sincronizacion</returns>
        [HttpPost]
        public string ActualizarAplicacion([FromForm] AplicacionRespuestaCarencia respuestaCarencia)
        {
            var aplicacion = _context.Aplicacion.Find(respuestaCarencia.Id);
            if (aplicacion != null)
            {
                aplicacion.Carencias = respuestaCarencia.Carencias;
                _context.Aplicacion.Update(aplicacion);
                _context.SaveChanges();
            }

            return "ok";
        }

        private void RegistrarCurp(AplicacionApiModel aplicacion, BeneficiarioApiModel representante,
            List<BeneficiarioApiModel> Integrantes)
        {
            var now = new DateTime();
            var respuestas = _context.AplicacionPregunta
                .Where(ap => ap.AplicacionId.Equals(aplicacion.Id) && ap.PreguntaId.Equals("-1"))
                .ToDictionary(ap => ap.RespuestaIteracion);
            var respuesta = new AplicacionPregunta();
            var nuevaRespuesta = false;
            if (respuestas.ContainsKey(0))
            {
                respuesta = respuestas[0];
            }
            else
            {
                nuevaRespuesta = true;
                respuesta.Id = Guid.NewGuid().ToString();
                respuesta.AplicacionId = aplicacion.Id;
                respuesta.PreguntaId = "-1";
                respuesta.RespuestaIteracion = 0;
                respuesta.CreatedAt = now;
            }

            respuesta.UpdatedAt = now;
            respuesta.Valor = representante.Curp;
            if (nuevaRespuesta)
            {
                _context.AplicacionPregunta.Add(respuesta);
            }
            else
            {
                _context.AplicacionPregunta.Update(respuesta);
            }

            foreach (var integrante in Integrantes)
            {
                respuesta = new AplicacionPregunta();
                nuevaRespuesta = false;
                if (respuestas.ContainsKey(integrante.NumIntegrante - 1))
                {
                    respuesta = respuestas[integrante.NumIntegrante - 1];
                }
                else
                {
                    nuevaRespuesta = true;
                    respuesta.Id = Guid.NewGuid().ToString();
                    respuesta.AplicacionId = aplicacion.Id;
                    respuesta.PreguntaId = "-1";
                    respuesta.RespuestaIteracion = integrante.NumIntegrante - 1;
                    respuesta.CreatedAt = now;
                }

                respuesta.UpdatedAt = now;
                respuesta.Valor = integrante.Curp;
                if (nuevaRespuesta)
                {
                    _context.AplicacionPregunta.Add(respuesta);
                }
                else
                {
                    _context.AplicacionPregunta.Update(respuesta);
                }
            }

            _context.SaveChanges();
        }

        private string GetCurp(string nombre, string primerApellido, string segundoApellido, string estadoId,
            string sexoId, string fecha_nacimiento)
        {
            var curp = "";
            var fecha = fecha_nacimiento.Replace("-", "").Substring(0, 8);
            var estado = _context.Estado.Find(estadoId).Abreviacion;
            var sexo = _context.Sexo.Find(sexoId).Nombre.Substring(0, 1).ToUpper();
            var cadena = primerApellido + "/" + segundoApellido + "/" + nombre + "/" + sexo + "/" + fecha + "/" +
                         estado;
            var request = (HttpWebRequest) WebRequest.Create(_configuration["WS_DATOS"] + "/" +
                                                             _configuration["CA_WS_CURP"] +
                                                             "/" + cadena);
            var response = request.GetResponseAsync();
            using (var stream = response.GetAwaiter().GetResult())
            {
                try
                {
                    using (var reader = new StreamReader(stream.GetResponseStream()))
                    {
                        var curpResponse = JsonConvert.DeserializeObject<CurpResponse>(reader.ReadToEnd());
                        if (curpResponse.Mensaje.Equals("OK"))
                        {
                            curp = curpResponse.Resultado.CurpCollection.First().CURP;
                        }
                    }
                }
                catch (Exception e)
                {
                    Excepcion.Registrar(e);
                }
            }

            return curp;
        }

        public static async Task CompletarCurp(string id, string conexion, string url, string password,
            HttpClient client, string idApiCurp)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer(conexion);
            using (var db = new ApplicationDbContext(optionsBuilder.Options))
            {
                try
                {
                    db.Database.BeginTransaction();
                    var beneficiario = db.Beneficiario.Where(b => b.Id.Equals(id)).Include(b => b.Sexo)
                        .Include(b => b.Estado).FirstOrDefault();
                    await ImportacionDiagnosticos.CompletarCurp(beneficiario, db, url, password, DateTime.Now, client, idApiCurp);
                    DiagnosticoCode.ActualizarCurpEnSabana(db, beneficiario);
                    db.Database.CommitTransaction();
                }
                catch (Exception e)
                {
                    Excepcion.Registrar(e);
                    db.Database.RollbackTransaction();
                }
            }
        }

        public static async Task CompletarDireccionAsync(string id, string conexion, string url, HttpClient client)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer(conexion);
            using (var db = new ApplicationDbContext(optionsBuilder.Options))
            {
                try
                {
                    db.Database.BeginTransaction();
                    var domicilio = db.Domicilio.Find(id);
                    await ImportacionDiagnosticos.CompletarDomicilio(domicilio, url, client);
                    db.Domicilio.Update(domicilio);
                    db.SaveChanges();
                    db.Database.CommitTransaction();
                }
                catch (Exception e)
                {
                    Excepcion.Registrar(e);
                    db.Database.RollbackTransaction();
                }
            }
        }

        public string GetToken([FromForm] LoginMovilModel model)
        {
            var response = new TokenResponse();
            var user = _context.Users.First(u => u.UserName.Equals(model.Username));
            if (user == null)
            {
                response.Result = "error";
            }
            else
            {
                var result = _userManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, model.Password);
                if (result == PasswordVerificationResult.Success)
                {
                    var token = Utils.RandomPassword();
                    if (user.FechaToken == null)
                    {
                        user.Token = token;
                        user.FechaToken = DateTime.Now;
                        _context.Users.Update(user);
                        _context.SaveChanges();
                    }
                    else
                    {
                        if (user.FechaToken.Value.AddHours(24) < DateTime.Now)
                        {
                            user.Token = token;
                            user.FechaToken = DateTime.Now;
                            _context.Users.Update(user);
                            _context.SaveChanges();
                        }
                    }

                    response.Token = user.Token;
                    response.Result = "ok";
                }
                else
                {
                    response.Result = "error";
                }
            }

            return JsonConvert.SerializeObject(response);
        }

        public string GetDiagnosticos([FromForm] Diagnostico diagnostico)
        {
            var builder = new StringBuilder();
            try
            {
                var user = _context.Users.First(u => u.Token.Equals(diagnostico.Token));
                if (user == null)
                {
                    throw new Exception();
                }

                if (user.FechaToken == null || user.FechaToken.Value.AddHours(24) < DateTime.Now)
                {
                    throw new Exception();
                }

                var req = new DiagnosticoRequest();
                if (!string.IsNullOrEmpty(diagnostico.Municipio))
                {
                    req.Municipio = diagnostico.Municipio;
                }

                if (!string.IsNullOrEmpty(diagnostico.Localidad))
                {
                    req.Localidad = diagnostico.Localidad;
                }

                if (!string.IsNullOrEmpty(diagnostico.Ageb))
                {
                    req.Ageb = diagnostico.Ageb;
                }

                if (!string.IsNullOrEmpty(diagnostico.Manzana))
                {
                    req.Manzana = diagnostico.Manzana;
                }

                if (!string.IsNullOrEmpty(diagnostico.CodigoPostal))
                {
                    req.CodigoPostal = diagnostico.CodigoPostal;
                }

                if (!string.IsNullOrEmpty(diagnostico.Colonia))
                {
                    req.CodigoPostal = diagnostico.CodigoPostal;
                }

                if (!string.IsNullOrEmpty(diagnostico.Familia))
                {
                    req.BeneficiarioId = diagnostico.Familia;
                }

                var util = new DiagnosticoCode(_context);
                var query = util.BuildBaseMadreQuery(diagnostico, diagnostico.Inicio, diagnostico.Fin);
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = query.Key;
                    var iParametro = 0;
                    foreach (var parametro in query.Value)
                    {
                        command.Parameters.Add(new SqlParameter("@" + iParametro++, parametro));
                    }

                    _context.Database.OpenConnection();
                    using (var reader = command.ExecuteReader())
                    {
                        var columnas = new List<string>();
                        var tableSchema = reader.GetSchemaTable();
                        foreach (DataRow row in tableSchema.Rows)
                        {
                            columnas.Add(row["ColumnName"].ToString());
                        }

                        builder.AppendLine(string.Join('|', columnas));
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                columnas.Clear();
                                for (var i = 0; i < reader.FieldCount; i++)
                                {
                                    if (reader.IsDBNull(i))
                                    {
                                        columnas.Add("");
                                    }
                                    else
                                    {
                                        int numValor;
                                        if (int.TryParse(reader.GetString(i), out numValor))
                                        {
                                            columnas.Add(numValor.ToString());
                                        }
                                        else
                                        {
                                            columnas.Add(i == 0
                                                ? reader.GetString(i)
                                                : "'" + reader.GetString(i) + "'");
                                        }
                                    }
                                }

                                builder.AppendLine(string.Join('|', columnas));
                            }
                        }
                    }

                    _context.Database.CloseConnection();
                }

                _context.Bitacora.Add(new Bitacora()
                {
                    UsuarioId = user.Id,
                    Accion = AccionBitacora.EXPORTACION,
                    Mensaje = "Se exportó sabana.",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                });
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                Excepcion.Registrar(e);
            }

            return builder.ToString();
        }

        public void SendEmailAsync(List<string> aplicaciones, string conexion, string url, string mailgun, string key,
            string cuenta, string request)
        {
            if (aplicaciones.Any())
            {
                var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
                optionsBuilder.UseSqlServer(conexion);
                using (var db = new ApplicationDbContext(optionsBuilder.Options))
                {
                    try
                    {
                        var administrador = db.Users.Where(u => u.PerfilId.Equals("1")).First();
                        if (administrador != null)
                        {
                            var aplicacionesDB = db.Aplicacion.Where(a => aplicaciones.Contains(a.Id))
                                .Include(a => a.Beneficiario)
                                .ThenInclude(b => b.Domicilio)
                                .ThenInclude(d => d.Localidad)
                                .ThenInclude(l => l.Municipio)
                                .Include(a => a.Trabajador).ToList();
                            var message =
                                "<html><h5>Estos son los nuevos diagnósticos que se han capturado en el sistema :</h5><table style='padding:10px; border:1px solid gray;'><tr><th>Articulador</th><th>Fecha de inicio</th><th>Fecha de término</th><th>Domicilio</th><th>Municipio</th><th>Localidad</th></tr>";
                            foreach (var aplicacion in aplicacionesDB)
                            {
                                var domicilio = aplicacion.Beneficiario.Domicilio;
                                message += "<tr>";
                                message +=
                                    "<td>" + (aplicacion.Trabajador == null ? "" : aplicacion.Trabajador.Nombre) +
                                    "</td>";
                                message += "<td>" + (aplicacion.FechaInicio?.ToString("dd/MM/yyyy") ?? "") + "</td>";
                                message += "<td>" + (aplicacion.FechaFin?.ToString("dd/MM/yyyy") ?? "") + "</td>";
                                message += "<td>" + (aplicacion.Beneficiario == null
                                    ? ""
                                    : (aplicacion.Beneficiario.Nombre + " " +
                                       aplicacion.Beneficiario.ApellidoPaterno)) + "</td>";
                                message += "<td>" + (domicilio == null ? "" : (domicilio.Municipio.Nombre)) + "</td>";
                                message += "<td>" + (domicilio == null ? "" : (domicilio.Localidad.Nombre)) + "</td>";
                                message += "</tr>";
                            }

                            message += "</table></html>";
                            message += "<a href='" + url +
                                       "' style='margin-top: 20px; padding: 10px; border: 1px solid gray !important; display: block; text-decoration:none; width: 200px; text-align: center; background-color:#0159bc; color:white'>Entrar </a>";
                            using (var client = new HttpClient {BaseAddress = new Uri(mailgun)})
                            {
                                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                                    Convert.ToBase64String(Encoding.ASCII.GetBytes(key)));

                                var content = new FormUrlEncodedContent(new[]
                                {
                                    new KeyValuePair<string, string>("from", cuenta),
                                    new KeyValuePair<string, string>("to", administrador.Email),
                                    new KeyValuePair<string, string>("subject",
                                        "Le notificamos que se han sincronizado nuevos diagnósticos"),
                                    new KeyValuePair<string, string>("html", message)
                                });
                                client.PostAsync(request, content);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Excepcion.Registrar(e);
                    }
                }
            }
        }

        private void LimpiarValor(AplicacionPregunta app, string valor, string complemento,
            Dictionary<string, Pregunta> preguntas)
        {
            if (!preguntas.ContainsKey(app.PreguntaId)) return;
            var preg = preguntas[app.PreguntaId];
            if (preg.TipoPregunta == TipoPregunta.Abierta.ToString() ||
                preg.TipoComplemento == TipoComplemento.Abierta.ToString())
            {
                if (preg.TipoPregunta == TipoPregunta.Abierta.ToString())
                {
                    app.Valor = Regex.Replace(valor.Normalize(NormalizationForm.FormD), @"[^a-zA-Z0-9_ ]+", "");
                }

                if (preg.TipoComplemento == TipoComplemento.Abierta.ToString())
                {
                    app.Complemento = Regex.Replace(complemento.Normalize(NormalizationForm.FormD), @"[^a-zA-Z0-9_ ]+",
                        "");
                }
            }
            else
            {
                app.Valor = valor;
            }
        }

        [HttpPost]
        public string SyncEstados([FromForm] RequestSync request)
        {
            var api = new ResponseGeograficos();
            var lastSync = Convert.ToDateTime(request.LastSyncDate);

            api.Estados = GetEstados(lastSync, true);
            api.Municipios = GetMunicipios(lastSync, true);
            api.Localidades = GetLocalidades(lastSync, true);
            api.Dependencias = GetDependencias(lastSync, true);
            api.Zonas = GetZonas(lastSync, true);
            api.MunicipiosZonas = GetMunicipiosZonas(lastSync, true);
            return JsonConvert.SerializeObject(api);
        }

        [HttpPost]
        public string SyncManzanas([FromForm] RequestSync request)
        {
            var api = new ResponseManzanas();
            var municipioId = JsonConvert.DeserializeObject<List<string>>(request.MunicipiosId).FirstOrDefault();
            var lastSync = Convert.ToDateTime(request.LastSyncDate);

            api.Agebs = GetAgebs(lastSync, municipioId);
            api.Manzanas = GetManzanas(lastSync, municipioId);
            api.Colonias = GetColonias(lastSync, municipioId);
            api.Calles = GetCalles(lastSync, municipioId);
            return JsonConvert.SerializeObject(api);
        }

        private List<EstadoApiModel> GetEstados(DateTime lastSync, bool first)
        {
            var estados = first
                ? _context.Estado.Where(x => DateTime.Compare(x.UpdatedAt, lastSync) >= 0 && x.DeletedAt == null)
                    .ToList()
                : _context.Estado.Where(x => DateTime.Compare(x.UpdatedAt, lastSync) >= 0).ToList();
            return estados.Select(estado => new EstadoApiModel
            {
                Id = estado.Id,
                Clave = estado.Clave,
                Abreviacion = estado.Abreviacion,
                Nombre = estado.Nombre,
                CreatedAt = estado.CreatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                UpdatedAt = estado.UpdatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                DeletedAt = estado.DeletedAt?.ToString("yyyy-MM-ddTHH:mm:sszzz")
            }).ToList();
        }

        private List<MunicipioApiModel> GetMunicipios(DateTime lastSync, bool first)
        {
            var municipios = first
                ? _context.Municipio.Where(x => DateTime.Compare(x.UpdatedAt, lastSync) >= 0 && x.DeletedAt == null)
                    .ToList()
                : _context.Municipio.Where(x => DateTime.Compare(x.UpdatedAt, lastSync) >= 0).ToList();
            return municipios.Select(municipio => new MunicipioApiModel
            {
                Id = municipio.Id,
                Nombre = municipio.Nombre,
                Indice = municipio.Indice,
                CreatedAt = municipio.CreatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                UpdatedAt = municipio.UpdatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                DeletedAt = municipio.DeletedAt?.ToString("yyyy-MM-ddTHH:mm:sszzz")
            }).ToList();
        }

        private List<LocalidadApiModel> GetLocalidades(DateTime lastSync, bool first)
        {
            var localidades = first
                ? _context.Localidad.Where(x => DateTime.Compare(x.UpdatedAt, lastSync) >= 0 && x.DeletedAt == null)
                    .ToList()
                : _context.Localidad.Where(x => DateTime.Compare(x.UpdatedAt, lastSync) >= 0).ToList();
            return localidades.Select(localidad => new LocalidadApiModel
            {
                Id = localidad.Id,
                Nombre = localidad.Nombre,
                MunicipioId = localidad.MunicipioId,
                CreatedAt = localidad.CreatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                UpdatedAt = localidad.UpdatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                DeletedAt = localidad.DeletedAt?.ToString("yyyy-MM-ddTHH:mm:sszzz")
            }).ToList();
        }

        private List<MunicipioZonaApiModel> GetMunicipiosZonas(DateTime lastSync, bool first)
        {
            var municipioZonas = first
                ? _context.MunicipioZona.Where(x => DateTime.Compare(x.UpdatedAt, lastSync) >= 0 && x.DeletedAt == null)
                    .ToList()
                : _context.MunicipioZona.Where(x => DateTime.Compare(x.UpdatedAt, lastSync) >= 0).ToList();
            return municipioZonas.Select(MunicipioZona => new MunicipioZonaApiModel
            {
                Id = MunicipioZona.Id,
                MunicipioId = MunicipioZona.MunicipioId,
                ZonaId = MunicipioZona.ZonaId,
                CreatedAt = MunicipioZona.CreatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                UpdatedAt = MunicipioZona.UpdatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                DeletedAt = MunicipioZona.DeletedAt?.ToString("yyyy-MM-ddTHH:mm:sszzz")
            }).ToList();
        }

        private List<AgebApiModel> GetAgebs(DateTime lastSync, string municipioId)
        {
            var agebs = municipioId != null
                ? _context.Ageb.Where(x =>
                    DateTime.Compare(x.UpdatedAt, lastSync) >= 0 && x.DeletedAt == null &&
                    x.MunicipioId.Equals(municipioId)).ToList()
                : _context.Ageb.Where(x => DateTime.Compare(x.UpdatedAt, lastSync) >= 0).ToList();
            return agebs.Select(ageb => new AgebApiModel
            {
                Id = ageb.Id,
                Clave = ageb.Clave,
                LocalidadId = ageb.LocalidadId,
                MunicipioId = ageb.MunicipioId,
                EstadoId = ageb.EstadoId,
                CreatedAt = ageb.CreatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                UpdatedAt = ageb.UpdatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                DeletedAt = ageb.DeletedAt?.ToString("yyyy-MM-ddTHH:mm:sszzz")
            }).ToList();
        }

        private List<ManzanaApiModel> GetManzanas(DateTime lastSync, string municipioId)
        {
            var manzanas = municipioId != null
                ? _context.Manzana.Where(x =>
                    DateTime.Compare(x.UpdatedAt, lastSync) >= 0 && x.DeletedAt == null &&
                    x.MunicipioId.Equals(municipioId)).ToList()
                : _context.Manzana.Where(x => DateTime.Compare(x.UpdatedAt, lastSync) >= 0).ToList();
            return manzanas.Select(manzana => new ManzanaApiModel
            {
                Id = manzana.Id,
                Nombre = manzana.Nombre,
                LocalidadId = manzana.LocalidadId,
                MunicipioId = manzana.MunicipioId,
                AgebId = manzana.AgebId,
                CreatedAt = manzana.CreatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                UpdatedAt = manzana.UpdatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                DeletedAt = manzana.DeletedAt?.ToString("yyyy-MM-ddTHH:mm:sszzz")
            }).ToList();
        }

        private List<DependenciaApiModel> GetDependencias(DateTime lastSync, bool first)
        {
            var dependencias = first
                ? _context.Dependencia.Where(x => DateTime.Compare(x.UpdatedAt, lastSync) >= 0 && x.DeletedAt == null)
                    .ToList()
                : _context.Dependencia.Where(x => DateTime.Compare(x.UpdatedAt, lastSync) >= 0).ToList();
            return dependencias.Select(dependencia => new DependenciaApiModel
            {
                Id = dependencia.Id,
                Nombre = dependencia.Nombre,
                CreatedAt = dependencia.CreatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                UpdatedAt = dependencia.UpdatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                DeletedAt = dependencia.DeletedAt?.ToString("yyyy-MM-ddTHH:mm:sszzz")
            }).ToList();
        }

        private List<ZonaApiModel> GetZonas(DateTime lastSync, bool first)
        {
            var zonas = first
                ? _context.Zona.Where(x => DateTime.Compare(x.UpdatedAt, lastSync) >= 0 && x.DeletedAt == null).ToList()
                : _context.Zona.Where(x => DateTime.Compare(x.UpdatedAt, lastSync) >= 0).ToList();
            return zonas.Select(zona => new ZonaApiModel
            {
                Id = zona.Id,
                Clave = zona.Clave,
                DependenciaId = zona.DependenciaId,
                CreatedAt = zona.CreatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                UpdatedAt = zona.UpdatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                DeletedAt = zona.DeletedAt?.ToString("yyyy-MM-ddTHH:mm:sszzz")
            }).ToList();
        }

        public async Task<IActionResult> DownloadAlgoritmo()
        {
            var version = Configuracion.getVersionAlgoritmo(_context);
            var folderName = Path.Combine("wwwroot", "coneval");
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            var conevalPath = Path.Combine(pathToSave, "coneval" + version + ".apk");
            var memory = new MemoryStream();
            using (var stream = new FileStream(conevalPath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }

            memory.Position = 0;
            var ext = Path.GetExtension(conevalPath).ToLowerInvariant();
            return File(memory, GetMimeTypes()[ext], Path.GetFileName("coneval.apk"));
        }

        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jar", "application/java-archive"},
                {".apk", "application/java-archive"}
            };
        }

        [HttpPost]
        public VersionAlgoritmoResponse GetVersionAlgoritmo()
        {
            return new VersionAlgoritmoResponse
            {
                Version = Configuracion.getVersionAlgoritmo(_context)
            };
        }

        private List<ColoniaApiModel> GetColonias(DateTime lastSync, string municipioId)
        {
            var colonias = municipioId != null
                ? _context.Colonia.Where(x => DateTime.Compare(x.UpdatedAt, lastSync) >= 0 && x.DeletedAt == null &&
                                              x.MunicipioId.Equals(municipioId)).ToList()
                : _context.Colonia.Where(x => DateTime.Compare(x.UpdatedAt, lastSync) >= 0).ToList();
            return colonias.Select(colonia => new ColoniaApiModel
            {
                Id = colonia.Id,
                Nombre = colonia.Nombre,
                MunicipioId = colonia.MunicipioId,
                CodigoPostal = colonia.CodigoPostal,
                CreatedAt = colonia.CreatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                UpdatedAt = colonia.UpdatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                DeletedAt = colonia.DeletedAt?.ToString("yyyy-MM-ddTHH:mm:sszzz")
            }).ToList();
        }

        private List<CalleApiModel> GetCalles(DateTime lastSync, string municipioId)
        {
            var calles = municipioId != null
                ? _context.Calle.Where(x => DateTime.Compare(x.UpdatedAt, lastSync) >= 0 && x.DeletedAt == null &&
                                            x.MunicipioId.Equals(municipioId)).ToList()
                : _context.Calle.Where(x => DateTime.Compare(x.UpdatedAt, lastSync) >= 0).ToList();
            return calles.Select(calle => new CalleApiModel
            {
                Id = calle.Id,
                Nombre = calle.Nombre,
                MunicipioId = calle.MunicipioId,
                LocalidadId = calle.LocalidadId,
                CreatedAt = calle.CreatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                UpdatedAt = calle.UpdatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                DeletedAt = calle.DeletedAt?.ToString("yyyy-MM-ddTHH:mm:sszzz")
            }).ToList();
        }
    }
}