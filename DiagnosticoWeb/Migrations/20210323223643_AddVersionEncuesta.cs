using Microsoft.EntityFrameworkCore.Migrations;

namespace DiagnosticoWeb.Migrations
{
    public partial class AddVersionEncuesta : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VersionAplicacion",
                table: "Domicilio",
                maxLength:10,
                nullable: true);
            migrationBuilder.AddColumn<string>(
                name: "VersionAplicacion",
                table: "Beneficiarios",
                maxLength:10,
                nullable: true);
            migrationBuilder.AddColumn<string>(
                name: "VersionAplicacion",
                table: "Aplicaciones",
                maxLength:10,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn("VersionAplicacion", "Aplicaciones");
            migrationBuilder.DropColumn("VersionAplicacion", "Beneficiarios");
            migrationBuilder.DropColumn("VersionAplicacion", "Domicilio");
        }
    }
}
