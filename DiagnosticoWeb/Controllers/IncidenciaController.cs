using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using DiagnosticoWeb.Code;
using DiagnosticoWeb.Database;
using DiagnosticoWeb.Models;
using DiagnosticoWeb.Validaciones;
using Model = DiagnosticoWeb.Models.Model;

namespace DiagnosticoWeb.Controllers
{
    public class IncidenciaController : Controller
    { 
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public IncidenciaController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        
        [HttpGet]
        [Authorize]
        [RequireClaim("Permiso", Value = "incidencias.ver")]
        public IActionResult Index()
        {
            var response = new IncidenciasResponse();
            var date = DateTime.Now;
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            response.Inicio = firstDayOfMonth.ToString("yyyy-MM-dd");
            response.Fin = lastDayOfMonth.ToString("yyyy-MM-dd");
            response.TiposIncidencias = new List<Model>();
            response.Trabajadores = new List<Model>();
            response.Municipios = new List<Model>();
            foreach (var tipoIncidencia in _context.TipoIncidencia.Where(ti=>ti.DeletedAt==null))
            {
                response.TiposIncidencias.Add(new Model
                {
                    Id = tipoIncidencia.Id,
                    Nombre = tipoIncidencia.Nombre
                });
            }
            foreach (var municipio in _context.Municipio.Where(ti=>ti.DeletedAt==null))
            {
                response.Municipios.Add(new Model
                {
                    Id = municipio.Id,
                    Nombre = municipio.Nombre
                });
            }
            foreach (var trabajador in _context.Trabajador.Where(ti=>ti.DeletedAt==null).OrderBy(t=>t.Nombre))
            {
                response.Trabajadores.Add(new Model
                {
                    Id = trabajador.Id,
                    Nombre = trabajador.Nombre
                });
            }
            return View("Index", response);
        }
        
        [HttpPost]
        [Authorize]
        [RequireClaim("Permiso", Value = "incidencias.ver")]
        public string GetIncidencias([FromBody] IncidenciasRequest request)
        {
            var response = new IncidenciaResponse
            {
                Incidencias = new List<IncidenciaShortResponse>()
            };
            var query = BuildQuery(request, false);
            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = query.Key;
                if (request.PageSize > 0)
                {
                    if (request.PageSize > 0)
                    {
                        command.CommandText += " OFFSET " + ((request.PageIndex - 1) * request.PageSize) +
                                               " ROWS FETCH NEXT " + request.PageSize + " ROWS ONLY";
                    }
                }
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
                            var incidencia = new IncidenciaShortResponse
                            {
                                Id = reader.GetString(0),
                                Observaciones = reader.GetString(1),
                                CreatedAt = reader.GetDateTime(2).ToString(),
                                TipoIncidencia = reader.GetString(3),
                                Folio = reader.GetString(7),
                                Trabajador = reader.GetString(8),
                                Municipio = reader.GetString(4),
                                Localidad = reader.GetString(5),
                                ZonaImpulso = reader.IsDBNull(6) ? "": reader.GetString(6),
                            };
                            response.Incidencias.Add(incidencia);
                        }
                    }
                }
            }
            query = BuildQuery(request, true);

            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = query.Key;
                var iParametro = 0;
                foreach (var parametro in query.Value)
                {
                    command.Parameters.Add(new SqlParameter("@" + (iParametro++), parametro));
                }
                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            response.Total = reader.GetInt32(0);
                        }
                    }
                }
            }
            _context.Database.CloseConnection();
            return JsonSedeshu.SerializeObject(response);
        }
        
        

        [HttpPost]
        [Authorize]
        public IActionResult ExportarIncidencias(IncidenciasRequest request)
        {
            using (var excel = new ExcelPackage())
            {
                excel.Workbook.Worksheets.Add("incidencias");
                var pestana = excel.Workbook.Worksheets.First();
                var fila = 1;
                pestana.Cells[fila, 1].Value = "Folio";
                pestana.Cells[fila, 2].Value = "Fecha de registro";
                pestana.Cells[fila, 3].Value = "Tipo";
                pestana.Cells[fila, 4].Value = "Encuestador";
                pestana.Cells[fila, 5].Value = "Municipio";
                pestana.Cells[fila, 6].Value = "Localidad";
                pestana.Cells[fila, 7].Value = "Zona de impulso";
                pestana.Cells[fila++, 8].Value = "Observaciones";

                var query = BuildQuery(request, false);
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
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
                                pestana.Cells[fila, 1].Value = reader.GetString(7);
                                pestana.Cells[fila, 2].Value = reader.GetDateTime(2).ToString();
                                pestana.Cells[fila, 3].Value = reader.GetString(3);
                                pestana.Cells[fila, 4].Value = reader.GetString(8);
                                pestana.Cells[fila, 5].Value = reader.GetString(4);
                                pestana.Cells[fila, 6].Value = reader.GetString(5);
                                pestana.Cells[fila, 7].Value = reader.IsDBNull(6) ? "": reader.GetString(6);
                                pestana.Cells[fila++, 8].Value = reader.GetString(1);
                            }
                        }
                    }
                }
                
                _context.Bitacora.Add(new Bitacora()
                {
                    UsuarioId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                    Accion = AccionBitacora.EXPORTACION,
                    Mensaje = "Se exportó el listado de incidencias.",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                });
                _context.SaveChanges();

                return File(
                    fileContents: excel.GetAsByteArray(),
                    contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    fileDownloadName: "incidencias.xlsx"
                );
            }
        }

        private KeyValuePair<string, List<string>> BuildQuery(IncidenciasRequest request, bool total)
        {
            var now = DateTime.Now;
            var inicio = new DateTime(now.Year, now.Month, 1).ToString("yyyy-MM-dd");
            var fin = new DateTime(now.Year, now.Month, DateTime.DaysInMonth(now.Year, now.Month)).ToString("yyyy-MM-dd");
            if (!string.IsNullOrEmpty(request.Inicio))
            {
                inicio = request.Inicio.Substring(0, 10);
            }

            if (!string.IsNullOrEmpty(request.Fin))
            {
                fin = request.Fin.Substring(0, 10);
            }
            var select = "select count(I.id)";
            if (!total) {
                select = "select I.Id, I.Observaciones, I.CreatedAt, TI.Nombre, M.Nombre, L.Nombre, ZI.Nombre, B.Folio, T.Nombre ";
            }
            var query = " from Incidencias I inner join Trabajadores T on T.Id = I.TrabajadorId "+
                        " inner join Beneficiarios B on B.id = I.BeneficiarioId "+
                        " inner join TipoIncidencias TI on TI.Id = I.TipoIncidenciaId inner join Domicilio D on D.Id = B.DomicilioId "+
                        " inner join Municipios M On M.Id=D.MunicipioId inner join Localidades L on L.Id = D.LocalidadId "+
                        " Left join ZonasImpulso ZI on ZI.Id = D.ZonaImpulsoId ";
            var where = " where I.CreatedAt >= @0 and I.CreatedAt <= @1 and I.DeletedAt is null";
            var iParametro = 2;
            var parametros = new List<string>();
            parametros.Add(inicio);
            parametros.Add(fin);
            if (!string.IsNullOrEmpty(request.Folio))
            {
                parametros.Add(request.Folio);
                where += " and B.Folio=@"+(iParametro++)+" ";
            }
            if (!string.IsNullOrEmpty(request.TrabajadorId))
            {
                parametros.Add(request.TrabajadorId);
                where += " and I.TrabajadorId=@"+(iParametro++)+" ";
            }
            if (!string.IsNullOrEmpty(request.TipoIncidenciaId))
            {
                parametros.Add(request.TipoIncidenciaId);
                where += " and I.TipoIncidenciaId=@"+(iParametro++)+" ";
            }
            if (!string.IsNullOrEmpty(request.MunicipioId))
            {
                parametros.Add(request.MunicipioId);
                where += " and D.MunicipioId=@"+(iParametro++)+" ";
            }
            if (!string.IsNullOrEmpty(request.LocalidadId))
            {
                parametros.Add(request.LocalidadId);
                where += " and D.LocalidadId=@"+(iParametro++)+" ";
            }
            if (!string.IsNullOrEmpty(request.ZonaImpulsoId))
            {
                parametros.Add(request.ZonaImpulsoId);
                where += " and D.ZonaImpulsoId=@"+(iParametro++)+" ";
            }
            if (!total) { 
                where += " ORDER BY I.CreatedAt Desc";
            }

            var response = new KeyValuePair<string, List<string>>(select+query+where, parametros);
            return  response;
        }
    }
}