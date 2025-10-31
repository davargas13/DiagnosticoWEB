using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DiagnosticoWeb.Migrations
{
    public partial class AddBeneficiariosDiscapacidades : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BeneficiarioDiscapacidades",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 450, nullable: false),
                    BeneficiarioId = table.Column<string>(maxLength: 450, nullable: true),
                    DiscapacidadId = table.Column<string>(maxLength: 450, nullable: true),
                    GradoId = table.Column<string>(maxLength: 450, nullable: true),
                    CausaId = table.Column<string>(maxLength: 450, nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BeneficiarioDiscapacidades", x => x.Id);
                    table.ForeignKey(name: "FK_BeneficiarioDiscapacidades_Respuestas_DiscapacidadId",
                        column: x => x.DiscapacidadId, principalTable: "Respuestas", principalColumn: "Id", onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(name: "FK_BeneficiarioDiscapacidades_RespuestaGrado_GradoId",
                        column: x => x.GradoId, principalTable: "RespuestaGrado", principalColumn: "Id", onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(name: "FK_BeneficiarioDiscapacidades_CausaDiscapacidad_CausaId",
                        column: x => x.CausaId, principalTable: "CausaDiscapacidad", principalColumn: "Id", onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(name: "FK_BeneficiarioDiscapacidades_Beneficiarios_BeneficiarioId",
                        column: x => x.BeneficiarioId, principalTable: "Beneficiarios", principalColumn: "Id", onDelete: ReferentialAction.NoAction);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "BeneficiarioDiscapacidades");
        }
    }
}
