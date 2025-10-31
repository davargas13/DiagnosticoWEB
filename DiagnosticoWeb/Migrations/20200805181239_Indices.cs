using Microsoft.EntityFrameworkCore.Migrations;

namespace DiagnosticoWeb.Migrations
{
    public partial class Indices : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Agebs_LocalidadId",
                table: "Agebs",
                column: "LocalidadId");
            migrationBuilder.CreateIndex(
                name: "IX_Agebs_MunicipioId",
                table: "Agebs",
                column: "MunicipioId");
            migrationBuilder.CreateIndex(
                name: "IX_Agebs_EstadoId",
                table: "Agebs",
                column: "EstadoId");
            
            migrationBuilder.CreateIndex(
                name: "IX_Aplicaciones_BeneficiarioId",
                table: "Aplicaciones",
                column: "BeneficiarioId");
            migrationBuilder.CreateIndex(
                name: "IX_Aplicaciones_EncuestaVersionId",
                table: "Aplicaciones",
                column: "EncuestaVersionId");
            migrationBuilder.CreateIndex(
                name: "IX_Aplicaciones_TrabajadorId",
                table: "Aplicaciones",
                column: "TrabajadorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Respuestas_AplicacionPreguntas",
                table: "AplicacionPreguntas",
                column: "RespuestaId",
                principalTable: "Respuestas",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
            migrationBuilder.CreateIndex(
                name: "IX_AplicacionPreguntas_AplicacionId",
                table: "AplicacionPreguntas",
                column: "AplicacionId");   
            migrationBuilder.CreateIndex(
                name: "IX_AplicacionPreguntas_PreguntaId",
                table: "AplicacionPreguntas",
                column: "PreguntaId");
            migrationBuilder.CreateIndex(
                name: "IX_AplicacionPreguntas_RespuestaId",
                table: "AplicacionPreguntas",
                column: "RespuestaId");

            migrationBuilder.CreateIndex(
                name: "IX_BeneficiarioHistorico_BeneficiarioId",
                table: "BeneficiarioHistorico",
                column: "BeneficiarioId");
            migrationBuilder.CreateIndex(
                name: "IX_BeneficiarioHistorico_EstudioId",
                table: "BeneficiarioHistorico",
                column: "EstudioId");
            migrationBuilder.CreateIndex(
                name: "IX_BeneficiarioHistorico_EstadoId",
                table: "BeneficiarioHistorico",
                column: "EstadoId");
            migrationBuilder.CreateIndex(
                name: "IX_BeneficiarioHistorico_EstadoCivilId",
                table: "BeneficiarioHistorico",
                column: "EstadoCivilId");
            migrationBuilder.CreateIndex(
                name: "IX_BeneficiarioHistorico_DiscapacidadId",
                table: "BeneficiarioHistorico",
                column: "DiscapacidadId");
            migrationBuilder.CreateIndex(
                name: "IX_BeneficiarioHistorico_SexoId",
                table: "BeneficiarioHistorico",
                column: "SexoId");
            migrationBuilder.CreateIndex(
                name: "IX_BeneficiarioHistorico_TrabajadorId",
                table: "BeneficiarioHistorico",
                column: "TrabajadorId");
            migrationBuilder.CreateIndex(
                name: "IX_BeneficiarioHistorico_GradoEstudioId",
                table: "BeneficiarioHistorico",
                column: "GradoEstudioId");
            migrationBuilder.CreateIndex(
                name: "IX_BeneficiarioHistorico_CausaDiscapacidadId",
                table: "BeneficiarioHistorico",
                column: "CausaDiscapacidadId");
            migrationBuilder.CreateIndex(
                name: "IX_BeneficiarioHistorico_DiscapacidadGradoId",
                table: "BeneficiarioHistorico",
                column: "DiscapacidadGradoId");
            
            migrationBuilder.CreateIndex(
                name: "IX_Beneficiarios_EstadoId",
                table: "Beneficiarios",
                column: "EstadoId");
            migrationBuilder.CreateIndex(
                name: "IX_Beneficiarios_EstudioId",
                table: "Beneficiarios",
                column: "EstudioId");
            migrationBuilder.CreateIndex(
                name: "IX_Beneficiarios_EstadoCivilId",
                table: "Beneficiarios",
                column: "EstadoCivilId");
            migrationBuilder.CreateIndex(
                name: "IX_Beneficiarios_DiscapacidadId",
                table: "Beneficiarios",
                column: "DiscapacidadId");
            migrationBuilder.CreateIndex(
                name: "IX_Beneficiarios_SexoId",
                table: "Beneficiarios",
                column: "SexoId");
            migrationBuilder.CreateIndex(
                name: "IX_Beneficiarios_TrabajadorId",
                table: "Beneficiarios",
                column: "TrabajadorId");
            migrationBuilder.CreateIndex(
                name: "IX_Beneficiarios_PadreId",
                table: "Beneficiarios",
                column: "PadreId"); 
            migrationBuilder.CreateIndex(
                name: "IX_Beneficiarios_ParentescoId",
                table: "Beneficiarios",
                column: "ParentescoId"); 
            migrationBuilder.CreateIndex(
                name: "IX_Beneficiarios_GradoEstudioId",
                table: "Beneficiarios",
                column: "GradoEstudioId");
            migrationBuilder.CreateIndex(
                name: "IX_Beneficiarios_CausaDiscapacidadId",
                table: "Beneficiarios",
                column: "CausaDiscapacidadId");
            migrationBuilder.CreateIndex(
                name: "IX_Beneficiarios_DiscapacidadGradoId",
                table: "Beneficiarios",
                column: "DiscapacidadGradoId");
            migrationBuilder.CreateIndex(
                name: "IX_Beneficiarios_HijoId",
                table: "Beneficiarios",
                column: "HijoId");
            
            migrationBuilder.CreateIndex(
                name: "IX_Bitacora_UsuarioId",
                table: "Bitacora",
                column: "UsuarioId");
            
            migrationBuilder.CreateIndex(
                name: "IX_DiscapacidadGrado_DiscapacidadId",
                table: "DiscapacidadGrado",
                column: "DiscapacidadId");
            migrationBuilder.CreateIndex(
                name: "IX_DiscapacidadGrado_GradoId",
                table: "DiscapacidadGrado",
                column: "GradoId");
            
            migrationBuilder.CreateIndex(
                name: "IX_Domicilio_AgebId",
                table: "Domicilio",
                column: "AgebId");  
            migrationBuilder.CreateIndex(
                name: "IX_Domicilio_BeneficiarioId",
                table: "Domicilio",
                column: "BeneficiarioId");
            migrationBuilder.CreateIndex(
                name: "IX_Domicilio_MunicipioId",
                table: "Domicilio",
                column: "MunicipioId");
            migrationBuilder.CreateIndex(
                name: "IX_Domicilio_LocalidadId",
                table: "Domicilio",
                column: "LocalidadId");
            migrationBuilder.CreateIndex(
                name: "IX_Domicilio_TrabajadorId",
                table: "Domicilio",
                column: "TrabajadorId");
            migrationBuilder.CreateIndex(
                name: "IX_Domicilio_ManzanaId",
                table: "Domicilio",
                column: "ManzanaId");
            migrationBuilder.CreateIndex(
                name: "IX_Domicilio_CarreteraId",
                table: "Domicilio",
                column: "CarreteraId");
            migrationBuilder.CreateIndex(
                name: "IX_Domicilio_CaminoId",
                table: "Domicilio",
                column: "CaminoId");
            migrationBuilder.CreateIndex(
                name: "IX_Domicilio_TipoAsentamientoId",
                table: "Domicilio",
                column: "TipoAsentamientoId");
            
            migrationBuilder.CreateIndex(
                name: "IX_EncuestaVersiones_EncuestaId",
                table: "EncuestaVersiones",
                column: "EncuestaId");
            
            migrationBuilder.CreateIndex(
                name: "IX_Importaciones_EncuestaId",
                table: "Importaciones",
                column: "UsuarioId"); 
           
            migrationBuilder.CreateIndex(
                name: "IX_Localidades_ZonaIndigenaId",
                table: "Localidades",
                column: "ZonaIndigenaId");
            migrationBuilder.CreateIndex(
                name: "IX_Localidades_MunicipioId",
                table: "Localidades",
                column: "MunicipioId");
            migrationBuilder.CreateIndex(
                name: "IX_Localidades_EstadoId",
                table: "Localidades",
                column: "EstadoId");
            
            migrationBuilder.CreateIndex(
                name: "IX_Manzanas_LocalidadId",
                table: "Manzanas",
                column: "LocalidadId");
            migrationBuilder.CreateIndex(
                name: "IX_Manzanas_MunicipioId",
                table: "Manzanas",
                column: "MunicipioId");
            migrationBuilder.CreateIndex(
                name: "IX_Manzanas_AgebId",
                table: "Manzanas",
                column: "AgebId");
            
            migrationBuilder.CreateIndex(
                name: "IX_Municipios_EstadoId",
                table: "Municipios",
                column: "EstadoId");
            
            migrationBuilder.CreateIndex(
                name: "IX_MunicipiosZonas_ZonaId",
                table: "MunicipiosZonas",
                column: "ZonaId");
            migrationBuilder.CreateIndex(
                name: "IX_MunicipiosZonas_MunicipioId",
                table: "MunicipiosZonas",
                column: "MunicipioId");
            
            migrationBuilder.CreateIndex(
                name: "IX_PerfilPermiso_PerfilId",
                table: "PerfilPermiso",
                column: "PerfilId");
            migrationBuilder.CreateIndex(
                name: "IX_PerfilPermiso_PermisoId",
                table: "PerfilPermiso",
                column: "PermisoId");
            
            migrationBuilder.CreateIndex(
                name: "IX_PreguntaGrado_PreguntaId",
                table: "PreguntaGrado",
                column: "PreguntaId");  
            
            migrationBuilder.CreateIndex(
                name: "IX_Preguntas_EncuestaVersionId",
                table: "Preguntas",
                column: "EncuestaVersionId");
            migrationBuilder.CreateIndex(
                name: "IX_Preguntas_CarenciaId",
                table: "Preguntas",
                column: "CarenciaId");   
            
            migrationBuilder.CreateIndex(
                name: "IX_RespuestaGrado_RespuestaId",
                table: "RespuestaGrado",
                column: "RespuestaId");
            migrationBuilder.CreateIndex(
                name: "IX_RespuestaGrado_PreguntaGradoId",
                table: "RespuestaGrado",
                column: "PreguntaGradoId"); 
            
            migrationBuilder.CreateIndex(
                name: "IX_Respuestas_PreguntaId",
                table: "Respuestas",
                column: "PreguntaId");
            
            migrationBuilder.CreateIndex(
                name: "IX_TrabajadoresDependencias_DependenciaId",
                table: "TrabajadoresDependencias",
                column: "DependenciaId");
            migrationBuilder.CreateIndex(
                name: "IX_TrabajadoresDependencias_TrabajadorId",
                table: "TrabajadoresDependencias",
                column: "TrabajadorId");

            migrationBuilder.CreateIndex(
                name: "IX_UsuariosDependencias_DependenciaId",
                table: "UsuariosDependencias",
                column: "DependenciaId");  
            migrationBuilder.CreateIndex(
                name: "IX_UsuariosDependencias_UsuarioId",
                table: "UsuariosDependencias",
                column: "UsuarioId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex("IX_Agebs_LocalidadId","Agebs");
            migrationBuilder.DropIndex("IX_Agebs_MunicipioId","Agebs");
            migrationBuilder.DropIndex("IX_Agebs_EstadoId","Agebs");
            
            migrationBuilder.DropIndex("IX_Aplicaciones_BeneficiarioId","Aplicaciones");
            migrationBuilder.DropIndex("IX_Aplicaciones_EncuestaVersionId","Aplicaciones");
            migrationBuilder.DropIndex("IX_Aplicaciones_TrabajadorId","Aplicaciones");
            
            migrationBuilder.DropIndex("IX_AplicacionPreguntas_AplicacionId","AplicacionPreguntas");
            migrationBuilder.DropIndex("IX_AplicacionPreguntas_PreguntaId","AplicacionPreguntas");
            migrationBuilder.DropIndex("IX_AplicacionPreguntas_RespuestaId","AplicacionPreguntas");
            migrationBuilder.DropForeignKey("FK_Respuestas_AplicacionPreguntas","AplicacionPreguntas");
            
            migrationBuilder.DropIndex("IX_BeneficiarioHistorico_BeneficiarioId","BeneficiarioHistorico");
            migrationBuilder.DropIndex("IX_BeneficiarioHistorico_EstudioId","BeneficiarioHistorico");
            migrationBuilder.DropIndex("IX_BeneficiarioHistorico_EstadoId","BeneficiarioHistorico");
            migrationBuilder.DropIndex("IX_BeneficiarioHistorico_EstadoCivilId","BeneficiarioHistorico");
            migrationBuilder.DropIndex("IX_BeneficiarioHistorico_DiscapacidadId","BeneficiarioHistorico");
            migrationBuilder.DropIndex("IX_BeneficiarioHistorico_SexoId","BeneficiarioHistorico");
            migrationBuilder.DropIndex("IX_BeneficiarioHistorico_TrabajadorId","BeneficiarioHistorico");
            migrationBuilder.DropIndex("IX_BeneficiarioHistorico_GradoEstudioId","BeneficiarioHistorico");
            migrationBuilder.DropIndex("IX_BeneficiarioHistorico_CausaDiscapacidadId","BeneficiarioHistorico");
            migrationBuilder.DropIndex("IX_BeneficiarioHistorico_DiscapacidadGradoId","BeneficiarioHistorico");
            
            migrationBuilder.DropIndex("IX_Beneficiarios_EstadoId","Beneficiarios");
            migrationBuilder.DropIndex("IX_Beneficiarios_EstudioId","Beneficiarios");
            migrationBuilder.DropIndex("IX_Beneficiarios_EstadoCivilId","Beneficiarios");
            migrationBuilder.DropIndex("IX_Beneficiarios_DiscapacidadId","Beneficiarios");
            migrationBuilder.DropIndex("IX_Beneficiarios_SexoId","Beneficiarios");
            migrationBuilder.DropIndex("IX_Beneficiarios_TrabajadorId","Beneficiarios");
            migrationBuilder.DropIndex("IX_Beneficiarios_PadreId","Beneficiarios");
            migrationBuilder.DropIndex("IX_Beneficiarios_ParentescoId","Beneficiarios");
            migrationBuilder.DropIndex("IX_Beneficiarios_GradoEstudioId","Beneficiarios");
            migrationBuilder.DropIndex("IX_Beneficiarios_CausaDiscapacidadId","Beneficiarios");
            migrationBuilder.DropIndex("IX_Beneficiarios_DiscapacidadGradoId","Beneficiarios");
            migrationBuilder.DropIndex("IX_Beneficiarios_HijoId","Beneficiarios");
            
            migrationBuilder.DropIndex("IX_Bitacora_UsuarioId","Bitacora");
            
            migrationBuilder.DropIndex("IX_DiscapacidadGrado_DiscapacidadId","DiscapacidadGrado");
            migrationBuilder.DropIndex("IX_DiscapacidadGrado_GradoId","DiscapacidadGrado");
            
            migrationBuilder.DropIndex("IX_Domicilio_AgebId","Domicilio");
            migrationBuilder.DropIndex("IX_Domicilio_BeneficiarioId","Domicilio");
            migrationBuilder.DropIndex("IX_Domicilio_MunicipioId","Domicilio");
            migrationBuilder.DropIndex("IX_Domicilio_LocalidadId","Domicilio");
            migrationBuilder.DropIndex("IX_Domicilio_TrabajadorId","Domicilio");
            migrationBuilder.DropIndex("IX_Domicilio_ManzanaId","Domicilio");
            migrationBuilder.DropIndex("IX_Domicilio_CarreteraId","Domicilio");
            migrationBuilder.DropIndex("IX_Domicilio_CaminoId","Domicilio");
            migrationBuilder.DropIndex("IX_Domicilio_TipoAsentamientoId","Domicilio");
            
            migrationBuilder.DropIndex("IX_EncuestaVersiones_EncuestaId","EncuestaVersiones");
            
            migrationBuilder.DropIndex("IX_Importaciones_EncuestaId","Importaciones");
            
            migrationBuilder.DropIndex("IX_Localidades_ZonaIndigenaId","Localidades");
            migrationBuilder.DropIndex("IX_Localidades_MunicipioId","Localidades");
            migrationBuilder.DropIndex("IX_Localidades_EstadoId","Localidades");
         
            migrationBuilder.DropIndex("IX_Manzanas_LocalidadId","Manzanas");
            migrationBuilder.DropIndex("IX_Manzanas_MunicipioId","Manzanas");
            migrationBuilder.DropIndex("IX_Manzanas_AgebId","Manzanas");
            
            migrationBuilder.DropIndex("IX_Municipios_EstadoId","Municipios");
            
            migrationBuilder.DropIndex("IX_MunicipiosZonas_ZonaId","MunicipiosZonas");
            migrationBuilder.DropIndex("IX_MunicipiosZonas_MunicipioId","MunicipiosZonas");
            
            migrationBuilder.DropIndex("IX_PerfilPermiso_PerfilId","PerfilPermiso");
            migrationBuilder.DropIndex("IX_PerfilPermiso_PermisoId","PerfilPermiso");
     
            migrationBuilder.DropIndex("IX_PreguntaGrado_PreguntaId","PreguntaGrado");
            
            migrationBuilder.DropIndex("IX_Preguntas_EncuestaVersionId","Preguntas");
            migrationBuilder.DropIndex("IX_Preguntas_CarenciaId","Preguntas");
            
            migrationBuilder.DropIndex("IX_RespuestaGrado_RespuestaId","RespuestaGrado");
            migrationBuilder.DropIndex("IX_RespuestaGrado_PreguntaGradoId","RespuestaGrado");
            
            migrationBuilder.DropIndex("IX_Respuestas_PreguntaId","Respuestas");
            
            migrationBuilder.DropIndex("IX_TrabajadoresDependencias_DependenciaId","TrabajadoresDependencias");
            migrationBuilder.DropIndex("IX_TrabajadoresDependencias_TrabajadorId","TrabajadoresDependencias");
            
            migrationBuilder.DropIndex("IX_UsuariosDependencias_DependenciaId","UsuariosDependencias");
            migrationBuilder.DropIndex("IX_UsuariosDependencias_UsuarioId","UsuariosDependencias");
        }
    }
}
