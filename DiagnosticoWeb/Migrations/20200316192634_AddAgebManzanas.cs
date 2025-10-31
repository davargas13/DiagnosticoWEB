using Microsoft.EntityFrameworkCore.Migrations;

namespace DiagnosticoWeb.Migrations
{
    public partial class AddAgebManzanas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AgebId",
                table: "Manzanas",
                maxLength:450,
                nullable: true);
            migrationBuilder.AddForeignKey(
                name: "FK_Municipios_Manzanas_MunicipioId",
                table: "Manzanas",
                column: "MunicipioId",
                principalTable: "Municipios",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
            migrationBuilder.AddForeignKey(
                name: "FK_Localidades_Manzanas_LocalidadId",
                table: "Manzanas",
                column: "LocalidadId",
                principalTable: "Localidades",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
            migrationBuilder.AddForeignKey(
                name: "FK_Agebs_Manzanas_AgebId",
                table: "Manzanas",
                column: "AgebId",
                principalTable: "Agebs",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Agebs_Manzanas_AgebId",
                table: "Manzanas"); 
            migrationBuilder.DropForeignKey(
                name: "FK_Localidades_Manzanas_LocalidadId",
                table: "Manzanas"); 
            migrationBuilder.DropForeignKey(
                name: "FK_Municipios_Manzanas_MunicipioId",
                table: "Manzanas");
            migrationBuilder.DropColumn(
                name: "AgebId",
                table: "Manzanas");
        }
    }
}
