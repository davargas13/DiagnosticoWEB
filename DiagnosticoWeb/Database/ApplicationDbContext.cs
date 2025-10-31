using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DiagnosticoWeb.Models;

namespace DiagnosticoWeb.Database
{
    /// <summary>
    /// Clase que mapea las tablas de la base de datos en objetos de persistencia que se usaran en los controladores
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            Database.SetCommandTimeout(150000);
        }

        public DbSet<Archivo> Archivo { get; set; }
        public DbSet<Beneficiario> Beneficiario { get; set; }
        public DbSet<BeneficiarioHistorico> BeneficiarioHistorico { get; set; }
        public DbSet<Dependencia> Dependencia { get; set; }
        public DbSet<Domicilio> Domicilio { get; set; }
        public DbSet<DomicilioHistorico> DomicilioHistorico { get; set; }
        public DbSet<ProgramaSocial> ProgramaSocial { get; set; }
        public DbSet<Solicitud> Solicitud { get; set; }
        public DbSet<Trabajador> Trabajador { get; set; }
        public DbSet<TrabajadorDependencia> TrabajadorDependencia { get; set; }
        public DbSet<UsuarioDependencia> UsuarioDependencia { get; set; }
        public DbSet<Vertiente> Vertiente { get; set; }
        public DbSet<Configuracion> Configuracion { get; set; }
        public DbSet<Respuesta> Respuesta { get; set; }
        public DbSet<Encuesta> Encuesta { get; set; }
        public DbSet<EncuestaVersion> EncuestaVersion { get; set; }
        public DbSet<Pregunta> Pregunta { get; set; }
        public DbSet<Aplicacion> Aplicacion { get; set; }
        public DbSet<AplicacionPregunta> AplicacionPregunta { get; set; }
        public DbSet<AplicacionCarencia> AplicacionCarencia { get; set; }
        public DbSet<Estado> Estado { get; set; }
        public DbSet<Municipio> Municipio { get; set; }
        public DbSet<Localidad> Localidad { get; set; }
        public DbSet<Ageb> Ageb { get; set; }
        public DbSet<Manzana> Manzana { get; set; }
        public DbSet<Colonia> Colonia { get; set; }
        public DbSet<Calle> Calle { get; set; }
        public DbSet<Carretera> Carretera { get; set; }
        public DbSet<Camino> Camino { get; set; }
        public DbSet<ZonaImpulso> ZonaImpulso { get; set; }
        public DbSet<Parentesco> Parentesco { get; set; }
        public DbSet<Sexo> Sexo { get; set; }
        public DbSet<Estudio> Estudio { get; set; }
        public DbSet<Discapacidad> Discapacidad { get; set; }
        public DbSet<EstadoCivil> EstadoCivil { get; set; }
        public DbSet<Ocupacion> Ocupacion { get; set; }
        public DbSet<Zona> Zona { get; set; }
        public DbSet<MunicipioZona> MunicipioZona { get; set; }
        public DbSet<VertienteArchivo> VertienteArchivo { get; set; }
        public DbSet<TipoAsentamiento> TipoAsentamiento { get; set; }
        public DbSet<TipoIncidencia> TipoIncidencia { get; set; }
        public DbSet<Incidencia> Incidencias { get; set; }
        public DbSet<Carencia> Carencia { get; set; }
        public DbSet<VertienteCarencia> VertienteCarencia { get; set; }
        public DbSet<Importacion> Importacion { get; set; }
        public DbSet<PreguntaGrado> PreguntaGrado { get; set; }
        public DbSet<RespuestaGrado> RespuestaGrado { get; set; }
        public DbSet<UsuarioRegion> UsuarioRegion { get; set; }
        public DbSet<TrabajadorRegion> TrabajadorRegion { get; set; }
        public DbSet<CausaDiscapacidad> CausaDiscapacidad { get; set; }
        public DbSet<DiscapacidadGrado> DiscapacidadGrado { get; set; }
        public DbSet<GradosEstudio> GradosEstudio { get; set; }
        public DbSet<Grado> Grado { get; set; }
        public DbSet<Unidad> Unidad { get; set; }
        public DbSet<Bitacora> Bitacora { get; set; }
        public DbSet<Perfil> Perfil { get; set; }
        public DbSet<Permiso> Permiso { get; set; }
        public DbSet<PerfilPermiso> PerfilPermiso { get; set; }
        public DbSet<BeneficiarioDiscapacidad> BeneficiarioDiscapacidades { get; set; }
        public DbSet<AlgoritmoVersion> AlgoritmoVersion { get; set; }
        public DbSet<AlgoritmoResultado> AlgoritmoResultado { get; set; }
        public DbSet<AlgoritmoIntegranteResultado> AlgoritmoIntegranteResultado { get; set; }
        public DbSet<LineaBienestar> LineaBienestar { get; set; }
    }
}