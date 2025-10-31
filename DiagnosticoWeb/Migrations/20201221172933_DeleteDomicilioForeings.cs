using Microsoft.EntityFrameworkCore.Migrations;

namespace DiagnosticoWeb.Migrations
{
    public partial class DeleteDomicilioForeings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "FK_Domicilio_Localidad", table: "Domicilio");
            migrationBuilder.DropForeignKey(name: "FK_Caminos_Domicilio_DomicilioId", table: "Domicilio");
            migrationBuilder.DropForeignKey(name: "FK_Manzanas_Domicilio_DomicilioId", table: "Manzanas");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey(
                name: "FK_Domicilio_Localidad",
                table: "Domicilio",
                column: "LocalidadId",
                principalTable: "Localidades",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
            
            migrationBuilder.AddForeignKey(
                name: "FK_Caminos_Domicilio_DomicilioId",
                table: "Domicilio",
                column: "CaminoId",
                principalTable: "Caminos",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Manzanas_Domicilio_DomicilioId",
                table: "Domicilio",
                column: "ManzanaId",
                principalTable: "Manzanas",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

        }
    }
}
