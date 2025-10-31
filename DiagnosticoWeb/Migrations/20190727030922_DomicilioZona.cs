using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DiagnosticoWeb.Migrations
{
    public partial class DomicilioZona : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Indigena",
                table: "Localidades",
                newName: "ZonaIndigenaId");

            migrationBuilder.AddColumn<string>(
                name: "ZonaImpulsoId",
                table: "Domicilio",
                nullable: true);
            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ZonaImpulsoId",
                table: "Domicilio");

            migrationBuilder.RenameColumn(
                name: "ZonaIndigenaId",
                table: "Localidades",
                newName: "Indigena");
        }
    }
}
