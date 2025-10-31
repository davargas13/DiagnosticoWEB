using System;
using Microsoft.EntityFrameworkCore.Migrations;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;

namespace DiagnosticoWeb.Migrations
{
    public partial class Carencias : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Carencias",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 450, nullable: false),
                    Clave = table.Column<string>(maxLength: 256, nullable: true),
                    Nombre = table.Column<string>(maxLength: 256, nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_Carencias", x => x.Id); });
            
            migrationBuilder.CreateTable(
                name: "VertienteCarencias",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 450, nullable: false),
                    VertienteId = table.Column<string>(maxLength: 450, nullable: true),
                    CarenciaId = table.Column<string>(maxLength: 450, nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VertienteCarencias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VertienteCarencia_Carencias",
                        column: x => x.CarenciaId,
                        principalTable: "Carencias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VertienteCarencia_Vertientes",
                        column: x => x.VertienteId,
                        principalTable: "Vertientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "VertienteCarencias");
            migrationBuilder.DropTable(name: "Carencias");
        }
    }
}