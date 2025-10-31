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
    [Table("CausaDiscapacidad")]
    public class CausaDiscapacidad
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }        
    }
    
    public class CausaDiscapacidadApiModel
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public string DeletedAt { get; set; }
    }

    public class CausaDiscapacidadRequest
    {
        public string Usuario { get; set; }
        public string ImportedAt { get; set; }
        public string Nombre { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }

    public class CausaDiscapacidadResponse
    {
        public int Total { get; set; }
        public List<CausaDiscapacidad> CausasDiscapacidad { get; set; }
    }

    public class CausaDiscapacidadModel
    {
        [Required(ErrorMessage = "El Id de la causa de discapacidad es obligatorio.")]
        [RegularExpression("([0-9]*)", ErrorMessage = "El Id debe ser numérico")]
        [MaxLength(10, ErrorMessage = "El Id de la causa de discapacidad debe tener como máximo 10 caracteres.")]
        public string Id { get; set; }
        public string IdAnterior { get; set; }

        [Required(ErrorMessage = "El nombre de la causa de discapacidad es obligatorio.")]
        [MaxLength(255, ErrorMessage = "El nombre de la causa de discapacidad debe tener como máximo 255 caracteres.")]
        public string Nombre { get; set; }
    }
} 