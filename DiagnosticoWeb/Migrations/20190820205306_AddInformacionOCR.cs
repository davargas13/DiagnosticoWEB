using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DiagnosticoWeb.Migrations
{
    public partial class AddInformacionOCR : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CadenaOCR",
                table: "Domicilio",
                nullable: true,
                maxLength: 450);

            migrationBuilder.AddColumn<string>(
                name: "Porcentaje",
                table: "Domicilio",
                nullable: true,
                maxLength: 450);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CadenaOCR",
                table: "Domicilio");

            migrationBuilder.DropColumn(
                name: "Porcentaje",
                table: "Domicilio");
        }
    }
}
