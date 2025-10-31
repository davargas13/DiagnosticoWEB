using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DiagnosticoWeb.Migrations
{
    public partial class AddTemaCategoria : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>("PadreId", "Carencias", maxLength: 450, nullable: true);
            migrationBuilder.AddForeignKey(
                name: "FK_Carencias_Carencias_PadreId",
                table: "Carencias",
                column: "PadreId",
                principalTable: "Carencias",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
            migrationBuilder.CreateIndex(name: "IX_Carencias_PadreId", table: "Carencias", column: "PadreId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex("IX_Carencias_PadreId", "Carencias");
            migrationBuilder.DropForeignKey("FK_Carencias_Carencias_PadreId", "Carencias");
            migrationBuilder.DropColumn("PadreId", "Carencias");
        }
    }
}
