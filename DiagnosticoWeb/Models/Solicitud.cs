using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DiagnosticoWeb.Models
{
    /// <summary>
    /// Clases que mapean la tabla de solicitudes a objetos de persistencia, tabla que guarda las solicitudes de apoyos de los beneficiarios o aspirantes
    /// </summary>
    [Table("Solicitudes")]
    public class Solicitud
    {
        public string Id { get; set; }
        public string BeneficiarioId { get; set; }
        public string TrabajadorId { get; set; }
        public string VertienteId { get; set; }
        public string AplicacionId { get; set; }
        public string DomicilioId { get; set; }
        public double Cantidad { get; set; }
        public double Costo { get; set; }
        public double Economico { get; set; }
        public string Estatus { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual Beneficiario Beneficiario { get; set; }
        public virtual Trabajador Trabajador { get; set; }
        public virtual Vertiente Vertiente { get; set; }
        public virtual Aplicacion Aplicacion { get; set; }
        public virtual Domicilio Domicilio { get; set; }
    }

    public class SolicitudApiModel
    {
        public string Id { get; set; }
        public string BeneficiarioId { get; set; }
        public string TrabajadorCampoId { get; set; }
        public string VertienteId { get; set; }
        public string AplicacionId { get; set; }
        public string DomicilioId { get; set; }
        public double Cantidad { get; set; }
        public double Costo { get; set; }
        public double Economico { get; set; }
        public string Estatus { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public string DeletedAt { get; set; }
    }

    public class SolicitudesListModel
    {
        public string Beneficiario { get; set; }
        public string ProgramaSocial { get; set; }
        public string SolicitudId { get; set; }
        public string Vertiente { get; set; }
        public string Unidad { get; set; }
        public double Cantidad { get; set; }
        public double Costo { get; set; }
        public double Economico { get; set; }
        public string Estatus { get; set; }
        public string Trabajador { get; set; }
        public DateTime CreatedAt { get; set; }

        public DomicilioResponse Domicilio { get; set; }
        public string Latitud { get; set; }
        public string Longitud { get; set; }
        public string DomicilioN { get; set; }
        public bool Ver { get; set; }
    }

    public class SolicitudesRequest
    {
        public string Valores { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string Inicio { get; set; }
        public string Fin { get; set; }
    }

    public class SolicitudesResponse
    {
        public List<SolicitudesListModel> Solicitudes { get; set; }
        public int Total { get; set; }
        public int GranTotal { get; set; }
    }

    public class VerSolicitudModel
    {
        public SolicitudesListModel Solicitud { get; set; }
        public List<EncuestaSolicitudModel> Preguntas { get; set; }
        public List<ImagenModel> Imagenes { get; set; }
        public List<ImagenModel> Documentos { get; set; }
        public List<Carencia> Carencias { get; set; }
    }

    public class ImagenModel
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string Base64 { get; set; }
        public bool Show { get; set; }
    }

    public class ResponseSolicitudIndex
    {
        public Dictionary<string, List<KeyValuePair<string, bool>>> Filtros { get; set; }

        public Dictionary<string, List<Model>> Catalogos { get; set; }
        public string Inicio { get; set; }
        public string Fin { get; set; }
    }

    public class Filtro<T>
    {
        public List<T> Filtros { get; set; }
    }

    public class DictaminacionModel
    {
        public string Id { get; set; }
    }

    public class SolicitudIndex
    {
        public KeyValuePair<string, bool> Nombre;
        public KeyValuePair<string, bool> Sexo;
        public KeyValuePair<string, bool> Curp;
        public KeyValuePair<string, bool> Rfc;
        public KeyValuePair<string, bool> Municipio;
        public KeyValuePair<string, bool> Localidad;
    }

    public class Dashboard
    {
        public string Inicio { get; set; }
        public string Fin { get; set; }
        public List<Model> MunicipiosFiltro { get; set; }
        public List<Model> DependenciasFiltro { get; set; }
        public Dictionary<string, int> Dependencias { get; set; }
        public Dictionary<string, int> Carencias { get; set; }
        public Dictionary<string, int> Niveles { get; set; }
        public Dictionary<string, int> Trabajadores { get; set; }
        public Dictionary<string, int> Estatus { get; set; }
        public int NumDomicilios { get; set; }
        public int NumDiagnosticos { get; set; }
        public int NumPendientes{ get; set; }
        public double PromedioCaptura{ get; set; }
    }

    public class DashboardRequest
    {
        public string Inicio { get; set; }
        public string Fin { get; set; }
        public string MunicipioId { get; set; }
        public string DependenciaId { get; set; }

    }
}