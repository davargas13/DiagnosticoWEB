using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;

namespace DiagnosticoWeb.Models
{
    /// <summary>
    /// Clases que mapean la tabla de dependencias a objetos de persistencia, tabla que guarda las dependencias que pueden entregar apoyos a los beneficiarios
    /// </summary>
    [Table("Dependencias")]
    public class Dependencia
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        
        public virtual List<Zona> Zonas { get; set; }
        public virtual List<TrabajadorDependencia> TrabajadorDependencias { get; set; }
    }

    public class DependenciaIndex
    {
        public List<Zona> Zonas { get; set; }
        public List<Municipio> Municipios { get; set; }
        public Dependencia Dependencia { get; set; }
    }
    
    public class DependenciasResponse
    {
        public List<Dependencia> Dependencias { get; set; }
        public int Total { get; set; }
    }

    public class DependenciasResponseSelect 
    {
        public List<Dependencia> Dependencias { get; set; }
    }
    
    public class DependenciasRequest
    {
        public string Nombre { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }

    public class DependenciaCreateEditModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [MaxLength(250, ErrorMessage = "El nombre debe tener como máximo 250 caracteres.")]
        public string Nombre { get; set; }
        public List<Municipio> Municipios { get; set; }
        public List<Zona> Zonas { get; set; }
    }

    public class DependenciaApiModel
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public string DeletedAt { get; set; }
    }
}