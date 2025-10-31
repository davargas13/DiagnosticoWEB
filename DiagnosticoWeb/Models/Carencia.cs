using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace DiagnosticoWeb.Models
{
    /// <summary>
    /// Clases que mapean la tabla de carencias a objetos de persistencia, tabla que guarda las 6 carencias definidas por CONEVAL
    /// </summary>
    [Table("Carencias")]
    public class Carencia
    {
        public string Id { get; set; }
        public string Clave { get; set; }
        public string Nombre { get; set; }
        public string Color { get; set; }
        public string PadreId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        
        public virtual Carencia Padre { get; set; }
        [NotMapped]
        public bool Carente { get; set; }
        [NotMapped]
        public int Numero { get; set; }
    }
    
    public class CarenciaApiModel
    {
        public string Id { get; set; }
        public string Clave { get; set; }
        public string Nombre { get; set; }
        public string Color { get; set; }
        public string PadreId { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public string DeletedAt { get; set; }
    }
    
    public class CategoriaRequest
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
    
    public class CategoriaResponse
    {
        public int Total { get; set; }
        public List<Carencia> Carencias { get; set; }
    }

    public class CarenciaModel
    {
        [Required(ErrorMessage = "El Id de la categoría es obligatorio.")]
        [RegularExpression("([0-9]*)", ErrorMessage = "El Id debe ser numérico")]
        [MaxLength(10, ErrorMessage = "El Id de la categoría debe tener como máximo 10 caracteres.")]
        public string Id { get; set; }        
        public string IdAnterior { get; set; }

        [Required(ErrorMessage = "La clave de la categoría es obligatorio.")]
        [MaxLength(20, ErrorMessage = "La clave de la categoría debe tener como máximo 20 caracteres.")] 
        public string Clave { get; set; }
        
        [Required(ErrorMessage = "El nombre de la categoría es obligatorio.")]
        [MaxLength(250, ErrorMessage = "El nombre de la categoría debe tener como máximo 250 caracteres.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El color de la categoría es obligatorio.")]
        public string Color { get; set; }
        public string PadreId { get; set; }
        public List<Carencia> Categorias { get; set; }
    }
} 