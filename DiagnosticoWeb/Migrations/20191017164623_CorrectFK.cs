using Microsoft.EntityFrameworkCore.Migrations;

namespace DiagnosticoWeb.Migrations
{
    public partial class CorrectFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Beneficiario_Discapacidad",
                table: "Beneficiarios");

            migrationBuilder.AddForeignKey(
                name: "FK_Beneficiario_Discapacidad",
                table: "Beneficiarios",
                column: "DiscapacidadId",
                principalTable: "Discapacidades",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
        }
    }
}
