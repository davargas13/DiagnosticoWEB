using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;

namespace DiagnosticoWeb.Models
{
    /// <summary>
    /// Clases que mapean la tabla de archivos de vertiente a objetos de persistencia, tabla que guarda los documentos que se deberan de adjuntar al solicitar apoyo de alguna vertiente
    /// </summary>
    [Table("VertientesArchivos")]
    public class VertienteArchivo
    {
        public string Id { get; set; }
        [Required(ErrorMessage = "El documento es requerido."), MaxLength(100,ErrorMessage = "El documento debe ser menor a 100 caracteres")]

        public string TipoArchivo { get; set; }
        public string VertienteId { get; set; }
        
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual Vertiente Vertiente { get; set; }
    }

    public class VertienteArchivoApiModel
    {
        public string Id { get; set; }
        public string TipoArchivo { get; set; }
        public string VertienteId { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public string DeletedAt { get; set; }
    }
}