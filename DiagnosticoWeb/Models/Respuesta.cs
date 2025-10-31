using DiagnosticoWeb.Validaciones;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiagnosticoWeb.Models
{
    /// <summary>
    /// Clases que mapean la tabla de respuestas a objetos de persistencia, tabla que guarda las respuestas de las preguntas de la encuesta CONEVAL
    /// </summary>
    [Table("Respuestas")]
    public class Respuesta
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string PreguntaId { get; set; }
        public int Numero { get; set; }
        public bool Negativa { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string Complemento { get; set; }
        public string TipoComplemento { get; set; }

        public virtual Pregunta Pregunta { get; set; }
        public virtual List<RespuestaGrado> RespuestaGrados { get; set; }
    }

    public class RespuestasEditarModel
    {
        [Required]
        public int Id { get; set; }
        public int Numero { get; set; }
        [Required]
        public string Nombre { get; set; }
        public string TipoComplemento { get; set; }
        public string Complemento { get; set; }
        public bool Negativa { get; set; }
        public bool IsComplemento { get; set; }
        public bool Nueva { get; set; }
        public virtual List<RespuestaGradoCreateModel> RespuestaGrados { get; set; }

    }

    public class RespuestaApiModel
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string PreguntaId { get; set; }
        public int Numero { get; set; }
        public bool Negativa { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public string DeletedAt { get; set; }
        public string Complemento { get; set; }
        public string TipoComplemento { get; set; }
    }

    public class RespuestaEncuestaModel
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public bool Negativa { get; set; }
        public int Numero { get; set; }
        public string TipoComplemento { get; set; }
        public string Complemento { get; set; }
        public List<Model> Catalogo { get; set; }
        public Dictionary<string, RespuestaGrado> RespuestaGrados { get; set; }
    }
}