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
    [Table("AplicacionCarencias")]
    public class AplicacionCarencia
    {
        public string Id { get; set; }
        public string BeneficiarioId { get; set; }
        public string AplicacionId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool Educativa { get; set; }
        public bool TieneActa { get; set; }
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
        public string ParentescoId { get; set; }
        public string SexoId { get; set; }
        public bool Discapacidad { get; set; }
        public short GradoEducativa { get; set; }
        public int Edad { get; set; }
        public double Ingreso { get; set; }
        public int LineaBienestar { get; set; }
        public int VersionAlgoritmo { get; set; }
        public int NumIntegrante { get; set; }
        public string NivelPobreza { get; set; }
        public bool PerdidaEmpleo { get; set; }
        
        public virtual Beneficiario Beneficiario { get; set; }
        public virtual Aplicacion Aplicacion { get; set; }
        
        [NotMapped]
        public int NumCarencias { get; set; }
    }

    public class AplicacionCarenciaApiModel
    {
        public string Id { get; set; }
        public string BeneficiarioId { get; set; }
        public string AplicacionId { get; set; }
        public bool Educativa { get; set; }
        public bool TieneActa { get; set; }
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
        public string ParentescoId { get; set; }
        public string SexoId { get; set; }
        public bool Discapacidad { get; set; }
        public short GradoEducativa { get; set; }
        public int Edad { get; set; }
        public double Ingreso { get; set; }
        public int LineaBienestar { get; set; }
        public int VersionAlgoritmo { get; set; }
        public int NumIntegrante { get; set; }
        public string NivelPobreza { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public string DeletedAt { get; set; }
    }

    public class Cuadernillo
    {
        public string Inicio { get; set; }
        public string Fin { get; set; }
        public Dictionary<string, int> Alimentarias { get; set; }
        public Dictionary<string, int> Educativas { get; set; }
        public Dictionary<string, int> Salud { get; set; }
        public Dictionary<string, int> SeguroSocial { get; set; }
        public Dictionary<string, int> Vivienda { get; set; }
        public Dictionary<string, int> Servicios { get; set; }
        public Dictionary<string, int> Tejido { get; set; }
        public Dictionary<string, int> Pobreza { get; set; }
        public Dictionary<string, Dictionary<string, int>> Edades { get; set; }
        public Dictionary<string, int> Hacinamiento { get; set; }
        public Dictionary<string, int> Analfabetismo { get; set; }
        public Dictionary<string, int> GradoEducativa { get; set; }
        public Dictionary<string, int> NivelEducativa { get; set; }
        public Dictionary<string, int> Discapacidad { get; set; }
        public Dictionary<string, int> GradoAlimentarias { get; set; }
        public Dictionary<string, int> Piso { get; set; }
        public Dictionary<string, int> Techo { get; set; }
        public Dictionary<string, int> Muro { get; set; }
        public Dictionary<string, int> Agua { get; set; }
        public Dictionary<string, int> Drenaje { get; set; }
        public Dictionary<string, int> Electricidad { get; set; }
        public Dictionary<string, int> Combustible { get; set; }
        public Dictionary<string, int> Propiedad { get; set; }
        public Dictionary<string, int> Acta { get; set; }
        public Dictionary<string, int> Satisfaccion { get; set; }
        public Dictionary<string, int> Movilidad { get; set; }
        public Dictionary<string, int> Redes { get; set; }
        public Dictionary<string, int> Instituciones { get; set; }
        public Dictionary<string, int> Lideres { get; set; }
        public Dictionary<string, int> Publicos { get; set; }
        public Dictionary<string, int> Parque { get; set; }
        public int HacinamientoPersonas{ get; set; }
        public int AlimentariaPersonas{ get; set; }
        public int Personas{ get; set; }
        public int Viviendas{ get; set; }
        public string Ruta { get; set; }
    }
}