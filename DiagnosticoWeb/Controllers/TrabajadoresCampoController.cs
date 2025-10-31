using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using DiagnosticoWeb.Code;
using DiagnosticoWeb.Database;
using DiagnosticoWeb.Models;
using DiagnosticoWeb.Validaciones;

namespace DiagnosticoWeb.Controllers
{
    /// <summary>
    /// Clase con la logica de negocio para la visualizacion, adicion y edicion de los trabajadores de campo, tambien llamados promotores,
    /// los cuales usaran la aplicacion movil para el registro de beneficiarios y de solicitudes
    /// </summary>
    public class TrabajadoresCampoController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private const string Llave = "tresfact";
                
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="context">Conexion a la base de datos</param>
        /// <param name="userManager">Variable para saber que usuario esta usando el sistema</param>
        public TrabajadoresCampoController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        /// <summary>
        /// Funcion que muestra la vista con el listado de trabajadores de campo registrados en la base de datos
        /// </summary>
        /// <returns>Vista con el listado de trabajadores de campo registrados en la base de datos</returns>
        [HttpGet]
        [Authorize]
        [Authorize(Policy = "Trabajadores")]
        [RequireClaim("Permiso", Value = "trabajadorcampo.ver")]
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Funcion que consulta en la base de datos los trabajadores de campo registrados de acuerdo a los filtros de
        /// busqueda seleccinados por el usuario
        /// </summary>
        /// <param name="request">Filtros de busqueda</param>
        /// <returns>Cadena JSON con el listado de trabajadores de campo</returns>
        [HttpPost]
        [Authorize]
        public string GetTrabajadores([FromBody] TrabajadoresCampoRequest request) {
            var response = new TrabajadoresCampoResponse();
            var admin = HttpContext.User.IsInRole("Administrador");
            IQueryable<Trabajador> usuariosQuery;
            if (!admin)
            {
                var user = _userManager.GetUserAsync(HttpContext.User).Result;
                usuariosQuery = _context.Trabajador.Where(x => x.DeletedAt == null).Join(_context.TrabajadorDependencia, t=>t.Id, td=>td.TrabajadorId, (x,t)=>new
                {
                    Trabajador=x,
                    TrabajadorDependencia=t
                }).Where(t=>t.TrabajadorDependencia.DependenciaId.Equals(user.DependenciaId)&&t.TrabajadorDependencia.DeletedAt==null)
                    .Select(t=>t.Trabajador);
            }
            else
            { 
                usuariosQuery = _context.Trabajador.Where(x => x.DeletedAt == null);
            }


            if (!string.IsNullOrEmpty(request.Nombre)) {
                usuariosQuery = usuariosQuery.Where(x => x.Nombre.Contains(request.Nombre));
            }

            if (!string.IsNullOrEmpty(request.Username)) {
                usuariosQuery = usuariosQuery.Where(x => x.Username.Contains(request.Username));
            }

            if (!string.IsNullOrEmpty(request.Email)) {
                usuariosQuery = usuariosQuery.Where(x => x.Email.Contains(request.Email));
            }

            response.Total = usuariosQuery.Count();

            var usuarios = usuariosQuery.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize).ToList();
            var usuariosList = new List<TrabajadorList>();
            foreach (var usuario in usuarios) {
                var dependencias = _context.TrabajadorDependencia.Where(x => x.DeletedAt == null && x.TrabajadorId.Equals(usuario.Id))
                    .OrderBy(x => x.UpdatedAt).Join(_context.Dependencia, UserDep => UserDep.DependenciaId, Dep => Dep.Id,
                        (UserDep, Dep) => new { UsuarioDependencia = UserDep, Dependencia = Dep })
                    .Select(x => x.Dependencia.Nombre).ToArray();

                usuariosList.Add(new TrabajadorList()
                {
                    Id = usuario.Id,
                    Nombre = usuario.Nombre,
                    //Email = usuario.Email,
                    Username = usuario.Username,
                    Dependencias = dependencias.Length > 3 ? dependencias.Length + " dependencias." : string.Join(", ", dependencias)
                });
            }

            response.Usuarios = usuariosList;

            return JsonConvert.SerializeObject(response);
        }

        /// <summary>
        /// Funcion que muesra la vista con el formulario para la captura de la informacion de un trabajador de campo
        /// </summary>
        /// <param name="id">Identificador en base de datos del trabajador de campo, null si el registro es nuevo</param>
        /// <returns>Vista con el formulario para la captura de la informacion de un trabajador de campo</returns>
        [HttpGet]
        [Authorize]
        [Authorize(Policy = "Trabajadores")]
        [RequireClaim("Permiso", Value = "trabajadorcampo.editar")]
        public IActionResult CreateEdit(string id = "") {
            TrabajadorCampoCreateEditModel usuario;
            if (string.IsNullOrEmpty(id)) {
                usuario = new TrabajadorCampoCreateEditModel()
                {
                    Id = "",
                    Nombre = "",
                    Email = "",
                    UserName = "",
                    Password = "",
                    DependenciaId = "",
                    Codigo = "",
                    Regiones = new List<string>()
                };
                ViewData["Title"] = "Crear trabajador de campo";
            } else {
                var user = _context.Trabajador.Find(id);
                var dependencias = _context.TrabajadorDependencia.Where(x => x.TrabajadorId == id && x.DeletedAt == null)
                    .OrderBy(x => x.UpdatedAt)
                    .Select(x => x.DependenciaId).ToList();
                var regiones = _context.TrabajadorRegion.Where(tr => tr.DeletedAt == null && tr.TrabajadorId.Equals(id));
                usuario = new TrabajadorCampoCreateEditModel()
                {
                    Id = user.Id,
                    Nombre = user.Nombre,
                    Email = user.Email,
                    UserName = user.Username,
                    Codigo = user.Codigo,
                    Password = user.Password.Substring(0,6),
                    DependenciaId = dependencias.First()==null?"":dependencias.First(),
                    Regiones = regiones.Select(tr => tr.ZonaId).ToList()
                };
                ViewData["Title"] = "Editar trabajador de campo";
            }

            return View(usuario);
        }

        /// <summary>
        /// Funcion que consulta las dependencias registradas en la base de datos para que el usuario asigne al trabajador
        /// de campo en alguna depdencia
        /// </summary>
        /// <returns>Cadena JSON con el listado de dependencias</returns>
        [HttpPost]
        [Authorize]
        public string GetDependencias()
        {
            var admin = HttpContext.User.IsInRole("Administrador");
            var response = new DependenciasResponseSelect();
            if (!admin)
            {
                var user = _userManager.GetUserAsync(HttpContext.User).Result;
                response.Dependencias = _context.Dependencia.Where(d => d.Id.Equals(user.DependenciaId)).ToList();
            }
            else
            {
                response.Dependencias = _context.Dependencia.OrderBy(x => x.UpdatedAt).ToList();
                
            }
            return JsonConvert.SerializeObject(response);
        }

        /// <summary>
        /// Funcion que guarda la informacion del trabajador de campo en la base de datos
        /// </summary>
        /// <param name="model">Datos del trabajador de campo</param>
        /// <returns>Estatus del guardado: ok, error</returns>
        [HttpPost]
        [Authorize]
        [Authorize(Policy = "Trabajadores")]
        public string Guardar([FromBody] TrabajadorCampoCreateEditModel model) {
            var now = DateTime.Now;
            var resultEmail = _context.Trabajador.Any(x => x.Id != model.Id && x.Email == model.Email && x.DeletedAt == null);
            if (resultEmail) {
                ModelState.AddModelError("Email", "Este correo electrónico ya existe.");
            }

            var resultUsername = _context.Trabajador.Any(x => x.Id != model.Id && x.Username == model.UserName && x.DeletedAt == null);
            if (resultUsername) {
                ModelState.AddModelError("UserName", "Este nombre de usuario ya existe.");
            }

            if (!ModelState.IsValid) {
                var errors = ModelState.Select(x => new { Key = x.Key, Error = x.Value.Errors.First().ErrorMessage }).ToList();

                return JsonConvert.SerializeObject(errors);
            }

            if (string.IsNullOrEmpty(model.Id)) {
                var usuario = new Trabajador()
                {
                    Nombre = model.Nombre,
                    Email = model.Email,
                    Codigo = model.Codigo,
                    Username = model.UserName,
                    Password = Utils.HashDescryptableString(Llave, model.Password, true),
                    CreatedAt = now,
                    UpdatedAt = now
                };

                _context.Trabajador.Add(usuario);
                var dependencia = new TrabajadorDependencia();
                dependencia.TrabajadorId = usuario.Id;
                dependencia.DependenciaId = model.DependenciaId;
                dependencia.CreatedAt = now;
                dependencia.UpdatedAt = now;

                _context.TrabajadorDependencia.Add(dependencia);
                foreach (var region in model.Regiones)
                {
                    var usuarioRegion = new TrabajadorRegion()
                    {
                        TrabajadorId = usuario.Id,
                        ZonaId = region,
                        CreatedAt = now,
                        UpdatedAt = now
                    };
                    _context.TrabajadorRegion.Add(usuarioRegion);
                }

                _context.Bitacora.Add(new Bitacora()
                {
                    UsuarioId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                    Accion = AccionBitacora.INSERCION,
                    Mensaje = "Se insertó el trabajador de campo " + usuario.Nombre + ".",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                });
            } else {
                var usuario = _context.Trabajador.Find(model.Id);
                var dependenciasHash = _context.TrabajadorDependencia.Where(x => x.DeletedAt == null && x.TrabajadorId.Equals(usuario.Id))
                    .ToDictionary(x => x.TrabajadorId + "," + x.DependenciaId);
                var regiones = _context.TrabajadorRegion.Where(ur => ur.TrabajadorId.Equals(usuario.Id)).ToDictionary(ur=>ur.ZonaId);
                foreach (var dep in dependenciasHash.Values) {
                    dep.UpdatedAt = now;
                    dep.DeletedAt = now;
                    _context.TrabajadorDependencia.Update(dep);
                }
                foreach (var region in regiones)
                {
                    region.Value.DeletedAt = now;
                    _context.TrabajadorRegion.Update(region.Value);
                }

                usuario.Nombre = model.Nombre;
                usuario.Email = model.Email;
                usuario.Codigo = model.Codigo;
                usuario.Username = model.UserName;
                if (!usuario.Password.Substring(0,6).Equals(model.Password.Substring(0,6)))
                {
                    usuario.Password = Utils.HashDescryptableString(Llave, model.Password, true);
                }
                usuario.UpdatedAt = now;

                _context.Trabajador.Update(usuario);
                var trabajadorDependencia =
                    _context.TrabajadorDependencia.First(td => td.TrabajadorId.Equals(usuario.Id));
                trabajadorDependencia.DependenciaId = model.DependenciaId;
                trabajadorDependencia.UpdatedAt = now;
                trabajadorDependencia.DeletedAt = null;
                _context.TrabajadorDependencia.Update(trabajadorDependencia);
                foreach (var region in model.Regiones)
                {
                    TrabajadorRegion usuarioRegion;
                    regiones.TryGetValue(region, out usuarioRegion);
                    if (usuarioRegion == null)
                    {
                        usuarioRegion = new TrabajadorRegion()
                        {
                            TrabajadorId = usuario.Id,
                            ZonaId = region,
                            CreatedAt = now,
                            UpdatedAt = now
                        };
                        _context.TrabajadorRegion.Add(usuarioRegion);
                    }
                    else
                    {
                        usuarioRegion.DeletedAt = null;
                        _context.TrabajadorRegion.Update(usuarioRegion);
                    }
                }

                _context.Bitacora.Add(new Bitacora()
                {
                    UsuarioId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                    Accion = AccionBitacora.EDICION,
                    Mensaje = "Se modificó el trabajador de campo " + usuario.Nombre + ".",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                });
            }
            _context.SaveChanges();

            TempData["Alert"] = "Show";
            TempData.Keep("Alert");

            return "ok";
        }

        /// <summary>
        /// Funcion que encripta la contraseña definida por el usuario
        /// </summary>
        /// <param name="password">Contraseña del usuario</param>
        /// <returns>Contraseña encriptada</returns>
        /// <exception cref="ArgumentNullException">Error que sucede cuando la contraseña especificada esta vacia</exception>
        public string HashPassword(string password)
        {
            byte[] salt;
            byte[] buffer2;
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }

            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, 0x10, 0x3e8))
            {
                salt = bytes.Salt;
                buffer2 = bytes.GetBytes(0x20);
            }
            byte[] dst = new byte[0x31];
            Buffer.BlockCopy(salt, 0, dst, 1, 0x10);
            Buffer.BlockCopy(buffer2, 0, dst, 0x11, 0x20);
            return Convert.ToBase64String(dst);
        }

        /// <summary>
        /// Funcion que marca como inactivo al trabajador de campo seleccionado por el usuario
        /// </summary>
        /// <param name="model">Datos de trabajador de campo a inactivar</param>
        /// <returns>Estatus de la actualizacion: ok, error</returns>
        [HttpPost]
        [Authorize]
        [Authorize(Policy = "Trabajadores")]
        public string Eliminar([FromBody] TrabajadorCampoCreateEditModel model)
        {
            var trabajador = _context.Trabajador.Find(model.Id);
            trabajador.DeletedAt = DateTime.Now;
            _context.Trabajador.Update(trabajador);

            _context.Bitacora.Add(new Bitacora() {
                UsuarioId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                Accion = AccionBitacora.ELIMINACION,
                Mensaje = "Se eliminó el trabajador de campo " + trabajador.Nombre + ".",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            });

            _context.SaveChanges();

            return "ok";
        }
    }
}
