using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiagnosticoWeb.Excepciones
{
    public class ValorNuloException : Exception
    {
        public string Valor { get; set; }
        public string Auxiliar { get; set; }
        public int Fila { get; set; }

        public ValorNuloException()
        {
        }

        public ValorNuloException(string valor, int fila)
        {
            Valor = valor;
            Fila = fila;
        }

        public ValorNuloException(string valor, string auxiliar, int fila)
        {
            Valor = valor;
            Auxiliar = auxiliar;
            Fila = fila;
        }

        public ValorNuloException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
