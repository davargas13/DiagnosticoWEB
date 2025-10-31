using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using DiagnosticoWeb.Database;
using DiagnosticoWeb.Models;

namespace DiagnosticoWeb.Code
{
    public class BeneficiarioCode
    {
        private readonly ApplicationDbContext _context;
        private List<string> PreguntasIntegranteCero = new List<string> { "132", "133", "134", "135", "136","968" };

        public BeneficiarioCode(ApplicationDbContext context)
        {
            _context = context;
        }

        public static string GetNombreDedo(int dedo)
        {
            switch (dedo)
            {
                case 0:
                    return "Pulgar derecho";
                case 1:
                    return "Índice derecho";
                case 2:
                    return "Medio derecho";
                case 3:
                    return "Anular derecho";
                case 4:
                    return "Meñique izquierdo";
                case 5:
                    return "Pulgar izquierdo";
                case 6:
                    return "Índice izquierdo";
                case 7:
                    return "Medio izquierdo";
                case 8:
                    return "Anular izquierdo";
                case 9:
                    return "Meñique izquierdo";
             default:
                 return "";
            }
        }
        
        public KeyValuePair<string, List<string>> BuildBeneficiariosQuery(BeneficiariosRequest request, TipoConsulta tipoConsulta,
            bool hijos, string inicio, string fin, bool total)
        {
            var i = 2;
            var campos = new List<string> {total ? "count(B.Id)" : "B.Id"};
            var query = " from Beneficiarios B left join Domicilio D on D.Id = B.DomicilioId ";
            var inners = "";
            var where = " where B.DeletedAt is null "+( hijos ? "" : " and B.PadreId is null ")+" and B.HijoId is null and B.Prueba=0";
            switch (tipoConsulta)
            {
                case TipoConsulta.Aplicaciones:
                case TipoConsulta.Cuadernillo:
                case TipoConsulta.Resumen:
                    if (!string.IsNullOrEmpty(inicio))
                    {
                        where += " and A.FechaInicio >= @0";
                    }
                    if (!string.IsNullOrEmpty(fin))
                    {
                        where += " and A.FechaInicio <= @1";
                    }
                    where += " and A.DeletedAt is null and D.TrabajadorId <> '120989e6-f6b7-453b-83af-fd4ccc0639b6'";
                    break;
                default:
                    if (!string.IsNullOrEmpty(inicio))
                    {
                        where += " and B.CreatedAt >= @0";
                    }
                    if (!string.IsNullOrEmpty(fin))
                    {
                        where += " and B.CreatedAt <= @1";
                    }
                    break;
            }
            var valores = new List<string>();
            if (!string.IsNullOrEmpty(inicio))
            {
                valores.Add(inicio);
            }
            if (!string.IsNullOrEmpty(fin))
            {
                valores.Add(fin);
            }
            var tablas = new Dictionary<string, string>();
            var json = request.Valores == null ? JsonConvert.DeserializeObject(JsonConvert.Null) : JsonConvert.DeserializeObject(request.Valores);
            var domicilioProperties = typeof(Domicilio).GetProperties();
            var beneficiarioProperties = typeof(Beneficiario).GetProperties();
            var localidadProperties = typeof(Localidad).GetProperties();
            var municipioProperties = typeof(Municipio).GetProperties();
            var aplicacionProperties = typeof(Aplicacion).GetProperties();

            var domicilioDictionary = Query.BuildPropertyDictionary(domicilioProperties);
            var beneficiarioDictionary = Query.BuildPropertyDictionary(beneficiarioProperties);
            var localidadDictionary = Query.BuildPropertyDictionary(localidadProperties);
            var municipioDictionary = Query.BuildPropertyDictionary(municipioProperties);
            var aplicacionDictionary = Query.BuildPropertyDictionary(aplicacionProperties);
            var preguntasDictionary = new Dictionary<string, Pregunta>();
            
            var encuesta = _context.Encuesta.Where(e => e.Activo).Include(e => e.EncuestaVersiones)
                .ThenInclude(ev => ev.Preguntas).ThenInclude(p => p.Respuestas).First();
            foreach (var pregunta in encuesta.EncuestaVersiones.First(ev => ev.Activa).Preguntas.Where(p=>p.Activa))
            {
                preguntasDictionary.Add(pregunta.Numero + ".-" + pregunta.Nombre.Replace(" ", "").ToLower(), pregunta);
            }

            switch (tipoConsulta)
            {
                case TipoConsulta.Resumen:
                    tablas.Add("A", " inner join Aplicaciones A on B.Id = A.BeneficiarioId ");
                    //tablas.Add("S", " inner join Sabana S on B.Id = S.id_fam ");
                    var tieneFiltros = false;
                    if (json != null)
                    {
                        foreach (var item in (JObject)json)
                        {
                            if (item.Value.ToString().Length > 0)
                            {
                                if (preguntasDictionary.ContainsKey(item.Key.ToLower()))
                                {
                                    tieneFiltros = true;
                                    break;
                                }
                            }
                        }
                    }
                    if (tieneFiltros) {
                        tablas.Add("AP", " inner join AplicacionPreguntas AP on AP.AplicacionId = A.Id ");
                        tablas.Add("P", " inner join Preguntas P on P.Id = AP.PreguntaId ");
                    }
                    break;
                case TipoConsulta.Aplicaciones:
                    tablas.Add("A", " inner join Aplicaciones A on B.Id = A.BeneficiarioId ");
                    tablas.Add("AP", " inner join AplicacionPreguntas AP on AP.AplicacionId = A.Id ");
                    tablas.Add("P", " inner join Preguntas P on P.Id = AP.PreguntaId ");
                    tablas.Add("TR", " inner join Trabajadores TR on TR.Id = A.TrabajadorId ");
                    break;
                case TipoConsulta.Cuadernillo:
                    tablas.Add("A", " inner join Aplicaciones A on B.Id = A.BeneficiarioId and A.activa = 1");
                    tablas.Add("AC", " inner join AplicacionCarencias AC on A.Id = AC.AplicacionId ");
                    tablas.Add("S", " inner join Sabana S on B.Id = S.id_fam ");
                    break;
                case TipoConsulta.Exportar: 
                    tablas.Add("M", " left join Municipios M on M.Id = D.MunicipioId ");
                    tablas.Add("L", " left join Localidades L on L.Id = D.LocalidadId ");
                    tablas.Add("CO", " left join Colonias CO on CO.Id = D.ColoniaId ");
                    tablas.Add("CA", " left join Calles CA on CA.Id = D.CalleId ");
                    tablas.Add("ZI", " left join ZonasImpulso ZI on ZI.Id = D.zonaImpulsoId ");
                    tablas.Add("AG", " left join Agebs AG on AG.Id = D.AgebId ");
                    tablas.Add("MA", " left join Manzanas MA on MA.Id = D.ManzanaId ");
                    tablas.Add("CR", " left join Carreteras CR on CR.Id = D.CarreteraId ");
                    tablas.Add("CM", " left join Caminos CM on CM.Id = D.CaminoId ");
                    tablas.Add("TA", " left join TipoAsentamientos TA on TA.Id = D.TipoAsentamientoId ");
                    tablas.Add("P", " left join Parentescos P on P.Id = B.ParentescoId ");
                    tablas.Add("E", " left join Estados E on E.Id = B.EstadoId ");
                    tablas.Add("SX", " left join Sexos SX on SX.Id = B.SexoId ");
                    tablas.Add("EC", " left join EstadosCiviles EC on EC.Id = B.EstadoCivilId ");
                    tablas.Add("ES", " left join Estudios ES on ES.Id = B.EstudioId ");
                    tablas.Add("GE", " left join GradosEstudio GE on GE.Id = B.GradoEstudioId ");
                    tablas.Add("TR", " inner join Trabajadores TR on TR.Id = D.TrabajadorId ");
                    tablas.Add("SB", " left join Sabana SB on SB.id_fam = B.Id and SB.id_integrante = 1 ");
                    break;
                case TipoConsulta.Domicilios:
                    if (!total)
                    {
                        tablas.Add("M", " left join Municipios M on M.Id = D.MunicipioId ");
                        tablas.Add("L", " left join Localidades L on L.Id = D.LocalidadId ");
                        tablas.Add("Z", " left join ZonasImpulso Z on Z.Id = D.ZonaImpulsoId ");
                    }
                    break;
            }

            if (json != null)
            {
                foreach (var item in (JObject)json)
                {
                    if (item.Value.ToString().Length > 0)
                    {
                        if (preguntasDictionary.ContainsKey(item.Key.ToLower()))
                        {
                            var campo = preguntasDictionary[item.Key.ToLower()];
                            var esId = campo.Catalogo != null || campo.Respuestas.Any();
                            where += " and AP.preguntaId = @" + i + " and AP."
                                     + (esId ? "RespuestaId = @" + (i + 1) + "" : "Valor like @" + (i + 1) + "");
                            valores.Add(campo.Id);
                            valores.Add(esId ? item.Value.ToString() : "%" + item.Value.ToString() + "%");
                            i += 2;
                            if (!tablas.ContainsKey("A"))
                            {
                                var aplicacionesCompletas = new List<TipoConsulta>
                                {
                                    TipoConsulta.Aplicaciones, TipoConsulta.Cuadernillo
                                };
                                tablas.Add("A", " inner join Aplicaciones A on B.Id = A.BeneficiarioId "+(aplicacionesCompletas.Contains(tipoConsulta)?"":" and A.activa = 1 "));
                            }

                            if (!tablas.ContainsKey("AP"))
                            {
                                tablas.Add("AP", " inner join AplicacionPreguntas AP on AP.AplicacionId = A.Id ");
                            }
                        }
                        else
                        {
                            if (Query.FiltrarSolicitudes(ref where, ref i, beneficiarioDictionary, item.Key,
                            item.Value.ToString(), valores, "B"))
                            {
                                continue;
                            }
                            if (Query.FiltrarSolicitudes(ref where, ref i, domicilioDictionary, item.Key,
                                item.Value.ToString(), valores, "D")) { 
                                continue;
                            }
                            if (Query.FiltrarSolicitudes(ref where, ref i, localidadDictionary, item.Key,
                                item.Value.ToString(), valores, "L"))
                            {

                                if (!tablas.ContainsKey("L"))
                                {
                                    tablas.Add("L", " inner join Localidades L on L.Id = D.LocalidadId ");
                                }
                            }
                            else
                            {
                                if (Query.FiltrarSolicitudes(ref where, ref i, municipioDictionary, item.Key,
                                    item.Value.ToString(), valores, "MZ"))
                                {
                                    if (!tablas.ContainsKey("MZ"))
                                    {
                                        tablas.Add("MZ", " inner join MunicipiosZonas MZ on MZ.MunicipioId = D.MunicipioId ");
                                    }
                                }
                                else
                                {
                                    if (Query.FiltrarSolicitudes(ref where, ref i, aplicacionDictionary, item.Key, item.Value.ToString(), valores, "A"))
                                    {
                                        if (!tablas.ContainsKey("A"))
                                        {
                                            tablas.Add("A", " inner join Aplicaciones A on B.Id = A.BeneficiarioId and A.activa = 1 ");
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            foreach (var tabla in tablas)
            {
                inners += tabla.Value;
            }

            switch (tipoConsulta)
            {
                case TipoConsulta.Resumen:
                    campos.AddRange(new []{"A.Id","D.MunicipioId","A.Resultado"});
                    where += " GROUP BY B.Id, A.Id, D.MunicipioId, A.Resultado ORDER BY B.Id, A.Id";
                    break;
                case TipoConsulta.Aplicaciones:
                    campos.AddRange(new []{"A.TrabajadorId","AP.AplicacionId","P.Id","AP.RespuestaId","AP.Valor", "A.Resultado","P.Excluir","AP.Grado","AP.ValorCatalogo",
                        "AP.RespuestaIteracion","AP.Complemento","A.FechaInicio","A.FechaFin", "A.FechaSincronizacion","A.CreatedAt","D.LatitudAtm", "D.LongitudAtm",
                        "D.ClaveMunicipioCalculado","D.MunicipioCalculado","D.ClaveLocalidadCalculada", "D.LocalidadCalculado", "D.AgebCalculado", "D.ManzanaCalculado", 
                        "D.TipoCalculo","D.ZonaImpulsoCalculada","D.ClaveZonaImpulsoCalculada","D.ZapCalculado","D.TipoZap","D.ColoniaCalculada","D.CodigoPostalCalculado","D.ClaveMunicipioCisCalculado","D.CisCercano","D.DomicilioCisCalculado",
                        "D.IndiceDesarrolloHumano","D.MarginacionAgeb", "D.MarginacionLocalidad","D.MarginacionMunicipio",
                        "D.MunicipioId","D.LocalidadId","D.AgebId", "D.ManzanaId","D.CarreteraId","D.CaminoId","D.TipoAsentamientoId","D.NombreAsentamiento","D.DomicilioN","D.EntreCalle1","D.EntreCalle2","D.CallePosterior",
                        "D.NumExterior","D.NumInterior","D.CodigoPostal","D.Telefono","B.Folio","D.LatitudCorregida","D.LongitudCorregida","D.EstatusDireccion","TR.Nombre","A.Tiempo","A.VersionAplicacion","A.UpdatedAt"});
                    where += " ORDER BY B.Id, AP.AplicacionId, AP.RespuestaIteracion, P.Numero ";
                    break;
                case TipoConsulta.Cuadernillo:
                    campos.AddRange(new []{"A.Id", "AC.Alimentaria","AC.Educativa","AC.ServicioSalud","AC.SeguridadSocial","A.Vivienda",
                        "A.Servicios", "A.Tejido","A.NivelPobreza","A.Hacinamiento","AC.Edad", "AC.Hacinamiento","AC.Alimentaria","AC.SexoId","AC.Analfabetismo",
                        "AC.GradoEducativa","AC.PrimariaIncompleta","AC.SecundariaIncompleta","AC.Discapacidad","A.GradoAlimentaria","A.Piso",
                        "A.Techo","A.Muro","A.Agua", "A.Drenaje","A.Electricidad","A.Combustible","A.PropiedadVivienda","AC.TieneActa","A.Satisfaccion",
                        "A.Movilidad","A.RedesSociales","A.ConfianzaInstituciones","A.ConfianzaLiderez","A.SeguridadPublica","A.SeguridadParque"
                    });

                    where += " GROUP BY  B.Id , A.Id , AC.Alimentaria , AC.ServicioSalud, AC.SeguridadSocial, AC.Educativa , A.Servicios , A.Tejido ,A.NivelPobreza ,A.Hacinamiento , AC.Edad , AC.Hacinamiento ,AC.Alimentaria , AC.SexoId , AC.Analfabetismo , AC.GradoEducativa , AC.PrimariaIncompleta , AC.SecundariaIncompleta , AC.Discapacidad , A.GradoAlimentaria ,A.Piso , A.Techo , A.Muro , A.Agua , A.Drenaje , A.Electricidad , A.Combustible , A.PropiedadVivienda , AC.TieneActa ,A.Satisfaccion , A.Movilidad , A.RedesSociales , A.ConfianzaInstituciones , A.ConfianzaLiderez , A.SeguridadPublica , A.SeguridadParque, AC.AplicacionId,  AC.NumIntegrante, A.Vivienda ORDER BY AC.AplicacionId, AC.NumIntegrante";
                    break;
                case TipoConsulta.Exportar: 
                        campos.AddRange(new []{"B.CreatedAt","B.Folio","B.NumIntegrante","B.Nombre", "B.ApellidoPaterno","B.ApellidoMaterno","P.Nombre",
                        "B.FechaNacimiento","E.Nombre", "SX.Nombre","B.CURP","B.RFC","ES.Nombre","GE.Nombre","EC.Nombre",
                        "D.Telefono", "D.TelefonoCasa","D.DomicilioN","NumExterior", "NumInterior","D.EntreCalle1","D.EntreCalle2","D.CallePosterior","D.CodigoPostal",
                        "M.Nombre","L.Nombre","AG.Clave","MA.Nombre","CO.Nombre","CA.Nombre","ZI.Nombre","CR.Nombre","CM.Nombre","TA.Nombre","D.NombreAsentamiento"
                        ,"D.Latitud", "D.Longitud","ZapCalculado", "D.TipoZap", "CisCercano","ClaveMunicipioCisCalculado","DomicilioCisCalculado", "TipoCalculo", "IndiceDesarrolloHumano", 
                        "MarginacionMunicipio","MarginacionLocalidad","MarginacionAgeb", "B.EstatusInformacion","D.LatitudCorregida","D.LongitudCorregida","D.EstatusDireccion","TR.Nombre",
                        "SB.var1116_1","SB.var1125_1","SB.var1126_1","SB.var1127_1","SB.var1128_1"});
                        break;
                case TipoConsulta.Domicilios:
                    if (!total)
                    { 
                        campos.AddRange(new []{"B.Nombre", "B.ApellidoPaterno","B.ApellidoMaterno","B.ParentescoId",
                            "B.FechaNacimiento", "B.SexoId","B.CURP","B.RFC","B.EstudioId","B.GradoEstudioId","B.MismoDomicilio",
                            "D.Telefono", "D.TelefonoCasa","D.DomicilioN","M.Nombre","L.Nombre","D.AgebId","D.Latitud", 
                            "D.Longitud","D.CodigoPostal","B.EstatusInformacion","B.CreatedAt","B.PadreId", "B.Folio","Z.Nombre","B.DomicilioId","D.LatitudAtm","D.LongitudAtm"});
                        where += " ORDER BY B.Id, B.PadreId";
                    }
                    break;
            }
            var q = new KeyValuePair<string, List<string>>("select " + string.Join(" , ", campos.ToArray()) + query + inners + where, valores);
            return new KeyValuePair<string, List<string>>("select "+string.Join(" , ", campos.ToArray())+query + inners + where, valores);
        }

        public string BuildSabana(BeneficiariosRequest request, string inicio, string fin)
        {
            var builder = new StringBuilder();
            var preguntas = _context.Pregunta.Where(p => p.DeletedAt == null).OrderBy(p => p.Numero)
                .Include(p=>p.Respuestas).ToDictionary(p=>p.Id);
            var idIterable = "0";// preguntas.Values.First(p => p.Iterable).CondicionIterable.Substring(1);
            var preguntaIterable = new Pregunta();
            var preguntasList = new List<string> {"id_fam","cab_idencuestador","nombre_encuestador","cab_idencuesta","cab_fechaencuesta","cab_horaencuesta","cab_fechafinencuesta","cab_horafinencuesta","cab_tiempoencuesta","cab_fechainsert","fecha_sync","hora_sync","cab_latitud","cab_longitud",
                "LatitudCorregida","LongitudCorregida","EstatusAPI","id_integrante","T","Version",
                "cve_mun","nom_mun","cve_loc","nom_localidad","cve_ageb","cve_manzana","bandera_geografica","cve_poli","clave_zona_impulso","zap","tipozap","colonia_oficial",
                "codigo_postal","clave_municipio_cis","nombre_oficial_cis","domicilio_cis","idh_municipio","marginacion_ageb","marginacion_localidad","marginacion_municipio",
                "curprenapo","var969_1","var970_1","var971_1","var972_1","var973_1","var974_1","var975_1","var976_1","var977_1","var978_1","var979_1","var980_1","var981_1","var982_1","var983_1"
            };
            foreach (var pregunta in preguntas.Values) {
                if (pregunta.Id.Equals("-1"))
                {
                    continue;
                }
                pregunta.RespuestasMap = new Dictionary<string, Respuesta>();
                if (pregunta.Id.Equals(idIterable))
                {
                    preguntaIterable = pregunta;
                }
                if ((pregunta.Gradual||pregunta.TipoPregunta.Equals(TipoPregunta.Check.ToString())) && pregunta.Respuestas.Any()) {
                    pregunta.RespuestasMap = pregunta.Respuestas.OrderBy(r =>r.Numero).ToDictionary(r=>r.Id);
                    var numRespuesta = 1;
                    foreach (var respuesta in pregunta.RespuestasMap.Values) { 
                        preguntasList.Add("var"+pregunta.Id+"_"+numRespuesta);
                        if (pregunta.Gradual)
                        {
                            if (!respuesta.Negativa)
                            {
                                preguntasList.Add("vartx1_" + pregunta.Id + "_" + numRespuesta);
                                if (respuesta.Complemento != null)
                                {
                                    preguntasList.Add("vartx2_" + pregunta.Id + "_" + numRespuesta);
                                }
                            }
                        }
                        else
                        {
                            if (respuesta.Complemento != null) {
                                preguntasList.Add("vartx1_" + pregunta.Id + "_" + numRespuesta);
                            }
                        }

                        numRespuesta++;
                    }
                } else {
                    pregunta.RespuestasMap = pregunta.Respuestas.OrderBy(r =>r.Numero).ToDictionary(r=>r.Id);
                    preguntasList.Add("var" + pregunta.Id + "_1");
                    if (!pregunta.TipoPregunta.Equals(TipoPregunta.Radio.ToString())) continue;
                    var numRespuesta = 1;
                    preguntasList.AddRange(pregunta.RespuestasMap.Values.Where(r => r.Complemento != null).ToList().Select(respuesta => "vartx1_" + pregunta.Id + "_" + numRespuesta++));
                }
            }            
            builder.AppendLine(string.Join("|", preguntasList.ToArray()));
            return builder + BuildDetalles(request, inicio, fin, preguntas, preguntaIterable).ToString();
        }
        
        private StringBuilder BuildDetalles(BeneficiariosRequest request, string inicio, string fin, Dictionary<string, Pregunta> preguntas, 
            Pregunta preguntaIterable)
        {
            var builder = new StringBuilder();
            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                var familia = new FamiliaSabana
                {
                    Integrantes = new List<IntegranteSabana>()
                    {
                        new IntegranteSabana()
                        {
                            Curp = "", 
                            Preguntas =  new Dictionary<string, Dictionary<int, string>>()
                        }
                    } 
                };
                
                var numHijos = 0;
                var totalIntegrantes = 0;
                var numAplicacion = 0;
                var numHijo = -1;
                var numPregunta = "";
                var beneficiarioId = "";
                var aplicacionId = "";
                var domicilio = new Domicilio();
                var util = new BeneficiarioCode(_context);
                var query= util.BuildBeneficiariosQuery(request, TipoConsulta.Aplicaciones, false, inicio, fin, false);
                command.CommandText = query.Key;
                var iParametro = 0;
                foreach (var parametro in query.Value)
                {
                    command.Parameters.Add(new SqlParameter("@"+(iParametro++),parametro));
                }
                command.CommandTimeout = 300;
                _context.Database.OpenConnection();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        var fechaInicio = new DateTime();
                        var fechaSync = new DateTime();
                        var fechaFin = new DateTime();
                        while (reader.Read())
                        {
                            var id = reader.GetString(0);
                            var aplicacion = reader.GetString(2);
                            var pregunta = reader.GetString(3);
                            var respuestaId = reader.IsDBNull(4) ? 1 :int.Parse(reader.GetString(4));
                            var valor =  reader.IsDBNull(4) ? (reader.IsDBNull(9) ? (reader.IsDBNull(5) ? "" : (preguntas[pregunta].TipoPregunta==TipoPregunta.Numerica.ToString() ? reader.GetString(5) : "'"+reader.GetString(5)+"'")) : reader.GetString(9)) : reader.GetString(4);
                            var numIntegrante = reader.GetBoolean(7) ? reader.GetInt32(10)+numHijos : reader.GetInt32(10); //Si la pregunta es de tipo excluyente entonces aumentamos el numero de integrante para que en la sabana aparezca como un registro nuevo
                            var grado = reader.IsDBNull(8) ? "0" : reader.GetString(8);
                         
                            if (!beneficiarioId.Equals(id)){ //Construccion de una nueva familia
                                if (!string.IsNullOrEmpty(beneficiarioId))
                                {
                                    for (var i = -1; i <= totalIntegrantes; i++)
                                    {
                                        builder.AppendLine(string.Join("|",exportarBeneficiario(
                                            preguntas.Values.ToList(), familia.Integrantes[i+1].Preguntas,
                                            domicilio, numAplicacion, i+1,fechaInicio, fechaFin, fechaSync, familia.Integrantes[i+1].Curp)));
                                    }
                                }
                                fechaInicio = reader.GetDateTime(12);
                                if (!reader.IsDBNull(13)) { 
                                    fechaFin = reader.GetDateTime(13);
                                }
                                fechaSync =reader.GetDateTime(14);
                                domicilio = new Domicilio
                                {
                                    Id = id,
                                    AplicacionId = aplicacion,
                                    TrabajadorId = reader.IsDBNull(1) ? "" : reader.GetString(1),
                                    CreatedAt = reader.GetDateTime(15),                                //FechaInsert
                                    Latitud = reader.IsDBNull(16) ? "" : reader.GetString(16),
                                    Longitud = reader.IsDBNull(17) ? "" : reader.GetString(17),
                                    LatitudCorregida = reader.IsDBNull(55) ? "" : reader.GetString(55),
                                    LongitudCorregida = reader.IsDBNull(56) ? "" : reader.GetString(56),
                                    EstatusDireccion = reader.IsDBNull(57) ? "" : reader.GetString(57),
                                    ClaveMunicipioCalculado = reader.IsDBNull(18) ? "" : reader.GetString(18),
                                    MunicipioCalculado = reader.IsDBNull(19) ? "" : reader.GetString(19),
                                    ClaveLocalidadCalculada = reader.IsDBNull(20) ? "" : reader.GetString(20),
                                    LocalidadCalculado = reader.IsDBNull(21) ? "" : reader.GetString(21),
                                    AgebCalculado = reader.IsDBNull(22) ? "" : reader.GetString(22),
                                    ManzanaCalculado = reader.IsDBNull(23) ? "" : reader.GetString(23),
                                    TipoCalculo = reader.IsDBNull(24) ? "" : reader.GetString(24),
                                    ZonaImpulsoCalculada = reader.IsDBNull(25) ? "" : reader.GetString(25),
                                    ClaveZonaImpulsoCalculada = reader.IsDBNull(26) ? "" : reader.GetString(26),
                                    ZapCalculado = reader.IsDBNull(27) ? "" : reader.GetString(27),
                                    TipoZap = reader.IsDBNull(28) ? "" : reader.GetString(28),
                                    ColoniaCalculada = reader.IsDBNull(29) ? "" : reader.GetString(29),
                                    CodigoPostalCalculado = reader.IsDBNull(30) ? "" : reader.GetString(30),
                                    ClaveMunicipioCisCalculado = reader.IsDBNull(31) ? "" : reader.GetString(31),
                                    CisCercano = reader.IsDBNull(32) ? "" : reader.GetString(32),
                                    DomicilioCisCalculado = reader.IsDBNull(33) ? "" : reader.GetString(33),
                                    IndiceDesarrolloHumano = reader.IsDBNull(34) ? "" : reader.GetString(34),
                                    MarginacionAgeb = reader.IsDBNull(35) ? "" : reader.GetString(35),
                                    MarginacionLocalidad = reader.IsDBNull(36) ? "" : reader.GetString(36),
                                    MarginacionMunicipio = reader.IsDBNull(37) ? "" : reader.GetString(37),
                                    MunicipioId = reader.GetString(38),
                                    LocalidadId = reader.GetString(39),
                                    AgebId = reader.IsDBNull(40) ? "" : reader.GetString(40),
                                    ManzanaId = reader.IsDBNull(41) ? "" : reader.GetString(41),
                                    CarreteraId = reader.IsDBNull(42) ? "" : reader.GetString(42),
                                    CaminoId = reader.IsDBNull(43) ? "" : reader.GetString(43),
                                    TipoAsentamientoId = reader.GetString(44),
                                    NombreAsentamiento = reader.IsDBNull(45) ? "" : reader.GetString(45),
                                    DomicilioN = reader.GetString(46),
                                    EntreCalle1 = reader.IsDBNull(47) ? "" : reader.GetString(47),
                                    EntreCalle2 = reader.IsDBNull(48) ? "" : reader.GetString(48),
                                    CallePosterior = reader.IsDBNull(49) ? "" : reader.GetString(49),
                                    NumExterior = reader.IsDBNull(50) ? "" : reader.GetString(50),
                                    NumInterior = reader.IsDBNull(51) ? "" : reader.GetString(51),
                                    CodigoPostal = reader.IsDBNull(52) ? "" : reader.GetString(52),
                                    Telefono = reader.IsDBNull(53) ? "" : reader.GetString(53),
                                    Folio = reader.IsDBNull(54) ? "" : reader.GetString(54),
                                    NombreEncuestador = reader.IsDBNull(58) ? "" : reader.GetString(58),
                                    Tiempo = reader.IsDBNull(59) ? 0 : reader.GetInt32(59),
                                    VersionAplicacion = reader.IsDBNull(60)?"":reader.GetString(60),
                                    UpdatedAt = reader.GetDateTime(61)
                                };

                                beneficiarioId = id;
                                numAplicacion = 0;
                                aplicacionId = "";
                                numHijos = 0;
                                totalIntegrantes = 0;
                                numHijo = -1;
                                numPregunta = "";
                                familia = new FamiliaSabana
                                {
                                    Integrantes = new List<IntegranteSabana>()
                                    {
                                        new IntegranteSabana()
                                        {
                                            Curp = "", 
                                            Preguntas =  new Dictionary<string, Dictionary<int, string>>()
                                        }
                                    } 
                                }; 
                            }

                            if (aplicacionId != aplicacion)
                            {
                                if (!string.IsNullOrEmpty(aplicacionId))
                                {
                                    for (var i = -1; i <= totalIntegrantes; i++)
                                    {
                                        builder.AppendLine(string.Join("|",exportarBeneficiario(
                                            preguntas.Values.ToList(), familia.Integrantes[i+1].Preguntas,
                                            domicilio, numAplicacion,i+1,fechaInicio, fechaFin, fechaSync, familia.Integrantes[i+1].Curp)));
                                    }
                                    numHijos = 0;
                                    numHijo = -1;
                                    totalIntegrantes = 0;
                                    numPregunta = "";
                                    familia = new FamiliaSabana
                                    {
                                        Integrantes = new List<IntegranteSabana>()
                                        {
                                            new IntegranteSabana()
                                            {
                                                Curp = "", 
                                                Preguntas =  new Dictionary<string, Dictionary<int, string>>()
                                            }
                                        } 
                                    }; 
                                }

                                aplicacionId = aplicacion;
                                numAplicacion++;
                            }

                            var agregarPregunta = numPregunta != pregunta;
                            totalIntegrantes = totalIntegrantes < numIntegrante ? numIntegrante : totalIntegrantes;
                            if (pregunta == preguntaIterable.Id && numHijos == 0)//Obtencion del numero de integrantes del hogar
                            {
                                numHijos = int.Parse(valor.Replace("'",""));
                            }
                            
                            if (numIntegrante != numHijo)
                            {
                                if (numIntegrante > numHijo)//Se agrega nuevo integrante a objeto familiar
                                {
                                    for (var i = familia.Integrantes.Count; i <= numIntegrante+1; i++)
                                    {
                                        familia.Integrantes.Add(new IntegranteSabana
                                        {
                                            Preguntas = new Dictionary<string, Dictionary<int, string>>()
                                        });
                                    }

                                    agregarPregunta = true;
                                }
                                numHijo = numIntegrante;
                            }
                            var numHijoAux = PreguntasIntegranteCero.Contains(pregunta) ? 0 : numHijo + 1;
                            if (pregunta.Equals("-1"))
                            {
                                familia.Integrantes[numHijoAux].Curp = valor;
                            }
                            if (agregarPregunta)
                            {
                                familia.Integrantes[numHijoAux].Preguntas.Add(pregunta, new Dictionary<int, string>());
                                numPregunta=pregunta;
                            }
                            
                            if (!familia.Integrantes[numHijoAux].Preguntas[pregunta].ContainsKey(respuestaId))
                            {
                                familia.Integrantes[numHijoAux].Preguntas[pregunta].Add(respuestaId, valor);
                            }
                            else
                            {
                                familia.Integrantes[numHijoAux].Preguntas[pregunta][respuestaId] = valor;
                            }
                            
                            if (preguntas[pregunta].Gradual)//Preguntas graduales como discapacidad y articulos del hogar
                            {
                                if (!preguntas[pregunta].RespuestasMap[""+respuestaId].Negativa)
                                {
                                    
                                    familia.Integrantes[numHijoAux].Preguntas[pregunta][respuestaId] = ""+grado;
                                }
                            }
                            if (!reader.IsDBNull(11))//Preguntas con complemento
                            {
                                familia.Integrantes[numHijoAux].Preguntas[pregunta][respuestaId*1000] = reader.GetString(11);
                            }
                        }
                        for (var i = -1; i <= totalIntegrantes; i++)
                        {
                            builder.AppendLine(string.Join("|",exportarBeneficiario(
                                preguntas.Values.ToList(), familia.Integrantes[i+1].Preguntas,
                                domicilio, numAplicacion, i+1,fechaInicio, fechaFin, fechaSync, familia.Integrantes[i+1].Curp)));
                        }
                    }
                }
                _context.Database.CloseConnection();
            }
            return builder;  
        }

        private IEnumerable<string> exportarBeneficiario(List<Pregunta> preguntas, Dictionary<string, Dictionary<int, string>> persona, 
            Domicilio domicilio, int numAplicacion, int numIntegrante, DateTime fechaInicio, DateTime fechaFin, DateTime fechaSync, string curpRenapo)
        {
            var respuestas = new List<string>(new []{domicilio.Id, domicilio.TrabajadorId,"'"+domicilio.NombreEncuestador+"'", "'"+domicilio.Folio+"'", 
                fechaInicio.ToString("yyyy-MM-dd"), fechaInicio.ToString("HH:mm"),
                fechaFin == null ? "" : fechaFin.ToString("yyyy-MM-dd"),fechaFin==null?"": fechaFin.ToString("HH:mm"),
                calcularTiempoEncuesta(domicilio.Tiempo, fechaInicio, fechaFin, domicilio.UpdatedAt),
                domicilio.CreatedAt.ToString("yyyy-MM-dd"),
                fechaSync.ToString("yyyy-MM-dd"), fechaSync.ToString("HH:mm"), 
                domicilio.Latitud, domicilio.Longitud,
                (string.IsNullOrEmpty(domicilio.LatitudCorregida)?"":"'"+domicilio.LatitudCorregida+"'"),
                (string.IsNullOrEmpty(domicilio.LongitudCorregida)?"":"'"+domicilio.LongitudCorregida+"'"),
                (string.IsNullOrEmpty(domicilio.EstatusDireccion)?"":"'"+domicilio.EstatusDireccion+"'"),
                numIntegrante.ToString(), numAplicacion.ToString(), domicilio.VersionAplicacion,
                "'"+domicilio.ClaveMunicipioCalculado+"'","'"+domicilio.MunicipioCalculado+"'",
                "'"+domicilio.ClaveLocalidadCalculada+"'", "'"+domicilio.LocalidadCalculado+"'", 
                domicilio.AgebCalculado, domicilio.ManzanaCalculado,
                (string.IsNullOrEmpty(domicilio.TipoCalculo)?"":"'"+domicilio.TipoCalculo+"'"),
                domicilio.ClaveZonaImpulsoCalculada, domicilio.ZonaImpulsoCalculada ,domicilio.ZapCalculado, domicilio.TipoZap,
                (string.IsNullOrEmpty(domicilio.ColoniaCalculada)?"":"'"+domicilio.ColoniaCalculada+"'"),
                (string.IsNullOrEmpty(domicilio.CodigoPostalCalculado)?"":"'"+domicilio.CodigoPostalCalculado+"'"),
                (string.IsNullOrEmpty(domicilio.ClaveMunicipioCisCalculado)?"":"'"+domicilio.ClaveMunicipioCisCalculado+"'"),
                (string.IsNullOrEmpty(domicilio.CisCercano)?"":"'"+domicilio.CisCercano+"'"),
                (string.IsNullOrEmpty(domicilio.DomicilioCisCalculado)?"":"'"+domicilio.DomicilioCisCalculado+"'"),
                domicilio.IndiceDesarrolloHumano,domicilio.MarginacionAgeb,
                domicilio.MarginacionLocalidad, domicilio.MarginacionMunicipio, curpRenapo,
                (string.IsNullOrEmpty(domicilio.MunicipioId)?"":"'"+domicilio.MunicipioId+"'"),
                (string.IsNullOrEmpty(domicilio.LocalidadId)?"":"'"+domicilio.LocalidadId+"'"),
                (string.IsNullOrEmpty(domicilio.AgebId)?"":"'"+domicilio.AgebId+"'"),
                (string.IsNullOrEmpty(domicilio.ManzanaId)?"":"'"+domicilio.ManzanaId+"'"),
                (string.IsNullOrEmpty(domicilio.CarreteraId)?"":"'"+domicilio.CarreteraId+"'"),
                (string.IsNullOrEmpty(domicilio.CaminoId)?"":"'"+domicilio.CaminoId+"'"),
                (string.IsNullOrEmpty(domicilio.TipoAsentamientoId)?"":"'"+domicilio.TipoAsentamientoId+"'"),
                (string.IsNullOrEmpty(domicilio.NombreAsentamiento)?"":"'"+domicilio.NombreAsentamiento+"'"),
                (string.IsNullOrEmpty(domicilio.DomicilioN)?"":"'"+domicilio.DomicilioN+"'"),
                (string.IsNullOrEmpty(domicilio.EntreCalle1)?"":"'"+domicilio.EntreCalle1+"'")+" "+
                (string.IsNullOrEmpty(domicilio.EntreCalle2)?"":"'"+domicilio.EntreCalle2+"'"),
                (string.IsNullOrEmpty(domicilio.CallePosterior)?"":"'"+domicilio.CallePosterior+"'"),
                (string.IsNullOrEmpty(domicilio.NumExterior)?"":"'"+domicilio.NumExterior+"'"),
                (string.IsNullOrEmpty(domicilio.NumInterior)?"":"'"+domicilio.NumInterior+"'"),
                (string.IsNullOrEmpty(domicilio.CodigoPostal)?"":"'"+domicilio.CodigoPostal+"'"),
                (string.IsNullOrEmpty(domicilio.Telefono)?"":"'"+domicilio.Telefono+"'")
            });

            foreach (var pregunta in preguntas)
            {
                if (pregunta.Id.Equals("-1"))
                {
                    continue;
                }
                if (persona.ContainsKey(pregunta.Id))
                {
                    var ids = new List<string>();
                    if ((pregunta.Gradual || pregunta.TipoPregunta.Equals(TipoPregunta.Check.ToString())) && pregunta.RespuestasMap.Any())
                    {
                        foreach (var respuesta in pregunta.RespuestasMap.Values)
                        {
                            var respuestaId = int.Parse(respuesta.Id);
                            if (persona[pregunta.Id].ContainsKey(respuestaId))
                            {
                                if (pregunta.Gradual)
                                {
                                    ids.Add(respuestaId.ToString());
                                    if (!respuesta.Negativa) {
                                        ids.Add(persona[pregunta.Id][respuestaId]);
                                    }

                                }
                                else
                                {
                                    ids.Add(persona[pregunta.Id][respuestaId]);
                                }
                            }
                            else
                            {
                                if (pregunta.Gradual)
                                {
                                    ids.Add("");
                                    if (!respuesta.Negativa)
                                    {
                                        ids.Add("");
                                    }
                                }
                                else
                                {
                                    ids.Add("");
                                }
                            }

                            if (!string.IsNullOrEmpty(respuesta.Complemento))
                            {
                                if (persona[pregunta.Id].ContainsKey(respuestaId*1000))
                                {
                                    ids.Add(respuesta.Complemento == TipoComplemento.Abierta.ToString() ? "'" + persona[pregunta.Id][respuestaId * 1000] + "'" : persona[pregunta.Id][respuestaId * 1000]);
                                }
                                else
                                {
                                    ids.Add("");
                                }
                            }
                        }
                    }
                    else
                    {
                        if (pregunta.TipoPregunta==TipoPregunta.Fecha.ToString() || pregunta.TipoPregunta==TipoPregunta.FechaPasada.ToString()
                                                                                 || pregunta.TipoPregunta==TipoPregunta.FechaFutura.ToString())
                        {
                            var fecha = persona[pregunta.Id].First().Value.Replace("'","").Split("/");
                            ids.Add(fecha[2]+"-"+fecha[1]+"-"+fecha[0]);
                        }
                        else
                        {
                            ids.Add(persona[pregunta.Id].First().Value);
                        }

                        if (pregunta.TipoPregunta.Equals(TipoPregunta.Radio.ToString()))
                        {
                            var respuestasComplemento = pregunta.RespuestasMap.Values.Where(r => r.Complemento != null).ToList();
                            if (respuestasComplemento.Any())
                            {
                                foreach (var respuesta in respuestasComplemento)
                                {
                                    var respuestaId = int.Parse(respuesta.Id);
                                    if (persona[pregunta.Id].ContainsKey(respuestaId*1000))
                                    {
                                        ids.Add(persona[pregunta.Id][respuestaId*1000]);
                                    }
                                    else
                                    {
                                        ids.Add("");
                                    }
                                }
                            }
                        }
                    }
                    respuestas.Add( string.Join("|",ids.ToArray()));
                }
                else
                {
                    if ((pregunta.Gradual||pregunta.TipoPregunta.Equals(TipoPregunta.Check.ToString())) && pregunta.RespuestasMap.Any())
                    {
                        foreach (var respuesta in pregunta.RespuestasMap.Values)
                        {
                            if (pregunta.Gradual)
                            {
                                respuestas.Add("");
                                if (!respuesta.Negativa)
                                {
                                    respuestas.Add("");
                                }
                            }
                            else
                            {
                                respuestas.Add("");
                            }
                            if (respuesta.Complemento != null)
                            {
                                respuestas.Add("");
                            }
                        }
                    }
                    else
                    {
                        respuestas.Add("");
                        if (!pregunta.TipoPregunta.Equals(TipoPregunta.Radio.ToString())) continue;
                        var respuestasComplemento = pregunta.RespuestasMap.Values.Where(r => r.Complemento != null).ToList();
                        if (!respuestasComplemento.Any()) continue;
                        respuestas.AddRange(respuestasComplemento.Select(unused => ""));
                    }
                }
            }
            return respuestas;
        }
        
        public KeyValuePair<string, List<string>> BuildVisitasQuery(BeneficiariosRequest request, string inicio, string fin)
        {
            var i = 2;
            var campos = new List<string>();
            var query = " from Aplicaciones A inner join Trabajadores T on A.TrabajadorId=T.Id and A.Activa = 1 inner join Beneficiarios B on B.Id = A.BeneficiarioId inner join Domicilio D on D.Id = B.DomicilioId ";
            var where = " where A.DeletedAt is null and A.Prueba = 0 and B.HijoId is null and A.FechaInicio >= @0 and A.FechaInicio <= @1";
            var inners = "";
            var valores = new List<string>{inicio, fin};
            var tablas = new Dictionary<string, string>();
            var json = JsonConvert.DeserializeObject(request.Valores);
            
            var beneficiarioProperties = typeof(Beneficiario).GetProperties();
            var domicilioProperties = typeof(Domicilio).GetProperties();
            var localidadProperties = typeof(Localidad).GetProperties();
            var municipioProperties = typeof(Municipio).GetProperties();
            var aplicacionProperties = typeof(Aplicacion).GetProperties();

            var beneficiarioDictionary = Query.BuildPropertyDictionary(beneficiarioProperties);
            var domicilioDictionary = Query.BuildPropertyDictionary(domicilioProperties);
            var localidadDictionary = Query.BuildPropertyDictionary(localidadProperties);
            var municipioDictionary = Query.BuildPropertyDictionary(municipioProperties);
            var aplicacionDictionary = Query.BuildPropertyDictionary(aplicacionProperties);

            foreach (var item in (JObject)json)
            {
                if (item.Value.ToString().Length > 0)
                {
                    
                    if (!Query.FiltrarSolicitudes(ref where, ref i, beneficiarioDictionary, item.Key,
                        item.Value.ToString(), valores, "B"))
                    {
                        if (!Query.FiltrarSolicitudes(ref where, ref i, domicilioDictionary, item.Key,
                            item.Value.ToString(), valores, "D"))
                        {
                            if (Query.FiltrarSolicitudes(ref where, ref i, localidadDictionary, item.Key,
                                item.Value.ToString(), valores, "L"))
                            {
                                if (!tablas.ContainsKey("L"))
                                {
                                    tablas.Add("L", " inner join Localidades L on L.Id = D.LocalidadId ");
                                }
                            }
                            else
                            {
                                if (Query.FiltrarSolicitudes(ref where, ref i, municipioDictionary, item.Key,
                                    item.Value.ToString(), valores, "MZ"))
                                {
                                    if (!tablas.ContainsKey("M"))
                                    {
                                        tablas.Add("M", " inner join Municipios M on M.Id = D.MunicipioId ");
                                    }
                                    if (!tablas.ContainsKey("MZ"))
                                    {
                                        tablas.Add("MZ", " inner join MunicipiosZonas MZ on MZ.MunicipioId = D.MunicipioId ");
                                    }
                                }
                                else
                                {
                                    Query.FiltrarSolicitudes(ref where, ref i, aplicacionDictionary, item.Key,
                                        item.Value.ToString(), valores, "A");
                                }
                            }  
                        }
                    }
                }
            }
            
            foreach (var tabla in tablas)
            {
                inners += tabla.Value;
            }
            campos.AddRange(new []{"A.TrabajadorId", "T.Nombre","A.Id","D.LatitudAtm","D.LongitudAtm","A.FechaInicio","A.FechaSincronizacion","A.Resultado"});
            where += " ORDER BY A.Id ";
            return new KeyValuePair<string, List<string>>("select "+string.Join(" , ", campos.ToArray())+query + inners + where, valores);
        }

        /// <summary>
        /// Metodo que inserta en la sabana las respuestas de un hogar
        /// </summary>
        /// <param name="_context">Conexión a la base de datos</param>
        /// <param name="aplicacionId">Id de la encuesta</param>
        public List<string> IngresarAplicacionEnSabana(Aplicacion aplicacion, DatabaseFacade conexion)
        {
            var registros = new List<string>();
            var beneficiario = _context.Beneficiario.Where(b => b.Id.Equals(aplicacion.BeneficiarioId)).
                Include(b=>b.Hijos).FirstOrDefault();
            var domicilio = _context.Domicilio.FirstOrDefault(d => d.Id.Equals(beneficiario.DomicilioId));
            var respuestas = _context.AplicacionPregunta.Where(ap=>ap.AplicacionId.Equals(aplicacion.Id)).OrderBy(ap=>ap.RespuestaIteracion)
                .Include(ap=>ap.Respuesta).OrderBy(ap=>ap.RespuestaIteracion).ThenBy(ap=>ap.Pregunta.Numero).ToList();
            var preguntas = _context.Pregunta.Where(p => p.DeletedAt == null).Include(p => p.Respuestas)
                .OrderBy(p => p.Numero).ToDictionary(p=>p.Id);
            var idIterable = preguntas.Values.First(p => p.Iterable).CondicionIterable.Substring(1);
            var preguntasDB = EncuestaCode.ObtenerPreguntasParaSabana(preguntas.Values.ToList());
            var numHijos = 0;
            var integrantes = new Dictionary<int, Dictionary<string, Dictionary<string, string>>>
            {
                { 0, new Dictionary<string, Dictionary<string, string>>() }
            };
            foreach (var respuesta in respuestas) {
                if (respuesta.Valor != null) { 
                    respuesta.Valor = respuesta.Valor.Replace("'","");
                }
                var preguntaDB = preguntas[respuesta.PreguntaId];
                if (respuesta.PreguntaId == idIterable)
                {
                    numHijos = int.Parse(respuesta.Valor);
                }
                var numIntegrante = PreguntasIntegranteCero.Contains(respuesta.PreguntaId) ? 0 : (preguntaDB.Excluir ? 
                    respuesta.RespuestaIteracion + numHijos+1: respuesta.RespuestaIteracion + 1);
                if (!integrantes.ContainsKey(numIntegrante))
                {
                    integrantes.Add(numIntegrante, new Dictionary<string, Dictionary<string, string>>());
                    integrantes[numIntegrante].Add("CURP", new Dictionary<string, string>());
                    if (numIntegrante == 1)
                    {
                        integrantes[numIntegrante]["CURP"].Add("0", beneficiario.Curp);
                    }
                    else
                    {
                        var familiar = beneficiario.Hijos.FirstOrDefault(h => h.NumIntegrante == numIntegrante);
                        integrantes[numIntegrante]["CURP"].Add("0", familiar==null?"":familiar.Curp);
                    }
                }

                if (!integrantes[numIntegrante].ContainsKey(respuesta.PreguntaId))
                {
                    integrantes[numIntegrante].Add(respuesta.PreguntaId, new Dictionary<string, string>());
                }
                var numRespuesta = preguntaDB.TipoPregunta == TipoPregunta.Check.ToString() || preguntaDB.Gradual?respuesta.Respuesta.Numero.ToString():"1";
                if (integrantes[numIntegrante][respuesta.PreguntaId].ContainsKey(numRespuesta))
                {
                    integrantes[numIntegrante][respuesta.PreguntaId][numRespuesta] = respuesta.RespuestaId ?? (respuesta.ValorCatalogo ?? (respuesta.ValorFecha.HasValue ? respuesta.ValorFecha.Value.ToString("dd-MM-yyyy") : respuesta.Valor));
                }
                else
                {
                    integrantes[numIntegrante][respuesta.PreguntaId].Add(numRespuesta, respuesta.RespuestaId ?? (respuesta.ValorCatalogo ?? (respuesta.ValorFecha.HasValue ? respuesta.ValorFecha.Value.ToString("dd-MM-yyyy") : respuesta.Valor)));
                }
                if (preguntaDB.Gradual)
                {
                    if (integrantes[numIntegrante][respuesta.PreguntaId].ContainsKey("G"+numRespuesta))
                    {
                        integrantes[numIntegrante][respuesta.PreguntaId]["G"+numRespuesta]=respuesta.Grado;
                    }
                    else
                    {
                        integrantes[numIntegrante][respuesta.PreguntaId].Add("G"+numRespuesta, respuesta.Grado);
                    }
                }
                if (respuesta.RespuestaId != null)
                {
                    var respuestaDB = preguntaDB.RespuestasMap.ContainsKey(respuesta.RespuestaId) ? preguntaDB.RespuestasMap[respuesta.RespuestaId] : null;
                    if (respuestaDB?.Complemento != null)
                    {
                        if (integrantes[numIntegrante][respuesta.PreguntaId].ContainsKey("C"+numRespuesta))
                        {
                            integrantes[numIntegrante][respuesta.PreguntaId]["C"+numRespuesta] = respuesta.Complemento;
                        }
                        else
                        {
                            integrantes[numIntegrante][respuesta.PreguntaId].Add("C"+numRespuesta, respuesta.Complemento); 
                        }
                    }
                }
                if (preguntas[respuesta.PreguntaId].Complemento != null)
                {
                    if (!integrantes[numIntegrante].ContainsKey("C"+respuesta.PreguntaId))
                    {
                        integrantes[numIntegrante].Add("C"+preguntaDB.Id, new Dictionary<string, string>());
                        integrantes[numIntegrante]["C"+preguntaDB.Id].Add("1", respuesta.Complemento);
                    }
                }
            }

            foreach (var integrante in integrantes)
            {
                bool hasAnterior;
                using (var command = conexion.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "SELECT id_fam, id_integrante from Sabana where id_fam= @0 and id_integrante=@1";
                    command.Parameters.Add(new SqlParameter("@0", beneficiario.Id));
                    command.Parameters.Add(new SqlParameter("@1", integrante.Key));
                    using (var reader = command.ExecuteReader())
                    {
                        hasAnterior = reader.HasRows;
                    }
                }
                
                var query = hasAnterior?"UPDATE Sabana SET ": "INSERT INTO Sabana(createdat, updatedat, localidad_id,ageb_id,manzana_id,id_fam,cab_idencuestador,nombre_encuestador,cab_idencuesta,cab_fechaencuesta,cab_horaencuesta,cab_fechafinencuesta,cab_horafinencuesta,cab_tiempoencuesta,cab_fechainsert,fecha_sync,hora_sync,cab_latitud,cab_longitud,latitud_corregida,longitud_corregida,estatus_api,id_integrante,T,id_viv,NumHogar,Version,"+
                                                              "cve_mun,nom_mun,cve_loc,nom_localidad,cve_ageb,cve_manzana,bandera_geografica,"
                                                              + "cve_poli,clave_zona_impulso,zap,tipozap,colonia_oficial,codigo_postal,clave_municipio_cis,nombre_oficial_cis,domicilio_cis,idh_municipio,marginacion_ageb,marginacion_localidad,marginacion_municipio,curp_renapo,telefono_casa,email,";
                var localidadId = string.IsNullOrEmpty(domicilio.ClaveLocalidadCalculada)?"":domicilio.ClaveLocalidadCalculada.Split("@")[0].Split("(")[0];
                var agebId = string.IsNullOrEmpty(domicilio.AgebCalculado)?"":domicilio.AgebCalculado.Split("@")[0].Split("(")[0];
                var manzanaId = string.IsNullOrEmpty(domicilio.ManzanaCalculado)?"":domicilio.ManzanaCalculado.Split("@")[0].Split("(")[0];
                if (!hasAnterior)
                {
                    for (var i = 969; i < 984; i++)
                    {
                        query += "var" + i + "_1,";
                    }
                    foreach (var preguntaDb in preguntasDB.Keys)
                    {
                        query += preguntaDb+",";
                    }
                    query = query.Remove(query.Length - 1);
                    query += ") VALUES('"+aplicacion.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss")+"','"+aplicacion.UpdatedAt.ToString("yyyy-MM-dd HH:mm:ss")+"','"+localidadId+"','"+agebId+"','"+manzanaId+"','" + beneficiario.Id + "','"+aplicacion.TrabajadorId+"','"+aplicacion.Trabajador.Nombre+"','"+beneficiario.Folio + "','" + aplicacion.FechaInicio.Value.ToString("yyyy-MM-dd") + "','" 
                             + aplicacion.FechaInicio.Value.ToString("HH:mm") + "','" + (aplicacion.FechaFin.HasValue ? aplicacion.FechaFin.Value.ToString("yyyy-MM-dd") : "") + "','" + 
                             (aplicacion.FechaFin.HasValue ? aplicacion.FechaFin.Value.ToString("HH:mm") : "") + "','" 
                             + calcularTiempoEncuesta(aplicacion.Tiempo, aplicacion.FechaInicio, aplicacion.FechaFin, aplicacion.UpdatedAt) + "','" + aplicacion.CreatedAt.ToString("yyyy-MM-dd") + "','" + aplicacion.FechaSincronizacion.Value.ToString("yyyy-MM-dd") + "','" + aplicacion.FechaSincronizacion.Value.ToString("HH:mm") + "','" 
                             + domicilio.LatitudAtm + "','" + domicilio.LongitudAtm + "','"+ (string.IsNullOrEmpty(domicilio.LatitudCorregida) ? "" : domicilio.LatitudCorregida) + "','" 
                             + (string.IsNullOrEmpty(domicilio.LongitudCorregida) ? "" : domicilio.LongitudCorregida) + "','" 
                             + (string.IsNullOrEmpty(domicilio.EstatusDireccion) ? "" : domicilio.EstatusDireccion) + "','" 
                             + integrante.Key+"','"+ aplicacion.NumeroAplicacion +"','"+domicilio.Id+"','"+beneficiario.NumFamilia+"','"+aplicacion.VersionAplicacion+ "','"+ domicilio.ClaveMunicipioCalculado + "','" + domicilio.MunicipioCalculado + "','" +
                             domicilio.ClaveLocalidadCalculada + "','" + domicilio.LocalidadCalculado + "','" + domicilio.AgebCalculado + "','" + domicilio.ManzanaCalculado + "','" +
                             (string.IsNullOrEmpty(domicilio.TipoCalculo) ? "" : domicilio.TipoCalculo) + "','" + domicilio.ClaveZonaImpulsoCalculada + "','" + domicilio.ZonaImpulsoCalculada + "','" +
                             domicilio.ZapCalculado + "','" + domicilio.TipoZap + "','" + (string.IsNullOrEmpty(domicilio.ColoniaCalculada) ? "" : domicilio.ColoniaCalculada) + "','" +
                             (string.IsNullOrEmpty(domicilio.CodigoPostalCalculado) ? "" : domicilio.CodigoPostalCalculado) + "','" +
                             (string.IsNullOrEmpty(domicilio.ClaveMunicipioCisCalculado) ? "" : domicilio.ClaveMunicipioCisCalculado) + "','" +
                             (string.IsNullOrEmpty(domicilio.CisCercano) ? "" : domicilio.CisCercano) + "','" +
                             (string.IsNullOrEmpty(domicilio.DomicilioCisCalculado) ? "" : domicilio.DomicilioCisCalculado) + "','" +
                             domicilio.IndiceDesarrolloHumano + "','" + domicilio.MarginacionAgeb + "','" +
                             domicilio.MarginacionLocalidad + "','" + domicilio.MarginacionMunicipio + "','"+(integrante.Value.ContainsKey("CURP") ? integrante.Value["CURP"]["0"]:"")+"','" +
                             (string.IsNullOrEmpty(domicilio.TelefonoCasa) ? "" : domicilio.TelefonoCasa) + "','"+
                             (string.IsNullOrEmpty(domicilio.Email) ? "" : domicilio.Email) + "','"+
                             (string.IsNullOrEmpty(domicilio.MunicipioId) ? "" : domicilio.MunicipioId) + "','" +
                             (string.IsNullOrEmpty(domicilio.LocalidadId) ? "" : domicilio.LocalidadId) + "','" +
                             (string.IsNullOrEmpty(domicilio.AgebId) ? "" : domicilio.AgebId) + "','" +
                             (string.IsNullOrEmpty(domicilio.ManzanaId) ? "" : domicilio.ManzanaId) + "','" +
                             (string.IsNullOrEmpty(domicilio.CarreteraId) ? "" : domicilio.CarreteraId) + "','" +
                             (string.IsNullOrEmpty(domicilio.CaminoId) ? "" : domicilio.CaminoId) + "','" +
                             (string.IsNullOrEmpty(domicilio.TipoAsentamientoId) ? "" : domicilio.TipoAsentamientoId) + "','" +
                             (string.IsNullOrEmpty(domicilio.NombreAsentamiento) ? "" : domicilio.NombreAsentamiento) + "','" +
                             (string.IsNullOrEmpty(domicilio.DomicilioN) ? "" : domicilio.DomicilioN) + "','" +
                             (string.IsNullOrEmpty(domicilio.EntreCalle1) ? "" : domicilio.EntreCalle1) + " " +
                             (string.IsNullOrEmpty(domicilio.EntreCalle2) ? "" : domicilio.EntreCalle2) + "','" +
                             (string.IsNullOrEmpty(domicilio.CallePosterior) ? "" : domicilio.CallePosterior) + "','" +
                             (string.IsNullOrEmpty(domicilio.NumExterior) ? "" : domicilio.NumExterior) + "','" +
                             (string.IsNullOrEmpty(domicilio.NumInterior) ? "" : domicilio.NumInterior) + "','" +
                             (string.IsNullOrEmpty(domicilio.CodigoPostal) ? "" : domicilio.CodigoPostal) + "','" +
                             (string.IsNullOrEmpty(domicilio.Telefono) ? "" : domicilio.Telefono)+"',";
                }
                else
                {
                    query += "localidad_id='" + localidadId + "',ageb_id='" + agebId + "',manzana_id='" + manzanaId + "',cab_idencuestador='" + aplicacion.TrabajadorId + "',nombre_encuestador='" + aplicacion.Trabajador.Nombre + "',cab_idencuesta='" + beneficiario.Folio + "',cab_fechaencuesta='" + aplicacion.FechaInicio.Value.ToString("yyyy-MM-dd") + "'" +
                             ",cab_horaencuesta='" + aplicacion.FechaInicio.Value.ToString("HH:mm") + "',cab_fechafinencuesta='" + (aplicacion.FechaFin.HasValue ? aplicacion.FechaFin.Value.ToString("yyyy-MM-dd") : "") + "',cab_horafinencuesta='" + (aplicacion.FechaFin.HasValue ? aplicacion.FechaFin.Value.ToString("HH:mm") : "") + "',"
                             + "cab_tiempoencuesta=" + calcularTiempoEncuesta(aplicacion.Tiempo, aplicacion.FechaInicio, aplicacion.FechaFin, aplicacion.UpdatedAt) + ",cab_fechainsert='" + aplicacion.CreatedAt.ToString("yyyy-MM-dd") + "',fecha_sync='" + aplicacion.FechaSincronizacion.Value.ToString("yyyy-MM-dd") + "',hora_sync='" + aplicacion.FechaSincronizacion.Value.ToString("HH:mm") + "'" +
                             ",cab_latitud='" + domicilio.LatitudAtm + "',cab_longitud='" + domicilio.LongitudAtm + "',latitud_corregida='" + (string.IsNullOrEmpty(domicilio.LatitudCorregida) ? "" : domicilio.LatitudCorregida) + "'" +
                             ",longitud_corregida='" + (string.IsNullOrEmpty(domicilio.LongitudCorregida) ? "" : domicilio.LongitudCorregida) + "',estatus_api='"
                             + (string.IsNullOrEmpty(domicilio.EstatusDireccion) ? "" : domicilio.EstatusDireccion) + "',T=" + aplicacion.NumeroAplicacion + ",id_viv='"+domicilio.Id+"',NumHogar=" + beneficiario.NumFamilia + ",Version='" + aplicacion.VersionAplicacion + "',cve_mun='" + domicilio.ClaveMunicipioCalculado + "',nom_mun='" + domicilio.MunicipioCalculado + "',cve_loc='" +
                             domicilio.ClaveLocalidadCalculada + "',nom_localidad='" + domicilio.LocalidadCalculado + "',cve_ageb='" + domicilio.AgebCalculado + "',cve_manzana='" + domicilio.ManzanaCalculado + "',bandera_geografica='" +
                             (string.IsNullOrEmpty(domicilio.TipoCalculo) ? "" : domicilio.TipoCalculo) + "',cve_poli='" + domicilio.ClaveZonaImpulsoCalculada + "',clave_zona_impulso='" + domicilio.ZonaImpulsoCalculada + "',zap='" +
                             domicilio.ZapCalculado + "',tipozap='" + domicilio.TipoZap + "',colonia_oficial='" + (string.IsNullOrEmpty(domicilio.ColoniaCalculada) ? "" : domicilio.ColoniaCalculada) + "',codigo_postal='" +
                             (string.IsNullOrEmpty(domicilio.CodigoPostalCalculado) ? "" : domicilio.CodigoPostalCalculado) + "',clave_municipio_cis='" +
                             (string.IsNullOrEmpty(domicilio.ClaveMunicipioCisCalculado) ? "" : domicilio.ClaveMunicipioCisCalculado) + "',nombre_oficial_cis='" +
                             (string.IsNullOrEmpty(domicilio.CisCercano) ? "" : domicilio.CisCercano) + "',domicilio_cis='" +
                             (string.IsNullOrEmpty(domicilio.DomicilioCisCalculado) ? "" : domicilio.DomicilioCisCalculado) + "',idh_municipio='" +
                             domicilio.IndiceDesarrolloHumano + "',marginacion_ageb='" + domicilio.MarginacionAgeb + "',marginacion_localidad='" +
                             domicilio.MarginacionLocalidad + "',marginacion_municipio='" + domicilio.MarginacionMunicipio + "',curp_renapo='" + (integrante.Value.ContainsKey("CURP") ? integrante.Value["CURP"]["0"] : "") + "',telefono_casa='" + domicilio.TelefonoCasa + "',email='"+domicilio.Email+"',var969_1='" +
                             (string.IsNullOrEmpty(domicilio.MunicipioId) ? "" : domicilio.MunicipioId) + "',var970_1='" +
                             (string.IsNullOrEmpty(domicilio.LocalidadId) ? "" : domicilio.LocalidadId) + "',var971_1='" +
                             (string.IsNullOrEmpty(domicilio.AgebId) ? "" : domicilio.AgebId) + "',var972_1='" +
                             (string.IsNullOrEmpty(domicilio.ManzanaId) ? "" : domicilio.ManzanaId) + "',var973_1='" +
                             (string.IsNullOrEmpty(domicilio.CarreteraId) ? "" : domicilio.CarreteraId) + "',var974_1='" +
                             (string.IsNullOrEmpty(domicilio.CaminoId) ? "" : domicilio.CaminoId) + "',var975_1='" +
                             (string.IsNullOrEmpty(domicilio.TipoAsentamientoId) ? "" : domicilio.TipoAsentamientoId) + "',var976_1='" +
                             (string.IsNullOrEmpty(domicilio.NombreAsentamiento) ? "" : domicilio.NombreAsentamiento) + "',var977_1='" +
                             (string.IsNullOrEmpty(domicilio.DomicilioN) ? "" : domicilio.DomicilioN) + "',var978_1='" +
                             (string.IsNullOrEmpty(domicilio.EntreCalle1) ? "" : domicilio.EntreCalle1) + " " +
                             (string.IsNullOrEmpty(domicilio.EntreCalle2) ? "" : domicilio.EntreCalle2) + "',var979_1='" +
                             (string.IsNullOrEmpty(domicilio.CallePosterior) ? "" : domicilio.CallePosterior) + "',var980_1='" +
                             (string.IsNullOrEmpty(domicilio.NumExterior) ? "" : domicilio.NumExterior) + "',var981_1='" +
                             (string.IsNullOrEmpty(domicilio.NumInterior) ? "" : domicilio.NumInterior) + "',var982_1='" +
                             (string.IsNullOrEmpty(domicilio.CodigoPostal) ? "" : domicilio.CodigoPostal) + "',var983_1='" +
                             (string.IsNullOrEmpty(domicilio.Telefono) ? "" : domicilio.Telefono) + "',";
                }

                foreach (var preguntaDb in preguntasDB.Keys)
                {
                    var items = preguntaDb.Split("_");
                    var preguntaId = items.Length == 2 ? items[0].Replace("var",""):items[1].Replace("tx1","").Replace("tx2","");
                    if (integrante.Value.ContainsKey(preguntaId))
                    {
                        var respuestaId = items.Length == 2 ? int.Parse(items[1]) : int.Parse(items[2]);
                        if (items[0].Contains("vartx1"))
                        {
                            var valor = preguntas[preguntaId].Gradual? "G"+respuestaId: "C"+respuestaId;
                            if (integrante.Value[preguntaId].ContainsKey(valor))
                            { 
                                query += hasAnterior ? preguntaDb+"='"+integrante.Value[preguntaId][valor]+"'," : "'"+integrante.Value[preguntaId][valor]+"',";
                            }
                            else
                            {
                                query += hasAnterior ? preguntaDb+"=null,":"null,";
                            }
                        }
                        else
                        {
                            if (items[0].Contains("vartx2"))
                            {
                                var valor = "C"+respuestaId;
                                if (integrante.Value[preguntaId].ContainsKey(valor))
                                {
                                    query += hasAnterior ? preguntaDb+"='"+integrante.Value[preguntaId][valor]+"',":"'"+integrante.Value[preguntaId][valor]+"',";
                                }
                                else {
                                    query += hasAnterior ? preguntaDb+"=null,":"null,";
                                }   
                            }
                            else
                            {
                                if (integrante.Value[preguntaId].ContainsKey(respuestaId.ToString()))
                                {
                                    query += hasAnterior ? preguntaDb+"='"+integrante.Value[preguntaId][respuestaId.ToString()]+"',":"'"+integrante.Value[preguntaId][respuestaId.ToString()]+"',";
                                }
                                else {
                                    query += hasAnterior ? preguntaDb+"=null,":"null,";
                                }
                            }
                        }
                        
                    } else {
                        query += hasAnterior ? preguntaDb+"=null,":"null,";
                    }
                }
                query = query.Remove(query.Length - 1);
                query += hasAnterior?" WHERE id_fam='"+beneficiario.Id+"' and id_integrante="+integrante.Key: ")";
                registros.Add(query);
            }

            return registros;
        }

        private string calcularTiempoEncuesta(int? tiempo, DateTime? fechaInicio, DateTime? fechaFin, DateTime updatedAt)
        {
            if (tiempo == null || tiempo == 0)
            {
                var fechaFinCalculada = !fechaFin.HasValue || fechaFin.Value.Year == 1 ? updatedAt : fechaFin.Value;
                return Convert.ToInt32((fechaFinCalculada - fechaInicio.Value).TotalSeconds).ToString();
            }
            return tiempo.ToString();
        }
    }
}