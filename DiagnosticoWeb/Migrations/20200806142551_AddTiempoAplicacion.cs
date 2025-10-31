using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DiagnosticoWeb.Migrations
{
    public partial class AddTiempoAplicacion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FechaInicio",
                table: "Aplicaciones",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaFin",
                table: "Aplicaciones",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FechaInicio",
                table: "Aplicaciones");
            
            migrationBuilder.DropColumn(
                name: "FechaFin",
                table: "Aplicaciones");
        }
    }
}
