using Microsoft.EntityFrameworkCore.Migrations;

namespace DiagnosticoWeb.Migrations
{
    public partial class ChangeSizeDomicilioInterior : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "NumInterior",
                table: "Domicilio",
                maxLength: 50, nullable:true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "NumInterior",
                table: "Domicilio",
                maxLength: 20);
        }
    }
}
