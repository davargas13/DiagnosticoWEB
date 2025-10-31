using System;

namespace DiagnosticoWeb.Models
{
    /// <summary>
    /// Clase que muestra errores detectados por el sistema
    /// </summary>
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}