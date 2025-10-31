using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DiagnosticoWeb.Migrations
{
    public partial class RenombrarEstado : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EstadoNacimiento",
                table: "Beneficiarios",
                newName: "EstadoId");

            migrationBuilder.AlterColumn<string>(
                name: "EstadoId",
                table: "Beneficiarios",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Beneficiario_Estado",
                table: "Beneficiarios",
                column: "EstadoId",
                principalTable: "Estados",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.DropColumn(
                name: "MunicipioId",
                table: "Beneficiarios");

            migrationBuilder.DropColumn(
                name: "LocalidadId",
                table: "Beneficiarios");

            migrationBuilder.AddColumn<string>(
                name: "MunicipioId",
                table: "Domicilio",
                nullable: true,
                maxLength: 450);

            migrationBuilder.AddColumn<string>(
                name: "LocalidadId",
                table: "Domicilio",
                nullable: true,
                maxLength: 450);

            migrationBuilder.AddForeignKey(
                name: "FK_Domicilio_Municipio",
                table: "Domicilio",
                column: "MunicipioId",
                principalTable: "Municipios",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Domicilio_Localidad",
                table: "Domicilio",
                column: "LocalidadId",
                principalTable: "Localidades",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.RenameColumn(
                name: "ClaveAGEB",
                table: "Domicilio",
                newName: "AgebId");

            migrationBuilder.AlterColumn<string>(
                name: "AgebId",
                table: "Domicilio",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Domicilio_Ageb",
                table: "Domicilio",
                column: "AgebId",
                principalTable: "Agebs",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EstadoId",
                table: "Beneficiarios",
                newName: "EstadoNacimiento");

            migrationBuilder.AlterColumn<string>(
                name: "EstadoNacimiento",
                table: "Beneficiarios",
                type: "nvarchar(50)",
                nullable: true);

            migrationBuilder.DropForeignKey(
                name: "FK_Beneficiario_Estado",
                table: "Beneficiarios");

            migrationBuilder.AddColumn<string>(
                name: "LocalidadId",
                table: "Beneficiarios",
                nullable: true,
                maxLength: 450);

            migrationBuilder.AddColumn<string>(
                name: "MunicipioId",
                table: "Beneficiarios",
                nullable: true,
                maxLength: 450);

            migrationBuilder.DropColumn(
                name: "MunicipioId",
                table: "Domicilio");

            migrationBuilder.DropColumn(
                name: "LocalidadId",
                table: "Domicilio");

            migrationBuilder.DropForeignKey(
                name: "FK_Domicilio_Municipio",
                table: "Domicilio");

            migrationBuilder.DropForeignKey(
                name: "FK_Domicilio_Localidad",
                table: "Domicilio");

            migrationBuilder.RenameColumn(
                name: "AgebId",
                table: "Domicilio",
                newName: "ClaveAGEB");

            migrationBuilder.AlterColumn<string>(
                name: "ClaveAGEB",
                table: "Domicilio",
                type: "nvarchar(4)",
                nullable: true);

            migrationBuilder.DropForeignKey(
                name: "FK_Domicilio_Ageb",
                table: "Domicilio");
        }
    }
}
