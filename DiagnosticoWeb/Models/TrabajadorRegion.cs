using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiagnosticoWeb.Models
{
    /// <summary>
    /// Clases que mapean la tabla de regiones del trabajador a objetos de persistencia, tabla que guarda las regiones que puede visitar un trabajador de campo
    /// </summary>
    [Table("TrabajadorRegion")]
    public class TrabajadorRegion
    {
        public string Id { get; set; }
        public string TrabajadorId { get; set; }
        public string ZonaId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual Zona Zona { get; set; }
        public virtual Trabajador Trabajador { get; set; }
    }

    public class TrabajadorRegionApiModel
    {
        public string Id { get; set; }
        public string TrabajadorId { get; set; }
        public string ZonaId { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public string DeletedAt { get; set; }
    }
}