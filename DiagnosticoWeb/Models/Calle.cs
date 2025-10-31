using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using DiagnosticoWeb.Validaciones;

namespace DiagnosticoWeb.Models
{
    [Table("Calles")]
    public class Calle
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string MunicipioId { get; set; }
        public string LocalidadId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        //Relations
        public virtual Municipio Municipio { get; set; }
        public virtual Localidad Localidad { get; set; }
        public virtual List<Domicilio> Domicilios { get; set; }

    }
    
    public class CalleApiModel
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string MunicipioId { get; set; }
        public string LocalidadId { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public string DeletedAt { get; set; }
    }
    
    public class CalleRequest
    {
        public string Nombre { get; set; }
        public string MunicipioId { get; set; }
        public string LocalidadId { get; set; }
        public string Usuario { get; set; }
        public string ImportedAt { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
    
    public class CalleResponse
    {
        public int Total{ get; set; }
        public List<Calle> Calles { get; set; }
    }
    
    public class ImportarCalleRequest
    {
        [FileExtension(ErrorMessage = "El archivo a importar debe tener la extensión .xlsx")]
        public IFormFile Calles { get; set; }
    }
    public class CalleCreateEditModel
    {
        [Required(ErrorMessage = "El Id de la calle es obligatorio.")]
        [RegularExpression("([0-9]*)", ErrorMessage = "El Id debe ser numérico")]
        [MaxLength(10, ErrorMessage = "El Id de la calle debe tener como máximo 10 caracteres.")]
        public string Id { get; set; }
        public string IdAnterior { get; set; }

        [Required(ErrorMessage = "El nombre de la calle es obligatorio.")]
        [MaxLength(255, ErrorMessage = "El nombre de la calle debe tener como máximo 255 caracteres.")]
        public string Nombre { get; set; }
        
        [Required(ErrorMessage = "El municipio de la calle es obligatorio.")]
        public string MunicipioId { get; set; }
        
        [Required(ErrorMessage = "La localidad de la calle es obligatorio.")]
        public string LocalidadId { get; set; }
        public List<Municipio> Municipios { get; set; }
        public List<Localidad> Localidades { get; set; }
    }
}