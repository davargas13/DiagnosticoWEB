using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DiagnosticoWeb.Migrations
{
    public partial class AddEjercicioPrograma : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Proyecto",
                table: "ProgramasSociales",
                nullable: true,
                maxLength: 450);   
            migrationBuilder.AddColumn<string>(
                name: "Ciclo",
                table: "Vertientes",
                nullable: true,
                maxLength: 450);
            migrationBuilder.AddColumn<string>(
                name: "Ejercicio",
                table: "Vertientes",
                nullable: true,
                maxLength: 450);   
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ejercicio", 
                table: "Vertientes");
            migrationBuilder.DropColumn(
                name: "Ciclo", 
                table: "Vertientes");
            migrationBuilder.DropColumn(
                name: "Proyecto", 
                table: "ProgramasSociales");
        }
    }
}
