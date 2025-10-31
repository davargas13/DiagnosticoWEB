using Microsoft.EntityFrameworkCore.Migrations;

namespace DiagnosticoWeb.Migrations
{
    public partial class TrabajadorDependencia : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrabajadoresDependencias_Trabajadores",
                table: "TrabajadoresDependencias");

            migrationBuilder.RenameColumn(
                name: "UsuarioId",
                table: "TrabajadoresDependencias",
                newName: "TrabajadorId");

            migrationBuilder.AddForeignKey(
                name: "FK_TrabajadoresDependencias_Trabajadores_TrabajadorId",
                table: "TrabajadoresDependencias",
                column: "TrabajadorId",
                principalTable: "Trabajadores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrabajadoresDependencias_Trabajadores_TrabajadorId",
                table: "TrabajadoresDependencias");

            migrationBuilder.RenameColumn(
                name: "TrabajadorId",
                table: "TrabajadoresDependencias",
                newName: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_TrabajadoresDependencias_Trabajadores",
                table: "TrabajadoresDependencias",
                column: "UsuarioId",
                principalTable: "Trabajadores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
