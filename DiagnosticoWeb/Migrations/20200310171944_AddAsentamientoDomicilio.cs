using Microsoft.EntityFrameworkCore.Migrations;

namespace DiagnosticoWeb.Migrations
{
    public partial class AddAsentamientoDomicilio : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TipoAsentamientoId",
                table: "Domicilio",
                nullable: true,
                maxLength: 450);
            migrationBuilder.AddForeignKey(
                name: "FK_TipoAsentamientos_Domicilio_DomicilioId",
                table: "Domicilio",
                column: "TipoAsentamientoId",
                principalTable: "TipoAsentamientos",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
            migrationBuilder.AddColumn<string>(
                name: "NombreAsentamiento",
                table: "Domicilio",
                nullable: true);
            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NombreAsentamiento",
                table: "Domicilio");
            migrationBuilder.DropForeignKey(
                name: "FK_Manzanas_Domicilio_DomicilioId",
                table: "Domicilio");
            
            migrationBuilder.DropColumn(
                name: "TipoAsentamientoId",
                table: "Domicilio");

        }
    }
}
