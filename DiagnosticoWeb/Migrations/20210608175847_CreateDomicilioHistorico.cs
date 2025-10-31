using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DiagnosticoWeb.Migrations
{
    public partial class CreateDomicilioHistorico : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DomicilioHistorico",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 450),
                    DomicilioId = table.Column<string>(maxLength: 450),
                    AgebId = table.Column<string>(maxLength: 450, nullable: true),
                    Telefono = table.Column<string>(maxLength: 20, nullable: true),
                    TelefonoCasa = table.Column<string>(maxLength: 20, nullable: true),
                    Email = table.Column<string>(maxLength: 200, nullable: true),
                    DomicilioN = table.Column<string>(maxLength: 100),
                    EntreCalle1 = table.Column<string>(maxLength: 100),
                    EntreCalle2 = table.Column<string>(maxLength: 10),
                    Latitud = table.Column<string>(maxLength: 100),
                    Longitud = table.Column<string>(maxLength: 100, nullable: true),
                    CodigoPostal = table.Column<string>(maxLength: 450, nullable: true),
                    CreatedAt = table.Column<DateTime>(),
                    UpdatedAt = table.Column<DateTime>(),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    MunicipioId = table.Column<string>(maxLength: 450),
                    LocalidadId = table.Column<string>(maxLength: 450),
                    ZonaImpulsoId = table.Column<string>(maxLength: 450, nullable: true),
                    ManzanaId = table.Column<string>(maxLength: 450, nullable: true),
                    CarreteraId = table.Column<string>(maxLength: 450),
                    CaminoId = table.Column<string>(maxLength: 450),
                    NombreAsentamiento = table.Column<string>(maxLength: 450),
                    TipoAsentamientoId = table.Column<string>(maxLength: 450),
                    CallePosterior = table.Column<string>(maxLength: 450, nullable: true),
                    NumExterior = table.Column<string>(maxLength: 100),
                    NumInterior = table.Column<string>(maxLength: 100, nullable: true),
                    ColoniaId = table.Column<string>(maxLength: 450, nullable: true),
                    CalleId = table.Column<string>(maxLength: 450, nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DomicilioHistorico", x => x.Id);
                    table.ForeignKey(name: "FK_DomicilioHistorico_Domicilio",
                        column: x => x.DomicilioId,
                        principalTable: "Domicilio",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(name: "FK_DomicilioHistorico_Agebs",
                        column: x => x.AgebId,
                        principalTable: "Agebs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(name: "FK_DomicilioHistorico_Municipios",
                        column: x => x.MunicipioId,
                        principalTable: "Municipios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(name: "FK_DomicilioHistorico_Localidades",
                        column: x => x.LocalidadId,
                        principalTable: "Localidades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(name: "FK_DomicilioHistorico_ZonasImpulso",
                        column: x => x.ZonaImpulsoId,
                        principalTable: "ZonasImpulso",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(name: "FK_DomicilioHistorico_Manzanas",
                        column: x => x.ManzanaId,
                        principalTable: "Manzanas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(name: "FK_DomicilioHistorico_Carreteras",
                        column: x => x.CarreteraId,
                        principalTable: "Carreteras",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(name: "FK_DomicilioHistorico_Caminos",
                        column: x => x.CaminoId,
                        principalTable: "Caminos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(name: "FK_DomicilioHistorico_TipoAsentamientos",
                        column: x => x.TipoAsentamientoId,
                        principalTable: "TipoAsentamientos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(name: "FK_DomicilioHistorico_Colonias",
                        column: x => x.ColoniaId,
                        principalTable: "Colonias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(name: "FK_DomicilioHistorico_Calles",
                        column: x => x.CalleId,
                        principalTable: "Calles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "DomicilioHistorico");
        }
    }
}
