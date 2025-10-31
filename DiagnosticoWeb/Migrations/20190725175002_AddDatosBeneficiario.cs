using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DiagnosticoWeb.Migrations
{
    public partial class AddDatosBeneficiario : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Carencias",
                table: "Aplicaciones",
                nullable: true,
                maxLength: 450);

            migrationBuilder.AddColumn<string>(
                name: "EstudioId",
                table: "Beneficiarios",
                nullable: true,
                maxLength: 450);

            migrationBuilder.AddForeignKey(
                name: "FK_Beneficiario_Estudios",
                table: "Beneficiarios",
                column: "EstudioId",
                principalTable: "Estudios",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddColumn<string>(
                name: "EstadoCivilId",
                table: "Beneficiarios",
                nullable: true,
                maxLength: 450);

            migrationBuilder.AddForeignKey(
                name: "FK_Beneficiario_EstadoCivil",
                table: "Beneficiarios",
                column: "EstadoCivilId",
                principalTable: "EstadosCiviles",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddColumn<string>(
                name: "DiscapacidadId",
                table: "Beneficiarios",
                nullable: true,
                maxLength: 450);

            migrationBuilder.AddForeignKey(
                name: "FK_Beneficiario_Discapacidad",
                table: "Beneficiarios",
                column: "DiscapacidadId",
                principalTable: "Estudios",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Carencias",
                table: "Aplicaciones");

            migrationBuilder.DropColumn(
                name: "EstudioId",
                table: "Beneficiarios");

            migrationBuilder.DropForeignKey(
                name: "FK_Beneficiario_Estudios",
                table: "Beneficiarios");

            migrationBuilder.DropColumn(
                name: "EstadoCivilId",
                table: "Beneficiarios");

            migrationBuilder.DropForeignKey(
                name: "FK_Beneficiario_EstadoCivil",
                table: "Beneficiarios");

            migrationBuilder.DropColumn(
                name: "DiscapacidadId",
                table: "Beneficiarios");

            migrationBuilder.DropForeignKey(
                name: "FK_Beneficiario_Discapacidad",
                table: "Beneficiarios");
        }
    }
}
