using Microsoft.EntityFrameworkCore;
using DiagnosticoWeb.Models.DiagnosticoAnterior;

namespace DiagnosticoWeb.Database
{
    public class DiagnosticoAnteriorDbContext : DbContext
    {
        public DiagnosticoAnteriorDbContext(DbContextOptions<DiagnosticoAnteriorDbContext> options)
            : base(options)
        { }
        
        public DbSet<tblDatosApp> tblDatosApp { get; set; }
        // public DbSet<tblEncuestaSabana> Encuestas { get; set; }
    }
}