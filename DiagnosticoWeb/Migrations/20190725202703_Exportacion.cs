using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DiagnosticoWeb.Migrations
{
    public partial class Exportacion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Importaciones",
                columns: table => new
                {
                    Clave = table.Column<string>(maxLength: 256),
                    UsuarioId = table.Column<string>(maxLength: 450, nullable: true),
                    ImportedAt = table.Column<DateTime>(nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Importaciones", x => x.Clave);
                    table.ForeignKey(
                        name: "FK_Importaciones_Usuarios",
                        column: x => x.UsuarioId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "Importaciones");

        }
    }
}
