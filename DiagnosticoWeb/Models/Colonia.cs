using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using DiagnosticoWeb.Validaciones;

namespace DiagnosticoWeb.Models
{
    [Table("Colonias")]
    public class Colonia
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string CodigoPostal { get; set; }
        public string MunicipioId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        //relations
        public virtual Municipio Municipio{ get; set; }
        public virtual List<Domicilio> Domicilios { get; set; }
    }
    
    public class ColoniaApiModel
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string CodigoPostal { get; set; }
        public string MunicipioId { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public string DeletedAt { get; set; }
    }
    
    public class ColoniaRequest
    {
        public string CodigoPostal { get; set; }
        public string Nombre { get; set; }
        public string MunicipioId { get; set; }
        public string Usuario { get; set; }
        public string ImportedAt { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
    
    public class ColoniaResponse
    {
        public int Total{ get; set; }
        public List<Colonia> Colonias { get; set; }
    }
    
    public class ImportarColoniaRequest
    {
        [FileExtension(ErrorMessage = "El archivo a importar debe tener la extensión .xlsx")]
        public IFormFile Colonias { get; set; }
    }
    public class ColoniaCreateEditModel
    {
    [Required(ErrorMessage = "El Id de la colonia es obligatorio.")]
    [RegularExpression("([0-9]*)", ErrorMessage = "El Id debe ser numérico")]
    [MaxLength(10, ErrorMessage = "El Id de la colonia debe tener como máximo 10 caracteres.")]
    public string Id { get; set; }
    public string IdAnterior { get; set; }

    [Required(ErrorMessage = "El nombre de la colonia es obligatorio.")]
    [MaxLength(255, ErrorMessage = "El nombre de la colonia debe tener como máximo 255 caracteres.")]
    public string Nombre { get; set; }
        
    [Required(ErrorMessage = "El codigo postal de la colonia es obligatorio.")]
    [MaxLength(5, ErrorMessage = "El codigo postal de la colonia debe tener como máximo 5 caracteres.")]
    public string CodigoPostal { get; set; }
    [Required(ErrorMessage = "El municipio de la colonia es obligatorio.")]
    public string MunicipioId { get; set; }
    public List<Municipio> Municipios{ get; set; }
    }
}