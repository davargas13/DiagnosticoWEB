using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DiagnosticoWeb.Models
{

    /// <summary>
    /// Clases que representan a la tabla Aplicaciones de la base de datos
    /// </summary>
    [Table("Aplicaciones")]
    public class Aplicacion
    {
        public string Id { get; set; }
        public string Estatus { get; set; }
        public string Resultado { get; set; }
        public string BeneficiarioId { get; set; }
        public string EncuestaVersionId { get; set; }
        public string Carencias { get; set; }
        public string TrabajadorId { get; set; }
        public int NumeroAplicacion { get; set; }
        public bool Activa { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public DateTime? FechaSincronizacion { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool Educativa { get; set; }
        public bool Analfabetismo { get; set; }
        public bool Inasistencia { get; set; }
        public bool PrimariaIncompleta { get; set; }
        public bool SecundariaIncompleta { get; set; }
        public bool ServicioSalud { get; set; }
        public bool SeguridadSocial { get; set; }
        public bool Vivienda { get; set; }
        public bool Piso { get; set; }
        public bool Techo { get; set; }
        public bool Muro { get; set; }
        public bool Hacinamiento { get; set; }
        public bool Servicios { get; set; }
        public bool Agua { get; set; }
        public bool Drenaje { get; set; }
        public bool Electricidad { get; set; }
        public bool Combustible { get; set; }
        public bool Alimentaria { get; set; }
        public string GradoAlimentaria { get; set; }
        public bool Pea { get; set; }
        public double Ingreso { get; set; }
        public int LineaBienestar { get; set; }
        public int VersionAlgoritmo { get; set; }
        public string NivelPobreza { get; set; }
        public bool TieneActa { get; set; }
        public short SeguridadPublica { get; set; }
        public short SeguridadParque { get; set; }
        public short ConfianzaLiderez { get; set; }
        public short ConfianzaInstituciones { get; set; }
        public short Movilidad { get; set; }
        public short RedesSociales { get; set; }
        public short Tejido { get; set; }
        public short Satisfaccion { get; set; }
        public bool PropiedadVivienda { get; set; }
        public bool Prueba { get; set; }
        public string DeviceId { get; set; }
        public int? Tiempo { get; set; }
        public string VersionAplicacion { get; set; }
        public int NumeroIntegrantes { get; set; }
        public int NumeroEducativas { get; set; }
        public int NumeroSalud { get; set; }
        public int NumeroSocial { get; set; }
        public int NumeroCarencias { get; set; }
        public int PreocupacionCovid { get; set; }
        public bool PerdidaEmpleo { get; set; }

        //Relaciones
        public virtual Beneficiario Beneficiario { get; set; }
        public virtual EncuestaVersion EncuestaVersion { get; set; }
        public virtual Trabajador Trabajador { get; set; }
        public virtual List<AplicacionPregunta> AplicacionPreguntas { get; set; }
    }

    public class AplicacionApiModel
    {
        public string Id { get; set; }
        public string Estatus { get; set; }
        public string Resultado { get; set; }
        public string BeneficiarioId { get; set; }
        public string EncuestaVersionId { get; set; }
        public string TrabajadorId { get; set; }
        public int NumeroAplicacion { get; set; }
        public bool Activa { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public bool Educativa { get; set; }
        public bool Analfabetismo { get; set; }
        public bool Inasistencia { get; set; }
        public bool PrimariaIncompleta { get; set; }
        public bool SecundariaIncompleta { get; set; }
        public bool ServicioSalud { get; set; }
        public bool SeguridadSocial { get; set; }
        public bool Vivienda { get; set; }
        public bool Piso { get; set; }
        public bool Techo { get; set; }
        public bool Muro { get; set; }
        public bool Hacinamiento { get; set; }
        public bool Servicios { get; set; }
        public bool Agua { get; set; }
        public bool Drenaje { get; set; }
        public bool Electricidad { get; set; }
        public bool Combustible { get; set; }
        public bool Alimentaria { get; set; }
        public string GradoAlimentaria { get; set; }
        public bool Pea { get; set; }
        public double Ingreso { get; set; }
        public int LineaBienestar { get; set; }
        public int VersionAlgoritmo { get; set; }
        public string NivelPobreza { get; set; }
        public bool TieneActa { get; set; }
        public short SeguridadPublica { get; set; }
        public short SeguridadParque { get; set; }
        public short ConfianzaLiderez { get; set; }
        public short ConfianzaInstituciones { get; set; }
        public short Movilidad { get; set; }
        public short RedesSociales { get; set; }
        public short Tejido { get; set; }
        public short Satisfaccion { get; set; }
        public bool PropiedadVivienda { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public string DeletedAt { get; set; }
        public string DeviceId { get; set; }
        public int? Tiempo { get; set; }
        public int NumeroIntegrantes { get; set; }
        public string VersionAplicacion { get; set; }
        public int NumeroEducativas { get; set; }
        public int NumeroSalud { get; set; }
        public int NumeroSocial { get; set; }
        public int NumeroCarencias { get; set; }
        public List<AplicacionPreguntaApiModel> AplicacionPreguntas { get; set; }
        public List<AplicacionCarenciaApiModel> AplicacionCarencias { get; set; }
    }

    public class AplicacionRespuestaCarencia
    {
        public string Id { get; set; }
        public string Carencias { get; set; }
    }

    public class AplicacionRespuestaRequest
    {
        public string asentamiento{ get; set; }
        public double indice{ get; set; }
        public float rural{ get; set; }
        public float minimarural{ get; set; }
        public float minimaurbana{ get; set; }
        public float urbana{ get; set; }
        public List<IteracionRequest> iteraciones;
    }

    public class PreguntaCarenciaRequest
    {
        public string id { get; set; }
        public List<string> valor { get; set; }

        public PreguntaCarenciaRequest(string id) {
            this.id = id;
            this.valor = new List<string>();
        }
    }

    public class IteracionRequest
    {
        public List<PreguntaCarenciaRequest> preguntas;
    }
    

    public class AplicacionResponse
    {
        public string Id { get; set; }
        public string BeneficiarioId { get; set; }
        public string Now { get; set; }
        public bool MismoDomicilio { get; set; }
        public Dictionary<int, Dictionary<int, AplicacionPreguntaResponse>> Respuestas { get; set; }
        public Dictionary<int, PreguntaEncuestaModel> Preguntas { get; set; }
        public Dictionary<string, Carencia> Carencias { get; set; }
        public Dictionary<int, Integrante> Integrantes { get; set; }
        public int TotalIntegrantes { get; set; }
        public int NumPreguntas { get; set; }
    }

    public class AplicacionRequest
    {
        public string Id { get; set; }
        public Dictionary<int, Dictionary<int, AplicacionPreguntaResponse>> Respuestas { get; set; }
    }


    public class AplicacionPreguntaResponse
    {
        public string Id { get; set; }
        public Dictionary<int, KeyValuePair<string, string>> Respuestas { get; set; }
        public Dictionary<int, KeyValuePair<string, string>> Valores { get; set; }
        public Dictionary<int, string> Grados { get; set; }
        public Dictionary<int, KeyValuePair<bool, string>> Multiples { get; set; }
    }

    public class AplicacionProductividadList
    {
        public string Id { get; set; }
        public string Estatus { get; set; }
        public string Resultado { get; set; }
        public string BeneficiarioId { get; set; }
        public string BeneficiarioNombre { get; set; }
        public string EncuestaVersionId { get; set; }
        public string TrabajadorId { get; set; }
        public int NumeroAplicacion { get; set; }
        public bool Activa { get; set; }
        public string MunicipioNombre { get; set; }
        public string LocalidadNombre { get; set; }
        public string AgebClave { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public string DeletedAt { get; set; }
        public double DiasRetraso { get; set; }
    }

    public class AplicacionShortResponse
    {
        public string Id { get; set; }
        public string Estatus { get; set; }
        public string Resultado { get; set; }
        public string BeneficiarioId { get; set; }
        public string BeneficiarioNombre { get; set; }
        public string EncuestaVersionId { get; set; }
        public string TrabajadorId { get; set; }
        public int NumeroAplicacion { get; set; }
        public bool Activa { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public string DeletedAt { get; set; }
    }

    public class Integrante
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
    }
}