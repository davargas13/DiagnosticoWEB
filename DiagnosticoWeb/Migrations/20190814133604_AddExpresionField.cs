using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DiagnosticoWeb.Migrations
{
    public partial class AddExpresionField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Expresion",
                table: "Preguntas",
                nullable: true,
                maxLength: 450);

            migrationBuilder.AddColumn<string>(
                name: "ExpresionEjemplo",
                table: "Preguntas",
                nullable: true,
                maxLength: 450);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Expresion",
                table: "Preguntas");

            migrationBuilder.DropColumn(
                name: "ExpresionEjemplo",
                table: "Preguntas");
        }
    }
}
