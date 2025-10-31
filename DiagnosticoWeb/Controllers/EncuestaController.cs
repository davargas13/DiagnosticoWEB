using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DiagnosticoWeb.Models;
using DiagnosticoWeb.Code;
using DiagnosticoWeb.Database;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data.SqlClient;
using System.Security.Claims;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore.Internal;
using DiagnosticoWeb.Validaciones;

namespace DiagnosticoWeb.Controllers
{
    /// <summary>
    /// Clase que permite la visualizacion y configuracion de la encuesta que se aplicará a los beneficiarios para detectar las carencias que hay en su hogar
    /// </summary>
    public class EncuestaController : Controller
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="context">Conexion a la base de datos</param>
        public EncuestaController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Funcion que muestra la vista donde se ve la ultima version de la encuesta CONEVAL
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [RequireClaim("Permiso", Value = "encuesta.ver")]
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Funcion que muestra las encuestas registradas en el sistema, que para este caso solo será la encuesta CONEVAL
        /// </summary>
        /// <param name="request">Filtros de busqueda</param>
        /// <returns>Cadena JSON con la informacion de la encuesta activa</returns>
        [HttpPost]
        [Authorize]
        public string GetEncuestas([FromBody] EncuestaRequest request)
        {
            var response = new EncuestaResponse();
            var encuestaModel = new List<EncuestaModelResponse>();

            var encuestasQuery = _context.Encuesta.Where(x => x.DeletedAt == null);

            if (!string.IsNullOrEmpty(request.Nombre))
            {
                encuestasQuery = encuestasQuery.Where(x => x.Nombre.Contains(request.Nombre));
            }

            response.Total = encuestasQuery.Count();
            var encuestas = encuestasQuery.OrderByDescending(x => x.Activo).ThenByDescending(x => x.UpdatedAt)
                .Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize).ToList();

            foreach (var encuesta in encuestas)
            {
                var version = _context.EncuestaVersion.FirstOrDefault(x => x.EncuestaId.Equals(encuesta.Id) && x.Activa);
                encuestaModel.Add(new EncuestaModelResponse()
                {
                    Id = encuesta.Id,
                    Nombre = encuesta.Nombre,
                    Activo = encuesta.Activo,
                    Vigencia = encuesta.Vigencia,
                    Version = version?.Numero ?? 1,
                    Preguntas = version == null ? 0 :_context.Pregunta.Count(x => x.EncuestaVersionId.Equals(version.Id) && x.Activa)
                });
            }

            response.Encuestas = encuestaModel;

            return JsonConvert.SerializeObject(response);
        }

        /// <summary>
        /// Funcion que guarda los cambios en la configuracion de la encuesta CONEVAL
        /// </summary>
        /// <param name="model">Datos de la encuesta</param>
        /// <returns>Estatus del guardado: ok, error</returns>
        [HttpPost]
        [Authorize]
        public string Guardar([FromBody] EncuestaNombreModel model)
        {
            if (!ModelState.IsValid)
            {
                var errores = ModelState.Select(x => new {Key = x.Key, Error = x.Value.Errors.First().ErrorMessage})
                    .ToList();

                return JsonConvert.SerializeObject(errores);
            }

            var encuesta = new Encuesta()
            {
                Nombre = model.Nombre,
                Mensaje = model.Mensaje,
                Vigencia = int.Parse(model.Vigencia),
                Activo = !_context.Encuesta.Any(),
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                EncuestaVersiones = new List<EncuestaVersion>()
            };
            _context.Encuesta.Add(encuesta);
            _context.SaveChanges();

            var response = new EncuestaResponseCreate();
            response.Id = encuesta.Id;
            response.Result = "ok";

            TempData["Alert"] = "Show";
            TempData.Keep("Alert");

            return JsonConvert.SerializeObject(response);
        }

        /// <summary>
        /// Funcion que muestra la vista para modificar la configuracion de la encuesta CONEVAL y con esto generar una nueva version,
        /// nota: las versiones de la encuesta anterior se mantendran en la base de datos para aquellas solicitudes de apoyo que se generaron con una encuesta anterior
        /// </summary>
        /// <param name="id">Identificador en base de datos de la encuesta a modificar</param>
        /// <returns>Vista para modificar la configuracion de la encuesta CONEVAL</returns>
        [HttpGet]
        [Authorize]
        [RequireClaim("Permiso", Value = "encuesta.editar")]
        public IActionResult Editar(string id = "")
        {
            var response = new EncuestaEditarModelList();
            var encuesta = _context.Encuesta.Include(x => x.EncuestaVersiones).First(e => e.Id.Equals(id));
            var preguntas = new List<PreguntasEditarModel>();
            var preguntasHash = new Dictionary<string, string>();
            
            var version = encuesta.EncuestaVersiones.Where(x => x.Activa).FirstOrDefault();
            if (version != null)
            {
                response.Version = 1;
                var preguntasList = _context.Pregunta
                    .Where(p => p.EncuestaVersionId.Equals(version.Id) && p.DeletedAt == null && p.Id != "-1")
                    .Include(p => p.Carencia)
                    .Include(p => p.Respuestas).ThenInclude(r => r.RespuestaGrados)
                    .Include(p => p.PreguntaGrados)
                    .OrderBy(p => p.Numero);
                var numPregunta = 1;
                foreach (var pregunta in preguntasList)
                {
                    var respuestas = new List<RespuestasEditarModel>();
                    if (pregunta.Respuestas.Any())
                    {
                        foreach (var respuesta in pregunta.Respuestas.OrderBy(x => x.Numero))
                        {
                            var respuestaModel = new RespuestasEditarModel()
                            {
                                Id = Int32.Parse(respuesta.Id),
                                Numero = respuesta.Numero,
                                Nombre = respuesta.Nombre,
                                TipoComplemento = respuesta.TipoComplemento,
                                Complemento = respuesta.Complemento,
                                Negativa = respuesta.Negativa,
                                IsComplemento = respuesta.TipoComplemento != null,
                                RespuestaGrados = new List<RespuestaGradoCreateModel>()
                            };
                            respuesta.RespuestaGrados.ForEach(rg => respuestaModel.RespuestaGrados.Add(new RespuestaGradoCreateModel()
                            {
                                Id = rg.Id,
                                RespuestaId = rg.RespuestaId,
                                PreguntaGradoId = rg.PreguntaGradoId,
                                Grado = rg.Grado,
                            }));
                            respuestas.Add(respuestaModel);
                        }
                    }

                    if (!string.IsNullOrEmpty(pregunta.Catalogo))
                    {
                        var index = 1;
                        var catalogo = Utils.BuildCatalogo(_context, pregunta.Catalogo);
                        respuestas.AddRange(catalogo.Select(model => new RespuestasEditarModel {Id = int.Parse(model.Id), Nombre = model.Nombre, Numero = index++}));
                    }

                    var preguntaModel = new PreguntasEditarModel()
                    {
                        Id = int.Parse(pregunta.Id),
                        Numero = pregunta.Numero,
                        Nombre = pregunta.Nombre,
                        Condicion = pregunta.Condicion,
                        CondicionLista = pregunta.CondicionLista,
                        TipoCondicionalAdicional = pregunta.Respuestas.Any() || pregunta.Catalogo!=null,
                        CondicionIterable = pregunta.CondicionIterable==null?"":pregunta.CondicionIterable.Substring(1),
                        Iterable = pregunta.Iterable,
                        Respuestas = new List<RespuestasEditarModel>(respuestas),
                        TipoPregunta = pregunta.TipoPregunta,
                        Catalogo = pregunta.Catalogo,
                        IsCatalogo = pregunta.Catalogo != null,
                        Gradual = pregunta.Gradual,
                        IsGradual = pregunta.Gradual,
                        PreguntaGrados = new List<PreguntaGradoCreateModel>(),
                        CarenciaId = pregunta.CarenciaId,
                        IsAbierta = pregunta.TipoPregunta.Equals("Abierta"),
                        Expresion = pregunta.Expresion,
                        ExpresionEjemplo = pregunta.ExpresionEjemplo,
                        Editable = pregunta.Editable,
                        TipoComplemento = pregunta.TipoComplemento,
                        Complemento = pregunta.Complemento,
                        Activa = pregunta.Activa,
                        Obligatoria = pregunta.Obligatoria,
                        Excluir = pregunta.Excluir,
                        SeleccionarRespuestas = pregunta.SeleccionarRespuestas,
                        Maximo = pregunta.Maximo,
                        Numerica = pregunta.TipoPregunta.Equals(TipoPregunta.Numerica.ToString())
                                   || (respuestas.Any(r=>r.TipoComplemento!=null &&
                                       (r.TipoComplemento.Equals(TipoComplemento.Numerica.ToString()) || 
                                        (r.TipoComplemento.Equals(TipoComplemento.Listado.ToString()) && r.Complemento.Contains("1,2,3,4,5,6,7")))))
                    };
                    ConstruirCondiciones(preguntaModel);
                    if (pregunta.Carencia != null)
                    {
                        preguntaModel.Carencia = pregunta.Carencia.Nombre;
                        preguntaModel.Color = pregunta.Carencia.Color;
                    }

                    pregunta.PreguntaGrados.ForEach(pg => preguntaModel.PreguntaGrados.Add(new PreguntaGradoCreateModel()
                    {
                        Id = pg.Id,
                        PreguntaId = pg.PreguntaId,
                        Grado = pg.Grado
                    }));
                    preguntas.Add(preguntaModel);
                    preguntasHash[pregunta.Id.Trim()] = preguntaModel.Numero.ToString().Trim();
                }
            }

            var carencias = _context.Carencia.ToList();
            response.Carencias = new List<CarenciaModel>();
            carencias.ForEach(c => response.Carencias.Add(new CarenciaModel()
            {
                Id = c.Id,
                Nombre = c.Nombre,
                Color = c.Color
            }));
            response.Encuesta = new EncuestaEditarModel()
            {
                Id = id,
                Nombre = encuesta.Nombre,
                Vigencia = encuesta.Vigencia,
                Mensaje = encuesta.Mensaje,
                Preguntas = new List<PreguntasEditarModel>(preguntas.Where(p=>p.Activa)),
                PreguntasInactivas = new List<PreguntasEditarModel>(preguntas.Where(p=>!p.Activa))
            };
            response.PreguntasHash = preguntasHash;
            response.TiposRespuestas = Enum.GetNames(typeof(TipoRespuesta)).ToList();
            response.TiposComplementos = Enum.GetNames(typeof(TipoComplemento)).ToList();
            response.Catalogos = Enum.GetNames(typeof(Catalogo)).ToList();
            response.UltimoIdPregunta = _context.Pregunta.Max(r=>int.Parse(r.Id));
            response.UltimoIdRespuesta = _context.Respuesta.Max(r=>int.Parse( r.Id));
            return View(response);
        }

        private void ConstruirCondiciones(PreguntasEditarModel pregunta)
        {
            pregunta.CondicionIds = new List<string>();
            pregunta.Condiciones = new Dictionary<string, string>();
            pregunta.CondicionRespuestas = new Dictionary<string, string[]>();
            pregunta.CondicionOperadores = new Dictionary<string, string[]>();
            if(pregunta.Condicion != null)
            {
                var condiciones = pregunta.Condicion.Substring(1).Split("|");
                foreach (var condicion in condiciones)
                {
                    var items = condicion.Split(',');
                    pregunta.CondicionIds.Add(items[0].Trim());
                    pregunta.Condiciones[items[0].Trim()] = items[1].Trim();
                }

                foreach (var condicion in pregunta.Condiciones)
                {
                    var operador = Regex.Replace(condicion.Value, @"[0-9\-]", string.Empty);
                    if (operador.Any()){
                        if (operador.Contains('/'))
                        {
                            pregunta.CondicionRespuestas[condicion.Key] = condicion.Value.Split('/');
                        }else{
                            var items = condicion.Value.Split("&");
                            if (items.Length > 1)
                            {
                                var op1 = Regex.Replace(items[0], @"[0-9\-]", string.Empty);
                                var op2 = Regex.Replace(items[1], @"[0-9\-]", string.Empty);
                                pregunta.CondicionRespuestas[condicion.Key] = new[]{items[0].Substring(op1.Length), items[1].Substring(op2.Length)};
                                pregunta.CondicionOperadores[condicion.Key] = new[]{op1.PadRight(2,' '), op2.PadRight(2,' ')};
                            }else{
                                pregunta.CondicionRespuestas[condicion.Key] = new[]{condicion.Value.Substring(operador.Length)};
                                pregunta.CondicionOperadores[condicion.Key] = new[]{operador.PadRight(2,' ')}; 
                            }
                        }
                    }else
                    {
                        pregunta.CondicionOperadores[condicion.Key] = new[]{""}; 
                        pregunta.CondicionRespuestas[condicion.Key] = new[]{condicion.Value};
                    }        
                }
            }
        }

        private string ConstruirCondicion(PreguntasEditarModel pregunta)
        {
            var condicion = "";
            var condiciones = new List<string>();
            if (pregunta.CondicionIds!=null && pregunta.CondicionIds.Count > 0){
                foreach (var condicionId in pregunta.CondicionIds)
                {
                    if (!pregunta.CondicionRespuestas.ContainsKey(condicionId)) continue;
                    if (pregunta.CondicionRespuestas[condicionId] == null)
                    {
                        condiciones.Add(condicionId+",");
                    }
                    else
                    {
                        if (pregunta.CondicionRespuestas[condicionId].Length > 1)
                        {
                            // var operador = Regex.Replace(pregunta.CondicionOperadores[condicionId][0], @"[0-9\-]", string.Empty);
                            if (pregunta.CondicionOperadores.ContainsKey(condicionId) && pregunta.CondicionOperadores[condicionId].Any())
                            {
                                condiciones.Add(condicionId+","+pregunta.CondicionOperadores[condicionId][0] +
                                                pregunta.CondicionRespuestas[condicionId][0]+"&"+
                                                pregunta.CondicionOperadores[condicionId][1] +
                                                pregunta.CondicionRespuestas[condicionId][1]);
                            }
                            else
                            {
                                var items = pregunta.CondicionRespuestas[condicionId].Join("/");
                                condiciones.Add(condicionId+","+items);
                            }
                        }else
                        {
                            var valor = pregunta.CondicionRespuestas[condicionId].Length == 0
                                ? "" : pregunta.CondicionRespuestas[condicionId][0];
                            var operador = pregunta.CondicionOperadores.ContainsKey(condicionId)
                                ? pregunta.CondicionOperadores[condicionId][0] : "";
                            condiciones.Add(condicionId+","+operador+valor);
                        }
                    }
                }
                condicion = "?"+condiciones.Join("|");
            } 
            return condicion.Any() ? condicion : null;
        }

        /// <summary>
        /// Funcion que busca una pregunta de acuerdo a la condición de otra pregunta
        /// </summary>
        /// <param name="condicion">Cadena con la condicion</param>
        /// <param name="preguntasMap">Diccionario de las preguntas de la encuesta</param>
        /// <param name="posicion">Variable que indica en que parte de la condicion se encuenstra el Id de la pregunta a buscar</param>
        /// <returns>Cadena con el nombre de la pregunta encontrada</returns>
        public string[] ObtenerPreguntaCondicion(string condicion, Dictionary<string, Pregunta> preguntasMap, int posicion)
        {
            var condiciones = new List<String>();
            if (!string.IsNullOrEmpty(condicion))
            {
                var items = condicion.Substring(1).Split("|");
                foreach (var item in items)
                {
                    var splitCondicion = item.Split(",");
                    condiciones.Add(splitCondicion[posicion]);
                }
            }

            return condiciones.ToArray();
        }

        /// <summary>
        /// Funcion que busca una pregunta de acuerdo a la condicion y respuesta de otra pregunta
        /// </summary>
        /// <param name="condicion">Cadena con la condicion que contiene la pregunta a buscar</param>
        /// <param name="posicion">Posicion dentro de la cadena donde se encuentra el Id de la pregunta a buscar</param>
        /// <param name="preguntasMap">Diccionario de las preguntas de la encuesta</param>
        /// <returns></returns>
        public Pregunta ObtenerPregunta(string condicion, int posicion, Dictionary<string, Pregunta> preguntasMap)
        {
            var splitCondicion = condicion.Substring(1).Split(",");
            var cond = splitCondicion[posicion];
            if (preguntasMap.ContainsKey(cond))
            {
                var pregunta = preguntasMap[cond];
                for (var i = 0; i < pregunta.Respuestas.Count(); i++)
                {
                    pregunta.Respuestas[i].Pregunta = null;
                }

                pregunta.EncuestaVersion = null;
                return pregunta;
            }

            return null;
        }

        /// <summary>
        /// Funcion que valida que la configuracion de la pregunta en la encuesta este correcta
        /// </summary>
        /// <param name="model">Datos de la pregunta a validar</param>
        /// <returns>Estatus de la validacion: ok, error</returns>
        [HttpPost]
        [Authorize]
        public string ValidarCampos([FromBody] EncuestaNombreModel model)
        {
            if (!ModelState.IsValid)
            {
                var errores = ModelState.Select(x => new {Key = x.Key, Error = x.Value.Errors.First().ErrorMessage})
                    .ToList();

                return JsonConvert.SerializeObject(errores);
            }

            var encuesta = _context.Encuesta.FirstOrDefault();
            Encuesta.AcutalizarEncuesta(_context, encuesta, model.Nombre, int.Parse(model.Vigencia), model.Mensaje);
            _context.SaveChanges();
            return "ok";
        }

        /// <summary>
        /// Funcion que valida la estrutura de la pregunta que se quiere agregar a la encuesta
        /// </summary>
        /// <param name="pregunta">Datos de la pregunta</param>
        /// <returns>Estatus de la validacion: ok, error</returns>
        [HttpPost]
        [Authorize]
        public string ValidarRespuestasPregunta([FromBody] PreguntaRespuestasValidationModel pregunta)
        {
            var preguntasConRespuestas = new List<string> { TipoPreguntaString.Listado, TipoPreguntaString.Radio,
                TipoPreguntaString.Check};
            var tiposConRevision = new List<string> {TipoComplementoString.Listado, TipoComplementoString.Catalogo};
            if (!string.IsNullOrEmpty(pregunta.TipoComplemento))
            {
                if (tiposConRevision.Contains(pregunta.TipoComplemento) && string.IsNullOrEmpty(pregunta.Complemento))
                {
                    ModelState.AddModelError("Complemento","El complemento es obligatorio si se selecciona un tipo de complemento");
                }
            }
            for (int i = 0; i < pregunta.Respuestas.Count; i++)
            {
                var respuesta = pregunta.Respuestas[i];
                if (respuesta.Nueva && _context.Respuesta.FirstOrDefault(r=>r.Id == respuesta.Id.ToString()) != null)
                {
                    ModelState.AddModelError("Respuestas["+i+"].Id","Este Id de respuesta ya se encuentra en uso");
                }

                if (!string.IsNullOrEmpty(respuesta.TipoComplemento))
                {
                    if (tiposConRevision.Contains(respuesta.TipoComplemento) && string.IsNullOrEmpty(respuesta.Complemento))
                    {
                        ModelState.AddModelError("Respuestas["+i+"].Complemento","El complemento es obligatorio si se selecciona un tipo de complemento");
                    }
                }
            }

            if (preguntasConRespuestas.Contains(pregunta.TipoPregunta))
            {
                if (!pregunta.Respuestas.Any() && !pregunta.IsCatalogo)
                {
                    ModelState.AddModelError("TipoPregunta","La pregunta debe tener por lo menos una respuesta");
                }
            }

            if (pregunta.IsCatalogo && string.IsNullOrEmpty(pregunta.Catalogo))
            {
                    ModelState.AddModelError("TipoPregunta","La pregunta debe tener un catálogo seleccionado");
            }
            if (!ModelState.IsValid)
            {
                var errores = ModelState.Select(x => new {x.Key, Error = x.Value.Errors.First().ErrorMessage})
                    .ToList();

                return JsonConvert.SerializeObject(errores);
            }
            return "ok";
        }
        
        [HttpPost]
        [Authorize]
        public string ValidarCondicion([FromBody] PreguntasEditarModel pregunta)
        {
            var preguntasConRespuestas = new List<string>() { TipoPreguntaString.Listado, TipoPreguntaString.Radio,
                TipoPreguntaString.Check};
            foreach (var condicion in pregunta.CondicionIds)
            {
                var preguntaDB = _context.Pregunta.Find(condicion);
                if (preguntasConRespuestas.Contains(preguntaDB.TipoPregunta))
                {
                    if (pregunta.CondicionRespuestas.ContainsKey(condicion))
                    {
                        if (!pregunta.CondicionRespuestas[condicion].Any())
                        {
                            ModelState.AddModelError("Condiciones["+condicion+"]","Debes seleccionar una opción");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("Condiciones["+condicion+"]","Debes seleccionar una pregunta en la condición");
                    }
                }
                else
                {
                    if (pregunta.CondicionOperadores.ContainsKey(condicion))
                    {
                        foreach (var operador in pregunta.CondicionOperadores[condicion])
                        {
                            if (string.IsNullOrEmpty(operador))
                            {
                                ModelState.AddModelError("Condiciones["+condicion+"]","Debes agregar una opción");
                            }
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("Condiciones["+condicion+"]","Debes seleccionar una pregunta en la condición");
                    }
                }
            }
            if (!ModelState.IsValid)
            {
                var errores = ModelState.Select(x => new {x.Key, Error = x.Value.Errors.First().ErrorMessage})
                    .ToList();

                return JsonConvert.SerializeObject(errores);
            }
            return "ok";
        }

        public string ValidarPregunta([FromBody] PreguntaValidationModel pregunta)
        {
            if (!ModelState.IsValid)
            {
                var errores = ModelState.Select(x => new {Key = x.Key, Error = x.Value.Errors.First().ErrorMessage})
                    .ToList();

                return JsonConvert.SerializeObject(errores);
            }

            return "ok";
        }

        /// <summary>
        /// Funcion que guarda las nuevas preguntas o las modificaciones a las preguntas ya existentes en la base de datos,
        /// al existir cambios en la estructura de la encuesta entonces se generará una nueva version de la encuesta, dejando la anterior como historico
        /// para no poerder la integridad con los domicilio que contestaron la encuesta anterior
        /// </summary>
        /// <param name="encuesta">Datos de las preguntas de la encuesta CONEVAL</param>
        /// <returns>Estatus de guardado: ok, error</returns>
        [HttpPost]
        [Authorize]
        public string GuardarEncuesta([FromBody] EncuestaEditarModel encuesta)
        {
            var preguntasIdDic = new HashSet<int>();
            var preguntasNumDic = new HashSet<int>();
            var preguntasConRespuestas = new List<string>() { TipoPreguntaString.Listado, TipoPreguntaString.Radio,
                TipoPreguntaString.Check};
            for (var i = 0; i < encuesta.Preguntas.Count(); i++)
            {
                var pregunta = encuesta.Preguntas[i];
                if (pregunta.Activa)
                {
                    if (preguntasIdDic.Contains(pregunta.Id))
                    {
                        ModelState.AddModelError("Id" + i, "Este Id ya se encuentran en uso.");
                    }
                    else
                    {
                        preguntasIdDic.Add(pregunta.Id);
                    }
                }
                if (pregunta.Activa)
                {
                    if (preguntasNumDic.Contains(pregunta.Numero))
                    {
                        ModelState.AddModelError("Numero" + i, "Este número ya se encuentran en uso.");
                    }
                    else
                    {
                        preguntasNumDic.Add(pregunta.Numero);
                    }
                }

                pregunta.Condicion = ConstruirCondicion(pregunta);
                if (pregunta.Iterable && string.IsNullOrEmpty(pregunta.CondicionIterable))
                {
                    ModelState.AddModelError("Iterable" + i, "Es necesario seleccionar la pregunta sobre la que se iterará.");
                }


                if (string.IsNullOrEmpty(pregunta.Condicion)) continue;
                var condiciones = pregunta.Condicion.Substring(1).Split("|");
                foreach (var condicion in condiciones)
                {
                    var items = condicion.Split(",");
                    var respuesta = items[1];
                    if (respuesta.Length == 0)
                    {
                        ModelState.AddModelError("Condicion" + i, "Si se selecciona una pregunta, la condición es obligatoria.");
                    }
                    else
                    {
                        respuesta =  respuesta.Replace(" ", "").Replace("=", "").Replace("<", "").Replace(">", "")
                            .Replace("!", "");
                        if (!respuesta.Any())
                        {
                            ModelState.AddModelError("Condicion" + i, "Si se selecciona una pregunta, la condición es obligatoria.");    
                        }
                    }
                }
            }

            for (var i = 0; i < encuesta.Preguntas.Count; i++)
            {
                var pregunta = encuesta.Preguntas[i];
                if (pregunta.Nueva && _context.Pregunta.FirstOrDefault(p=>p.Id == pregunta.Id.ToString()) != null)
                {
                    ModelState.AddModelError("Id" + i, "Este Id ya se encuentra en uso.");
                }

                if (pregunta.Id == 0)
                {
                    ModelState.AddModelError("Id" + i, "El Id es obligatorio");
                }
                
                if (pregunta.Numero == 0)
                {
                    ModelState.AddModelError("Numero" + i, "El Numero es obligatorio");
                }
                if (string.IsNullOrEmpty(pregunta.Nombre))
                {
                    ModelState.AddModelError("Nombre" + i, "El Nombre es obligatorio");
                }
                else
                {
                    if (pregunta.Nombre.Length > 255)
                    {
                        ModelState.AddModelError("Nombre" + i, "El Nombre debe ser menor a 255 carecteres");
                    }
                }

                if (preguntasConRespuestas.Contains(pregunta.TipoPregunta))
                {
                    if (!pregunta.Respuestas.Any() && !pregunta.IsCatalogo)
                    {
                        ModelState.AddModelError("TipoPregunta","La pregunta debe tener al menos una respuesta");
                    }
                }

                if (pregunta.IsCatalogo && string.IsNullOrEmpty(pregunta.Catalogo))
                {
                    ModelState.AddModelError("TipoPregunta","La pregunta debe tener un catálogo seleccionado");
                }
            }

            if (!ModelState.IsValid)
            {
                var errores = ModelState.Select(x => new {Key = x.Key, Error = x.Value.Errors.First().ErrorMessage});

                return JsonConvert.SerializeObject(errores);
            }

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var now = DateTime.Now;
                    var preguntasDB = _context.Pregunta.ToDictionary(p => p.Id);
                    var enc = _context.Encuesta.Any() ? _context.Encuesta.First() : null;
                    enc = Encuesta.AcutalizarEncuesta(_context, enc, encuesta.Nombre, encuesta.Vigencia, encuesta.Mensaje);
                    _context.SaveChanges();

                    var versionAnterior = EncuestaVersion.FindActiva(_context, enc.Id);
                    if (versionAnterior != null)
                    {
                        versionAnterior.Activa = false;
                        _context.EncuestaVersion.Update(versionAnterior);
                    }

                    var encuestaVersion = EncuestaVersion.AgregarVersion(versionAnterior.Numero, enc.Id);
                    enc.EncuestaVersiones.Add(encuestaVersion);
                    _context.Encuesta.Update(enc);
                    _context.SaveChanges();

                    foreach (var preg in encuesta.Preguntas)
                    {
                        var nuevaPregunta = false;
                        Pregunta pregunta;
                        preguntasDB.TryGetValue(preg.Id.ToString(), out pregunta);
                        if (pregunta == null)
                        {
                            pregunta = new Pregunta();
                            nuevaPregunta = true;
                        }   
                        pregunta.Id = preg.Id.ToString();
                        pregunta.Nombre = preg.Nombre;
                        pregunta.CarenciaId = preg.CarenciaId;
                        pregunta.Catalogo = string.IsNullOrEmpty(preg.Catalogo) ? null : preg.Catalogo;
                        pregunta.TipoPregunta = preg.TipoPregunta;
                        pregunta.Condicion = preg.Condicion;
                        pregunta.CondicionLista = preg.CondicionLista;
                        pregunta.CondicionIterable = preg.CondicionIterable.Any()?"?"+preg.CondicionIterable:null;
                        pregunta.Iterable = preg.Iterable;
                        pregunta.EncuestaVersionId = encuestaVersion.Id;
                        pregunta.Numero = preg.Numero;
                        pregunta.CreatedAt = now;
                        pregunta.UpdatedAt = now;
                        pregunta.Gradual = preg.Gradual;
                        pregunta.Respuestas = new List<Respuesta>();
                        pregunta.Expresion = preg.Expresion;
                        pregunta.ExpresionEjemplo = preg.ExpresionEjemplo;
                        pregunta.Editable = preg.Editable;
                        pregunta.TipoComplemento = string.IsNullOrEmpty(preg.TipoComplemento) ? null : preg.TipoComplemento;
                        pregunta.Complemento = string.IsNullOrEmpty(preg.Complemento) ? null : preg.Complemento;
                        pregunta.Activa = preg.Activa;
                        pregunta.Obligatoria = preg.Obligatoria;
                        pregunta.Excluir = preg.Excluir;
                        pregunta.Maximo = preg.Maximo;
                        pregunta.SeleccionarRespuestas = preg.SeleccionarRespuestas;
                        
                        if (nuevaPregunta)
                        {
                            _context.Pregunta.Add(pregunta);
                        }
                        else
                        {
                            _context.Pregunta.Update(pregunta);
                        }
                            
                        var preguntasGrados = _context.PreguntaGrado.Where(pg => pg.PreguntaId.Equals(pregunta.Id))
                            .ToDictionary(pg => pg.Grado);
                     
                        foreach (var grado in preg.PreguntaGrados)
                        {
                            PreguntaGrado preguntaGrado;
                            preguntasGrados.TryGetValue(grado.Grado, out preguntaGrado);
                            if (preguntaGrado == null)
                            {
                                preguntaGrado = new PreguntaGrado();
                            }

                            preguntaGrado.PreguntaId = preg.Id.ToString();
                            preguntaGrado.Grado = grado.Grado;
                            if (preguntaGrado.Id == null)
                            {
                                preguntaGrado.CreatedAt = now;
                                preguntaGrado.UpdatedAt = now;
                                _context.PreguntaGrado.Add(preguntaGrado);
                            }
                            else
                            {
                                preguntaGrado.UpdatedAt = now;
                                _context.PreguntaGrado.Update(preguntaGrado);
                            }
                        }

                        if (string.IsNullOrEmpty(preg.Catalogo))
                        {
                            var respuestas = _context.Respuesta.Where(pg => pg.PreguntaId.Equals(pregunta.Id))
                                .ToDictionary(pg => pg.Id);
                            foreach (var resp in preg.Respuestas)
                            {
                                var nuevaRespuesta = false;
                                Respuesta respuesta;
                                respuestas.TryGetValue(resp.Id.ToString(), out respuesta);
                                if (respuesta == null)
                                {
                                    respuesta = new Respuesta();
                                    nuevaRespuesta = true;
                                }

                                respuesta.Id = resp.Id.ToString();
                                respuesta.PreguntaId = preg.Id.ToString();
                                respuesta.Nombre = resp.Nombre;
                                respuesta.Numero = resp.Numero;
                                respuesta.Negativa = resp.Negativa;
                                respuesta.TipoComplemento = string.IsNullOrEmpty(resp.TipoComplemento) ? null : resp.TipoComplemento;
                                respuesta.Complemento = string.IsNullOrEmpty(resp.Complemento) ? null : resp.Complemento;
                                respuesta.Pregunta = pregunta;
                                if (nuevaRespuesta)
                                {
                                    respuesta.CreatedAt = now;
                                    respuesta.UpdatedAt = now;
                                    _context.Respuesta.Add(respuesta);
                                }
                                else
                                {
                                    respuesta.UpdatedAt = now;
                                    _context.Respuesta.Update(respuesta);
                                }

                                if (resp.RespuestaGrados == null || !resp.RespuestaGrados.Any()) continue;
                                var respuestasGrados = _context.RespuestaGrado
                                    .Where(rg => rg.RespuestaId.Equals(resp.Id.ToString())).ToDictionary(rg => rg.Grado);
                                foreach (var grado in resp.RespuestaGrados)
                                {
                                    RespuestaGrado respuestaGrado;
                                    respuestasGrados.TryGetValue(grado.Grado, out respuestaGrado);
                                    if (respuestaGrado == null)
                                    {
                                        respuestaGrado = new RespuestaGrado();
                                    }

                                    respuestaGrado.RespuestaId = resp.Id.ToString();
                                    respuestaGrado.Grado = grado.Grado;
                                    respuestaGrado.PreguntaGradoId = grado.PreguntaGradoId;
                                    if (respuestaGrado.Id == null)
                                    {
                                        respuestaGrado.CreatedAt = now;
                                        respuestaGrado.UpdatedAt = now;
                                        _context.RespuestaGrado.Add(respuestaGrado);
                                    }
                                    else
                                    {
                                        respuestaGrado.UpdatedAt = now;
                                        _context.RespuestaGrado.Update(respuestaGrado);
                                    }
                                }
                            }
                        }
                    }

                    foreach (var preg in encuesta.PreguntasInactivas)
                    {
                        if (preguntasDB.ContainsKey(preg.Id.ToString()))
                        {
                            var pregunta = preguntasDB[preg.Id.ToString()];
                            pregunta.EncuestaVersionId = encuestaVersion.Id;
                            pregunta.Activa = false;
                            _context.Pregunta.Update(pregunta);
                        }
                    }

                    _context.SaveChanges();

                    _context.Bitacora.Add(new Bitacora()
                    {
                        UsuarioId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                        Accion = AccionBitacora.EDICION,
                        Mensaje = "Se modificó la encuesta " + enc.Nombre + ".",
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    });

                    _context.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    return e.Message;
                }
            }

            TempData["Alert"] = "Show";
            TempData.Keep("Alert");

            return "ok";
        }

        /// <summary>
        /// Funcion que construye la cadena de condicion en el formato que requiere la aplicacion movil para mostrar o
        /// no una pregunta dentro de la aplicacion de la encuesta CONEVAL
        /// </summary>
        /// <param name="preguntasMap">Diccionario con las preguntas de la encuesta</param>
        /// <param name="preguntas">Id de la pregunta con la condicion</param>
        /// <param name="EsPregunta">Variable para identificar si la condicion tiene el id de una pregunta o respuesta</param>
        /// <param name="respuestas">Variable para identificar si la condicion tiene el id de una pregunta o respuesta</param>
        /// <returns>Cadena con la condicion en el formato requerido por la aplicacion movil</returns>
        public string ConstruirCondicion(string[] preguntas ,string[] respuestas)
        {
            var items = new List<String>();
            if (preguntas.Any() && respuestas.Any())
            {
                var i = 0;
                foreach (var pregunta in preguntas)
                {
                    items.Add((i == 0 ? "?" : "") + pregunta + "," + respuestas[i]);
                    i++;
                }
            }

            return items.Join("|");
        }

        public IActionResult GraficasOpciones()
        {
            var date = DateTime.Now;
            var response = new GraficasOpcionesResponse
            {
                Municipios = new List<Model>(),
                Preguntas = new List<Model>()
            };
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            response.Inicio = firstDayOfMonth.ToString("yyyy-MM-dd");
            response.Fin = lastDayOfMonth.ToString("yyyy-MM-dd");
            
            _context.Municipio.Where(m => m.DeletedAt == null).ToList().ForEach(m=>response.Municipios.Add(new Model
            {
                Id = m.Id, Nombre = m.Nombre
            }));
            _context.Pregunta.Where(p=>p.DeletedAt==null && p.TipoPregunta.Equals(TipoPregunta.Radio.ToString())
                                                       || p.TipoPregunta.Equals(TipoPregunta.Check.ToString())
                                                       || p.TipoPregunta.Equals(TipoPregunta.Listado.ToString()))
                .OrderBy(p=>p.Numero).Include(p=>p.Respuestas).ToList().ForEach(p=>response.Preguntas.Add(new Model
                {
                    Id = p.Id, Nombre = p.Numero+" "+p.Nombre
                }));
            return View("GraficasOpciones", response);
        }
        
        [HttpPost]
        [Authorize]
        public string GetUsoOpciones([FromBody] GraficasOpcionesRequest request)
        {
            var response = new Dictionary<string, UsoPregunta>();
            var preguntas = _context.Pregunta.Where(p=>p.DeletedAt==null && p.TipoPregunta.Equals(TipoPregunta.Radio.ToString())
                                                       || p.TipoPregunta.Equals(TipoPregunta.Check.ToString())
                                                       || p.TipoPregunta.Equals(TipoPregunta.Listado.ToString()));
            if (!string.IsNullOrEmpty(request.PreguntaId))
            {
                preguntas = preguntas.Where(p => p.Id.Equals(request.PreguntaId));
            }

            foreach (var pregunta in preguntas.Include(p => p.Respuestas).OrderBy(p=>p.Numero).ToList())
            {
                if (pregunta.Respuestas.Any())
                {
                    var usoPregunta = new UsoPregunta
                    {
                        Id = pregunta.Id,
                        Numero = pregunta.Numero,
                        Pregunta = pregunta.Nombre,
                        Mostrar = false,
                        Activa = pregunta.Activa,
                        Respuestas = new Dictionary<string, Respuesta>()
                    }; 
                    foreach (var respuesta in pregunta.Respuestas.OrderBy(r=>r.Numero))
                    {
                        usoPregunta.Respuestas.Add(respuesta.Id, new Respuesta
                        {
                            Id = respuesta.Id,
                            Nombre = respuesta.Nombre,
                            Numero = 0
                        });
                    }
                    response.Add(pregunta.Id, usoPregunta);
                }
                else
                {
                    if (pregunta.Catalogo!=null)
                    {
                        var usoPregunta = new UsoPregunta
                        {
                            Id = pregunta.Id,
                            Numero = pregunta.Numero,
                            Pregunta = pregunta.Nombre,
                            Mostrar = false,
                            Activa = pregunta.Activa,
                            Respuestas = new Dictionary<string, Respuesta>()
                        };
                        var catalogo = Utils.BuildCatalogo(_context, pregunta.Catalogo);
                        foreach (var respuesta in catalogo.OrderBy(r=>r.Nombre))
                        {
                            usoPregunta.Respuestas.Add(respuesta.Id, new Respuesta
                            {
                                Id = respuesta.Id,
                                Nombre = respuesta.Nombre,
                                Numero = 0
                            });
                        }
                        response.Add(pregunta.Id, usoPregunta);
                    }
                }
            }
            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                var now = DateTime.Now;
                var inicio = new DateTime(now.Year, now.Month, 1).ToString("yyyy-MM-dd");
                var fin = new DateTime(now.Year, now.Month, DateTime.DaysInMonth(now.Year, now.Month)).ToString("yyyy-MM-dd");
                if (!string.IsNullOrEmpty(request.Inicio))
                {
                    inicio = request.Inicio.Substring(0, 10);
                }

                if (!string.IsNullOrEmpty(request.Fin))
                {
                    fin = request.Fin.Substring(0, 10);
                }
                var query = "select AP.PreguntaId, AP.RespuestaId "+
                        " from AplicacionPreguntas AP inner join Aplicaciones A on A.Id = AP.AplicacionId And A.Activa = 1"+
                        " inner join Beneficiarios B on B.id = A.BeneficiarioId "+
                        " inner join Domicilio D on D.Id = B.DomicilioId ";
                var where = " where A.CreatedAt >= @0 and A.CreatedAt <= @1 and A.DeletedAt is null and AP.RespuestaId is not null ";
                var iParametro = 2;
                var parametros = new List<string> {inicio, fin};
                if (!string.IsNullOrEmpty(request.PreguntaId))
                {
                    where += " and Ap.PreguntaId=@2";
                    parametros.Add(request.PreguntaId);
                    iParametro = 3;
                }
                if (!string.IsNullOrEmpty(request.Folio))
                {
                    parametros.Add(request.Folio);
                    where += " and B.Folio=@"+(iParametro++)+" ";
                }
                if (!string.IsNullOrEmpty(request.MunicipioId))
                {
                    parametros.Add(request.MunicipioId);
                    where += " and D.MunicipioId = @"+(iParametro++)+" ";
                }
                if (!string.IsNullOrEmpty(request.LocalidadId))
                {
                    parametros.Add(request.LocalidadId);
                    where += " and D.LocalidadId = @"+(iParametro++)+" ";
                }
                if (!string.IsNullOrEmpty(request.AgebId))
                {
                    parametros.Add(request.AgebId);
                    where += " and D.AgebId = @"+(iParametro++)+" ";
                }
            
                if (!string.IsNullOrEmpty(request.ZonaImpulsoId))
                {
                    parametros.Add(request.ZonaImpulsoId);
                    where += " and D.ZonaImpulsoId = @"+(iParametro++)+" ";
                }

                where += " ORDER BY A.CreatedAt Desc";
                iParametro = 0;
                foreach (var parametro in parametros)
                {
                    command.Parameters.Add(new SqlParameter("@"+(iParametro++),parametro));
                }

                command.CommandText = query + where;
                _context.Database.OpenConnection();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var preguntaId = reader.GetString(0);
                            var respuestaId = reader.GetString(1);
                            var pregunta = response[preguntaId];
                            pregunta.Total++;
                            pregunta.Respuestas[respuestaId].Numero++;
                        }
                    }
                }
            }
            return JsonSedeshu.SerializeObject(new
            {
                ResultadosInactivos = response.Values.Where(p=>!p.Activa).ToList(),
                ResultadosActivos = response.Values.Where(p=>p.Activa).ToList().OrderBy(p=>p.Numero)
            });
        }

        public void BuildEstructuraSabana()
        {
            var existe = false; 
            var preguntas = _context.Pregunta.Where(p => p.DeletedAt == null).Include(p => p.Respuestas)
                    .OrderBy(p=>p.Numero).ToList(); 
            using (var command = _context.Database.GetDbConnection().CreateCommand()) 
            {
                command.CommandText = "select name from sys.objects where type = 'U' and name='Sabana'";
                _context.Database.OpenConnection();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        existe = !reader.IsDBNull(0);
                    }
                }
            }

            if (existe)
            {
                var variablesMap = new HashSet<string>();
                var nuevasVariables = new List<string>();
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "SELECT c.Name FROM sys.columns c JOIN sys.objects o ON o.object_id = c.object_id WHERE o.type = 'U' and o.name='Sabana' ORDER BY c.Name ";
                    _context.Database.OpenConnection();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            variablesMap.Add(reader.GetString(0));
                        }
                    } 
                } 
                foreach (var pregunta in preguntas) 
                {
                    if (pregunta.Id.Equals("-1")) { continue; } 
                    pregunta.RespuestasMap = new Dictionary<string, Respuesta>(); 
                    if ((pregunta.Gradual||pregunta.TipoPregunta.Equals(TipoPregunta.Check.ToString())) && pregunta.Respuestas.Any()) { 
                        pregunta.RespuestasMap = pregunta.Respuestas.OrderBy(r =>int.Parse(r.Id)).ToDictionary(r=>r.Id); 
                        var numRespuesta = 1; 
                        foreach (var respuesta in pregunta.RespuestasMap.Values) {
                            if (!variablesMap.Contains("var"+pregunta.Id+"_"+numRespuesta))
                            {
                                nuevasVariables.Add("var" + pregunta.Id + "_" + numRespuesta + " nvarchar(50)");
                            }
                            if (pregunta.Gradual) { 
                                if (!respuesta.Negativa) 
                                { 
                                    if (pregunta.Id.Equals("117")) 
                                    { 
                                        if (!variablesMap.Contains("vartx1_"+pregunta.Id+"_"+numRespuesta))
                                        {
                                            nuevasVariables.Add("vartx1_"+pregunta.Id+"_"+numRespuesta+" nvarchar(200)");
                                        }
                                    }
                                    else
                                    {
                                        if (!variablesMap.Contains("vartx1_"+pregunta.Id+"_"+numRespuesta))
                                        {
                                            nuevasVariables.Add("vartx1_"+pregunta.Id+"_"+numRespuesta+" nvarchar(200)");
                                        }
                                        if (respuesta.Complemento != null) {
                                            if (!variablesMap.Contains("vartx2_"+pregunta.Id+"_"+numRespuesta))
                                            {
                                                nuevasVariables.Add("vartx2_"+pregunta.Id+"_"+numRespuesta+" nvarchar(200)");
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (respuesta.Complemento != null) {
                                    if (!variablesMap.Contains("vartx1_"+pregunta.Id+"_"+numRespuesta))
                                    {
                                        nuevasVariables.Add("vartx1_"+pregunta.Id+"_"+numRespuesta+" nvarchar(200)");
                                    }
                                }
                            }

                            numRespuesta++;
                        }
                    } else {
                        pregunta.RespuestasMap = pregunta.Respuestas.OrderBy(r =>int.Parse(r.Id)).ToDictionary(r=>r.Id);
                        if (!variablesMap.Contains("var"+pregunta.Id+"_1"))
                        {
                            nuevasVariables.Add("var"+pregunta.Id+"_1 nvarchar(50)");
                        }
                        if (!pregunta.TipoPregunta.Equals(TipoPregunta.Radio.ToString())) continue;
                        var numRespuesta = 1;
                        foreach (var respuesta in pregunta.RespuestasMap.Values.Where(r => r.Complemento != null).ToList())
                        {
                            if (!variablesMap.Contains("vartx1_"+pregunta.Id+"_"+numRespuesta))
                            {
                                nuevasVariables.Add("vartx1_"+pregunta.Id+"_"+numRespuesta+" nvarchar(200)");
                            }
                            numRespuesta++;
                        }
                    }
                }
                foreach (var nuevaVariable in nuevasVariables)
                {
                    _context.Database.ExecuteSqlCommand("ALTER TABLE Sabana ADD "+nuevaVariable+" NULL;");
                }
            }
            else
            {
                var tabla = "CREATE TABLE Sabana(";
                tabla += "createdat datetime2 not null, ";
                tabla += "updatedat datetime2 not null, ";
                tabla += "localidad_id nvarchar(100) null, ";
                tabla += "ageb_id nvarchar(100) null, ";
                tabla += "manzana_id nvarchar(100) null, ";
                tabla += "id_fam nvarchar(100) not null, ";
                tabla += "cab_idencuestador nvarchar(100) not null, ";
                tabla += "nombre_encuestador nvarchar(200) not null, ";
                tabla += "cab_idencuesta nvarchar(100) not null, ";
                tabla += "cab_fechaencuesta nvarchar(20) not null, ";
                tabla += "cab_horaencuesta nvarchar(10) not null, ";
                tabla += "cab_fechafinencuesta nvarchar(20) not null, ";
                tabla += "cab_horafinencuesta nvarchar(10) not null, ";
                tabla += "cab_tiempoencuesta nvarchar(20) not null,";
                tabla += "cab_fechainsert nvarchar(100) not null, ";
                tabla += "fecha_sync nvarchar(100) not null, ";
                tabla += "hora_sync nvarchar(10) not null, ";
                tabla += "cab_latitud nvarchar(20) not null, ";
                tabla += "cab_longitud nvarchar(20) not null, ";
                tabla += "latitud_corregida nvarchar(20) not null, ";
                tabla += "longitud_corregida nvarchar(20) not null, ";
                tabla += "estatus_api nvarchar(20) not null, ";
                tabla += "id_integrante nvarchar(20) not null , ";
                tabla += "T nvarchar(20) not null, ";
                tabla += "id_viv nvarchar(100) not null, ";
                tabla += "NumHogar nvarchar(20) not null, ";                
                tabla += "Version nvarchar(10) not null, ";
                tabla += "cve_mun nvarchar(100) not null, ";
                tabla += "nom_mun nvarchar(200) not null, ";
                tabla += "cve_loc nvarchar(100) not null, ";
                tabla += "nom_localidad nvarchar(200) not null, ";
                tabla += "cve_ageb nvarchar(100) not null, ";
                tabla += "cve_manzana nvarchar(100) not null, ";
                tabla += "bandera_geografica nvarchar(100) not null, ";
                tabla += "cve_poli nvarchar(100) not null, ";
                tabla += "clave_zona_impulso nvarchar(100) not null, ";
                tabla += "zap nvarchar(100) not null, ";
                tabla += "tipozap nvarchar(100) not null, ";
                tabla += "colonia_oficial nvarchar(100) not null, ";
                tabla += "codigo_postal nvarchar(100) not null, ";
                tabla += "clave_municipio_cis nvarchar(100) not null, ";
                tabla += "nombre_oficial_cis nvarchar(200) not null, ";
                tabla += "domicilio_cis nvarchar(200) not null, ";
                tabla += "idh_municipio nvarchar(100) not null, ";
                tabla += "marginacion_ageb nvarchar(100) not null, ";
                tabla += "marginacion_localidad nvarchar(100) not null, ";
                tabla += "marginacion_municipio nvarchar(100) not null, ";
                tabla += "curp_renapo nvarchar(100) not null, ";
                tabla += "telefono_casa nvarchar(100) not null, ";
                tabla += "email nvarchar(200) not null, ";
                for (var  i = 969; i < 984; i++)
                {
                    tabla += "var"+i+"_1 nvarchar(100) not null, ";
                }

                var preguntasMap = EncuestaCode.ObtenerPreguntasParaSabana(preguntas);
                foreach (var keyValuePair in preguntasMap)
                {
                    tabla += keyValuePair.Key+" "+keyValuePair.Value+", ";
                }
                tabla += "primary key (id_fam, id_integrante)) ";
                _context.Database.ExecuteSqlCommand(tabla);
            }
        }
    }
}