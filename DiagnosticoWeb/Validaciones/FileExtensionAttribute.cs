using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DiagnosticoWeb.Validaciones
{
    /// <summary>
    /// Clase que valida las extensiones de los archivos importados al sistema
    /// </summary>
    public class FileExtensionAttribute : ValidationAttribute
    {
        private string _extension;

        public FileExtensionAttribute()
        {
            this._extension = ".xlsx";
        }
        public FileExtensionAttribute(string extension)
        {
            this._extension = extension;
        }

        /// <summary>
        /// Funcion que valida la extension del archivo
        /// </summary>
        /// <param name="value">Archivo a evaluar</param>
        /// <returns>Archivo valido</returns>
        public override bool IsValid(object value)
        {
            IFormFile file = (IFormFile) value;
            if (file != null) { 
                var ext = Path.GetExtension(file.FileName);

                return ext == _extension;
            }
            return true;
        }
    }
}