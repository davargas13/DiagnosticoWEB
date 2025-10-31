using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiagnosticoWeb.Models
{
    [Table("AlgoritmoResultados")]
    public class AlgoritmoResultado
    {
        public string Id { get; set; }
        public string BeneficiarioId { get; set; }
        public bool Educativa { get; set; }
        public bool Analfabetismo { get; set; }
        public bool Inasistencia { get; set; }
        public bool PrimariaIncompleta { get; set; }
        public bool SecundariaIncompleta { get; set; }
        public bool ServicioSalud { get; set; }
        public bool SeguridadSocial { get; set; }
        public bool Vivienda { get; set; }
        public bool Piso { get; set; }
        public bool Techo { get; set; }
        public bool Muro { get; set; }
        public bool Hacinamiento { get; set; }
        public bool Servicios { get; set; }
        public bool Agua { get; set; }
        public bool Drenaje { get; set; }
        public bool Electricidad { get; set; }
        public bool Combustible { get; set; }
        public bool Alimentaria { get; set; }
        public string GradoAlimentaria { get; set; }
        public bool Pea { get; set; }
        public double Ingreso { get; set; }
        public int LineaBienestar { get; set; }
        public string NivelPobreza { get; set; }
        public bool TieneActa { get; set; }
        public short SeguridadPublica { get; set; }
        public short SeguridadParque { get; set; }
        public short ConfianzaLiderez { get; set; }
        public short ConfianzaInstituciones { get; set; }
        public short Movilidad { get; set; }
        public short RedesSociales { get; set; }
        public short Tejido { get; set; }
        public short Satisfaccion { get; set; }
        public bool PropiedadVivienda { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string VersionId { get; set; }
        //Relations
        public virtual AlgoritmoVersion Version { get; set; }
        public virtual Beneficiario Beneficiario { get; set; }
    }
    
    public class AlgoritmoResultadoModel
    {
        public string Id { get; set; }
        public string Folio { get; set; }
        public string VersionId { get; set; }
        public bool Educativa { get; set; }
        public bool Analfabetismo { get; set; }
        public bool Inasistencia { get; set; }
        public bool PrimariaIncompleta { get; set; }
        public bool SecundariaIncompleta { get; set; }
        public bool ServicioSalud { get; set; }
        public bool SeguridadSocial { get; set; }
        public bool Vivienda { get; set; }
        public bool Piso { get; set; }
        public bool Techo { get; set; }
        public bool Muro { get; set; }
        public bool Hacinamiento { get; set; }
        public bool Servicios { get; set; }
        public bool Agua { get; set; }
        public bool Drenaje { get; set; }
        public bool Electricidad { get; set; }
        public bool Combustible { get; set; }
        public bool Alimentaria { get; set; }
        public string GradoAlimentaria { get; set; }
        public bool Pea { get; set; }
        public double Ingreso { get; set; }
        public int LineaBienestar { get; set; }
        public string NivelPobreza { get; set; }
        public bool TieneActa { get; set; }
        public short SeguridadPublica { get; set; }
        public short SeguridadParque { get; set; }
        public short ConfianzaLiderez { get; set; }
        public short ConfianzaInstituciones { get; set; }
        public short Movilidad { get; set; }
        public short RedesSociales { get; set; }
        public short Tejido { get; set; }
        public short Satisfaccion { get; set; }
        public bool PropiedadVivienda { get; set; }
        public string CreatedAt { get; set; }
        public List<AlgoritmoIntegranteResultadoModel> ResultadoIntegrantes { get; set; }
    }
}
