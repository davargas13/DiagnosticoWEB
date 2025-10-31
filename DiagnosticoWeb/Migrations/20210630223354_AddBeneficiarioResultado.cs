using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DiagnosticoWeb.Migrations
{
    public partial class AddBeneficiarioResultado : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BeneficiarioId",
                table: "AlgoritmoResultados",
                nullable: false,
                maxLength:450);
            
            migrationBuilder.AddForeignKey(
                name: "FK_AlgoritmoResultados_Beneficiario",
                table: "AlgoritmoResultados",
                column: "BeneficiarioId",
                principalTable: "Beneficiarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "FK_AlgoritmoResultados_Beneficiario", table: "AlgoritmoResultados");
            migrationBuilder.DropColumn(name: "BeneficiarioId", table: "AlgoritmoResultados");
        }
    }
}
