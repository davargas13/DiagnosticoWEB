using Microsoft.EntityFrameworkCore.Migrations;

namespace DiagnosticoWeb.Migrations
{
    public partial class BeneficiarioSexoId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sexo", 
                table: "Beneficiarios");

            migrationBuilder.AddColumn<string>(
                name: "SexoId",
                table: "Beneficiarios",
                nullable: true,
                maxLength: 450);

            migrationBuilder.AddForeignKey(
                name: "FK_Beneficiario_Sexo",
                table: "Beneficiarios",
                column: "SexoId",
                principalTable: "Sexos",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Beneficiario_Sexo",
                table: "Beneficiarios");

            migrationBuilder.DropColumn(
                name: "SexoId",
                table: "Beneficiarios");

            migrationBuilder.AddColumn<string>(
                name: "Sexo",
                table: "Beneficiarios",
                nullable: true,
                maxLength: 10);
        }
    }
}
