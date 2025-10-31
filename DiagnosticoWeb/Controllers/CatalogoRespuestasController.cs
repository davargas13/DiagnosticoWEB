using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using DiagnosticoWeb.Database;
using DiagnosticoWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiagnosticoWeb.Controllers
{
    public class CatalogoRespuestasController : Controller
    { 
        private readonly ApplicationDbContext _context;

        public CatalogoRespuestasController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Index ()
        {
            return View();
        }

//        [HttpPost]
//        [Authorize]
//        public string ObtenerCatalogos([FromBody] CatalogoRequest request) {
//            var catalogos = _context.Catalogo.Include(x => x.CatalogoRespuestas)
//                .Where(x => x.DeletedAt == null);
//
//            if (!string.IsNullOrEmpty(request.Nombre)) {
//                catalogos = catalogos.Where(x => x.Nombre.Contains(request.Nombre));
//            }
//
//            var response = new CatalogoResponse();
//            response.Total = catalogos.Count();
//
//            var catalogosList = new List<CatalogoListModel>();
//            
//            foreach (var catalogo in catalogos) {
//                var catResps = catalogo.CatalogoRespuestas.Where(x => x.DeletedAt == null);
//
//                catalogosList.Add(new CatalogoListModel() {
//                    Id = catalogo.Id,
//                    Nombre = catalogo.Nombre,
//                    Respuestas = catResps.Count() > 3 ? catResps.Count() + " respuestas." 
//                                : string.Join(", ", catResps.Select(x => x.Nombre).ToArray())
//                });
//            }
//            response.Catalogos = catalogosList;
//
//            return JsonConvert.SerializeObject(response);
//        }
//
//        [HttpGet]
//        [Authorize]
//        public IActionResult CreateEdit(string id = "")
//        {
//            CatalogoCreateEditModel catalogo;
//            if (!string.IsNullOrEmpty(id)) {
//                ViewData["Title"] = "Editar catálogo de respuestas";
//
//                var catalogosList = _context.Catalogo.Include(x => x.CatalogoRespuestas).Where(x => x.DeletedAt == null && x.Id.Equals(id)).First();
//
//                var catalogoRespuestas = new List<CatalogoRespuestaCreateEditModel>();
//                foreach (var catResp in catalogosList.CatalogoRespuestas.Where(x => x.DeletedAt == null).ToList()) {
//                    catalogoRespuestas.Add(new CatalogoRespuestaCreateEditModel() {
//                        Id = catResp.Id,
//                        Nombre = catResp.Nombre,
//                        CatalogoId = catResp.CatalogoId
//                    });
//                }
//
//                catalogo = new CatalogoCreateEditModel() {
//                    Id = catalogosList.Id,
//                    Nombre = catalogosList.Nombre,
//                    CatalogoRespuestas = catalogoRespuestas
//                };
//            } else {
//                ViewData["Title"] = "Crear catálogo de respuestas";
//
//                var CatalogoRespuestas = new List<CatalogoRespuestaCreateEditModel>();
//                CatalogoRespuestas.Add(new CatalogoRespuestaCreateEditModel()
//                {
//                    Id = "",
//                    Nombre = "",
//                    CatalogoId = ""
//                });
//
//                catalogo = new CatalogoCreateEditModel()
//                {
//                    Id = "",
//                    Nombre = "",
//                    CatalogoRespuestas = CatalogoRespuestas
//                };
//            }
//
//            return View(catalogo);
//        }
//
//        [HttpPost]
//        [Authorize]
//        public string Guardar([FromBody] CatalogoCreateEditModel model)
//        {
//            var nose = _context.Catalogo.Any(x => x.DeletedAt == null && x.Nombre.Equals(model.Nombre) && !x.Id.Equals(model.Id));
//            if (_context.Catalogo.Any(x => x.DeletedAt == null && x.Nombre.Equals(model.Nombre) && !x.Id.Equals(model.Id)))
//            {
//                ModelState.AddModelError("Nombre", "Este nombre de catálogo ya ha sido creado.");
//            }
//            if (!ModelState.IsValid) {
//                var errores = ModelState.Select(x => new { Key = x.Key, Error = x.Value.Errors.First().ErrorMessage }).ToList();
//
//                return JsonConvert.SerializeObject(errores);
//            }
//
//            if (!string.IsNullOrEmpty(model.Id)) {
//                var catalogo = _context.Catalogo.Find(model.Id);
//                catalogo.Nombre = model.Nombre;
//                catalogo.UpdatedAt = DateTime.Now;
//
//                _context.Catalogo.Update(catalogo);
//
//                var catalogoRespuestaHash = _context.CatalogoRespuesta.Where(x => x.DeletedAt == null && x.CatalogoId.Equals(model.Id))
//                        .ToDictionary(x => x.Id);
//                foreach (var catResp in catalogoRespuestaHash.Values) {
//                    catResp.UpdatedAt = DateTime.Now;
//                    catResp.DeletedAt = DateTime.Now;
//                    _context.CatalogoRespuesta.Update(catResp);
//                }
//
//                foreach (var catalogoRespuestas in model.CatalogoRespuestas) {
//                    CatalogoRespuesta catalogoResp;
//                    if (catalogoRespuestaHash.TryGetValue(catalogoRespuestas.Id, out catalogoResp)) {
//                        catalogoResp.Nombre = catalogoRespuestas.Nombre;
//                        catalogoResp.UpdatedAt = DateTime.Now;
//                        catalogoResp.DeletedAt = null;
//
//                        _context.CatalogoRespuesta.Update(catalogoResp);
//                    } else {
//                        catalogoResp = new CatalogoRespuesta() {
//                            Nombre = catalogoRespuestas.Nombre,
//                            CatalogoId = catalogo.Id,
//                            CreatedAt = DateTime.Now,
//                            UpdatedAt = DateTime.Now,
//                            Catalogo = catalogo
//                        };
//
//                        _context.CatalogoRespuesta.Add(catalogoResp);
//                    }
//                }
//            } else {
//                var catalogo = new Catalogo() {
//                    Nombre = model.Nombre,
//                    CreatedAt = DateTime.Now,
//                    UpdatedAt = DateTime.Now
//                };
//                _context.Catalogo.Add(catalogo);
//
//                var catalogoRespuestas = new List<CatalogoRespuesta>();
//                foreach(var catalogoResp in model.CatalogoRespuestas) {
//                    var catResp = new CatalogoRespuesta() {
//                        Nombre = catalogoResp.Nombre,
//                        CatalogoId = catalogo.Id,
//                        Catalogo = catalogo,
//                        CreatedAt = DateTime.Now,
//                        UpdatedAt = DateTime.Now
//                    };
//                    _context.CatalogoRespuesta.Add(catResp);
//                    catalogoRespuestas.Add(catResp);
//                }
//
//                catalogo.CatalogoRespuestas = catalogoRespuestas;
//            }
//
//            _context.SaveChanges();
//
//            TempData["Alert"] = "Show";
//            TempData.Keep("Alert");
//
//            return "ok";
//        }
    }
}
