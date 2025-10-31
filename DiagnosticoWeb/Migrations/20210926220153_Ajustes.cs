using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DiagnosticoWeb.Migrations
{
    public partial class Ajustes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>("DeletedAt","AlgoritmoVersiones",nullable: true);
            migrationBuilder.AddColumn<DateTime>("DeletedAt","AlgoritmoResultados",nullable: true);
            migrationBuilder.AddForeignKey("FK_AlgoritmoIntegranteResultados_Parentescos_ParentescoId",
                "AlgoritmoIntegranteResultados", "ParentescoId", "Parentescos",
                principalColumn: "Id", onDelete: ReferentialAction.NoAction);
            migrationBuilder.AddForeignKey("FK_AlgoritmoIntegranteResultados_Sexos_SexoId",
                "AlgoritmoIntegranteResultados", "SexoId", "Sexos",
                principalColumn: "Id", onDelete: ReferentialAction.NoAction);
            migrationBuilder.AddForeignKey("FK_AplicacionCarencias_Beneficiarios_BeneficiarioId",
                "AplicacionCarencias", "BeneficiarioId", "Beneficiarios",
                principalColumn: "Id", onDelete: ReferentialAction.NoAction);
            migrationBuilder.AddForeignKey("FK_AplicacionCarencias_Parentescos_ParentescoId",
                "AplicacionCarencias", "ParentescoId", "Parentescos",
                principalColumn: "Id", onDelete: ReferentialAction.NoAction);
            migrationBuilder.AddForeignKey("FK_AplicacionCarencias_Sexos_SexoId",
                "AplicacionCarencias", "SexoId", "Sexos",
                principalColumn: "Id", onDelete: ReferentialAction.NoAction);
            migrationBuilder.AddForeignKey("FK_BeneficiarioHistorico_Domicilio_DomicilioId",
                "BeneficiarioHistorico", "DomicilioId", "Domicilio",
                principalColumn: "Id", onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey("FK_BeneficiarioHistorico_Domicilio_DomicilioId","BeneficiarioHistorico");
            migrationBuilder.DropForeignKey("FK_AplicacionCarencias_Sexos_SexoId","AplicacionCarencias");
            migrationBuilder.DropForeignKey("FK_AplicacionCarencias_Parentescos_ParentescoId","AplicacionCarencias");
            migrationBuilder.DropForeignKey("FK_AplicacionCarencias_Beneficiarios_BeneficiarioId","AplicacionCarencias");
            migrationBuilder.DropForeignKey("FK_AlgoritmoIntegranteResultados_Sexos_SexoId","AlgoritmoIntegranteResultados");
            migrationBuilder.DropForeignKey("FK_AlgoritmoIntegranteResultados_Parentescos_ParentescoId","AlgoritmoIntegranteResultados");
            migrationBuilder.DropColumn("DeletedAt","AlgoritmoVersiones");
            migrationBuilder.DropColumn("DeletedAt","AlgoritmoResultados");
        }
    }
}
