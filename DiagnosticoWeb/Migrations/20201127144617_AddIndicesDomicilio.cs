using Microsoft.EntityFrameworkCore.Migrations;

namespace DiagnosticoWeb.Migrations
{
    public partial class AddIndicesDomicilio : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(name: "Indice", table: "Domicilio", nullable: true);
            migrationBuilder.AddColumn<string>(name: "IndiceDesarrolloHumano", table: "Domicilio", nullable: true, maxLength:50);
            migrationBuilder.AddColumn<string>(name: "MarginacionMunicipio", table: "Domicilio", nullable: true, maxLength:50);
            migrationBuilder.AddColumn<string>(name: "MarginacionLocalidad", table: "Domicilio", nullable: true, maxLength:50);
            migrationBuilder.AddColumn<string>(name: "MarginacionAgeb", table: "Domicilio", nullable: true, maxLength:50);
            migrationBuilder.AddColumn<string>(name: "ClaveMunicipioCalculado", table: "Domicilio", nullable: true, maxLength:50);
            migrationBuilder.AddColumn<string>(name: "ClaveLocalidadCalculada", table: "Domicilio", nullable: true, maxLength:50);
            migrationBuilder.AddColumn<string>(name: "ClaveMunicipioCisCalculado", table: "Domicilio", nullable: true, maxLength:50);
            migrationBuilder.AddColumn<string>(name: "DomicilioCisCalculado", table: "Domicilio", nullable: true, maxLength:200);
            migrationBuilder.AddColumn<string>(name: "ZonaImpulsoCalculada", table: "Domicilio", nullable: true, maxLength:200);
            migrationBuilder.AddColumn<string>(name: "ClaveZonaImpulsoCalculada", table: "Domicilio", nullable: true, maxLength:50);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn("ClaveZonaImpulsoCalculada", "Domicilio");
            migrationBuilder.DropColumn("ZonaImpulsoCalculada", "Domicilio");
            migrationBuilder.DropColumn("DomicilioCisCalculado", "Domicilio");
            migrationBuilder.DropColumn("ClaveMunicipioCisCalculado", "Domicilio");
            migrationBuilder.DropColumn("ClaveLocalidadCalculada", "Domicilio");
            migrationBuilder.DropColumn("ClaveMunicipioCalculado", "Domicilio");
            migrationBuilder.DropColumn("MarginacionAgeb", "Domicilio");
            migrationBuilder.DropColumn("MarginacionLocalidad", "Domicilio");
            migrationBuilder.DropColumn("MarginacionMunicipio", "Domicilio");
            migrationBuilder.DropColumn("IndiceDesarrolloHumano", "Domicilio");
            migrationBuilder.DropColumn("Indice", "Domicilio");
        }
    }
}
