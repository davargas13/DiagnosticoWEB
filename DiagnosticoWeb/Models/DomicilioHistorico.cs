using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using DiagnosticoWeb.Validaciones;

namespace DiagnosticoWeb.Models
{
    /// <summary>
    /// Clases que mapean la tabla de domicilios a objetos de persistencia, tabla que guarda la informacion de donde habita el beneficiario y su familia
    /// </summary>
    [Table("DomicilioHistorico")]
    public class DomicilioHistorico
    {
        public string Id { get; set; }
        public string DomicilioId { get; set; }
        public string Telefono { get; set; }
        public string TelefonoCasa { get; set; }
        public string Email { get; set; }
        public string DomicilioN { get; set; }
        public string NumExterior { get; set; }
        public string NumInterior { get; set; }
        public string EntreCalle1 { get; set; }
        public string EntreCalle2 { get; set; }
        public string CallePosterior { get; set; }
        public string CodigoPostal { get; set; }
        public string Latitud { get; set; }
        public string Longitud { get; set; }
        public string MunicipioId { get; set; }
        public string LocalidadId { get; set; }
        public string AgebId { get; set; }
        public string ManzanaId { get; set; }
        public string CalleId { get; set; }
        public string ZonaImpulsoId { get; set; }
        public string ColoniaId { get; set; }
        public string CaminoId { get; set; }
        public string CarreteraId { get; set; }
        public string TipoAsentamientoId { get; set; }
        public string NombreAsentamiento { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}