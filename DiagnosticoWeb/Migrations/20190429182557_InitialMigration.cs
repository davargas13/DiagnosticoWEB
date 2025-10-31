using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using DiagnosticoWeb.Models;

namespace DiagnosticoWeb.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_AspNetRoles", x => x.Id); });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 450, nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_AspNetUsers", x => x.Id); });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy",
                            SqlServerValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy",
                            SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new {x.LoginProvider, x.ProviderKey});
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new {x.UserId, x.RoleId});
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new {x.UserId, x.LoginProvider, x.Name});
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Trabajadores",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 450, nullable: false),
                    Nombre = table.Column<string>(maxLength: 100, nullable: true),
                    Username = table.Column<string>(maxLength: 50, nullable: true),
                    Password = table.Column<string>(maxLength: 450, nullable: true),
                    Email = table.Column<string>(maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_Trabajadores", x => x.Id); });

            migrationBuilder.CreateTable(
                name: "Dependencias",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 450, nullable: false),
                    Nombre = table.Column<string>(maxLength: 250, nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_Dependencias", x => x.Id); });

            migrationBuilder.CreateTable(
                name: "TrabajadoresDependencias",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 450, nullable: false),
                    DependenciaId = table.Column<string>(maxLength: 450, nullable: false),
                    UsuarioId = table.Column<string>(maxLength: 450, nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrabajadoresDependencias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrabajadoresDependencias_Dependencia",
                        column: x => x.DependenciaId,
                        principalTable: "Dependencias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_TrabajadoresDependencias_Trabajadores",
                        column: x => x.UsuarioId,
                        principalTable: "Trabajadores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "UsuariosDependencias",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 450, nullable: false),
                    DependenciaId = table.Column<string>(maxLength: 450, nullable: false),
                    UsuarioId = table.Column<string>(maxLength: 450, nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuariosDependencias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsuariosDependencias_Dependencia",
                        column: x => x.DependenciaId,
                        principalTable: "Dependencias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_UsuariosDependencias_Trabajadores",
                        column: x => x.UsuarioId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "ProgramasSociales",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 450, nullable: false),
                    Nombre = table.Column<string>(maxLength: 250, nullable: true),
                    DependenciaId = table.Column<string>(maxLength: 450, nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgramasSociales", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProgramasSociales_Dependencias",
                        column: x => x.DependenciaId,
                        principalTable: "Dependencias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Vertientes",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 450, nullable: false),
                    Nombre = table.Column<string>(maxLength: 250, nullable: true),
                    ProgramaSocialId = table.Column<string>(maxLength: 450, nullable: false),
                    Inmediato = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vertientes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vertientes_ProgramasSociales",
                        column: x => x.ProgramaSocialId,
                        principalTable: "ProgramasSociales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Beneficiarios",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 450, nullable: false),
                    ApellidoPaterno = table.Column<string>(maxLength: 256, nullable: true),
                    ApellidoMaterno = table.Column<string>(maxLength: 256, nullable: true),
                    Nombre = table.Column<string>(maxLength: 256, nullable: true),
                    FechaNacimiento = table.Column<DateTime>(nullable: true),
                    Sexo = table.Column<string>(maxLength: 10, nullable: true),
                    EstadoNacimiento = table.Column<string>(maxLength: 50, nullable: true),
                    Curp = table.Column<string>(maxLength: 18, nullable: true),
                    Rfc = table.Column<string>(maxLength: 13, nullable: true),
                    Comentarios = table.Column<string>(maxLength: 450, nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    DomicilioId = table.Column<string>(maxLength: 450, nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Beneficiarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Beneficiarios_Domicilio",
                        column: x => x.DomicilioId,
                        principalTable: "Domicilio",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Archivos",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 450, nullable: false),
                    Nombre = table.Column<string>(maxLength: 450, nullable: true),
                    Base64 = table.Column<string>(nullable: false, maxLength: null, type: "text"),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_Archivos", x => x.Id); });

            migrationBuilder.CreateTable(
                name: "Domicilio",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 450, nullable: false),
                    ClaveAGEB = table.Column<string>(maxLength: 4, nullable: true),
                    Telefono = table.Column<string>(maxLength: 13, nullable: true),
                    TelefonoCasa = table.Column<string>(maxLength: 13, nullable: true),
                    Email = table.Column<string>(nullable: true),
                    DomicilioN = table.Column<string>(maxLength: 100, nullable: true),
                    EntreCalle1 = table.Column<string>(maxLength: 100, nullable: true),
                    EntreCalle2 = table.Column<string>(maxLength: 100, nullable: true),
                    Latitud = table.Column<string>(maxLength:20, nullable: true),
                    Longitud = table.Column<string>(maxLength:20,nullable: true),
                    LatitudAtm = table.Column<string>(maxLength:20,nullable: true),
                    LongitudAtm = table.Column<string>(maxLength:20, nullable: true),
                    CodigoPostal = table.Column<string>(maxLength: 10, nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey(name: "PK_Domicilio", columns: x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Configuracion",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 450, nullable: false),
                    Nombre = table.Column<string>(maxLength:50),
                    Valor = table.Column<string>(maxLength:50),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_Configuracion", columns: x => x.Id); });

            migrationBuilder.CreateTable(
                name: "Encuestas",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 450, nullable: false),
                    Nombre = table.Column<string>(maxLength: 255, nullable: true),
                    Vigencia = table.Column<int>(nullable: true),
                    Activo = table.Column<bool>(maxLength: 255, nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_Encuestas", columns: x => x.Id); });

            migrationBuilder.CreateTable(
                name: "EncuestaVersiones",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 450, nullable: false),
                    Activa = table.Column<bool>(nullable: true),
                    EncuestaId = table.Column<string>(maxLength: 450, nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EncuestaVersiones", columns: x => x.Id);
                    table.ForeignKey(
                        name: "FK_EncuestaVersiones_Encuesta",
                        column: x => x.EncuestaId,
                        principalTable: "Encuestas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Preguntas",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 450, nullable: false),
                    Nombre = table.Column<string>(maxLength: 255, nullable: true),
                    TipoPregunta = table.Column<string>(maxLength: 100, nullable: true),
                    Condicion = table.Column<string>(maxLength: 450, nullable: true),
                    CondicionLista = table.Column<bool>(nullable: true),
                    CondicionIterable = table.Column<string>(maxLength: 450, nullable: true),
                    Iterable = table.Column<bool>(nullable: true),
                    EncuestaVersionId = table.Column<string>(maxLength: 450, nullable: true),
                    Numero = table.Column<int>(nullable: true),
                    Editable = table.Column<bool>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Preguntas", columns: x => x.Id);
                    table.ForeignKey(
                        name: "FK_EncuestaVersion_Pregunta",
                        column: x => x.EncuestaVersionId,
                        principalTable: "EncuestaVersiones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Respuestas",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 450, nullable: false),
                    Nombre = table.Column<string>(maxLength: 255, nullable: true),
                    PreguntaId = table.Column<string>(maxLength: 450, nullable: true),
                    Numero = table.Column<int>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Respuestas", columns: x => x.Id);
                    table.ForeignKey(
                        name: "FK_Preguntas_Respuestas",
                        column: x => x.PreguntaId,
                        principalTable: "Preguntas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Aplicaciones",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 450, nullable: false),
                    Estatus = table.Column<string>(maxLength: 450, nullable: false),
                    BeneficiarioId = table.Column<string>(maxLength: 450, nullable: false),
                    EncuestaVersionId = table.Column<string>(maxLength: 450, nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Aplicaciones", columns: x => x.Id);
                    table.ForeignKey(
                        name: "FK_Aplicaciones_EncuestaVersion",
                        column: x => x.EncuestaVersionId,
                        principalTable: "EncuestaVersiones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Beneficiarios_Aplicaciones",
                        column: x => x.BeneficiarioId,
                        principalTable: "Beneficiarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "AplicacionPreguntas",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 450, nullable: false),
                    Valor = table.Column<string>(maxLength: 255, nullable: true),
                    AplicacionId = table.Column<string>(maxLength: 450, nullable: true),
                    PreguntaId = table.Column<string>(maxLength: 450, nullable: true),
                    RespuestaId = table.Column<string>(maxLength: 450, nullable: true),
                    RespuestaValor = table.Column<double>(nullable: true),
                    RespuestaIteracion = table.Column<string>(maxLength: 2, nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AplicacionPreguntas", columns: x => x.Id);
                    table.ForeignKey(
                        name: "FK_Aplicaciones_AplicacionPreguntas",
                        column: x => x.AplicacionId,
                        principalTable: "Aplicaciones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Preguntas_AplicacionPreguntas",
                        column: x => x.PreguntaId,
                        principalTable: "Preguntas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Respuesta_AplicacionPreguntas",
                        column: x => x.RespuestaId,
                        principalTable: "Respuestas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Solicitudes",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 450, nullable: false),
                    BeneficiarioId = table.Column<string>(maxLength: 450, nullable: false),
                    TrabajadorId = table.Column<string>(maxLength: 450, nullable: false),
                    VertienteId = table.Column<string>(maxLength: 450, nullable: false),
                    AplicacionId = table.Column<string>(maxLength: 450, nullable: false),
                    Estatus = table.Column<string>(maxLength: 450, nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Solicitudes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Beneficiario_Solicitudes",
                        column: x => x.BeneficiarioId,
                        principalTable: "Beneficiarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Trabajador_Solicitudes",
                        column: x => x.TrabajadorId,
                        principalTable: "Trabajadores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Vertientes_Solicitudes",
                        column: x => x.VertienteId,
                        principalTable: "Vertientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Aplicacion_Solicitudes",
                        column: x => x.AplicacionId,
                        principalTable: "Aplicaciones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Dependencias");

            migrationBuilder.DropTable(
                name: "Trabajadores");

            migrationBuilder.DropTable(
                name: "TrabajadoresDependencias");

            migrationBuilder.DropTable(
                name: "ProgramasSociales");

            migrationBuilder.DropTable(
                name: "Vertientes");

            migrationBuilder.DropTable(
                name: "Solicitudes");

            migrationBuilder.DropTable(
                name: "SolicitudesVertientes");

            migrationBuilder.DropTable(
                name: "Archivos");

            migrationBuilder.DropTable(
                name: "Beneficiarios");

            migrationBuilder.DropTable(
                name: "Domicilio");

            migrationBuilder.DropTable(
                name: "Encuestas");

            migrationBuilder.DropTable(
                name: "EncuestaVersiones");

            migrationBuilder.DropTable(
                name: "Preguntas");

            migrationBuilder.DropTable(
                name: "Respuestas");

            migrationBuilder.DropTable(
                name: "Aplicaciones");

            migrationBuilder.DropTable(
                name: "AplicacionPreguntas");

            migrationBuilder.DropTable(
                name: "Configuracion");
        }
    }
}