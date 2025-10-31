using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DiagnosticoWeb.Migrations
{
    public partial class CarenciaPregunta : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                "DependenciaId",
                "AspNetUsers",
                nullable:true
            );
            
            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Dependencias",
                table: "AspNetUsers",
                column: "DependenciaId",
                principalTable: "Dependencias",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
            
            migrationBuilder.AddColumn<string>(
                "CarenciaId",
                "Preguntas",
                nullable:true,
                maxLength: 450
            );
            
            migrationBuilder.AddForeignKey(
                name: "FK_Preguntas_Carencias",
                table: "Preguntas",
                column: "CarenciaId",
                principalTable: "Carencias",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
            
            migrationBuilder.AddColumn<string>(
                "Color",
                "Carencias",
                nullable:true
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn("Color", "Carencias");
            migrationBuilder.DropForeignKey("FK_Preguntas_Carencias", "Carencias");
            migrationBuilder.DropColumn("CarenciaId", "Preguntas");
            migrationBuilder.DropForeignKey("FK_AspNetUsers_Dependencias", "AspNetUsers");
            migrationBuilder.DropColumn("DependenciaId", "AspNetUsers");
        }
    }
}
