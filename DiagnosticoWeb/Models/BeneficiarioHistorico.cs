using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;

namespace DiagnosticoWeb.Models
{
    /// <summary>
    /// Clases que mapean a modelos de persistencia la tabla Historico la cual guarda cada cambio hecho en los datos personales de los beneficiarios
    /// </summary>
    [Table("BeneficiarioHistorico")]
    public class BeneficiarioHistorico
    {
        public string Id { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string Nombre { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string Curp { get; set; }
        public string Rfc { get; set; }
        public string Comentarios { get; set; }
        public string BeneficiarioId { get; set; }
        public string EstudioId { get; set; }
        public string EstadoId { get; set; }
        public string DomicilioId { get; set; }
        public int? EstatusVisita { get; set; }
        public string EstadoCivilId { get; set; }
        public string DiscapacidadId { get; set; }
        public string SexoId { get; set; }
        public string TrabajadorId { get; set; }
        public string EstatusInformacion { get; set; }
        public DateTime EstatusUpdatedAt { get; set; }
        public string GradoEstudioId { get; set; }
        public string CausaDiscapacidadId { get; set; }
        public string DiscapacidadGradoId { get; set; }
        public bool Estatus { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string Huellas { get; set; }
        public string Folio { get; set; }

        public virtual Beneficiario Beneficiario { get; set; }
        public virtual Estudio Estudio { get; set; }
        public virtual GradosEstudio GradoEstudio { get; set; }
        public virtual Estado Estado { get; set; }
        public virtual EstadoCivil EstadoCivil { get; set; }
        public virtual Discapacidad Discapacidad { get; set; }
        public virtual DiscapacidadGrado DiscapacidadGrado { get; set; } 
        public virtual CausaDiscapacidad CausaDiscapacidad { get; set; }
        public virtual Sexo Sexo { get; set; }
        public virtual Trabajador Trabajador { get; set; }
        public virtual Domicilio Domicilio { get; set; }
    }
}