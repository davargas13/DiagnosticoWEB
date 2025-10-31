using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DiagnosticoWeb.Migrations
{
    public partial class AddHuellasBeneficiario : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Huellas",
                table: "Beneficiarios",
                nullable: true);
            
            migrationBuilder.AddColumn<string>(
                name: "Huellas",
                table: "BeneficiarioHistorico",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Huellas",
                table: "BeneficiarioHistorico");

            migrationBuilder.DropColumn(
                name: "Huellas",
                table: "Beneficiarios");
        }
    }
}
