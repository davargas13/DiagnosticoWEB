using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DiagnosticoWeb.Migrations
{
    public partial class AddValoresAplicacion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ValorNumerico",
                table: "AplicacionPreguntas",
                nullable: true);
            migrationBuilder.AddColumn<DateTime>(
                name: "ValorFecha",
                table: "AplicacionPreguntas",
                nullable: true);
            migrationBuilder.AddColumn<string>(
                name: "ValorCatalogo",
                table: "AplicacionPreguntas",
                nullable: true);
            
            migrationBuilder.AddColumn<string>(
                name: "TipoZap",
                table: "Domicilio",
                maxLength:100,
                nullable: true);
            migrationBuilder.AddColumn<string>(
                name: "CodigoPostalCalculado",
                table: "Domicilio",
                maxLength:10,
                nullable: true); 
            migrationBuilder.AddColumn<string>(
                name: "Colonia",
                table: "Domicilio",
                nullable: true);
            migrationBuilder.AddColumn<string>(
                name: "CisCercano",
                table: "Domicilio",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ValorCatalogo",
                table: "AplicacionPreguntas");
            migrationBuilder.DropColumn(
                name: "ValorFecha",
                table: "AplicacionPreguntas");
            migrationBuilder.DropColumn(
                name: "ValorNumerico",
                table: "AplicacionPreguntas");
            
            migrationBuilder.DropColumn(
                name: "CisCercano",
                table: "Domicilio");  
            migrationBuilder.DropColumn(
                name: "Colonia",
                table: "Domicilio");  
            migrationBuilder.DropColumn(
                name: "CodigoPostalCalculado",
                table: "Domicilio");  
            migrationBuilder.DropColumn(
                name: "TipoZap",
                table: "Domicilio");
        }
    }
}
