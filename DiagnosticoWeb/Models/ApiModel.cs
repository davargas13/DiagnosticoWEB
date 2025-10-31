using System;
using System.Collections.Generic;

namespace DiagnosticoWeb.Models
{
    /// <summary>
    /// Clase que mapea las tablas de la base de datos en objetos de persistencia para el proceso de sincronizacion con los dispositivos moviles
    /// </summary>
    public class ApiModel
    {
        public List<TrabajadorApiModel> Usuarios { get; set; }
        public List<TrabajadorDependenciaApiModel> UsuarioDependencias { get; set; }
        public List<TrabajadorRegionApiModel> TrabajadorRegiones { get; set; }
        public List<DependenciaApiModel> Dependencias { get; set; }
        public List<RespuestaApiModel> Respuestas { get; set; }
        public List<EncuestaApiModel> Encuestas { get; set; }
        public List<EncuestaVersionApiModel> Versiones { get; set; }
        public List<PreguntaApiModel> Preguntas { get; set; }
        public List<AplicacionApiModel> Aplicaciones { get; set; }
        public List<MunicipioApiModel> Municipios { get; set; }
        public List<MunicipioZonaApiModel> MunicipiosZonas { get; set; }
        public List<LocalidadApiModel> Localidades { get; set; }
        public List<BeneficiarioApiModel> Beneficiarios { get; set; }
        public List<DomicilioApiModel> Domicilios { get; set; }
        public List<AgebApiModel> Agebs { get; set; }
        public List<ManzanaApiModel> Manzanas { get; set; }
        public List<ColoniaApiModel> Colonias { get; set; }
        public List<CalleApiModel> Calles { get; set; }
        public List<CarreteraApiModel> Carreteras { get; set; }
        public List<CaminoApiModel> Caminos { get; set; }
        public List<DiscapacidadApiModel> Discapacidades { get; set; }
        public List<EstadoApiModel> Estados { get; set; }
        public List<EstadoCivilApiModel> EstadosCiviles { get; set; }
        public List<EstudioApiModel> Estudios { get; set; }
        public List<OcupacionApiModel> Ocupaciones { get; set; }
        public List<ParentescoApiModel> Parentescos { get; set; }
        public List<SexoApiModel> Sexos { get; set; }
        public List<ZonaImpulsoApiModel> ZonasImpulso { get; set; }
        public List<TipoAsentamientoApiModel> TiposAsentamientos { get; set; }
        public List<CarenciaApiModel> Carencias { get; set; }
        public List<PreguntaGradoApiModel> PreguntasGrados { get; set; }
        public List<RespuestaGradoApiModel> RespuestasGrados { get; set; }
        public List<CausaDiscapacidadApiModel> CausasDiscapacidades { get; set; }
        public List<DiscapacidadGradoApiModel> DiscapacidadesGrados { get; set; }
        public List<GradosEstudioApiModel> GradosEstudio { get; set; }
        public List<UnidadApiModel> Unidades { get; set; }
        public List<GradoApiModel> Grados { get; set; }
        public List<ZonaApiModel> Zonas { get; set; }
        public List<ConfiguracionApiModel> Configuraciones { get; set; }
        public List<TipoIncidenciaApiModel> TiposIncidencias { get; set; }
        public List<LineaBienestarApiModel> LineasBienestar { get; set; }
        public List<IncidenciaApiModel> Incidencias { get; set; }
        public DateTime SyncedAt { get; set; }
    }

    public class ImagesModel
    {
        public List<ArchivoApiModel> Archivos { get; set; }
    }

    public class RequestSync
    {
        public string LastSyncDate { get; set; }
        public string MunicipiosId { get; set; }
        public string LocalidadesId { get; set; }
        public string TrabajadorId { get; set; }
        public bool Geograficos { get; set; }
    }

    public class RequestBeneficiarios 
    {
        public string Beneficiario { get; set; }
        public string Hijos { get; set; }
        public string Domicilio { get; set; }
        public string DomiciliosHijos { get; set; }
        public string Aplicaciones { get; set; }
        public string Incidencias { get; set; }
    }

    public class ResponseBeneficiarios
    {
        public string Result { get; set; }
        public string Curp { get; set; }
        public string BeneficiarioId { get; set; }
        public List<Aplicacion> Aplicaciones { get; set; }
        public List<AplicacionPregunta> AplicacionesPreguntas { get; set; }
        public List<AplicacionCarencia> AplicacionesCarencias { get; set; }
    }

    public class ResponseGeograficos
    {
        public List<EstadoApiModel> Estados { get; set; }
        public List<MunicipioApiModel> Municipios { get; set; }
        public List<LocalidadApiModel> Localidades { get; set; }
        public List<DependenciaApiModel> Dependencias { get; set; }
        public List<ZonaApiModel> Zonas { get; set; }
        public List<MunicipioZonaApiModel> MunicipiosZonas { get; set; }
    }

    public class ResponseManzanas
    {
        public List<AgebApiModel> Agebs { get; set; } 
        public List<ManzanaApiModel> Manzanas { get; set; }
        public List<ColoniaApiModel> Colonias { get; set; }
        public List<CalleApiModel> Calles { get; set; }
    }

    public class VersionAlgoritmoResponse
    {
        public string Version { get; set; }
        
    }
}