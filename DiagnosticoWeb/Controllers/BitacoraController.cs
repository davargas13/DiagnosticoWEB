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
    public class BitacoraController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public BitacoraController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        [Authorize]
        [RequireClaim("Permiso", Value = "configuraciones.ver")]
        public IActionResult Index()
        {
            var model = new UsuariosBitacora();
            if (User.IsInRole("Administrador")) {
                model.Usuarios = _context.Users.ToList();
            } else if (User.IsInRole("Administrador de dependencia")) {
                var DependenciaId = _context.Users.Find(User.FindFirst(ClaimTypes.NameIdentifier).Value).DependenciaId;
                model.Usuarios = _context.Users.Where(x => x.DependenciaId.Equals(DependenciaId)).ToList();
            } else if (User.IsInRole("Analista de dependencia")) {
                model.Usuarios = new List<ApplicationUser>();
            }

            var date = DateTime.Now;
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            model.FechaInicio = firstDayOfMonth.ToString("yyyy-MM-dd");
            model.FechaFin = lastDayOfMonth.ToString("yyyy-MM-dd");

            model.Acciones = AccionBitacora.get();

            return View("Index", model);
        }

        [HttpPost]
        [Authorize]
        public string getBitacora([FromBody] BitacoraRequest request)
        {
            var response = new BitacoraResponse();
            var bitacoraQuery = _context.Bitacora.Where(x => x.DeletedAt == null);
            if (User.IsInRole("Administrador de dependencia")) {
                var DependenciaId = _context.Users.Find(User.FindFirst(ClaimTypes.NameIdentifier).Value).DependenciaId;
                var usuariosIds = _context.Users.Where(x => x.DependenciaId.Equals(DependenciaId)).Select(x => x.Id).ToList();

                bitacoraQuery = bitacoraQuery.Where(x => usuariosIds.Contains(x.UsuarioId));
            } else if (User.IsInRole("Analista de dependencia")) {
                bitacoraQuery = bitacoraQuery.Where(x => x.UsuarioId.Equals(User.FindFirst(ClaimTypes.NameIdentifier).Value));
            }
            bitacoraQuery = bitacoraQuery.Include(x => x.Usuario);

            if (!string.IsNullOrEmpty(request.FechaInicio)) {
                bitacoraQuery = bitacoraQuery.Where(e => e.CreatedAt >= StartOfDay(DateTime.Parse(request.FechaInicio)));
            }

            if (!string.IsNullOrEmpty(request.FechaFin)) {
                bitacoraQuery = bitacoraQuery.Where(e => e.CreatedAt <= EndOfDay(DateTime.Parse(request.FechaFin)));
            }

            if (!string.IsNullOrEmpty(request.UsuarioId)) {
                bitacoraQuery = bitacoraQuery.Where(e => e.UsuarioId.Equals(request.UsuarioId));
            }

            if (!string.IsNullOrEmpty(request.Accion)) {
                bitacoraQuery = bitacoraQuery.Where(e => e.Accion.Equals(request.Accion));
            }

            response.Total = bitacoraQuery.Count();
            var bitacoraList = new List<BitacoraList>();
            foreach (var bitacora in bitacoraQuery.OrderByDescending(x => x.CreatedAt).Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize).ToList()) {
                bitacoraList.Add(new BitacoraList() {
                    Usuario = bitacora.Usuario != null ? bitacora.Usuario.Name : "Sin usuario.",
                    Accion = bitacora.Accion,
                    Mensaje = bitacora.Mensaje,
                    CreatedAt = bitacora.CreatedAt 
                });
            }
            response.Bitacora = bitacoraList;
        
            return JsonConvert.SerializeObject(response);
        }

        private DateTime StartOfDay(DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
        }

        private DateTime EndOfDay(DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);
        }
    }
}