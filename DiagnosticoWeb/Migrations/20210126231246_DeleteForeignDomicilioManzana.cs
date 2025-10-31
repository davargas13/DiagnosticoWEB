using Microsoft.EntityFrameworkCore.Migrations;

namespace DiagnosticoWeb.Migrations
{
    public partial class DeleteForeignDomicilioManzana : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey("FK_Manzanas_Domicilio_DomicilioId", "Domicilio");
            migrationBuilder.DropForeignKey("FK_Carreteras_Domicilio_DomicilioId", "Domicilio");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey("FK_Manzanas_Domicilio_DomicilioId","Domicilio","ManzanaId","Manzanas");
            migrationBuilder.AddForeignKey("FK_Carreteras_Domicilio_DomicilioId", "Domicilio", "CarreteraId", "Carreteras");
        }
    }
}
