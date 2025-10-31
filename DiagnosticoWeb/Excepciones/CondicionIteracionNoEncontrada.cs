using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiagnosticoWeb.Excepciones
{
    public class CondicionIteracionNoEncontrada : Exception
    {
        public CondicionIteracionNoEncontrada()
        {
        }

        public CondicionIteracionNoEncontrada(string message)
            : base(message)
        {
        }

        public CondicionIteracionNoEncontrada(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
