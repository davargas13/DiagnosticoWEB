using DiagnosticoWeb.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DiagnosticoWeb.Models
{
    /// <summary>
    /// Clases que mapean la tabla de Version de encuesta a objetos de persistencia, tabla que guarda los cambios hechos a la encuesta CONEVAL
    /// </summary>
    [Table("EncuestaVersiones")]
    public class EncuestaVersion
    {
        public string Id { get; set; }
        public bool Activa { get; set; }
        public int Numero { get; set; }
        public string EncuestaId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual Encuesta Encuesta { get; set; }
        public virtual List<Pregunta> Preguntas { get; set; }

        public static EncuestaVersion AgregarVersion (int Numero, string Id)
        {
            var version = new EncuestaVersion()
            {
                EncuestaId = Id,
                Activa = true,
                Numero = Numero+1,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
 
            return version;
        }

        public static EncuestaVersion FindActiva(ApplicationDbContext db, string encuestaId)
        {
            var versiones = db.EncuestaVersion.Where(ev => ev.Activa && ev.EncuestaId == encuestaId)
                .OrderByDescending(ev => ev.CreatedAt);
            return versiones.Any() ? versiones.FirstOrDefault() : null;
        }
    }

    public class EncuestaVersionApiModel
    {
        public string Id { get; set; }
        public bool Activa { get; set; }
        public int Numero { get; set; }
        public string EncuestaId { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public string DeletedAt { get; set; }
    }
}
