using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiagnosticoWeb.Models
{
    [Table("Manzanas")]
    public class Manzana
    {
        public string Id { get; set; }    
        public string Nombre { get; set; }    
        public string LocalidadId { get; set; }    
        public string MunicipioId { get; set; }
        public string AgebId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        
        public virtual Municipio Municipio { get; set; }
        public virtual Localidad Localidad { get; set; }
        public virtual Ageb Ageb { get; set; }
    } 
    
    public class ManzanaApiModel
    {
        public string Id { get; set; }    
        public string Nombre { get; set; }    
        public string LocalidadId { get; set; }    
        public string MunicipioId { get; set; }
        public string AgebId { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public string DeletedAt { get; set; }
    }
    
    public class ManzanaRequest
    {
        public string Nombre { get; set; } 
        public string MunicipioId { get; set; } 
        public string LocalidadId { get; set; } 
        public string AgebId { get; set; } 
        public string Usuario { get; set; } 
        public string ImportedAt { get; set; } 
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
    
    public class ManzanaResponse
    {
        public List<Manzana> Manzanas { get; set; }
        public int Total { get; set; }
    }
    
    public class ManzanaCreateEditModel
    {
        [Required(ErrorMessage = "El Id de la manzana es obligatorio.")]
        [RegularExpression("([0-9]*)", ErrorMessage = "El Id debe ser numérico")]
        [MaxLength(16, ErrorMessage = "El Id de la manzana debe tener como máximo 16 caracteres.")]
        public string Id { get; set; }
        [Required(ErrorMessage = "El nombre de la manzana es obligatorio.")]
        [RegularExpression("([0-9]*)", ErrorMessage = "El nombre debe ser numérico")]
        [MaxLength(255, ErrorMessage = "El nombre de la mnzana debe tener como máximo 255 caracteres.")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El municipio es obligatorio.")]
        public string MunicipioId { get; set; }
        [Required(ErrorMessage = "La localidad es obligatoria.")]
        public string LocalidadId { get; set; }
        [Required(ErrorMessage = "La Ageb es obligatorio.")]
        public string AgebId { get; set; }
        public string IdAnterior { get; set; }
    }
}