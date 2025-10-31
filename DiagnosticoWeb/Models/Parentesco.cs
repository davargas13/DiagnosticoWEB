using Microsoft.AspNetCore.Http;
using DiagnosticoWeb.Code;
using DiagnosticoWeb.Database;
using DiagnosticoWeb.Validaciones;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DiagnosticoWeb.Models
{
    /// <summary>
    /// Clases que mapean la tabla de parentescos a objetos de persistencia, tabla que guarda el parentesco que tienen los miembros del hogar con el beneficiario
    /// </summary>
    [Table("Parentescos")]
    public class Parentesco
    {
        public string Id { get; set; }
        public string Nombre  { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
    
    public class ParentescoResponse
    {
        public int Total { get; set; }
        public List<Parentesco> Parentescos { get; set; }
    }

    public class ParentescoArchivo
    {
        [Required(ErrorMessage = "Es necesario seleccionar un archivo.")]
        [FileExtension(ErrorMessage = "El archivo a importar debe tener la extensión .xlsx")]
        public IFormFile File { get; set; }
    }

    public class ParentescoRequest
    {
        public string Usuario { get; set; }
        public string ImportedAt { get; set; }
        public string Nombre { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }

    public class ParentescoApiModel
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public string DeletedAt { get; set; }
    }
    
    public class ParentescoCreateEditModel
    {
        [Required(ErrorMessage = "El Id del parentesco es obligatorio.")]
        [RegularExpression("([0-9]*)", ErrorMessage = "El Id debe ser numérico")]
        [MaxLength(10, ErrorMessage = "El Id del parentesco debe tener como máximo 10 caracteres.")]
        public string Id { get; set; }
        public string IdAnterior { get; set; }

        [Required(ErrorMessage = "El nombre del parentesco es obligatorio.")]
        [MaxLength(255, ErrorMessage = "El nombre del parentesco debe tener como máximo 255 caracteres.")]
        public string Nombre { get; set; }
    }
}
