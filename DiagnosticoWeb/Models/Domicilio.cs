using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using DiagnosticoWeb.Validaciones;

namespace DiagnosticoWeb.Models
{
    /// <summary>
    /// Clases que mapean la tabla de domicilios a objetos de persistencia, tabla que guarda la informacion de donde habita el beneficiario y su familia
    /// </summary>
    [Table("Domicilio")]
    public class Domicilio
    {
        public string Id { get; set; }
        public string Telefono { get; set; }
        public string TelefonoCasa { get; set; }
        public string Email { get; set; }
        public string DomicilioN { get; set; }
        public string NumExterior { get; set; }
        public string NumInterior { get; set; }
        public int NumFamilia { get; set; }
        public int NumFamiliasRegistradas { get; set; }
        public string EntreCalle1 { get; set; }
        public string EntreCalle2 { get; set; }
        public string CallePosterior { get; set; }
        public string CodigoPostal { get; set; }
        public string Latitud { get; set; }
        public string Longitud { get; set; }
        public string LatitudAtm { get; set; }
        public string LongitudAtm { get; set; }
        public string MunicipioId { get; set; }
        public string LocalidadId { get; set; }
        public string AgebId { get; set; }
        public string ManzanaId { get; set; }
        public string CalleId { get; set; }
        public string ZonaImpulsoId { get; set; }
        public string ColoniaId { get; set; }
        public string CaminoId { get; set; }
        public string CarreteraId { get; set; }
        public string TrabajadorId { get; set; }
        public string CadenaOCR { get; set; }
        public string Porcentaje { get; set; }
        public bool Activa { get; set; }
        public string TipoAsentamientoId { get; set; }
        public string NombreAsentamiento { get; set; }
        public string MunicipioCalculado { get; set; }
        public string LocalidadCalculado { get; set; }
        public string AgebCalculado { get; set; }
        public string ManzanaCalculado { get; set; }
        public string ZapCalculado { get; set; }
        public string TipoZap { get; set; }
        public string CodigoPostalCalculado { get; set; }
        public string ColoniaCalculada { get; set; }
        public string CisCercano { get; set; }
        public string TipoCalculo { get; set; }
        public string MarginacionMunicipio { get; set; }
        public string MarginacionLocalidad { get; set; }
        public string MarginacionAgeb { get; set; }
        public string IndiceDesarrolloHumano { get; set; }
        public string ClaveMunicipioCalculado { get; set; }
        public string ClaveLocalidadCalculada { get; set; }
        public string ClaveMunicipioCisCalculado { get; set; }
        public string DomicilioCisCalculado { get; set; }
        public string ZonaImpulsoCalculada { get; set; }
        public string ClaveZonaImpulsoCalculada { get; set; }
        public double Indice { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public int NumIntentosCoordenadas { get; set; }
        public string LatitudCorregida { get; set; }
        public string LongitudCorregida { get; set; }
        public string EstatusDireccion { get; set; }
        public bool Prueba { get; set; }
        public string DeviceId { get; set; }
        public bool AutorizaFotos { get; set; }
        public string VersionAplicacion { get; set; }

        //Relaciones con otras tablas
        public virtual List<Beneficiario> Beneficiarios { get; set; }
        public virtual List<BeneficiarioHistorico> BeneficiariosHistoricos { get; set; }
        public virtual Municipio Municipio { get; set; }
        public virtual Localidad Localidad { get; set; }
        public virtual Trabajador Trabajador { get; set; }
        public virtual TipoAsentamiento TipoAsentamiento { get; set; }
        public virtual Ageb Ageb { get; set; }
        public virtual Colonia Colonia { get; set; }
        public virtual Calle Calle { get; set; }
        public virtual ZonaImpulso ZonaImpulso { get; set; }
        [NotMapped]
        public string AplicacionId { get; set; }
        [NotMapped]
        public string Folio { get; set; }
        [NotMapped]
        public string NombreEncuestador { get; set; } 
        [NotMapped]
        public int Tiempo { get; set; }
    }

    public class DomicilioApiModel
    {
        public string Id { get; set; }
        public string Telefono { get; set; }
        public string TelefonoCasa { get; set; }
        public string Email { get; set; }
        public string DomicilioN { get; set; }
        public string NumExterior { get; set; }
        public string NumInterior { get; set; }
        public int NumFamilia { get; set; }
        public int NumFamiliasRegistradas { get; set; }
        public string EntreCalle1 { get; set; }
        public string EntreCalle2 { get; set; }
        public string CallePosterior { get; set; }
        public string CodigoPostal { get; set; }
        public string Latitud { get; set; }
        public string Longitud { get; set; }
        public string LatitudAtm { get; set; }
        public string LongitudAtm { get; set; }
        public string MunicipioId { get; set; }
        public string LocalidadId { get; set; }
        public string AgebId { get; set; }
        public string ManzanaId { get; set; }
        public string ColoniaId { get; set; }
        public string CalleId { get; set; }
        public string ZonaImpulsoId { get; set; }
        public string CaminoId { get; set; }
        public string CarreteraId { get; set; }
        public string TrabajadorId { get; set; }
        public string CadenaOCR { get; set; }
        public string Porcentaje { get; set; }
        public string TipoAsentamientoId { get; set; }
        public string NombreAsentamiento { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public string DeletedAt { get; set; }
        public string DeviceId { get; set; }
        public string VersionAplicacion { get; set; }
        public bool AutorizaFotos { get; set; }
    }

    public class DomicilioResponse
    {
        public string Id { get; set; }
        public string Ageb { get; set; }
        public string Telefono { get; set; }
        public string TelefonoCasa { get; set; }
        public string Email { get; set; }
        public string Domicilio { get; set; }
        public string EntreCalle1 { get; set; }
        public string EntreCalle2 { get; set; }
        public string Latitud { get; set; }
        public string Longitud { get; set; }        
        public string LatitudAtm { get; set; }
        public string LongitudAtm { get; set; }
        public string CodigoPostal { get; set; }
        public string Municipio { get; set; }
        public string Localidad { get; set; }
        public string ZonaImpulso { get; set; }
        public string CadenaOCR { get; set; }
        public string Porcentaje { get; set; }
        public bool Activa { get; set; }
        public string TipoAsentamientoId { get; set; }
        public double Indice { get; set; }
        public string NumExterior { get; set; }
        public string NombreAsentamiento { get; set; }
        public string Calle { get; set; }
        public string Region { get; set; }
        public string AgebCalculado { get; set; }
        public string IndiceDesarrolloHumano { get; set; }
        public string MarginacionLocalidad { get; set; }
        public string MarginacionAgeb { get; set; }
    }

    public class VerDomicilioRequest
    {
        public int Numero { get; set; }
        public string DomicilioId { get; set; }
    }

    public class DomicilioEdit
    {
        public DomicilioEditModel Domicilio { get; set; }
        public List<Model> Municipios { get; set; }
        public List<Model> Caminos { get; set; }
        public List<Model> Carreteras { get; set; }
        public List<Model> TiposAsentamiento { get; set; }
    }

    public class DomicilioEditModel
    {
        public string Id { get; set; }
        [MaxLength(10, ErrorMessage = "El teléfono debe tener máximo 10 caracteres")]
        [Range(0, long.MaxValue, ErrorMessage = "Es requerido un número entero.")]
        public string Telefono { get; set; }
        [MaxLength(10, ErrorMessage = "El teléfono de casa debe tener máximo 10 caracteres")]
        [Range(0, long.MaxValue, ErrorMessage = "Es requerido un número entero.")]
        public string TelefonoCasa { get; set; }
        [MaxLength(100, ErrorMessage = "El correo electrónico debe tener máximo 100 caracteres")]
        public string Email { get; set; }
        [Required(ErrorMessage = "La dirección es obligatoria")]
        [MaxLength(100, ErrorMessage = "El domicilio debe tener máximo 100 caracteres")]
        public string DomicilioN { get; set; }
        [Required(ErrorMessage = "La calle posterior es obligatoria")]
        [MaxLength(100, ErrorMessage = "La calle posterior debe tener máximo 100 caracteres")]
        public string CallePosterior { get; set; }
        [Required(ErrorMessage = "El número exterior es obligatorio")]
        [MaxLength(100, ErrorMessage = "El número exterior debe tener máximo 20 caracteres")]
        public string NumExterior { get; set; }
        [MaxLength(100, ErrorMessage = "El número interior debe tener máximo 20 caracteres")]
        public string NumInterior { get; set; }
        
        [Required(ErrorMessage = "La entre calle es obligatoria")]
        [MaxLength(100, ErrorMessage = "La entre calle debe tener máximo 100 caracteres")]
        public string EntreCalle1 { get; set; }
        [Required(ErrorMessage = "La entre calle es obligatoria")]
        [MaxLength(100, ErrorMessage = "La entre calle debe tener máximo 100 caracteres")]
        public string EntreCalle2 { get; set; }
        [Required(ErrorMessage = "El código postal es obligatorio")]
        [MaxLength(5, ErrorMessage = "El código postal debe tener máximo 5 caracteres")]
        [Range(0, long.MaxValue, ErrorMessage = "Es requerido un número entero.")]
        public string CodigoPostal { get; set; }
        public string Latitud { get; set; }
        public string Longitud { get; set; }
        [Required(ErrorMessage = "El municipio es obligatorio")]
        public string MunicipioId { get; set; }
        [Required(ErrorMessage = "La localidad es obligatoria")]
        public string LocalidadId { get; set; }
        public string AgebId { get; set; }
        public string ColoniaId { get; set; }
        public string NombreAsentamiento  { get; set; }
        public string CalleId { get; set; }
        public string ZonaImpulsoId { get; set; }
        public string ManzanaId { get; set; }
        public string CarreteraId { get; set; }
        public string CaminoId { get; set; }
        [Required(ErrorMessage = "El tipo de asentamiento es obligatorio")]
        public string TipoAsentamientoId { get; set; }
        public bool CapturarUbicacion { get; set; }
        public string BeneficiarioId { get; set; }
        public int NumDatos { get; set; }
    }
    
    public class DomiciliosIncompletosRequest{
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string Inicio { get; set; }
        public string Fin { get; set; }
        public string Id { get; set; }
        public string Folio { get; set; }
        public string MunicipioId { get; set; }
        public string LocalidadId { get; set; }
        public string ZonaImpulsoId { get; set; }
        public string EstatusDireccion { get; set; }
    } 
    
    public class DomiciliosIncompletosResponse{
        public string Inicio { get; set; }
        public string Fin { get; set; }
        public int NumDomiciliosSinCalculo { get; set; }
        public List<Model> Municipios{ get; set; }
    }

    public class DomiciliosIncompletosQueryResponse
    {
        public List<Domicilio> Domicilios { get; set; }
        public int Total { get; set; }
        public int GranTotal { get; set; }
    }

    public class ImportarDomiciliosIncompletos
    {
        [Required(ErrorMessage = "Es necesario seleccionar un archivo.")]
        [FileExtension(ErrorMessage = "El archivo a importar debe tener la extensión .xlsx")]
        public IFormFile File { get; set; }
    }
}