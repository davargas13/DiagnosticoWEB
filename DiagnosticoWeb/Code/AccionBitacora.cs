using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiagnosticoWeb.Code
{
    /// <summary>
    /// Enumerado auxiliar que sirve para representar el estatus en la aplicacion de la encuesta CONEVAL en un hogar
    /// </summary>
    public class AccionBitacora
    {
        public static string LOGIN = "Inicio de sesión"; 
        public static string INSERCION = "Inserción";
        public static string EDICION = "Edición";
        public static string ELIMINACION = "Eliminación";
        public static string EXPORTACION = "Exportación";
        public static string IMPORTACION = "Importación";

        public static List<string> get()
        {
            var list = new List<string>();
            list.Add(LOGIN);
            list.Add(INSERCION);
            list.Add(EDICION);
            list.Add(ELIMINACION);
            list.Add(EXPORTACION);
            list.Add(IMPORTACION);
            
            return list;
        }
    }
}
