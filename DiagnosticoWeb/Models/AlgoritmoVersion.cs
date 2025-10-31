using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiagnosticoWeb.Models
{
    [Table("AlgoritmoVersiones")]
    public class AlgoritmoVersion
    {
        public string Id { get; set; }
        public int Version { get; set; }
        public DateTime? FechaAutorizacion { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UsuarioId { get; set; }
        public string AutorizadorId { get; set; }
        //Relations
        public virtual ApplicationUser Usuario { get; set; }
        public virtual ApplicationUser Autorizador { get; set; }
    }
    
    public class AlgoritmoVersionModel
    {
        public string Id { get; set; }
        public int Version { get; set; }
        public string FechaAutorizacion { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public string Usuario { get; set; }
        public string Autorizador { get; set; }
        public bool Selected { get; set; }
        public bool Autorizable { get; set; }
    }
}
