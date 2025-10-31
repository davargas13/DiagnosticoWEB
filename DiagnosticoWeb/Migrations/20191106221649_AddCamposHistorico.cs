using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace DiagnosticoWeb.Migrations
{
    public partial class AddCamposHistorico : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EstatusInformacion",
                table: "BeneficiarioHistorico",
                nullable: true,
                maxLength: 450);

            migrationBuilder.AddColumn<DateTime>(
                name: "EstatusUpdatedAt",
                table: "BeneficiarioHistorico",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GradoEstudioId",
                table: "BeneficiarioHistorico",
                nullable: true,
                maxLength: 450);

            migrationBuilder.AddForeignKey(
                name: "FK_BeneficiarioHistorico_GradosEstudio",
                table: "BeneficiarioHistorico",
                column: "GradoEstudioId",
                principalTable: "GradosEstudio",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddColumn<string>(
                name: "CausaDiscapacidadId",
                table: "BeneficiarioHistorico",
                nullable: true,
                maxLength: 450);

            migrationBuilder.AddForeignKey(
                name: "FK_BeneficiarioHistorico_CausaDiscapacidad",
                table: "BeneficiarioHistorico",
                column: "CausaDiscapacidadId",
                principalTable: "CausaDiscapacidad",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddColumn<string>(
                name: "DiscapacidadGradoId",
                table: "BeneficiarioHistorico",
                nullable: true,
                maxLength: 450);

            migrationBuilder.AddForeignKey(
                name: "FK_BeneficiarioHistorico_DiscapacidadGrado",
                table: "BeneficiarioHistorico",
                column: "DiscapacidadGradoId",
                principalTable: "DiscapacidadGrado",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);


        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BeneficiarioHistorico_DiscapacidadGrado",
                table: "BeneficiarioHistorico");

            migrationBuilder.DropColumn(
                name: "DiscapacidadGradoId",
                table: "BeneficiarioHistorico");

            migrationBuilder.DropForeignKey(
                name: "FK_Beneficiario_CausaDiscapacidad",
                table: "BeneficiarioHistorico");

            migrationBuilder.DropColumn(
                name: "CausaDiscapacidadId",
                table: "BeneficiarioHistorico");

            migrationBuilder.DropForeignKey(
                name: "FK_BeneficiarioHistorico_GradosEstudio",
                table: "BeneficiarioHistorico");

            migrationBuilder.DropColumn(
                name: "GradoEstudioId",
                table: "BeneficiarioHistorico");

            migrationBuilder.DropColumn(
                name: "EstatusInformacion",
                table: "BeneficiarioHistorico");

            migrationBuilder.DropColumn(
                name: "EstatusUpdatedAt",
                table: "BeneficiarioHistorico");
        }
    }
}
