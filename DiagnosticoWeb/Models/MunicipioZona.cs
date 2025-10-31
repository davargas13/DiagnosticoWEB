using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiagnosticoWeb.Models
{
    /// <summary>
    /// Clases que mapean la tabla de zonas de los municipios a objetos de persistencia, tabla que guarda la relacion entre los municipios y las zonas de las dependencias
    /// </summary>
    [Table("MunicipiosZonas")]
    public class MunicipioZona
    {
        public string Id { get; set; }
        public string ZonaId { get; set; }
        public string MunicipioId { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual Zona Zona { get; set; }
        public virtual Municipio Municipio { get; set; }
    }

    public class MunicipioZonaApiModel
    {
        public string Id { get; set; }
        public string ZonaId { get; set; }
        public string MunicipioId { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public string DeletedAt { get; set; }
    }
}