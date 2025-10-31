using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using moment.net;
using moment.net.Enums;
using Newtonsoft.Json;
using OfficeOpenXml;
using DiagnosticoWeb.Code;
using DiagnosticoWeb.Database;
using DiagnosticoWeb.Models;
using DiagnosticoWeb.Validaciones;

namespace DiagnosticoWeb.Controllers
{
    /// <summary>
    /// Clase que permite la consulta y edicion en los datos del domicilio de un beneficiario por parte del personal administrativo
    /// </summary>
    public class DomicilioController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _clientFactory;

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="context">Conexion a la base de datos</param>
        /// <param name="userManager">Objeto que permite saber que usuario esta usando el sistema</param>
        public DomicilioController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IConfiguration configuration, IHttpClientFactory clientFactory)
        {
            _context = context;
            _userManager = userManager;
            _configuration = configuration;
            _clientFactory = clientFactory;

        }

        /// <summary>
        /// Funcion que permite ver las versiones de la informacion del domicilio del beneficiario
        /// </summary>
        /// <param name="request">Numero de version de los datos del domicilio</param>
        /// <returns>Cadena JSON con los datos del domicilio</returns>
        [HttpPost]
        [Authorize]
        public string VerDatos([FromBody] VerDomicilioRequest request)
        {
            var domicilio = _context.Domicilio.FirstOrDefault(d => d.Id == request.DomicilioId);
            var response = new DomicilioEditModel
            {
                Id = domicilio.Id,
                Telefono = domicilio.Telefono,
                TelefonoCasa = domicilio.TelefonoCasa,
                Email = domicilio.Email,
                DomicilioN = domicilio.DomicilioN,
                NumExterior = domicilio.NumExterior,
                NumInterior = domicilio.NumInterior,
                CallePosterior = domicilio.CallePosterior,
                EntreCalle1 = domicilio.EntreCalle1,
                EntreCalle2 = domicilio.EntreCalle2,
                CodigoPostal = domicilio.CodigoPostal,
                MunicipioId = domicilio.MunicipioId,
                LocalidadId = domicilio.LocalidadId,
                AgebId = domicilio.AgebId,
                ManzanaId = domicilio.ManzanaId,
                ColoniaId = domicilio.ColoniaId,
                NombreAsentamiento = domicilio.NombreAsentamiento,
                CalleId = domicilio.CalleId,
                ZonaImpulsoId = domicilio.ZonaImpulsoId,
                CaminoId = domicilio.CaminoId,
                CarreteraId = domicilio.CarreteraId,
                TipoAsentamientoId = domicilio.TipoAsentamientoId,
                Latitud = domicilio.Latitud,
                Longitud = domicilio.Longitud,
                CapturarUbicacion = domicilio.Latitud != "" && domicilio.Longitud != ""
            };

            return JsonSedeshu.SerializeObject(response);
        } 
        
        [HttpPost]
        [Authorize]
        public string VerDatosHistoricos([FromBody] VerDomicilioRequest request)
        {
            var domicilioQuery = _context.DomicilioHistorico.Where(d => d.DomicilioId == request.DomicilioId).ToList();
            var domicilio = domicilioQuery[request.Numero - 2];
            var response = new DomicilioEditModel
            {
                Id = domicilio.Id,
                Telefono = domicilio.Telefono,
                TelefonoCasa = domicilio.TelefonoCasa,
                Email = domicilio.Email,
                DomicilioN = domicilio.DomicilioN,
                NumExterior = domicilio.NumExterior,
                NumInterior = domicilio.NumInterior,
                CallePosterior = domicilio.CallePosterior,
                EntreCalle1 = domicilio.EntreCalle1,
                EntreCalle2 = domicilio.EntreCalle2,
                CodigoPostal = domicilio.CodigoPostal,
                MunicipioId = domicilio.MunicipioId,
                LocalidadId = domicilio.LocalidadId,
                AgebId = domicilio.AgebId,
                ManzanaId = domicilio.ManzanaId,
                ColoniaId = domicilio.ColoniaId,
                NombreAsentamiento = domicilio.NombreAsentamiento,
                CalleId = domicilio.CalleId,
                ZonaImpulsoId = domicilio.ZonaImpulsoId,
                CaminoId = domicilio.CaminoId,
                CarreteraId = domicilio.CarreteraId,
                TipoAsentamientoId = domicilio.TipoAsentamientoId,
                Latitud = domicilio.Latitud,
                Longitud = domicilio.Longitud,
                CapturarUbicacion = domicilio.Latitud != "" && domicilio.Longitud != ""
            };

            return JsonSedeshu.SerializeObject(response);
        }

        /// <summary>
        /// Funcion que muestra la vista para la edicion de los datos del domicilio del beneficiario
        /// </summary>
        /// <param name="id">Identificador del registro del domicilio</param>
        /// <returns>Vista con el formulario para editar los datos del domicilio</returns>
        [HttpGet]
        [Authorize]
        [RequireClaim("Permiso", Value = "beneficiarios.editar")]
        public IActionResult Edit(string id)
        {
            var response = new DomicilioEdit
            {
                Domicilio = new DomicilioEditModel(),
                Municipios = new List<Model>(),
                Caminos = new List<Model>(),
                Carreteras = new List<Model>(),
                TiposAsentamiento = new List<Model>()
            };
            var beneficiario = _context.Beneficiario.FirstOrDefault(b => b.Id.Equals(id));
            var domicilio = _context.Domicilio.Find(beneficiario.DomicilioId);
            _context.Municipio.OrderBy(m => m.Clave).ToList().ForEach(m =>
                response.Municipios.Add(new Model { Id = m.Id, Nombre = m.Nombre }));
            _context.Camino.OrderBy(m => m.Nombre).ToList().ForEach(m =>
                response.Caminos.Add(new Model { Id = m.Id, Nombre = m.Nombre }));
            _context.Carretera.OrderBy(m => m.Nombre).ToList().ForEach(m =>
                response.Carreteras.Add(new Model { Id = m.Id, Nombre = m.Nombre }));
            _context.TipoAsentamiento.OrderBy(m => m.Nombre).ToList().ForEach(m =>
                response.TiposAsentamiento.Add(new Model { Id = m.Id, Nombre = m.Nombre }));
            response.Domicilio.Id = domicilio.Id;
            response.Domicilio.Telefono = domicilio.Telefono;
            response.Domicilio.TelefonoCasa = domicilio.TelefonoCasa;
            response.Domicilio.Email = domicilio.Email;
            response.Domicilio.DomicilioN = domicilio.DomicilioN;
            response.Domicilio.NumExterior = domicilio.NumExterior;
            response.Domicilio.NumInterior = domicilio.NumInterior;
            response.Domicilio.CallePosterior = domicilio.CallePosterior;
            response.Domicilio.EntreCalle1 = domicilio.EntreCalle1;
            response.Domicilio.EntreCalle2 = domicilio.EntreCalle2;
            response.Domicilio.CodigoPostal = domicilio.CodigoPostal;
            response.Domicilio.MunicipioId = domicilio.MunicipioId;
            response.Domicilio.LocalidadId = domicilio.LocalidadId;
            response.Domicilio.AgebId = domicilio.AgebId;
            response.Domicilio.ManzanaId = domicilio.ManzanaId;
            response.Domicilio.ColoniaId = domicilio.ColoniaId;
            response.Domicilio.NombreAsentamiento = domicilio.NombreAsentamiento;
            response.Domicilio.CalleId = domicilio.CalleId;
            response.Domicilio.ZonaImpulsoId = domicilio.ZonaImpulsoId;
            response.Domicilio.CaminoId = domicilio.CaminoId;
            response.Domicilio.CarreteraId = domicilio.CarreteraId;
            response.Domicilio.TipoAsentamientoId = domicilio.TipoAsentamientoId;
            response.Domicilio.Latitud = domicilio.Latitud;
            response.Domicilio.Longitud = domicilio.Longitud;
            response.Domicilio.CapturarUbicacion = domicilio.Latitud != "" && domicilio.Longitud != "";
            response.Domicilio.BeneficiarioId = id;
            response.Domicilio.NumDatos = _context.DomicilioHistorico.Count(dh=>dh.DeletedAt == null && dh.DomicilioId == domicilio.Id);
            return View("Edit", response);
        }

        /// <summary>
        /// Funcion que obtiene las localidades del municipio seleccionado
        /// </summary>
        /// <param name="id">Identificador en base de datos del municipio seleccionado</param>
        /// <returns>Cadena JSON con las localidades del municipio</returns>
        [HttpGet]
        [Authorize]
        public string GetLocalidades(string id)
        {
            var localidades = new List<Model>();
            _context.Localidad.Where(l => l.MunicipioId.Equals(id)).ToList()
                .ForEach(l => localidades.Add(new Model() { Id = l.Id, Nombre = l.Nombre }));
            return JsonSedeshu.SerializeObject(localidades);
        }

        /// <summary>
        /// Funcion que obtiene las AGEBS de la localidad seleccionada
        /// </summary>
        /// <param name="id">Identificador en base de datos de la localidad seleccionada</param>
        /// <returns>Cadena JSON con las AGEBs de la localidad seleccionada</returns>
        [HttpGet]
        [Authorize]
        public string GetAgebs(string id)
        {
            var agebs = new List<Model>();
            _context.Ageb.Where(a => a.LocalidadId.Equals(id)).ToList()
                .ForEach(l => agebs.Add(new Model() { Id = l.Id, Nombre = l.Clave }));
            return JsonSedeshu.SerializeObject(agebs);
        }

        [HttpGet]
        [Authorize]
        public string GetManzanas(string id)
        {
            var manzanas = new List<Model>();
            _context.Manzana.Where(a => a.AgebId.Equals(id)).ToList()
                .ForEach(l => manzanas.Add(new Model() { Id = l.Id, Nombre = l.Nombre }));
            return JsonSedeshu.SerializeObject(manzanas);
        }

        /// <summary>
        /// Funcion que guarda en la base de datos la nueva informacion del domicilio, esto generará un nuevo registro historico del domicilio del beneficiario 
        /// </summary>
        /// <param name="request">Datos del domicilio</param>
        /// <returns>Estatus de guardado: ok, error</returns>
        [HttpPost]
        [Authorize]
        public string Guardar([FromBody] DomicilioEditModel request)
        {
            if (!ModelState.IsValid)
            {
                var errores = ModelState.Select(x => new { Key = x.Key, Error = x.Value.Errors.First().ErrorMessage })
                    .ToList();
                return JsonConvert.SerializeObject(errores);
            }

            using (var transaction = _context.Database.BeginTransaction())
            {
                var now = DateTime.Now;
                var domicilio = _context.Domicilio.FirstOrDefault(d => d.Id.Equals(request.Id));
                if (domicilio.MunicipioId != request.MunicipioId || domicilio.LocalidadId != request.LocalidadId || domicilio.ZonaImpulsoId != request.ZonaImpulsoId
                || domicilio.AgebId != request.AgebId || domicilio.ManzanaId != request.ManzanaId || domicilio.CodigoPostal != request.CodigoPostal
                || domicilio.CalleId != request.CalleId || domicilio.DomicilioN != request.DomicilioN || domicilio.ColoniaId != request.ColoniaId
                || domicilio.NombreAsentamiento != request.NombreAsentamiento || domicilio.NumExterior != request.NumExterior
                || domicilio.NumInterior != request.NumInterior || domicilio.EntreCalle1 != request.EntreCalle1 || domicilio.EntreCalle2 != request.EntreCalle2
                || domicilio.CallePosterior != request.CallePosterior || domicilio.Telefono != request.Telefono || domicilio.TelefonoCasa != request.TelefonoCasa
                || domicilio.Email != request.Email || domicilio.CaminoId != request.CaminoId || domicilio.CarreteraId != request.CarreteraId 
                || domicilio.TipoAsentamientoId != request.TipoAsentamientoId || domicilio.Latitud != request.Latitud || domicilio.Longitud != request.Longitud)
                {
                    var domicilioHistorico = new DomicilioHistorico
                    {
                        DomicilioId = domicilio.Id,
                        MunicipioId = domicilio.MunicipioId,
                        LocalidadId = domicilio.LocalidadId,
                        ZonaImpulsoId = domicilio.ZonaImpulsoId,
                        AgebId = domicilio.AgebId,
                        ManzanaId = domicilio.ManzanaId,
                        CodigoPostal = domicilio.CodigoPostal,
                        ColoniaId = domicilio.ColoniaId,
                        NombreAsentamiento = domicilio.NombreAsentamiento,
                        CalleId = domicilio.CalleId,
                        DomicilioN = domicilio.DomicilioN,
                        NumExterior = domicilio.NumExterior,
                        NumInterior = domicilio.NumInterior,
                        EntreCalle1 = domicilio.EntreCalle1,
                        EntreCalle2 = domicilio.EntreCalle2,
                        CallePosterior = domicilio.CallePosterior,
                        Telefono = domicilio.Telefono,
                        TelefonoCasa = domicilio.TelefonoCasa,
                        Email = domicilio.Email,
                        CaminoId = domicilio.CaminoId,
                        CarreteraId = domicilio.CarreteraId,
                        TipoAsentamientoId = domicilio.TipoAsentamientoId,
                        CreatedAt = now,
                        UpdatedAt = now
                    };
                    if (request.CapturarUbicacion)
                    {
                        domicilioHistorico.Latitud = request.Latitud;
                        domicilioHistorico.Longitud = request.Longitud;
                    }
                    else
                    {
                        domicilioHistorico.Latitud = "";
                        domicilioHistorico.Longitud = "";
                    }
                    _context.DomicilioHistorico.Add(domicilioHistorico);
                }
                domicilio.MunicipioId = request.MunicipioId;
                domicilio.LocalidadId = request.LocalidadId;
                domicilio.ZonaImpulsoId = request.ZonaImpulsoId;
                domicilio.AgebId = request.AgebId;
                domicilio.ManzanaId = request.ManzanaId;
                
                domicilio.CodigoPostal = request.CodigoPostal;
                domicilio.ColoniaId = request.ColoniaId;
                domicilio.NombreAsentamiento = request.NombreAsentamiento ;
                domicilio.CalleId = request.CalleId;
                domicilio.DomicilioN = request.DomicilioN;
                domicilio.NumExterior = request.NumExterior;
                domicilio.NumInterior = request.NumInterior;
                domicilio.EntreCalle1 = request.EntreCalle1;
                domicilio.EntreCalle2 = request.EntreCalle2;
                domicilio.CallePosterior = request.CallePosterior;
                domicilio.Telefono = request.Telefono;
                domicilio.TelefonoCasa = request.TelefonoCasa;
                domicilio.Email = request.Email;
                domicilio.CaminoId = request.CaminoId;
                domicilio.CarreteraId = request.CarreteraId;
                domicilio.TipoAsentamientoId = request.TipoAsentamientoId;
                if (request.CapturarUbicacion)
                {
                    domicilio.Latitud = request.Latitud;
                    domicilio.Longitud = request.Longitud;
                } else
                {
                    domicilio.Latitud = "";
                    domicilio.Longitud = "";
                }
                domicilio.UpdatedAt = now;
                _context.Domicilio.Update(domicilio);

                _context.Bitacora.Add(new Bitacora()
                {
                    UsuarioId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                    Accion = AccionBitacora.EDICION,
                    Mensaje = "Se modificó el domicilio .",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                });

                _context.SaveChanges();
                transaction.Commit();
            }

            return "ok";
        }

        /// <summary>
        /// Funcion que activa la version de los datos del domicilio del beneficiario, es decir, inactiva la version actual y activa una version anterior,
        /// la activacion de un dato historico cambiara la fecha de acutalizacion en el domicilio del beneficiario para que los dispositivos moviles
        /// sincronizen esta informacion
        /// </summary>
        /// <param name="id">Identificador en base de datos del domicilio a activar</param>
        /// <returns>Estatus de guardado: ok, error</returns>
        [HttpPost]
        [Authorize]
        public string Activar(string id)
        {
            var now = DateTime.Now;
            using (var transaction = _context.Database.BeginTransaction())
            {
                var domicilio = _context.Domicilio.Find(id);
                domicilio.Activa = true;
                domicilio.UpdatedAt = now;
                _context.Domicilio.Update(domicilio);

                _context.Bitacora.Add(new Bitacora()
                {
                    UsuarioId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                    Accion = AccionBitacora.EDICION,
                    Mensaje = "Se modificó el domicilio .",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                });

                _context.SaveChanges();
                transaction.Commit();
            }

            return "ok";
        }

        /// <summary>
        /// Funcion que obtiene las Zonas de impulso social del municipio seleccionado
        /// </summary>
        /// <param name="id">Identificador en base de datos del municipio seleccionado</param>
        /// <returns>Cadena JSON con las zonas de impulso</returns>
        [HttpGet]
        [Authorize]
        public string GetZonas(string id)
        {
            var zonas = new List<Model>();
            _context.ZonaImpulso.Where(z => z.MunicipioId.Equals(id)).ToList()
                .ForEach(l => zonas.Add(new Model { Id = l.Id, Nombre = l.Nombre }));
            return JsonSedeshu.SerializeObject(zonas);
        }

        /// <summary>
        /// Funcion que obtiene las colonias de impulso social del municipio seleccionado
        /// </summary>
        /// <param name="id">Identificador en base de datos del municipio seleccionado</param>
        /// <param name="codigopostal">Codigo postal</param>
        /// <returns>Cadena JSON con las colonias de impulso</returns>
        [HttpGet]
        [Authorize]
        public string GetColonias([FromQuery]string id, [FromQuery]string codigopostal)
        {
            var colonias = new List<Model>();
            if (string.IsNullOrEmpty(codigopostal))
            {
                _context.Colonia.Where(z => z.MunicipioId.Equals(id)).ToList()
                    .ForEach(l => colonias.Add(new Model { Id = l.Id, Nombre = l.Nombre }));
            }
            else
            {
                _context.Colonia.Where(z => z.MunicipioId.Equals(id) && z.CodigoPostal.Equals(codigopostal)).ToList()
                    .ForEach(l => colonias.Add(new Model { Id = l.Id, Nombre = l.Nombre }));
            }
            return JsonSedeshu.SerializeObject(colonias);
        }

        /// <summary>
        /// Funcion que obtiene las calles de la localidad seleccionada
        /// </summary>
        /// <param name="id">Identificador en base de datos de la localidad seleccionada</param>
        /// <returns>Cadena JSON con las colonias de impulso</returns>
        [HttpGet]
        [Authorize]
        public string GetCalles(string id)
        {
            var calles = new List<Model>();
            _context.Calle.Where(z => z.LocalidadId.Equals(id)).ToList()
                .ForEach(l => calles.Add(new Model { Id = l.Id, Nombre = l.Nombre }));
            return JsonSedeshu.SerializeObject(calles);
        }

        public IActionResult VerDomiciliosIncompletos()
        {
            var response = new DomiciliosIncompletosResponse();
            var date = DateTime.Now;
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            response.Inicio = firstDayOfMonth.ToString("yyyy-MM-dd");
            response.Fin = lastDayOfMonth.ToString("yyyy-MM-dd");
            response.NumDomiciliosSinCalculo = _context.Domicilio.Count(d => d.MunicipioCalculado == "ND");
            response.Municipios = _context.Municipio.Where(m => m.DeletedAt == null).Select(m => new Model {
                Id = m.Id, Nombre = m.Nombre }).ToList();
            return View("DomiciliosIncompletos", response);
        }

        public string GetDomiciliosIncompletos([FromBody] DomiciliosIncompletosRequest request)
        {
            var response = new DomiciliosIncompletosQueryResponse
            {
                Domicilios = new List<Domicilio>(), Total = BuildDomiciliosIncompletos(request, 0).Count,
                GranTotal = _context.Domicilio.Count(d => d.DeletedAt == null && d.MunicipioCalculado == "ND")
            };
            foreach (var domicilio in BuildDomiciliosIncompletos(request, request.PageSize))
            {
                var beneficiario = domicilio.Beneficiarios.FirstOrDefault(b => b.PadreId == null);
                response.Domicilios.Add(new Domicilio
                {
                    Id = beneficiario == null ? "" : beneficiario.Id,
                    Folio = beneficiario == null ? "" : beneficiario.Folio,
                    DomicilioN = domicilio.DomicilioN,
                    NumInterior = domicilio.NumInterior,
                    EntreCalle1 = domicilio.EntreCalle1,
                    EntreCalle2 = domicilio.EntreCalle2,
                    ZonaImpulsoCalculada = domicilio.ZonaImpulso == null ? "" : domicilio.ZonaImpulso.Nombre,
                    MunicipioCalculado = domicilio.Municipio.Nombre,
                    LocalidadCalculado = domicilio.Localidad == null ? domicilio.LocalidadId : domicilio.Localidad.Nombre,
                    Latitud = domicilio.LatitudAtm,
                    Longitud = domicilio.LongitudAtm,
                    CreatedAt = domicilio.CreatedAt,
                    NumIntentosCoordenadas = domicilio.NumIntentosCoordenadas
                });
            }
            return JsonSedeshu.SerializeObject(response);
        }

        [HttpPost]
        [Authorize]

        public IActionResult ExportarDomiciliosIncompletos(DomiciliosIncompletosRequest request)
        {
            using (var excel = new ExcelPackage())
            {
                excel.Workbook.Worksheets.Add("Domicilios");
                var pestana = excel.Workbook.Worksheets.First();
                var fila = 1;
                pestana.Cells[fila, 1].Value = "Id familia";
                pestana.Cells[fila, 2].Value = "Id encuesta";
                pestana.Cells[fila, 3].Value = "Dirección";
                pestana.Cells[fila, 4].Value = "Entre calles";
                pestana.Cells[fila, 5].Value = "Código postal";
                pestana.Cells[fila, 6].Value = "Polígono";
                pestana.Cells[fila, 7].Value = "Municipio";
                pestana.Cells[fila, 8].Value = "Localidad";
                pestana.Cells[fila, 9].Value = "Latitud";
                pestana.Cells[fila, 10].Value = "Longitud";
                pestana.Cells[fila, 11].Value = "LatitudAtm";
                pestana.Cells[fila, 12].Value = "LongitudAtm";
                pestana.Cells[fila, 13].Value = "Fecha de creación";
                pestana.Cells[fila, 14].Value = "Latitud corregida";
                pestana.Cells[fila++, 15].Value = "Longitud corregida";

                foreach (var domicilio in BuildDomiciliosIncompletos(request, 0).ToList())
                {
                    var beneficiario = domicilio.Beneficiarios.FirstOrDefault(b => b.PadreId == null);
                    pestana.Cells[fila, 1].Value = beneficiario == null ? "" : beneficiario.Id;
                    pestana.Cells[fila, 2].Value = beneficiario == null ? "" : beneficiario.Folio;
                    pestana.Cells[fila, 3].Value = domicilio.DomicilioN + " " + domicilio.NumInterior;
                    pestana.Cells[fila, 4].Value = domicilio.EntreCalle1 + " " + domicilio.EntreCalle2;
                    pestana.Cells[fila, 5].Value = domicilio.CodigoPostal;
                    pestana.Cells[fila, 6].Value = domicilio.ZonaImpulso == null ? "" : domicilio.ZonaImpulso.Nombre;
                    pestana.Cells[fila, 7].Value = domicilio.Municipio.Nombre;
                    pestana.Cells[fila, 8].Value = domicilio.Localidad == null ? domicilio.LocalidadId : domicilio.Localidad.Nombre;
                    pestana.Cells[fila, 9].Value = domicilio.Latitud;
                    pestana.Cells[fila, 10].Value = domicilio.Longitud;
                    pestana.Cells[fila, 11].Value = domicilio.LatitudAtm;
                    pestana.Cells[fila, 12].Value = domicilio.LongitudAtm;
                    pestana.Cells[fila++, 13].Value = domicilio.CreatedAt.ToString("dd/MM/yyyy");
                }

                return File(fileContents: excel.GetAsByteArray(),
                    contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    fileDownloadName: "domicilios.xlsx"
                );
            }
        }

        private List<Domicilio> BuildDomiciliosIncompletos(DomiciliosIncompletosRequest request, int PageSize)
        {
            var domiciliosQuery = _context.Domicilio.Where(d =>!d.Prueba && d.DeletedAt == null && d.MunicipioCalculado == "ND");
            if (!string.IsNullOrEmpty(request.Id))
            {
                domiciliosQuery = domiciliosQuery.Where(d => d.Beneficiarios.Any(b => b.Id == request.Id && b.PadreId==null));
            }

            if (!string.IsNullOrEmpty(request.Folio))
            {
                domiciliosQuery = domiciliosQuery.Where(d => d.Beneficiarios.Any(b => b.Folio == request.Folio && b.PadreId == null));
            }

            if (!string.IsNullOrEmpty(request.Inicio))
            {
                domiciliosQuery = domiciliosQuery.Where(d => d.CreatedAt >= DateTime.Parse(request.Inicio).Date);
            }

            if (!string.IsNullOrEmpty(request.Fin))
            {
                domiciliosQuery = domiciliosQuery.Where(d => d.CreatedAt <= DateTime.Parse(request.Fin).Date);
            }

            if (!string.IsNullOrEmpty(request.MunicipioId))
            {
                domiciliosQuery = domiciliosQuery.Where(d => d.MunicipioId.Equals(request.MunicipioId));
            }

            if (!string.IsNullOrEmpty(request.LocalidadId))
            {
                domiciliosQuery = domiciliosQuery.Where(d => d.LocalidadId.Equals(request.LocalidadId));
            }

            if (!string.IsNullOrEmpty(request.ZonaImpulsoId))
            {
                domiciliosQuery = domiciliosQuery.Where(d => d.ZonaImpulsoId.Equals(request.ZonaImpulsoId));
            }

            if (!string.IsNullOrEmpty(request.EstatusDireccion))
            {
                if (request.EstatusDireccion == "1")
                {
                    domiciliosQuery = domiciliosQuery.Where(d => d.NumIntentosCoordenadas == 0);
                }
                else
                {
                    domiciliosQuery = domiciliosQuery.Where(d => d.NumIntentosCoordenadas > 0);
                }
            }
            if (PageSize > 0)
            {
                domiciliosQuery = domiciliosQuery.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize);
            }
            return domiciliosQuery.Include(d => d.Beneficiarios)
                .Include(d => d.Municipio)
                .Include(d => d.Localidad)
                .Include(d => d.ZonaImpulso).ToList();
        }

        [HttpPost]
        [Authorize]
        public string ImportarDomiciliosIncompletos(ImportarDomiciliosIncompletos model)
        {
            if (!ModelState.IsValid)
            {
                var errores = ModelState.Select(x => new { Key = x.Key, Error = x.Value.Errors.First().ErrorMessage })
                    .ToList();
                return JsonConvert.SerializeObject(errores);
            }

            try
            {
                using (var package = new ExcelPackage(model.File.OpenReadStream()))
                {
                    var archivoExcel = package.Workbook;
                    if (archivoExcel != null)
                    {
                        if (archivoExcel.Worksheets.Count > 0)
                        {
                            using (var transaction = _context.Database.BeginTransaction())
                            {
                                var now = DateTime.Now;
                                var pestana = archivoExcel.Worksheets.First();
                                for (var i = 2; i <= pestana.Dimension.End.Row; i++)
                                {
                                    if (pestana.Cells[i, 14].Value != null && !string.IsNullOrEmpty(pestana.Cells[i, 14].Value.ToString()))
                                    {
                                        var id = pestana.Cells[i, 1].Value.ToString();
                                        var beneficiario = _context.Beneficiario.FirstOrDefault(b => b.Id == id);
                                        if (beneficiario != null)
                                        {
                                            var domicilio = _context.Domicilio.FirstOrDefault(d => d.Id == beneficiario.DomicilioId);
                                            domicilio.LatitudCorregida = pestana.Cells[i, 14].Value.ToString();
                                            domicilio.LongitudCorregida = pestana.Cells[i, 15].Value.ToString();
                                            domicilio.MunicipioCalculado = "ND";
                                            domicilio.UpdatedAt = now;
                                            _context.Domicilio.Update(domicilio);
                                        }
                                    }
                                }
                                _context.SaveChanges();
                                transaction.Commit();
                                CompletarDirecciones();
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("File",
                    "Ocurrió un error importando los domicilios. Revise el archivo de excel.");
                var errors = ModelState.Where(x => x.Value.Errors.Any())
                    .Select(x => new { Key = x.Key, Error = x.Value.Errors.First().ErrorMessage }).ToList();
                Excepcion.Registrar(e);
                return JsonConvert.SerializeObject(errors);
            }

            TempData["Alert"] = "Show";
            TempData.Keep("Alert");

            return "ok";
        }

        [Authorize]
        [HttpPost]
        public async Task<string> ActualizarCalculados([FromBody] BeneficiariosRequest request) {
            var now = DateTime.Now;
            var domicilios = new List<string>();
            var inicio = new DateTime(now.Year, now.Month, 1).StartOf(DateTimeAnchor.Day).ToString("yyyy-MM-dd hh:mm:ss");
            var fin = new DateTime(now.Year, now.Month, DateTime.DaysInMonth(now.Year, now.Month)).EndOf(DateTimeAnchor.Day).ToString("yyyy-MM-dd hh:mm:ss");
            if (!string.IsNullOrEmpty(request.Inicio))
            {
                inicio = DateTime.Parse(request.Inicio.Substring(0, 10)).StartOf(DateTimeAnchor.Day).ToString("yyyy-MM-dd hh:mm:ss");
            }
            if (!string.IsNullOrEmpty(request.Fin))
            {
                fin = DateTime.Parse(request.Fin.Substring(0, 10)).EndOf(DateTimeAnchor.Day).ToString("yyyy-MM-dd hh:mm:ss");
            }
            
            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {                
                var util = new BeneficiarioCode(_context);
                var query = util.BuildBeneficiariosQuery(request, TipoConsulta.Domicilios, false, inicio, fin, false);
                command.CommandText = query.Key;
                var iParametro = 0;
                foreach (var parametro in query.Value)
                {
                    command.Parameters.Add(new SqlParameter("@" + (iParametro++), parametro));
                }
                _context.Database.OpenConnection();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            domicilios.Add(reader.GetString(26));
                        }
                    }
                }
            }
            
            var url = _configuration["WS_DIRECCION"];
            var conexion = _configuration.GetConnectionString("DefaultConnection");
            new Thread(async () => {
                var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
                optionsBuilder.UseSqlServer(conexion);
                using (var db = new ApplicationDbContext(optionsBuilder.Options))
                {
                    var client = _clientFactory.CreateClient();

                    foreach (var id in domicilios)
                    {
                        var domicilio = db.Domicilio.FirstOrDefault(m => m.Id == id);
                        try
                        {
                            await ImportacionDiagnosticos.CompletarDomicilio(domicilio, url, client);
                            DiagnosticoCode.ActualizarDomicilioEnSabana(db, domicilio);
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
            TempData["Alert"] = "Show";
            TempData.Keep("Alert");
            TempData["Mensaje"] = "Estamos actualizando los datos geograficos de los domicilio que coincidieron con tu consulta, esto lo estamos haciendo en un segundo proceso, porfavor vuelve más tarde para que veas reflejado el resultado, gracias";
            return "ok";
        }

        public void CompletarDirecciones()
        {
            var url = _configuration["WS_DIRECCION"];
            var conexion = _configuration.GetConnectionString("DefaultConnection");
            new Thread(async () => {
                var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
                optionsBuilder.UseSqlServer(conexion);
                using (var db = new ApplicationDbContext(optionsBuilder.Options))
                {
                    var client = _clientFactory.CreateClient();
                    var domicilios = db.Domicilio.Where(m => m.DeletedAt == null && m.Activa && m.MunicipioCalculado == "ND").ToList();
                    foreach (var domicilio in domicilios)
                    {
                        try
                        {
                            await ImportacionDiagnosticos.CompletarDomicilio(domicilio, url, client);
                            db.Domicilio.Update(domicilio);
                            DiagnosticoCode.ActualizarDomicilioEnSabana(db, domicilio);
                            db.SaveChanges();
                        }
                        catch (Exception e)
                        {
                            Excepcion.Registrar(e);
                        }
                    }
                }
            }).Start();
            TempData["Alert"] = "Show";
            TempData.Keep("Alert");
            TempData["Mensaje"] = "Estamos actualizando los datos geograficos de los domicilio que coincidieron con tu consulta, esto lo estamos haciendo en un segundo proceso, porfavor vuelve más tarde para que veas reflejado el resultado, gracias";
        }
    }
}