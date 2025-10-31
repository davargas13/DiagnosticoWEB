using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DiagnosticoWeb.Migrations
{
    public partial class ChangeRespuestaIteracion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "RespuestaIteracion",
                type: "int",
                table: "AplicacionPreguntas");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "RespuestaIteracion",
                type: "nvarchar(max)",
                table: "AplicacionPreguntas");
        }
    }
}
