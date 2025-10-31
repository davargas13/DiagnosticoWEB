using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DiagnosticoWeb.Migrations
{
    public partial class Zonas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Zonas",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 450, nullable: false),
                    Clave = table.Column<string>(nullable: true),
                    DependenciaId = table.Column<string>(maxLength: 450, nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zonas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Zonas_Dependencias",
                        column: x => x.DependenciaId,
                        principalTable: "Dependencias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MunicipiosZonas",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 450, nullable: false),
                    ZonaId = table.Column<string>(maxLength: 450, nullable: false),
                    MunicipioId = table.Column<string>(maxLength: 450, nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZonasMunicipios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ZonasMunicipios_Municipios",
                        column: x => x.MunicipioId,
                        principalTable: "Municipios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ZonasMunicipios_Zonas",
                        column: x => x.ZonaId,
                        principalTable: "Zonas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "ZonasMunicipios");
            migrationBuilder.DropTable(name: "Zonas");
        }
    }
}