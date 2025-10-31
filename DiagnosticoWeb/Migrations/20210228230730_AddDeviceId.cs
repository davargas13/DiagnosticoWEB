using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace DiagnosticoWeb.Migrations
{
    public partial class AddDeviceId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(name: "DeviceId", table: "Beneficiarios", nullable: true, maxLength:100);
            migrationBuilder.AddColumn<string>(name: "DeviceId", table: "Domicilio", nullable: true, maxLength:100);
            migrationBuilder.AddColumn<string>(name: "DeviceId", table: "Aplicaciones", nullable: true, maxLength:100);
            migrationBuilder.AddColumn<string>(name: "DeviceId", table: "Incidencias", nullable: true, maxLength:100);
            migrationBuilder.AddColumn<int>(name: "NumIntegrante", table: "AplicacionPreguntas", nullable: false, defaultValue:0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "NumIntegrante", table: "AplicacionPreguntas");
            migrationBuilder.DropColumn(name: "DeviceId", table: "Incidencias");
            migrationBuilder.DropColumn(name: "DeviceId", table: "Aplicaciones");
            migrationBuilder.DropColumn(name: "DeviceId", table: "Domicilio");
            migrationBuilder.DropColumn(name: "DeviceId", table: "Beneficiarios");
        }
    }
}
