using DiagnosticoWeb.Code;
using DiagnosticoWeb.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DiagnosticoWeb.Models
{
    /// <summary>
    /// Clase generica para manipular los registros de las tablas en las vistas del sistema
    /// </summary>
    public class Model
    {
        public string Id { get; set; }
        public string Nombre  { get; set; }
        public DateTime CreatedAt  { get; set; }
    }
}
