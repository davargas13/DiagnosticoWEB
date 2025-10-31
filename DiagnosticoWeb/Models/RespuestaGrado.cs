using DiagnosticoWeb.Validaciones;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiagnosticoWeb.Models
{
    /// <summary>
    /// Clases que mapean la tabla de grados de respuesta a objetos de persistencia, tabla que guarda los grados que puede tener una respuesta
    /// </summary>
    [Table("RespuestaGrado")]
    public class RespuestaGrado
    {
        public string Id { get; set; }
        public string RespuestaId { get; set; }
        public string PreguntaGradoId { get; set; }
        public string Grado { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual Respuesta Respuesta { get; set; }
        public virtual PreguntaGrado PreguntaGrado { get; set; }
    }

    public class RespuestaGradoApiModel
    {
        public string Id { get; set; }
        public string RespuestaId { get; set; }
        public string PreguntaGradoId { get; set; }
        public int Grado { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public string DeletedAt { get; set; }
    }

    public class RespuestaGradoCreateModel
    {
        public string Id { get; set; }
        public string RespuestaId { get; set; }
        public string PreguntaGradoId { get; set; }
        [Required]
        public string Grado { get; set; }
    }
}