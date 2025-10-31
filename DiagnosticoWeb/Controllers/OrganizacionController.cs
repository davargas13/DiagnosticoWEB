using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using DiagnosticoWeb.Code;
using DiagnosticoWeb.Database;
using DiagnosticoWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace DiagnosticoWeb.Controllers
{
    public class OrganizacionController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private const string Llave = "tresfact";
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="context">Conexion a la base de datos</param>
        /// <param name="userManager">Variable para saber que usuario esta usando el sistema</param>
        public OrganizacionController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, 
            RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }
        /// <summary>
        /// Funcion que muestra la vista con el listado de trabajadores de campo registrados en la base de datos
        /// </summary>
        /// <returns>Vista con el listado de trabajadores de campo registrados en la base de datos</returns>
        [HttpGet]
        [Authorize]
        public IActionResult IndexTrabajadores()
        {
            if (!((ClaimsIdentity) User.Identity).HasClaim("Permiso", "trabajadorcampo.ver"))
            {
                return Forbid();
            }
            var isAdmin = HttpContext.User.IsInRole("Administrador");
            var user = _userManager.GetUserAsync(HttpContext.User).Result;
            return View("TrabajadoresCampo/Index",JsonSedeshu.SerializeObject(new
            {
                Title = _configuration["ID_SISTEMA"]=="1"?"Encuestador":"Articulador",
                Dependencias = Utils.GetDepedenciasByUsuario(isAdmin, user, _context)
            }));
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
            if (!((ClaimsIdentity) User.Identity).HasClaim("Permiso", "trabajadorcampo.ver"))
            {
                return null;
            }
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
                if (!string.IsNullOrEmpty(request.DependenciaId))
                {
                    usuariosQuery = _context.Trabajador.Where(x => x.DeletedAt == null).Join(_context.TrabajadorDependencia, t=>t.Id, td=>td.TrabajadorId, (x,t)=>new
                        {
                            Trabajador=x,
                            TrabajadorDependencia=t
                        }).Where(t=>t.TrabajadorDependencia.DependenciaId.Equals(request.DependenciaId) &&
                                    t.TrabajadorDependencia.DeletedAt==null)
                        .Select(t=>t.Trabajador);
                }
                else
                {
                    usuariosQuery = _context.Trabajador.Where(x => x.DeletedAt == null);
                }
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
            
            usuariosQuery = usuariosQuery.Where(x => x.Tipo == (_configuration["ID_SISTEMA"] == "1" ? 
                TipoTrabajador.ENCUESTADOR:TipoTrabajador.ARTICULADOR));

            var total = usuariosQuery.Count();

            var usuarios = usuariosQuery.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize).ToList();
            var usuariosList = new List<TrabajadorList>();
            foreach (var usuario in usuarios) {
                var dependencias = _context.TrabajadorDependencia.Where(x => x.DeletedAt == null && x.TrabajadorId.Equals(usuario.Id))
                    .OrderBy(x => x.UpdatedAt).Join(_context.Dependencia, UserDep => UserDep.DependenciaId, Dep => Dep.Id,
                        (UserDep, Dep) => new { UsuarioDependencia = UserDep, Dependencia = Dep })
                    .Select(x => x.Dependencia.Nombre).ToArray();

                usuariosList.Add(new TrabajadorList
                {
                    Id = usuario.Id,
                    Nombre = usuario.Nombre,
                    Tipo = usuario.Tipo==0?"Encuestador":"Articulador",
                    Username = usuario.Username,
                    Dependencias = dependencias.Length > 3 ? dependencias.Length + " dependencias." : string.Join(", ", dependencias)
                });
            }
            
            return JsonConvert.SerializeObject(new
            {
                Usuarios = usuariosList,
                Total = total
            });
        }

        /// <summary>
        /// Funcion que muesra la vista con el formulario para la captura de la informacion de un trabajador de campo
        /// </summary>
        /// <param name="id">Identificador en base de datos del trabajador de campo, null si el registro es nuevo</param>
        /// <returns>Vista con el formulario para la captura de la informacion de un trabajador de campo</returns>
        [HttpGet]
        [Authorize] 
        public IActionResult CreateEditTrabajadores(string id = "") {
            if (!((ClaimsIdentity) User.Identity).HasClaim("Permiso", "trabajadorcampo.editar"))
            {
                return Forbid();
            }
            TrabajadorCampoCreateEditModel usuario;
            if (string.IsNullOrEmpty(id)) {
                usuario = new TrabajadorCampoCreateEditModel
                {
                    Id = "",
                    Nombre = "",
                    Email = "",
                    UserName = "",
                    Password = "",
                    DependenciaId = "",
                    Codigo = "",
                    Tipo = _configuration["ID_SISTEMA"] == "1" ? TipoTrabajador.ENCUESTADOR:TipoTrabajador.ARTICULADOR,
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
                    Tipo = _configuration["ID_SISTEMA"] == "1" ? TipoTrabajador.ENCUESTADOR:TipoTrabajador.ARTICULADOR,
                    Password = user.Password.Substring(0,6),
                    DependenciaId = dependencias.First()==null?"":dependencias.First(),
                    Regiones = regiones.Select(tr => tr.ZonaId).ToList()
                };
                ViewData["Title"] = "Editar trabajador de campo";
            }

            return View("TrabajadoresCampo/CreateEdit", JsonSedeshu.SerializeObject(usuario));
        }

        /// <summary>
        /// Funcion que guarda la informacion del trabajador de campo en la base de datos
        /// </summary>
        /// <param name="model">Datos del trabajador de campo</param>
        /// <returns>Estatus del guardado: ok, error</returns>
        [HttpPost]
        [Authorize]
        public string GuardarTrabajador([FromBody] TrabajadorCampoCreateEditModel model) {
            if (!((ClaimsIdentity) User.Identity).HasClaim("Permiso", "trabajadorcampo.editar"))
            {
                return null;
            }
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
                var usuario = new Trabajador
                {
                    Id =Guid.NewGuid().ToString(),
                    Nombre = model.Nombre,
                    Email = model.Email,
                    Codigo = model.Codigo,
                    Tipo = model.Tipo,
                    Username = model.UserName,
                    Password = Utils.HashDescryptableString(Llave, model.Password, true),
                    CreatedAt = now,
                    UpdatedAt = now
                };

                _context.Trabajador.Add(usuario);
                var dependencia = new TrabajadorDependencia
                {
                    Id = Guid.NewGuid().ToString(),
                    TrabajadorId = usuario.Id, DependenciaId = model.DependenciaId, CreatedAt = now, UpdatedAt = now
                };

                _context.TrabajadorDependencia.Add(dependencia);
                foreach (var region in model.Regiones)
                {
                    var usuarioRegion = new TrabajadorRegion
                    {
                        Id = Guid.NewGuid().ToString(),
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
                usuario.Tipo = model.Tipo;
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
        public string EliminarTrabajador([FromBody] TrabajadorCampoCreateEditModel model)
        {
            if (!((ClaimsIdentity) User.Identity).HasClaim("Permiso", "trabajadorcampo.eliminar"))
            {
                return null;
            }
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
        
        [HttpGet]
        [Authorize]
        public IActionResult IndexPerfiles()
        {
            if (!((ClaimsIdentity) User.Identity).HasClaim("Permiso", "perfil.ver"))
            {
                return Forbid();
            }
            return View("Perfil/Index");
        }

        [HttpPost]
        [Authorize]
        public string getPermisos([FromBody] PerfilRequest request)
        {
            if (!((ClaimsIdentity) User.Identity).HasClaim("Permiso", "perfil.ver"))
            {
                return null;
            }

            var idSistema = int.Parse(_configuration["ID_SISTEMA"]);
            var sistemas = new List<int> {0, idSistema};
            var response = new PerfilResponse();
            var perfiles = _context.Perfil.Where(p=>p.DeletedAt == null&&sistemas.Contains(p.Sistema));

            if (!string.IsNullOrEmpty(request.Perfil)) {
                perfiles = perfiles.Where(x => x.Nombre.Contains(request.Perfil));
            }

            response.Total = perfiles.Count(p=> sistemas.Contains(p.Sistema));
            var list = perfiles.OrderBy(x => x.Nombre).Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize).ToList();

            var perfilesResult = new List<PerfilList>();
            foreach (var l in list) {
                perfilesResult.Add(new PerfilList {
                    Id = l.Id,
                    Nombre = l.Nombre,
                    Sistema = l.Sistema,
                    Permisos = _context.PerfilPermiso.Count(x => x.PerfilId == l.Id && x.DeletedAt == null)
                });
            }

            response.Perfiles = perfilesResult;

            return JsonConvert.SerializeObject(response);
        }

        [HttpGet]
        [Authorize]
        public IActionResult CreateEditPerfil(string Id = "") {
            if (!((ClaimsIdentity) User.Identity).HasClaim("Permiso", "perfil.editar"))
            {
                return Forbid();
            }
            var response = new PerfilCreateEdit();
            var idSistema = int.Parse(_configuration["ID_SISTEMA"]);
            if (string.IsNullOrEmpty(Id)) {
                response.Id = "";
                response.Nombre = "";
                response.PermisosIds = new List<string>();
                response.Sistema = idSistema;
                ViewData["Title"] = "Crear perfil";
            } else {
                var perfilDb = _context.Perfil.Find(Id);
                response.Id = Id;
                response.Nombre = perfilDb.Nombre;
                response.PermisosIds = _context.PerfilPermiso.Where(x => x.PerfilId.Equals(Id) && x.DeletedAt == null)
                                            .Select(x => x.PermisoId).ToList();
                response.Sistema = perfilDb.Sistema;
                ViewData["Title"] = "Editar perfil";
            }

            var sistemas = new List<int> {0, idSistema};
            response.Permisos = _context.Permiso.Where(x => x.DeletedAt == null && sistemas.Contains(x.Sistema)).
                OrderBy(x=>x.Clave).ToList();
            return View("Perfil/CreateEdit", JsonSedeshu.SerializeObject(response));
        }

        [HttpPost]
        [Authorize]
        public string GuardarPerfil([FromBody] PerfilCreateEdit model)
        {
            if (!((ClaimsIdentity) User.Identity).HasClaim("Permiso", "perfil.editar"))
            {
                return null;
            }
            if (!ModelState.IsValid) {
                var errors = ModelState.Select(x => new { Key = x.Key, Error = x.Value.Errors.First().ErrorMessage }).ToList();

                return JsonConvert.SerializeObject(errors);
            }

            if (string.IsNullOrEmpty(model.Id)) {
                var perfil = new Perfil {
                    Id = Guid.NewGuid().ToString(),
                    Nombre = model.Nombre,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
                if (model.Sistema > 0)
                {
                    perfil.Sistema = int.Parse(_configuration["ID_SISTEMA"]);
                }
                _context.Perfil.Add(perfil);

                foreach (var permiso in model.PermisosIds) {
                    _context.PerfilPermiso.Add(new PerfilPermiso
                    {
                        Id = Guid.NewGuid().ToString(),
                        PerfilId = perfil.Id,
                        PermisoId = permiso,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    });
                }

                _context.Bitacora.Add(new Bitacora
                {
                    UsuarioId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                    Accion = AccionBitacora.INSERCION,
                    Mensaje = "Se insertó el perfil " + perfil.Nombre + ".",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                });
            } else {
                var perfil = _context.Perfil.Find(model.Id);
                perfil.Nombre = model.Nombre;
                perfil.UpdatedAt = DateTime.Now;
                if (model.Sistema > 0)
                {
                    perfil.Sistema = int.Parse(_configuration["ID_SISTEMA"]);
                }
                _context.Perfil.Update(perfil);

                var perfilPermisos = _context.PerfilPermiso.Where(x => x.DeletedAt == null 
                                                    && x.PerfilId.Equals(perfil.Id)).ToDictionary(x => x.PermisoId);
                foreach (var perfilPermiso in perfilPermisos.Values) {
                    perfilPermiso.DeletedAt = DateTime.Now;

                    _context.PerfilPermiso.Update(perfilPermiso);
                }

                foreach (var permiso in model.PermisosIds) {
                    PerfilPermiso perfilPermiso;
                    if (perfilPermisos.TryGetValue(permiso, out perfilPermiso)) {
                        perfilPermiso.DeletedAt = null;

                        _context.PerfilPermiso.Update(perfilPermiso);
                    } else {
                        _context.PerfilPermiso.Add(new PerfilPermiso
                        {
                            Id = Guid.NewGuid().ToString(),
                            PerfilId = perfil.Id,
                            PermisoId = permiso,
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now
                        });
                    }                    
                }

                _context.Bitacora.Add(new Bitacora
                {
                    UsuarioId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                    Accion = AccionBitacora.EDICION,
                    Mensaje = "Se modificó el perfil " + perfil.Nombre + ".",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                });
            }
            _context.SaveChanges();

            TempData["Alert"] = "Show";
            TempData.Keep("Alert");

            return "ok";
        }
        
        [HttpPost]
        [Authorize]
        public async Task<string> EliminarPerfil([FromBody] PerfilCreateEdit model)
        {
            if (!((ClaimsIdentity) User.Identity).HasClaim("Permiso", "perfil.eliminar"))
            {
                return null;
            }
            var perfil = _context.Perfil.FirstOrDefault(e=>e.Id==model.Id);
            
            if (perfil != null)
            {
                perfil.DeletedAt = DateTime.Now;
                _context.Perfil.Update(perfil);
                _context.SaveChanges();
            }
            return "ok";
        }
        
        /// <summary>
        /// Funcion que muestra la vista para la consulta de las dependencias registradas en la base de datos
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public IActionResult IndexDependencias()
        {
            if (!((ClaimsIdentity) User.Identity).HasClaim("Permiso", "dependencias.ver"))
            {
                return Forbid();
            }
            return View("Dependencias/Index");
        }

        /// <summary>
        /// Funcion que consulta en la base de datos las dependencias registradas de acuerdo a los filtros seleccionados por el usuario
        /// </summary>
        /// <param name="request">Filtros seleccionados por el usuario</param>
        /// <returns>Cadena JSON con el listado de dependencias encontradas</returns>
        [HttpPost]
        [Authorize]
        public string GetDependenciasList([FromBody] DependenciasRequest request)
        {
            if (!((ClaimsIdentity) User.Identity).HasClaim("Permiso", "dependencias.ver"))
            {
                return null;
            }
            var response = new DependenciasResponse();
            var dependencias = _context.Dependencia.Where(v => v.DeletedAt == null);

            if (!string.IsNullOrEmpty(request.Nombre))
            {
                dependencias = dependencias.Where(v => v.Nombre.Contains(request.Nombre));
            }

            response.Total = dependencias.Count();
            response.Dependencias = dependencias.OrderBy(x => x.Nombre).Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize).ToList();
            return JsonConvert.SerializeObject(response);
        }

        /// <summary>
        /// Funcion que meustra la vista para la captura o modificacion en la informacion de una dependencia
        /// </summary>
        /// <param name="id">Identificador de la dependencia a modificar o null si es una nueva dependencia</param>
        /// <returns>Vista para la edicion o creacion de una dependencia</returns>
        [HttpGet]
        [Authorize]
        public IActionResult CreateEditDependencia(string id = "")
        {
            if (!((ClaimsIdentity) User.Identity).HasClaim("Permiso", "dependencias.editar"))
            {
                return Forbid();
            }
            var municipiosId = new List<string>();
            DependenciaCreateEditModel model;
            var municipios = _context.Municipio.ToDictionary(m => m.Id);

            if (string.IsNullOrEmpty(id))
            {
                ViewData["Title"] = "Crear dependencia";
                model = new DependenciaCreateEditModel()
                {
                    Id = "",
                    Nombre = "",
                    Zonas = new List<Zona>()
                };
            }
            else
            {
                ViewData["Title"] = "Editar dependencia";
                var dependencia = _context.Dependencia.Where(x => x.Id.Equals(id)).First();
                var zonas = _context.Zona.Where(z => z.DependenciaId.Equals(dependencia.Id) && z.DeletedAt == null)
                    .ToList();
                model = new DependenciaCreateEditModel()
                {
                    Id = dependencia.Id,
                    Nombre = dependencia.Nombre,
                    Zonas = zonas,
                };
                foreach (var zona in model.Zonas)
                {
                    var municipiosZona = _context.MunicipioZona
                        .Where(mz => mz.ZonaId.Equals(zona.Id) && mz.DeletedAt == null).ToList();
                    zona.Municipios = new List<MunicipiosZona>();
                    foreach (var municipioZona in municipiosZona)
                    {
                        var mz = new MunicipiosZona();
                        mz.Id = municipioZona.MunicipioId;
                        mz.Nombre = municipios[municipioZona.MunicipioId].Nombre;
                        municipiosId.Add(municipioZona.MunicipioId);
                        zona.Municipios.Add(mz);
                    }
                }
            }
            model.Municipios = municipios.Values.Where(m => !municipiosId.Contains(m.Id)).ToList();
            return View("Dependencias/CreateEdit", JsonSedeshu.SerializeObject(model));
        }

        /// <summary>
        /// Funcion que registra en la base de datos la informacion de la dependencia
        /// </summary>
        /// <param name="model">Datos de la dependencia</param>
        /// <returns>Estatus de registro en la base de datos</returns>
        [HttpPost]
        [Authorize]
        public string GuardarDependencia([FromBody] DependenciaCreateEditModel model)
        {
            if (!((ClaimsIdentity) User.Identity).HasClaim("Permiso", "dependencias.editar"))
            {
                return null;
            }
            var result = _context.Dependencia.Any(x => x.Nombre == model.Nombre && x.Id != model.Id && x.DeletedAt == null);
            if (result) {
                ModelState.AddModelError("Nombre", "Esta dependencia ya fue creada.");
            }
            var izona = 0;
            foreach (var zonaModel in model.Zonas)
            {
                if (zonaModel.Municipios.Count == 0)
                {
                    ModelState.AddModelError("Zonas["+izona+"].Municipios", "La región no tiene municipios asociados.");    
                }

                izona++;
            }
            if (!ModelState.IsValid) {
                var errors = ModelState.Select(x => new { Key = x.Key, Error = x.Value.Errors.First().ErrorMessage }).ToList();

                return JsonConvert.SerializeObject(errors);
            }


            using (var transaction = _context.Database.BeginTransaction())
            {
                var now = DateTime.Now;

                Dependencia dep;

                if (string.IsNullOrEmpty(model.Id)) {
                    dep = new Dependencia {
                        Id = Guid.NewGuid().ToString(),
                        Nombre = model.Nombre,
                        CreatedAt = now,
                        UpdatedAt = now
                    };
                    _context.Dependencia.Add(dep);

                    _context.Bitacora.Add(new Bitacora
                    {
                        UsuarioId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                        Accion = AccionBitacora.INSERCION,
                        Mensaje = "Se insertó la dependencia " + dep.Nombre + ".",
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    });
                } else {
                    dep = _context.Dependencia.Find(model.Id);
                    dep.Nombre = model.Nombre;
                    dep.UpdatedAt = now;

                    _context.Dependencia.Update(dep);

                    _context.Bitacora.Add(new Bitacora
                    {
                        UsuarioId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                        Accion = AccionBitacora.EDICION,
                        Mensaje = "Se modificó la dependencia " + dep.Nombre + ".",
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    });
                }

                var zonasDb = _context.Zona.Where(z => z.DependenciaId.Equals(dep.Id)).ToDictionary(z => z.Id);
                var municipiosDic = new Dictionary<string, Dictionary<string, MunicipioZona>>();
                foreach (var zonadb in zonasDb) {
                    zonadb.Value.DeletedAt = now;
                    _context.Zona.Update(zonadb.Value);
                    municipiosDic.Add(zonadb.Key, _context.MunicipioZona.Where(mz => mz.ZonaId.Equals(zonadb.Key) && mz.DeletedAt==null)
                        .ToDictionary(mz=>mz.MunicipioId));
                    foreach (var zonaMunicipioDB in municipiosDic[zonadb.Key])
                    {
                        zonaMunicipioDB.Value.DeletedAt = now;
                        _context.MunicipioZona.Update(zonaMunicipioDB.Value);
                    }
                }

                foreach (var zonaModel in model.Zonas) {
                    var zona = new Zona();
                    if (zonaModel.Id != null && zonasDb.ContainsKey(zonaModel.Id)) {
                        zona = zonasDb[zonaModel.Id];
                        zona.Clave = zonaModel.Clave;
                        zona.UpdatedAt = now;
                        zona.DeletedAt = null;
                        _context.Zona.Update(zona);
                    } else
                    {
                        zona.Id = Guid.NewGuid().ToString();
                        zona.Clave = zonaModel.Clave;
                        zona.DependenciaId = dep.Id;
                        zona.CreatedAt = now;
                        zona.UpdatedAt = now;
                        _context.Zona.Add(zona);
                    }
                    foreach (var municipio in zonaModel.Municipios)
                    {
                        var zonaMunicipio = new MunicipioZona();
                        if (zonaModel.Id != null && municipiosDic.ContainsKey(zonaModel.Id) && municipiosDic[zonaModel.Id].ContainsKey(municipio.Id))
                        {
                            zonaMunicipio = municipiosDic[zonaModel.Id][municipio.Id];
                            zonaMunicipio.UpdatedAt = now;
                            zonaMunicipio.DeletedAt = null;
                            _context.MunicipioZona.Update(zonaMunicipio);
                        }
                        else
                        {
                            zonaMunicipio.Id = Guid.NewGuid().ToString();
                            zonaMunicipio.CreatedAt = now;
                            zonaMunicipio.UpdatedAt = now;
                            zonaMunicipio.ZonaId = zona.Id;
                            zonaMunicipio.MunicipioId = municipio.Id;
                            _context.MunicipioZona.Add(zonaMunicipio);
                        }
                    }
                }

                _context.SaveChanges();
                transaction.Commit();
            }

            TempData["Alert"] = "Show";
            TempData.Keep("Alert");

            return "ok";
        }

        /// <summary>
        /// Funcion que consulta las regiones que tiene configurada la dependencia, recordar que cada dependencia puede asignar sus propias zonas del estado
        /// </summary>
        /// <param name="id">Identificador en base de datos de la dependencia</param>
        /// <returns>Cadena JSON con las regiones y sus municipios de la dependencia</returns>
        [HttpGet]
        [Authorize]
        public string GetRegiones(string id="")
        {
            var regiones = new List<Zona>();
            var zonas = _context.Zona.Where(z => z.DeletedAt == null && z.DependenciaId.Equals(id)).Include(z=>z.MunicipioZonas).ThenInclude(mz=>mz.Municipio);
            foreach (var zona in zonas)
            {
                var region = new Zona()
                {
                    Id = zona.Id,
                    Clave = zona.Clave,
                    MunicipioZonas = new List<MunicipioZona>()
                };
                foreach (var zonaMunicipioZona in zona.MunicipioZonas.OrderBy(mz=>mz.Municipio.Nombre))
                {
                    region.MunicipioZonas.Add(new MunicipioZona()
                    {
                        Municipio = new Municipio()
                        {
                            Id = zonaMunicipioZona.MunicipioId,
                            Nombre = zonaMunicipioZona.Municipio.Nombre
                        }
                    });
                }
                regiones.Add(region);
            }
            return JsonSedeshu.SerializeObject(regiones);
        }
        
        /// <summary>
        /// Funcion que muestra la vista con el listado de usuario registrados en la base de datos
        /// </summary>
        /// <returns>Vista con el listado de usuario registrados en la base de datos</returns>
        [HttpGet]
        [Authorize]
        public IActionResult IndexUsuarios()
        {
            if (!((ClaimsIdentity) User.Identity).HasClaim("Permiso", "usuarios.ver"))
            {
                return Forbid();
            }
            var isAdmin = HttpContext.User.IsInRole("Administrador");
            var user = _userManager.GetUserAsync(HttpContext.User).Result;
            return View("Usuarios/Index",  JsonSedeshu.SerializeObject(Utils.GetDepedenciasByUsuario(isAdmin, user, _context)));
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
            if (!((ClaimsIdentity) User.Identity).HasClaim("Permiso", "usuarios.ver"))
            {
                return null;
            }
            var usuarios = _context.Users.Include(u => u.Dependencia).Where(x => x.Id != null);
            var admin = HttpContext.User.IsInRole("Administrador");

            if (!admin)
            {
                var idSistema = int.Parse(_configuration["ID_SISTEMA"]);
                var sistemas = new List<int> {0, idSistema};
                var perfiles = _context.Perfil.Where(x => x.DeletedAt == null&&sistemas.Contains(x.Sistema)).
                    Select(p=>p.Id).ToList();
                var user = _userManager.GetUserAsync(HttpContext.User).Result;
                usuarios = usuarios.Where(x => x.DependenciaId == user.DependenciaId && perfiles.Contains(x.PerfilId));
            }
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
            
            if (!string.IsNullOrEmpty(request.DependenciaId))
            {
                usuarios = usuarios.Where(x => x.DependenciaId == request.DependenciaId);
            }

            var total = usuarios.Count();
            var usuariosList = usuarios.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize)
                .ToList();
            foreach (var usuario in usuarios)
            {
                usuario.IsDisabled =
                    DateTime.Compare(usuario.LastLoginDate ?? DateTime.Now, DateTime.Now.AddMonths(-6)) < 0 ||
                    (usuario.LockoutEnd.HasValue && usuario.LockoutEnd.Value < DateTimeOffset.Now);
            }
            return JsonConvert.SerializeObject(new
            {
                Total = total,
                Usuarios = usuariosList
            });
        }

        /// <summary>
        /// Funcion que muestra la vista para la captura o edicion de un usuario
        /// </summary>
        /// <param name="id">Identificador en base de datos del usuario a modificar, null si el registro es nuevo</param>
        /// <returns>Vista con el formulario para configurar los datos de un usuario</returns>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> CreateEditUsuario(string id = "")
        {
            if (!((ClaimsIdentity) User.Identity).HasClaim("Permiso", "usuarios.editar"))
            {
                return Forbid();
            }
            var roles = _roleManager.Roles.ToDictionary(r => r.Name);
            var isAdmin = HttpContext.User.IsInRole("Administrador");
            var userSesion = _userManager.GetUserAsync(HttpContext.User).Result;
            var dependencias = Utils.GetDepedenciasByUsuario(isAdmin, userSesion, _context);
            var idSistema = int.Parse(_configuration["ID_SISTEMA"]);
            var sistemas = new List<int> {0, idSistema};
            var perfiles = _context.Perfil.Where(x => x.DeletedAt == null&&sistemas.Contains(x.Sistema)).
                OrderBy(p=>p.Nombre).ToList();
            TrabajadorCreateEditModel trabajador;

            if (string.IsNullOrEmpty(id))
            {
                ViewData["Title"] = "Crear usuario";
                trabajador = new TrabajadorCreateEditModel
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
                var user = _context.Users.FirstOrDefault(u => u.Id == id);
                var regiones = _context.UsuarioRegion.Where(us => us.DeletedAt == null && us.UsuarioId.Equals(user.Id));
                var rols = await _userManager.GetRolesAsync(user);
                trabajador = new TrabajadorCreateEditModel
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
                    IsDisabled = DateTime.Compare(user.LastLoginDate ?? DateTime.Now, DateTime.Now.AddMonths(-6)) < 0 || 
                    (user.LockoutEnd.HasValue && user.LockoutEnd.Value > DateTimeOffset.Now),
                    Codigo="0000"
                };
                foreach (var rol in rols)
                {
                    trabajador.Rol = roles[rol].NormalizedName;
                }
            }

            return View("Usuarios/CreateEdit", JsonSedeshu.SerializeObject(trabajador));
        }

        [HttpPost]
        [Authorize]
        public string Activar([FromBody] ActivarUsuario model)
        {
            if (!((ClaimsIdentity) User.Identity).HasClaim("Permiso", "trabajadorcampo.editar"))
            {
                return null;
            }
            var user = _context.Users.Find(model.Id);
            user.LastLoginDate = DateTime.Now;
            user.LockoutEnd = DateTimeOffset.Now;
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
            var isAdmin = HttpContext.User.IsInRole("Administrador");
            var user = _userManager.GetUserAsync(HttpContext.User).Result;

            response.Dependencias = Utils.GetDepedenciasByUsuario(isAdmin, user, _context);

            return JsonConvert.SerializeObject(response);
        }

        /// <summary>
        /// Funcion que guarda en la base de datos la informacion del usuario
        /// </summary>
        /// <param name="model">Datos del usuario</param>
        /// <returns>Estatus del guardado: ok, error</returns>
        [HttpPost]
        [Authorize]
        public async Task<string> GuardarUsuario([FromBody] TrabajadorCreateEditModel model)
        {
            if (!((ClaimsIdentity) User.Identity).HasClaim("Permiso", "usuarios.editar"))
            {
                return null;
            }
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
                user = new ApplicationUser
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
                    var usuarioRegion = new UsuarioRegion
                    {
                        Id = Guid.NewGuid().ToString(),
                        UsuarioId = user.Id,
                        ZonaId = region,
                        CreatedAt = now,
                        UpdatedAt = now
                    };
                    _context.UsuarioRegion.Add(usuarioRegion);
                }

                _context.Bitacora.Add(new Bitacora {
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
                await _userManager.AddToRoleAsync(user, model.Rol);
                var roles = _userManager.GetRolesAsync(user).Result.Where(r=>r != model.Rol);
                await _userManager.RemoveFromRolesAsync(user, roles.ToArray());
              
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
                        usuarioRegion = new UsuarioRegion
                        {
                            Id = Guid.NewGuid().ToString(),
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

                _context.Bitacora.Add(new Bitacora
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
        /// Funcion que inhabilita en la base de datos el usuario
        /// </summary>
        /// <param name="model">Datos del encuestador</param>
        /// <returns>Estatus del guardado: ok, error</returns>
        [HttpPost]
        [Authorize]
        public async Task<string> Inhabilitar([FromBody] TrabajadorCreateEditModel model)
        {
            if (!((ClaimsIdentity) User.Identity).HasClaim("Permiso", "usuarios.editar"))
            {
                return null;
            }
            var usuario = _context.Users.FirstOrDefault(e=>e.Id==model.Id);
            
            if (usuario != null)
            {
                usuario.LastLoginDate = DateTime.Today.AddYears(-10);
                usuario.LockoutEnd = DateTime.Today.AddYears(10);
                usuario.LockoutEnabled = true;
                _context.Users.Update(usuario);
                _context.SaveChanges();
            }

            return "ok";
        }
        
        /// <summary>
        /// Funcion que habilita en la base de datos el usuario
        /// </summary>
        /// <param name="model">Datos del usuario</param>
        /// <returns>Estatus del guardado: ok, error</returns>
        [HttpPost]
        [Authorize]
        public async Task<string> Habilitar([FromBody] TrabajadorCreateEditModel model)
        {
            if (!((ClaimsIdentity) User.Identity).HasClaim("Permiso", "usuarios.editar"))
            {
                return null;
            }
            var usuario = _context.Users.FirstOrDefault(e=>e.Id==model.Id);
            
            if (usuario != null)
            {
                usuario.LastLoginDate = DateTime.Today;
                usuario.LockoutEnabled = true;
                usuario.LockoutEnd = null;
                _context.Users.Update(usuario);
                _context.SaveChanges();
            }

            return "ok";
        }
    }
}