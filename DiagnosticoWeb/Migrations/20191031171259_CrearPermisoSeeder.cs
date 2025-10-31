using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DiagnosticoWeb.Migrations
{
    public partial class CrearPermisoSeeder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var now = DateTime.Now;
            var permisos = new string[] {
                "Id", "Clave", "Nombre", "CreatedAt", "UpdatedAt"
            };
            var perfil = new string[] {
                "Id", "Nombre", "CreatedAt", "UpdatedAt"
            };
            var perfilPermiso = new string[] {
                "Id", "PerfilId", "PermisoId", "CreatedAt", "UpdatedAt"
            };
            //Inserción de permisos
            migrationBuilder.InsertData(table: "Permiso",
                columns: permisos,
                values: new object[] {
                    "1", "catalogos.ver", "Ver catálogos iniciales", now, now
                });
            migrationBuilder.InsertData(table: "Permiso",
                columns: permisos,
                values: new object[] {
                    "2", "catalogos.exportar", "Exportar catálogos iniciales", now, now
                });
            migrationBuilder.InsertData(table: "Permiso",
                columns: permisos,
                values: new object[] {
                    "3", "catalogos.importar", "Importar catálogos iniciales", now, now
                });
            migrationBuilder.InsertData(table: "Permiso",
                columns: permisos,
                values: new object[] {
                    "4", "catalogos.crear", "Crear catálogos iniciales", now, now
                });
            migrationBuilder.InsertData(table: "Permiso",
                columns: permisos,
                values: new object[] {
                    "5", "catalogos.editar", "Editar catálogos iniciales", now, now
                });
            migrationBuilder.InsertData(table: "Permiso",
                columns: permisos,
                values: new object[] {
                    "6", "trabajadorcampo.crear", "Crear trabajador de campo", now, now
                });
            migrationBuilder.InsertData(table: "Permiso",
                columns: permisos,
                values: new object[] {
                    "7", "trabajadorcampo.editar", "Editar trabajador de campo", now, now
                });
            migrationBuilder.InsertData(table: "Permiso",
                columns: permisos,
                values: new object[] {
                    "8", "trabajadorcampo.ver", "Ver trabajador de campo", now, now
                });
            migrationBuilder.InsertData(table: "Permiso",
                columns: permisos,
                values: new object[] {
                    "9", "trabajadorcampo.eliminar", "Eliminar trabajador de campo", now, now
                });
            migrationBuilder.InsertData(table: "Permiso",
                columns: permisos,
                values: new object[] {
                    "10", "dependencias.ver", "Ver catálogo de dependencias", now, now
                });
            migrationBuilder.InsertData(table: "Permiso",
                columns: permisos,
                values: new object[] {
                    "11", "dependencias.crear", "Crear dependencia", now, now
                });
            migrationBuilder.InsertData(table: "Permiso",
                columns: permisos,
                values: new object[] {
                    "12", "dependencias.editar", "Editar dependencia", now, now
                });
            migrationBuilder.InsertData(table: "Permiso",
                columns: permisos,
                values: new object[] {
                    "13", "programasocial.ver", "Ver catálogo de programas sociales", now, now
                });
            migrationBuilder.InsertData(table: "Permiso",
                columns: permisos,
                values: new object[] {
                    "14", "programasocial.editar", "Editar programa social", now, now
                });
            migrationBuilder.InsertData(table: "Permiso",
                columns: permisos,
                values: new object[] {
                    "15", "programasocial.crear", "Crear programa social", now, now
                });
            migrationBuilder.InsertData(table: "Permiso",
                columns: permisos,
                values: new object[] {
                    "16", "usuarios.ver", "Ver catálogo de usuarios", now, now
                });
            migrationBuilder.InsertData(table: "Permiso",
                columns: permisos,
                values: new object[] {
                    "17", "usuarios.crear", "Crear usuario", now, now
                });
            migrationBuilder.InsertData(table: "Permiso",
                columns: permisos,
                values: new object[] {
                    "18", "usuarios.editar", "Editar usuario", now, now
                });
            migrationBuilder.InsertData(table: "Permiso",
                columns: permisos,
                values: new object[] {
                    "19", "encuesta.ver", "Ver encuesta", now, now
                });
            migrationBuilder.InsertData(table: "Permiso",
                columns: permisos,
                values: new object[] {
                    "20", "encuesta.editar", "Editar encuesta", now, now
                });
            migrationBuilder.InsertData(table: "Permiso",
                columns: permisos,
                values: new object[] {
                    "21", "beneficiarios.ver", "Ver catálogo de beneficiarios", now, now
                }); 
            migrationBuilder.InsertData(table: "Permiso",
                columns: permisos,
                values: new object[] {
                    "22", "beneficiarios.editar", "Editar beneficiario", now, now
                });
            migrationBuilder.InsertData(table: "Permiso",
                columns: permisos,
                values: new object[] {
                    "23", "beneficiarios.exportar", "Exportar catálogo de beneficiarios", now, now
                });
            migrationBuilder.InsertData(table: "Permiso",
                columns: permisos,
                values: new object[] {
                    "24", "solicitudes.ver", "Ver solicitudes", now, now
                });
            migrationBuilder.InsertData(table: "Permiso",
                columns: permisos,
                values: new object[] {
                    "25", "solicitudes.editar", "Dictaminar solicitudes", now, now
                });
            migrationBuilder.InsertData(table: "Permiso",
                columns: permisos,
                values: new object[] {
                    "26", "solicitudes.exportar", "Exportar catálogo de solicitudes", now, now
                });
            migrationBuilder.InsertData(table: "Permiso",
                columns: permisos,
                values: new object[] {
                    "27", "perfil.ver", "Ver catálogo de perfiles", now, now
                });
            migrationBuilder.InsertData(table: "Permiso",
                columns: permisos,
                values: new object[] {
                    "28", "perfil.crear", "Crear perfil", now, now
                });
            migrationBuilder.InsertData(table: "Permiso",
                columns: permisos,
                values: new object[] {
                    "29", "perfil.editar", "Editar perfil", now, now
                });

            //Inserción de perfil
            migrationBuilder.InsertData(table: "Perfil",
                columns: perfil,
                values: new object[] {
                    "1", "Administrador", now, now
                });

            //Inserción relación perfil-permiso
            migrationBuilder.InsertData(table: "PerfilPermiso",
                columns: perfilPermiso,
                values: new object[] {
                    "1", "1", "1", now, now
                });
            migrationBuilder.InsertData(table: "PerfilPermiso",
                columns: perfilPermiso,
                values: new object[] {
                    "2", "1", "2", now, now
                });
            migrationBuilder.InsertData(table: "PerfilPermiso",
                columns: perfilPermiso,
                values: new object[] {
                    "3", "1", "3", now, now
                });
            migrationBuilder.InsertData(table: "PerfilPermiso",
                columns: perfilPermiso,
                values: new object[] {
                    "4", "1", "4", now, now
                });
            migrationBuilder.InsertData(table: "PerfilPermiso",
                columns: perfilPermiso,
                values: new object[] {
                    "5", "1", "5", now, now
                });
            migrationBuilder.InsertData(table: "PerfilPermiso",
                columns: perfilPermiso,
                values: new object[] {
                    "6", "1", "6", now, now
                });
            migrationBuilder.InsertData(table: "PerfilPermiso",
                columns: perfilPermiso,
                values: new object[] {
                    "7", "1", "7", now, now
                });
            migrationBuilder.InsertData(table: "PerfilPermiso",
                columns: perfilPermiso,
                values: new object[] {
                    "8", "1", "8", now, now
                });
            migrationBuilder.InsertData(table: "PerfilPermiso",
                columns: perfilPermiso,
                values: new object[] {
                    "9", "1", "9", now, now
                });
            migrationBuilder.InsertData(table: "PerfilPermiso",
                columns: perfilPermiso,
                values: new object[] {
                    "10", "1", "10", now, now
                });
            migrationBuilder.InsertData(table: "PerfilPermiso",
                columns: perfilPermiso,
                values: new object[] {
                    "11", "1", "11", now, now
                });
            migrationBuilder.InsertData(table: "PerfilPermiso",
                columns: perfilPermiso,
                values: new object[] {
                    "12", "1", "12", now, now
                });
            migrationBuilder.InsertData(table: "PerfilPermiso",
                columns: perfilPermiso,
                values: new object[] {
                    "13", "1", "13", now, now
                });
            migrationBuilder.InsertData(table: "PerfilPermiso",
                columns: perfilPermiso,
                values: new object[] {
                    "14", "1", "14", now, now
                });
            migrationBuilder.InsertData(table: "PerfilPermiso",
                columns: perfilPermiso,
                values: new object[] {
                    "15", "1", "15", now, now
                });
            migrationBuilder.InsertData(table: "PerfilPermiso",
                columns: perfilPermiso,
                values: new object[] {
                    "16", "1", "16", now, now
                });
            migrationBuilder.InsertData(table: "PerfilPermiso",
                columns: perfilPermiso,
                values: new object[] {
                    "17", "1", "17", now, now
                });
            migrationBuilder.InsertData(table: "PerfilPermiso",
                columns: perfilPermiso,
                values: new object[] {
                    "18", "1", "18", now, now
                });
            migrationBuilder.InsertData(table: "PerfilPermiso",
                columns: perfilPermiso,
                values: new object[] {
                    "19", "1", "19", now, now
                });
            migrationBuilder.InsertData(table: "PerfilPermiso",
                columns: perfilPermiso,
                values: new object[] {
                    "20", "1", "20", now, now
                });
            migrationBuilder.InsertData(table: "PerfilPermiso",
                columns: perfilPermiso,
                values: new object[] {
                    "21", "1", "21", now, now
                });
            migrationBuilder.InsertData(table: "PerfilPermiso",
                columns: perfilPermiso,
                values: new object[] {
                    "22", "1", "22", now, now
                });
            migrationBuilder.InsertData(table: "PerfilPermiso",
                columns: perfilPermiso,
                values: new object[] {
                    "23", "1", "23", now, now
                });
            migrationBuilder.InsertData(table: "PerfilPermiso",
                 columns: perfilPermiso,
                 values: new object[] {
                    "24", "1", "24", now, now
                 });
            migrationBuilder.InsertData(table: "PerfilPermiso",
                 columns: perfilPermiso,
                 values: new object[] {
                    "25", "1", "25", now, now
                 });
            migrationBuilder.InsertData(table: "PerfilPermiso",
                 columns: perfilPermiso,
                 values: new object[] {
                    "26", "1", "26", now, now
                 });
            migrationBuilder.InsertData(table: "PerfilPermiso",
                 columns: perfilPermiso,
                 values: new object[] {
                    "27", "1", "27", now, now
                 });
            migrationBuilder.InsertData(table: "PerfilPermiso",
                 columns: perfilPermiso,
                 values: new object[] {
                    "28", "1", "28", now, now
                 });
            migrationBuilder.InsertData(table: "PerfilPermiso",
                columns: perfilPermiso,
                values: new object[] {
                    "29", "1", "29", now, now
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
        }
    }
}
