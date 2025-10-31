using Microsoft.EntityFrameworkCore.Migrations;

namespace DiagnosticoWeb.Migrations
{
    public partial class DropColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Respuesta_AplicacionPreguntas",
                table: "AplicacionPreguntas");

            migrationBuilder.DropColumn(
                name: "Inmediato",
                table: "Vertientes");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddForeignKey(
                name: "FK_Respuesta_AplicacionPreguntas",
                table: "AplicacionPreguntas",
                column: "RespuestaId",
                principalTable: "Respuestas",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddColumn<bool>(
                name: "Inmediato",
                table: "Vertientes",
                nullable: true);
        }
    }
}
