using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DiagnosticoWeb.Migrations
{
    public partial class CreateAlgoritmo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AlgoritmoVersiones",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 450),
                    Version = table.Column<int>(nullable:false, defaultValue:0),
                    FechaAutorizacion = table.Column<DateTime>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    UsuarioId = table.Column<string>(maxLength: 450),
                    AutorizadorId = table.Column<string>(maxLength:450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlgoritmoVersiones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AlgoritmoVersiones_AspNetUsers_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_AlgoritmoVersiones_AspNetUsers_AutorizadorId",
                        column: x => x.AutorizadorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });
            
            migrationBuilder.CreateIndex("IX_AlgoritmoVersiones_UsuarioId", "AlgoritmoVersiones", "UsuarioId");
            migrationBuilder.CreateIndex("IX_AlgoritmoVersiones_AutorizadorId", "AlgoritmoVersiones", "AutorizadorId");
            
            migrationBuilder.CreateTable(
                name: "AlgoritmoResultados",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength:450),
                    Educativa = table.Column<bool>(),
                    Analfabetismo = table.Column<bool>(),
                    Inasistencia = table.Column<bool>(),
                    PrimariaIncompleta = table.Column<bool>(),
                    SecundariaIncompleta = table.Column<bool>(),
                    ServicioSalud = table.Column<bool>(),
                    SeguridadSocial = table.Column<bool>(),
                    Vivienda = table.Column<bool>(),
                    Piso = table.Column<bool>(),
                    Techo = table.Column<bool>(),
                    Muro = table.Column<bool>(),
                    Hacinamiento = table.Column<bool>(),
                    Servicios = table.Column<bool>(),
                    Agua = table.Column<bool>(),
                    Drenaje = table.Column<bool>(),
                    Electricidad = table.Column<bool>(),
                    Combustible = table.Column<bool>(),
                    Alimentaria = table.Column<bool>(),
                    GradoAlimentaria = table.Column<string>(maxLength: 100),
                    TieneActa = table.Column<bool>(),
                    Pea = table.Column<bool>(),
                    Ingreso = table.Column<double>(),
                    LineaBienestar = table.Column<int>(),
                    NivelPobreza = table.Column<string>(maxLength: 100),
                    SeguridadPublica = table.Column<short>(),
                    SeguridadParque = table.Column<short>(),
                    ConfianzaLiderez = table.Column<short>(),
                    ConfianzaInstituciones = table.Column<short>(),
                    Movilidad = table.Column<short>(),
                    RedesSociales = table.Column<short>(),
                    Tejido = table.Column<short>(),
                    Satisfaccion = table.Column<short>(),
                    PropiedadVivienda = table.Column<bool>(),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    VersionId = table.Column<string>(maxLength:450)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlgoritmoResultados", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AlgoritmoResultados_AlgoritmoVersiones_VersionId",
                        column: x => x.VersionId,
                        principalTable: "AlgoritmoVersiones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });
            
            migrationBuilder.CreateIndex("IX_AlgoritmoResultados_VersionId", "AlgoritmoResultados", "VersionId");
            
            migrationBuilder.CreateTable(
                name: "AlgoritmoIntegranteResultados",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength:450),
                    Folio = table.Column<string>(maxLength:100),
                    Educativa = table.Column<bool>(),
                    Analfabetismo = table.Column<bool>(),
                    Inasistencia = table.Column<bool>(),
                    PrimariaIncompleta = table.Column<bool>(),
                    SecundariaIncompleta = table.Column<bool>(),
                    ServicioSalud = table.Column<bool>(),
                    SeguridadSocial = table.Column<bool>(),
                    Vivienda = table.Column<bool>(),
                    Piso = table.Column<bool>(),
                    Techo = table.Column<bool>(),
                    Muro = table.Column<bool>(),
                    Hacinamiento = table.Column<bool>(),
                    Servicios = table.Column<bool>(),
                    Agua = table.Column<bool>(),
                    Drenaje = table.Column<bool>(),
                    Electricidad = table.Column<bool>(),
                    Combustible = table.Column<bool>(),
                    Alimentaria = table.Column<bool>(),
                    GradoAlimentaria = table.Column<string>(maxLength: 100),
                    TieneActa = table.Column<bool>(),
                    Pea = table.Column<bool>(),
                    Ingreso = table.Column<double>(),
                    LineaBienestar = table.Column<int>(),
                    NivelPobreza = table.Column<string>(maxLength: 100),
                    ParentescoId = table.Column<string>(maxLength:450),
                    SexoId = table.Column<string>(maxLength:450),
                    Discapacidad = table.Column<string>(),
                    GradoEducativa = table.Column<int>(),
                    Edad = table.Column<int>(),
                    NumIntegrante = table.Column<int>(),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    ResultadoId = table.Column<string>(maxLength:450)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlgoritmoIntegranteResultados", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AlgoritmoIntegranteResultados_AlgoritmoResultados_ResultadoId",
                        column: x => x.ResultadoId,
                        principalTable: "AlgoritmoResultados",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });
            
            migrationBuilder.CreateIndex("IX_AlgoritmoIntegranteResultados_ResultadoId", "AlgoritmoIntegranteResultados", "ResultadoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex("IX_AlgoritmoIntegranteResultados_ResultadoId");
            migrationBuilder.DropIndex("IX_AlgoritmoResultados_VersionId");
            migrationBuilder.DropIndex("IX_AlgoritmoVersiones_UsuarioId");
            migrationBuilder.DropIndex("IX_AlgoritmoVersiones_AutorizadorId");

            migrationBuilder.DropTable("AlgoritmoIntegranteResultados");
            migrationBuilder.DropTable("AlgoritmoResultados");
            migrationBuilder.DropTable("AlgoritmoVersiones");
        }
    }
}
