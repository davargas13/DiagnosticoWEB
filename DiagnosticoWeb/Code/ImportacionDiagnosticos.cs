using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using DiagnosticoWeb.Database;
using DiagnosticoWeb.Models;
using static System.Guid;

namespace DiagnosticoWeb.Code
{
    public class ImportacionDiagnosticos
    {
        private readonly DiagnosticoAnteriorDbContext _diagnosticoContext;
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private const string Llave = "tresfact";
        private readonly IHttpClientFactory _clientFactory;
        public ImportacionDiagnosticos(DiagnosticoAnteriorDbContext diagnosticoContext, ApplicationDbContext context,
            IConfiguration configuration, IHttpClientFactory clientFactory)
        {
            _diagnosticoContext = diagnosticoContext;
            _context = context;
            _configuration = configuration;
            _clientFactory = clientFactory;
        }

        private HashSet<int> CalcularPreguntasConComplemento()
        {
            var preguntasConComplementoDic = new HashSet<int>();

            var preguntasDB = _context.Pregunta.Where(p => p.DeletedAt == null).Include(p => p.Respuestas).ToList();
            foreach (var pregunta in preguntasDB)
            {
                if (pregunta.Complemento != null)
                {
                    preguntasConComplementoDic.Add(int.Parse(pregunta.Id));
                }
                else
                {
                    if (pregunta.Respuestas.Any(r => r.Complemento != null))
                    {
                        preguntasConComplementoDic.Add(int.Parse(pregunta.Id));
                    }
                }
            }

            return preguntasConComplementoDic;
        }

        public void importar(DateTime inicio, DateTime fin)
        {
            var beneficiario = new Beneficiario { Referencia = "", NumIntegrante = 1 };
            var domicilio = new Domicilio();
            var aplicacion = new Aplicacion { Id = "" };
            var encuestador = new Trabajador { Id = "" };
            var encuestadorDependencia = new TrabajadorDependencia { Id = "" };

            var now = DateTime.Now;
            var password = Utils.HashDescryptableString(Llave, "sedeshu", true);
            var encuestaVersionId = _context.EncuestaVersion.First(ev => ev.Activa).Id;

            //Catalogos
            var parentescos = _context.Parentesco.ToDictionary(p => p.Nombre.Trim().ToLower());
            var sexos = _context.Sexo.ToDictionary(p => p.Nombre.Trim());
            var estados = _context.Estado.ToDictionary(p => p.Nombre.Trim().ToUpper());
            var asentamientos = _context.TipoAsentamiento.ToDictionary(p => p.Nombre.Trim().ToUpper());
            var respuestas = _context.Respuesta.ToDictionary(r => r.Id);
            var causasDiscapacidad = _context.CausaDiscapacidad.ToDictionary(cd => cd.Id);
            var grados = _context.DiscapacidadGrado.ToDictionary(cd => cd.DiscapacidadId + "-" + cd.Grado);
            var gradosDiscapadidad = new Dictionary<string, string>
            {
                {"No tiene dificultad","4"},
                {"Lo hace con poca dificultad","3"},
                {"Lo hace con mucha dificultad","2"},
                {"No puede hacerlo","1"}
            };
            var articulos = _context.Respuesta.Where(r => !r.Id.Equals("627") && r.PreguntaId.Equals("117")).ToDictionary(dg => dg.Id);
            var trabajadores = _context.Trabajador.ToDictionary(t => t.Id);
            var resultados = _context.Respuesta.Where(r => r.PreguntaId.Equals("138")).ToDictionary(r => r.Id);
            var sedeshu = _context.Dependencia.First(d => d.Nombre.Equals("Secretaría de Desarrollo Social y Humano"));

            //Diccionarios
            var trabajadoresList = new List<Trabajador>();
            var trabajadoresDependenciasList = new List<TrabajadorDependencia>();
            var beneficiariosList = new List<Beneficiario>();
            var domiciliosList = new List<Domicilio>();
            var aplicacionesList = new List<Aplicacion>();
            var aplicacionPreguntasList = new List<AplicacionPregunta>();

            var articulosDic = new Dictionary<string, AplicacionPregunta>();
            var preguntasConComplementoDic = CalcularPreguntasConComplemento();
            string padreId = null;

            using (var command = _diagnosticoContext.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText =
                    "select A.id, A.idencuesta,A.idencuestador,A.fechaencuesta,A.fechafinencuesta, AP.idpregunta, " +
                    "AP.idrespuesta, AP.txtrespuesta, AP.idintegrante, AP.id, A.latitud, A.longitud, A.FechaInsert " +
                    "from tblDatosApp A inner join tblEncuestaSabana AP on A.id = AP.idcabrespact " +
                    "where A.fechaencuesta >='" + inicio.ToString("yyyy-MM-dd") + "' and " +
                    "A.fechaencuesta<'" + fin.ToString("yyyy-MM-dd") + "' " +
                    "order by A.Id, AP.idintegrante, AP.idpregunta";
                _diagnosticoContext.Database.OpenConnection();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var id = reader.GetInt64(0).ToString();
                            var folio = reader.GetString(1);
                            var numIntegrante = reader.GetByte(8);

                            if (!trabajadores.ContainsKey(reader.GetInt64(2).ToString()))
                            {
                                encuestador = new Trabajador
                                {
                                    Id = reader.GetInt64(2).ToString(),
                                    Nombre = encuestador.Id,
                                    Username = "Trabajador " + encuestador.Id,
                                    Password = password,
                                    Codigo = encuestador.Id,
                                    Email = "",
                                    CreatedAt = now,
                                    UpdatedAt = now,
                                };
                                trabajadores.Add(encuestador.Id, encuestador);
                                encuestadorDependencia = new TrabajadorDependencia
                                {
                                    Id = NewGuid().ToString(),
                                    DependenciaId = sedeshu.Id,
                                    TrabajadorId = encuestador.Id,
                                    CreatedAt = now,
                                    UpdatedAt = now
                                };
                                trabajadoresList.Add(encuestador);
                                trabajadoresDependenciasList.Add(encuestadorDependencia);

                            }
                            else
                            {
                                encuestador = trabajadores[reader.GetInt64(2).ToString()];
                            }

                            if (!beneficiario.Referencia.Equals(id))
                            {
                                var latitud = reader.GetString(10);
                                var longitud = reader.GetString(11);
                                beneficiario = new Beneficiario
                                {
                                    Id = id,
                                    Nombre = id,
                                    Referencia = id,
                                    ApellidoPaterno = "",
                                    ApellidoMaterno = "",
                                    FechaNacimiento = now,
                                    EstadoId = "1",
                                    Curp = "",
                                    Rfc = "",
                                    Comentarios = "",
                                    EstudioId = "506",
                                    EstadoCivilId = "3",
                                    DiscapacidadId = "124",
                                    Estatus = true,
                                    SexoId = "1",
                                    TrabajadorId = encuestador.Id,
                                    EstatusInformacion = EstatusInformacion.ENCUESTA,
                                    CreatedAt = inicio,
                                    UpdatedAt = inicio,
                                    EstatusUpdatedAt = inicio,
                                    GradoEstudioId = "567",
                                    NumIntegrante = 1,
                                    NumFamilia = 1,
                                    Folio = folio
                                };
                                domicilio = new Domicilio
                                {
                                    Id = id,
                                    Telefono = "",
                                    TelefonoCasa = "",
                                    Email = "",
                                    DomicilioN = "",
                                    NumFamilia = 1,
                                    NumFamiliasRegistradas = 1,
                                    EntreCalle1 = "",
                                    EntreCalle2 = "",
                                    CodigoPostal = "",
                                    CreatedAt = inicio,
                                    UpdatedAt = inicio,
                                    MunicipioId = "001",
                                    LocalidadId = "110010001",
                                    TrabajadorId = encuestador.Id,
                                    Activa = true,
                                    CadenaOCR = "",
                                    NombreAsentamiento = "",
                                    CallePosterior = "",
                                    Latitud = latitud,
                                    LatitudAtm = latitud,
                                    Longitud = longitud,
                                    LongitudAtm = longitud,
                                    Indice = 0
                                };
                                beneficiario.DomicilioId = domicilio.Id;
                                beneficiariosList.Add(beneficiario);
                                domiciliosList.Add(domicilio);
                            }

                            if (numIntegrante == 0)
                            {
                                padreId = "" + beneficiario.Id;
                            }

                            if (!aplicacion.Id.Equals(id))
                            {
                                aplicacion = new Aplicacion
                                {
                                    Id = id,
                                    CreatedAt = reader.GetDateTime(12),
                                    UpdatedAt = reader.GetDateTime(4),
                                    FechaInicio = reader.GetDateTime(3),
                                    FechaFin = reader.GetDateTime(4),
                                    FechaSincronizacion = now,
                                    TrabajadorId = encuestador.Id,
                                    BeneficiarioId = beneficiario.Id,
                                    Activa = true,
                                    Estatus = EstatusAplicacion.COMPLETO,
                                    NumeroAplicacion = 1,
                                    EncuestaVersionId = encuestaVersionId
                                };
                                aplicacionesList.Add(aplicacion);
                                articulosDic = new Dictionary<string, AplicacionPregunta>();
                                foreach (var articulo in articulos)
                                {
                                    var aplicacionPregunta = new AplicacionPregunta
                                    {
                                        Id = NewGuid().ToString(),
                                        AplicacionId = aplicacion.Id,
                                        PreguntaId = "117",
                                        RespuestaIteracion = numIntegrante > 0 ? (numIntegrante - 1) : 0,
                                        CreatedAt = reader.GetDateTime(3),
                                        UpdatedAt = reader.GetDateTime(4),
                                        Grado = "0",
                                        Complemento = "0",
                                        RespuestaId = articulo.Key
                                    };
                                    articulosDic.Add(articulo.Key, aplicacionPregunta);
                                    aplicacionPreguntasList.Add(aplicacionPregunta);
                                }
                            }

                            var ignorar = false;
                            var preguntaId = reader.GetInt32(5);
                            var valorId = reader.GetInt32(6).ToString();
                            var valor = reader.IsDBNull(7) ? "" : Utils.LimpiarCadena(reader.GetString(7));
                            string catalogo = null;

                            if (numIntegrante == 0)
                            {
                                switch (preguntaId)
                                {
                                    case 969:
                                        domicilio.MunicipioId = valor.Trim().PadLeft(3, '0');
                                        ignorar = true;
                                        break;
                                    case 970:
                                        domicilio.LocalidadId = "11" + domicilio.MunicipioId + valor.PadLeft(4, '0');
                                        ignorar = true;
                                        break;
                                    case 971:
                                        domicilio.AgebId = valor;
                                        ignorar = true;
                                        break;
                                    case 972:
                                        domicilio.ManzanaId = valor;
                                        ignorar = true;
                                        break;
                                    case 973:
                                        domicilio.CarreteraId = valor;
                                        ignorar = true;
                                        break;
                                    case 974:
                                        domicilio.CaminoId = valor;
                                        ignorar = true;
                                        break;
                                    case 975:
                                        domicilio.TipoAsentamientoId = asentamientos[valor.Trim().ToUpper()].Id;
                                        ignorar = true;
                                        break;
                                    case 976:
                                        domicilio.NombreAsentamiento = valor;
                                        ignorar = true;
                                        break;
                                    case 977:
                                        domicilio.DomicilioN = valor;
                                        ignorar = true;
                                        break;
                                    case 978:
                                        domicilio.EntreCalle1 = valor;
                                        ignorar = true;
                                        break;
                                    case 979:
                                        domicilio.CallePosterior = valor;
                                        ignorar = true;
                                        break;
                                    case 980:
                                        domicilio.NumExterior = valor;
                                        ignorar = true;
                                        break;
                                    case 981:
                                        domicilio.NumInterior = valor;
                                        ignorar = true;
                                        break;
                                    case 982:
                                        domicilio.CodigoPostal = valor;
                                        ignorar = true;
                                        break;
                                    case 983:
                                        domicilio.Telefono = valor;
                                        ignorar = true;
                                        break;
                                    default:
                                        ignorar = false;
                                        break;
                                }
                            }
                            else
                            {
                                if (beneficiario.NumIntegrante != numIntegrante)
                                {
                                    beneficiario = new Beneficiario
                                    {
                                        Id = NewGuid().ToString(),
                                        Nombre = id,
                                        Referencia = id,
                                        ApellidoPaterno = "",
                                        ApellidoMaterno = "",
                                        FechaNacimiento = now,
                                        EstadoId = "1",
                                        Curp = "",
                                        Rfc = "",
                                        Comentarios = "",
                                        EstudioId = "506",
                                        EstadoCivilId = "3",
                                        DiscapacidadId = "124",
                                        Estatus = true,
                                        SexoId = "1",
                                        TrabajadorId = encuestador.Id,
                                        EstatusInformacion = EstatusInformacion.ENCUESTA,
                                        CreatedAt = inicio,
                                        UpdatedAt = inicio,
                                        EstatusUpdatedAt = inicio,
                                        GradoEstudioId = "567",
                                        NumIntegrante = numIntegrante,
                                        PadreId = numIntegrante > 1 ? padreId : null,
                                        DomicilioId = domicilio.Id,
                                        Folio = folio
                                    };
                                    beneficiariosList.Add(beneficiario);
                                }

                                switch (preguntaId)
                                {
                                    case 993:
                                        catalogo = parentescos[valor.Trim().ToLower()].Id;
                                        break;
                                    case 998:
                                        catalogo = sexos[valor.Trim()].Id;
                                        break;
                                    case 999:
                                        catalogo = estados[valor.Trim().ToUpper().Equals("DISTRITO FEDERAL") ? "CIUDAD DE MÉXICO" : valor.Trim().ToUpper()].Id;
                                        break;
                                }
                            }

                            switch (preguntaId)
                            {
                                case 130:
                                    beneficiario.EstudioId = valorId;
                                    break;
                                case 131:
                                    beneficiario.GradoEstudioId = valorId;
                                    break;
                                case 24:
                                    beneficiario.EstadoCivilId = valorId;
                                    break;
                                case 989:
                                    beneficiario.Nombre = valor;
                                    break;
                                case 990:
                                    beneficiario.ApellidoPaterno = valor;
                                    break;
                                case 991:
                                    beneficiario.ApellidoMaterno = valor;
                                    break;
                                case 993:
                                    beneficiario.ParentescoId = parentescos[valor.Trim().ToLower()].Id;
                                    valorId = parentescos[valor.Trim().ToLower()].Id;
                                    break;
                                case 995:
                                    beneficiario.Curp = valor;
                                    break;
                                case 996:
                                    DateTime dt;
                                    DateTime.TryParseExact(valor,
                                        "dd-MM-yyyy",
                                        CultureInfo.InvariantCulture,
                                        DateTimeStyles.None,
                                        out dt);
                                    beneficiario.FechaNacimiento = dt;
                                    valor = valor.Substring(6, 4) + "/" + valor.Substring(3, 2) + "/" +
                                            valor.Substring(0, 2);
                                    break;
                                case 998:
                                    beneficiario.SexoId = sexos[valor.Trim()].Id;
                                    valorId = sexos[valor.Trim()].Id;
                                    break;
                                case 999:
                                    beneficiario.EstadoId = estados[valor.Trim().ToUpper().Equals("DISTRITO FEDERAL") ? "CIUDAD DE MÉXICO" : valor.Trim().ToUpper()].Id;
                                    valorId = estados[valor.Trim().ToUpper().Equals("DISTRITO FEDERAL") ? "CIUDAD DE MÉXICO" : valor.Trim().ToUpper()].Id;
                                    break;
                                case 138:
                                    aplicacion.Resultado = resultados[valorId == "0" ? "605" : valorId].Nombre;
                                    break;
                            }

                            if (!ignorar)
                            {
                                if (preguntaId == 42)
                                {
                                    beneficiario.DiscapacidadId = valorId;
                                    var grado = (string.IsNullOrEmpty(valor.Trim()) ? null : gradosDiscapadidad[valor.Trim()]);
                                    if (grado != null)
                                    {
                                        beneficiario.DiscapacidadGradoId = grados[valorId + "-" + grado].Id;
                                    }
                                }
                                if (preguntaId == 43)
                                {
                                    var causaId = valorId.Equals("1081") ? 1 : int.Parse(valorId) - 123;
                                    beneficiario.CausaDiscapacidadId = causasDiscapacidad["" + causaId].Id;
                                }
                                if (preguntaId == 117 && valorId != "627")
                                {
                                    if (preguntaId == 117)
                                    {
                                        var aplicacionPregunta = articulosDic[valorId];
                                        aplicacionPregunta.Id = reader.GetInt64(9).ToString();
                                        aplicacionPregunta.Complemento = valor.ToLower().Equals("sirve") ? "1" : "2";
                                        aplicacionPregunta.RespuestaId = valorId;
                                        aplicacionPregunta.Grado = "1";
                                    }
                                }
                                else
                                {
                                    var aplicacionPregunta = new AplicacionPregunta
                                    {
                                        Id = reader.GetInt64(9).ToString(),
                                        AplicacionId = aplicacion.Id,
                                        PreguntaId = preguntaId.ToString(),
                                        RespuestaId = valorId == "0"
                                            ? null
                                            : (respuestas.ContainsKey(valorId) ? valorId : null),
                                        Valor = string.IsNullOrEmpty(valor) ? null : valor,
                                        RespuestaIteracion = numIntegrante > 0 ? numIntegrante - 1 : 0,
                                        CreatedAt = reader.GetDateTime(3),
                                        UpdatedAt = reader.GetDateTime(4),
                                        ValorCatalogo = catalogo,
                                        Grado = preguntaId == 42 ? (string.IsNullOrEmpty(valor.Trim()) ? null : gradosDiscapadidad[valor.Trim()]) : null
                                    };
                                    if (preguntasConComplementoDic.Contains(preguntaId) && preguntaId != 42)
                                    {
                                        aplicacionPregunta.Complemento = valor;
                                    }

                                    if (preguntaId == 996)
                                    {
                                        DateTime dt;
                                        DateTime.TryParseExact(valor, "yyyy-MM-dd",
                                            CultureInfo.InvariantCulture,
                                            DateTimeStyles.None,
                                            out dt);
                                        aplicacionPregunta.ValorFecha = dt;
                                    }
                                    aplicacionPreguntasList.Add(aplicacionPregunta);
                                }
                            }
                        }
                    }

                    _diagnosticoContext.Database.CloseConnection();
                    using (var transaction = _context.Database.BeginTransaction())
                    {
                        _context.BulkInsert(trabajadoresList);
                        _context.BulkInsert(trabajadoresDependenciasList);
                        _context.BulkInsert(domiciliosList);     
                        _context.BulkInsert(beneficiariosList);
                        _context.BulkInsert(aplicacionesList);
                        _context.BulkInsert(aplicacionPreguntasList);
                        transaction.Commit();
                    }
                }
            }
        }

        public static async Task CompletarCurp(Beneficiario beneficiario, ApplicationDbContext db, string url, 
            string passwordCurp, DateTime now, HttpClient client, string idApiCurp)
        {
            var cadena = url+"/"+passwordCurp+"/"+ beneficiario.ApellidoPaterno + "/" + beneficiario.ApellidoMaterno + "/" +
                                         beneficiario.Nombre + "/"
                                         + beneficiario.Sexo.Nombre.Substring(0, 1).ToUpper() + "/" +
                                         beneficiario.FechaNacimiento.Value.ToString("yyyyMMdd") + "/" +
                                         beneficiario.Estado.Abreviacion+"/"+idApiCurp;            
            var request = new HttpRequestMessage(HttpMethod.Get, cadena);
            request.Headers.Add("User-Agent", "HttpClientFactory-Sample");
            request.Headers.Host = request.RequestUri.Host;
            try
            {
                var response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    using (var stream = response.Content.ReadAsStringAsync())
                    { 
                        var curpResponse = JsonConvert.DeserializeObject<CurpResponse>(stream.Result);
                        if (curpResponse.Mensaje.Equals("OK"))
                        {
                            beneficiario.Curp = curpResponse.Resultado.CurpCollection.First().CURP;
                            db.Beneficiario.Update(beneficiario);
                            db.SaveChanges();
                        }
                    }                    
                }
                else
                {
                    Excepcion.Registrar(new Exception(), url + " " + response.StatusCode.ToString());
                }
            }
            catch (Exception e)
            {                
                Excepcion.Registrar(e, url);
            }
        }

        public void CompletarCurps(DateTime inicio, DateTime fin)
        {
            var url = _configuration["WS_DATOS"];
            var passwordCurp = _configuration["CA_WS_CURP"];
            var idApiCurp = _configuration["ID_WS_CURP"];
            var conexion = _configuration.GetConnectionString("DefaultConnection");
            new Thread(async () => {
                var now = new DateTime();
                var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
                optionsBuilder.UseSqlServer(conexion);
                using (var db = new ApplicationDbContext(optionsBuilder.Options))
                {
                    try
                    {
                        var client = _clientFactory.CreateClient();
                        var beneficiarios = db.Beneficiario.Where(b => b.Curp.Length < 18 
                                                                       && b.CreatedAt >= inicio && b.CreatedAt <= fin
                                                                       && !b.Nombre.Equals("") && !b.ApellidoPaterno.Equals("") && !b.ApellidoMaterno.Equals(""))
                            .Include(b => b.Estado).Include(b => b.Sexo).ToList();
                        foreach (var beneficiario in beneficiarios)
                        {
                            await CompletarCurp(beneficiario, db, url, passwordCurp, now, client, idApiCurp);
                        }
                    }
                    catch (Exception e)
                    {
                        Excepcion.Registrar(e);
                    }
                }
            }).Start();
        }

        public void CompletarDirecciones(DateTime inicio, DateTime fin)
        {
            var url = _configuration["WS_DIRECCION"];
            var conexion = _configuration.GetConnectionString("DefaultConnection");            
            new Thread(async () => {
                var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
                optionsBuilder.UseSqlServer(conexion);
                using (var db = new ApplicationDbContext(optionsBuilder.Options))
                {
                    var client = _clientFactory.CreateClient();
                    var domicilios = db.Domicilio.Where(m => m.DeletedAt == null && m.Activa && m.MunicipioCalculado == null && m.LatitudAtm != ""
                        && m.LatitudAtm != "0.0" && (m.LatitudCorregida == null || m.LatitudCorregida != m.LongitudCorregida
                        && m.CreatedAt >= inicio && m.CreatedAt <= fin)).ToList();
                    foreach (var domicilio in domicilios)
                    {
                        try
                        {
                            await CompletarDomicilio(domicilio, url, client);
                            db.Domicilio.Update(domicilio);
                            db.SaveChanges();
                        }
                        catch (Exception e)
                        {
                            Excepcion.Registrar(e);
                        }
                    }
                }
            }).Start();            
        }

        public static async Task CompletarDomicilio(Domicilio domicilio, string url, HttpClient client)
        {
            var cadena = domicilio.LatitudCorregida == null ? ("lat=" + domicilio.LatitudAtm + "&lon=" + domicilio.LongitudAtm) : ("lat=" + domicilio.LatitudCorregida + "&lon=" + domicilio.LongitudCorregida);
            var request = new HttpRequestMessage(HttpMethod.Get,url+cadena+"&periodo="+domicilio.CreatedAt.ToString("yyyy"));
            request.Headers.Add("User-Agent", "HttpClientFactory-Sample");
            request.Headers.Host = request.RequestUri.Host;
            try
            {
                var response = await client.SendAsync(request);                
                if (response.IsSuccessStatusCode)
                {
                    using (var stream = response.Content.ReadAsStringAsync()) {
                        var json = JsonConvert.DeserializeObject(stream.Result);
                        foreach (var item in (JObject)json)
                        {
                            if (item.Value.ToString().Length > 0)
                            {
                                domicilio.EstatusDireccion = domicilio.MunicipioCalculado == "ND" ? "Corregida" : "Correcta";
                                domicilio.NumIntentosCoordenadas = 0;
                                switch (item.Key)
                                {
                                    case "marginacion_loc":
                                        domicilio.MarginacionLocalidad = Utils.LimpiarCadena(item.Value.ToString());
                                        break;
                                    case "manzana":
                                        domicilio.ManzanaCalculado = Utils.LimpiarCadena(item.Value.ToString());
                                        break;
                                    case "idh_municipal":
                                        domicilio.IndiceDesarrolloHumano = Utils.LimpiarCadena(item.Value.ToString());
                                        break;
                                    case "colonia":
                                        domicilio.ColoniaCalculada = Utils.LimpiarCadena(item.Value.ToString());
                                        break;
                                    case "bandera":
                                        domicilio.TipoCalculo = Utils.LimpiarCadena(item.Value[0].ToString());
                                        break;
                                    case "marginacion_mun":
                                        domicilio.MarginacionMunicipio = Utils.LimpiarCadena(item.Value.ToString());
                                        break;
                                    case "zap":
                                        domicilio.ZapCalculado = Utils.LimpiarCadena(item.Value.ToString());
                                        break;
                                    case "marginacion_ageb":
                                        domicilio.MarginacionAgeb = Utils.LimpiarCadena(item.Value.ToString());
                                        break;
                                    case "ageb":
                                        domicilio.AgebCalculado = Utils.LimpiarCadena(item.Value.ToString());
                                        break;
                                    case "tipo_zap":
                                        domicilio.TipoZap = Utils.LimpiarCadena(item.Value.ToString());
                                        break;
                                    case "cp":
                                        domicilio.CodigoPostalCalculado = Utils.LimpiarCadena(item.Value.ToString());
                                        break;
                                    case "zis":
                                        if (item.Value.GetType() == typeof(JArray))
                                        {
                                            domicilio.ClaveZonaImpulsoCalculada = Utils.LimpiarCadena(item.Value[1].ToString());
                                            domicilio.ZonaImpulsoCalculada = Utils.LimpiarCadena(item.Value[0].ToString());
                                        }
                                        else
                                        {
                                            domicilio.ClaveZonaImpulsoCalculada = Utils.LimpiarCadena(item.Value.ToString());
                                            domicilio.ZonaImpulsoCalculada = Utils.LimpiarCadena(item.Value.ToString());
                                        }
                                        break;
                                    case "cis":
                                        if (item.Value.GetType() == typeof(JArray))
                                        {
                                            domicilio.ClaveMunicipioCisCalculado = Utils.LimpiarCadena(item.Value[0].ToString());
                                            domicilio.CisCercano = Utils.LimpiarCadena(item.Value[1].ToString());
                                            domicilio.DomicilioCisCalculado = Utils.LimpiarCadena(item.Value[2].ToString());
                                        }
                                        else
                                        {
                                            domicilio.ClaveMunicipioCisCalculado = Utils.LimpiarCadena(item.Value.ToString());
                                            domicilio.CisCercano = Utils.LimpiarCadena(item.Value.ToString());
                                            domicilio.DomicilioCisCalculado = Utils.LimpiarCadena(item.Value.ToString());
                                        }
                                        break;
                                    case "Localidad":
                                        if (item.Value.GetType() == typeof(JArray))
                                        {
                                            domicilio.ClaveLocalidadCalculada = Utils.LimpiarCadena(item.Value[0].ToString());
                                            domicilio.LocalidadCalculado = Utils.LimpiarCadena(item.Value[1].ToString());
                                        }
                                        else
                                        {
                                            var localidadArray = Utils.LimpiarCadena(item.Value.ToString()).Split("@");
                                            if (localidadArray.Length > 1)
                                            {
                                                var codigo1 = localidadArray[0].Split(",");
                                                var codigo2 = localidadArray[0].Split(",");
                                                domicilio.ClaveLocalidadCalculada = codigo1[0] + "@" + codigo2[0];
                                                domicilio.LocalidadCalculado = codigo1[1] + "@" + codigo2[1];
                                            }
                                            else {
                                                var codigo1 = localidadArray[0].Split(",");                                                
                                                domicilio.ClaveLocalidadCalculada = codigo1[0] + codigo1[1];                                                
                                            }
                                        }
                                        break;
                                    case "municipio":
                                        if (item.Value.GetType() == typeof(JArray))
                                        {
                                            domicilio.ClaveMunicipioCalculado = Utils.LimpiarCadena(item.Value[0].ToString());
                                            domicilio.MunicipioCalculado = Utils.LimpiarCadena(item.Value[1].ToString());
                                        }
                                        else
                                        {
                                            domicilio.ClaveMunicipioCalculado = Utils.LimpiarCadena(item.Value.ToString());
                                            domicilio.MunicipioCalculado = Utils.LimpiarCadena(item.Value.ToString());
                                        }
                                        break;
                                }
                            }
                        }
                    }
                }
                else
                {
                    domicilio.NumIntentosCoordenadas++;
                    domicilio.EstatusDireccion = "Error";
                    Excepcion.Registrar(new Exception(), url+" "+ response.StatusCode.ToString());
                }
            }
            catch (Exception e)
            {
                domicilio.NumIntentosCoordenadas++;
                domicilio.EstatusDireccion = "Error";
                Excepcion.Registrar(e, url);
            }
            if (domicilio.MunicipioCalculado == "ND")
            {
                domicilio.NumIntentosCoordenadas++;
                domicilio.EstatusDireccion = "Error";
            }
            domicilio.UpdatedAt = DateTime.Now;
        }

        public void CompletarCarencias(DateTime inicio, DateTime fin)
        {
            var aplicaciones = _context.Aplicacion.Where(a => a.DeletedAt == null && a.Activa 
                    && a.Estatus==EstatusAplicacion.COMPLETO && a.FechaInicio>=inicio && a.FechaFin<=fin)
                .Include(a => a.Beneficiario).ThenInclude(b => b.Domicilio).ThenInclude(d => d.Municipio).ToList();
            foreach (var aplicacion in aplicaciones)
            {
                var domicilio = aplicacion.Beneficiario.Domicilio;
                var coneval = new Coneval();
                coneval.CalcularCarencias(aplicacion.Id, domicilio.TipoAsentamientoId, domicilio.Municipio.Indice, _context, null);
            }
        }
    }
}