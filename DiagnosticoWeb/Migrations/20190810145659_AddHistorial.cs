using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace DiagnosticoWeb.Migrations
{
    public partial class AddHistorial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BeneficiarioHistorico",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 450, nullable: false),
                    ApellidoPaterno = table.Column<string>(maxLength: 256, nullable: true),
                    ApellidoMaterno = table.Column<string>(maxLength: 256, nullable: true),
                    Nombre = table.Column<string>(maxLength: 256, nullable: true),
                    FechaNacimiento = table.Column<DateTime>(nullable: true),
                    Curp = table.Column<string>(maxLength: 18, nullable: true),
                    Rfc = table.Column<string>(maxLength: 13, nullable: true),
                    Comentarios = table.Column<string>(maxLength: 450, nullable: true),
                    BeneficiarioId = table.Column<string>(maxLength: 450, nullable: false),
                    EstudioId = table.Column<string>(maxLength: 450, nullable: true),
                    EstadoId = table.Column<string>(maxLength: 450, nullable: true),
                    EstadoCivilId = table.Column<string>(maxLength: 450, nullable: true),
                    DiscapacidadId = table.Column<string>(maxLength: 450, nullable: true),
                    SexoId = table.Column<string>(maxLength: 450, nullable: true),
                    TrabajadorId = table.Column<string>(maxLength: 450, nullable: true),
                    DomicilioId = table.Column<string>(maxLength: 450, nullable: true),
                    Estatus = table.Column<bool>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BeneficiarioHistorico", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HistorialBeneficiario_Beneficiario",
                        column: x => x.BeneficiarioId,
                        principalTable: "Beneficiarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_HistorialBeneficiario_Estudio",
                        column: x => x.EstudioId,
                        principalTable: "Estudios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_HistorialBeneficiario_EstadoCivil",
                        column: x => x.EstadoCivilId,
                        principalTable: "EstadosCiviles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_HistorialBeneficiario_Discapacidad",
                        column: x => x.DiscapacidadId,
                        principalTable: "Discapacidades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_HistorialBeneficiario_Sexo",
                        column: x => x.SexoId,
                        principalTable: "Sexos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_HistorialBeneficiario_Trabajador",
                        column: x => x.TrabajadorId,
                        principalTable: "Trabajadores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction); 
                    table.ForeignKey(
                        name: "FK_HistorialBeneficiario_Domicilio",
                        column: x => x.DomicilioId,
                        principalTable: "Domicilio",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.AddColumn<bool>(
                name: "Activa",
                table: "Domicilio",
                nullable: true,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Activa",
                table: "Aplicaciones",
                nullable: true,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BeneficiarioHistorico");

            migrationBuilder.DropColumn(
                name: "Activa",
                table: "Domicilio");

            migrationBuilder.DropColumn(
                name: "Activa",
                table: "Aplicaciones");
        }
    }
}
