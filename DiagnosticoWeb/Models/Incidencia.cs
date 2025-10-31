using Microsoft.AspNetCore.Http;
using DiagnosticoWeb.Validaciones;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace DiagnosticoWeb.Models
{
    /// <summary>
    /// Clases que mapean la tabla de tipos de incidencias a objetos de persistencia, tabla que guarda el tipo de incidencia que reporta el encuestador
    /// </summary>
    [Table("Incidencias")]
    public class Incidencia
    {
        [Key]
        public string Id { get; set; }
        public string Observaciones { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string TrabajadorId { get; set; }
        public string TipoIncidenciaId { get; set; }
        public string BeneficiarioId { get; set; }
        public string DeviceId { get; set; }
        
        //Relaciones
        public virtual Trabajador Trabajador { get; set; }
        public virtual TipoIncidencia TipoIncidencia { get; set; }
        public virtual Beneficiario Beneficiario { get; set; }
    }
    
    public class IncidenciaApiModel
    {
        public string Id { get; set; }
        public string Observaciones { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public string DeletedAt { get; set; } 
        public string TrabajadorId { get; set; }
        public string TipoIncidenciaId { get; set; }
        public string BeneficiarioId { get; set; }
        public string DeviceId { get; set; }

    }

    public class IncidenciasResponse
    {
        public string Inicio { get; set; }
        public string Fin { get; set; }
        public List<Model> TiposIncidencias { get; set; }
        public List<Model> Municipios { get; set; }
        public List<Model> Trabajadores { get; set; }
    } 
    
    public class IncidenciasRequest
    {
        public string Folio { get; set; }
        public string TrabajadorId { get; set; }
        public string TipoIncidenciaId { get; set; }
        public string MunicipioId { get; set; }
        public string LocalidadId { get; set; }
        public string ZonaImpulsoId { get; set; }
        public string Inicio { get; set; }
        public string Fin { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }

    public class IncidenciaShortResponse
    {
        public string Id { get; set; }
        public string Observaciones { get; set; }
        public string CreatedAt { get; set; }
        public string Trabajador { get; set; }
        public string TipoIncidencia { get; set; }
        public string Folio { get; set; }
        public string Municipio { get; set; }
        public string Localidad { get; set; }
        public string ZonaImpulso { get; set; }
    }
    
    public class IncidenciaResponse
    {
        public List<IncidenciaShortResponse> Incidencias { get; set; }
        public int Total { get; set; }
    }
}