using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace DiagnosticoWeb.Migrations
{
    public partial class add_datos_vertientes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Unidades",
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
                    table.PrimaryKey("PK_Unidades", x => x.Id);
                });

            migrationBuilder.AddColumn<string>(
                name: "UnidadId",
                table: "Vertientes",
                nullable: true,
                maxLength: 450);

            migrationBuilder.AddForeignKey(
                name: "FK_Vertiente_Unidad",
                table: "Vertientes",
                column: "UnidadId",
                principalTable: "Unidades",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddColumn<double>(
                name: "Costo",
                table: "Vertientes",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Cantidad",
                table: "Solicitudes",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Costo",
                table: "Solicitudes",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Economico",
                table: "Solicitudes",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Unidades");

            migrationBuilder.DropForeignKey(
                name: "FK_Vertiente_Unidad",
                table: "Vertientes");

            migrationBuilder.DropColumn(
                name: "UnidadId", 
                table: "Vertientes");

            migrationBuilder.DropColumn(
                name: "Costo",
                table: "Vertientes");

            migrationBuilder.DropColumn(
                name: "Cantidad",
                table: "Solicitudes");

            migrationBuilder.DropColumn(
                name: "Costo",
                table: "Solicitudes");

            migrationBuilder.DropColumn(
                name: "Economico",
                table: "Solicitudes");
        }
    }
}
