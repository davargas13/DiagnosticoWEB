using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiagnosticoWeb.Code
{
    /// <summary>
    /// Clase auxiliar que sirve para serializar los modelos de la base de datos a cadenas JSON sin que haya problemas de referencia circular
    /// </summary>
    public class JsonSedeshu
    {
        /// <summary>
        /// Funcion que convierte un objeto a una cadena JSON sin dar problemas de referencia circular
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static string SerializeObject(object model) {
            var settings = new JsonSerializerSettings();
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

            return JsonConvert.SerializeObject(model, settings);
        }
    }
}
