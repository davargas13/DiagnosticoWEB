using System.Collections.Generic;

namespace DiagnosticoWeb.Code
{
    public class CurpResponse
    {
        public string Mensaje { get; set; }
        public Resultado Resultado { get; set; }
    }

    public class Resultado
    {
        public List<CurpCollection> CurpCollection { get; set; }
    }

    public class CurpCollection
    {
        public string CURP { get; set; }
        public string apellido1 { get; set; }
        public string apellido2 { get; set; }
        public string nombres { get; set; }
        public string sexo { get; set; }
        public string fechNac { get; set; }
        public string nacionalidad { get; set; }
    }
}