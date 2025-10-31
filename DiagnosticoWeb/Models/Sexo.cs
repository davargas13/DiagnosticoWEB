using DiagnosticoWeb.Code;
using DiagnosticoWeb.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using DiagnosticoWeb.Validaciones;

namespace DiagnosticoWeb.Models
{
    /// <summary>
    /// Clases que mapean la tabla de sexos a objetos de persistencia, tabla que guarda el sexo de los beneficiarios
    /// </summary>
    [Table("Sexos")]
    public class Sexo
    {
        public string Id { get; set; }
        public string Nombre  { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }

    public class SexoApiModel
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public string DeletedAt { get; set; }
    }
    
    public class SexoArchivo
    {
        [Required(ErrorMessage = "Es necesario seleccionar un archivo.")]
        [FileExtension(ErrorMessage = "El archivo a importar debe tener la extensión .xlsx")]
        public IFormFile File { get; set; }
    }
    
    public class SexoResponse
    {
        public int Total { get; set; }
        public List<Sexo> Sexos { get; set; }
    }
    
    public class SexoRequest
    {
        public string Usuario { get; set; }
        public string ImportedAt { get; set; }
        public string Nombre { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
    
    public class SexoCreateEditModel
    {
        [Required(ErrorMessage = "El Id del género es obligatorio.")]
        [RegularExpression("([0-9]*)", ErrorMessage = "El Id debe ser numérico")]
        [MaxLength(10, ErrorMessage = "El Id del género debe tener como máximo 10 caracteres.")]
        public string Id { get; set; }
        public string IdAnterior { get; set; }

        [Required(ErrorMessage = "El nombre del género es obligatorio.")]
        [MaxLength(255, ErrorMessage = "El nombre del género debe tener como máximo 255 caracteres.")]
        public string Nombre { get; set; }
    }
}
