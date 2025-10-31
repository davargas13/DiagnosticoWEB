using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using DiagnosticoWeb.Code;
using DiagnosticoWeb.Code.Curp;
using DiagnosticoWeb.Database;
using DiagnosticoWeb.Models;
using DiagnosticoWeb.Validaciones;
using Microsoft.Extensions.Configuration;
using Estado = DiagnosticoWeb.Code.Curp.Estado;

namespace DiagnosticoWeb.Controllers
{
    /// <summary>
    /// Clase que permite modificar las respuestas de la aplicacion de una encuesta en un hogar visitado, esto solo lo pueden hacer los usuarios administradores
    /// </summary>
    public class AplicacionController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpClientFactory _clientFactory;

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="context">Conexion a la base de datos</param>
        public AplicacionController(ApplicationDbContext context, IConfiguration configuration,
            UserManager<ApplicationUser> userManager, IHttpClientFactory clientFactory)
        {
            _context = context;
            _configuration = configuration;
            _userManager = userManager;
            _clientFactory = clientFactory;
        }

        /// <summary>
        /// Funcion que muestra la pantalla para editar las respuestas de una encuesta aplicada en un hogar
        /// </summary>
        /// <param name="id">Identificador en base de datos de la encuesta aplicada</param>
        /// <returns>Vista para editar las respuestas</returns>
        [HttpGet]
        [Authorize]
        [RequireClaim("Permiso", Value = "beneficiarios.editar")]
        public IActionResult Editar(string id = "")
        {
            var aplicacion = _context.Aplicacion.FirstOrDefault(a => a.Id == id);
            var ultimaVersion = _context.EncuestaVersion.FirstOrDefault(ev => ev.DeletedAt == null && ev.Activa)?.Id;
            if (aplicacion.EncuestaVersionId != ultimaVersion)
            {
                // return NotFound();
            }
            var response = new AplicacionResponse
            {
                Id = id,
                BeneficiarioId = _context.Aplicacion.Find(id).BeneficiarioId,
                Respuestas = new Dictionary<int, Dictionary<int, AplicacionPreguntaResponse>>(),
                Preguntas = new Dictionary<int, PreguntaEncuestaModel>(),
                Carencias = new Dictionary<string, Carencia>(),
                Integrantes = new Dictionary<int, Integrante>(),
                Now = DateTime.Now.ToString("yyy-MM-dd")
            };
            var preguntas = new List<Pregunta>();
            var encuesta = _context.EncuestaVersion.First(ev => ev.Activa);
            var familia = _context.Beneficiario.FirstOrDefault(b => b.Id == aplicacion.BeneficiarioId);
            var preguntasAOmitir = new List<string>();
            if (familia.NumFamilia > 1)
            {
                string[] input = {"968", "132", "133", "134", "135", "136", "137", "984", "985", "986"};
                preguntasAOmitir = new List<string>(input);
                response.MismoDomicilio = true;
            }

            var preguntasQuery = _context.Pregunta.Where(p => p.DeletedAt == null &&p.Activa && p.EncuestaVersionId.Equals(encuesta.Id))
                .Include(p => p.Respuestas).ThenInclude(r => r.RespuestaGrados)
                .Include(p => p.PreguntaGrados).OrderBy(p => p.Numero)
                .Include(p => p.Carencia);
            if (preguntasAOmitir.Any())
            {
                preguntas = preguntasQuery.Where(p=>!preguntasAOmitir.Contains(p.Id)).ToList();
            }
            else
            {
                preguntas = preguntasQuery.ToList();
            }

            var respuestas = _context.AplicacionPregunta.Where(ap => ap.DeletedAt == null && ap.AplicacionId == aplicacion.Id)
                .Include(ap => ap.Pregunta).Include(ap => ap.Respuesta).ToList();
            var totalIntegrantes = 1;

            foreach (var pregunta in preguntas)
            {
                response.NumPreguntas = pregunta.Numero;
                var p = new PreguntaEncuestaModel
                {
                    Id = pregunta.Id,
                    Nombre = pregunta.Nombre,
                    Condicion = pregunta.Condicion,
                    Iterable = pregunta.Iterable,
                    CondicionIterable = pregunta.CondicionIterable,
                    CondicionLista = pregunta.CondicionLista,
                    Catalogo = pregunta.Catalogo,
                    Numero = pregunta.Numero,
                    Gradual = pregunta.Gradual,
                    CarenciaId = pregunta.CarenciaId,
                    TipoPregunta = pregunta.TipoPregunta,
                    Respuestas = new List<RespuestaEncuestaModel>(),
                    PreguntaGrados = new List<PreguntaGrado>(),
                    Mostrar = pregunta.Numero == (preguntasAOmitir.Any() ? 11 :1),
                    Expresion = pregunta.Expresion,
                    ExpresionEjemplo = pregunta.ExpresionEjemplo,
                    TipoComplemento = pregunta.TipoComplemento,
                    Complemento = pregunta.Complemento?.Trim(),
                    SeleccionarRespuestas = pregunta.SeleccionarRespuestas,
                    Maximo = pregunta.Maximo
                };
                if (pregunta.PreguntaGrados.Count > 0)
                {
                    foreach (var grado in pregunta.PreguntaGrados)
                    {
                        p.PreguntaGrados.Add(new PreguntaGrado()
                        {
                            Id = grado.Id,
                            Grado = grado.Grado
                        });
                    }
                }

                if (pregunta.Catalogo == null)
                {
                    var solicitudController = new SolicitudesController(_context, _userManager);
                    var admin = HttpContext.User.IsInRole("Administrador");
                    var user = _userManager.GetUserAsync(HttpContext.User).Result;
                    foreach (var respuesta in pregunta.Respuestas.Where(r=>r.DeletedAt==null).OrderBy(r => r.Numero))
                    {
                        var r = new RespuestaEncuestaModel
                        {
                            Id = respuesta.Id,
                            Nombre = respuesta.Nombre,
                            Negativa = respuesta.Negativa,
                            Numero = respuesta.Numero,
                            TipoComplemento = respuesta.TipoComplemento,
                            Complemento = respuesta.Complemento?.Trim(),
                            RespuestaGrados = new Dictionary<string, RespuestaGrado>(),
                            Catalogo = new List<Model>()
                        };
                        if (r.TipoComplemento != null &&
                            r.TipoComplemento.Equals(TipoComplemento.Catalogo.ToString()))
                        {
                            r.Catalogo = solicitudController.BuildCatalogosList(r.Complemento?.Trim(), "0", admin, user);
                        }

                        foreach (var grado in respuesta.RespuestaGrados)
                        {
                            r.RespuestaGrados.Add(grado.PreguntaGradoId, new RespuestaGrado()
                            {
                                Id = grado.Id,
                                Grado = grado.Grado
                            });
                        }

                        p.Respuestas.Add(r);
                    }
                }
                else
                {
                    foreach (var elemento in Utils.BuildCatalogo(_context, pregunta.Catalogo))
                    {
                        p.Respuestas.Add(new RespuestaEncuestaModel
                        {
                            Id = elemento.Id,
                            Nombre = elemento.Nombre
                        });
                            
                    }
                }
                response.Preguntas.Add(p.Numero, p);
            }


            foreach (var respuesta in respuestas)
            {
                if(respuesta.PreguntaId == "998")
                {
                    var stop = "estopa";
                }
                var preguntaDB = respuesta.Pregunta;
                if (!preguntaDB.Activa || (respuesta.RespuestaId != null && respuesta.Respuesta.DeletedAt != null))
                {
                    continue;
                }
                var numIteracion = preguntaDB.Iterable ? respuesta.RespuestaIteracion + 1 : 0;
                if (totalIntegrantes < numIteracion)
                {
                    totalIntegrantes = numIteracion;
                }

                if (!response.Respuestas.ContainsKey(preguntaDB.Numero))
                {
                    response.Respuestas.Add(preguntaDB.Numero,
                        new Dictionary<int, AplicacionPreguntaResponse>());
                }

                var respuestaResponse = response.Respuestas[preguntaDB.Numero];
                if (!respuestaResponse.ContainsKey(numIteracion))
                {
                    respuestaResponse[numIteracion] = new AplicacionPreguntaResponse
                    {
                        Id = respuesta.PreguntaId,
                        Valores = new Dictionary<int, KeyValuePair<string, string>>(),
                        Respuestas = new Dictionary<int, KeyValuePair<string, string>>(),
                        Grados = new Dictionary<int, string>(),
                        Multiples = new Dictionary<int, KeyValuePair<bool, string>>()
                    };
                }
                if (preguntaDB.TipoPregunta == TipoPreguntaString.Check)
                {
                    if (preguntaDB.Catalogo == null)
                    {
                        if (respuesta.Respuesta.DeletedAt == null)
                        {
                            respuestaResponse[numIteracion].Multiples.Add(respuesta.Respuesta.Numero - 1,
                                new KeyValuePair<bool, string>(true, respuesta.Complemento?.Trim()));
                        }
                    }
                    else
                    {
                        respuestaResponse[numIteracion].Multiples.Add(int.Parse(respuesta.ValorCatalogo)-1,
                            new KeyValuePair<bool, string>(true, respuesta.Complemento?.Trim()));
                    }
                }

                var valor = "";
                switch (preguntaDB.TipoPregunta)
                {
                    case TipoPreguntaString.Abierta:
                    case TipoPreguntaString.Numerica:
                    case TipoPreguntaString.Integrantes:
                        valor = respuesta.Valor;
                        break;
                    case TipoPreguntaString.Fecha:
                    case TipoPreguntaString.FechaFutura:
                    case TipoPreguntaString.FechaPasada:
                        if (respuesta.ValorFecha != null)
                            valor = respuesta.ValorFecha.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                        break;
                    default:
                        if (respuesta.Pregunta.Catalogo == null)
                        {
                            valor = respuesta.RespuestaId;
                        }
                        else
                        {
                            valor = respuesta.ValorCatalogo;
                        }

                        break;
                }

                if (respuesta.RespuestaId != null || respuesta.Pregunta.Catalogo != null)
                {
                    var numeroRespuesta = 0;
                    if (preguntaDB.Catalogo == null)
                    {
                        if (preguntaDB.TipoPregunta == TipoPreguntaString.Check || preguntaDB.Gradual )
                        {
                            numeroRespuesta = respuesta.Respuesta.Numero - 1;
                        }
                    }
                    else
                    {
                        numeroRespuesta = int.Parse(respuesta.ValorCatalogo) - 1;
                    }
                    respuestaResponse[numIteracion].Respuestas
                        .Add(numeroRespuesta, new KeyValuePair<string, string>(valor, respuesta.Complemento?.Trim()));
                    respuestaResponse[numIteracion].Valores
                        .Add(numeroRespuesta, new KeyValuePair<string, string>(valor, respuesta.Complemento?.Trim()));
                }
                else
                {
                    respuestaResponse[numIteracion].Valores.Add(0, new KeyValuePair<string, string>(valor, respuesta.Complemento?.Trim()));
                }

                if (respuesta.Grado != null)
                {
                    respuestaResponse[numIteracion].Grados.Add(respuesta.Respuesta.Numero - 1, respuesta.Grado);
                }
            }

            //for (var i = 1; i <= totalIntegrantes; i++)
            //{
            //    if (!response.Respuestas.ContainsKey(16)) continue;
            //    if (!response.Respuestas[16].ContainsKey(i)) continue;
            //    var integrante = new Integrante
            //    {
            //        Id = (i-1).ToString(),
            //        Nombre = response.Respuestas[13][i].Valores[0].Key+" "+
            //                 response.Respuestas[14][i].Valores[0].Key+" "+
            //                 response.Respuestas[15][i].Valores[0].Key
            //    };
            //    response.Integrantes.Add(i, integrante);
            //}


           /*for (var i = 0; i <= totalIntegrantes; i++)
            {
                if (!response.Respuestas.ContainsKey(16)) continue;
                if (!response.Respuestas[16].ContainsKey(i)) continue;
                var integrante = new Integrante
                {
                    Id = (i - 1).ToString(),
                    Nombre = response.Respuestas[13][i].Valores[0].Key + " " +
                             response.Respuestas[14][i].Valores[0].Key + " " +
                             response.Respuestas[15][i].Valores[0].Key
                };
                response.Integrantes.Add(i, integrante);
            }*/


            response.TotalIntegrantes = totalIntegrantes;
            return View("Index", response);
        }

        /// <summary>
        /// Funcion que guarda en la base de datos las nuevas respuestas de la encuesta aplicada por el promotor
        /// </summary>
        /// <param name="request">Listado de respuestas modificadas por el usuario administrador</param>
        /// <returns>Estatus del guardado en la base de datos</returns>
        [HttpPost]
        public string Guardar([FromBody] AplicacionRequest request)
        {
            try
            {
                var totalIntegrantes = 1;
                var aplicacion = _context.Aplicacion.Find(request.Id);
                var jefe = _context.Beneficiario.Find(aplicacion.BeneficiarioId);
                var completa = false;
                using (var transaction = _context.Database.BeginTransaction())
                {
                    var preguntas = new Dictionary<string, AplicacionPregunta>();
                    var preguntasDB = _context.Pregunta.Where(d => d.DeletedAt == null).ToDictionary(d=>d.Id);
                    var ultimaPregunta = _context.Pregunta.Where(p=> p.DeletedAt==null && p.Activa).OrderBy(p => p.Numero).LastOrDefault();
                    foreach (var ap in _context.AplicacionPregunta
                        .Include(ap => ap.Pregunta)
                        .Where(ap => ap.AplicacionId.Equals(aplicacion.Id) && ap.DeletedAt == null))
                    {
                        preguntas.Add(ap.Pregunta.Numero + "-" +
                                      (ap.Pregunta.Iterable ? (ap.RespuestaIteracion + 1).ToString() : "0")
                                      + "-" + (TipoPreguntaString.Multiples().Contains(ap.Pregunta.TipoPregunta) ? ap.RespuestaId: "0"), ap);
                    }

                    foreach (var respuestaRequest in request.Respuestas)
                    {
                        var preguntaRequest = preguntasDB[respuestaRequest.Value.FirstOrDefault().Value.Id];
                        if (!completa && preguntaRequest.Id==ultimaPregunta.Id)
                        {
                            completa = true;
                        }
                        foreach (var iteracion in respuestaRequest.Value)
                        {
                            int iRespuesta = 0;
                            foreach (var respuesta in iteracion.Value.Valores)
                            {
                                var respuestaId = TipoPreguntaString.Multiples().Contains(preguntaRequest.TipoPregunta) ? respuesta.Value.Key : "0";
                                var llave = preguntaRequest.Numero + "-" + iteracion.Key + "-" + respuestaId;
                                var ap = new AplicacionPregunta();
                                var nuevoRegistro = true;
                                if (preguntas.ContainsKey(llave))
                                {
                                    ap = preguntas[llave];
                                    nuevoRegistro = false;
                                }
                                
                                ap.PreguntaId = preguntaRequest.Id;
                                ap.AplicacionId = aplicacion.Id;
                                ap.RespuestaIteracion = iteracion.Key > 0 ? (iteracion.Key - 1) : iteracion.Key;
                                ap.UpdatedAt = DateTime.Now;
                                ap.DeletedAt = null;
                                ap.Complemento = iteracion.Value.Valores.FirstOrDefault().Value.Value?.Trim();
                                
                                switch(preguntaRequest.TipoPregunta)
                                {
                                    case TipoPreguntaString.Abierta:
                                    case TipoPreguntaString.Integrantes:
                                        ap.Valor = respuesta.Value.Key;
                                        break;
                                    case TipoPreguntaString.Numerica:
                                        ap.Valor = respuesta.Value.Key;
                                        ap.ValorNumerico = int.Parse(respuesta.Value.Key);
                                        ap.RespuestaValor = double.Parse(respuesta.Value.Key);
                                        if (ap.PreguntaId=="987")
                                        {
                                            totalIntegrantes = int.Parse(ap.Valor);
                                        }
                                        break;
                                    case TipoPreguntaString.Fecha:
                                    case TipoPreguntaString.FechaFutura:
                                    case TipoPreguntaString.FechaPasada:
                                        ap.ValorFecha = DateTime.Parse(respuesta.Value.Key);
                                        ap.Valor = ap.ValorFecha.Value.ToString("yyyy/MM/dd");
                                        break;
                                    case TipoPreguntaString.Radio:
                                        if (preguntaRequest.Catalogo==null)
                                        {
                                            ap.RespuestaId = respuesta.Value.Key;
                                        }
                                        else
                                        {
                                            ap.ValorCatalogo = respuesta.Value.Key;
                                        }
                                        ap.Complemento = iteracion.Value.Respuestas.FirstOrDefault().Value.Value;
                                        break;
                                    default:
                                        if (preguntaRequest.Gradual && iteracion.Value.Grados.ContainsKey(iRespuesta))
                                        {
                                            ap.Complemento = iteracion.Value.Respuestas.FirstOrDefault().Value.Value;
                                            ap.Grado = iteracion.Value.Grados[iRespuesta];
                                            ap.Valor = iteracion.Value.Valores[iRespuesta].Key;
                                        }
                                        if (preguntaRequest.Catalogo==null)
                                        {
                                            ap.RespuestaId = respuesta.Value.Key;
                                        }
                                        else
                                        {
                                            ap.ValorCatalogo = respuesta.Value.Key;
                                        }

                                        if (preguntaRequest.TipoPregunta == TipoPreguntaString.Check)
                                        {
                                            ap.Complemento = iteracion.Value.Multiples[respuesta.Key].Value?.Trim();
                                        }
                                        if (preguntaRequest.Gradual)
                                        {
                                            ap.Complemento = iteracion.Value.Respuestas[respuesta.Key].Value?.Trim();
                                        }
                                        break;
                                }

                                if (nuevoRegistro)
                                {
                                    ap.CreatedAt = DateTime.Now;
                                    _context.AplicacionPregunta.Add(ap);
                                }
                                else
                                {
                                    _context.AplicacionPregunta.Update(ap);
                                }

                                iRespuesta++;
                            }
                        }
                    }
                    
                    _context.Bitacora.Add(new Bitacora()
                    {
                        UsuarioId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                        Accion = AccionBitacora.EDICION,
                        Mensaje = "Se modificó la aplicación de encuesta con Folio" + jefe.Folio + ".",
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    });
                    if (completa)
                    {
                        aplicacion.Estatus = EstatusAplicacion.COMPLETO;
                    }
                    aplicacion.Activa = true;
                    _context.Aplicacion.Update(aplicacion);
                    _context.SaveChanges();
                    transaction.Commit();
                }

                if (completa)
                {
                    var app = _context.Aplicacion.Find(request.Id);
                    RegistrarIntegrantes(jefe, request.Respuestas, totalIntegrantes);//Vamos a registrar los integrantes del hogar
                    var api = new ApiController(_context, _configuration, _userManager, _clientFactory);
                    var respuestas = _context.AplicacionPregunta.Where(ap => ap.AplicacionId == app.Id &&
                                                                             ap.PreguntaId.Equals("42") && ap.Grado != null && !ap.Grado.Equals("1")).ToList();
                    var respuestasApiModel = respuestas.Select(respuesta => new AplicacionPreguntaApiModel
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
                        })
                        .ToList();
                    api.RegistrarDiscapacidades(app.BeneficiarioId, respuestasApiModel);//Vamos a registrar las discapacidades de los integrantes
                    var domicilio = _context.Domicilio.Find(jefe.DomicilioId);
                    var municipio = _context.Municipio.FirstOrDefault(m => m.Id == domicilio.MunicipioId);
                    new Coneval().CalcularCarencias(app.Id, domicilio.TipoAsentamientoId, municipio.Indice, _context, null);//Vamos a calcular las carencias del hogar
                    var carencias = _context.AplicacionCarencia.Where(ac=>ac.AplicacionId == app.Id).ToList();
                    new Coneval().CalcularCarenciasCOVID(app, carencias, _context);
                    _context.SaveChanges();
                    
                    var url = _configuration["WS_DIRECCION"];
                    var conexion = _configuration.GetConnectionString("DefaultConnection");
                    
                    if (string.IsNullOrEmpty(domicilio.MunicipioCalculado))
                    {
                        var client = _clientFactory.CreateClient();
                        new Thread(async () => {
                            await ImportacionDiagnosticos.CompletarDomicilio(domicilio, url, client);
                        }).Start();
                    }
                    url = _configuration["WS_DATOS"];
                    var password = _configuration["CA_WS_CURP"];
                    var idApiCurp = _configuration["ID_WS_CURP"];
                    new Thread(async() => {
                        foreach (var integrante in carencias)
                        {
                            await ApiController.CompletarCurp(integrante.BeneficiarioId, conexion, url, password,
                                _clientFactory.CreateClient(), idApiCurp);
                        }
                    }).Start();
                }
            }
            catch (Exception e)
            {
                Excepcion.Registrar(e);
            }
            return "ok";
        }

        private void RegistrarIntegrantes(Beneficiario jefe, Dictionary<int, Dictionary<int, AplicacionPreguntaResponse>> respuestas, 
            int totalIntegrantes)
        {
            var hijos = _context.Beneficiario.Where(b => b.DeletedAt == null && b.HijoId == null &&
                                                               (b.Id == jefe.Id || b.PadreId == jefe.Id)).ToList().ToDictionary(b => b.NumIntegrante);
            var parentescos = _context.Parentesco.Where(s=>s.DeletedAt==null).ToDictionary(s=>s.Id);
            var sexos = _context.Sexo.Where(s=>s.DeletedAt==null).ToDictionary(s=>s.Id);
            var estados = _context.Estado.Where(s=>s.DeletedAt==null).ToDictionary(s=>s.Id);
            var civiles = _context.EstadoCivil.Where(s=>s.DeletedAt==null).ToDictionary(s=>s.Id);
            var niveles = _context.Estudio.Where(s=>s.DeletedAt==null).ToDictionary(s=>s.Id);
            var grados = _context.GradosEstudio.Where(s=>s.DeletedAt==null).ToDictionary(s=>s.Id);
            var discapacidades = _context.Discapacidad.Where(s=>s.DeletedAt==null).ToDictionary(s=>s.Id);
            var causas = _context.CausaDiscapacidad.Where(s=>s.DeletedAt==null).ToDictionary(s=>s.Id);
            var gradosD = _context.DiscapacidadGrado.Where(s=>s.DeletedAt==null && s.Grado != null).ToDictionary(s=> s.DiscapacidadId+"-"+s.Grado);


            var noPerteneceAlHogar = new List<string> {"16", "17","18", "19"};
            var preguntaNombre = _context.Pregunta.FirstOrDefault(p => p.Id == "989");
            var preguntaApellidoP = _context.Pregunta.FirstOrDefault(p => p.Id == "990");
            var preguntaHabitaHogar = _context.Pregunta.FirstOrDefault(p => p.Id == "25");
            var preguntaApellidoM = _context.Pregunta.FirstOrDefault(p => p.Id == "991");
            var preguntaParentesco = _context.Pregunta.FirstOrDefault(p => p.Id == "993");
            var preguntaFechaNacimiento = _context.Pregunta.FirstOrDefault(p => p.Id == "996");
            var preguntaSexo = _context.Pregunta.FirstOrDefault(p => p.Id == "998");
            var preguntaEstado = _context.Pregunta.FirstOrDefault(p => p.Id == "999");
            var preguntaEstadoCivil = _context.Pregunta.FirstOrDefault(p => p.Id == "24");
            var preguntaDiscapacidad = _context.Pregunta.FirstOrDefault(p => p.Id == "42");
            var preguntaEstudio = _context.Pregunta.FirstOrDefault(p => p.Id == "130");
            var preguntaGrado = _context.Pregunta.FirstOrDefault(p => p.Id == "131");
            var preguntaCurp = _context.Pregunta.FirstOrDefault(p => p.Id == "995");
            
            var now = DateTime.Now;
            for (var i = 1; i <= totalIntegrantes; i++) {
                if (noPerteneceAlHogar.Contains(respuestas[preguntaHabitaHogar.Numero][i].Respuestas[0].Key)){
                    continue;
                }
                Beneficiario hijo;
                var nuevoIntegrante = false;
                if (hijos.ContainsKey(i)) {
                    hijo = hijos[i];
                } else {
                    nuevoIntegrante = true;
                    hijo = new Beneficiario {Id = Guid.NewGuid().ToString(), CreatedAt = now};
                }
                hijo.DeviceId = jefe.DeviceId;
                hijo.VersionAplicacion = jefe.VersionAplicacion;
                hijo.Nombre = respuestas[preguntaNombre.Numero][i].Valores[0].Key;
                hijo.ApellidoPaterno = respuestas[preguntaApellidoP.Numero][i].Valores[0].Key;
                hijo.ApellidoMaterno = respuestas[preguntaApellidoM.Numero][i].Valores[0].Key;

                if (respuestas[preguntaFechaNacimiento.Numero].ContainsKey(i))
                {
                    hijo.FechaNacimiento = DateTime.Parse(respuestas[preguntaFechaNacimiento.Numero][i].Valores[0].Key);
                }

                if (respuestas[preguntaSexo.Numero].ContainsKey(i)) {
                    if (sexos.ContainsKey(respuestas[preguntaSexo.Numero][i].Valores[0].Key)){
                        var sexo = sexos[respuestas[preguntaSexo.Numero][i].Valores[0].Key];
                        hijo.SexoId = sexo.Id;
                        hijo.Sexo = sexo;
                    }
                }
                
                if (respuestas[preguntaEstado.Numero].ContainsKey(i)) {
                    if (estados.ContainsKey(respuestas[preguntaEstado.Numero][i].Valores[0].Key)){
                        var estado = estados[respuestas[preguntaEstado.Numero][i].Valores[0].Key];
                        hijo.EstadoId = estado.Id;
                        hijo.Estado = estado;
                    }
                }                
                
                if (respuestas[preguntaEstadoCivil.Numero].ContainsKey(i)) {
                    if (civiles.ContainsKey(respuestas[preguntaEstadoCivil.Numero][i].Valores[0].Key)){
                        var estadoCivil = civiles[respuestas[preguntaEstadoCivil.Numero][i].Valores[0].Key];
                        hijo.EstadoCivilId = estadoCivil.Id;
                    }
                }   
                
                if (respuestas[preguntaParentesco.Numero].ContainsKey(i)) {
                    if (parentescos.ContainsKey(respuestas[preguntaParentesco.Numero][i].Valores[0].Key)){
                        var parentesco = parentescos[respuestas[preguntaParentesco.Numero][i].Valores[0].Key];
                        hijo.ParentescoId = parentesco.Id;
                    }
                }
                
                foreach (var r in respuestas[preguntaDiscapacidad.Numero][i].Respuestas)
                {
                    if (r.Value.Value!=null && r.Value.Value != "1")
                    {
                        hijo.DiscapacidadId = r.Value.Key;
                        hijo.CausaDiscapacidadId = causas[r.Value.Value].Id;
                        hijo.DiscapacidadGradoId = gradosD[r.Value.Key+"-"+r.Value.Value].Id;
                        break;
                    }
                }
                
                if(string.IsNullOrEmpty(hijo.DiscapacidadId)){
                    hijo.DiscapacidadId = "124";//No tiene discapacidad
                }

                if (respuestas[preguntaEstudio.Numero].ContainsKey(i)) {
                    if (niveles.ContainsKey(respuestas[preguntaEstudio.Numero][i].Valores[0].Key)){
                        var nivel = niveles[respuestas[preguntaEstudio.Numero][i].Valores[0].Key];
                        hijo.EstudioId = nivel.Id;
                    }
                }
                else
                {
                    hijo.EstudioId = "566";
                }
                
                if (respuestas[preguntaGrado.Numero].ContainsKey(i)) {
                    if (grados.ContainsKey(respuestas[preguntaGrado.Numero][i].Valores[0].Key)){
                        var grado = grados[respuestas[preguntaGrado.Numero][i].Valores[0].Key];
                        hijo.GradoEstudioId = grado.Id;
                    }
                }

                if (i > 1)
                {
                    hijo.PadreId = jefe.Id;
                    hijo.Folio = jefe.Folio;
                    hijo.NumFamilia = jefe.NumFamilia;
                }
                
                if (respuestas.ContainsKey(preguntaCurp.Numero) && respuestas[preguntaCurp.Numero].ContainsKey(i)) {
                    var curp = respuestas[preguntaCurp.Numero][i].Valores[0].Key;

                    if (!string.IsNullOrEmpty(curp) && curp.Length == 18){
                        hijo.Curp = curp;
                    } else{
                        var CurpGenerator = new Curp(hijo.Nombre, hijo.ApellidoPaterno, hijo.ApellidoMaterno, 
                            hijo.Sexo.Nombre, hijo.FechaNacimiento.Value,Utils.EstadoToEnum(hijo.Estado));
                        hijo.Curp = CurpGenerator.CURP;
                    }
                }else{
                    var CurpGenerator = new Curp(hijo.Nombre, hijo.ApellidoPaterno, hijo.ApellidoMaterno, 
                        hijo.Sexo.Nombre, hijo.FechaNacimiento.Value, Utils.EstadoToEnum(hijo.Estado));
                    hijo.Curp = CurpGenerator.CURP;
                }
                hijo.MismoDomicilio = true;
                hijo.NumIntegrante = i;
                hijo.EstatusInformacion = "Encuesta";
                hijo.UpdatedAt = now;
                hijo.EstatusUpdatedAt = now;
                hijo.TrabajadorId = jefe.TrabajadorId;
                hijo.DomicilioId = jefe.DomicilioId;
                if (nuevoIntegrante)
                {
                    _context.Beneficiario.Add(hijo);
                }
                else
                {
                    _context.Beneficiario.Update(hijo);
                }
            }
            _context.SaveChanges();
        }
    }
}