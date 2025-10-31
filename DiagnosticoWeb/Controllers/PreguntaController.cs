using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using DiagnosticoWeb.Code;
using DiagnosticoWeb.Database;
using DiagnosticoWeb.Models;
using DiagnosticoWeb.Validaciones;

namespace DiagnosticoWeb.Controllers
{
    /// <summary>
    /// Clase con la logica de negocio para la visualizacion, adicion o edicion de los programas sociales
    /// </summary>
    public class PreguntaController : Controller
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="context">Conexion a la base de datos</param>
        public PreguntaController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Funcion que muestra la vista con el listado de programas sociales 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public string GetRespuestas(string id)
        {
            switch (id)
            {
                case "TipoAsentamientos":
                    var tipos = _context.TipoAsentamiento.ToList();
                    return JsonSedeshu.SerializeObject(tipos);
                case "Sexos":
                    var sexos = _context.Sexo.ToList();
                    return JsonSedeshu.SerializeObject(sexos);
                case "Parentescos":
                    var parentescos = _context.Parentesco.ToList();
                    return JsonSedeshu.SerializeObject(parentescos);
                case "Ocupaciones":
                    var ocupaciones = _context.Ocupacion.ToList();
                    return JsonSedeshu.SerializeObject(ocupaciones);
                case "Municipios" :
                    var municipios = _context.Municipio.ToList();
                    return JsonSedeshu.SerializeObject(municipios);
                default:
                    return "";
            }
        }
    }
}