using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;

namespace DiagnosticoWeb.Models
{
    /// <summary>
    /// Clases que mapean la tabla beneficiarios a modelos de persistencia, tabla que guarda los datos personales de los beneficiarios
    /// </summary>
    [Table("Beneficiarios")]
    public class Beneficiario
    {
        public string Id { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string Nombre { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string SexoId { get; set; }
        public string EstadoId { get; set; }
        public string Curp { get; set; }
        public string Rfc { get; set; }
        public string Comentarios { get; set; }
        public string EstudioId { get; set; }
        public string EstadoCivilId { get; set; }
        public string DiscapacidadId { get; set; }
        public string TrabajadorId { get; set; }
        public string EstatusInformacion { get; set; }
        public DateTime EstatusUpdatedAt { get; set; }
        public string PadreId { get; set; }
        public string HijoId { get; set; }
        public bool MismoDomicilio { get; set; }
        public string ParentescoId { get; set; }
        public string GradoEstudioId { get; set; }
        public string CausaDiscapacidadId { get; set; }
        public string DiscapacidadGradoId { get; set; }
        public bool Estatus { get; set; }
        public string Folio { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string Huellas { get; set; }
        public int NumIntegrante { get; set; }
        public int NumFamilia { get; set; }
        public string DomicilioId { get; set; }
        public int? EstatusVisita { get; set; }
        public bool Prueba { get; set; }
        public string DeviceId { get; set; }
        public string VersionAplicacion { get; set; }
        //Relaciones
        public virtual Estado Estado { get; set; }
        public virtual List<Aplicacion> Aplicaciones { get; set; }
        public virtual Estudio Estudio { get; set; }
        public virtual EstadoCivil EstadoCivil { get; set; }
        public virtual Discapacidad Discapacidad { get; set; }
        public virtual Trabajador Trabajador { get; set; }
        public virtual Sexo Sexo { get; set; }
        public virtual Parentesco Parentesco { get; set; }
        public virtual GradosEstudio GradoEstudio { get; set; }
        public virtual DiscapacidadGrado DiscapacidadGrado { get; set; }
        public virtual CausaDiscapacidad CausaDiscapacidad { get; set; }
        public virtual List<Solicitud> Solicitudes { get; set; }
        public virtual Beneficiario Padre { get; set; }
        public virtual Domicilio Domicilio { get; set; }
        
        [ForeignKey("PadreId")]
        public virtual List<Beneficiario> Hijos { get; set; }
        [NotMapped]
        public int NumSolicitudes { get; set; }
        [NotMapped]
        public string DomicilioString { get; set; }
        [NotMapped]
        public string Referencia { get; set; }
        [NotMapped]
        public int Edad { get; set; } 
        [NotMapped]
        public Aplicacion Aplicacion { get; set; } //Variable auxiliar para actualizar la aplicacion del beneficiario recien creado
    }

    public class BeneficiarioApiModel
    {
        public string Id { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string Nombre { get; set; }
        public string NombreCompleto { get; set; }
        public string FechaNacimiento { get; set; }
        public string SexoId { get; set; }
        public string EstadoId { get; set; }
        public string Curp { get; set; }
        public string Rfc { get; set; }
        public string Comentarios { get; set; }
        public string EstudioId { get; set; }
        public string EstadoCivilId { get; set; }
        public string DiscapacidadId { get; set; }
        public string TrabajadorId { get; set; }
        public string EstatusInformacion { get; set; }
        public string EstatusUpdatedAt { get; set; }
        public string PadreId { get; set; }
        public string HijoId { get; set; }
        public bool MismoDomicilio { get; set; }
        public string ParentescoId { get; set; }
        public string GradoEstudioId { get; set; }
        public string CausaDiscapacidadId { get; set; }
        public string DiscapacidadGradoId { get; set; }
        public string DomicilioId { get; set; }
        public int? EstatusVisita { get; set; }
        public bool Estatus { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public string DeletedAt { get; set; }
        public string Huellas { get; set; }
        public string Folio { get; set; }
        public int NumIntegrante { get; set; }
        public int NumFamilia { get; set; }
        public string DeviceId { get; set; }
        public string VersionAplicacion { get; set; }
    }

    public class ReponseBeneficiarioIndex
    {
        public string Inicio { get; set; }
        public string Fin { get; set; }
        public List<KeyValuePair<string, List<KeyValuePair<string, bool>>>> Filtros { get; set; }
        public Dictionary<string, List<Model>> Catalogos { get; set; }
    }
    
    public class BeneficiariosResponse
    {
        public List<BeneficiarioShortResponse> Beneficiarios { get; set; }
        public int Total { get; set; }
        public int GranTotal { get; set; }
    }
    
    public class BeneficiariosRequest
    {
        public string Valores { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string Inicio { get; set; }
        public string Fin { get; set; }
        public bool Hijos { get; set; }
        public string Tipo { get; set; }
        public string Estatus { get; set; } // Agregado para filtrar por estatus

    }

    public class BeneficiarioResponse
    {
        public BeneficiarioResponseModel Beneficiario { get; set; }
        public DomicilioResponse Domicilio { get; set; }
        public List<SolicitudesListModel> Solicitudes { get; set; }
        public List<AplicacionPregunta> Preguntas { get; set; }
        public Aplicacion Aplicacion { get; set; }
        public int NumDomicilios { get; set; }
        public int NumEncuestas { get; set; }
        public int NumIntegrantes { get; set; }
        public List<AplicacionCarencia> Carencias { get; set; }
        public List<DiscapacidadIntegrante> Discapacidades { get; set; }
        public List<ImagenModel> Documentos { get; set; }
        public List<Huella> Huellas { get; set; }
        public List<ImagenModel> Fotos { get; set; }
        public List<BeneficiarioResponseModel> Hijos { get; set; }
        public List<List<BeneficiarioResponseModel>> Hojas { get; set; }
        public string Ruta { get; set; }
        public string Dependencia { get; set; }
        public string FamiliaId { get; set; }
        public string Qr { get; set; }
        public string VersionEncuestaActual { get; set; }
        public List<AplicacionPregunta> Asiste { get; set; }
    }

    public class BeneficiarioResponseModel
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string Curp { get; set; }
        public string Rfc { get; set; }
        public string FechaNacimiento { get; set; }
        public string Estado { get; set; }
        public string Estudio { get; set; }
        public string GradoEstudio { get; set; }
        public string EstadoCivil { get; set; }
        public string Discapacidad { get; set; }
        public string GradoDiscapacidad { get; set; }
        public string CausaDiscapacidad { get; set; }
        public string Parentesco { get; set; }
        public int NumIntegrante { get; set; }
        public bool MismoDomicilio { get; set; }
        public string DomicilioString { get; set; }
        public string Comentarios { get; set; }
        public string Estatus { get; set; }
        public string EstatusInformacion { get; set; }
        public string Sexo { get; set; }
        public bool Activa { get; set; }
        public string Folio { get; set; }
    }

    public class VerDatosRequest
    {
        public int Numero { get; set; }
        public string BeneficiarioId { get; set; }
    }

    public class BeneficiarioEdit
    {
        public string FamiliaId { get; set; }
        public BeneficiarioEditModel Beneficiario { get; set; }
        public List<Estado> Estados { get; set; }
        public List<Estudio> Estudios { get; set; }
        public List<GradosEstudio> GradosEstudio { get; set; }
        public List<EstadoCivil> EstadosCiviles { get; set; }
        public List<Discapacidad> Discapacidades { get; set; }
        public List<CausaDiscapacidad> CausaDiscapacidad { get; set; }
        public List<Sexo> Sexos { get; set; }
    }
    
    public class BeneficiarioEditModel
    {
        public string Id { get; set; }
        [Required]
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public DateTime FechaNacimiento { get; set; }
        [Required]
        public string Curp { get; set; }
        public string Rfc { get; set; }
        public string Comentarios { get; set; }
        [Required]
        public string EstudioId { get; set; }
        public string GradoEstudioId { get; set; }
        [Required]
        public string EstadoCivilId { get; set; }
        [Required]
        public string DiscapacidadId { get; set; }
        public string DiscapacidadGradoId { get; set; }
        public string CausaDiscapacidadId { get; set; }
        [Required]
        public string SexoId { get; set; }
        [Required]
        public string EstadoId { get; set; }
        [Required]
        public bool Estatus { get; set; }
        public string Folio { get; set; }
        public string PadreId { get; set; }
        public int NumDatos { get; set; }
        public int NumIntegrante { get; set; }
    }

    public class Huella
    {
        public string archivoId { get; set; }
        public int dedo { get; set; }
        public int status { get; set; }
        public string nombre { get; set; }
    }

    public class BeneficiarioShortResponse
    {
        public string Id { get; set; }
        public string Folio { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string EstatusInformacion { get; set; }
        public string CreatedAt { get; set; }
        public string ZonaImpulso { get; set; }
        public string Municipio { get; set; }
        public string Localidad { get; set; }
        public string FechaRegistro { get; set; }
        public string Latitud { get; set; }
        public string Longitud { get; set; }
    }
}