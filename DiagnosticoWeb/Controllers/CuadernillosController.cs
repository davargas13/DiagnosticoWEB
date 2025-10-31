using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using DiagnosticoWeb.Code;
using DiagnosticoWeb.Database;
using DiagnosticoWeb.Models;
using Rotativa.AspNetCore;

namespace DiagnosticoWeb.Controllers
{
    public class CuadernillosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;

        public CuadernillosController(ApplicationDbContext context, IConfiguration configuration,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _configuration = configuration;
            _userManager = userManager;
        }

        // GET
        public IActionResult Index()
        {
            var response = new ReponseBeneficiarioIndex();
            var date = DateTime.Now;
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            response.Inicio = firstDayOfMonth.ToString("yyyy-MM-dd");
            response.Fin = lastDayOfMonth.ToString("yyyy-MM-dd");
            return View("Index", response);
        }

        [HttpPost]
        public IActionResult Cuadernillo(BeneficiariosRequest request)
        {
            return View("Cuadernillo", BuildCuadernillo(request));
        }

        [HttpPost]
        public string Buscar([FromBody] BeneficiariosRequest request)
        {
            return JsonSedeshu.SerializeObject(BuildCuadernillo(request));
        }

        [HttpPost]
        public IActionResult Pdf(BeneficiariosRequest request)
        {
            return new ViewAsPdf("~/Views/Cuadernillos/Cuadernillo.cshtml", BuildCuadernillo(request));
        }

        public Cuadernillo BuildCuadernillo(BeneficiariosRequest request)
        {
            var aplicacionId = "";
            var response = new Cuadernillo
            {
                Alimentarias = new Dictionary<string, int> {["Sin carencia"] = 0, ["Con carencia"] = 0},
                Educativas = new Dictionary<string, int> {["Sin carencia"] = 0, ["Con carencia"] = 0},
                Salud = new Dictionary<string, int> {["Sin carencia"] = 0, ["Con carencia"] = 0},
                SeguroSocial = new Dictionary<string, int> {["Sin carencia"] = 0, ["Con carencia"] = 0},
                Vivienda = new Dictionary<string, int> {["Sin carencia"] = 0, ["Con carencia"] = 0},
                Servicios = new Dictionary<string, int> {["Sin carencia"] = 0, ["Con carencia"] = 0},
                Tejido = new Dictionary<string, int> {["Bajo"] = 0, ["Medio"] = 0, ["Alto"] = 0},
                Hacinamiento = new Dictionary<string, int> {["Sin carencia"] = 0, ["Con carencia"] = 0},
                Pobreza = new Dictionary<string, int>
                {
                    [NivelPobreza.POBREZA_EXTREMA] = 0, [NivelPobreza.POBREZA_MODERADA] = 0
                },
                Edades = new Dictionary<string, Dictionary<string, int>>
                {
                    [Edades.MAYORES] = new Dictionary<string, int>{["1"]=0, ["2"]=0},
                    [Edades.ADULTOS] = new Dictionary<string, int>{["1"]=0, ["2"]=0} , 
                    [Edades.MADUROS] = new Dictionary<string, int>{["1"]=0, ["2"]=0} , 
                    [Edades.JOVENES] = new Dictionary<string, int>{["1"]=0, ["2"]=0} , 
                    [Edades.MENORES] = new Dictionary<string, int>{["1"]=0, ["2"]=0} 
                },
                Analfabetismo = new Dictionary<string, int> {["Sin carencia"] = 0, ["Con carencia"] = 0},
                GradoEducativa = new Dictionary<string, int> {["3 a 5 años"] = 0, ["Mayor 16 años antes de 1982"] = 0,["Mayor 16 años despues de 1982"] = 0},
                NivelEducativa = new Dictionary<string, int> {["Primaria incompleta"] = 0, ["Secundaria incompleta"] = 0},
                Discapacidad = new Dictionary<string, int> {["Con discapacidad"] = 0, ["Sin discapacidad"] = 0},
                GradoAlimentarias = new Dictionary<string, int>
                {
                    [GradoAlimentaria.SEGURIDAD] = 0, [GradoAlimentaria.LEVE] = 0,[GradoAlimentaria.MODERADA] = 0,[GradoAlimentaria.SEVERA] = 0
                },
                Piso = new Dictionary<string, int> {["Sin carencia"] = 0, ["Con carencia"] = 0},
                Techo = new Dictionary<string, int> {["Sin carencia"] = 0, ["Con carencia"] = 0},
                Muro = new Dictionary<string, int> {["Sin carencia"] = 0, ["Con carencia"] = 0},
                Agua = new Dictionary<string, int> {["Sin carencia"] = 0, ["Con carencia"] = 0},
                Drenaje = new Dictionary<string, int> {["Sin carencia"] = 0, ["Con carencia"] = 0},
                Electricidad = new Dictionary<string, int> {["Sin carencia"] = 0, ["Con carencia"] = 0},
                Combustible = new Dictionary<string, int> {["Sin carencia"] = 0, ["Con carencia"] = 0},
                Propiedad = new Dictionary<string, int> {["Sin carencia"] = 0, ["Con carencia"] = 0},
                Acta = new Dictionary<string, int> {["Sin carencia"] = 0, ["Con carencia"] = 0},
                Satisfaccion = new Dictionary<string, int> {["Insatisfechos"] = 0, ["Neutrales"] = 0,["Satisfechos"]=0},
                Movilidad = new Dictionary<string, int> {["Insatisfechos"] = 0, ["Neutrales"] = 0,["Satisfechos"]=0},
                Redes = new Dictionary<string, int> {["Insatisfechos"] = 0, ["Neutrales"] = 0,["Satisfechos"]=0},
                Instituciones = new Dictionary<string, int> {["Insatisfechos"] = 0, ["Neutrales"] = 0,["Satisfechos"]=0},
                Lideres = new Dictionary<string, int> {["Insatisfechos"] = 0, ["Neutrales"] = 0,["Satisfechos"]=0},
                Publicos = new Dictionary<string, int> {["Insatisfechos"] = 0, ["Neutrales"] = 0,["Satisfechos"]=0},
                Parque = new Dictionary<string, int> {["Insatisfechos"] = 0, ["Neutrales"] = 0,["Satisfechos"]=0}
            };
            var now = DateTime.Now;
            var inicio = new DateTime(now.Year, now.Month, 1).ToString("yyyy-MM-dd");
            var fin =
                new DateTime(now.Year, now.Month, DateTime.DaysInMonth(now.Year, now.Month)).ToString("yyyy-MM-dd");
            if (!string.IsNullOrEmpty(request.Inicio))
            {
                inicio = request.Inicio.Substring(0, 10);
            }

            if (!string.IsNullOrEmpty(request.Fin))
            {
                fin = request.Fin.Substring(0, 10);
            }

            var util = new BeneficiarioCode(_context);
            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                var query = util.BuildBeneficiariosQuery(request, TipoConsulta.Cuadernillo,
                    false, inicio, fin, false);
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
                            var id = reader.GetString(1); //Indicadores por integrante
                            var edad = reader.GetInt32(11);
                            var sexo = (reader.GetString(14));
                            response.Educativas[reader.GetBoolean(3) ? "Con carencia" : "Sin carencia"]++;
                            response.Salud[reader.GetBoolean(4) ? "Con carencia" : "Sin carencia"]++;
                            response.SeguroSocial[reader.GetBoolean(5) ? "Con carencia" : "Sin carencia"]++;
                            if (reader.GetBoolean(12))
                            {
                                response.HacinamientoPersonas++;
                            }
                            if (reader.GetBoolean(13))
                            {
                                response.AlimentariaPersonas++;
                            }
                            
                            if (edad >= 0 && edad < 15)
                            {
                                response.Edades[Edades.MENORES][sexo]++;
                            }
                            else
                            {
                                if (edad >= 15 && edad < 30)
                                {
                                    response.Edades[Edades.JOVENES][sexo]++;
                                }
                                else
                                {
                                    if (edad >= 30 && edad < 45)
                                    {
                                        response.Edades[Edades.ADULTOS][sexo]++;
                                    }
                                    else
                                    {
                                        if (edad >= 45 && edad < 65)
                                        {
                                            response.Edades[Edades.MADUROS][sexo]++;
                                        }
                                        else
                                        {
                                            response.Edades[Edades.MAYORES][sexo]++;
                                        }
                                    }
                                }
                            }
                            
                            response.Analfabetismo[reader.GetBoolean(15) ? "Con carencia" : "Sin carencia"]++;
                            switch (reader.GetInt16(16))
                            {
                                case 1:
                                    response.GradoEducativa["3 a 5 años"]++;
                                    break;
                                case 2:
                                    response.GradoEducativa["Mayor 16 años antes de 1982"]++;
                                    break;
                                case 3:
                                    response.GradoEducativa["Mayor 16 años despues de 1982"]++;
                                    break;
                            }

                            if (reader.GetBoolean(17))
                            {
                                response.NivelEducativa["Primaria incompleta"]++;
                            }
                            if (reader.GetBoolean(18))
                            {
                                response.NivelEducativa["Secundaria incompleta"]++;
                            }
                            response.Discapacidad[reader.GetBoolean(19) ? "Con discapacidad" : "Sin discapacidad"]++;
                            response.Acta[reader.GetBoolean(29) ? "Con carencia" : "Sin carencia"]++;
                            response.Personas++;
                            
                            if (aplicacionId.Equals(id)) continue; //Indicadores del hogar
                            response.Vivienda[reader.GetBoolean(6) ? "Con carencia" : "Sin carencia"]++;
                            response.Alimentarias[reader.GetBoolean(2) ? "Con carencia" : "Sin carencia"]++;
                            response.Servicios[reader.GetBoolean(7) ? "Con carencia" : "Sin carencia"]++;
                            response.Hacinamiento[reader.GetBoolean(10) ? "Con carencia" : "Sin carencia"]++;
                            switch (reader.GetInt16(8))
                            {
                                case 1:
                                    response.Tejido["Bajo"]++;
                                    break;
                                case 2:
                                    response.Tejido["Medio"]++;
                                    break;
                                case 3:
                                    response.Tejido["Alto"]++;
                                    break;
                            }

                            if (!reader.IsDBNull(9))
                            {
                                switch (reader.GetString(9))
                                {
                                    case NivelPobreza.POBREZA_EXTREMA:
                                        response.Pobreza[NivelPobreza.POBREZA_EXTREMA]++;
                                        break;
                                    case NivelPobreza.POBREZA_MODERADA:
                                        response.Pobreza[NivelPobreza.POBREZA_MODERADA]++;
                                        break;
                                }
                            }

                            if (!reader.IsDBNull(20))
                            {
                                response.GradoAlimentarias[reader.GetString(20)]++;
                            }
                            response.Piso[reader.GetBoolean(21) ? "Con carencia" : "Sin carencia"]++;
                            response.Techo[reader.GetBoolean(22) ? "Con carencia" : "Sin carencia"]++;
                            response.Muro[reader.GetBoolean(23) ? "Con carencia" : "Sin carencia"]++;
                            response.Agua[reader.GetBoolean(24) ? "Con carencia" : "Sin carencia"]++;
                            response.Drenaje[reader.GetBoolean(25) ? "Con carencia" : "Sin carencia"]++;
                            response.Electricidad[reader.GetBoolean(26) ? "Con carencia" : "Sin carencia"]++;
                            response.Combustible[reader.GetBoolean(27) ? "Con carencia" : "Sin carencia"]++;
                            response.Propiedad[reader.GetBoolean(28) ? "Con carencia" : "Sin carencia"]++;

                            BuildSatisfaccion(reader.GetInt16(30), response.Satisfaccion);
                            BuildSatisfaccion(reader.GetInt16(31), response.Movilidad);
                            BuildSatisfaccion(reader.GetInt16(32), response.Redes);
                            BuildSatisfaccion(reader.GetInt16(33), response.Instituciones);
                            BuildSatisfaccion(reader.GetInt16(34), response.Lideres);
                            BuildSatisfaccion(reader.GetInt16(35), response.Publicos);
                            BuildSatisfaccion(reader.GetInt16(36), response.Parque);

                            response.Viviendas++;
                            aplicacionId = id;
                        }
                    }
                }
            }
            _context.Database.CloseConnection();
            response.Inicio = request.Inicio;
            response.Fin = request.Fin;            
            response.Ruta = _configuration["SITE_URL"];
            return response;
        }

        private void BuildSatisfaccion(Int16 valor, Dictionary<string, int> datos)
        {
            switch (valor)
            {
                case 1:
                    datos["Insatisfechos"]++;
                    break;
                case 2:
                    datos["Neutrales"]++;
                    break;
                case 3:
                    datos["Satisfechos"]++;
                    break;
            }
        }
    }
}