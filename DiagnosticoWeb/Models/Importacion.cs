using Microsoft.AspNetCore.Http;
using DiagnosticoWeb.Validaciones;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace DiagnosticoWeb.Models
{
    /// <summary>
    /// Clases que mapean la tabla de importaciones a objetos de persistencia, tabla que guarda los datos de quien y a que hora se importo algun catalogo al sistema
    /// </summary>
    [Table("Importaciones")]
    public class Importacion
    {
        [Key]
        public string Clave { get; set; }
        public string UsuarioId { get; set; }
        public DateTime? ImportedAt { get; set; }
        
        public virtual ApplicationUser Usuario { get; set; }

    }
}