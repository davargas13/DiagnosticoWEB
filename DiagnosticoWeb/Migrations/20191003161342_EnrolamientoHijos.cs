using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace DiagnosticoWeb.Migrations
{
    public partial class EnrolamientoHijos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Grados",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 450, nullable: false),
                    Nombre = table.Column<string>(maxLength: 256, nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_GradosDiscapacidad", x => x.Id); });

            migrationBuilder.CreateTable(
                name: "DiscapacidadGrado",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 450, nullable: false),
                    DiscapacidadId = table.Column<string>(maxLength: 450, nullable: true),
                    GradoId = table.Column<string>(maxLength: 450, nullable: true),
                    Grado = table.Column<string>(maxLength: 10, nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscapacidadGrado", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiscapacidadGrado_Discapacidades",
                        column: x => x.DiscapacidadId,
                        principalTable: "Discapacidades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_DiscapacidadGrado_Grado",
                        column: x => x.GradoId,
                        principalTable: "Grados",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "CausaDiscapacidad",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 450, nullable: false),
                    Nombre = table.Column<string>(maxLength: 256, nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_Grados", x => x.Id); });

            migrationBuilder.CreateTable(
                name: "GradosEstudio",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 450, nullable: false),
                    Nombre = table.Column<string>(maxLength: 256, nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table => {
                    table.PrimaryKey("PK_GradosEstudio", x => x.Id);
                });

            migrationBuilder.AddColumn<string>(
                name: "PadreId",
                table: "Beneficiarios",
                nullable: true,
                maxLength: 450);

            migrationBuilder.AddForeignKey(
                name: "FK_Beneficiarios_Padre",
                table: "Beneficiarios",
                column: "PadreId",
                principalTable: "Beneficiarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddColumn<bool>(
                name: "MismoDomicilio",
                table: "Beneficiarios",
                nullable: true,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ParentescoId",
                table: "Beneficiarios",
                nullable: true,
                maxLength: 450);

            migrationBuilder.AddForeignKey(
                name: "FK_Beneficiario_Parentesco",
                table: "Beneficiarios",
                column: "ParentescoId",
                principalTable: "Parentescos",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddColumn<string>(
                name: "GradoEstudioId",
                table: "Beneficiarios",
                nullable: true,
                maxLength: 450);

            migrationBuilder.AddForeignKey(
                name: "FK_Beneficiario_GradosEstudio",
                table: "Beneficiarios",
                column: "GradoEstudioId",
                principalTable: "GradosEstudio",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddColumn<string>(
                name: "CausaDiscapacidadId",
                table: "Beneficiarios",
                nullable: true,
                maxLength: 450);

            migrationBuilder.AddForeignKey(
                name: "FK_Beneficiario_CausaDiscapacidad",
                table: "Beneficiarios",
                column: "CausaDiscapacidadId",
                principalTable: "CausaDiscapacidad",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        
            migrationBuilder.AddColumn<string>(
                name: "DiscapacidadGradoId",
                table: "Beneficiarios",
                nullable: true,
                maxLength: 450);

            migrationBuilder.AddForeignKey(
                name: "FK_Beneficiario_DiscapacidadGrado",
                table: "Beneficiarios",
                column: "DiscapacidadGradoId",
                principalTable: "DiscapacidadGrado",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Beneficiario_DiscapacidadGrado",
                table: "Beneficiarios");

            migrationBuilder.DropColumn(
                name: "DiscapacidadGradoId",
                table: "Beneficiarios");

            migrationBuilder.DropForeignKey(
                name: "FK_Beneficiario_CausaDiscapacidad",
                table: "Beneficiarios");

            migrationBuilder.DropColumn(
                name: "CausaDiscapacidadId",
                table: "Beneficiarios");

            migrationBuilder.DropForeignKey(
                name: "FK_Beneficiario_GradosEstudio",
                table: "Beneficiarios");

            migrationBuilder.DropColumn(
                name: "GradoEstudioId",
                table: "Beneficiarios");

            migrationBuilder.DropForeignKey(
                name: "FK_Beneficiario_Parentesco",
                table: "Beneficiarios");

            migrationBuilder.DropColumn(
                name: "ParentescoId",
                table: "Beneficiarios");

            migrationBuilder.DropColumn(
                name: "MismoDomicilio",
                table: "Beneficiarios");

            migrationBuilder.DropForeignKey(
                name: "FK_Beneficiarios_Padre",
                table: "Beneficiarios");

            migrationBuilder.DropColumn(
                name: "PadreId",
                table: "Beneficiarios");

            migrationBuilder.DropTable(name: "GradosEstudio");
            migrationBuilder.DropTable(name: "CausaDiscapacidad");
            migrationBuilder.DropTable(name: "DiscapacidadGrado");
            migrationBuilder.DropTable(name: "Grados");
        }
    }
}
