using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiagnosticoWeb.Models
{
    [Table("Caminos")]
    public class Camino
    {
        public string Id { get; set; }    
        public string Nombre { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    } 
    
    public class CaminoApiModel
    {
        public string Id { get; set; }    
        public string Nombre { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public string DeletedAt { get; set; }
    }
    
    public class CaminoRequest
    {
        public string Nombre { get; set; }
        public string Usuario { get; set; } 
        public string ImportedAt { get; set; } 
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
    
    public class CaminoResponse
    {
        public List<Camino> Caminos { get; set; }
        public int Total { get; set; }
    }
    
    public class CaminoCreateEditModel
    {
        [Required(ErrorMessage = "El Id del camino es obligatorio.")]
        [RegularExpression("([0-9]*)", ErrorMessage = "El Id debe ser numérico")]
        [MaxLength(10, ErrorMessage = "El Id del camino debe tener como máximo 10 caracteres.")]
        public string Id { get; set; }
        [Required(ErrorMessage = "El nombre del camino es obligatorio.")]
        [MaxLength(255, ErrorMessage = "El nombre del camino debe tener como máximo 255 caracteres.")]
        public string Nombre { get; set; }
        public string IdAnterior { get; set; }
    }
}