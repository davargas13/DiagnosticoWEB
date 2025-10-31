using Microsoft.EntityFrameworkCore.Migrations;

namespace DiagnosticoWeb.Migrations
{
    public partial class AddVersionAlgoritmoAplicaciones : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>("VersionAlgoritmo", "Aplicaciones", defaultValue:1);
            migrationBuilder.AddColumn<int>("VersionAlgoritmo", "AplicacionCarencias", defaultValue:1);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn("VersionAlgoritmo", "Aplicaciones");
            migrationBuilder.DropColumn("VersionAlgoritmo", "AplicacionCarencias");
        }
    }
}
