using System.Collections.Generic;

namespace DiagnosticoWeb.Code
{
    public class FamiliaSabana
    {
        public List<IntegranteSabana> Integrantes{ get; set; }
    }

    public class IntegranteSabana
    {
        public string Curp { get; set; } 
        public Dictionary<string, Dictionary<int, string>> Preguntas{ get; set; }
    }
}