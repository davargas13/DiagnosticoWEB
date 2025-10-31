using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using DiagnosticoWeb.Code;
using DiagnosticoWeb.Database;
using DiagnosticoWeb.Models;
using DiagnosticoWeb.Validaciones;

namespace DiagnosticoWeb.Controllers
{
    /// <summary>
    /// Clase con la logica de negocio para la visualizacion, adicion, edicion de los usuarios analistas y administrativos de las dependencias
    /// </summary>
    public class UsuariosController : Controller
    {
        public readonly ApplicationDbContext _context;
        public readonly UserManager<ApplicationUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="context">Conexion a la base de datos</param>
        /// <param name="userManager">Variable para identificar que usuario esta usando el sistema</param>
        /// <param name="roleMgr">Variable para identificar poder asignar a los usuarios en los roles que ya estan definidos</param>
        public UsuariosController(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleMgr)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleMgr;
        }

        /// <summary>
        /// Funcion que muestra la vista con el listado de usuario registrados en la base de datos
        /// </summary>
        /// <returns>Vista con el listado de usuario registrados en la base de datos</returns>
        [HttpGet]
        [Authorize]
        [RequireClaim("Permiso", Value = "usuarios.ver")]
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Funcion que consulta en la base de datos los usuarios registrados de acuerdo a los filtros de busqueda
        /// seleccionados por el usuario
        /// </summary>
        /// <param name="request">Filtros de busqueda</param>
        /// <returns>Cadena JSON con el listado de usuarios encotrados</returns>
        [HttpPost]
        [Authorize]
        public string GetUsuarios([FromBody] TrabajadorRequest request)
        {
            var response = new TrabajadorResponse();
            var usuarios = _context.Users.Include(u => u.Dependencia).Where(x => x.Id != null);

            if (!string.IsNullOrEmpty(request.Nombre))
            {
                usuarios = usuarios.Where(x => x.Name.Contains(request.Nombre));
            }

            if (!string.IsNullOrEmpty(request.UserName))
            {
                usuarios = usuarios.Where(x => x.UserName.Contains(request.UserName));
            }

            if (!string.IsNullOrEmpty(request.Email))
            {
                usuarios = usuarios.Where(x => x.Email.Contains(request.Email));
            }

            response.Total = usuarios.Count();
            response.Usuarios = usuarios.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize)
                .ToList();
            return JsonConvert.SerializeObject(response);
        }

        /// <summary>
        /// Funcion que muestra la vista para la captura o edicion de un usuario
        /// </summary>
        /// <param name="id">Identificador en base de datos del usuario a modificar, null si el registro es nuevo</param>
        /// <returns>Vista con el formulario para configurar los datos de un usuario</returns>
        [HttpGet]
        [Authorize]
        [RequireClaim("Permiso", Value = "usuarios.editar")]
        public async Task<IActionResult> CreateEdit(string id = "")
        {
            var roles = _roleManager.Roles.ToDictionary(r => r.Name);
            var dependencias = _context.Dependencia.Where(d => d.DeletedAt == null).ToList();
            var perfiles = _context.Perfil.Where(x => x.DeletedAt == null).ToList();
            TrabajadorCreateEditModel trabajador;

            if (string.IsNullOrEmpty(id))
            {
                ViewData["Title"] = "Crear usuario";
                trabajador = new TrabajadorCreateEditModel()
                {
                    Name = "",
                    UserName = "",
                    Email = "",
                    Password = "",
                    Roles = roles.Values.ToList(),
                    Dependencias = dependencias,
                    Dependencia = null,
                    Rol = "",
                    Perfil = "",
                    Perfiles = perfiles,
                    IsDependencia = "false",
                    Regiones = new List<string>(),
                    IsDisabled = false,
                    Codigo="0000"
                };
            }
            else
            {
                ViewData["Title"] = "Editar usuario";
                var user = _userManager.FindByIdAsync(id).Result;
                var regiones = _context.UsuarioRegion.Where(us => us.DeletedAt == null && us.UsuarioId.Equals(user.Id));
                var rols = await _userManager.GetRolesAsync(user);
                trabajador = new TrabajadorCreateEditModel()
                {
                    Id = user.Id,
                    Name = user.Name,
                    UserName = user.UserName,
                    Email = user.Email,
                    Password = user.PasswordHash.Substring(0, 6),
                    Dependencias = dependencias,
                    Dependencia = user.DependenciaId,
                    Roles = roles.Values.ToList(),
                    Perfil = user.PerfilId,
                    Perfiles = perfiles,
                    IsDependencia = "false",
                    Regiones = regiones.Select(r => r.ZonaId).ToList(),
                    IsDisabled = DateTime.Compare(user.LastLoginDate ?? DateTime.Now, DateTime.Now.AddMonths(-6)) < 0,
                    Codigo="0000"
                };
                foreach (var rol in rols)
                {
                    trabajador.Rol = roles[rol].NormalizedName;
                }
                if (trabajador.Rol!=null && trabajador.Rol.Equals("Administrador")) {
                    trabajador.IsDisabled = false;
                }
            }

            return View("CreateEdit", trabajador);
        }

        [HttpPost]
        [Authorize]
        public string Activar([FromBody] ActivarUsuario model)
        {
            var user = _context.Users.Find(model.Id);
            user.LastLoginDate = DateTime.Now;
            _context.Users.Update(user);
            _context.SaveChanges();

            TempData["Alert"] = "Show";
            TempData.Keep("Alert");

            return "ok";
        }

        /// <summary>
        /// Funcion que consulta en la base de datos las dependencias para que se pueda asociar la dependencia a la que
        /// pertenece el usuario
        /// </summary>
        /// <returns>Cadena JSON con las dependencias encontradas</returns>
        [HttpPost]
        [Authorize]
        public string GetDependencias()
        {
            var response = new DependenciasResponseSelect();

            response.Dependencias = _context.Dependencia.ToList();

            return JsonConvert.SerializeObject(response);
        }

        /// <summary>
        /// Funcion que guarda en la base de datos la informacion del usuario
        /// </summary>
        /// <param name="model">Datos del usuario</param>
        /// <returns>Estatus del guardado: ok, error</returns>
        [HttpPost]
        [Authorize]
        public async Task<string> Guardar([FromBody] TrabajadorCreateEditModel model)
        {
            var now = DateTime.Now;
            var username = model.UserName;
            var resultEmail = _context.Users.Any(x => x.Id != model.Id && x.Email == model.Email);
            if (resultEmail)
            {
                ModelState.AddModelError("Email", "Este correo electrónico ya existe.");
            }

            var resultUsername = _context.Users.Any(x => x.Id != model.Id && x.UserName == username);
            if (resultUsername)
            {
                ModelState.AddModelError("UserName", "Este nombre de usuario ya existe.");
            }

            if (string.IsNullOrEmpty(model.Perfil)) {
                ModelState.AddModelError("Perfil", "Es necesario seleccionar un perfil.");
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Select(x => new {Key = x.Key, Error = x.Value.Errors.First().ErrorMessage})
                    .ToList();

                return JsonConvert.SerializeObject(errors);
            }

            ApplicationUser user = await _userManager.FindByIdAsync(model.Id);

            if (string.IsNullOrEmpty(model.Id))
            {
                user = new ApplicationUser()
                {
                    Name = model.Name,
                    UserName = username,
                    Email = model.Email,
                    PasswordHash = model.Password,
                    EmailConfirmed = true,
                    DependenciaId = (model.Dependencia == "" || model.Rol == "Administrador") ? null : model.Dependencia,
                    LastLoginDate = DateTime.Now,
                    PerfilId = model.Perfil
                };
                await _userManager.CreateAsync(user, model.Password);
                await _userManager.AddToRoleAsync(user, model.Rol);
                foreach (var region in model.Regiones)
                {
                    var usuarioRegion = new UsuarioRegion()
                    {
                        UsuarioId = user.Id,
                        ZonaId = region,
                        CreatedAt = now,
                        UpdatedAt = now
                    };
                    _context.UsuarioRegion.Add(usuarioRegion);
                }

                _context.Bitacora.Add(new Bitacora() {
                    UsuarioId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                    Accion = AccionBitacora.INSERCION,
                    Mensaje = "Se insertó el usuario " + user.Name + ".",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                });

                _context.SaveChanges();
            }
            else
            {
                var regiones = _context.UsuarioRegion.Where(ur => ur.UsuarioId.Equals(user.Id))
                    .ToDictionary(ur => ur.ZonaId);
                user.Name = model.Name;
                user.UserName = username;
                user.Email = model.Email;
                user.DependenciaId = (model.Dependencia == "" || model.Rol == "Administrador")
                    ? null : model.Dependencia;
                user.PerfilId = model.Perfil;

                if (!user.PasswordHash.Substring(0, 6).Equals(model.Password))
                {
                    user.PasswordHash = HashPassword(model.Password);
                }

                if (!_userManager.GetRolesAsync(user).Result.Contains(model.Rol))
                {
                    await _userManager.RemoveFromRoleAsync(user, model.Rol);
                    await _userManager.AddToRoleAsync(user, model.Rol);
                }

                foreach (var region in regiones)
                {
                    region.Value.DeletedAt = now;
                    _context.UsuarioRegion.Update(region.Value);
                }

                foreach (var region in model.Regiones)
                {
                    UsuarioRegion usuarioRegion;
                    regiones.TryGetValue(region, out usuarioRegion);
                    if (usuarioRegion == null)
                    {
                        usuarioRegion = new UsuarioRegion()
                        {
                            UsuarioId = user.Id,
                            ZonaId = region,
                            CreatedAt = now,
                            UpdatedAt = now
                        };
                        _context.UsuarioRegion.Add(usuarioRegion);
                    }
                    else
                    {
                        usuarioRegion.DeletedAt = null;
                        _context.UsuarioRegion.Update(usuarioRegion);
                    }
                }

                _context.Users.Update(user);

                _context.Bitacora.Add(new Bitacora()
                {
                    UsuarioId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                    Accion = AccionBitacora.EDICION,
                    Mensaje = "Se modificó el usuario " + user.Name + ".",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                });
                _context.SaveChanges();
            }

            TempData["Alert"] = "Show";
            TempData.Keep("Alert");

            return "ok";
        }

        /// <summary>
        /// Funcion que encripta la contraseña del usuario
        /// </summary>
        /// <param name="password">Cadena a encriptar</param>
        /// <returns>Cadena encriptada</returns>
        /// <exception cref="ArgumentNullException">Error que sucede cuando la contraseña a encriptar esta vacia</exception>
        public static string HashPassword(string password)
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
    }
}