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
    /// Clases que mapean la tabla de Dispacaidades a objetos de persistencia, tabla que guarda las discapacidades que pueden tener los beneficiarios o sus hijos
    /// </summary>
    [Table("Discapacidades")]
    public class Discapacidad
    {
        public string Id { get; set; }
        public string Nombre  { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }

    public class DiscapacidadApiModel
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public string DeletedAt { get; set; }
    }
    
    public class DiscapacidadArchivo
    {
        [Required(ErrorMessage = "Es necesario seleccionar un archivo.")]
        [FileExtension(ErrorMessage = "El archivo a importar debe tener la extensión .xlsx")]
        public IFormFile File { get; set; }
    }
    
    public class DiscapacidadResponse
    {
        public int Total { get; set; }
        public List<Discapacidad> Discapacidades { get; set; }
    }
    
    public class DiscapacidadRequest
    {
        public string Usuario { get; set; }
        public string ImportedAt { get; set; }
        public string Nombre { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
    
    public class DiscapacidadCreateEditModel
    {
        [Required(ErrorMessage = "El Id de la discapacidad es obligatorio.")]
        [RegularExpression("([0-9]*)", ErrorMessage = "El Id debe ser numérico")]
        [MaxLength(10, ErrorMessage = "El Id de la discapacidad debe tener como máximo 10 caracteres.")]
        public string Id { get; set; }
        public string IdAnterior { get; set; }

        [Required(ErrorMessage = "El nombre de la discapacidad es obligatorio.")]
        [MaxLength(255, ErrorMessage = "El nombre de la discapacidad debe tener como máximo 255 caracteres.")]
        public string Nombre { get; set; }
        public List<string> GradosId { get; set; }
        public List<Grado> Grados { get; set; }
    }

    public class DiscapacidadIntegrante
    {
        public bool Caminar { get; set; }
        public bool Ver { get; set; }
        public bool Hablar { get; set; }
        public bool Oir { get; set; }
        public bool Vestirse { get; set; }
        public bool Atencion { get; set; }
        public bool Mental { get; set; }
        public int NumIntegrante { get; set; }
    }
}
