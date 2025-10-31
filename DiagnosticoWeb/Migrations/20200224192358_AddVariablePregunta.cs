using Microsoft.EntityFrameworkCore.Migrations;

namespace DiagnosticoWeb.Migrations
{
    public partial class AddVariablePregunta : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Variable",
                table: "Preguntas",
                nullable: true); 
            
            migrationBuilder.AddColumn<string>(
                name: "Codigo",
                table: "Trabajadores",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Variable",
                table: "Preguntas");
            
            migrationBuilder.DropColumn(
                name: "Codigo",
                table: "Trabajadores");
        }
    }
}
