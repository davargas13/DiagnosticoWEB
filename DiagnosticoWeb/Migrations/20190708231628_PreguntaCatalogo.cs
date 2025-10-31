using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DiagnosticoWeb.Migrations
{
    public partial class PreguntaCatalogo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Catalogo",
                table: "Preguntas",
                nullable: true
            );
            
            migrationBuilder.CreateTable(
                name: "Parentescos",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 450, nullable: false),
                    Nombre = table.Column<string>(maxLength: 256, nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_Parentescos", x => x.Id); });
            
            migrationBuilder.CreateTable(
                name: "Sexos",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 450, nullable: false),
                    Nombre = table.Column<string>(maxLength: 256, nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_Sexos", x => x.Id); });
            
            migrationBuilder.CreateTable(
                name: "Estudios",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 450, nullable: false),
                    Nombre = table.Column<string>(maxLength: 256, nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_Estudios", x => x.Id); });
            
            migrationBuilder.CreateTable(
                name: "Discapacidades",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 450, nullable: false),
                    Nombre = table.Column<string>(maxLength: 256, nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_Discapacidades", x => x.Id); });
            
            migrationBuilder.CreateTable(
                name: "EstadosCiviles",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 450, nullable: false),
                    Nombre = table.Column<string>(maxLength: 256, nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_EstadosCiviles", x => x.Id); });
            
            migrationBuilder.CreateTable(
                name: "Ocupaciones",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 450, nullable: false),
                    Nombre = table.Column<string>(maxLength: 256, nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_Ocupaciones", x => x.Id); });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "Catalogo", table: "Preguuntas");
            migrationBuilder.DropTable("Ocupaciones");
            migrationBuilder.DropTable("EstadosCiviles");
            migrationBuilder.DropTable("Discapacidades");
            migrationBuilder.DropTable("EstadosCiviles");
            migrationBuilder.DropTable("Ocupaciones");
        }
    }
}
