using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DiagnosticoWeb.Migrations
{
    public partial class crear_perfil_permiso : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Permiso",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 450, nullable: false),
                    Clave = table.Column<string>(maxLength: 450, nullable: true),
                    Nombre = table.Column<string>(maxLength: 450, nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    DeletedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permiso", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Perfil",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 450, nullable: false),
                    Nombre = table.Column<string>(maxLength: 450, nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    DeletedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Perfil", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PerfilPermiso",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 450, nullable: false),
                    PerfilId = table.Column<string>(maxLength: 450, nullable: true),
                    PermisoId = table.Column<string>(maxLength: 450, nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    DeletedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PerfilPermiso", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PerfilPermiso_Perfil",
                        column: x => x.PerfilId,
                        principalTable: "Perfil",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_PerfilPermiso_Permiso",
                        column: x => x.PermisoId,
                        principalTable: "Permiso",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.AddColumn<string>(
                name: "PerfilId",
                table: "AspNetUsers",
                nullable: true,
                maxLength: 450);

            migrationBuilder.AddForeignKey(
                name: "FK_Perfil_Usuarios",
                table: "AspNetUsers",
                column: "PerfilId",
                principalTable: "Perfil",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Perfil_Usuarios",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PerfilId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "PerfilPermiso");

            migrationBuilder.DropTable(
                name: "Perfil");

            migrationBuilder.DropTable(
                name: "Permiso");
        }
    }
}
