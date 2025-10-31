using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DiagnosticoWeb.Migrations
{
    public partial class ConfVertientes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Vigencia",
                table: "Vertientes",
                nullable: true
            );
            migrationBuilder.AddColumn<string>(
                name: "TipoEntrega",
                table: "Vertientes",
                nullable: true
            );
            migrationBuilder.CreateTable(
                name: "VertientesArchivos",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 450, nullable: false),
                    TipoArchivo = table.Column<string>(nullable: true),
                    VertienteId = table.Column<string>(maxLength: 450, nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VertientesArchivos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VertientesArchivos",
                        column: x => x.VertienteId,
                        principalTable: "Vertientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
            migrationBuilder.CreateTable(
                name: "TipoAsentamientos",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 450, nullable: false),
                    Nombre = table.Column<string>(maxLength: 256, nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoAsentamientos", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("VertientesArchivos");
            migrationBuilder.DropColumn(name:"TipoEntrega", table:"Vertientes");
            migrationBuilder.DropColumn(name:"Vigencia", table:"Vertientes");
        }
    }
}
