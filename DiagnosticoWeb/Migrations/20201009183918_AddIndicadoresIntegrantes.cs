using Microsoft.EntityFrameworkCore.Migrations;

namespace DiagnosticoWeb.Migrations
{
    public partial class AddIndicadoresIntegrantes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<short>(name: "SeguridadPublica", table: "Aplicaciones", nullable: false, defaultValue: 0);
            migrationBuilder.AddColumn<short>(name: "SeguridadParque", table: "Aplicaciones", nullable: false, defaultValue: 0);
            migrationBuilder.AddColumn<short>(name: "ConfianzaLiderez", table: "Aplicaciones", nullable: false, defaultValue: 0);
            migrationBuilder.AddColumn<short>(name: "ConfianzaInstituciones", table: "Aplicaciones", nullable: false, defaultValue: 0);
            migrationBuilder.AddColumn<short>(name: "Movilidad", table: "Aplicaciones", nullable: false, defaultValue: 0);
            migrationBuilder.AddColumn<short>(name: "RedesSociales", table: "Aplicaciones", nullable: false, defaultValue: 0);
            migrationBuilder.AddColumn<short>(name: "Tejido", table: "Aplicaciones", nullable: false, defaultValue: 0);
            migrationBuilder.AddColumn<short>(name: "Satisfaccion", table: "Aplicaciones", nullable: false, defaultValue: 0);
            migrationBuilder.AddColumn<bool>(name: "PropiedadVivienda", table: "Aplicaciones", nullable: false, defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "SeguridadPublica", table: "Aplicaciones");
            migrationBuilder.DropColumn(name: "SeguridadParque", table: "Aplicaciones");
            migrationBuilder.DropColumn(name: "ConfianzaLiderez", table: "Aplicaciones");
            migrationBuilder.DropColumn(name: "ConfianzaInstituciones", table: "Aplicaciones");
            migrationBuilder.DropColumn(name: "Movilidad", table: "Aplicaciones");
            migrationBuilder.DropColumn(name: "RedesSociales", table: "Aplicaciones");
            migrationBuilder.DropColumn(name: "Tejido", table: "Aplicaciones");
            migrationBuilder.DropColumn(name: "Satisfaccion", table: "Aplicaciones");
            migrationBuilder.DropColumn(name: "PropiedadVivienda", table: "Aplicaciones");
        }
    }
}
