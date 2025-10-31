using System;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DiagnosticoWeb.Migrations
{
    public partial class Catalogos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Estados",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 450, nullable: false),
                    Clave = table.Column<string>(maxLength: 256, nullable: true),
                    Nombre = table.Column<string>(maxLength: 256, nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_Estados", x => x.Id); });

            migrationBuilder.CreateTable(
                name: "Municipios",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 450, nullable: false),
                    Clave = table.Column<string>(maxLength: 256, nullable: true),
                    Nombre = table.Column<string>(maxLength: 256, nullable: true),
                    EstadoId = table.Column<string>(maxLength: 450, nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Municipios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Municipios_Estados",
                        column: x => x.EstadoId,
                        principalTable: "Estados",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Localidades",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 450, nullable: false),
                    Clave = table.Column<string>(maxLength: 256, nullable: true),
                    Nombre = table.Column<string>(maxLength: 256, nullable: true),
                    Indigena = table.Column<string>(maxLength: 256, nullable: true),
                    MunicipioId = table.Column<string>(maxLength: 450, nullable: true),
                    EstadoId = table.Column<string>(maxLength: 450, nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Localidades", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Localidades_Municipios",
                        column: x => x.MunicipioId,
                        principalTable: "Municipios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Localidades_Estados",
                        column: x => x.EstadoId,
                        principalTable: "Estados",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });
            migrationBuilder.CreateTable(
                name: "Agebs",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 450, nullable: false),
                    Clave = table.Column<string>(maxLength: 256, nullable: true),
                    Nombre = table.Column<string>(maxLength: 256, nullable: true),
                    LocalidadId = table.Column<string>(maxLength: 450, nullable: true),
                    MunicipioId = table.Column<string>(maxLength: 450, nullable: true),
                    EstadoId = table.Column<string>(maxLength: 450, nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agebs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Agebs_Localidades",
                        column: x => x.LocalidadId,
                        principalTable: "Localidades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Agebs_Municipios",
                        column: x => x.MunicipioId,
                        principalTable: "Municipios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Agebs_Estados",
                        column: x => x.EstadoId,
                        principalTable: "Estados",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });
            migrationBuilder.CreateTable(
                name: "ZonasImpulso",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 450, nullable: false),
                    Nombre = table.Column<string>(maxLength: 256, nullable: true),
                    MunicipioId = table.Column<string>(maxLength: 450, nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZonasImpulso", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ZonasImpulso_Municipios",
                        column: x => x.MunicipioId,
                        principalTable: "Municipios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
            migrationBuilder.AddColumn<string>(name: "LocalidadId", table: "Beneficiarios", nullable: true,
                maxLength: 450);
            migrationBuilder.AddColumn<string>(name: "MunicipioId", table: "Beneficiarios", nullable: true,
                maxLength: 450);
//            migrationBuilder.AddForeignKey(name: "FK_Beneficiarios_Localidades", table: "Beneficiarios",
//                column: "LocalidadId", principalTable: "Localidades");
//            migrationBuilder.AddForeignKey(name: "FK_Beneficiarios_Municipios", table: "Beneficiarios",
//                column: "MunicipioId", principalTable: "Municipios");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "ZonasImpulso");
            migrationBuilder.DropTable(name: "Agebs");
            migrationBuilder.DropTable(name: "Localidades");
            migrationBuilder.DropTable(name: "Municipios");
            migrationBuilder.DropTable(name: "Estados");
        }
    }
}