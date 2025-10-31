using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DiagnosticoWeb.Migrations
{
    public partial class AddAplicacionCarencias : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AplicacionCarencias",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 450),
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
                    Pea = table.Column<bool>(),
                    ParentescoId = table.Column<string>(),
                    Edad = table.Column<int>(),
                    Ingreso = table.Column<double>(),
                    LineaBienestar = table.Column<int>(),
                    NumIntegrante = table.Column<int>(),
                    NivelPobreza = table.Column<string>(maxLength: 100),
                    CreatedAt = table.Column<DateTime>(),
                    UpdatedAt = table.Column<DateTime>(),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    AplicacionId = table.Column<string>(maxLength: 450),
                    BeneficiarioId = table.Column<string>(maxLength: 450)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AplicacionCarencias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AplicacionCarencias_Aplicacion",
                        column: x => x.AplicacionId,
                        principalTable: "Aplicaciones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_AplicacionCarencias_Beneficiario",
                        column: x => x.BeneficiarioId,
                        principalTable: "Beneficiarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.AddColumn<bool>(name: "Educativa", table: "Aplicaciones", defaultValue: false);
            migrationBuilder.AddColumn<bool>(name: "Analfabetismo", table: "Aplicaciones", defaultValue: false);
            migrationBuilder.AddColumn<bool>(name: "Inasistencia", table: "Aplicaciones", defaultValue: false);
            migrationBuilder.AddColumn<bool>(name: "PrimariaIncompleta", table: "Aplicaciones", defaultValue: false);
            migrationBuilder.AddColumn<bool>(name: "SecundariaIncompleta", table: "Aplicaciones", defaultValue: false);
            migrationBuilder.AddColumn<bool>(name: "ServicioSalud", table: "Aplicaciones", defaultValue: false);
            migrationBuilder.AddColumn<bool>(name: "SeguridadSocial", table: "Aplicaciones", defaultValue: false);
            migrationBuilder.AddColumn<bool>(name: "Vivienda", table: "Aplicaciones", defaultValue: false);
            migrationBuilder.AddColumn<bool>(name: "Piso", table: "Aplicaciones", defaultValue: false);
            migrationBuilder.AddColumn<bool>(name: "Techo", table: "Aplicaciones", defaultValue: false);
            migrationBuilder.AddColumn<bool>(name: "Muro", table: "Aplicaciones", defaultValue: false);
            migrationBuilder.AddColumn<bool>(name: "Hacinamiento", table: "Aplicaciones", defaultValue: false);
            migrationBuilder.AddColumn<bool>(name: "Servicios", table: "Aplicaciones", defaultValue: false);
            migrationBuilder.AddColumn<bool>(name: "Agua", table: "Aplicaciones", defaultValue: false);
            migrationBuilder.AddColumn<bool>(name: "Drenaje", table: "Aplicaciones", defaultValue: false);
            migrationBuilder.AddColumn<bool>(name: "Electricidad", table: "Aplicaciones", defaultValue: false);
            migrationBuilder.AddColumn<bool>(name: "Combustible", table: "Aplicaciones", defaultValue: false);
            migrationBuilder.AddColumn<bool>(name: "Alimentaria", table: "Aplicaciones", defaultValue: false);
            migrationBuilder.AddColumn<string>(name: "GradoAlimentaria", table: "Aplicaciones", maxLength: 100, nullable: true);
            migrationBuilder.AddColumn<bool>(name: "Pea", table: "Aplicaciones", defaultValue: false);
            migrationBuilder.AddColumn<double>(name: "Ingreso", table: "Aplicaciones", defaultValue: 0);
            migrationBuilder.AddColumn<int>(name: "LineaBienestar", table: "Aplicaciones", defaultValue: 0);
            migrationBuilder.AddColumn<string>(name: "NivelPobreza", table: "Aplicaciones", maxLength: 100, nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "AplicacionCarencias");
            migrationBuilder.DropColumn("Educativa", "Aplicaciones");
            migrationBuilder.DropColumn("Analfabetismo", "Aplicaciones");
            migrationBuilder.DropColumn("Inasistencia", "Aplicaciones");
            migrationBuilder.DropColumn("PrimariaIncompleta", "Aplicaciones");
            migrationBuilder.DropColumn("SecundariaIncompleta", "Aplicaciones");
            migrationBuilder.DropColumn("ServicioSalud", "Aplicaciones");
            migrationBuilder.DropColumn("SeguridadSocial", "Aplicaciones");
            migrationBuilder.DropColumn("Vivienda", "Aplicaciones");
            migrationBuilder.DropColumn("Piso", "Aplicaciones");
            migrationBuilder.DropColumn("Techo", "Aplicaciones");
            migrationBuilder.DropColumn("Muro", "Aplicaciones");
            migrationBuilder.DropColumn("Hacinamiento", "Aplicaciones");
            migrationBuilder.DropColumn("Servicios", "Aplicaciones");
            migrationBuilder.DropColumn("Agua", "Aplicaciones");
            migrationBuilder.DropColumn("Drenaje", "Aplicaciones");
            migrationBuilder.DropColumn("Electricidad", "Aplicaciones");
            migrationBuilder.DropColumn("Combustible", "Aplicaciones");
            migrationBuilder.DropColumn("Alimentaria", "Aplicaciones");
            migrationBuilder.DropColumn("Pea", "Aplicaciones");
            migrationBuilder.DropColumn("Ingreso", "Aplicaciones");
            migrationBuilder.DropColumn("LineaBienestar", "Aplicaciones");
        }
    }
}