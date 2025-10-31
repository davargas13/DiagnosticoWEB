using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DiagnosticoWeb.Migrations
{
    public partial class ChaneCoordendasDomciilio : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Latitud",
                table: "Domicilio",
                nullable: true,
                oldClrType: typeof(float),
                oldNullable: true,
                maxLength:15);
            
            migrationBuilder.AlterColumn<string>(
                name: "Longitud",
                table: "Domicilio",
                nullable: true,
                oldClrType: typeof(float),
                oldNullable: true,
                maxLength:15);

            migrationBuilder.AlterColumn<string>(
                name: "LatitudAtm",
                table: "Domicilio",
                nullable: true,
                oldClrType: typeof(float),
                oldNullable: true,
                maxLength:15);
            
            migrationBuilder.AlterColumn<string>(
                name: "LongitudAtm",
                table: "Domicilio",
                nullable: true,
                oldClrType: typeof(float),
                oldNullable: true,
                maxLength:15);

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "Latitud",
                table: "Domicilio",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
            
            migrationBuilder.AlterColumn<float>(
                name: "Longitud",
                table: "Domicilio",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "LatitudAtm",
                table: "Domicilio",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
            
            migrationBuilder.AlterColumn<float>(
                name: "LongitudAtm",
                table: "Domicilio",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
