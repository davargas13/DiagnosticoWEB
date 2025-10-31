using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiagnosticoWeb.Models
{
    /// <summary>
    /// Clases que mapean la tabla de regiones del usuario a objetos de persistencia, tabla que guarda las regiones que puede ver el usuario en sesion
    /// </summary>
    [Table("UsuarioRegion")]
    public class UsuarioRegion
    {
        public string Id { get; set; }
        public string UsuarioId { get; set; }
        public string ZonaId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual Zona Zona { get; set; }
        public virtual ApplicationUser Usuario { get; set; }
    }
}