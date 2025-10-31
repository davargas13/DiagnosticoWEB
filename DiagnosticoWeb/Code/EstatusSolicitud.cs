using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiagnosticoWeb.Code
{
    /// <summary>
    /// Enumerado auxiliar que representa el estatus de una solicitud de apoyo
    /// </summary>
    public class EstatusSolicitud
    {
        public static string INCOMPLETA = "Incompleta"; //La solicitud no se ha completado de registrar
        public static string PENDIENTE = "Pendiente"; //La solicitud no ha sido dictaminada por el personal administrativo de la dependencia
        public static string ACEPTADO = "Aceptado"; //La solicitud fue acepatada para su entrega
        public static string RECHAZADO = "Rechazado"; //La solicitud fue rechazada para su entrega
        public static string ENTREGADO = "Entregado"; //La solicitud fue entregada
    }
}
