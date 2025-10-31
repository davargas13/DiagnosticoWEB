using Microsoft.EntityFrameworkCore.Migrations;

namespace DiagnosticoWeb.Migrations
{
    public partial class AddNumCarencias : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(name: "NumeroEducativas", table: "Aplicaciones", nullable: false, defaultValue: 0);
            migrationBuilder.AddColumn<int>(name: "NumeroSalud", table: "Aplicaciones", nullable: false, defaultValue: 0);
            migrationBuilder.AddColumn<int>(name: "NumeroSocial", table: "Aplicaciones", nullable: false, defaultValue: 0);
            migrationBuilder.AddColumn<int>(name: "NumeroCarencias", table: "Aplicaciones", nullable: false, defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "NumeroCarencias", table: "Aplicaciones");
            migrationBuilder.DropColumn(name: "NumeroSocial", table: "Aplicaciones");
            migrationBuilder.DropColumn(name: "NumeroSalud", table: "Aplicaciones");
            migrationBuilder.DropColumn(name: "NumeroEducativas", table: "Aplicaciones");
        }
    }
}
