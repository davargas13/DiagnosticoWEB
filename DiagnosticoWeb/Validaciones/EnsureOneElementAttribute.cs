using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace DiagnosticoWeb.Validaciones
{
    /// <summary>
    /// Clase que valida que solo se haya seleccionado un valor dentro de un listado
    /// </summary>
    public class EnsureOneElementAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var list = value as IList;
            if (list != null)
            {
                return list.Count > 0;
            }

            return false;
        } 
    }
}