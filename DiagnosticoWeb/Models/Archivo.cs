using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiagnosticoWeb.Models
{
    /// <summary>
    /// Clases que mapean la tabla de archivos a objetos de persistencia, tabla que almacena las fotos de los beneficiarios y sus solicitudes de apoyos
    /// </summary>
    [Table("Archivos")]
    public class Archivo
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        [Column(TypeName = "text")]
        public string Base64 { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }

    public class ArchivoApiModel
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string Base64 { get; set; }
        public bool Enviado { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public string DeletedAt { get; set; }
    } 

    public class ArchivoRequest
    {
        public string Archivo { get; set; }
    }

    public class DescargaImagenApiModel
    {
        public string Id { get; set; }
        public string Archivos { get; set; }
    }
}