using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace DiagnosticoWeb.Models
{
    /// <summary>
    /// Clase con los grados de discapacidad que puede tener una persona
    /// </summary>
    [Table("DiscapacidadGrado")]
    public class DiscapacidadGrado
    {
        public string Id { get; set; }
        public string DiscapacidadId { get; set; }
        public string GradoId { get; set; }
        public string Grado { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        [ForeignKey("GradoId")]
        public virtual Grado Grade { get; set; }
    }
    
    public class DiscapacidadGradoApiModel
    {
        public string Id { get; set; }
        public string DiscapacidadId { get; set; }
        public string GradoId { get; set; }
        public string Grado { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public string DeletedAt { get; set; }
    }

    public class GradosModel
    {
        public string DiscapacidadGradoId { get; set; }
        public string GradoId { get; set; }
        public string Nombre { get; set; }
    }
} 