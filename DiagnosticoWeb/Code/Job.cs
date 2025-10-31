using System;
using System.Net.Http;
using DiagnosticoWeb.Controllers;
using DiagnosticoWeb.Database;
using Microsoft.Extensions.Configuration;
using moment.net;
using moment.net.Enums;

namespace DiagnosticoWeb.Code
{
    public class Job
    {
        
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _clientFactory;

        public Job(ApplicationDbContext context, IConfiguration configuration, IHttpClientFactory clientFactory)
        {
            _context = context;
            _configuration = configuration;
            _clientFactory = clientFactory;
        }
        
        public void CompletarDatos()
        {
            /*DESACTIVAMOS
            var inicio = DateTime.Now.AddDays(-1).StartOf(DateTimeAnchor.Day);
            var fin = DateTime.Now;
            var importacion = new ImportacionDiagnosticos(null, _context, _configuration, _clientFactory);
            importacion.CompletarDirecciones(inicio, fin);
            importacion.CompletarCurps(inicio, fin);
            var home = new HomeController(_context, null, null, null, null, null);
            home.IngresarAplicacionEnSabana(inicio.ToString("yyyy-MM-dd"), fin.ToString("yyyy-MM-dd"));*/
        }
    }
}