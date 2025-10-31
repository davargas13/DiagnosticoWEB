using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using DiagnosticoWeb.Database;
using DiagnosticoWeb.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DiagnosticoWeb.Claims
{
    public class ClaimPrincipal : UserClaimsPrincipalFactory<ApplicationUser, IdentityRole>
    {
        public IConfiguration Configuration;
        private readonly ApplicationDbContext _context;

        public ClaimPrincipal(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, 
                    IOptions<IdentityOptions> options) : base(userManager, roleManager, options)
        {
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            Configuration = configurationBuilder.Build();

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            _context = new ApplicationDbContext(optionsBuilder.Options);
        }

        public async override Task<ClaimsPrincipal> CreateAsync(ApplicationUser user)
        {
            var principal = await base.CreateAsync(user);
            var permisos = _context.PerfilPermiso.Where(x => x.PerfilId.Equals(user.PerfilId) && x.DeletedAt == null)
                .Join(_context.Permiso, perPer => perPer.PermisoId, perm => perm.Id,
                    (perPer, perm) => new { PerfilPermiso = perPer, Permiso = perm }).Select(x => x.Permiso).ToList();

            foreach (var permiso in permisos) {
                ((ClaimsIdentity) principal.Identity).AddClaim(new Claim("Permiso", permiso.Clave));
            }

            return principal;
        }
    }
}
