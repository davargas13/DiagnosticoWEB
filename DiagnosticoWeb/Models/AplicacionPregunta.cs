using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DiagnosticoWeb.Models
{
    /// <summary>
    /// Clases que representan a la tabla AplicacionPreguntas de la base datos, tabla donde se guardan las respuestas de un beneficiario en la encuesta CONEVAL
    /// </summary>
    [Table("AplicacionPreguntas")]
    public class AplicacionPregunta
    {
        public string Id { get; set; }
        public string Valor { get; set; }
        public string AplicacionId { get; set; }
        public string PreguntaId { get; set; }
        public string RespuestaId { get; set; }
        public double RespuestaValor { get; set; }
        public int RespuestaIteracion { get; set; }
        public string Grado { get; set; }
        public string Complemento { get; set; }
        public int? ValorNumerico { get; set; }
        public DateTime? ValorFecha { get; set; }
        public string ValorCatalogo { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public int NumIntegrante { get; set; }
        //Relaciones
        public virtual Aplicacion Aplicacion { get; set; }
        public virtual Pregunta Pregunta { get; set; }
        public virtual Respuesta Respuesta { get; set; }
    } 

    public class AplicacionPreguntaApiModel
    {
        public string Id { get; set; }
        public string Valor { get; set; }
        public string AplicacionId { get; set; }
        public string PreguntaId { get; set; }
        public string RespuestaId { get; set; }
        public double RespuestaValor { get; set; }
        public int RespuestaIteracion { get; set; }
        public string Grado { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public string DeletedAt { get; set; }
        public string Complemento { get; set; }
        public int? ValorNumerico { get; set; }
        public string ValorFecha { get; set; }
        public string ValorCatalogo { get; set; }
    }
}