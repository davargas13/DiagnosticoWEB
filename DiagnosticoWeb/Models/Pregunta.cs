using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DiagnosticoWeb.Validaciones;

namespace DiagnosticoWeb.Models
{
    /// <summary>
    /// Clases que mapean la tabla de preguntas a objetos de persistencia, tabla que guarda la configuracion de cada pregunta de la encuesta CONEVAL
    /// </summary>
    [Table("Preguntas")]
    public class Pregunta
    {
        public string Id { get; set; }
        public string Nombre  { get; set; }
        public string TipoPregunta { get; set; }
        public string Condicion { get; set; }
        public bool CondicionLista { get; set; }
        public string CondicionIterable { get; set; }
        public bool Iterable { get; set; }
        public string EncuestaVersionId { get; set; }
        public int Numero { get; set; }
        public bool Editable { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string Catalogo { get; set; }
        public bool Gradual { get; set; }
        public string CarenciaId { get; set; }
        public string Expresion { get; set; }
        public string ExpresionEjemplo { get; set; }
        public bool EsNombre { get; set; }
        public string Complemento { get; set; }
        public string TipoComplemento { get; set; }
        public string Variable { get; set; }
        public bool Obligatoria { get; set; }
        public bool SeleccionarRespuestas { get; set; }
        public bool Activa { get; set; }
        public bool Excluir { get; set; }
        public int Maximo { get; set; }
        public virtual EncuestaVersion EncuestaVersion { get; set; }
        public virtual List<Respuesta> Respuestas { get; set; }
        public virtual List<PreguntaGrado> PreguntaGrados { get; set; }
        public virtual Carencia Carencia { get; set; }
        [NotMapped] 
        public bool Mostrar { get; set; }
        [NotMapped] 
        public virtual Dictionary<string, Respuesta> RespuestasMap { get; set; }

    }

    public class PreguntasEditarModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El tipo de pregunta es obligatorio.")]
        public string TipoPregunta { get; set; }
        public string Condicion { get; set; }
        public bool CondicionLista { get; set; }
        public bool TipoCondicionalAdicional { get; set; }
        public string CondicionIterable { get; set; }
        public bool Iterable { get; set; }
        public int Numero { get; set; }
        public bool Editable { get; set; }
        [RequiredIf("IsCatalogo",true)]
        public string Catalogo { get; set; }
        public bool IsCatalogo { get; set; }
        public bool IsAbierta { get; set; }
        public bool IsGradual { get; set; }
        public bool Gradual { get; set; }
        [RequiredIf("IsAbierta",true)]

        public string Expresion { get; set; }
        [RequiredIf("IsAbierta",true)]
        public string ExpresionEjemplo { get; set; }
        public List<RespuestasEditarModel> Respuestas { get; set; }
        public List<PreguntaGradoCreateModel> PreguntaGrados { get; set; }
        public string CarenciaId { get; set; }
        public string Carencia { get; set; }
        public string Color { get; set; }
        public string TipoComplemento { get; set; }
        public string Complemento { get; set; }
        public bool Activa { get; set; }
        public bool Obligatoria { get; set; }
        public bool Numerica { get; set; }
        public bool Excluir { get; set; }
        public bool SeleccionarRespuestas { get; set; }
        public int Maximo { get; set; }
        public bool Nueva { get; set; }
        public List<string> CondicionIds { get; set; }
        public Dictionary<string, string> Condiciones { get; set; }
        public Dictionary<string, string[]> CondicionRespuestas { get; set; }
        public Dictionary<string, string[]> CondicionOperadores { get; set; }
    }

    public class PreguntaValidationModel
    {
        [Required(ErrorMessage = "El id de la pregunta es obligatorio.")]
        [Range(1,9999, ErrorMessage = "El Id debe ser mayor a 0 y menor a 9999")]
        public int Id { get; set; }
        [Required(ErrorMessage = "El número de la pregunta es obligatorio.")]
        [Range(1,9999, ErrorMessage = "El Número debe ser mayor a 0 y menor a 9999")]
        public int Numero { get; set; }
        [Required(ErrorMessage = "El nombre de la pregunta es obligatorio.")]
        [MaxLength(255, ErrorMessage = "El nombre de la pregunta debe tener como máximo 255 caracteres.")]
        public string Nombre { get; set; }
    }
    
    public class PreguntaRespuestasValidationModel
    {
        [Required(ErrorMessage = "El tipo de pregunta es obligatorio.")]
        public bool IsCatalogo { get; set; }
        public string TipoPregunta { get; set; }
        [RequiredIf("IsCatalogo", true)]
        public string Catalogo { get; set; }
        public string TipoComplemento { get; set; }
        public string Complemento { get; set; }
        public List<PreguntaGradoCreateModel> PreguntaGrados { get; set; }
        public List<RespuestaValidationModel> Respuestas { get; set; }
    }

    public class RespuestaValidationModel
    {
        [Required(ErrorMessage = "El id de la respuesta es obligatorio.")]
        [Range(1,9999, ErrorMessage = "El Id debe ser mayor a 0 y menor a 9999")]
        public int Id { get; set; }
        [Required(ErrorMessage = "El nombre de la respuesta es obligatorio.")]
        public string Nombre { get; set; }
        public bool Nueva { get; set; }
        public int Numero { get; set; }
        public string TipoComplemento { get; set; }
        public string Complemento { get; set; }

        public List<RespuestaGradoCreateModel> RespuestaGrados { get; set; }
    }

    public class PreguntaApiModel
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string TipoPregunta { get; set; }
        public string Condicion { get; set; }
        public bool CondicionLista { get; set; }
        public string CondicionIterable { get; set; }
        public bool Iterable { get; set; }
        public string EncuestaVersionId { get; set; }
        public int Numero { get; set; }
        public bool Editable { get; set; }
        public string Catalogo { get; set; }
        public bool Gradual { get; set; }
        public string Expresion { get; set; }
        public string ExpresionEjemplo { get; set; }
        public string CarenciaId { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public string DeletedAt { get; set; }
        public bool EsNombre { get; set; }
        public string Complemento { get; set; }
        public string TipoComplemento { get; set; }
        public bool Activa { get; set; }
        public bool Obligatoria { get; set; }
        public bool SeleccionarRespuestas { get; set; }
        public int Maximo { get; set; }
    }

    public class PreguntaEncuestaModel
    {
        public string Id { get; set; } 
        public string Nombre { get; set; } 
        public string Condicion { get; set; } 
        public bool Iterable { get; set; } 
        public string CondicionIterable { get; set; } 
        public bool CondicionLista { get; set; } 
        public string Catalogo { get; set; } 
        public int Numero { get; set; } 
        public bool Gradual { get; set; } 
        public string CarenciaId { get; set; } 
        public string TipoPregunta { get; set; } 
        public string Expresion { get; set; } 
        public string ExpresionEjemplo { get; set; } 
        public List<RespuestaEncuestaModel> Respuestas { get; set; } 
        public List<PreguntaGrado> PreguntaGrados { get; set; } 
        public bool Mostrar { get; set; }
        public bool SeleccionarRespuestas { get; set; }
        public string TipoComplemento { get; set; }
        public string Complemento { get; set; }
        public int Maximo { get; set; }
    }
}
