using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DiagnosticoWeb.Models
{
    public class Diagnostico
    {
        public string Token { get; set; }
        public string Inicio { get; set; }
        public string Fin { get; set; }
        public string Municipio { get; set; }
        public string Localidad { get; set; }
        public string Ageb { get; set; }
        public string Manzana { get; set; }
        public string CodigoPostal { get; set; }
        public string Colonia { get; set; }
        public string Familia { get; set; }
        public string Encuestador { get; set; }
    }

    public class DiagnosticoRequest
    {
        public string Municipio { get; set; }
        public string Localidad { get; set; }
        public string Ageb { get; set; }
        public string Manzana { get; set; }
        public string CodigoPostal { get; set; }
        public string Colonia { get; set; }
        public string BeneficiarioId { get; set; }
    }

    public class VerRutasResponse
    {
        public string Inicio { get; set; }
        public string Fin { get; set; }
        public List<DependenciaApiModel> Dependencias { get; set; }
        public List<MunicipioApiModel> Municipios { get; set; }
    }

    public class GetTrabajadoresResponse
    {
        public List<TrabajadorList> Trabajadores { get; set; }
    }

    public class GetRutasRequest
    {
        public string TrabajadorId { get; set; }
        public string MunicipiosId { get; set; }
        public string LocalidadesId { get; set; }
        public string AgebsId { get; set; }
        public string Inicio { get; set; }
        public string Fin { get; set; }
    }
    
    public class RutasDomicilioModel
    {
        public string DomicilioId { get; set; }
        public string Ageb { get; set; }
        public string Domicilio { get; set; }
        public string EntreCalle1 { get; set; }
        public string EntreCalle2 { get; set; }
        public string Latitud { get; set; }
        public string Longitud { get; set; }
        public string CodigoPostal { get; set; }
        public string Municipio { get; set; }
        public string Localidad { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public BeneficiarioShortResponse Beneficiario { get; set; }
    }

    public class GetRutasResponse
    {
        public string Status { get; set; }
        public List<RutasDomicilioModel> Domicilios { get; set; }
        public int Total { get; set; }
    }

    public class GetProductividadRequest
    {
        public string DependenciaId { get; set; }
        public string MunicipiosId { get; set; }
        public string LocalidadesId { get; set; }
        public string AgebsId { get; set; }
        public string Inicio { get; set; }
        public string Fin { get; set; }
    }

    public class GetProductividadResponse
    {
        public string Status { get; set; }
        public List<TrabajadorProductividadList> Trabajadores { get; set; }
        public int Total { get; set; }
    }
    
    public class TrabajadorProductividadList
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public int Total { get; set; }
        public int Completas { get; set; }
        public int Incompletas { get; set; }
        public double DiasRetraso { get; set; }
        public bool IsOpened { get; set; }
        public TimeSpan Promedio { get; set; }
        public List<TimeSpan> Tiempos { get; set; }
        public List<AplicacionProductividadList> Aplicaciones { get; set; }
    }

    public class VerEstadisticasResponse
    {
        public List<DependenciaApiModel> Dependencias { get; set; }
        public List<MunicipioApiModel> Municipios { get; set; }
    }

    public class GetAvancesPerfilRequest
    {
        public string DependenciasId { get; set; }
        public string TrabajadoresId { get; set; }
        public string MunicipiosId { get; set; }
        public string LocalidadesId { get; set; }
        public string AgebsId { get; set; }
        public string Inicio { get; set; }
        public string Fin { get; set; }
    }

    public class GetAvancesPerfilResponse
    {
        public string Status { get; set; }
        
        public List<EstadisticaModel> Aplicaciones { get; set; }
        public int TotalGeneral { get; set; }
        public int TotalCompletas { get; set; }
        public int TotalIncompletas { get; set; }
        public Dictionary<string, List<int>> CompletadosList { get; set; }
        public Dictionary<string, Double> PromediosList { get; set; }
        public Dictionary<string, double> DiasList { get; set; }
    }
    
    public class GetEstadisticaRequest
    {
        public string DependenciaId { get; set; }
        public string MunicipiosId { get; set; }
        public string LocalidadesId { get; set; }
        public string AgebsId { get; set; }
        public string Inicio { get; set; }
        public string Fin { get; set; }
    }

    public class DatosDiarios
    {
        public string Fecha { get; set; }
        public int Completas { get; set; }
        public int Incompletas { get; set; }
        public int Totales { get; set; }
    }

    public class GetEstadisticasResponse
    {
        public string Status { get; set; }
        
        public List<EstadisticaModel> Aplicaciones { get; set; }
        public int TotalGeneral { get; set; }
        public int TotalCompletas { get; set; }
        public int TotalIncompletas { get; set; }
        public int EncuestadosCount { get; set; }
        public int FamiliasCount { get; set; }
        public TimeSpan EncuestasPromedio { get; set; }
        public double DiasPromedio { get; set; }
        public Dictionary<string, List<int>> EncuestadoresList { get; set; }
        public Dictionary<string, int> CarenciasList { get; set; }
        public List<DatosDiarios> DatosDiarios { get; set; }
    }

    public class EstadisticaModel
    {
        public AplicacionApiModel Aplicacion { get; set; }
        public BeneficiarioShortResponse Beneficiario { get; set; }
        public DomicilioResponse Domicilio { get; set; }
        public MunicipioApiModel Municipio { get; set; }
        public LocalidadApiModel Localidad { get; set; }
        public TrabajadorApiModel Trabajador { get; set; }
        // public TrabajadorDependencia TrabajadorDependencia { get; set; }
        public DependenciaApiModel Dependencia { get; set; } 
    }

    public class DiagnosticoResponse
    {
        public string Inicio { get; set; }
        public string Fin { get; set; }
        public string Hoy { get; set; }
        public List<Model> Municipios { get; set; }
        public List<Model> Encuestadores { get; set; }
    }

    public class IngresarSabanaRequest {       
        [Required(ErrorMessage = "La fecha de inicio es obligatoria.")]
        public string Inicio { get; set; }
        [Required(ErrorMessage = "La fecha de término es obligatoria.")]
        public string Fin { get; set; }
    }
}