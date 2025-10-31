using System.Collections.Generic;
using System.Linq;
using DiagnosticoWeb.Models;

namespace DiagnosticoWeb.Code
{
    public class EncuestaCode
    {
        public static Dictionary<string, string> ObtenerPreguntasParaSabana(List<Pregunta> preguntas)
        {
            var preguntasMap = new Dictionary<string, string>();
            foreach (var pregunta in preguntas) 
            {
                if (pregunta.Id.Equals("-1")) { continue; } 
                pregunta.RespuestasMap = new Dictionary<string, Respuesta>(); 
                if ((pregunta.Gradual||pregunta.TipoPregunta.Equals(TipoPregunta.Check.ToString())) && pregunta.Respuestas.Any()) { 
                    pregunta.RespuestasMap = pregunta.Respuestas.OrderBy(r =>r.Numero).ToDictionary(r=>r.Id); 
                    var numRespuesta = 1; 
                    foreach (var respuesta in pregunta.RespuestasMap.Values) {
                        preguntasMap.Add("var"+pregunta.Id+"_"+numRespuesta, "nvarchar(50)");
                        if (pregunta.Gradual) { 
                            if (!respuesta.Negativa) 
                            { 
                                preguntasMap.Add("vartx1_"+pregunta.Id+"_"+numRespuesta, "nvarchar(200)");
                                if (respuesta.Complemento != null) {
                                    preguntasMap.Add("vartx2_"+pregunta.Id+"_"+numRespuesta, "nvarchar(200)");
                                }
                            }
                        }
                        else
                        {
                            if (respuesta.Complemento != null) {
                                preguntasMap.Add("vartx1_"+pregunta.Id+"_"+numRespuesta, "nvarchar(200)");
                            }
                        }

                        numRespuesta++;
                    }
                } else {
                    pregunta.RespuestasMap = pregunta.Respuestas.OrderBy(r =>r.Numero).ToDictionary(r=>r.Id);
                    preguntasMap.Add("var"+pregunta.Id+"_1", "nvarchar(200)");
                    if (!pregunta.TipoPregunta.Equals(TipoPregunta.Radio.ToString())) continue;
                    var numRespuesta = 1;
                    foreach (var respuesta in pregunta.RespuestasMap.Values.Where(r => r.Complemento != null).ToList())
                    {
                        preguntasMap.Add("vartx1_"+pregunta.Id+"_"+numRespuesta, "nvarchar(200)");
                        numRespuesta++;
                    }
                }
            } 
            return preguntasMap;
        }
    }
}