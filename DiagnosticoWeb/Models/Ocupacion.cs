using Microsoft.AspNetCore.Http;
using DiagnosticoWeb.Validaciones;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiagnosticoWeb.Models
{
    /// <summary>
    /// Clases que mapean la tabla de ocupaciones a objetos de persistencia, tabla que guarda las posibles ocupaciones de los beneficiarios
    /// </summary>
    [Table("Ocupaciones")]
    public class Ocupacion
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }

    public class OcupacionResponse
    {
        public int Total { get; set; }
        public List<Ocupacion> Ocupaciones { get; set; }
    }

    public class OcupacionArchivo
    {
        [Required(ErrorMessage = "Es necesario seleccionar un archivo.")]
        [FileExtension(ErrorMessage = "El archivo a importar debe tener la extensión .xlsx")]
        public IFormFile File { get; set; }
    }

    public class OcupacionRequest
    {
        public string Usuario { get; set; }
        public string ImportedAt { get; set; }
        public string Nombre { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }

    public class OcupacionApiModel
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public string DeletedAt { get; set; }
    }

    public class OcupacionCreateEditModel
    {
        [Required(ErrorMessage = "El Id de la ocupación es obligatorio.")]
        [RegularExpression("([0-9]*)", ErrorMessage = "El Id debe ser numérico")]
        [MaxLength(10, ErrorMessage = "El Id de la ocupación debe tener como máximo 10 caracteres.")]
        public string Id { get; set; }
        public string IdAnterior { get; set; }

        [Required(ErrorMessage = "El nombre de la ocupación es obligatorio.")]
        [MaxLength(255, ErrorMessage = "El nombre de la ocupación debe tener como máximo 255 caracteres.")]
        public string Nombre { get; set; }
    }
}