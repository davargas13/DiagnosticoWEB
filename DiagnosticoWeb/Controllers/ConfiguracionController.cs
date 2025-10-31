using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query.ExpressionVisitors.Internal;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using DiagnosticoWeb.Code;
using DiagnosticoWeb.Database;
using DiagnosticoWeb.Models;
using DiagnosticoWeb.Validaciones;

namespace DiagnosticoWeb.Controllers
{
    public class ConfiguracionController : Controller
    { 
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public ConfiguracionController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, 
            IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _configuration = configuration;
        }
        
        [HttpGet]
        [Authorize]
        public IActionResult Index()
        {
            return View("Index");
        }

        [HttpPost]
        [Authorize]
        public string GetConfiguracion()
        {
            var response = new ConfiguracionResponse();
            var configuraciones = _context.Configuracion.ToList();

            response.Configuraciones = configuraciones;
            
            return JsonConvert.SerializeObject(response);
        }

        [HttpGet]
        [Authorize]
        public IActionResult CreateEdit(string id = "")
        {
            var conf = _context.Configuracion.Find(id);
            
            return View("CreateEdit", conf);
        }

        [HttpPost]
        [Authorize]
        public string Guardar([FromBody] ConfiguracionModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Select(x => new {x.Key, Error = x.Value.Errors.First().ErrorMessage})
                    .ToList();
                return JsonConvert.SerializeObject(errors);
            }

            var configuracion = _context.Configuracion.Find(model.Id);
            configuracion.Nombre = model.Nombre;
            configuracion.Valor = model.Valor;
            configuracion.UpdatedAt = DateTime.Now;

            _context.Configuracion.Update(configuracion);
            _context.SaveChanges();
            
            TempData["Alert"] = "Show";
            TempData.Keep("Alert");

            return "ok";
        }

        [HttpGet]
        [Authorize]
        [RequireClaim("Permiso", Value = "algoritmo.gestionar")]
        public IActionResult Algoritmo()
        {
            var numIntentos = 5;
            var numIntentosConf = _configuration["NUMERO_PRUEBAS_ALGORITMO"];
            if (!string.IsNullOrEmpty(numIntentosConf))
            {
                numIntentos = int.Parse(numIntentosConf);
            }
            var configuracionDb = _context.Configuracion.FirstOrDefault(c=>c.Nombre.Equals("Algoritmo"));
            if (configuracionDb == null)
            {
                configuracionDb = new Configuracion
                {
                    Id = Guid.NewGuid().ToString(),
                    Nombre = "Algoritmo",
                    Valor = "1"
                };
                _context.Configuracion.Add(configuracionDb);
                _context.SaveChanges();
            }

            var response = new ConfiguracionModel
            {
                Id = configuracionDb.Id,
                Nombre = configuracionDb.Nombre,
                Valor = configuracionDb.Valor,
                Versiones = new List<AlgoritmoVersionModel>()
            };

            var versionesDb = _context.AlgoritmoVersion.Include(av=>av.Usuario).Include(av=>av.Autorizador)
                .OrderBy(v => v.CreatedAt).ToList();
            var ultimaVersion = versionesDb.Count;
            foreach (var version in versionesDb)
            {
                response.Versiones.Add(new AlgoritmoVersionModel
                {
                    Id = version.Id,
                    Version = version.Version,
                    CreatedAt = version.CreatedAt.ToString(CultureInfo.InvariantCulture),
                    UpdatedAt = version.UpdatedAt.ToString(CultureInfo.InvariantCulture),
                    FechaAutorizacion = version.FechaAutorizacion==null?"":version.FechaAutorizacion.ToString(),
                    Usuario = version.Usuario.Name, 
                    Autorizador = version.AutorizadorId == null ? "" : version.Autorizador.Name,
                    Selected = version.Version == ultimaVersion,
                    Autorizable = version.Version == ultimaVersion && _context.AlgoritmoResultado.Count(ar=>ar.VersionId==version.Id) > numIntentos
                });
            }
            return View("Algoritmo", response);
        }
        
        [HttpPost]
        [Authorize]
        [DisableRequestSizeLimit]
        public string GuardarAlgoritmo(AlgoritmoRequest algoritmo)
        {
            if (!ModelState.IsValid)
            {
                var errores = ModelState.Select(x => new {x.Key, Error = x.Value.Errors.FirstOrDefault()?.ErrorMessage})
                    .ToList();

                return JsonConvert.SerializeObject(errores);
            }
            try
            {
                var now = DateTime.Now;
                var nuevaVersion = false;
                var user = _userManager.GetUserAsync(HttpContext.User).Result;
                var folderName = Path.Combine("wwwroot","coneval");
                var ultimaVersion = _context.AlgoritmoVersion.OrderBy(av => av.Version).Last();
                if (ultimaVersion.FechaAutorizacion != null)
                {
                    nuevaVersion = true;
                     ultimaVersion = new AlgoritmoVersion
                    {
                        Id = Guid.NewGuid().ToString(),
                        CreatedAt = now,
                        Version = ultimaVersion.Version+1
                    };
                }

                ultimaVersion.UsuarioId = user.Id;
                ultimaVersion.UpdatedAt = now;
                if (nuevaVersion)
                {
                    _context.AlgoritmoVersion.Add(ultimaVersion);
                }
                else
                {
                    _context.AlgoritmoVersion.Update(ultimaVersion);
                }
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                if (algoritmo.Jar.Length > 0)
                {
                    var fullPath = Path.Combine(pathToSave, "coneval"+ultimaVersion.Version+".jar");
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        algoritmo.Jar.CopyTo(stream);
                    }
                    if (algoritmo.Apk.Length > 0)
                    {
                        fullPath = Path.Combine(pathToSave, "coneval"+ultimaVersion.Version+".apk");
                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            algoritmo.Apk.CopyTo(stream);
                        }   
                    }
                    if (algoritmo.Zip.Length > 0)
                    {
                        fullPath = Path.Combine(pathToSave, "coneval"+ultimaVersion.Version+".zip");
                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            algoritmo.Apk.CopyTo(stream);
                        }   
                    }

                    var pruebasAEliminar = _context.AlgoritmoResultado.Where(ar => ar.VersionId == ultimaVersion.Id)
                        .ToList();
                    if (pruebasAEliminar.Any())
                    {
                        foreach (var resultado in pruebasAEliminar)
                        {
                            var pruebaIntegrantes =
                                _context.AlgoritmoIntegranteResultado.Where(air => air.ResultadoId == resultado.Id);
                            _context.AlgoritmoIntegranteResultado.RemoveRange(pruebaIntegrantes);
                        }
                        _context.AlgoritmoResultado.RemoveRange(pruebasAEliminar);
                    }
                    _context.SaveChanges();
                    return "ok";
                }
            }
            catch (Exception e)
            {
                Excepcion.Registrar(e);
                return "error";
            }
            return "error";
        }

        [HttpPost]
        [Authorize]
        public string AutorizarAlgoritmo([FromBody] AlgoritmoVersionModel version)
        {
            var now = DateTime.Now;
            var user = _userManager.GetUserAsync(HttpContext.User).Result;
            var versionDb = _context.AlgoritmoVersion.FirstOrDefault(av=>av.Id.Equals(version.Id));
            versionDb.AutorizadorId = user.Id;
            versionDb.FechaAutorizacion = now;
            versionDb.UpdatedAt = now;
            _context.AlgoritmoVersion.Update(versionDb);
            
            var configuracion = _context.Configuracion.FirstOrDefault(c=>c.Nombre.Equals("Algoritmo"));
            configuracion.Valor = versionDb.Version.ToString();
            configuracion.UpdatedAt = now;
            _context.Configuracion.Update(configuracion);
            _context.SaveChanges();
            return "ok";
        }

        public IActionResult Fundamento()
        {
            var configuracionDb = _context.Configuracion.FirstOrDefault(c=>c.Nombre.Equals("Fundamento"));
            if (configuracionDb == null)
            {
                configuracionDb = new Configuracion
                {
                    Id = Guid.NewGuid().ToString(),
                    Nombre = "Fundamento",
                    Valor = "Fundamento legal"
                };
                _context.Configuracion.Add(configuracionDb);
                _context.SaveChanges();
            }
            return View("Fundamento", new ConfiguracionModel
            {
                Id = configuracionDb.Id,
                Nombre = configuracionDb.Nombre,
                Valor = configuracionDb.Valor
            });
        }
        
        [HttpPost]
        [Authorize]
        public string GuardarFundamento([FromBody] FundamentoRequest fundamento)
        {
            if (!ModelState.IsValid)
            {
                var errores = ModelState.Select(x => new {Key = x.Key, Error = x.Value.Errors.First().ErrorMessage})
                    .ToList();

                return JsonConvert.SerializeObject(errores);
            }
            try
            {
                var fundamentoDB = _context.Configuracion.Where(f=>f.Nombre.Equals("Fundamento")).FirstOrDefault();
                fundamentoDB.Valor = fundamento.Valor;
                fundamentoDB.UpdatedAt = DateTime.Now;
                _context.Configuracion.Update(fundamentoDB);
                _context.SaveChanges();
            }
            catch (Exception)
            {
                return "error";
            }
            return "ok";
        }

        [HttpGet]
        [Authorize]
        public string GetResultados(string id)
        {
            var resultados = new List<AlgoritmoResultadoModel>();
            var resultadosDB = _context.AlgoritmoResultado.Where(ar => ar.VersionId.Equals(id)).Include(ar=>ar.Beneficiario);
            foreach (var ar in resultadosDB)
            {
                resultados.Add(new AlgoritmoResultadoModel
                {
                    Id = ar.Id,
                    Folio = ar.Beneficiario.Folio,
                    Educativa = ar.Educativa,
                    ServicioSalud = ar.ServicioSalud,
                    SeguridadSocial = ar.SeguridadSocial,
                    Alimentaria = ar.Alimentaria,
                    GradoAlimentaria = ar.GradoAlimentaria,
                    Vivienda = ar.Vivienda,
                    Servicios = ar.Servicios,
                    NivelPobreza = ar.NivelPobreza,
                    CreatedAt = ar.CreatedAt.ToString(CultureInfo.InvariantCulture),
                    VersionId = ar.VersionId
                });
            }
            return JsonSedeshu.SerializeObject(resultados);
        }
        
        [HttpGet]
        [Authorize]
        public string GetResultado(string id)
        {
            var ar = _context.AlgoritmoResultado.Where(a => a.Id==id).Include(a=>a.Beneficiario).FirstOrDefault();
            var integrantes = _context.AlgoritmoIntegranteResultado.Where(a => a.ResultadoId.Equals(id));
            var resultado = new AlgoritmoResultadoModel
            {
                Id = ar.Id,
                Folio = ar.Beneficiario.Folio,
                Educativa = ar.Educativa,
                ServicioSalud = ar.ServicioSalud,
                SeguridadSocial = ar.SeguridadSocial,
                Alimentaria = ar.Alimentaria,
                GradoAlimentaria = ar.GradoAlimentaria,
                Vivienda = ar.Vivienda,
                Servicios = ar.Servicios,
                NivelPobreza = ar.NivelPobreza,
                Agua = ar.Agua,
                Analfabetismo = ar.Analfabetismo,
                Combustible = ar.Combustible,
                Drenaje = ar.Drenaje,
                Electricidad = ar.Electricidad,
                Hacinamiento = ar.Hacinamiento,
                Inasistencia = ar.Inasistencia,
                Ingreso = ar.Ingreso,
                Movilidad = ar.Movilidad,
                Muro = ar.Muro,
                Pea = ar.Pea,
                Piso = ar.Piso,
                Satisfaccion = ar.Satisfaccion,
                Techo = ar.Techo,
                Tejido = ar.Tejido,
                ConfianzaInstituciones = ar.ConfianzaInstituciones,
                ConfianzaLiderez = ar.ConfianzaLiderez,
                LineaBienestar = ar.LineaBienestar,
                PrimariaIncompleta = ar.PrimariaIncompleta,
                PropiedadVivienda = ar.PropiedadVivienda,
                RedesSociales = ar.RedesSociales,
                SecundariaIncompleta = ar.SecundariaIncompleta,
                SeguridadParque = ar.SeguridadParque,
                SeguridadPublica = ar.SeguridadPublica,
                TieneActa = ar.TieneActa,
                CreatedAt = ar.CreatedAt.ToString(CultureInfo.InvariantCulture),
                VersionId = ar.VersionId,
                ResultadoIntegrantes = new List<AlgoritmoIntegranteResultadoModel>()
            };
            foreach (var integrante in integrantes)
            {
                resultado.ResultadoIntegrantes.Add(new AlgoritmoIntegranteResultadoModel
                {
                    Id = integrante.Id,
                    Agua = integrante.Agua,
                    Alimentaria = integrante.Alimentaria,
                    Analfabetismo = integrante.Analfabetismo,
                    Combustible = integrante.Combustible,
                    Discapacidad  = integrante.Discapacidad,
                    Drenaje = integrante.Drenaje,
                    Edad = integrante.Edad,
                    Educativa = integrante.Educativa,
                    Electricidad = integrante.Electricidad,
                    Hacinamiento = integrante.Hacinamiento,
                    Inasistencia = integrante.Inasistencia,
                    Ingreso = integrante.Ingreso,
                    Muro = integrante.Muro,
                    Pea = integrante.Pea,
                    Piso = integrante.Piso,
                    Servicios = integrante.Servicios,
                    Techo = integrante.Techo,
                    Vivienda = integrante.Vivienda,
                    GradoAlimentaria = integrante.GradoAlimentaria,
                    GradoEducativa = integrante.GradoEducativa,
                    LineaBienestar = integrante.LineaBienestar,
                    NivelPobreza = integrante.NivelPobreza,
                    NumIntegrante = integrante.NumIntegrante,
                    ParentescoId = integrante.ParentescoId,
                    SexoId = integrante.SexoId,
                    PrimariaIncompleta = integrante.PrimariaIncompleta,
                    SecundariaIncompleta = integrante.SecundariaIncompleta,
                    SeguridadSocial = integrante.SeguridadSocial,
                    ServicioSalud = integrante.ServicioSalud,
                    TieneActa = integrante.TieneActa,
                    ResultadoId = integrante.ResultadoId,
                });
            }
            
            return JsonSedeshu.SerializeObject(resultado);
        }

        [HttpPost]
        [Authorize] 
        public string BuscarFamilia([FromBody] FamiliaRequest familiaRequest)
        {
            var familia = new FamiliaResponse();
            var beneficiario = _context.Beneficiario.Where(b => b.DeletedAt == null && b.Folio.Equals(familiaRequest.Folio) && b.PadreId == null)
                .Include(b=>b.Domicilio).ThenInclude(d=>d.Municipio)
                .Include(b=>b.Domicilio).ThenInclude(d=>d.Localidad)
                .Include(b=>b.Domicilio).ThenInclude(d=>d.ZonaImpulso).FirstOrDefault();
            if (beneficiario != null)
            {
                familia.Id = beneficiario.Id;
                familia.Folio = beneficiario.Folio;
                familia.Nombres = beneficiario.ApellidoPaterno+" "+beneficiario.ApellidoMaterno+" "+beneficiario.Nombre;
                familia.Calle = beneficiario.Domicilio.DomicilioN;
                familia.Municipio = beneficiario.Domicilio.Municipio.Nombre;
                familia.Localidad = beneficiario.Domicilio.Localidad.Nombre;
                familia.ZonaImpulso = beneficiario.Domicilio.ZonaImpulsoId == null ? "" : beneficiario.Domicilio.ZonaImpulso.Nombre;
                familia.NumIntegrantes = _context.Beneficiario.Count(b => b.DeletedAt == null && b.PadreId.Equals(beneficiario.Id) && b.HijoId == null);
                familia.Estatus = _context.Aplicacion.FirstOrDefault(a =>
                                      a.DeletedAt == null && a.Activa && a.BeneficiarioId.Equals(familia.Id)&&a.Estatus.Equals(EstatusAplicacion.COMPLETO)) != null;
            }
            return JsonSedeshu.SerializeObject(familia);
        }
        
        [HttpPost]
        [Authorize] 
        public string ProbarResultado([FromBody] FamiliaResponse familiaResponse)
        {
            var beneficiario = _context.Beneficiario.Where(b => b.DeletedAt == null && b.Id.Equals(familiaResponse.Id))
                .Include(b=>b.Domicilio).ThenInclude(d=>d.TipoAsentamiento)
                .Include(b=>b.Domicilio).ThenInclude(d=>d.Municipio)
                .FirstOrDefault();
            var aplicacion = _context.Aplicacion.FirstOrDefault(a =>
                a.DeletedAt == null && a.Activa && a.BeneficiarioId.Equals(familiaResponse.Id));
            var coneval =  new Coneval();
            return coneval.CalcularCarencias(aplicacion.Id, beneficiario.Domicilio.TipoAsentamiento.Nombre, beneficiario.Domicilio.Indice, 
                _context, familiaResponse.Version);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetArchivo(string id)
        {
            var folderName = Path.Combine("wwwroot","coneval");
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            var conevalPath = Path.Combine(pathToSave, id);
            var memory = new MemoryStream();
            using (var stream = new FileStream(conevalPath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            var ext = Path.GetExtension(conevalPath).ToLowerInvariant();
            return File(memory, GetMimeTypes()[ext], Path.GetFileName(conevalPath));
        }
        
        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jar", "application/java-archive"},
                {".apk", "application/java-archive"},
                {".zip", "application/zip"}
            };
        }
        
        [HttpGet]
        [Authorize]
        public IActionResult CorreccionDatos()
        {
            var date = DateTime.Now;
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            var response = new CompletarDatos
            {
                Inicio = firstDayOfMonth.ToString("yyyy-MM-dd"),
                Fin = lastDayOfMonth.ToString("yyyy-MM-dd"),
                NumDomiciliosSinCalculo = _context.Domicilio.Count(m =>
                    m.DeletedAt == null && m.Activa && m.MunicipioCalculado == null && m.LatitudAtm != ""
                    && m.LatitudAtm != "0.0" &&
                    (m.LatitudCorregida == null || m.LatitudCorregida != m.LongitudCorregida)),
            };
            return View("CorreccionDatos", response);
        }
    }
}