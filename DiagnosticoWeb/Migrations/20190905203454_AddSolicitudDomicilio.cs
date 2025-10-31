using Microsoft.EntityFrameworkCore.Migrations;

namespace DiagnosticoWeb.Migrations
{
    public partial class AddSolicitudDomicilio : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DomicilioId",
                table: "Solicitudes",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Solicitudes_DomicilioId",
                table: "Solicitudes",
                column: "DomicilioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Solicitudes_Domicilio_DomicilioId",
                table: "Solicitudes",
                column: "DomicilioId",
                principalTable: "Domicilio",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Solicitudes_Domicilio_DomicilioId",
                table: "Solicitudes");

            migrationBuilder.DropIndex(
                name: "IX_Solicitudes_DomicilioId",
                table: "Solicitudes");

            migrationBuilder.DropColumn(
                name: "DomicilioId",
                table: "Solicitudes");
        }
    }
}
