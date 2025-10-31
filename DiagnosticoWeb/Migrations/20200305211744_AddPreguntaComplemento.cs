using Microsoft.EntityFrameworkCore.Migrations;

namespace DiagnosticoWeb.Migrations
{
    public partial class AddPreguntaComplemento : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Complemento",
                table: "AplicacionPreguntas",
                nullable: true);
            migrationBuilder.AddColumn<string>(
                name: "Complemento",
                table: "Preguntas",
                nullable: true);
            migrationBuilder.AddColumn<string>(
                name: "TipoComplemento",
                table: "Preguntas",
                nullable: true);
            migrationBuilder.AddColumn<string>(
                name: "Complemento",
                table: "Respuestas",
                nullable: true);
            migrationBuilder.AddColumn<string>(
                name: "TipoComplemento",
                table: "Respuestas",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TipoComplemento",
                table: "Respuestas");
            migrationBuilder.DropColumn(
                name: "Complemento",
                table: "Respuestas");
            migrationBuilder.DropColumn(
                name: "TipoComplemento",
                table: "Preguntas");
            migrationBuilder.DropColumn(
                name: "Complemento",
                table: "Preguntas");
            migrationBuilder.DropColumn(
                name: "Complemento",
                table: "AplicacionPreguntas");
        }
    }
}
