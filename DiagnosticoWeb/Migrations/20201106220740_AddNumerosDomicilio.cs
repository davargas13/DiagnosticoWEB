using Microsoft.EntityFrameworkCore.Migrations;

namespace DiagnosticoWeb.Migrations
{
    public partial class AddNumerosDomicilio : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(name: "NumExterior", table: "Domicilio", nullable: true, maxLength:50);
            migrationBuilder.AddColumn<string>(name: "NumInterior", table: "Domicilio", nullable: true, maxLength:20);
            migrationBuilder.AddColumn<int>(name: "NumFamilia", table: "Domicilio", nullable: false, defaultValue:1);
            migrationBuilder.AddColumn<int>(name: "NumFamiliasRegistradas", table: "Domicilio", nullable: false, defaultValue:1);
            migrationBuilder.AddColumn<int>(name: "NumFamilia", table: "Beneficiarios", nullable: false, defaultValue:1);
            migrationBuilder.AddColumn<int>(name: "EstatusVisita", table: "Beneficiarios", nullable: true);
            migrationBuilder.AddColumn<int>(name: "EstatusVisita", table: "BeneficiarioHistorico", nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "NumExterior", table: "Domicilio");
            migrationBuilder.DropColumn(name: "NumInterior", table: "Domicilio");
            migrationBuilder.DropColumn(name: "NumFamilia", table: "Domicilio");
            migrationBuilder.DropColumn(name: "NumFamiliasRegistradas", table: "Domicilio");
            migrationBuilder.DropColumn(name: "NumFamilia", table: "Beneficiarios");
            migrationBuilder.DropColumn(name: "EstatusVisita", table: "Beneficiarios");
            migrationBuilder.DropColumn(name: "EstatusVisita", table: "BeneficiarioHistorico");
        }
    }
}
