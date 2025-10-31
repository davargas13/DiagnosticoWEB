using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OfficeOpenXml;
using DiagnosticoWeb.Code;
using DiagnosticoWeb.Database;
using DiagnosticoWeb.Models;
using DiagnosticoWeb.Validaciones;

namespace DiagnosticoWeb.Controllers
{
    public class PerfilController : Controller
    {
        public readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public PerfilController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        [Authorize]
        [RequireClaim("Permiso", Value = "perfil.ver")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public string getPermisos([FromBody] PerfilRequest request)
        {
            var response = new PerfilResponse();
            var perfiles = _context.Perfil.AsQueryable();

            if (!string.IsNullOrEmpty(request.Perfil)) {
                perfiles = perfiles.Where(x => x.Nombre.Contains(request.Perfil));
            }

            response.Total = perfiles.Count();
            var list = perfiles.OrderBy(x => x.Nombre).Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize).ToList();

            var perfilesResult = new List<PerfilList>();
            foreach (var l in list) {
                perfilesResult.Add(new PerfilList() {
                    Id = l.Id,
                    Nombre = l.Nombre,
                    Permisos = _context.PerfilPermiso.Count(x => x.PerfilId.Equals(l.Id) && x.DeletedAt == null)
                });
            }

            response.Perfiles = perfilesResult;

            return JsonConvert.SerializeObject(response);
        }

        [HttpGet]
        [Authorize]
        [RequireClaim("Permiso", Value = "perfil.editar")]
        public IActionResult CreateOrEdit(string Id = "") {
            var response = new PerfilCreateEdit();
            
            if (string.IsNullOrEmpty(Id)) {
                response.Id = "";
                response.Nombre = "";
                response.PermisosIds = new List<string>();

                ViewData["Title"] = "Crear perfil";
            } else {
                response.Id = Id;
                response.Nombre = _context.Perfil.Find(Id).Nombre;
                response.PermisosIds = _context.PerfilPermiso.Where(x => x.PerfilId.Equals(Id) && x.DeletedAt == null)
                                            .Select(x => x.PermisoId).ToList();

                ViewData["Title"] = "Editar perfil";
            }

            response.Permisos = _context.Permiso.Where(x => x.DeletedAt == null).ToList();

            return View(response);
        }

        [HttpPost]
        [Authorize]
        public string Guardar([FromBody] PerfilCreateEdit model)
        {
            if (!ModelState.IsValid) {
                var errors = ModelState.Select(x => new { Key = x.Key, Error = x.Value.Errors.First().ErrorMessage }).ToList();

                return JsonConvert.SerializeObject(errors);
            }

            if (string.IsNullOrEmpty(model.Id)) {
                var perfil = new Perfil() {
                    Nombre = model.Nombre,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
                _context.Perfil.Add(perfil);

                foreach (var permiso in model.PermisosIds) {
                    _context.PerfilPermiso.Add(new PerfilPermiso()
                    {
                        PerfilId = perfil.Id,
                        PermisoId = permiso,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    });
                }

                _context.Bitacora.Add(new Bitacora()
                {
                    UsuarioId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                    Accion = AccionBitacora.INSERCION,
                    Mensaje = "Se insert� el perfil " + perfil.Nombre + ".",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                });
            } else {
                var perfil = _context.Perfil.Find(model.Id);
                perfil.Nombre = model.Nombre;
                perfil.UpdatedAt = DateTime.Now;

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
                        _context.PerfilPermiso.Add(new PerfilPermiso()
                        {
                            PerfilId = perfil.Id,
                            PermisoId = permiso,
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now
                        });
                    }                    
                }

                _context.Bitacora.Add(new Bitacora()
                {
                    UsuarioId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                    Accion = AccionBitacora.EDICION,
                    Mensaje = "Se modific� el perfil " + perfil.Nombre + ".",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                });
            }
            _context.SaveChanges();

            TempData["Alert"] = "Show";
            TempData.Keep("Alert");

            return "ok";
        }
    }
}