using Microsoft.AspNetCore.Http;
using DiagnosticoWeb.Validaciones;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiagnosticoWeb.Models
{
    /// <summary>
    /// Clases que representan a la tabla AGEB de la base de datos
    /// </summary>
    [Table("Agebs")]
    public class Ageb
    {
        public string Id { get; set; }
        public string Clave { get; set; }
        public string LocalidadId { get; set; }
        public string MunicipioId { get; set; }
        public string EstadoId { get; set; }
        
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        
        public virtual Localidad Localidad { get; set; }
        public virtual Municipio Municipio { get; set; }
        
        [NotMapped]
        public string Nombre { get; set; }

    }

    public class AgebApiModel
    {
        public string Id { get; set; }
        public string Clave { get; set; }
        public string LocalidadId { get; set; }
        public string MunicipioId { get; set; }
        public string EstadoId { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public string DeletedAt { get; set; }
    }

    public class AgebArchivo
    {
        [Required(ErrorMessage = "Es necesario seleccionar un archivo.")]
        [FileExtension(ErrorMessage = "El archivo a importar debe tener la extensión .xlsx")]
        public IFormFile File { get; set; }
    }

    public class AgebRequest
    {
        public string Usuario { get; set; }
        public string ImportedAt { get; set; }
        public string Clave { get; set; }
        public string LocalidadId { get; set; }
        public string MunicipioId { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
    
    public class AgebResponse
    {
        public List<Ageb> Agebs { get; set; }
        public int Total{ get; set; }
    }
    
    public class AgebCreateEditModel
    {
        [Required(ErrorMessage = "El Id del AGEB es obligatorio.")]
        [RegularExpression("([a-z|A-Z|0-9]*)", ErrorMessage = "El Id debe ser alfanumérico")]
        [MaxLength(16, ErrorMessage = "El Ide del AGEB debe tener como máximo 16 caracteres.")]
        public string Id { get; set; }
        public string IdAnterior { get; set; }

        [Required(ErrorMessage = "La clave del AGEB es obligatorio.")]
        [RegularExpression("([0-9]*[a-z|A-Z|]?)", ErrorMessage = "La clave debe ser alfanumérica")]
        [MaxLength(255, ErrorMessage = "La clave del AGEB debe tener como máximo 255 caracteres.")]
        public string Clave { get; set; }
        [Required(ErrorMessage = "EL municipio es obligatorio.")]
        public string MunicipioId { get; set; }
        [Required(ErrorMessage = "La localidad es obligatoria.")]
        public string LocalidadId { get; set; }
        public List<Municipio> Municipios{ get; set; }
        public List<Localidad> Localidades{ get; set; }
    }

    public class IdsActualizar
    {
        public string OldId { get; set; }
        public string NewId { get; set; }
    }
}