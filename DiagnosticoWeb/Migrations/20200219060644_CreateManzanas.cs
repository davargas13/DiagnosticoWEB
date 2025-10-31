using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DiagnosticoWeb.Migrations
{
    public partial class CreateManzanas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Manzanas",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 450, nullable: false),
                    Nombre = table.Column<string>(maxLength: 256, nullable: true),
                    LocalidadId = table.Column<string>(maxLength: 256, nullable: true),
                    MunicipioId = table.Column<string>(maxLength: 256, nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Manzanas", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "Manzanas");
        }
    }
}
