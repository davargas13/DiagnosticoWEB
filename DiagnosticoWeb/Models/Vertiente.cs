using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DiagnosticoWeb.Validaciones;

namespace DiagnosticoWeb.Models
{
    /// <summary>
    /// Clases que mapean la tabla de vertientes a objetos de persistencia, tabla que guarda las vertienes seleccionadas en las solicitudes de apoyo por parte de los beneficiarios
    /// </summary>
    [Table("Vertientes")]
    public class Vertiente
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string ProgramaSocialId { get; set; }
        public string TipoEntrega { get; set; }
        public double Costo { get; set; }
        public string Ciclo { get; set; }
        public string Ejercicio { get; set; }
        public string UnidadId { get; set; }
        public int? Vigencia { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual ProgramaSocial ProgramaSocial { get; set; }
        public virtual List<Solicitud> Solicitudes { get; set; }
        public virtual List<VertienteCarencia> VertienteCarencias { get; set; }
        public virtual List<VertienteArchivo> VertienteArchivos { get; set; }
        public virtual Unidad Unidad { get; set; }
    }

    public class VertienteApiModel
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string ProgramaSocialId { get; set; }
        public string TipoEntrega { get; set; }
        public double Costo { get; set; }
        public string UnidadId { get; set; }
        public int? Vigencia { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public string DeletedAt { get; set; }
    }

    public class VertienteCreateEditModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [MaxLength(250, ErrorMessage = "El nombre debe tener como máximo 250 caracteres.")]
        public string Nombre { get; set; }
        
        [Required(ErrorMessage = "El ciclo es obligatorio.")]
        [MaxLength(250, ErrorMessage = "El ciclo debe tener como máximo 250 caracteres.")]
        public string Ciclo { get; set; }
        
        [Required(ErrorMessage = "El ejercicio fiscal es obligatorio.")]
        [MaxLength(250, ErrorMessage = "El ejercicio fiscal debe tener como máximo 250 caracteres.")]
        public string Ejercicio { get; set; }
        
        [Required(ErrorMessage = "El costo es obligatorio.")]
        public double Costo { get; set; }
        
        [Required(ErrorMessage = "El tipo de entrega es obligatorio.")]
        public string TipoEntrega { get; set; }
        [Required(ErrorMessage = "La vigencia es obligatoria.")]
        [Range(1, int.MaxValue, ErrorMessage = "Es requerido un número entero y mayor a 0.")]
        public int? Vigencia { get; set; }
        [Required(ErrorMessage = "La Unidad es obligatoria.")]
        public string UnidadId { get; set; }
        public List<VertienteArchivo> Evidencias { get; set; }
        
        [EnsureOneElement(ErrorMessage = "Las carencias son obligatorias.")] 
        public List<string> Carencias { get; set; }
    }
    
    public class VertienteRequest
    {
//        public string Nombre { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }

    public class VertienteResponse
    {
        public List<VertienteListModel> Vertientes { get; set; }
        public int Total { get; set; }
    }

    public class VertienteListModel
    {
        public string Id { get; set; }
        public string Vertiente { get; set; }
        public string Programa { get; set; }
    }
}