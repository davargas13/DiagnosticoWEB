using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace DiagnosticoWeb.Migrations
{
    public partial class AddEstatusBeneficiarios : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        { 
            migrationBuilder.AddColumn<string>(
                name: "EstatusInformacion",
                table: "Beneficiarios",
                nullable: true,
                maxLength: 450);

            migrationBuilder.AddColumn<DateTime>(
                name: "EstatusUpdatedAt",
                table: "Beneficiarios",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EstatusInformacion",
                table: "Beneficiarios");

            migrationBuilder.DropColumn(
                name: "EstatusUpdatedAt",
                table: "Beneficiarios");
        }
    }
}
