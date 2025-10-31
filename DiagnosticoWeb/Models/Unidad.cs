using DiagnosticoWeb.Validaciones;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiagnosticoWeb.Models
{
    [Table("Unidades")]
    public class Unidad
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }

    public class UnidadApiModel
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public string DeletedAt { get; set; }
    }
    public class UnidadRequest
    {
        public string Usuario { get; set; }
        public string ImportedAt { get; set; }
        public string Nombre { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }

    public class UnidadResponse
    {
        public int Total { get; set; }
        public List<Unidad> Unidades { get; set; }
    }

    public class UnidadModel
    {
        [Required(ErrorMessage = "El Id de la unidad es obligatorio.")]
        [MaxLength(40, ErrorMessage = "El Id de la unidad debe tener como máximo 40 caracteres.")]
        public string Id { get; set; }
        public string IdAnterior { get; set; }

        [Required(ErrorMessage = "El nombre de la unidad es obligatorio.")]
        [MaxLength(255, ErrorMessage = "El nombre de la unidad debe tener como máximo 255 caracteres.")]
        public string Nombre { get; set; }
    }
}