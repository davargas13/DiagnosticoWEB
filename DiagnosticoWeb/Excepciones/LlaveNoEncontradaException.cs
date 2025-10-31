using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiagnosticoWeb.Excepciones
{
    public class LlaveNoEncontradaException : Exception
    {
        public LlaveNoEncontradaException()
        {
        }

        public LlaveNoEncontradaException(string message)
            : base(message)
        {
        }

        public LlaveNoEncontradaException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
