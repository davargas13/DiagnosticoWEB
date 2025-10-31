using Microsoft.EntityFrameworkCore.Migrations;

namespace DiagnosticoWeb.Migrations
{
    public partial class AddDomicilioCalculados : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MunicipioCalculado",
                table: "Domicilio",
                nullable: true);
            migrationBuilder.AddColumn<string>(
                name: "LocalidadCalculado",
                table: "Domicilio",
                nullable: true);
            migrationBuilder.AddColumn<string>(
                name: "AgebCalculado",
                table: "Domicilio",
                nullable: true);
            migrationBuilder.AddColumn<string>(
                name: "ManzanaCalculado",
                table: "Domicilio",
                nullable: true);
            migrationBuilder.AddColumn<string>(
                name: "ZapCalculado",
                table: "Domicilio",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MunicipioCalculado",
                table: "Domicilio");
            migrationBuilder.DropColumn(
                name: "LocalidadCalculado",
                table: "Domicilio");
            migrationBuilder.DropColumn(
                name: "AgebCalculado",
                table: "Domicilio");
            migrationBuilder.DropColumn(
                name: "ManzanaCalculado",
                table: "Domicilio");
            migrationBuilder.DropColumn(
                name: "ZapCalculado",
                table: "Domicilio");
        }
    }
}
