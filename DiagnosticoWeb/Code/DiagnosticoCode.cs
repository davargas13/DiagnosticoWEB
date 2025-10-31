using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using DiagnosticoWeb.Database;
using DiagnosticoWeb.Models;

namespace DiagnosticoWeb.Code
{
    public class DiagnosticoCode
    {
        private readonly ApplicationDbContext _context;

        public DiagnosticoCode(ApplicationDbContext context)
        {
            _context = context;
        }

        public KeyValuePair<string, List<string>> BuildBaseMadreQuery(Diagnostico request, string inicio, string fin)
        {
            var campos = new List<string>();
            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = N'Sabana'";
                _context.Database.OpenConnection();
                using (var reader = command.ExecuteReader())
                {
                    var camposAIgnorar = new List<string>
                    {
                        "createdat",
                        "updatedat",
                        "localidad_id",
                        "ageb_id",
                        "manzana_id"
                    };
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var campo = reader.GetString(0);
                            if (!camposAIgnorar.Contains(campo))
                            {
                                campos.Add(campo);
                            }
                        }
                    }
                }
                _context.Database.CloseConnection();
            }

            var query = "SELECT * FROM ResultadosEncuesta_M WHERE ";//"select "+string.Join(',', campos)+" from sabana where ";
            //FechaDomicilioCreado >= " + inicio + " AND FechaDomicilioCreado < DATEADD(DAY, 1," + fin + ")
            var valores = new List<string>();
            var iFiltro = 0;
            if (!string.IsNullOrEmpty(inicio))
            {
                query += "FechaDomicilioCreado >= @" + iFiltro++;
                valores.Add(inicio);
            }
            if (!string.IsNullOrEmpty(fin))
            {
                query += iFiltro == 0 ? " " :" and " + "FechaDomicilioCreado <= @" + iFiltro++;
                valores.Add(fin);
            } 
            //if (!string.IsNullOrEmpty(request.Municipio))
            //{
            //    query += iFiltro == 0 ? " " :" and " + "cve_mun = @"+iFiltro++;
            //    valores.Add("11"+request.Municipio);
            //} 
            //if (!string.IsNullOrEmpty(request.Localidad))
            //{
            //    query += iFiltro == 0 ? " " :" and " + "localidad_id = @"+iFiltro++;
            //    valores.Add(request.Localidad);
            //}
            //if (!string.IsNullOrEmpty(request.Ageb))
            //{
            //    query += iFiltro == 0 ? " " :" and " + "ageb_id = @"+iFiltro++;
            //    valores.Add(request.Ageb);
            //}
            //if (!string.IsNullOrEmpty(request.Manzana))
            //{
            //    query += iFiltro == 0 ? " " :" and " + "manzana_id = @"+iFiltro++;
            //    valores.Add(request.Manzana);
            //}
            //if (!string.IsNullOrEmpty(request.CodigoPostal))
            //{
            //    query += iFiltro == 0 ? " " :" and " + "codigo_postal = @"+iFiltro++;
            //    valores.Add(request.CodigoPostal);
            //}
            //if (!string.IsNullOrEmpty(request.Familia))
            //{
            //    query += iFiltro == 0 ? " " :" and " + "cab_idencuestador = @"+iFiltro++;
            //    valores.Add(request.Familia);
            //}
            //if (!string.IsNullOrEmpty(request.Encuestador))
            //{
            //    query += iFiltro == 0 ? " " :" and " + "id_fam = @"+iFiltro++;
            //    valores.Add(request.Encuestador);
            //} 
            return new KeyValuePair<string, List<string>>(query, valores);
        }
        
        public static void ActualizarDomicilioEnSabana(ApplicationDbContext conexion, Domicilio domicilio){
            var query = "UPDATE Sabana set latitud_corregida='" + (string.IsNullOrEmpty(domicilio.LatitudCorregida) ? "" : domicilio.LatitudCorregida) + "'" +
                        ",longitud_corregida='" + (string.IsNullOrEmpty(domicilio.LongitudCorregida) ? "" : domicilio.LongitudCorregida) + "',estatus_api='"
                        + (string.IsNullOrEmpty(domicilio.EstatusDireccion) ? "" : domicilio.EstatusDireccion) + "',cve_mun='" + domicilio.ClaveMunicipioCalculado + "',nom_mun='" + domicilio.MunicipioCalculado + "',cve_loc='" +
                        domicilio.ClaveLocalidadCalculada + "',nom_localidad='" + domicilio.LocalidadCalculado + "',cve_ageb='" + domicilio.AgebCalculado + "',cve_manzana='" + domicilio.ManzanaCalculado + "',bandera_geografica='" +
                        (string.IsNullOrEmpty(domicilio.TipoCalculo) ? "" : domicilio.TipoCalculo) + "',cve_poli='" + domicilio.ClaveZonaImpulsoCalculada + "',clave_zona_impulso='" + domicilio.ZonaImpulsoCalculada + "',zap='" +
                        domicilio.ZapCalculado + "',tipozap='" + domicilio.TipoZap + "',colonia_oficial='" + (string.IsNullOrEmpty(domicilio.ColoniaCalculada) ? "" : domicilio.ColoniaCalculada) + "',codigo_postal='" +
                        (string.IsNullOrEmpty(domicilio.CodigoPostalCalculado) ? "" : domicilio.CodigoPostalCalculado) + "',clave_municipio_cis='" +
                        (string.IsNullOrEmpty(domicilio.ClaveMunicipioCisCalculado) ? "" : domicilio.ClaveMunicipioCisCalculado) + "',nombre_oficial_cis='" +
                        (string.IsNullOrEmpty(domicilio.CisCercano) ? "" : domicilio.CisCercano) + "',domicilio_cis='" +
                        (string.IsNullOrEmpty(domicilio.DomicilioCisCalculado) ? "" : domicilio.DomicilioCisCalculado) + "',idh_municipio='" +
                        domicilio.IndiceDesarrolloHumano + "',marginacion_ageb='" + domicilio.MarginacionAgeb + "',marginacion_localidad='" +
                        domicilio.MarginacionLocalidad + "',marginacion_municipio='" + domicilio.MarginacionMunicipio + "' WHERE id_viv='"+domicilio.Id+"'";
             conexion.Database.ExecuteSqlCommand(query);
        }
        
        public static void ActualizarCurpEnSabana(ApplicationDbContext conexion, Beneficiario integrante){
            var query = "UPDATE Sabana set curp_renapo='"+integrante.Curp+"' WHERE id_fam='"+(integrante.PadreId??integrante.Id)+"' and id_integrante="+integrante.NumIntegrante;
             conexion.Database.ExecuteSqlCommand(query);
        }
    }
    
}