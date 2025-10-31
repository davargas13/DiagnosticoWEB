using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiagnosticoWeb.Models
{
    [Table("Permiso")]
    public class Permiso
    {
        public string Id { get; set; }
        public string Clave { get; set; }
        public string Nombre { get; set; }
        public int Sistema { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
} 