using Microsoft.AspNetCore.Http;
using DiagnosticoWeb.Validaciones;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiagnosticoWeb.Models
{
    /// <summary>
    /// Clases que mapean la tabla de estados civiles a objetos de persistencia, tabla que guarda los posibles estados civiles de los beneficiarios
    /// </summary>
    [Table("EstadosCiviles")]
    public class EstadoCivil
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
    
    public class EstadoCivilResponse
    {
        public int Total { get; set; }
        public List<EstadoCivil> EstadosCiviles { get; set; }
    }

    public class EstadoCivilArchivo
    {
        [Required(ErrorMessage = "Es necesario seleccionar un archivo.")]
        [FileExtension(ErrorMessage = "El archivo a importar debe tener la extensión .xlsx")]
        public IFormFile File { get; set; }
    }

    public class EstadoCivilRequest
    {
        public string Usuario { get; set; }
        public string ImportedAt { get; set; }
        public string Nombre { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }

    public class EstadoCivilApiModel
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public string DeletedAt { get; set; }
    }
    
    public class EstadoCivilCreateEditModel
    {
        [Required(ErrorMessage = "El Id del estado civil es obligatorio.")]
        [RegularExpression("([0-9]*)", ErrorMessage = "El Id debe ser numérico")]
        [MaxLength(10, ErrorMessage = "El Id del estado civil debe tener como máximo 10 caracteres.")]
        public string Id { get; set; }
        public string IdAnterior { get; set; }

        [Required(ErrorMessage = "El nombre del estado civil es obligatorio.")]
        [MaxLength(255, ErrorMessage = "El nombre del estado civil debe tener como máximo 255 caracteres.")]
        public string Nombre { get; set; }
    }
}