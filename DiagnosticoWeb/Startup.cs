using System;
using System.Net;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DiagnosticoWeb.Claims;
using DiagnosticoWeb.Code;
using DiagnosticoWeb.Database;
//using Hangfire;
//using Hangfire.SqlServer;
using DiagnosticoWeb.Models;
using DiagnosticoWeb.Validaciones;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace DiagnosticoWeb
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var url = Configuration["url"];
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
                this.Configuration.GetConnectionString("DefaultConnection"),
             sqlServerOptions => sqlServerOptions.CommandTimeout(500))
            );
            services.AddDbContext<DiagnosticoAnteriorDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DiagnosticoAnteriorConnection")));

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1).AddJsonOptions(
                options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );

            services.AddRequireClaimAttributeAuthorization();
            services.ConfigureApplicationCookie(options => options.LoginPath = url + "/Home/Index");
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Catalogos", policy => policy.RequireRole("Administrador"));
                options.AddPolicy("Encuesta", policy => policy.RequireRole("Administrador"));
                options.AddPolicy("Trabajadores", policy => policy.RequireRole("Administrador", "Administrador de dependencia"));
                options.AddPolicy("EditarBeneficiario", policy => policy.RequireRole("Administrador", "Administrador de dependencia"));
            });
            services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, ClaimPrincipal>();
            services.Configure<FormOptions>(x =>
            {
                x.ValueLengthLimit = int.MaxValue;
                x.MultipartBodyLengthLimit = int.MaxValue; // In case of multipart
            });

            /*services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(Configuration.GetConnectionString("HangfireConnection"), new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    DisableGlobalLocks = true
                }));*/

            // Add the processing server as IHostedService
            //services.AddHangfireServer();
            services.AddHttpClient();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            var url = Configuration["url"];
            if (!env.IsDevelopment())
            {
                app.UseHsts();
            }
            app.UseExceptionHandler("/Error");
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.Use(async (context, next) =>
            {
                context.Request.PathBase = url;
                await next.Invoke();
            });
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.All
            });
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            //app.UseHangfireServer (); // Iniciar el servicio hangfire
            Rotativa.AspNetCore.RotativaConfiguration.Setup(env);
            //DESACTIVAMOS JOBS
            //RecurringJob.AddOrUpdate<Job>("correccion_datos",x=>x.CompletarDatos(), Cron.Daily(6));

        }
    }
}
