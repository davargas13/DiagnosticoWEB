using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiagnosticoWeb.Models
{
    /// <summary>
    /// Clases que mapean la tabla de dependencias del usuario a objetos de persistencia, tabla que guarda las dependencias que puede ver el usuario en sesion
    /// </summary>
    [Table("UsuariosDependencias")]
    public class UsuarioDependencia
    {
        public string Id { get; set; }
        public string DependenciaId { get; set; }
        public string UsuarioId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual Dependencia Dependencia { get; set; }
        public virtual ApplicationUser Usuario { get; set; }
    }
}