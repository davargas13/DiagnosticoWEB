using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using DiagnosticoWeb.Code;
using DiagnosticoWeb.Database;
using DiagnosticoWeb.Models;

namespace DiagnosticoWeb.Controllers
{
    public class HijoController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public HijoController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        [Authorize]
        public ActionResult Ver(string id)
        {
            var hijoDB = _context.Beneficiario.First(b => b.DeletedAt == null && b.Id.Equals(id));
            var queryPadre = _context.Beneficiario.Where(x => x.DeletedAt == null && x.HijoId.Equals(hijoDB.Id));
            Beneficiario padreDB = null;
            if (queryPadre.Count() > 0)
            {
                padreDB = queryPadre.First();
            }
            var hijoResponse = new HijoResponse();
            hijoResponse.Discapacidades = new List<Discapacidad>();
            hijoResponse.Grados = new List<Grado>();
            hijoResponse.Causas = new List<CausaDiscapacidad>();
            hijoResponse.Estados = new List<Estado>();
            hijoResponse.Municipios = new List<Municipio>();
            hijoResponse.Estudios = new List<Estudio>();
            hijoResponse.Documentos = new List<ImagenModel>();
            hijoResponse.GradosEstudios = new List<GradosEstudio>();
            hijoResponse.Sexos = new List<Sexo>();
            hijoResponse.EstadosCiviles = new List<EstadoCivil>();
            hijoResponse.Parentescos = new List<Parentesco>();
            hijoResponse.Huellas = new List<Huella>();
            foreach (var discapacidad in _context.Discapacidad.Where(d => d.DeletedAt == null))
            {
                hijoResponse.Discapacidades.Add(new Discapacidad()
                {
                    Id = discapacidad.Id,
                    Nombre = discapacidad.Nombre
                });
            }

            foreach (var causa in _context.CausaDiscapacidad.Where(d => d.DeletedAt == null))
            {
                hijoResponse.Causas.Add(new CausaDiscapacidad()
                {
                    Id = causa.Id,
                    Nombre = causa.Nombre
                });
            }

            foreach (var estado in _context.Estado.Where(d => d.DeletedAt == null))
            {
                hijoResponse.Estados.Add(new Estado()
                {
                    Id = estado.Id,
                    Nombre = estado.Nombre
                });
            }

            foreach (var municipio in _context.Municipio.Where(d => d.DeletedAt == null))
            {
                hijoResponse.Municipios.Add(new Municipio()
                {
                    Id = municipio.Id,
                    Nombre = municipio.Nombre
                });
            }

            foreach (var estudio in _context.Estudio.Where(d => d.DeletedAt == null))
            {
                hijoResponse.Estudios.Add(new Estudio()
                {
                    Id = estudio.Id,
                    Nombre = estudio.Nombre
                });
            }

            foreach (var grado in _context.GradosEstudio.Where(d => d.DeletedAt == null))
            {
                hijoResponse.GradosEstudios.Add(new GradosEstudio()
                {
                    Id = grado.Id,
                    Nombre = grado.Nombre
                });
            }

            foreach (var sexo in _context.Sexo.Where(d => d.DeletedAt == null))
            {
                hijoResponse.Sexos.Add(new Sexo()
                {
                    Id = sexo.Id,
                    Nombre = sexo.Nombre
                });
            }

            foreach (var estadoCivil in _context.EstadoCivil.Where(d => d.DeletedAt == null))
            {
                hijoResponse.EstadosCiviles.Add(new EstadoCivil()
                {
                    Id = estadoCivil.Id,
                    Nombre = estadoCivil.Nombre
                });
            }

            foreach (var parentesco in _context.Parentesco.Where(d => d.DeletedAt == null))
            {
                hijoResponse.Parentescos.Add(new Parentesco()
                {
                    Id = parentesco.Id,
                    Nombre = parentesco.Nombre
                });
            }

            if (hijoDB != null)
            {
                Domicilio domicilio = null;
                if (!hijoDB.MismoDomicilio)
                {
                    var query = _context.Domicilio.Where(d => d.DeletedAt == null &&
                                    d.Activa && d.Id.Equals(hijoDB.DomicilioId));
                    if (query.Count() > 0)
                    {
                        domicilio = query.First();
                    }
                }

                var hijo = new Hijo();
                hijo.Id = hijoDB.Id;
                hijo.PadreId = hijoDB.PadreId;
                hijo.Nombre = hijoDB.Nombre;
                hijo.ApellidoPaterno = hijoDB.ApellidoPaterno;
                hijo.ApellidoMaterno = hijoDB.ApellidoMaterno;
                hijo.Curp = hijoDB.Curp;
                hijo.FechaNacimiento = hijoDB.FechaNacimiento.Value;
                hijo.ParentescoId = hijoDB.ParentescoId;
                hijo.EstadoId = hijoDB.EstadoId;
                hijo.SexoId = hijoDB.SexoId;
                hijo.EstudioId = hijoDB.EstudioId;
                hijo.EstadoCivilId = hijoDB.EstadoCivilId;
                hijo.GradoEstudioId = hijoDB.GradoEstudioId;
                hijo.DiscapacidadId = hijoDB.DiscapacidadId;
                hijo.DiscapacidadGradoId = hijoDB.DiscapacidadGradoId;
                hijo.CausaDiscapacidadId = hijoDB.CausaDiscapacidadId;
                hijo.MismoDomicilio = hijoDB.MismoDomicilio;
                hijo.Huellas = hijoDB.Huellas;
                hijoResponse.Hijo = hijo;

                var archivosBeneficiario = _context.Archivo
                    .Where(x => x.DeletedAt == null && x.Nombre.Contains(hijo.Id)&&!x.Nombre.Contains("dedo:"))
                    .Select(a => new { a.Id, a.Nombre }).ToList();

                foreach (var archivo in archivosBeneficiario)
                {
                    hijoResponse.Documentos.Add(new ImagenModel()
                    {
                        Id = archivo.Id,
                        Nombre = archivo.Nombre.Replace(hijo.Id, "").Replace("_", " ").Trim(),
                        Show = false
                    });
                }
                if (hijo.Huellas!=null)
                {
                    hijoResponse.Huellas = JsonConvert.DeserializeObject<List<Huella>>(hijo.Huellas);
                    foreach (var huella in hijoResponse.Huellas)
                    {
                        huella.nombre = BeneficiarioCode.GetNombreDedo(huella.dedo);
                    }
                }
                if (domicilio != null)
                {
                    hijoResponse.Domicilio = new DomicilioEditModel()
                    {
                        Id = domicilio.Id,
                        DomicilioN = domicilio.DomicilioN,
                        EntreCalle1 = domicilio.EntreCalle1,
                        EntreCalle2 = domicilio.EntreCalle2,
                        CodigoPostal = domicilio.CodigoPostal,
                        Telefono = domicilio.Telefono,
                        TelefonoCasa = domicilio.TelefonoCasa,
                        Email = domicilio.Email,
                        MunicipioId = domicilio.MunicipioId,
                        LocalidadId = domicilio.LocalidadId,
                        AgebId = domicilio.AgebId,
                        Latitud = domicilio.Latitud,
                        Longitud = domicilio.Longitud,
                        CapturarUbicacion = domicilio.Latitud != "" && domicilio.Longitud != ""
                    };
                }
                else
                {
                    hijoResponse.Domicilio = new DomicilioEditModel()
                    {
                        Id = "",
                        DomicilioN = "",
                        EntreCalle1 = "",
                        EntreCalle2 = "",
                        CodigoPostal = "",
                        Telefono = "",
                        TelefonoCasa = "",
                        Email = "",
                        MunicipioId = "",
                        LocalidadId = "",
                        Latitud = "",
                        Longitud = "",
                        AgebId = "",
                        CapturarUbicacion = true
                    };
                }
            }

            if (padreDB != null)
            {
                Domicilio domicilioTutor = null;
                if (!padreDB.MismoDomicilio)
                {
                    var query = _context.Domicilio.Where(d => d.DeletedAt == null &&
                                    d.Activa && d.Id.Equals(padreDB.DomicilioId));
                    if (query.Count() > 0)
                    {
                        domicilioTutor = query.First();
                    }
                }

                var padre = new Hijo();
                padre.Id = padreDB.Id;
                padre.PadreId = padreDB.PadreId;
                padre.Nombre = padreDB.Nombre;
                padre.ApellidoPaterno = padreDB.ApellidoPaterno;
                padre.ApellidoMaterno = padreDB.ApellidoMaterno;
                padre.Curp = padreDB.Curp;
                padre.FechaNacimiento = padreDB.FechaNacimiento.Value;
                padre.ParentescoId = padreDB.ParentescoId;
                padre.EstadoId = padreDB.EstadoId;
                padre.SexoId = padreDB.SexoId;
                padre.EstudioId = padreDB.EstudioId;
                padre.EstadoCivilId = padreDB.EstadoCivilId;
                padre.GradoEstudioId = padreDB.GradoEstudioId;
                padre.DiscapacidadId = padreDB.DiscapacidadId;
                padre.DiscapacidadGradoId = padreDB.DiscapacidadGradoId;
                padre.CausaDiscapacidadId = padreDB.CausaDiscapacidadId;
                padre.MismoDomicilio = padreDB.MismoDomicilio;
                hijoResponse.Tutor = padre;
                if (domicilioTutor != null)
                {
                    hijoResponse.DomicilioTutor = new DomicilioEditModel()
                    {
                        Id = domicilioTutor.Id,
                        DomicilioN = domicilioTutor.DomicilioN,
                        EntreCalle1 = domicilioTutor.EntreCalle1,
                        EntreCalle2 = domicilioTutor.EntreCalle2,
                        CodigoPostal = domicilioTutor.CodigoPostal,
                        Telefono = domicilioTutor.Telefono,
                        TelefonoCasa = domicilioTutor.TelefonoCasa,
                        Email = domicilioTutor.Email,
                        MunicipioId = domicilioTutor.MunicipioId,
                        LocalidadId = domicilioTutor.LocalidadId,
                        AgebId = domicilioTutor.AgebId
                    };
                }
                else
                {
                    hijoResponse.DomicilioTutor = new DomicilioEditModel()
                    {
                        Id = "",
                        DomicilioN = "",
                        EntreCalle1 = "",
                        EntreCalle2 = "",
                        CodigoPostal = "",
                        Telefono = "",
                        TelefonoCasa = "",
                        Email = "",
                        MunicipioId = "",
                        LocalidadId = "",
                        AgebId = ""
                    };
                }
            }

            return View("Ver", hijoResponse);
        }

        [HttpPost]
        [Authorize]
        public string ObtenerGradoDiscapacidad([FromBody] string id = "")
        {
            var gradosResponse = new List<GradosModel>();
            var Grados = _context.DiscapacidadGrado.Where(x => x.DiscapacidadId.Equals(id)).Include(x => x.Grade).ToList();
            foreach (var grado in Grados)
            {
                gradosResponse.Add(new GradosModel()
                {
                    DiscapacidadGradoId = grado.Id,
                    GradoId = grado.GradoId,
                    Nombre = grado.Grade.Nombre
                });
            }

            return JsonConvert.SerializeObject(gradosResponse);
        }

        [HttpPost]
        [Authorize]
        public string BuscarLocalidades([FromBody] string id = "")
        {
            var Localidades = _context.Localidad.Where(x => x.MunicipioId.Equals(id)).ToList();

            return JsonConvert.SerializeObject(Localidades);
        }

        [HttpPost]
        [Authorize]
        public string BuscarAGEBs([FromBody] string id = "")
        {
            var AGEBs = _context.Ageb.Where(x => x.LocalidadId.Equals(id)).ToList();

            return JsonConvert.SerializeObject(AGEBs);
        }

        [HttpPost]
        [Authorize]
        public string Guardar([FromBody] DatosInformacion datos)
        {
            if (datos.Hijo.MismoDomicilio)
            {
                var errors = ModelState.Where(x => !x.Key.Contains("Domicilio"))
                    .Select(x => new { Key = x.Key, Error = x.Value.Errors.First().ErrorMessage }).ToList();
                if (errors.Count() > 0)
                {
                    return JsonConvert.SerializeObject(errors);
                }
            }
            else
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Select(x => new { Key = x.Key, Error = x.Value.Errors.First().ErrorMessage }).ToList();

                    return JsonConvert.SerializeObject(errors);
                }
            }

            var hijo = _context.Beneficiario.Find(datos.Hijo.Id);
            if (hijo != null)
            {
                hijo.Curp = datos.Hijo.Curp;
                hijo.ApellidoPaterno = datos.Hijo.ApellidoPaterno;
                hijo.ApellidoMaterno = datos.Hijo.ApellidoMaterno;
                hijo.Nombre = datos.Hijo.Nombre;
                hijo.FechaNacimiento = StartOfDay(datos.Hijo.FechaNacimiento ?? DateTime.Now);
                hijo.ParentescoId = datos.Hijo.ParentescoId;
                hijo.EstadoId = datos.Hijo.EstadoId;
                hijo.SexoId = datos.Hijo.SexoId;
                hijo.EstudioId = datos.Hijo.EstudioId;
                hijo.GradoEstudioId = datos.Hijo.GradoEstudioId;
                hijo.EstadoCivilId = datos.Hijo.EstadoCivilId;
                hijo.DiscapacidadId = datos.Hijo.DiscapacidadId;
                hijo.DiscapacidadGradoId = datos.Hijo.DiscapacidadGradoId;
                hijo.CausaDiscapacidadId = datos.Hijo.CausaDiscapacidadId;
                hijo.MismoDomicilio = datos.Hijo.MismoDomicilio;
                if (hijo.EstatusInformacion.Equals(EstatusInformacion.DATOS_PERSONALES))
                {
                    hijo.EstatusInformacion = EstatusInformacion.DOMICILIO;
                    hijo.EstatusUpdatedAt = DateTime.Now;
                }

                _context.Beneficiario.Update(hijo);
            }

            if (!hijo.MismoDomicilio)
            {
                var domicilio = _context.Domicilio.Find(datos.Domicilio.Id);
                if (domicilio != null)
                {
                    domicilio.Email = datos.Domicilio.Email;
                    domicilio.DomicilioN = datos.Domicilio.DomicilioN;
                    domicilio.EntreCalle1 = datos.Domicilio.EntreCalle1;
                    domicilio.EntreCalle2 = datos.Domicilio.EntreCalle2;
                    domicilio.CodigoPostal = datos.Domicilio.CodigoPostal;
                    domicilio.Telefono = datos.Domicilio.Telefono;
                    domicilio.TelefonoCasa = datos.Domicilio.TelefonoCasa;
                    domicilio.MunicipioId = datos.Domicilio.MunicipioId;
                    domicilio.LocalidadId = datos.Domicilio.LocalidadId;
                    if (datos.Domicilio.CapturarUbicacion)
                    {
                        domicilio.Latitud = datos.Domicilio.Latitud;
                        domicilio.Longitud = datos.Domicilio.Longitud;
                        domicilio.LatitudAtm = datos.Domicilio.Latitud;
                        domicilio.LongitudAtm = datos.Domicilio.Longitud;
                    }
                    else
                    {
                        domicilio.Latitud = "";
                        domicilio.Longitud = "";
                        domicilio.LatitudAtm = "";
                        domicilio.LongitudAtm = "";
                    }

                    domicilio.AgebId = datos.Domicilio.AgebId;
                    domicilio.UpdatedAt = DateTime.Now;

                    _context.Domicilio.Update(domicilio);
                }
                else
                {
                    _context.Domicilio.Where(x => x.DeletedAt == null && x.Activa && x.Id.Equals(hijo.DomicilioId))
                                .ToList().ForEach(a => a.Activa = false);

                    domicilio = new Domicilio()
                    {
                        Email = datos.Domicilio.Email,
                        DomicilioN = datos.Domicilio.DomicilioN,
                        EntreCalle1 = datos.Domicilio.EntreCalle1,
                        EntreCalle2 = datos.Domicilio.EntreCalle2,
                        CodigoPostal = datos.Domicilio.CodigoPostal,
                        Telefono = datos.Domicilio.Telefono,
                        TelefonoCasa = datos.Domicilio.TelefonoCasa,
                        MunicipioId = datos.Domicilio.MunicipioId,
                        LocalidadId = datos.Domicilio.LocalidadId,
                        AgebId = !string.IsNullOrEmpty(datos.Domicilio.AgebId) ? datos.Domicilio.AgebId : null,
                        Activa = true,
                        Latitud = "",
                        Longitud = "",
                        LatitudAtm = "",
                        LongitudAtm = "",
                        TrabajadorId = hijo.TrabajadorId,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    };

                    _context.Domicilio.Add(domicilio);
                }
            }
            else
            {
                _context.Domicilio.Where(x => x.DeletedAt == null && x.Activa && x.Id.Equals(hijo.DomicilioId))
                                .ToList().ForEach(a => a.Activa = false);
            }

            var beneficiario = _context.Beneficiario.Find(hijo.Id);
            _context.Bitacora.Add(new Bitacora()
            {
                UsuarioId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                Accion = AccionBitacora.EDICION,
                Mensaje = "Se modificó la información del hijo con CURP " + beneficiario.Curp + ".",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            });

            _context.SaveChanges();

            TempData["Alert"] = "Show";
            TempData.Keep("Alert");

            return "ok";
        }

        private DateTime StartOfDay(DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
        }

        [HttpPost]
        [Authorize]
        public string GuardarTutor([FromBody] DatosInformacionTutor datos)
        {
            if (datos.Tutor.MismoDomicilio)
            {
                var errors = ModelState.Where(x => !x.Key.Contains("Domicilio"))
                    .Select(x => new { Key = x.Key, Error = x.Value.Errors.First().ErrorMessage }).ToList();
                if (errors.Count() > 0)
                {
                    return JsonConvert.SerializeObject(errors);
                }
            }
            else
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Select(x => new { Key = x.Key, Error = x.Value.Errors.First().ErrorMessage }).ToList();

                    return JsonConvert.SerializeObject(errors);
                }
            }

            var tutor = _context.Beneficiario.Find(datos.Tutor.Id);
            if (tutor != null)
            {
                tutor.Curp = datos.Tutor.Curp;
                tutor.ApellidoPaterno = datos.Tutor.ApellidoPaterno;
                tutor.ApellidoMaterno = datos.Tutor.ApellidoMaterno;
                tutor.Nombre = datos.Tutor.Nombre;
                tutor.FechaNacimiento = StartOfDay(datos.Tutor.FechaNacimiento ?? DateTime.Now);
                tutor.EstadoId = datos.Tutor.EstadoId;
                tutor.SexoId = datos.Tutor.SexoId;
                tutor.EstudioId = datos.Tutor.EstudioId;
                tutor.GradoEstudioId = datos.Tutor.GradoEstudioId;
                tutor.EstadoCivilId = datos.Tutor.EstadoCivilId;
                tutor.DiscapacidadId = datos.Tutor.DiscapacidadId;
                tutor.DiscapacidadGradoId = datos.Tutor.DiscapacidadGradoId;
                tutor.CausaDiscapacidadId = datos.Tutor.CausaDiscapacidadId;
                tutor.MismoDomicilio = datos.Tutor.MismoDomicilio;
                if (tutor.EstatusInformacion.Equals(EstatusInformacion.DATOS_PERSONALES))
                {
                    tutor.EstatusInformacion = EstatusInformacion.DOMICILIO;
                    tutor.EstatusUpdatedAt = DateTime.Now;
                }

                _context.Beneficiario.Update(tutor);
            }

            if (!tutor.MismoDomicilio)
            {
                var domicilio = _context.Domicilio.Find(datos.DomicilioTutor.Id);
                if (domicilio != null)
                {
                    domicilio.Email = datos.DomicilioTutor.Email;
                    domicilio.DomicilioN = datos.DomicilioTutor.DomicilioN;
                    domicilio.EntreCalle1 = datos.DomicilioTutor.EntreCalle1;
                    domicilio.EntreCalle2 = datos.DomicilioTutor.EntreCalle2;
                    domicilio.CodigoPostal = datos.DomicilioTutor.CodigoPostal;
                    domicilio.Telefono = datos.DomicilioTutor.Telefono;
                    domicilio.TelefonoCasa = datos.DomicilioTutor.TelefonoCasa;
                    domicilio.MunicipioId = datos.DomicilioTutor.MunicipioId;
                    domicilio.LocalidadId = datos.DomicilioTutor.LocalidadId;
                    domicilio.AgebId = datos.DomicilioTutor.AgebId;
                    domicilio.UpdatedAt = DateTime.Now;

                    _context.Domicilio.Update(domicilio);
                }
                else
                {
                    _context.Domicilio.Where(x => x.DeletedAt == null && x.Activa && x.Id.Equals(tutor.DomicilioId))
                                .ToList().ForEach(a => a.Activa = false);

                    domicilio = new Domicilio()
                    {
                        Email = datos.DomicilioTutor.Email,
                        DomicilioN = datos.DomicilioTutor.DomicilioN,
                        EntreCalle1 = datos.DomicilioTutor.EntreCalle1,
                        EntreCalle2 = datos.DomicilioTutor.EntreCalle2,
                        CodigoPostal = datos.DomicilioTutor.CodigoPostal,
                        Telefono = datos.DomicilioTutor.Telefono,
                        TelefonoCasa = datos.DomicilioTutor.TelefonoCasa,
                        MunicipioId = datos.DomicilioTutor.MunicipioId,
                        LocalidadId = datos.DomicilioTutor.LocalidadId,
                        AgebId = !string.IsNullOrEmpty(datos.DomicilioTutor.AgebId) ? datos.DomicilioTutor.AgebId : null,
                        Activa = true,
                        Latitud = "",
                        Longitud = "",
                        LatitudAtm = "",
                        LongitudAtm = "",
                        TrabajadorId = tutor.TrabajadorId,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    };

                    _context.Domicilio.Add(domicilio);
                }
            }
            else
            {
                _context.Domicilio.Where(x => x.DeletedAt == null && x.Activa && x.Id.Equals(tutor.DomicilioId))
                                .ToList().ForEach(a => a.Activa = false);
            }

            var beneficiario = _context.Beneficiario.Find(tutor.Id);
            _context.Bitacora.Add(new Bitacora()
            {
                UsuarioId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                Accion = AccionBitacora.EDICION,
                Mensaje = "Se modificó la información de un padre o tutor con CURP " + beneficiario.Curp + ".",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            });

            _context.SaveChanges();

            TempData["Alert"] = "Show";
            TempData.Keep("Alert");

            return "ok";
        }
    }
}