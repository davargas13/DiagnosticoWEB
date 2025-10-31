using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DiagnosticoWeb.Migrations
{
    public partial class CrearBitacora : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bitacora",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 450),
                    UsuarioId = table.Column<string>(maxLength: 450, nullable: true),
                    Accion = table.Column<string>(maxLength: 450, nullable: true),
                    Mensaje = table.Column<string>(maxLength: 450, nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bitacora", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bitacora_Usuarios",
                        column: x => x.UsuarioId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "Bitacora");
        }
    }
}
