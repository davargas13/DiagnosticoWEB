using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace DiagnosticoWeb.Models
{
    /// <summary>
    /// Clase que mapea la tabla AspNetUsers donde se almacenan los usuarios del portal admnistrativo
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public string DependenciaId { get; set; }
        public string PerfilId { get; set; }
        public string Token { get; set; }
        public DateTime? FechaToken { get; set; }
        
        public virtual Dependencia Dependencia { get; set; }
        [NotMapped] 
        public bool IsDisabled;
    }
}
