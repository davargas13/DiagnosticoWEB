using Microsoft.EntityFrameworkCore.Migrations;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;

namespace DiagnosticoWeb.Migrations
{
    public partial class ChangeDomicilioSize : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {			
			migrationBuilder.AlterColumn<string>(name: "Telefono",table: "Domicilio");
			migrationBuilder.AlterColumn<string>(name: "TelefonoCasa", table: "Domicilio");
			migrationBuilder.AlterColumn<string>(name: "Latitud", table: "Domicilio");
			migrationBuilder.AlterColumn<string>(name: "Longitud", table: "Domicilio");
			migrationBuilder.AlterColumn<string>(name: "LatitudAtm", table: "Domicilio");
			migrationBuilder.AlterColumn<string>(name: "LongitudAtm", table: "Domicilio");
			migrationBuilder.AlterColumn<string>(name: "CodigoPostal", table: "Domicilio");
			migrationBuilder.AlterColumn<string>(name: "CadenaOCR", table: "Domicilio", nullable:true);
			migrationBuilder.AlterColumn<string>(name: "Porcentaje", table: "Domicilio", nullable:true);
			migrationBuilder.AlterColumn<string>(name: "TipoZap", table: "Domicilio", nullable:true);
			migrationBuilder.AlterColumn<string>(name: "CodigoPostalCalculado", table: "Domicilio", nullable:true);
			migrationBuilder.AlterColumn<string>(name: "indicedesarrollohumano", table: "Domicilio", nullable:true);
			migrationBuilder.AlterColumn<string>(name: "marginacionlocalidad", table: "Domicilio", nullable:true);
			migrationBuilder.AlterColumn<string>(name: "marginacionmunicipio", table: "Domicilio", nullable:true);
			migrationBuilder.AlterColumn<string>(name: "marginacionlocalidad", table: "Domicilio", nullable:true);
			migrationBuilder.AlterColumn<string>(name: "marginacionageb", table: "Domicilio", nullable:true);
			migrationBuilder.AlterColumn<string>(name: "clavemunicipiocalculado", table: "Domicilio", nullable:true);
			migrationBuilder.AlterColumn<string>(name: "clavelocalidadcalculada", table: "Domicilio", nullable:true);
			migrationBuilder.AlterColumn<string>(name: "clavezonaimpulsocalculada", table: "Domicilio", nullable:true);
			migrationBuilder.AddColumn<bool>(name:"Prueba", table:"Beneficiarios", defaultValue:false);
			migrationBuilder.AddColumn<bool>(name: "Prueba", table: "Aplicaciones", defaultValue: false);
			migrationBuilder.AddColumn<bool>(name: "Prueba", table: "Domicilio", defaultValue: false);
		}

        protected override void Down(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.AlterColumn<string>(name: "Telefono", table: "Domicilio", maxLength:13);
			migrationBuilder.AlterColumn<string>(name: "TelefonoCasa", table: "Domicilio", maxLength: 13);
			migrationBuilder.AlterColumn<string>(name: "Latitud", table: "Domicilio", maxLength: 20);
			migrationBuilder.AlterColumn<string>(name: "Longitud", table: "Domicilio", maxLength: 20);
			migrationBuilder.AlterColumn<string>(name: "LatitudAtm", table: "Domicilio", maxLength: 20);
			migrationBuilder.AlterColumn<string>(name: "LongitudAtm", table: "Domicilio", maxLength: 20);
			migrationBuilder.AlterColumn<string>(name: "CodigoPostal", table: "Domicilio", maxLength: 10);
			migrationBuilder.AlterColumn<string>(name: "CadenaOCR", table: "Domicilio", maxLength: 450);
			migrationBuilder.AlterColumn<string>(name: "Porcentaje", table: "Domicilio", maxLength: 450);
			migrationBuilder.AlterColumn<string>(name: "TipoZap", table: "Domicilio", maxLength: 100);
			migrationBuilder.AlterColumn<string>(name: "CodigoPostalCalculado", table: "Domicilio", maxLength: 10);
			migrationBuilder.AlterColumn<string>(name: "indicedesarrollohumano", table: "Domicilio", maxLength: 50);
			migrationBuilder.AlterColumn<string>(name: "marginacionlocalidad", table: "Domicilio", maxLength: 50);
			migrationBuilder.AlterColumn<string>(name: "marginacionmunicipio", table: "Domicilio", maxLength: 50);
			migrationBuilder.AlterColumn<string>(name: "marginacionlocalidad", table: "Domicilio", maxLength: 50);
			migrationBuilder.AlterColumn<string>(name: "marginacionageb", table: "Domicilio", maxLength: 50);
			migrationBuilder.AlterColumn<string>(name: "clavemunicipiocalculado", table: "Domicilio", maxLength: 50);
			migrationBuilder.AlterColumn<string>(name: "clavelocalidadcalculada", table: "Domicilio", maxLength: 50);
			migrationBuilder.AlterColumn<string>(name: "clavezonaimpulsocalculada", table: "Domicilio", maxLength: 50);
			migrationBuilder.DropColumn(name: "Prueba", table: "Beneficiarios");
			migrationBuilder.DropColumn(name: "Prueba", table: "Aplicaciones");
			migrationBuilder.DropColumn(name: "Prueba", table: "Domicilio");
		}
    }
}
