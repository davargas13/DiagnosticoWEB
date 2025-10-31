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
using DiagnosticoWeb.Validaciones;

namespace DiagnosticoWeb.Controllers
{
    public class CategoriaController : Controller
    {
        public readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CategoriaController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        [Authorize]
        [RequireClaim("Permiso", Value = "encuesta.ver")]
        public IActionResult Index()
        {
            return View("Index");
        }

        public string getCategorias([FromBody] CategoriaRequest request)
        {
            var response = new CategoriaResponse();
            var categoriasQuery = _context.Carencia.Where(c => c.DeletedAt == null);
            response.Total = categoriasQuery.Count();
            response.Carencias = categoriasQuery.OrderBy(e => e.Id).Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize).Include(c => c.Padre)
                .ToList();
            return JsonSedeshu.SerializeObject(response);
        }

        [HttpGet]
        [Authorize]
        public IActionResult CreateEdit(string id = "")
        {
            var carencia = new CarenciaModel();
            var carenciaDB = _context.Carencia.Find(id);
            var nueva = carenciaDB == null;
            ViewData["Title"] = nueva ? "Crear categoría" : "Editar categoría";

            carencia.Id = nueva ? "" : carenciaDB.Id;
            carencia.IdAnterior = nueva ? "" : carenciaDB.Id;
            carencia.Clave = nueva ? "" : carenciaDB.Clave;
            carencia.Nombre = nueva ? "" : carenciaDB.Nombre;
            carencia.Color = nueva ? "" : carenciaDB.Color;
            carencia.PadreId = nueva ? "" : carenciaDB.PadreId;

            carencia.Categorias = new List<Carencia>();
            var carenciasDB = _context.Carencia.Where(c => c.PadreId == null);
            if (!string.IsNullOrEmpty(id))
            {
                carenciasDB = carenciasDB.Where(c => !c.Id.Equals(id));
            }

            foreach (var c in carenciasDB.OrderBy(c => c.Nombre))
            {
                carencia.Categorias.Add(new Carencia
                {
                    Id = c.Id,
                    Nombre = c.Nombre
                });
            }

            return View("Create", carencia);
        }

        /// <summary>
        /// Funcion para guardar en la base de datos la informacion del nivel educativo
        /// </summary>
        /// <param name="model">Datos del nivel educativo</param>
        /// <returns>Estatus del guardado; ok, error</returns>
        [HttpPost]
        [Authorize]
        [RequireClaim("Permiso", Value = "encuesta.editar")]
        public string Guardar([FromBody] CarenciaModel model)
        {
            if (_context.Carencia.Any(x => x.DeletedAt == null && x.Id == model.Id && x.Id != model.IdAnterior))
            {
                ModelState.AddModelError("Id", "Este Id ya fue creado.");
            }

            if (_context.Carencia.Any(x => x.Id != model.Id && model.Nombre != model.Nombre))
            {
                ModelState.AddModelError("Nombre", "Esta categoría de discapacidad ya fue creada.");
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Select(x => new {Key = x.Key, Error = x.Value.Errors.First().ErrorMessage})
                    .ToList();

                return JsonConvert.SerializeObject(errors);
            }

            using (var transaction = _context.Database.BeginTransaction())
            {
                var now = DateTime.Now;
                var carenciaDB = _context.Carencia.Find(model.Id);
                if (carenciaDB == null)
                {
                    carenciaDB = new Carencia();
                    carenciaDB.Id = model.Id;
                    carenciaDB.Nombre = model.Nombre;
                    carenciaDB.Clave = model.Clave;
                    carenciaDB.Color = model.Color;
                    carenciaDB.PadreId = string.IsNullOrEmpty(model.PadreId) ? null : model.PadreId;
                    carenciaDB.CreatedAt = now;
                    carenciaDB.UpdatedAt = now;
                    _context.Carencia.Add(carenciaDB);

                    _context.Bitacora.Add(new Bitacora()
                    {
                        UsuarioId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                        Accion = AccionBitacora.INSERCION,
                        Mensaje = "Se insertó la categoría " + model.Nombre + ".",
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    });
                }
                else
                {
                    carenciaDB.Id = model.Id;
                    carenciaDB.Nombre = model.Nombre;
                    carenciaDB.Clave = model.Clave;
                    carenciaDB.Color = model.Color;
                    carenciaDB.PadreId = model.PadreId;
                    carenciaDB.UpdatedAt = now;
                    carenciaDB.DeletedAt = null;
                    _context.Carencia.Update(carenciaDB);

                    _context.Bitacora.Add(new Bitacora()
                    {
                        UsuarioId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                        Accion = AccionBitacora.EDICION,
                        Mensaje = "Se modificó la causa de discapacidad " + model.Nombre + ".",
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    });
                }

                _context.SaveChanges();
                transaction.Commit();
            }

            TempData["Alert"] = "Show";
            TempData.Keep("Alert");
            return "ok";
        }
    }
}