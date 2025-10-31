using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DiagnosticoWeb.Migrations
{
    public partial class Regiones : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateTable(
                name: "TrabajadorRegion",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    TrabajadorId = table.Column<string>(nullable: true),
                    ZonaId = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    DeletedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrabajadorRegion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrabajadorRegion_Trabajadores_TrabajadorId",
                        column: x => x.TrabajadorId,
                        principalTable: "Trabajadores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrabajadorRegion_Zonas_ZonaId",
                        column: x => x.ZonaId,
                        principalTable: "Zonas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UsuarioRegion",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UsuarioId = table.Column<string>(nullable: true),
                    ZonaId = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    DeletedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioRegion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsuarioRegion_AspNetUsers_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UsuarioRegion_Zonas_ZonaId",
                        column: x => x.ZonaId,
                        principalTable: "Zonas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrabajadorRegion_TrabajadorId",
                table: "TrabajadorRegion",
                column: "TrabajadorId");

            migrationBuilder.CreateIndex(
                name: "IX_TrabajadorRegion_ZonaId",
                table: "TrabajadorRegion",
                column: "ZonaId");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioRegion_UsuarioId",
                table: "UsuarioRegion",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioRegion_ZonaId",
                table: "UsuarioRegion",
                column: "ZonaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrabajadorRegion");

            migrationBuilder.DropTable(
                name: "UsuarioRegion");
        }
    }
}
