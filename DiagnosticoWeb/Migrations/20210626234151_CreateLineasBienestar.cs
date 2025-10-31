using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DiagnosticoWeb.Migrations
{
    public partial class CreateLineasBienestar : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LineasBienestar",
                columns: table => new
                {
                    Id = table.Column<int>().Annotation("SqlServer:Identity", "1, 1"),
                    MinimaRural = table.Column<float>(nullable:false, defaultValue:0),
                    Rural = table.Column<float>(nullable:false, defaultValue:0),
                    MinimaUrbana = table.Column<float>(nullable:false, defaultValue:0),
                    Urbana = table.Column<float>(nullable:false, defaultValue:0),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LineasBienestar", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "LineasBienestar");
        }
    }
}
