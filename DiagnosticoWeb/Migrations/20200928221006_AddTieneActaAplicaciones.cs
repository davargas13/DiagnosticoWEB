using Microsoft.EntityFrameworkCore.Migrations;

namespace DiagnosticoWeb.Migrations
{
    public partial class AddTieneActaAplicaciones : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(name: "TieneActa", table: "Aplicaciones", nullable: false, defaultValue: false);

            migrationBuilder.AddColumn<bool>(name: "TieneActa", table: "AplicacionCarencias", nullable: false, defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "TieneActa", table: "Aplicaciones");

            migrationBuilder.DropColumn(name: "TieneActa", table: "AplicacionCarencias");
        }
    }
}
