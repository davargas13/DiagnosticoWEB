using DiagnosticoWeb.Validaciones;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiagnosticoWeb.Models
{
    /// <summary>
    /// Clases que mapean la tabla de programas sociales a objetos de persistencia, tabla que guarda los programas sociales que entregan apoyos de las dependencias
    /// </summary>
    [Table("ProgramasSociales")]
    public class ProgramaSocial
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string Proyecto { get; set; }
        public string DependenciaId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual Dependencia Dependencia { get; set; }
        public virtual List<Vertiente> Vertientes { get; set; }
    }

    public class ProgramaSocialModel
    {
        public string Id { get; set; }
        
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [MaxLength(250, ErrorMessage = "El nombre debe tener como máximo 250 caracteres.")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El proyecto de inversión es obligatorio.")]
        [MaxLength(250, ErrorMessage = "El proyecto de inversión debe tener como máximo 250 caracteres.")]
        public string Proyecto { get; set; }
        
        [Required(ErrorMessage = "La dependencia es obligatoria.")]
        public string DependenciaId { get; set; } 

        [EnsureOneElement(ErrorMessage = "Se debe agregar al menos una vertiente.")]
        public List<VertienteCreateEditModel> Vertientes { get; set; }
        public List<Carencia> Carencias { get; set; }
    }
    
    public class ProgramaSocialResponse
    {
        public List<ListadoProgramaSocial> ProgramasSociales { get; set; }
        public int Total { get; set; }
    }

    public class ListadoProgramaSocial
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public Dependencia Dependencia { get; set; }
        public string Vertientes { get; set; }
    }

    public class ProgramaSocialRequest
    {
        public string Nombre { get; set; }
        public string DependenciaId { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }        
    }
    
    public class ProgramaSocialApiModel
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string DependenciaId { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public string DeletedAt { get; set; }
    }
}