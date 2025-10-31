using Microsoft.AspNetCore.Http;
using DiagnosticoWeb.Validaciones;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiagnosticoWeb.Models
{
    /// <summary>
    /// Clases que mapean la tabla de Zonas de impulso a objetos de persistencia, tabla que guarda las zonas de impulso del estado
    /// </summary>
    [Table("ZonasImpulso")]
    public class ZonaImpulso
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string MunicipioId { get; set; }
        
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public virtual Municipio Municipio { get; set; }
        public virtual List<Domicilio> Domicilios { get; set; }
    }

    public class ZonaImpulsoApiModel
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string MunicipioId { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public string DeletedAt { get; set; }
    }

    public class ZonasImpulsoArchivo
    {
        [Required(ErrorMessage = "Es necesario seleccionar un archivo.")]
        [FileExtension(ErrorMessage = "El archivo a importar debe tener la extensión .xlsx")]
        public IFormFile File { get; set; }
    }

    public class ZonaImpulsoRequest
    {
        public string Usuario { get; set; }
        public string ImportedAt { get; set; }
        public string MunicipioId { get; set; }
        public string Nombre { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }

    public class ZonaResponse
    {
        public List<ZonaImpulso> Zonas { get; set; }
        public int Total { get; set; }
    }
    
    public class ZonaCreateEditModel
    {
        [Required(ErrorMessage = "El Id de la zona es obligatorio.")]
        [RegularExpression("([0-9]*)", ErrorMessage = "El Id debe ser numérico")]
        [MaxLength(10, ErrorMessage = "El Id de la zona debe tener como máximo 10 caracteres.")]
        public string Id { get; set; }
        public string IdAnterior { get; set; }

        [Required(ErrorMessage = "El nombre de la zona es obligatorio.")]
        [MaxLength(255, ErrorMessage = "El nombre de la zona debe tener como máximo 255 caracteres.")]
        public string Nombre { get; set; }
        public string MunicipioId { get; set; }
        public List<Municipio> Municipios{ get; set; }
    }
}