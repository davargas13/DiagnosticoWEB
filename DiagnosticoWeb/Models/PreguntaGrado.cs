using DiagnosticoWeb.Code;
using DiagnosticoWeb.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using DiagnosticoWeb.Validaciones;

namespace DiagnosticoWeb.Models
{
    /// <summary>
    /// Clases que mapean la tabla de grados de pregutnas a objetos de persistencia, tabla que guarda los grados que puede tener una pregunta de opcion multiple
    /// </summary>
    [Table("PreguntaGrado")]
    public class PreguntaGrado
    {
        public string Id { get; set; }
        public string PreguntaId  { get; set; }
        public string Grado { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual Pregunta Pregunta { get; set; }
    }

    public class PreguntaGradoCreateModel
    {
        public string Id { get; set; }
        public string PreguntaId  { get; set; }
        [Required]
        public string Grado { get; set; }
    }

    public class PreguntaGradoApiModel
    {
        public string Id { get; set; }
        public string PreguntaId { get; set; }
        public string Grado { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public string DeletedAt { get; set; }
    }
}
