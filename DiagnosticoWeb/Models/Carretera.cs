using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiagnosticoWeb.Models
{
    [Table("Carreteras")]
    public class Carretera
    {
        public string Id { get; set; }    
        public string Nombre { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }

    public class CarreteraApiModel
    {
        public string Id { get; set; }    
        public string Nombre { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public string DeletedAt { get; set; }
    }
    
    public class CarreteraRequest
    {
        public string Nombre { get; set; }
        public string Usuario { get; set; } 
        public string ImportedAt { get; set; } 
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
    
    public class CarreteraResponse
    {
        public List<Carretera> Carreteras { get; set; }
        public int Total { get; set; }
    }
    
    public class CarreteraCreateEditModel
    {
        [Required(ErrorMessage = "El Id del tipo de carretera es obligatorio.")]
        [RegularExpression("([0-9]*)", ErrorMessage = "El Id debe ser numérico")]
        [MaxLength(10, ErrorMessage = "El Id del tipo de carretera debe tener como máximo 10 caracteres.")]
        public string Id { get; set; }
        [Required(ErrorMessage = "El nombre del tipo de carretera es obligatorio.")]
        [MaxLength(255, ErrorMessage = "El nombre del tipo de carretera debe tener como máximo 255 caracteres.")]
        public string Nombre { get; set; }
        public string IdAnterior { get; set; }
    }
}