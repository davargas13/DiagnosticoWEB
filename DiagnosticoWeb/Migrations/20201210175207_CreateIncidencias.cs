using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DiagnosticoWeb.Migrations
{
    public partial class CreateIncidencias : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TipoIncidencias",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 450),
                    Nombre = table.Column<string>(maxLength:100),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoIncidencias", x => x.Id);
                });  
            
            migrationBuilder.CreateTable(
                name: "Incidencias",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 450),
                    Observaciones = table.Column<string>(maxLength:1000),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    TrabajadorId = table.Column<string>(maxLength: 450),
                    TipoIncidenciaId = table.Column<string>(maxLength: 450),
                    BeneficiarioId = table.Column<string>(maxLength: 450),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Incidencias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Incidencias_TipoIncidencias_TipoIncidenciaId",
                        column: x => x.TipoIncidenciaId,
                        principalTable: "TipoIncidencias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Incidencias_Trabajadores_TrabajadorId",
                        column: x => x.TrabajadorId,
                        principalTable: "Trabajadores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Incidencias_Beneficiarios_BeneficiarioId",
                        column: x => x.BeneficiarioId,
                        principalTable: "Beneficiarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });
            migrationBuilder.CreateIndex("IX_Incidencias_TrabajadorId", "Incidencias", "TrabajadorId");
            migrationBuilder.CreateIndex("IX_Incidencias_TipoIncidenciaId", "Incidencias", "TipoIncidenciaId");
            migrationBuilder.CreateIndex("IX_Incidencias_BeneficiarioId", "Incidencias", "BeneficiarioId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex("IX_Incidencias_UsuarioId");
            migrationBuilder.DropIndex("IX_Incidencias_TipoIncidenciaId");
            migrationBuilder.DropTable("Incidencias");
            migrationBuilder.DropTable("TipoIncidencias");
        }
    }
}
