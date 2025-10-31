using System;

namespace DiagnosticoWeb.Code
{
    public class IntegranteFamilia
    {
        public string Id { get; set;}
        public bool Encontrado { get; set;}
        public int NumIntegrante { get; set;}
        public bool Analfabetismo { get; set;}
        public bool Inasistencia { get; set;}
        public bool PrimariaCompleta { get; set;}
        public bool SecundariaCompleta { get; set;}
        public bool Educativa { get; set;}
        public bool ServiciosSalud { get; set;}
        public bool SeguridadSocial { get; set;}
        public bool AsisteEscuela { get; set;}
        public bool EconomicamenteActiva { get; set;}
        public bool Ssalud { get; set;}
        public bool SeguroPopular { get; set;}
        public bool SaludDirecto { get; set;}
        public bool SeguroDirecto { get; set;}
        public bool TieneActa { get; set;}
        public string ParentescoId { get; set;}
        public string SexoId { get; set;}
        public bool Discapacidad { get; set;}
        public short GradoEducativa { get; set;}
        public int Edad { get; set;}
    }
}