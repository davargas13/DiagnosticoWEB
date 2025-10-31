using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiagnosticoWeb.Code
{
    /// <summary>
    /// Enumerado auxiliar que representa el estatus de captura de un aspirante a beneficiario
    /// </summary>
    public class EstatusInformacion
    {
        public static string DATOS_PERSONALES = "Datos personales"; //Solo se cuenta con los datos personales del beneficiario
        public static string DOMICILIO = "Domicilio";//Se cuenta con los datos personales y el domicilio del beneficiario
        public static string ENCUESTA = "Encuesta"; //Se cuenta con los datos personales, domicilio y encuesta aplicada
        public static string SIN_CARENCIAS = "Sin carencias"; //Se aplico la encuesta pero el algoritmo CONEVAL no encontro carencias
        public static string NO_INTERESADO = "Desinteresado"; //Se encontraron carencias pero al aspirante no le interesa ningun apoyo
        public static string SIN_VERTIENTES = "Sin vertientes";//La dependencia no puede ofrecer apoyos porque aunque hay carencias no hay apoyos para esas carencias
        public static string SOLICITUD = "Solicitud"; //Se solicito un apoyo
    }
}
