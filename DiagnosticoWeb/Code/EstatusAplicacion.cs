using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiagnosticoWeb.Code
{
    /// <summary>
    /// Enumerado auxiliar que sirve para representar el estatus en la aplicacion de la encuesta CONEVAL en un hogar
    /// </summary>
    public class EstatusAplicacion
    {
        public static string INCOMPLETO = "Incompleto"; //Que no se ha terminado de aplicar la encuesta
        public static string COMPLETO = "Completo"; //Que ya se termino de aplicar la encuesta
        public static string GENERADO = "Generado"; //Que la encuesta se generó automaticamente para hacer pruebas
    }
}
