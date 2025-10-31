using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DiagnosticoWeb.Migrations
{
    public partial class AddNumeroEncuestaVersion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Numero", table: "EncuestaVersiones", defaultValue:0);

            migrationBuilder.AddColumn<bool>(
                name: "Activa", table: "Preguntas", defaultValue: true);
            
            migrationBuilder.AddColumn<bool>(
                name: "Obligatoria", table: "Preguntas", defaultValue: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "Obligatoria", table: "Preguntas");
            migrationBuilder.DropColumn(name: "Activa", table: "Preguntas");
            migrationBuilder.DropColumn(name: "Numero", table: "EncuestaVersiones");
        }
    }
}
