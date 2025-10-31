using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiagnosticoWeb.Models
{
    /// <summary>
    /// Clases que mapean la tabla de Dependencias del trabajador a objetos de persistencia, tabla que guarda las dependencias que puede tener un trabajador de campo
    /// </summary>
    [Table("TrabajadoresDependencias")]
    public class TrabajadorDependencia
    {
        public string Id { get; set; }
        public string DependenciaId { get; set; }
        public string TrabajadorId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual Dependencia Dependencia { get; set; }
        public virtual Trabajador Trabajador { get; set; }
    }

    public class TrabajadorDependenciaApiModel
    {
        public string Id { get; set; }
        public string DependenciaId { get; set; }
        public string UsuarioId { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public string DeletedAt { get; set; }
    }
}