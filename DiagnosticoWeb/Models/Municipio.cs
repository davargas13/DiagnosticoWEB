using Microsoft.AspNetCore.Http;
using DiagnosticoWeb.Validaciones;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiagnosticoWeb.Models
{
    /// <summary>
    /// Clases que mapean la tabla de municipios a objetos de persistencia, tabla que guarda los 46 municipios del estado
    /// </summary>
    [Table("Municipios")]
    public class Municipio
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string Clave { get; set; }
        public double Indice { get; set; }
        public string EstadoId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual List<Localidad> Localidades { get; set; }
        public virtual Estado Estado { get; set; }
        public virtual List<MunicipioZona> MunicipioZonas { get; set; }
        [NotMapped]
        public string ZonaId { get; set; }
    }

    public class MunicipioApiModel
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public double Indice { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public string DeletedAt { get; set; }
    }

    public class MunicipioCreateEditModel
    {
        [Required(ErrorMessage = "El Id del municipio es obligatorio.")]
        [RegularExpression("([0-9]*)", ErrorMessage = "El Id debe ser numérico")]
        [MaxLength(10, ErrorMessage = "El Id del estado debe tener como máximo 10 caracteres.")]
        public string Id { get; set; }

        [Required(ErrorMessage = "La clave del municipio es obligatorio.")]
        [RegularExpression("([0-9]*)", ErrorMessage = "La clave debe ser numérica")]
        [MaxLength(10, ErrorMessage = "La clave del municipio debe tener como máximo 10 caracteres.")]
        public string Clave { get; set; }

        [Required(ErrorMessage = "El nombre del municipio es obligatorio.")]
        [MaxLength(255, ErrorMessage = "El nombre del municipio debe tener como máximo 255 caracteres.")]
        public string Nombre { get; set; }
        public string EstadoId { get; set; }        
        public string IdAnterior { get; set; }
    }

    public class MunicipioArchivo
    {
        [Required(ErrorMessage = "Es necesario seleccionar un archivo.")]
        [FileExtension(ErrorMessage = "El archivo a importar debe tener la extensión .xlsx")]
        public IFormFile File { get; set; }
    }

    public class MunicipioRequest
    {
        public string Nombre { get; set; } 
        public string Usuario { get; set; } 
        public string ImportedAt { get; set; } 
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }

    public class MunicipioResponse
    {
        public List<MunicipioListModel> Municipios { get; set; }
        public int Total { get; set; }
    }

    public class MunicipioListModel
    {
        public string Id { get; set; }
        public string Municipio { get; set; }
        public string Localidades { get; set; }
    }
}