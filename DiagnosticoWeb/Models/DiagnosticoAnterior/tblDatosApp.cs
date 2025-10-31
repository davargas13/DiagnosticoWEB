using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace DiagnosticoWeb.Models.DiagnosticoAnterior
{
    public class tblDatosApp
    {
        [Key]
        public long id { get; set; }
        public string idencuesta { get; set; }
        public long idencuestador { get; set; }
        public DateTime fechaencuesta { get; set; }
        public DateTime fechafinencuesta { get; set; }
        public int tiempoencuesta { get; set; }
        public string latitud { get; set; }
        public string longitud { get; set; }
        public DateTime FechaInsert { get; set; }
        public string Name2 { get; set; }
    }
}