using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiagnosticoWeb.Models
{
    [Table("AlgoritmoIntegranteResultados")]
    public class AlgoritmoIntegranteResultado
    {
        public string Id { get; set; }
        public bool Educativa { get; set; }
        public bool TieneActa { get; set; }
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
        public string ParentescoId { get; set; }
        public string SexoId { get; set; }
        public string Discapacidad { get; set; }
        public int GradoEducativa { get; set; }
        public int Edad { get; set; }
        public double Ingreso { get; set; }
        public int LineaBienestar { get; set; }
        public int NumIntegrante { get; set; }
        public string NivelPobreza { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string ResultadoId { get; set; }
        //Relations
        public virtual AlgoritmoResultado Resultado { get; set; }
    }
    
    public class AlgoritmoIntegranteResultadoModel
    {
        public string Id { get; set; }
        public bool Educativa { get; set; }
        public bool TieneActa { get; set; }
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
        public string ParentescoId { get; set; }
        public string SexoId { get; set; }
        public string Discapacidad { get; set; }
        public int GradoEducativa { get; set; }
        public int Edad { get; set; }
        public double Ingreso { get; set; }
        public int LineaBienestar { get; set; }
        public int NumIntegrante { get; set; }
        public string NivelPobreza { get; set; }
        public string ResultadoId { get; set; }
    }
}
