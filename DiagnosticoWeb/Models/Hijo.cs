using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiagnosticoWeb.Models
{
    public class Hijo
    {
        public string Id { get; set; }
        public string PadreId { get; set; }
        [Required(ErrorMessage = "El primer apellido es obligatorio")]
        [MaxLength(100, ErrorMessage = "El primero apellido debe tener m�ximo 100 caracteres")]
        public string ApellidoPaterno { get; set; }
        [MaxLength(100, ErrorMessage = "El segundo apellido debe tener m�ximo 100 caracteres")]
        public string ApellidoMaterno { get; set; }
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [MaxLength(100, ErrorMessage = "El nombre debe tener m�ximo 100 caracteres")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria")]
        public DateTime? FechaNacimiento { get; set; }
        [Required(ErrorMessage = "La CURP es obligatoria")]
        [MaxLength(18, ErrorMessage = "La CURP debe contener 18 caracteres")]
        [MinLength(18, ErrorMessage = "La CURP debe contener 18 caracteres")]
        public string Curp { get; set; }
        public string Rfc { get; set; }
        public string Comentarios { get; set; }
        [Required(ErrorMessage = "El grado de estudio es obligatorio")]
        public string EstudioId { get; set; }
        [Required(ErrorMessage = "El estado civil es obligatorio")]
        public string EstadoCivilId { get; set; }
        [Required(ErrorMessage = "La discapacidad es obligatoria")]
        public string DiscapacidadId { get; set; }
        [Required(ErrorMessage = "El sexo es obligatorio")]
        public string SexoId { get; set; }
        [Required(ErrorMessage = "El estado de nacimiento es obligatorio")]
        public string EstadoId { get; set; }
        public string ParentescoId { get; set; }
        public string GradoEstudioId { get; set; }
        public string DiscapacidadGradoId { get; set; }
        public string CausaDiscapacidadId { get; set; }
        public bool MismoDomicilio { get; set; }
        public string Huellas { get; set; }
    }

    public class HijoResponse
    {
        public List<Discapacidad> Discapacidades { get; set; }
        public List<Grado> Grados { get; set; }
        public List<CausaDiscapacidad> Causas { get; set; }
        public List<Sexo> Sexos { get; set; }
        public List<Estado> Estados { get; set; }
        public List<Municipio> Municipios { get; set; }
        public List<Estudio> Estudios { get; set; }
        public List<GradosEstudio> GradosEstudios { get; set; }
        public List<EstadoCivil> EstadosCiviles { get; set; }
        public List<ImagenModel> Documentos { get; set; }
        public List<Parentesco> Parentescos { get; set; }
        public Hijo Hijo { get; set; }
        public DomicilioEditModel Domicilio { get; set; }
        public Hijo Tutor { get; set; }
        public DomicilioEditModel DomicilioTutor { get; set; }
        public List<Huella> Huellas { get; set; }
    } 

    public class DatosInformacion
    {
        public Hijo Hijo { get; set; }
        public DomicilioEditModel Domicilio { get; set; }
    }

    public class DatosInformacionTutor
    {
        public Hijo Tutor { get; set; }
        public DomicilioEditModel DomicilioTutor { get; set; }
    }
}