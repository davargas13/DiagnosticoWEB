using Microsoft.EntityFrameworkCore.Migrations;

namespace DiagnosticoWeb.Migrations
{
    public partial class AddTrabajadorId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TrabajadorId",
                table: "Beneficiarios",
                nullable: true,
                maxLength: 450);

            migrationBuilder.AddForeignKey(
                name: "FK_Beneficiario_Trabajador",
                table: "Beneficiarios",
                column: "TrabajadorId",
                principalTable: "Trabajadores",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddColumn<string>(
                name: "TrabajadorId",
                table: "Domicilio",
                nullable: true,
                maxLength: 450);

            migrationBuilder.AddForeignKey(
                name: "FK_Domicilio_Trabajador",
                table: "Domicilio",
                column: "TrabajadorId",
                principalTable: "Trabajadores",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddColumn<string>(
                name: "TrabajadorId",
                table: "Aplicaciones",
                nullable: true,
                maxLength: 450);

            migrationBuilder.AddForeignKey(
                name: "FK_Aplicacion_Trabajador",
                table: "Aplicaciones",
                column: "TrabajadorId",
                principalTable: "Trabajadores",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Beneficiario_Trabajador",
                table: "Beneficiarios");

            migrationBuilder.DropColumn(
                name: "TrabajadorId",
                table: "Beneficiarios");

            migrationBuilder.DropForeignKey(
                name: "FK_Domicilio_Trabajador",
                table: "Domicilio");

            migrationBuilder.DropColumn(
                name: "TrabajadorId",
                table: "Domicilio");

            migrationBuilder.DropForeignKey(
                name: "FK_Aplicacion_Trabajador",
                table: "Aplicaciones");

            migrationBuilder.DropColumn(
                name: "TrabajadorId",
                table: "Aplicaciones");
        }
    }
}
