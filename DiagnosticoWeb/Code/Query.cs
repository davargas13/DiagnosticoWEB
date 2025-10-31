using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using DiagnosticoWeb.Database;
using DiagnosticoWeb.Models;

namespace DiagnosticoWeb.Code
{
    /// <summary>
    /// Clase auxiliar que permite generar consultas dinamicas para la pantalla del listado de solicitudes
    /// </summary>
    public class Query
    {
        /// <summary>
        /// Funcion que permite generar una consulta a la base de datos con sentencias SQL
        /// </summary>
        /// <param name="query">Consulta en sql</param>
        /// <param name="map">Arreglo de objetos donde se almacenará el resultado de la consulta</param>
        /// <param name="_context">Conexion a la base de datos</param>
        /// <typeparam name="T">Modelo de base de datos</typeparam>
        /// <returns>Listado de objetos de acuerdo a la consulta</returns>
        public static List<T> RawSqlQuery<T>(string query, Func<DbDataReader, T> map, ApplicationDbContext _context)
        {
            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = query;
                command.CommandType = CommandType.Text;

                _context.Database.OpenConnection();

                using (var result = command.ExecuteReader())
                {
                    var entities = new List<T>();

                    while (result.Read())
                    {
                        entities.Add(map(result));
                    }

                    return entities;
                }
            }
        }

        /// <summary>
        /// Funcion que construye un diccionario de propiedades de un modelo de base de datos, se usa para validar que un filtro del listado de solicitudes pertenezca a un modelo
        /// y entonces se pueda aplicar el filtro correspondiente en la consulta
        /// </summary>
        /// <param name="properties">Listado de las propiedades del modelo</param>
        /// <returns>Diccionario de propiedades del modelo</returns>
        public static Dictionary<string, bool> BuildPropertyDictionary(PropertyInfo[] properties)
        {
            Dictionary<string, bool> dictionary = new Dictionary<string, bool>();
            foreach (var property in properties)
            {
                var isCatalogo = property.Name.Contains("Id");
                var prop = property.Name.Replace("Id", "");
                if (!dictionary.ContainsKey(prop))
                {
                    dictionary.Add(prop, isCatalogo);
                }
            }

            return dictionary;
        }

        /// <summary>
        /// Funcion que construye una consulta SQL dinamicamente dependiendo de los filtros seleccionados por el usuario, se utiliza en las pantallas del listado de solicitudes y
        /// listado de beneficiarios
        /// </summary>
        /// <param name="consulta">Cadena con la consulta SQL</param>
        /// <param name="i">Contador de condiciones anexadas a la consulta SQL</param>
        /// <param name="dictionary">Diccionario de condiciones anexadas a la consulta SQL</param>
        /// <param name="llave">Nombre del filtro</param>
        /// <param name="valor">Valor del filtro</param>
        /// <param name="valores">Listado de valores de los filtros que se pasaran a la consulta</param>
        /// <param name="prefijo">Alias de la tabla en la consulta</param>
        /// <returns>Devuelve si encontro la propiedad en el diccionario de propiedades</returns>
        public static bool FiltrarSolicitudes(ref string consulta, ref int i, Dictionary<string, bool> dictionary,
            string llave, string valor, List<string> valores, string prefijo)
        {
            var encontrado = dictionary.ContainsKey(llave);
            if (!encontrado) return encontrado;
            var campo = dictionary[llave];
            consulta += " and " + prefijo + "." + llave +
                        (campo ? "Id" : "") +
                        (campo ? " = @" + i + " " : " like @" + i + "");
            switch (valor)
            {
                case "NO_CARENTE":
                case "SIN_BIENESTAR":
                    valores.Add("0");
                    break;
                case "CARENTE":
                case "BIENESTAR_MINIMO":
                    valores.Add("1");
                    break;
                case "BIENESTAR":
                    valores.Add("2");
                    break;
                default:
                    valores.Add(campo ? valor : "%" + valor + "%");
                    break;
            }

            i++;

            return encontrado;       
        }
    }    
}