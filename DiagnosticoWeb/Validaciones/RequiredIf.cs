using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace DiagnosticoWeb.Validaciones
{
    /// <summary>
    /// Clase que valida un campo solo si una condicion se cumple
    /// </summary>
    public class RequiredIf:ValidationAttribute
    {
        private string propertyNameToCheck;
        private bool propertyValueToCheck;

        /// <summary>
        /// Consturctor de la clase
        /// </summary>
        /// <param name="propertyNameToCheck">Propuedad a evaluar</param>
        /// <param name="propertyValueToCheck">Conidcion que al dar positiva se evaluara la otra propiedad</param>
        public RequiredIf(string propertyNameToCheck, bool propertyValueToCheck)
        {
            this.propertyNameToCheck = propertyNameToCheck;
            this.propertyValueToCheck = propertyValueToCheck;
        }
        
        /// <summary>
        /// Funcion que aplica la validacion o no dependiendo la condicion especificada
        /// </summary>
        /// <param name="value">Valor del campo</param>
        /// <param name="validationContext">Modelo cuyo campo se validará</param>
        /// <returns></returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var propertyName = validationContext.ObjectType.GetProperty(propertyNameToCheck);
            if (propertyName == null)
                return new ValidationResult(string.Format(CultureInfo.CurrentCulture, "Debe seleccionar el {0} ", new[] { propertyNameToCheck }));

            var propertyValue = propertyName.GetValue(validationContext.ObjectInstance, null) as string;

            if (propertyValue=="true")
            {
                if (value==null)
                { 
                    return new ValidationResult(string.Format(CultureInfo.CurrentCulture, "El {0} es obligatorio", new[] { validationContext.DisplayName }));
                }
            }
            

            return ValidationResult.Success;
        }
        
    }
}