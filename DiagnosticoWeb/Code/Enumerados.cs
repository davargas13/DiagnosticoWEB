using System.Collections.Generic;

namespace DiagnosticoWeb.Code
{
    /// <summary>
    /// Enumerado auxiliar para representar los tipos de preguntas admitidos en la configuracion de la encuesta CONEVAL
    /// </summary>
    public enum Llave
    {
        EncuestaTitulo,
        Numerica,
        Abierta,
        RespuestaUnica,
        RespuestaUnicaLista,
        Multiple
    }

    /// <summary>
    /// Enumerado auxiliar para representar los tipo de preguntas de la encuesta CONEVAL
    /// </summary>
    public enum TipoRespuesta
    {
        Numerica = 1, //Solo admite respuestas numericas
        Abierta = 2, //Admite texto abierto
        Radio = 3, //Es una pregunta de opcion multiple pero solo se puede seleccionar un valor
        Listado = 4, //Es una pregunta de opcion multiple pero solo se puede seleccionar un valor
        Check = 5, //Es una pregunta de opcion multiple con multiseleccion
        Fecha = 6, //Pregunta que mostrara un datepicker
        FechaPasada = 7, //Pregunta que mostrara un datepicker
        FechaFutura = 8, //Pregunta que mostrara un datepicker
        Integrantes = 9, //Pregunta que mostrara un datepicker
    }

    /// <summary>
    /// Enumerado auxiliar que sirve para identificar si una pregunta de la encuesta CONEVAL se debe aplicar a cada miembro del hogar visitado por el promotor
    /// </summary>
    public enum PreguntaIterable
    {
        ITERABLE,
        NO_ITERABLE
    }

    /// <summary>
    /// Enumerado auxiliar que sirve para identificar si alguna pregunta de la encuesta CONEVAL tiene como respuestas un catalogo interno del sistema
    /// </summary>
    public enum Catalogo
    {
        Estados, Municipios, Localidades, Agebs, ZonasImpulso, Caminos, Carreteras, TipoAsentamientos, Ocupaciones, EstadosCiviles, Estudios, GradosEstudio, Parentescos, Sexos, Discapacidades, Grados, CausaDiscapacidad
    }

    /// <summary>
    /// Enumerado auxiliar para identificar las preguntas o respuestas que tienen un complemento, es decir, que el usuario especificará mas datos en la encuesta
    /// </summary>
    public enum TipoComplemento {
        Abierta, Numerica, Listado, Catalogo, Integrante
    } 
    
    public static class TipoComplementoString {
        public const string Abierta = "Abierta";
        public const string Numerica = "Numerica";
        public const string Listado = "Listado";
        public const string Catalogo = "Catalogo";
        public const string Integrante = "Integrante";
    }

    public enum TipoPregunta
    {
        Numerica, Abierta , Radio, Listado, Check, Fecha , FechaPasada, FechaFutura,
    }

    public static class TipoPreguntaString
    {
        public const string Abierta = "Abierta";
        public const string Numerica = "Numerica";
        public const string Radio = "Radio";
        public const string Listado = "Listado";
        public const string Check = "Check";
        public const string Fecha = "Fecha";
        public const string FechaPasada = "FechaPasada";
        public const string FechaFutura = "FechaFutura";
        public const string Integrantes = "Integrantes";

        public static List<string> Seleccionables()
        {
            var seleccionables = new List<string> {Radio, Listado, Check};
            return seleccionables;
        }
        
        public static List<string> Multiples()
        {
            var multiples = new List<string> {Listado, Check};
            return multiples;
        }
    }

    public enum TipoConsulta
    {
        Aplicaciones, Cuadernillo, Domicilios, Exportar, Resumen
    }

    public class Edades
    {
        public const string MENORES = "0 a 14 años de edad";
        public const string JOVENES = "15 a 29 años";
        public const string ADULTOS = "30 a 44 años";
        public const string MADUROS = "45 a 64 años";
        public const string MAYORES = "65 años y mas";
    }

    public class GradoAlimentaria
    {
        public const string SEGURIDAD = "Seguridad alimentaria";
        public const string LEVE = "Inseguridad alimentaria leve";
        public const string MODERADA = "Inseguridad alimentaria moderada";
        public const string SEVERA = "Inseguridad alimentaria severa";
    }

    public class TipoTrabajador
    {
        public const byte ENCUESTADOR = 0;
        public const byte ARTICULADOR = 1;
    }
}