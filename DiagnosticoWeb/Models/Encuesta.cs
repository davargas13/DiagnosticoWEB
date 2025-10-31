using Microsoft.AspNetCore.Http;
using DiagnosticoWeb.Database;
using DiagnosticoWeb.Validaciones;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using DiagnosticoWeb.Code;

namespace DiagnosticoWeb.Models
{
    /// <summary>
    /// Clases que mapean la tabla de encuestas a objetos de persistencia, tabla que guarda el nombre y vigencia de la encuesta CONEVAL
    /// </summary>
    [Table("Encuestas")]
    public class Encuesta
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string Mensaje { get; set; }
        public int Vigencia { get; set; }
        public bool Activo { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual List<EncuestaVersion> EncuestaVersiones { get; set; }

        public static Encuesta AgregarEncuesta(ApplicationDbContext db, string nombre)
        {
            var encuesta = new Encuesta()
            {
                Nombre = nombre,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                EncuestaVersiones = new List<EncuestaVersion>()
            };
            db.Encuesta.Add(encuesta);

            return encuesta;
        }

        internal Encuesta Include()
        {
            throw new NotImplementedException();
        }

        public static Encuesta AcutalizarEncuesta(ApplicationDbContext db, Encuesta encuesta, string nombre, int vigencia,
            string mensaje)
        {
            encuesta.Nombre = nombre;
            encuesta.Mensaje = mensaje;
            encuesta.Vigencia = vigencia;
            encuesta.UpdatedAt = DateTime.Now;
            db.Encuesta.Update(encuesta);
            return encuesta;
        }
    }

    public class EncuestaResponse
    {
        public List<EncuestaModelResponse> Encuestas { get; set; }
        public int Total { get; set; }
    }

    public class EncuestaModelResponse
    {
        public string Id { get; set; }
        public bool Activo { get; set; }
        public string Nombre { get; set; }
        public int Vigencia { get; set; }
        public int Version { get; set; }
        public int Preguntas { get; set; }
    }

    public class EncuestaEditarModelList
    {
        public EncuestaEditarModel Encuesta { get; set; }
        public int Version { get; set; }
        public List<string> TiposRespuestas { get; set; }
        public List<string> TiposComplementos { get; set; }
        public List<string> Catalogos { get; set; }
        public List<CarenciaModel> Carencias { get; set; }
        public Dictionary<string, string> PreguntasHash { get; set; }
        public int UltimoIdPregunta { get; set; }
        public int UltimoIdRespuesta { get; set; }
    }

    public class EncuestaEditarModel
    {
        public string Id { get; set; }
        [Required(ErrorMessage = "El nombre de la encuesta es obligatorio.")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El fundamento de la encuesta es obligatorio.")]
        [MaxLength(1000, ErrorMessage = "El fundamento de la encuesta es obligatorio.")]
        public string Mensaje { get; set; }
        [EnsureOneElement(ErrorMessage = "Debe haber al menos una pregunta.")]
        public List<PreguntasEditarModel> Preguntas { get; set; }
        public List<PreguntasEditarModel> PreguntasInactivas { get; set; }
        public int Vigencia { get; set; }
    }

    public class ImportarEncuestaViewModel
    {
        public IFormFile File { get; set; }
    }

    public class EncuestaApiModel
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string Mensaje { get; set; }
        public int Vigencia { get; set; }
        public bool Activo { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public string DeletedAt { get; set; }
    }

    public class EncuestaSolicitudModel
    {
        public string Pregunta { get; set; }
        public string Respuesta { get; set; }
    }

    public class EncuestaValores
    {
        public int Index { get; set; }
        public List<EncuestaSolicitudModel> Preguntas { get; set; }
    }

    public class EncuestaNombreModel
    {
        [Required(ErrorMessage = "La vigencia de la encuesta es obligatoria.")]
        [Range(1, int.MaxValue, ErrorMessage = "Es requerido un número entero y mayor a 0.")]
        public string Vigencia { get; set; }

        [Required(ErrorMessage = "El nombre de la encuesta es obligatorio.")]
        [MaxLength(255, ErrorMessage = "El nombre de la encuesta debe tener como máximo 255 caracteres.")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El fundamento de la encuesta es obligatorio.")]
        [MaxLength(1000, ErrorMessage = "El fundamento de la encuesta debe tener como máximo 1000 caracteres.")]
        public string Mensaje { get; set; }
    }

    public class EncuestaResponseCreate
    {
        public string Result { get; set; }
        public string Id { get; set; }
    }

    public class EncuestaRequest
    {
        public string Nombre { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }

    public class GraficasOpcionesResponse
    {
        public string Inicio { get; set; }
        public string Fin { get; set; }
        public List<Model> Municipios { get; set; }
        public List<Model> Preguntas { get; set; }
    }    
    
    public class GraficasOpcionesRequest
    {
        public string Inicio { get; set; }
        public string Fin { get; set; }
        public string PreguntaId { get; set; }
        public string Folio { get; set; }
        public string MunicipioId { get; set; }
        public string LocalidadId { get; set; }
        public string AgebId { get; set; }
        public string ZonaImpulsoId { get; set; }
    }

    public class UsoPregunta
    {
        public string Id { get; set; }
        public int Numero { get; set; }
        public string Pregunta { get; set; }
        public bool Mostrar { get; set; }
        public bool Activa { get; set; }
        public int Total { get; set; }
        public Dictionary<string, Respuesta> Respuestas { get; set; }

    }
}
