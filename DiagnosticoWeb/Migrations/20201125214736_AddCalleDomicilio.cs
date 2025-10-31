using System;
using Microsoft.EntityFrameworkCore.Migrations;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;

namespace DiagnosticoWeb.Migrations
{
    public partial class AddCalleDomicilio : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Colonias",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 450, nullable: false),
                    Nombre = table.Column<string>(maxLength: 256),
                    CodigoPostal = table.Column<string>(maxLength: 5),
                    MunicipioId = table.Column<string>(maxLength: 450),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Colonias", x => x.Id);
                    table.ForeignKey(name: "FK_Colonias_Municipio", column: x => x.MunicipioId, principalTable: "Municipios", principalColumn: "Id");
                });
            migrationBuilder.CreateIndex(name: "IX_Colonias_MunicipioId", table: "Colonias", column: "MunicipioId");

            migrationBuilder.CreateTable(
                name: "Calles",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 450, nullable: false),
                    Nombre = table.Column<string>(maxLength: 256),
                    MunicipioId = table.Column<string>(maxLength: 450),
                    LocalidadId = table.Column<string>(maxLength: 450),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Calles", x => x.Id);
                    table.ForeignKey(name: "FK_Calles_Municipio", column: x => x.MunicipioId, principalTable: "Municipios", principalColumn: "Id");
                    table.ForeignKey(name: "FK_Calles_Localidad", column: x => x.LocalidadId, principalTable: "Localidades", principalColumn: "Id");
                });
            migrationBuilder.CreateIndex(name: "IX_Calles_MunicipioId", table: "Calles", column: "MunicipioId");
            migrationBuilder.CreateIndex(name: "IX_Calles_LocalidadId", table: "Calles", column: "LocalidadId");

            migrationBuilder.AddColumn<string>("ColoniaId", "Domicilio", maxLength: 450, nullable: true);
            migrationBuilder.AddColumn<string>("CalleId", "Domicilio", maxLength: 450, nullable: true);

            migrationBuilder.CreateIndex(name: "IX_Colonias_DomicilioId", table: "Domicilio", column: "ColoniaId");
            migrationBuilder.CreateIndex(name: "IX_Calles_DomicilioId", table: "Domicilio", column: "CalleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Colonias_Domicilio_ColoniaId",
                table: "Domicilio",
                column: "ColoniaId",
                principalTable: "Colonias",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Calles_Domicilio_CalleId",
                table: "Domicilio",
                column: "CalleId",
                principalTable: "Calles",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.RenameColumn("Colonia", "Domicilio", "ColoniaCalculada");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn("ColoniaCalculada", "Domicilio", "Colonia");
            migrationBuilder.DropForeignKey("FK_Calles_Domicilio_CalleId","Domicilio");
            migrationBuilder.DropForeignKey("FK_Colonias_Domicilio_ColoniaId","Domicilio");
            migrationBuilder.DropIndex("IX_Calles_DomicilioId", "Domicilio");
            migrationBuilder.DropIndex("IX_Colonias_DomicilioId", "Domicilio");
            migrationBuilder.DropColumn("CalleId", "Domicilio");
            migrationBuilder.DropColumn("ColoniaId", "Domicilio");
            migrationBuilder.DropTable(name: "Colonias");
            migrationBuilder.DropTable(name: "Calles");
        }
    }
}
