using Microsoft.AspNetCore.Http;
using DiagnosticoWeb.Validaciones;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiagnosticoWeb.Models
{
    /// <summary>
    /// Clases que mapean la tabla de estados a objetos de persistencia, tabla que guarda los 32 estados del pais
    /// </summary>
    [Table("Estados")]
    public class Estado
    {
        public string Id { get; set; }
        public string Clave { get; set; }
        public string Abreviacion { get; set; }
        public string Nombre { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }

    public class EstadoApiModel
    {
        public string Id { get; set; }
        public string Clave { get; set; }
        public string Abreviacion { get; set; }
        public string Nombre { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public string DeletedAt { get; set; }
    }

    public class EstadoArchivo
    {
        [Required(ErrorMessage = "Es necesario seleccionar un archivo.")]
        [FileExtension(ErrorMessage = "El archivo a importar debe tener la extensión .xlsx")]
        public IFormFile File { get; set; }
    }

    public class EstadoRequest
    {
        public string Clave { get; set; }
        public string Nombre { get; set; }
        public string Usuario { get; set; }
        public string ImportedAt { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }

    public class ExportarRequest {
        public string Clave { get; set; }
        public string Nombre { get; set; }
    }
    
    public class EstadoResponse
    {
        public List<Estado> Estados { get; set; }
        public int Total{ get; set; }
    }

    public class EstadoModel
    {
        [Required(ErrorMessage = "El Id del estado es obligatorio.")]
        [RegularExpression("([0-9]*)", ErrorMessage = "El Id debe ser numérico")]
        [MaxLength(10, ErrorMessage = "El Id del estado debe tener como máximo 10 caracteres.")]
        public string Id { get; set; }
        [Required(ErrorMessage = "La clave del estado es obligatorio.")]
        [MaxLength(10, ErrorMessage = "La clave del estado debe tener como máximo 10 caracteres.")]
        public string Clave { get; set; }
        [Required(ErrorMessage = "La abreviación del estado es obligatorio.")]
        [MaxLength(2, ErrorMessage = "La abreviación del estado debe tener como máximo 2 caracteres.")]
        public string Abreviacion { get; set; }
        [Required(ErrorMessage = "El nombre del estado es obligatorio.")]
        [MaxLength(250, ErrorMessage = "El nombre del estado debe tener como máximo 250 caracteres.")]
        public string Nombre { get; set; }
        public string IdAnterior { get; set; }
    }
}