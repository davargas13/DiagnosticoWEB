using System.Collections.Generic;

namespace DiagnosticoWeb.Code
{
    public class DomicilioCompleto
    {
        public string cp { get; set; }
        public List<string> cis { get; set; }
        public string colonia { get; set; }
        public List<string> bandera { get; set; }
        public string manzana { get; set; }
        public List<string> zis { get; set; }
        public string idh_municipal { get; set; }
        public string tipo_zap { get; set; }
        public string ageb { get; set; }
        public List<string> municipio { get; set; }
        public string zap { get; set; }
        public string marginacion_loc { get; set; }
        public string marginacion_ageb { get; set; }
        public string marginacion_mun { get; set; }
        public List<string> Localidad { get; set; }
    }
}