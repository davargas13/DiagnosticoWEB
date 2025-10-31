using Microsoft.EntityFrameworkCore.Migrations;

namespace DiagnosticoWeb.Migrations
{
    public partial class AddSexoIdAplicaciones : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(name: "SexoId", table: "AplicacionCarencias",nullable:true);
            migrationBuilder.AddColumn<bool>(name: "Discapacidad", table: "AplicacionCarencias",nullable:false, defaultValue:false);
            migrationBuilder.AddColumn<short>(name: "GradoEducativa", table: "AplicacionCarencias",nullable:true);
            migrationBuilder.AddColumn<bool>(name: "Discapacidad", table: "Aplicaciones", nullable:false, defaultValue:false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "SexoId", table: "AplicacionCarencias");
            migrationBuilder.DropColumn(name: "Discapacidad", table: "AplicacionCarencias");
            migrationBuilder.DropColumn(name: "GradoEducativa", table: "AplicacionCarencias");
            migrationBuilder.DropColumn(name: "Discapacidad", table: "Aplicaciones");
        }
    }
}
