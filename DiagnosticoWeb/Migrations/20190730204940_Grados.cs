using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DiagnosticoWeb.Migrations
{
    public partial class Grados : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PreguntaGrado",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 450, nullable: false),
                    PreguntaId = table.Column<string>(maxLength: 450, nullable: true),
                    Grado = table.Column<string>(maxLength: 250, nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreguntaGrado", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PreguntaGrado_Preguntas",
                        column: x => x.PreguntaId,
                        principalTable: "Preguntas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RespuestaGrado",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 450, nullable: false),
                    RespuestaId = table.Column<string>(maxLength: 450, nullable: true),
                    PreguntaGradoId = table.Column<string>(maxLength: 450, nullable: true),
                    Grado = table.Column<string>(maxLength: 250, nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RespuestaGrado", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RespuestaGrado_Respuesta",
                        column: x => x.RespuestaId,
                        principalTable: "Respuestas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_RespuestaGrado_PreguntaGrado",
                        column: x => x.PreguntaGradoId,
                        principalTable: "PreguntaGrado",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.AddColumn<bool>(
                "Gradual",
                "Preguntas",
                nullable: true
            );

            migrationBuilder.AddColumn<bool>(
                "Negativa",
                "Respuestas",
                nullable: true
            );
            migrationBuilder.AddColumn<string>(
                "Grado",
                "AplicacionPreguntas",
                maxLength: 20,
                nullable: true
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn("Grado", "AplicacionPregunta");
            migrationBuilder.DropColumn("Negativa", "Respuestas");
            migrationBuilder.DropColumn("Gradual", "Preguntas");
            migrationBuilder.DropTable("RespuestaGrado");
            migrationBuilder.DropTable("PreguntaGrado");
        }
    }
}