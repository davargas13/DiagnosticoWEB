using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using DiagnosticoWeb.Database;
using DiagnosticoWeb.Models;
using DiagnosticoWeb.Validaciones;
using moment.net;
using moment.net.Enums;

namespace DiagnosticoWeb.Code
{
    /// <summary>
    /// Clase para calcular las carencias de la encuesta CONEVAL
    /// </summary>
    public class Coneval
    {
        /// <summary>
        /// Función que contruye el JSON que necesita el JAR coneval.jar para caluclar las carencias del hogar visitado, el JSON constituye las respuestas de la encuesta,
        /// esta cadena se envía a una aplicación en java la cual devuelve otro JSON con el resultado de las carencias
        /// </summary>
        /// <param name="aplicacion">Registro en base de datos de la encuesta aplicada al hogar</param>
        /// <param name="preguntasList">Preguntas de la encuesta CONEVAL</param>
        public string CalcularCarencias(string id, string asentamiento, double indice, ApplicationDbContext _context,
            int? version)
        {
            var preguntas = _context.AplicacionPregunta.Where(ap => ap.AplicacionId.Equals(id))
                .OrderBy(ap => ap.RespuestaIteracion).ToList();
            var aplicacion = _context.Aplicacion.Find(id);
            var lineaBienestar = _context.LineaBienestar
                .Where(lb => lb.CreatedAt <= aplicacion.FechaInicio.Value.StartOf(DateTimeAnchor.Month))
                .OrderBy(lb => lb.CreatedAt).LastOrDefault();
            if (lineaBienestar == null)
            {
                lineaBienestar = _context.LineaBienestar.FirstOrDefault();
            }

            var request = new AplicacionRespuestaRequest
            {
                asentamiento = asentamiento, indice = indice, minimarural = lineaBienestar.MinimaRural,
                rural = lineaBienestar.Rural,
                minimaurbana = lineaBienestar.MinimaUrbana, urbana = lineaBienestar.Urbana
            };

            var preguntasMap = new Dictionary<int, Dictionary<string, PreguntaCarenciaRequest>>();
            foreach (var aplicacionPregunta in preguntas)
            {
                var valor = aplicacionPregunta.RespuestaId != null
                    ? aplicacionPregunta.RespuestaId
                    : (aplicacionPregunta.ValorCatalogo != null
                        ? aplicacionPregunta.ValorCatalogo
                        : aplicacionPregunta.Valor);
                if (aplicacionPregunta.PreguntaId.Equals("51"))
                {
                    valor = aplicacionPregunta.Complemento ?? aplicacionPregunta.Valor;
                }

                if (!preguntasMap.ContainsKey(aplicacionPregunta.RespuestaIteracion))
                {
                    preguntasMap.Add(aplicacionPregunta.RespuestaIteracion,
                        new Dictionary<string, PreguntaCarenciaRequest>());
                }

                if (!preguntasMap[aplicacionPregunta.RespuestaIteracion].ContainsKey(aplicacionPregunta.PreguntaId))
                {
                    preguntasMap[aplicacionPregunta.RespuestaIteracion].Add(aplicacionPregunta.PreguntaId,
                        new PreguntaCarenciaRequest(aplicacionPregunta.PreguntaId));
                }

                if (aplicacionPregunta.PreguntaId.Equals("117"))
                {
                    if (aplicacionPregunta.Grado != null && aplicacionPregunta.Grado.Equals("1"))
                    {

                        preguntasMap[aplicacionPregunta.RespuestaIteracion][aplicacionPregunta.PreguntaId].valor
                            .Add(valor);
                    }
                }
                else
                {
                    preguntasMap[aplicacionPregunta.RespuestaIteracion][aplicacionPregunta.PreguntaId].valor.Add(valor);
                }

            }

            var iteraciones = new List<IteracionRequest>();
            foreach (var value in preguntasMap.Values)
            {
                var ir = new IteracionRequest {preguntas = new List<PreguntaCarenciaRequest>(value.Values)};
                iteraciones.Add(ir);

            }

            request.iteraciones = iteraciones;

            AlgoritmoVersion ultimaversion = null;
            var versionLocal = 1;
            if (version == null)
            {
                ultimaversion = _context.AlgoritmoVersion.Where(av => av.AutorizadorId != null)
                    .OrderBy(av => av.Version).Last();
                versionLocal = ultimaversion.Version;
            }
            else
            {
                ultimaversion = _context.AlgoritmoVersion.FirstOrDefault(av => av.Version == version);
                versionLocal = ultimaversion.Version;
            }

            var json = JsonSedeshu.SerializeObject(request);
            json = json.Replace("\"", "'");
            var folderName = Path.Combine("wwwroot", "coneval");
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            var conevalPath = Path.Combine(pathToSave, "coneval" + versionLocal + ".jar");
            var output = "";
            try
            {
                using (var process = new Process
                {
                    EnableRaisingEvents = false,
                    StartInfo = {FileName = "java", Arguments = "-jar "+conevalPath+" \"" + json + "\" ", RedirectStandardError = true}
                }) {
                    process.StartInfo.RedirectStandardOutput = true;
                    process.Start(); 
                    output = process.StandardOutput.ReadToEnd(); 
                    process.WaitForExit();
                    if (string.IsNullOrEmpty(output))
                    {
                        var error = process.StandardError.ReadToEnd();
                        Excepcion.Registrar(error);
                        return error;
                    }
                    var familia = JsonConvert.DeserializeObject<Familia>(output);
                    ActualizarResultados(familia, id, _context, ultimaversion, version == null);
                }
            }
            catch (Exception e)
            {
                Excepcion.Registrar(e);
                return e.Message;
            }
            return "ok";
        }

        public void ActualizarResultados(Familia familia, string id ,ApplicationDbContext _context, AlgoritmoVersion version, bool actual)
        {
            var now = DateTime.Now;
            var numEducativas = 0;
            var numSalud = 0;
            var numSocial = 0;
            var numCarencias = 0;
            
            var aplicacion = _context.Aplicacion.FirstOrDefault(a => a.Id.Equals(id));
            if (actual)
            {
                var aplicacionIntegrantes = _context.AplicacionCarencia
                    .Where(ap => ap.DeletedAt == null && ap.AplicacionId.Equals(aplicacion.Id))
                    .ToDictionary(ap => ap.NumIntegrante);
                var jefeFamilia =
                    _context.Beneficiario.FirstOrDefault(b => b.DeletedAt == null && b.Id==aplicacion.BeneficiarioId);
                var integrantes =
                    _context.Beneficiario.Where(b => b.DeletedAt == null && b.HijoId == null && b.PadreId.Equals(jefeFamilia.Id)).ToDictionary(h=>h.NumIntegrante);
                aplicacion.Analfabetismo = familia.Analfabetismo;
                aplicacion.Inasistencia = familia.Inasistencia;
                aplicacion.PrimariaIncompleta = familia.PrimariaCompleta;
                aplicacion.SecundariaIncompleta = familia.SecundariaCompleta;
                aplicacion.Educativa = familia.Educativa;
                aplicacion.ServicioSalud = familia.ServiciosSalud;
                aplicacion.SeguridadSocial = familia.SeguridadSocial;
                aplicacion.Vivienda = familia.Vivienda;
                aplicacion.Piso = familia.Piso;
                aplicacion.Techo = familia.Techo;
                aplicacion.Muro = familia.Muro;
                aplicacion.Hacinamiento = familia.Hacinamiento;
                aplicacion.Servicios = familia.Servicios;
                aplicacion.Agua = familia.Agua;
                aplicacion.Drenaje = familia.Drenaje;
                aplicacion.Electricidad = familia.Electricidad;
                aplicacion.Combustible = familia.Combustible;
                aplicacion.Alimentaria = familia.Alimentaria;
                aplicacion.Ingreso = familia.IngresoFamiliar;
                aplicacion.Pea = familia.EconomicamenteActiva;
                aplicacion.LineaBienestar = familia.LineaBienestar;
                aplicacion.NivelPobreza = familia.NivelPobreza;
                aplicacion.VersionAlgoritmo = version.Version;
                aplicacion.GradoAlimentaria = familia.GradoAlimnetaria;
                aplicacion.TieneActa = familia.TieneActa;
                aplicacion.SeguridadPublica = familia.SeguridadPublica;
                aplicacion.SeguridadParque = familia.SeguridadParque;
                aplicacion.ConfianzaLiderez = familia.ConfianzaLiderez;
                aplicacion.ConfianzaInstituciones = familia.ConfianzaInstituciones;
                aplicacion.Movilidad = familia.Movilidad;
                aplicacion.RedesSociales = familia.RedesSociales;
                aplicacion.Tejido = familia.Tejido;
                aplicacion.Satisfaccion = familia.Satisfaccion;
                aplicacion.PropiedadVivienda = familia.PropiedadVivienda;
                aplicacion.UpdatedAt = now;
                aplicacion.NumeroIntegrantes = familia.Integrantes.Count;
                _context.Aplicacion.Update(aplicacion);
                
                foreach (var integrante in familia.Integrantes)
                {
                    var aplicacionIntegrante = new AplicacionCarencia();
                    var nuevoIntegrante = true;
                    if (aplicacionIntegrantes.ContainsKey(integrante.NumIntegrante))
                    {
                        nuevoIntegrante = false;
                        aplicacionIntegrante = aplicacionIntegrantes[integrante.NumIntegrante];
                    }
                    else
                    {
                        aplicacionIntegrante.Id = Guid.NewGuid().ToString();
                        aplicacionIntegrante.CreatedAt = now;
                        aplicacionIntegrante.AplicacionId = aplicacion.Id;
                    }

                    aplicacionIntegrante.Analfabetismo = integrante.Analfabetismo;
                    aplicacionIntegrante.Inasistencia = integrante.Inasistencia;
                    aplicacionIntegrante.PrimariaIncompleta = integrante.PrimariaCompleta;
                    aplicacionIntegrante.SecundariaIncompleta = integrante.SecundariaCompleta;
                    aplicacionIntegrante.Educativa = integrante.Educativa;
                    aplicacionIntegrante.ServicioSalud = integrante.ServiciosSalud;
                    aplicacionIntegrante.SeguridadSocial = integrante.SeguridadSocial;
                    aplicacionIntegrante.Vivienda = familia.Vivienda;
                    aplicacionIntegrante.Piso = familia.Piso;
                    aplicacionIntegrante.Techo = familia.Techo;
                    aplicacionIntegrante.Muro = familia.Muro;
                    aplicacionIntegrante.Hacinamiento = familia.Hacinamiento;
                    aplicacionIntegrante.Servicios = familia.Servicios;
                    aplicacionIntegrante.Agua = familia.Agua;
                    aplicacionIntegrante.Drenaje = familia.Drenaje;
                    aplicacionIntegrante.Electricidad = familia.Electricidad;
                    aplicacionIntegrante.Combustible = familia.Combustible;
                    aplicacionIntegrante.Alimentaria = familia.Alimentaria;
                    aplicacionIntegrante.Ingreso = familia.IngresoFamiliar;
                    aplicacionIntegrante.LineaBienestar = familia.LineaBienestar;
                    aplicacionIntegrante.NivelPobreza = familia.NivelPobreza;
                    aplicacionIntegrante.VersionAlgoritmo = version.Version;
                    aplicacionIntegrante.GradoAlimentaria = familia.GradoAlimnetaria;
                    aplicacionIntegrante.Pea = integrante.EconomicamenteActiva;
                    aplicacionIntegrante.TieneActa = integrante.TieneActa;
                    aplicacionIntegrante.ParentescoId = integrante.ParentescoId;
                    aplicacionIntegrante.SexoId = integrante.SexoId;
                    aplicacionIntegrante.Discapacidad = integrante.Discapacidad;
                    aplicacionIntegrante.GradoEducativa = integrante.GradoEducativa;
                    aplicacionIntegrante.NumIntegrante = integrante.NumIntegrante;
                    aplicacionIntegrante.Edad = integrante.Edad;
                    aplicacionIntegrante.UpdatedAt = now;
                    
                    aplicacionIntegrante.BeneficiarioId = integrante.NumIntegrante==1?jefeFamilia.Id:integrantes[integrante.NumIntegrante].Id;
                    if (aplicacionIntegrante.Educativa){
                        numEducativas++;
                        numCarencias++;
                    }
                    if (aplicacionIntegrante.ServicioSalud){
                        numSalud++;
                        numCarencias++;
                    }
                    if (aplicacionIntegrante.SeguridadSocial){
                        numSocial++;
                        numCarencias++;
                    }
                    if (nuevoIntegrante)
                    {
                        _context.AplicacionCarencia.Add(aplicacionIntegrante);
                    }
                    else
                    {
                        _context.AplicacionCarencia.Update(aplicacionIntegrante);
                    }
                }
                if (aplicacion.Alimentaria){
                    numCarencias++;
                }
                if (aplicacion.Vivienda){
                    numCarencias++;
                }
                if (aplicacion.Servicios){
                    numCarencias++;
                }
                aplicacion.NumeroEducativas = numEducativas;
                aplicacion.NumeroSalud = numSalud;
                aplicacion.NumeroSocial = numSocial;
                aplicacion.NumeroCarencias = numCarencias;
                _context.Aplicacion.Update(aplicacion);
                _context.SaveChanges();
            }
            else
            {
                var nuevoResultado = false;
                var resultado = _context.AlgoritmoResultado.FirstOrDefault(ar => ar.BeneficiarioId == aplicacion.BeneficiarioId && ar.VersionId.Equals(version.Id));
                var resultadoIntegrantes = new Dictionary<int, AlgoritmoIntegranteResultado>();
                if (resultado == null)
                {
                    resultado = new AlgoritmoResultado
                    {
                        Id = Guid.NewGuid().ToString(),
                        CreatedAt = now,
                        VersionId = version.Id,
                        BeneficiarioId = aplicacion.BeneficiarioId
                    };
                    nuevoResultado = true;
                }
                else
                {
                    resultadoIntegrantes = _context.AlgoritmoIntegranteResultado.Where(air => air.ResultadoId.Equals(resultado.Id)).ToDictionary(air=>air.NumIntegrante);
                }
                
                resultado.Analfabetismo = familia.Analfabetismo;
                resultado.Inasistencia = familia.Inasistencia;
                resultado.PrimariaIncompleta = familia.PrimariaCompleta;
                resultado.SecundariaIncompleta = familia.SecundariaCompleta;
                resultado.Educativa = familia.Educativa;
                resultado.ServicioSalud = familia.ServiciosSalud;
                resultado.SeguridadSocial = familia.SeguridadSocial;
                resultado.Vivienda = familia.Vivienda;
                resultado.Piso = familia.Piso;
                resultado.Techo = familia.Techo;
                resultado.Muro = familia.Muro;
                resultado.Hacinamiento = familia.Hacinamiento;
                resultado.Servicios = familia.Servicios;
                resultado.Agua = familia.Agua;
                resultado.Drenaje = familia.Drenaje;
                resultado.Electricidad = familia.Electricidad;
                resultado.Combustible = familia.Combustible;
                resultado.Alimentaria = familia.Alimentaria;
                resultado.Ingreso = familia.IngresoFamiliar;
                resultado.Pea = familia.EconomicamenteActiva;
                resultado.LineaBienestar = familia.LineaBienestar;
                resultado.NivelPobreza = familia.NivelPobreza;
                resultado.GradoAlimentaria = familia.GradoAlimnetaria;
                resultado.TieneActa = familia.TieneActa;
                resultado.SeguridadPublica = familia.SeguridadPublica;
                resultado.SeguridadParque = familia.SeguridadParque;
                resultado.ConfianzaLiderez = familia.ConfianzaLiderez;
                resultado.ConfianzaInstituciones = familia.ConfianzaInstituciones;
                resultado.Movilidad = familia.Movilidad;
                resultado.RedesSociales = familia.RedesSociales;
                resultado.Tejido = familia.Tejido;
                resultado.Satisfaccion = familia.Satisfaccion;
                resultado.PropiedadVivienda = familia.PropiedadVivienda;
       
                if (nuevoResultado)
                {
                    _context.AlgoritmoResultado.Add(resultado);
                }
                else
                {
                    _context.AlgoritmoResultado.Update(resultado);
                }
                
                foreach (var integrante in familia.Integrantes)
                {
                    var integranteResultado = new AlgoritmoIntegranteResultado();
                    var nuevoIntegrante = true;
                    if (resultadoIntegrantes.ContainsKey(integrante.NumIntegrante))
                    {
                        nuevoIntegrante = false;
                        integranteResultado = resultadoIntegrantes[integrante.NumIntegrante];
                    }
                    else
                    {
                        integranteResultado.Id = Guid.NewGuid().ToString();
                        integranteResultado.CreatedAt = now;
                        integranteResultado.ResultadoId = resultado.Id;
                    }

                    integranteResultado.Analfabetismo = integrante.Analfabetismo;
                    integranteResultado.Inasistencia = integrante.Inasistencia;
                    integranteResultado.PrimariaIncompleta = integrante.PrimariaCompleta;
                    integranteResultado.SecundariaIncompleta = integrante.SecundariaCompleta;
                    integranteResultado.Educativa = integrante.Educativa;
                    integranteResultado.ServicioSalud = integrante.ServiciosSalud;
                    integranteResultado.SeguridadSocial = integrante.SeguridadSocial;
                    integranteResultado.Vivienda = familia.Vivienda;
                    integranteResultado.Piso = familia.Piso;
                    integranteResultado.Techo = familia.Techo;
                    integranteResultado.Muro = familia.Muro;
                    integranteResultado.Hacinamiento = familia.Hacinamiento;
                    integranteResultado.Servicios = familia.Servicios;
                    integranteResultado.Agua = familia.Agua;
                    integranteResultado.Drenaje = familia.Drenaje;
                    integranteResultado.Electricidad = familia.Electricidad;
                    integranteResultado.Combustible = familia.Combustible;
                    integranteResultado.Alimentaria = familia.Alimentaria;
                    integranteResultado.Ingreso = familia.IngresoFamiliar;
                    integranteResultado.LineaBienestar = familia.LineaBienestar;
                    integranteResultado.NivelPobreza = familia.NivelPobreza;
                    integranteResultado.GradoAlimentaria = familia.GradoAlimnetaria;
                    integranteResultado.Pea = integrante.EconomicamenteActiva;
                    integranteResultado.TieneActa = integrante.TieneActa;
                    integranteResultado.ParentescoId = integrante.ParentescoId;
                    integranteResultado.SexoId = integrante.SexoId;
                    integranteResultado.Discapacidad = integrante.Discapacidad.ToString();
                    integranteResultado.GradoEducativa = integrante.GradoEducativa;
                    integranteResultado.NumIntegrante = integrante.NumIntegrante;
                    integranteResultado.Edad = integrante.Edad;
                    integranteResultado.UpdatedAt = now;
                    if (nuevoIntegrante)
                    {
                        _context.AlgoritmoIntegranteResultado.Add(integranteResultado);
                    }
                    else
                    {
                        _context.AlgoritmoIntegranteResultado.Update(integranteResultado);
                    }
                }
                _context.SaveChanges();
            }
        }

        public void CalcularCarenciasCOVID(Aplicacion aplicacion, List<AplicacionCarencia> integrantes, ApplicationDbContext _context)
        {
            var respuestaPreocupacion = _context.AplicacionPregunta.FirstOrDefault(ap => ap.DeletedAt == null
                                                                                         && ap.AplicacionId == aplicacion.Id &&
                                                                                         ap.PreguntaId == "1042");
            var respuestasPerdidaEmpleo = _context.AplicacionPregunta.Where(ap => ap.DeletedAt == null
                                                                                         && ap.AplicacionId == aplicacion.Id &&
                                                                                            ap.PreguntaId == "1066").ToList();
            if (respuestaPreocupacion != null)
            {
                aplicacion.PreocupacionCovid = int.Parse(respuestaPreocupacion.Valor);
            }
            
            foreach (var respuesta in respuestasPerdidaEmpleo)
            {
                integrantes[respuesta.RespuestaIteracion].PerdidaEmpleo = true;
                aplicacion.PerdidaEmpleo = true;
            }
        }
    }
}