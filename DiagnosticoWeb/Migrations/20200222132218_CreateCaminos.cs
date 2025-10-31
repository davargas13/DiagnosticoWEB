using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DiagnosticoWeb.Migrations
{
    public partial class CreateCaminos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Carreteras",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 450, nullable: false),
                    Nombre = table.Column<string>(maxLength: 256, nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carreteras", x => x.Id);
                });
            
            migrationBuilder.CreateTable(
                name: "Caminos",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 450, nullable: false),
                    Nombre = table.Column<string>(maxLength: 256, nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Caminos", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "Carreteras");
            migrationBuilder.DropTable(name: "Caminos");
        }
    }
}
