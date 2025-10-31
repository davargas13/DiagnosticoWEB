/*using System;
using System.Net;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace DiagnosticoWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {            
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel(
                    options =>
                    {
                        // options.Listen(IPAddress.Any, 5000);
                        options.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(30);
                    })
                .UseStartup<Startup>();

    }
}*/

using System;
using System.Net;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace DiagnosticoWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel(options =>
                {
                    // Mantener la conexión viva hasta 30 minutos
                    options.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(30);

                    // Tiempo máximo para leer los headers de la petición
                    options.Limits.RequestHeadersTimeout = TimeSpan.FromMinutes(10);

                    // Si quieres, puedes descomentar para escuchar en un puerto específico
                    // options.Listen(IPAddress.Any, 5000);
                })
                .UseStartup<Startup>();
    }
}

