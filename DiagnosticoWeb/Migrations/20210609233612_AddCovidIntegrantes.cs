using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DiagnosticoWeb.Migrations
{
    public partial class AddCovidIntegrantes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(name: "PreocupacionCovid", table: "Aplicaciones", nullable: false, defaultValue: 0);
            migrationBuilder.AddColumn<bool>(name: "PerdidaEmpleo", table: "Aplicaciones", nullable: false, defaultValue: false);
            migrationBuilder.AddColumn<bool>(name: "PerdidaEmpleo", table: "AplicacionCarencias", nullable: false, defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn("PerdidaEmpleo","AplicacionCarencias");
            migrationBuilder.DropColumn("PerdidaEmpleo","Aplicaciones");
            migrationBuilder.DropColumn("PreocupacionCovid","Aplicaciones");
        }
    }
}
