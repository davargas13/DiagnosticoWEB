using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiagnosticoWeb.Models
{
    /// <summary>
    /// Clases que mapean la tabla de carencia de vertiente a objetos de persistencia, tabla que guarda las carencias que puede atender la vertiente solicitada en el apoyo
    /// </summary>
    [Table("VertienteCarencias")]
    public class VertienteCarencia
    {
        public string Id { get; set; }
        public string VertienteId { get; set; }
        public string CarenciaId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual Vertiente Vertiente { get; set; }
        public virtual Carencia Carencia { get; set; }
    }
    
    public class VertienteCarenciaApiModel
    {
        public string Id { get; set; }
        public string VertienteId { get; set; }
        public string CarenciaId { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public string DeletedAt { get; set; }
    }
}