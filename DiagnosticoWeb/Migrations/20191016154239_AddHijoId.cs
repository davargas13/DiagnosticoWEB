using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DiagnosticoWeb.Migrations
{
    public partial class AddHijoId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HijoId",
                table: "Beneficiarios",
                nullable: true,
                maxLength: 450);

            migrationBuilder.AddForeignKey(
                name: "FK_Beneficiarios_Hijo",
                table: "Beneficiarios",
                column: "HijoId",
                principalTable: "Beneficiarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Beneficiarios_Hijo",
                table: "Beneficiarios");

            migrationBuilder.DropColumn(
                name: "HijoId",
                table: "Beneficiarios");
        }
    }
}
