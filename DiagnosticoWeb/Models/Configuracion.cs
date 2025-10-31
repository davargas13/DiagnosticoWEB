using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Microsoft.AspNetCore.Http;
using DiagnosticoWeb.Database;
using DiagnosticoWeb.Validaciones;

namespace DiagnosticoWeb.Models
{
    [Table("Configuracion")]
    public class Configuracion
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string Valor { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public static string getVersionAlgoritmo(ApplicationDbContext _context)
        {
            var configuracionDB = _context.Configuracion.FirstOrDefault(c=>c.Nombre.Equals("Algoritmo"));
            if (configuracionDB == null)
            {
                configuracionDB = new Configuracion
                {
                    Id = Guid.NewGuid().ToString(),
                    Nombre = "Algoritmo",
                    Valor = "1"
                };
                _context.Configuracion.Add(configuracionDB);
                _context.SaveChanges();
            }

            return configuracionDB.Valor;
        }
    }  
    
    public class ConfiguracionApiModel
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string Valor { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
    }
    
    public class ConfiguracionModel
    {
        public string Id { get; set; }
        [Required(ErrorMessage = "Este campo es obligatorio.")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "Este campo es obligatorio.")]
        public string Valor { get; set; }

        public List<AlgoritmoVersionModel> Versiones { get; set; }
    }

    public class ConfiguracionResponse
    {
        public List<Configuracion> Configuraciones { get; set; }
    }

    public class AlgoritmoRequest
    {
        [Required(ErrorMessage = "El archivo Jar es obligatorio")]
        [FileExtension(".jar", ErrorMessage = "El archivo a importar debe tener la extensión .jar")]
        public IFormFile Jar { get; set; }
        
        [Required(ErrorMessage = "El archivo Apk es obligatorio")]
        [FileExtension(".apk", ErrorMessage = "El archivo a importar debe tener la extensión .apk")]
        public IFormFile Apk { get; set; }
        
        [Required(ErrorMessage = "El archivo Zip es obligatorio")]
        [FileExtension(".zip", ErrorMessage = "El archivo a importar debe tener la extensión .zip")]
        public IFormFile Zip { get; set; }
    }    
    
    public class FundamentoRequest
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        [Required]
        public string Valor { get; set; }
    }

    public class FamiliaRequest
    {
        public string Folio { get; set; }
    }

    public class FamiliaResponse
    {
        public string Id { get; set; }
        public string Folio { get; set; }
        public string Nombres { get; set; }
        public string Calle { get; set; }
        public string Municipio { get; set; }
        public string Localidad { get; set; }
        public int NumIntegrantes { get; set; }
        public string ZonaImpulso { get; set; }
        public int Version { get; set; }
        public bool Estatus { get; set; }
    }

    public class CompletarDatos
    {
        public string Inicio { get; set; }
        public string Fin { get; set; }
        public string Accion { get; set; }
        public int NumDomiciliosSinCalculo{ get; set; }
    }
} 