using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiagnosticoWeb.Models
{
    /// <summary>
    /// Clases que mapean la tabla de archivos a objetos de persistencia, tabla que almacena las fotos de los beneficiarios y sus solicitudes de apoyos
    /// </summary>
    [Table("LineasBienestar")]
    public class LineaBienestar
    {
        public int Id { get; set; }
        public float MinimaRural { get; set; }
        public float Rural { get; set; }
        public float MinimaUrbana { get; set; }
        public float Urbana { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class LineaBienestarApiModel
    {
        public int Id { get; set; }
        public float MinimaRural { get; set; }
        public float Rural { get; set; }
        public float MinimaUrbana { get; set; }
        public float Urbana { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
    }
}