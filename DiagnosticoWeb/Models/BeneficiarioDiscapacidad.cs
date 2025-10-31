using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;

namespace DiagnosticoWeb.Models
{
    
    [Table("BeneficiarioDiscapacidades")]
    public class BeneficiarioDiscapacidad
    {
        public string Id { get; set; }
        public string BeneficiarioId { get; set; }
        public string DiscapacidadId { get; set; }
        public string GradoId { get; set; }
        public string CausaId { get; set; }
        
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual Beneficiario Beneficiario { get; set; }
        public virtual Respuesta Discapacidad { get; set; }
        public virtual Grado Grado { get; set; }
        public virtual CausaDiscapacidad Causa { get; set; }
    }
}