using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiagnosticoWeb.Models
{
    /// <summary>
    /// Clases que mapean la tabla de regiones a objetos de persistencia, tabla que guarda las regiones definidas por cada dependencia
    /// </summary>
    [Table("Zonas")]
    public class Zona
    {
        public string Id { get; set; }
        [Required(ErrorMessage = "El nombre de la región es obligatorio.")]
        public string Clave { get; set; }
        public string DependenciaId { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual Dependencia Dependencia { get; set; }
        public virtual List<MunicipioZona> MunicipioZonas { get; set; }
        [NotMapped]
        public virtual List<MunicipiosZona> Municipios{ get; set; }
    }

    public class ZonaApiModel
    {
        public string Id { get; set; }
        public string Clave { get; set; }
        public string DependenciaId { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public string DeletedAt { get; set; }
    }


    public class MunicipiosZona
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
    }
}