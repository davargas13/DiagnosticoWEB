using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace DiagnosticoWeb.Models.DiagnosticoAnterior
{
    public class tblEncuestaSabana
    {
        [NotMapped]
        public BigInteger id { get; set; }
        public int idcabrespact { get; set; }
        public int idpregunta { get; set; }
        public int idrespuesta { get; set; }
        public string txtrespuesta { get; set; }
        public string idintegrante { get; set; }
    }
}