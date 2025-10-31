using Microsoft.EntityFrameworkCore.Migrations;

namespace DiagnosticoWeb.Migrations
{
    public partial class AddManzanaBeneficiario : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ManzanaId",
                table: "Domicilio",
                nullable: true,
                maxLength: 450);

            migrationBuilder.CreateIndex(
                name: "IX_Manzanas_DomicilioId",
                table: "Domicilio",
                column: "ManzanaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Manzanas_Domicilio_DomicilioId",
                table: "Domicilio",
                column: "ManzanaId",
                principalTable: "Manzanas",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
            
            migrationBuilder.AddColumn<string>(
                name: "CarreteraId",
                table: "Domicilio",
                nullable: true,
                maxLength: 450);

            migrationBuilder.CreateIndex(
                name: "IX_Carreteras_DomicilioId",
                table: "Domicilio",
                column: "CarreteraId");

            migrationBuilder.AddForeignKey(
                name: "FK_Carreteras_Domicilio_DomicilioId",
                table: "Domicilio",
                column: "CarreteraId",
                principalTable: "Carreteras",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
            
            migrationBuilder.AddColumn<string>(
                name: "CaminoId",
                table: "Domicilio",
                nullable: true,
                maxLength: 450);

            migrationBuilder.CreateIndex(
                name: "IX_Caminos_DomicilioId",
                table: "Domicilio",
                column: "CaminoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Caminos_Domicilio_DomicilioId",
                table: "Domicilio",
                column: "CaminoId",
                principalTable: "Caminos",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
            
            migrationBuilder.AddColumn<string>(
                name: "NombreAsentamiento",
                table: "Domicilio",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Manzanas_Domicilio_DomicilioId",
                table: "Manzanas");

            migrationBuilder.DropIndex(
                name: "IX_Manzanas_DomicilioId",
                table: "Manzanas");

            migrationBuilder.DropColumn(
                name: "ManzanaId",
                table: "Manzanas");
            
            migrationBuilder.DropForeignKey(
                name: "FK_Carreteras_Domicilio_DomicilioId",
                table: "Carreteras");

            migrationBuilder.DropIndex(
                name: "IX_Carreteras_DomicilioId",
                table: "Carreteras");

            migrationBuilder.DropColumn(
                name: "CarreteraId",
                table: "Carreteras");
            
            migrationBuilder.DropForeignKey(
                name: "FK_Caminos_Domicilio_DomicilioId",
                table: "Caminos");

            migrationBuilder.DropIndex(
                name: "IX_Caminos_DomicilioId",
                table: "Caminos");

            migrationBuilder.DropColumn(
                name: "CaminoId",
                table: "Caminos");
            
            migrationBuilder.DropColumn(
                name: "NombreAsentamiento",
                table: "Caminos");
        }
    }
}
