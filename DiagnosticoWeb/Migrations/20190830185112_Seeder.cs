using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Migrations;
using DiagnosticoWeb.Models;

namespace DiagnosticoWeb.Migrations
{
    public partial class Seeder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var now = DateTime.Now;
            var hasher = new PasswordHasher<ApplicationUser>();
            var roles = new[] {"Id", "Name", "NormalizedName"};
            var dependencias = new[] {"Id", "Nombre", "CreatedAt", "UpdatedAt"};
            var usuarios = new[]
            {
                "Id", "Name", "UserName", "NormalizedUserName", "Email", "NormalizedEmail", "EmailConfirmed",
                "PasswordHash", "SecurityStamp", "AccessFailedCount", "PhoneNumberConfirmed", "TwoFactorEnabled",
                "LockoutEnabled"
            };
            var carencias = new[] {"Id", "Clave", "Nombre", "CreatedAt", "UpdatedAt", "Color"};
            var preguntas = new string[]
            {
                "Id", "Nombre", "TipoPregunta", "Condicion", "CondicionLista", "CondicionIterable", "Iterable",
                "EncuestaVersionId", "Numero", "Editable", "CreatedAt", "UpdatedAt", "Catalogo", "Gradual",
                "CarenciaId", "Expresion", "ExpresionEjemplo"
            };

            var respuestas = new string[]
            {
                "Id", "Nombre", "PreguntaId", "Numero", "CreatedAt", "UpdatedAt", "Negativa"
            };
            var preguntasGrados = new string[]
            {
                "Id", "PreguntaId", "Grado", "CreatedAt", "UpdatedAt"
            };  
            var respuestasGrados = new string[]
            {
                "Id", "RespuestaId", "PreguntaGradoId","Grado", "CreatedAt", "UpdatedAt"
            };
            migrationBuilder.InsertData(table: "AspNetRoles",
                columns: roles,
                values: new object[]
                {
                    "1",
                    "Administrador",
                    "Administrador"
                });
            migrationBuilder.InsertData(table: "AspNetRoles",
                columns: roles,
                values: new object[]
                {
                    "2",
                    "Administrador de dependencia",
                    "Administrador de dependencia"
                });
            migrationBuilder.InsertData(table: "AspNetRoles",
                columns: roles,
                values: new object[]
                {
                    "3",
                    "Analista de dependencia",
                    "Analista de dependencia"
                });
            migrationBuilder.InsertData(table: "Dependencias",
                columns: dependencias,
                values: new object[]
                {
                    "e587835d-66ec-4cd2-813c-ce9ac91affcc",
                    "Secretaría de Desarrollo Social y Humano",
                    now, now
                });
            migrationBuilder.InsertData(table: "AspNetUsers",
                columns: usuarios,
                values: new object[]
                {
                    "b7d2de8e-8448-4d84-9e5c-3c20bfff554a",
                    "Administrador",
                    "admin",
                    "admin",
                    "cnunez@guanajuato.gob.mx",
                    "cnunez@guanajuato.gob.mx",
                    true, hasher.HashPassword(null, "secret"), string.Empty, 0, false, false, false
                });
            migrationBuilder.InsertData(table: "AspNetUserRoles",
                columns: new string[] {"UserId", "RoleId"},
                values: new object[]
                {
                    "b7d2de8e-8448-4d84-9e5c-3c20bfff554a",
                    "1"
                });
            migrationBuilder.InsertData(table: "Carencias",
                columns: carencias,
                values: new object[]
                {
                    "1", "educativa", "Rezago educativo", now, now, "#fff3f3"
                });
            migrationBuilder.InsertData(table: "Carencias",
                columns: carencias,
                values: new object[]
                {
                    "2", "salud", "Acceso a los servicios de salud", now, now, "#fffef3"
                });
            migrationBuilder.InsertData(table: "Carencias",
                columns: carencias,
                values: new object[]
                {
                    "3", "seguridad", "Acceso a la seguridad social", now, now, "#f6fff3"
                });
            migrationBuilder.InsertData(table: "Carencias",
                columns: carencias,
                values: new object[]
                {
                    "4", "vivienda", "Calidad y espacios de la vivienda", now, now, "#f3fffe"
                });
            migrationBuilder.InsertData(table: "Carencias",
                columns: carencias,
                values: new object[]
                {
                    "5", "infraestructura", "Acceso a los servicios básicos en la vivienda", now, now, "#f3f7ff"
                });
            migrationBuilder.InsertData(table: "Carencias",
                columns: carencias,
                values: new object[]
                {
                    "6", "alimentacion", "Acceso a la alimentación", now, now, "#f9f3ff"
                });
            migrationBuilder.InsertData(table: "Encuestas",
                columns: new string[] {"Id", "Nombre", "Vigencia", "Activo", "CreatedAt", "UpdatedAt"},
                values: new object[]
                {
                    "1", "CONEVAL",  24, true, now, now
                });
            migrationBuilder.InsertData(table: "EncuestaVersiones",
                columns: new string[] {"Id", "Activa", "EncuestaId", "CreatedAt", "UpdatedAt"},
                values: new object[]
                {
                    "1", true, "1", now, now
                });
            
            migrationBuilder.InsertData(table: "Preguntas",
                columns: preguntas,
                values: new object[]
                {
                    "969", "Municipio vivienda", "Listado", null, false, null, false, "1", 1, false, now, now,
                    "Municipios", false, null, null, null,
                });
            migrationBuilder.InsertData(table: "Preguntas",
                columns: preguntas,
                values: new object[]
                {
                    "975", "Tipo de asentamiento", "Listado", null, false, null, false, "1", 2, false, now, now,
                    "TipoAsentamientos", false, null, null, null,
                });
            migrationBuilder.InsertData(table: "Preguntas",
                columns: preguntas,
                values: new object[]
                {
                    "984",
                    "¿Cuántas personas viven normalmente en esta vivienda contando a los niños chiquitos y a los ancianos?",
                    "Numerica", null, false, null, false, "1", 3, false, now, now, null, false, null, null, null,
                });
            migrationBuilder.InsertData(table: "Preguntas",
                columns: preguntas,
                values: new object[]
                {
                    "987", "¿Cuántas personas forman parte de este hogar?", "Numerica", null, false, null, false, "1", 4,
                    false, now, now, null, false, null, null, null,
                });
            migrationBuilder.InsertData(table: "Preguntas",
                columns: preguntas,
                values: new object[]
                {
                    "993", "Parentesco con el jefe de familia", "Listado", null, false, "?987", true, "1", 5, false, now,
                    now, "Parentescos", false, null, null, null,
                });
            migrationBuilder.InsertData(table: "Preguntas",
                columns: preguntas,
                values: new object[]
                {
                    "24", "¿Actualmente...", "Listado", null, false, "?987", true, "1", 6, false, now, now, null, false,
                    null, null, null,
                });
            migrationBuilder.InsertData(table: "Preguntas",
                columns: preguntas,
                values: new object[]
                {
                    "998", "Sexo habitante", "Listado", null, false, "?987", true, "1", 7, false, now, now, "Sexos",
                    false, null, null, null,
                });
            migrationBuilder.InsertData(table: "Preguntas",
                columns: preguntas,
                values: new object[]
                {
                    "208", "CURP", "Abierta", null, false, "?987", true, "1", 8, true, now, now, null, false, null,
                    "[A-Z]{1}[AEIOU]{1}[A-Z]{2}[0-9]{2}(0[1-9]|1[0-2])(0[1-9]|1[0-9]|2[0-9]|3[0-1])[HM]{1}(AS|BC|BS|CC|CS|CH|CL|CM|DF|DG|GT|GR|HG|JC|MC|MN|MS|NT|NL|OC|PL|QT|QR|SP|SL|SR|TC|TS|TL|VZ|YN|ZS|NE)[B-DF-HJ-NP-TV-Z]{3}[0-9A-Z]{1}[0-9]{1}$",
                    "LARS881101HGTZZR05",
                });
            migrationBuilder.InsertData(table: "Preguntas",
                columns: preguntas,
                values: new object[]
                {
                    "996", "Fecha de nacimiento", "Fecha", null, false, "?987", true, "1", 9, false, now, now, null,
                    false, "1", null, null,
                });
            migrationBuilder.InsertData(table: "Preguntas",
                columns: preguntas,
                values: new object[]
                {
                    "997", "¿Cuántos años cumplidos tiene?", "Numerica", null, false, "?987", true, "1", 10, false, now,
                    now, null, false, "1", null, null,
                });
            migrationBuilder.InsertData(table: "Preguntas",
                columns: preguntas,
                values: new object[]
                {
                    "27", "¿sabe leer y escribir un recado?", "Listado", "?997,>6", false, "?987", true, "1", 11, false,
                    now, now, null, false, "1", null, null,
                });
            migrationBuilder.InsertData(table: "Preguntas",
                columns: preguntas,
                values: new object[]
                {
                    "130", "¿Cuál es el último nivel que aprobó en la escuela?", "Listado", "?997,>2", false, "?987",
                    true, "1", 12, false, now, now, null, false, "1", null, null,
                });
            migrationBuilder.InsertData(table: "Preguntas",
                columns: preguntas,
                values: new object[]
                {
                    "131",
                    "¿Cuál es el último grado que aprobó en la escuela? (último año  aprobado del nivel referido en la pregunta antecedente)",
                    "Listado", "?997,>6", false, "?987", true, "1", 13, false, now, now, null, false, "1", null, null,
                });
            migrationBuilder.InsertData(table: "Preguntas",
                columns: preguntas,
                values: new object[]
                {
                    "29", "¿Asiste actualmente a la escuela, estancia infantil, CENDI, CADI o guardería?", "Listado",
                    null, false, "?987", true, "1", 14, false, now, now, null, false, "1", null, null,
                });
            migrationBuilder.InsertData(table: "Preguntas",
                columns: preguntas,
                values: new object[]
                {
                    "37", "Tiene derecho a los servicios médicos:", "Listado", null, false, "?987", true, "1", 15, false,
                    now, now, null, false, "2", null, null,
                });
            migrationBuilder.InsertData(table: "Preguntas",
                columns: preguntas,
                values: new object[]
                {
                    "82", "¿Está afiliado o inscrito a la institución por?", "Listado", "?37,!=102", false, "?987",
                    true, "1", 16, false, now, now, null, false, "2", null, null,
                });
            migrationBuilder.InsertData(table: "Preguntas",
                columns: preguntas,
                values: new object[]
                {
                    "38", "Cuando tiene problemas de salud, ¿En dónde se atiende?", "Listado", null, false, "?987",
                    true, "1", 17, false, now, now, null, false, "2", null, null,
                });
            migrationBuilder.InsertData(table: "Preguntas",
                columns: preguntas,
                values: new object[]
                {
                    "42", "En su vida diaria, ¿tiene dificultad al realizar las siguientes actividades:", "Listado",
                    null, false, "?987", true, "1", 18, false, now, now, null, true, "2", null, null,
                });
            migrationBuilder.InsertData(table: "Preguntas",
                columns: preguntas,
                values: new object[]
                {
                    "33", "¿El mes pasado…", "Listado", "?997,>16", false, "?987", true, "1", 19, false, now, now, null,
                    false, "3", null, null,
                });
            migrationBuilder.InsertData(table: "Preguntas",
                columns: preguntas,
                values: new object[]
                {
                    "91", "En su trabajo principal del mes pasado ¿tuvo un jefe(a) o supervisor?", "Listado", "?33,77",
                    false, "?987", true, "1", 20, false, now, now, null, false, "3", null, null,
                });
            migrationBuilder.InsertData(table: "Preguntas",
                columns: preguntas,
                values: new object[]
                {
                    "92",
                    "Entonces en el trabajo principal del mes pasado de ¿se dedicó a un negocio o actividad por su cuenta?",
                    "Listado", "?91,!=325", false, "?987", true, "1", 21, false, now, now, null, false, "3", null, null,
                });
            migrationBuilder.InsertData(table: "Preguntas",
                columns: preguntas,
                values: new object[]
                {
                    "88", "En su trabajo principal del mes pasado ¿se desempeñó como…?", "Listado", "?33,77", false,
                    "?987", true, "1", 22, false, now, now, "Ocupaciones", false, "3", null, null,
                });
            migrationBuilder.InsertData(table: "Preguntas",
                columns: preguntas,
                values: new object[]
                {
                    "93", "En su trabajo principal del mes pasado ¿recibió un pago?", "Listado", "?33,77", false,
                    "?987", true, "1", 23, false, now, now, null, false, "3", null, null,
                });
            migrationBuilder.InsertData(table: "Preguntas",
                columns: preguntas,
                values: new object[]
                {
                    "34", "¿En este trabajo le dieron las siguientes prestaciones, aunque no las haya utilizado?",
                    "Check", "?33,77/78/79/80", false, "?987", true, "1", 24, false, now, now, null, false, "3", null, null,
                });
            migrationBuilder.InsertData(table: "Preguntas",
                columns: preguntas,
                values: new object[]
                {
                    "96", "¿Tiene contratado voluntariamente…", "Check", null, false, "?987", true, "1", 25, false, now,
                    now, null, false, "3", null, null,
                });
            migrationBuilder.InsertData(table: "Preguntas",
                columns: preguntas,
                values: new object[]
                {
                    "99", "¿Recibe dinero por ...", "Listado", "?997,>64", false, "?987", true, "1", 26, false, now, now,
                    null, false, "3", null, null,
                });
            migrationBuilder.InsertData(table: "Preguntas",
                columns: preguntas,
                values: new object[]
                {
                    "100", "¿Realiza regularmente las siguientes actividades? (trabajo secundario)", "Listado",
                    "?997,>16",
                    false, "?987", true, "1", 27, false, now, now, null, false, "3", null, null,
                });
            migrationBuilder.InsertData(table: "Preguntas",
                columns: preguntas,
                values: new object[]
                {
                    "79", "¿Alguien en el hogar recibe dinero proveniente de otros países?", "Listado", null, false,
                    null, false, "1", 28, false, now, now, null, false, "3", null, null,
                });
            migrationBuilder.InsertData(table: "Preguntas",
                columns: preguntas,
                values: new object[]
                {
                    "44", "¿De qué material es la mayor parte del piso de esta vivienda?", "Listado", null, false, null,
                    false, "1", 29, false, now, now, null, false, "4", null, null,
                });
            migrationBuilder.InsertData(table: "Preguntas",
                columns: preguntas,
                values: new object[]
                {
                    "46", "¿De qué material es la mayor parte de las paredes o muros de esta vivienda?", "Listado",
                    null,
                    false, null, false, "1", 30, false, now, now, null, false, "4", null, null,
                });
            migrationBuilder.InsertData(table: "Preguntas",
                columns: preguntas,
                values: new object[]
                {
                    "48", "¿De qué material es la mayor parte del techo de esta vivienda?", "Listado", null, false,
                    null, false, "1", 31, false, now, now, null, false, "4", null, null,
                });
            migrationBuilder.InsertData(table: "Preguntas",
                columns: preguntas,
                values: new object[]
                {
                    "141",
                    "¿Cúantos cuartos tiene en total esta vivienda contando la cocina? (no cuente pasillos ni baños)",
                    "Numerica", null, false, null, false, "1", 32, false, now, now, null, false, "4", null, null,
                });
            migrationBuilder.InsertData(table: "Preguntas",
                columns: preguntas,
                values: new object[]
                {
                    "51", "¿Cuántos cuartos se usan para dormir sin contar pasillos y cocina? ", "Numerica", null,
                    false,
                    null, false, "1", 33, false, now, now, null, false, "4", null, null,
                });
            migrationBuilder.InsertData(table: "Preguntas",
                columns: preguntas,
                values: new object[]
                {
                    "113", "¿Qué tipo de baño o escusado tiene su vivienda?", "Listado", null, false,
                    null, false, "1", 34, false, now, now, null, false, "4", null, null,
                });
            migrationBuilder.InsertData(table: "Preguntas",
                columns: preguntas,
                values: new object[]
                {
                    "54", "¿En esta vivienda tienen...", "Listado", null, false, null, false, "1", 35, false, now, now,
                    null, false, "5", null, null,
                });
            migrationBuilder.InsertData(table: "Preguntas",
                columns: preguntas,
                values: new object[]
                {
                    "56", "¿Esta vivienda tiene drenaje o desagüe conectado a...", "Listado", null, false, null, false,
                    "1", 36, false, now, now, null, false, "5", null, null,
                });
            migrationBuilder.InsertData(table: "Preguntas",
                columns: preguntas,
                values: new object[]
                {
                    "57", "¿El combustible que más usan para cocinar es...", "Listado", null, false, null, false, "1", 
                    37, false, now, now, null, false, "5", null, null,
                });
            migrationBuilder.InsertData(table: "Preguntas",
                columns: preguntas,
                values: new object[]
                {
                    "58", "¿La estufa (fogón) de leña o carbón con la que cocinan tiene chimenea?", "Listado",
                    "?57,177/178", false, null, false, "1", 38, false, now, now, null, false, "5", null, null,
                });
            migrationBuilder.InsertData(table: "Preguntas",
                columns: preguntas,
                values: new object[]
                {
                    "117", "¿En esta vivienda tienen y funciona…", "Check", null, false, null, false, "1", 39, false, now,
                    now, null, false, "5", null, null,
                });
            migrationBuilder.InsertData(table: "Preguntas",
                columns: preguntas,
                values: new object[]
                {
                    "118", "En su vivienda ¿La luz eléctrica la obtienen…", "Listado", null, false, null, false, "1", 40,
                    false, now, now, null, false, "5", null, null,
                });
            migrationBuilder.InsertData(table: "Preguntas",
                columns: preguntas,
                values: new object[]
                {
                    "119", "¿La vivienda que habita es…", "Listado", null, false, null, false, "1", 41, false, now, now,
                    null, false, "5", null, null,
                });
            migrationBuilder.InsertData(table: "Preguntas",
                columns: preguntas,
                values: new object[]
                {
                    "63",
                    "En los últimos tres meses, por falta de dinero o recursos… ¿Alguna vez usted o algún adulto en su hogar tuvo una alimentación basada en muy poca variedad de alimentos?",
                    "Listado", null, false, null, false, "1", 42, false, now, now, null, false, "6", null, null,
                });
            migrationBuilder.InsertData(table: "Preguntas",
                columns: preguntas,
                values: new object[]
                {
                    "64",
                    "En los últimos tres meses, por falta de dinero o recursos… ¿Alguna vez usted o algún adulto en su hogar dejó de desayunar, comer o cenar?",
                    "Listado", null, false, null, false, "1", 43, false, now, now, null, false, "6", null, null,
                });
            migrationBuilder.InsertData(table: "Preguntas",
                columns: preguntas,
                values: new object[]
                {
                    "65",
                    "En los últimos tres meses, por falta de dinero o recursos… ¿Alguna vez usted o algún adulto en su hogar comió menos de lo que usted piensa debía comer?",
                    "Listado", null, false, null, false, "1", 44, false, now, now, null, false, "6", null, null,
                });
            migrationBuilder.InsertData(table: "Preguntas",
                columns: preguntas,
                values: new object[]
                {
                    "66",
                    "En los últimos tres meses, por falta de dinero o recursos… ¿Alguna vez usted o algún adulto en su hogar se quedaron sin comida?",
                    "Listado", null, false, null, false, "1", 45, false, now, now, null, false, "6", null, null,
                });
            migrationBuilder.InsertData(table: "Preguntas",
                columns: preguntas,
                values: new object[]
                {
                    "67",
                    "En los últimos tres meses, por falta de dinero o recursos… ¿Alguna vez usted o algún adulto de este hogar sintió hambre pero no comió?",
                    "Listado", null, false, null, false, "1", 46, false, now, now, null, false, "6", null, null,
                });
            migrationBuilder.InsertData(table: "Preguntas",
                columns: preguntas,
                values: new object[]
                {
                    "68",
                    "En los últimos tres meses, por falta de dinero o recursos… ¿Alguna vez usted o algún adulto en su hogar solo comió una vez al día o dejo de comer todo un día?",
                    "Listado", null, false, null, false, "1", 47, false, now, now, null, false, "6", null, null,
                });
            migrationBuilder.InsertData(table: "Preguntas",
                columns: preguntas,
                values: new object[]
                {
                    "69",
                    "En los últimos tres meses, por falta de dinero o recursos… ¿Alguna vez algún menor de 18 años en su hogar tuvo una alimentación basada en muy poca variedad de alimentos?",
                    "Listado", "?997,<18", true, null, false, "1", 48, false, now, now, null, false, "6", null, null,
                });
            migrationBuilder.InsertData(table: "Preguntas",
                columns: preguntas,
                values: new object[]
                {
                    "70",
                    "En los últimos tres meses, por falta de dinero o recursos… ¿Alguna vez algún menor de 18 años en su hogar comió menos de lo que debía?",
                    "Listado", "?997,<18", true, null, false, "1", 49, false, now, now, null, false, "6", null, null,
                });
            migrationBuilder.InsertData(table: "Preguntas",
                columns: preguntas,
                values: new object[]
                {
                    "71",
                    "En los últimos tres meses, por falta de dinero o recursos, ¿alguna vez tuvieron que disminuir la cantidad servida en las comidas a algún menor de 18 años del hogar?",
                    "Listado", "?997,<18", true, null, false, "1", 50, false, now, now, null, false, "6", null, null,
                });
            migrationBuilder.InsertData(table: "Preguntas",
                columns: preguntas,
                values: new object[]
                {
                    "72",
                    "En los últimos tres meses, por falta de dinero o recursos, ¿alguna vez algún menor de 18 años en su hogar sintió hambre pero no comió?",
                    "Listado", "?997,<18", true, null, false, "1", 51, false, now, now, null, false, "6", null, null,
                });
            migrationBuilder.InsertData(table: "Preguntas",
                columns: preguntas,
                values: new object[]
                {
                    "73",
                    "En los últimos tres meses, por falta de dinero o recursos, ¿alguna vez algún menor de 18 años en su hogar se acostó con hambre?",
                    "Listado", "?997,<18", true, null, false, "1", 52, false, now, now, null, false, "6", null, null,
                });
            migrationBuilder.InsertData(table: "Preguntas",
                columns: preguntas,
                values: new object[]
                {
                    "74",
                    "En los últimos tres meses, por falta de dinero o recursos, ¿alguna vez algún menor de 18 años en su hogar comió una vez al día o dejó de comer todo un día?",
                    "Listado", "?997,<18", true, null, false, "1", 53, false, now, now, null, false, "6", null, null,
                });
            
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "358", "Cuidar sin pago y de manera exclusiva a niños, enfermos, adultos mayores o discapacitados",
                    "100", "1", now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "359", "Trabajo comunitario o voluntario", "100", 2, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "360", "Reparaciones a la vivienda, aparatos domésticos o vehículos.", "100", 3, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "361", "Realizar el quehacer de su hogar", "100", 4, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "362", "Acarrear agua o leña", "100", 5, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "623", "Ninguno", "100", 6, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "404", "Con conexión de agua/tiene descarga directa de agua", "113", 1, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "405", "Le echan agua con cubeta", "113", 2, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "406", "Sin admisión de agua (letrina seca o húmeda)", "113", 3, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "407", "Pozo u hoyo negro", "113", 4, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "408", "No tiene", "113", 5, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "423", "Refrigerador", "117", 1, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "424", "Lavadora automática", "117", 2, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "425", "VHS, DVD, BLU-RAY", "117", 3, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "426", "Vehículo (carro, camioneta o camión)", "117", 4, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "427", "Teléfono (fijo)", "117", 5, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "428", "Horno (microondas o eléctrico)", "117", 6, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "429", "Computadora", "117", 7, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "430", "Estufa / parrilla de gas", "117", 8, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "431", "Calentador de agua/ boiler de gas", "117", 9, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "432", "Calentador de agua/ solar", "117", 10, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "433", "Calentador de agua/ boiler electrico", "117", 11, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "434", "Internet", "117", 12, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "435", "Teléfono celular", "117", 13, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "436", "Aparato de televisión", "117", 14, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "437", "Aparato de televisión digital", "117", 15, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "438", "Servicio de televisión de paga (antena parabólica, SKY o TV por cable)", "117", 16, now,
                    now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "440", "Tinaco", "117", 17, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "441", "Aparato para regular la temperatura (ventilador, enfriador, clima, calefactor)", "117", 18,
                    now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "627", "Ninguno", "117", 19, now, now, true,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "618", "Regadera", "117", 20, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "443", "Del servicio público", "118", 1, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "444", "De una planta particular", "118", 2, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "445", "De panel solar", "118", 3, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "446", "De otra fuente", "118", 4, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "447", "No tienen luz eléctrica", "118", 5, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "448", "Propia y totalmente pagada", "119", 1, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "449", "Propia y la está pagando", "119", 2, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "450", "Propia y está hipotecada", "119", 3, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "451", "Rentada o alquilada", "119", 4, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "452", "Prestada o la está cuidando", "119", 5, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "453", "Intestada o está en litigio", "119", 6, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "506", "Kínder o preescolar", "130", 1, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "507", "Primaria", "130", 2, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "508", "Secundaria", "130", 3, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "509", "Preparatoria o Bachillerato", "130", 4, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "510", "Normal básica", "130", 5, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "511", "Carrera técnica o comercial con primaria completa", "130", 6, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "512", "Carrera técnica o comercial con secundaria completa", "130", 7, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "513", "Carrera técnica o comercial con preparatoria completa", "130", 8, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "514", "Profesional", "130", 9, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "515", "Posgrado (maestría o doctorado)", "130", 10, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "566", "Ninguno", "130", 11, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "567", "1 años", "131", 1, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "568", "2 años", "131", 2, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "569", "3 años", "131", 3, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "570", "4 años", "131", 4, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "271", "5 años", "131", 5, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "572", "6 años", "131", 6, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "3", "Vive en unión libre?", "24", 1, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "4", "Es casada(o)?", "24", 2, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "5", "Es separada(o)?", "24", 3, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "6", "Es divorciada(o)?", "24", 4, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "7", "Está viuda(o)?", "24", 5, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "8", "Es soltera(o)?", "24", 6, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "35", "No", "27", 1, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "34", "Si", "27", 2, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "573", "No sabe/No contesta", "27", 3, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "49", "Si", "29", 1, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "50", "No", "29", 2, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "77", "Trabajó (por lo menos una hora)", "33", 1, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "78", "Tenía trabajo, pero no trabajó", "33", 2, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "79", "Buscó trabajo", "33", 3, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "80", "Es pensionada(o) o jubilada(o)", "33", 4, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "81", "Es estudiante", "33", 5, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "82", "Se dedica a los quehaceres de su hogar", "33", 6, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "83", "Tiene alguna limitación física o mental permanente que le impide trabajar", "33", 7, now,
                    now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "84", "Estaba en otra situación diferente a las anteriores", "33", 8, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "85", "Incapacidad por enfermedad, accidente o maternidad", "34", 1, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "86", "SAR o AFORE", "34", 2, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "87", "Crédito para vivienda", "34", 3, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "88", "Guardería", "34", 4, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "89", "Aguinaldo", "34", 5, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "90", "Seguro de vida", "34", 6, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "91", "No tiene derecho a ninguna de estas prestaciones", "34", 7, now, now, true,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "92", "No sabe / no responde", "34", 8, now, now, true,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "95", "Del Seguro Social IMSS", "37", 1, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "96", "Del ISSSTE", "37", 2, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "97", "Del ISSSTE estatal", "37", 3, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "98", "PEMEX, Defensa o Marina", "37", 4, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "99", "Del Seguro Popular para una nueva generación", "37", 5, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "100", "De un seguro privado", "37", 6, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "101", "En otra institucións", "37", 7, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "102", "Entonces ¿no tiene derecho a servicios médicos?", "37", 8, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "103", "Seguro Social IMSS", "38", 1, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "104", "ISSSTE", "38", 2, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "105", "ISSSTE estatal", "38", 3, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "106", "PEMEX, Defensa o Marina", "38", 4, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "107", "Centro de Salud u Hospital de la SSA (Seguro Popular)", "38", 5, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "108", "IMSS-PROSPERA", "38", 6, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "1082", "Consultorio de una farmacia", "38", 7, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "1083", "Se automedica", "38", 8, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "109", "Consultorio, clinica u hospital privado", "38", 9, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "110", "Otro lugar", "38", 10, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "111", "No se atiende", "38", 11, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "112", "En otra institución", "38", 12, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "113", "Entonces ¿no tiene derecho a servicios médicos?", "38", 13, now, now, false,
                });  
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "117", "Caminar, moverse, subir o bajar", "42", 1, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "118", "Ver, aun usando lentes", "42", 2, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "119", "Hablar, comunicarse o conversar", "42", 3, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "120", "Oír, aun usando aparato auditivo", "42", 4, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "121", "Vestirse, bañarse o comer", "42", 5, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "122", "Poner atención o aprender cosas sencillas", "42", 6, now, now, false,
                }); 
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "123", "Tiene alguna limitación mental", "42", 7, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "124", "Entonces, ¿No tiene dificultad física o mental?", "42", 8, now, now, true,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "130", "Tierra", "44", 1, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "131", "Cemento o firme", "44", 2, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "132", "Madera, mosaico u otro recubrimiento", "44", 3, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "135", "Material de desecho", "46", 1, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "136", "Lámina de cartón", "46", 2, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "137", "Lámina de asbesto o metálica", "46", 3, now, now, false,
                });  
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "138", "Carrizo, bambú o palma", "46", 4, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "139", "Embarro o bajareque", "46", 5, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "140", "Madera", "46", 6, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "141", "Adobe", "46", 7, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "142", "Tabique, ladrillo, block, piedra, cantera, cemento o concreto", "46", 8, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "145", "Material de desecho", "48", 1, now, now, false,
                });  
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "146", "Lámina de cartón", "48", 2, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "147", "Lámina metálica", "48", 3, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "148", "Lámina de asbesto", "48", 4, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "149", "Palma o paja", "48", 5, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "150", "Madera o tejamanil", "48", 6, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "151", "Terrado con viguería", "48", 7, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "152", "Teja", "48", 8, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "153", "Losa de concreto o viguetas con bovedilla", "48", 9, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "162", "Agua entubada dentro de la vivienda", "54", 1, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "163", "Agua entubada fuera de la vivienda,pero dentro del terreno", "54", 2, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "164", "Agua entubada de llave pública (o hidrante)", "54", 3, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "165", "Agua entubada que acarrean de otra vivienda", "54", 4, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "166", "Agua de pipa", "54", 5, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "167", "Agua de un pozo, río, lago, arroyo u otra", "54", 6, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "643", "Agua captada de lluvia u otro medio", "54", 7, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "170", "La red pública", "56", 1, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "171", "Una fosa séptica", "56", 2, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "172", "Una tubería que va a dar a una barranca o grieta", "56", 3, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "173", "Una tubería que va a dar a un río, lago o mar", "56", 4, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "174", "No tiene drenaje", "56", 5, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "620", "Biodigestor", "56", 6, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "175", "Gas de cilindro o tanque (estacionario)", "57", 1, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "176", "Gas natural o de tubería", "57", 2, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "177", "Leña", "57", 3, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "178", "Carbón", "57", 4, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "179", "Electricidad", "57", 5, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "180", "Otro combustible", "57", 6, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "181", "Si", "58", 1, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "182", "No", "58", 2, now, now, false,
                });  
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "206", "Si", "63", 1, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "207", "No", "63", 2, now, now, false,
                }); 
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "208", "Si", "64", 1, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "209", "No", "64", 2, now, now, false,
                });  
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "210", "Si", "65", 1, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "211", "No", "65", 2, now, now, false,
                });   
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "212", "Si", "66", 1, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "213", "No", "66", 2, now, now, false,
                });      
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "214", "Si", "67", 1, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "215", "No", "67", 2, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "216", "Si", "68", 1, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "217", "No", "68", 2, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "218", "Si", "69", 1, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "219", "No", "69", 2, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "220", "Si", "70", 1, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "221", "No", "70", 2, now, now, false,
                }); 
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "222", "Si", "71", 1, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "223", "No", "71", 2, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "224", "Si", "72", 1, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "225", "No", "72", 2, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "226", "Si", "73", 1, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "227", "No", "73", 2, now, now, false,
                });  
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "228", "Si", "74", 1, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "229", "No", "74", 2, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "247", "Si", "79", 1, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "248", "No", "79", 2, now, now, false,
                });  
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "249", "No sabe", "79", 3, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "250", "No contesta", "79", 4, now, now, false,
                }); 
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "256", "Prestación en el trabajo", "82", 1, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "257", "Jubilación", "82", 2, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "258", "Invalidez", "82", 3, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "259", "Algún familiar en el hogar", "82", 4, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "260", "Muerte del asegurado", "82", 5, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "261", "Ser estudiante", "82", 6, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "262", "Contratación propia", "82", 7, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "263", "Contratación propia", "82", 8, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "264", "Apoyo del gobierno", "82", 9, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "265", "No sabe/No responde", "82", 10, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "325", "Si", "91", 1, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "326", "No", "91", 2, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "327", "No sabe/No responde", "91", 3, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "331", "Si", "93", 1, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "332", "No", "93", 2, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "333", "No sabe/No responde", "93", 3, now, now, false,
                }); 
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "341", "SAR, AFORE o fondo de retiro", "96", 1, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "342", "Seguro privado de gastos médicos", "96", 2, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "343", "Seguro de vida", "96", 3, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "344", "Seguro de invalidez", "96", 4, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "345", "Otro tipo de seguro", "96", 5, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "346", "Ninguno de los anteriores", "96", 6, now, now, true,
                });  
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "347", "No sabe/No responde", "96", 7, now, now, true,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "353", "Programa Pensión del Programa para Adultos Mayores", "99", 1, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "354", "Componente de apoyo del Programa para Adultos Mayores  (PROSPERA)", "99", 2, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "355", "Otros Programas para Adultos Mayores (Estatal o Municipal)", "99", 3, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "356", "Ninguno", "99", 4, now, now, false,
                });
            migrationBuilder.InsertData(table: "Respuestas",
                columns: respuestas,
                values: new object[]
                {
                    "357", "No sabe/No responde", "99", 5, now, now, false,
                }); 
            
            
            migrationBuilder.InsertData(table: "PreguntaGrado",
                columns: preguntasGrados,
                values: new object[]
                {
                    "1", "42", "No puede hacerlo", now, now
                });
            migrationBuilder.InsertData(table: "PreguntaGrado",
                columns: preguntasGrados,
                values: new object[]
                {
                    "2", "42", "Lo hace con mucha dificultad", now, now
                });
            migrationBuilder.InsertData(table: "PreguntaGrado",
                columns: preguntasGrados,
                values: new object[]
                {
                    "3", "42", "Lo hace con poca dificultad", now, now
                });
            migrationBuilder.InsertData(table: "PreguntaGrado",
                columns: preguntasGrados,
                values: new object[]
                {
                    "4", "42", "No tiene dificultad", now, now
                }); 
            
            migrationBuilder.InsertData(table: "RespuestaGrado",
                columns: respuestasGrados,
                values: new object[]
                {
                    "1", "117", "1","11", now, now
                });
            migrationBuilder.InsertData(table: "RespuestaGrado",
                columns: respuestasGrados,
                values: new object[]
                {
                    "2", "117", "2","12", now, now
                });
            migrationBuilder.InsertData(table: "RespuestaGrado",
                columns: respuestasGrados,
                values: new object[]
                {
                    "3", "117", "3","13", now, now
                });
            migrationBuilder.InsertData(table: "RespuestaGrado",
                columns: respuestasGrados,
                values: new object[]
                {
                    "4", "117", "4","14", now, now
                });
            migrationBuilder.InsertData(table: "RespuestaGrado",
                columns: respuestasGrados,
                values: new object[]
                {
                    "5", "118", "1","21", now, now
                });
            migrationBuilder.InsertData(table: "RespuestaGrado",
                columns: respuestasGrados,
                values: new object[]
                {
                    "6", "118", "2","22", now, now
                });
            migrationBuilder.InsertData(table: "RespuestaGrado",
                columns: respuestasGrados,
                values: new object[]
                {
                    "7", "118", "3","23", now, now
                });
            migrationBuilder.InsertData(table: "RespuestaGrado",
                columns: respuestasGrados,
                values: new object[]
                {
                    "8", "118", "4","24", now, now
                });
            migrationBuilder.InsertData(table: "RespuestaGrado",
                columns: respuestasGrados,
                values: new object[]
                {
                    "9", "119", "1","31", now, now
                });
            migrationBuilder.InsertData(table: "RespuestaGrado",
                columns: respuestasGrados,
                values: new object[]
                {
                    "10", "119", "2","32", now, now
                });
            migrationBuilder.InsertData(table: "RespuestaGrado",
                columns: respuestasGrados,
                values: new object[]
                {
                    "11", "119", "3","33", now, now
                });
            migrationBuilder.InsertData(table: "RespuestaGrado",
                columns: respuestasGrados,
                values: new object[]
                {
                    "12", "119", "4","34", now, now
                });
            migrationBuilder.InsertData(table: "RespuestaGrado",
                columns: respuestasGrados,
                values: new object[]
                {
                    "13", "120", "1","41", now, now
                });
            migrationBuilder.InsertData(table: "RespuestaGrado",
                columns: respuestasGrados,
                values: new object[]
                {
                    "14", "120", "2","42", now, now
                });
            migrationBuilder.InsertData(table: "RespuestaGrado",
                columns: respuestasGrados,
                values: new object[]
                {
                    "15", "120", "3","43", now, now
                });
            migrationBuilder.InsertData(table: "RespuestaGrado",
                columns: respuestasGrados,
                values: new object[]
                {
                    "16", "120", "4","44", now, now
                });
            migrationBuilder.InsertData(table: "RespuestaGrado",
                columns: respuestasGrados,
                values: new object[]
                {
                    "17", "121", "1","51", now, now
                });
            migrationBuilder.InsertData(table: "RespuestaGrado",
                columns: respuestasGrados,
                values: new object[]
                {
                    "18", "121", "2","52", now, now
                });
            migrationBuilder.InsertData(table: "RespuestaGrado",
                columns: respuestasGrados,
                values: new object[]
                {
                    "19", "121", "3","53", now, now
                });
            migrationBuilder.InsertData(table: "RespuestaGrado",
                columns: respuestasGrados,
                values: new object[]
                {
                    "20", "121", "4","54", now, now
                });
            migrationBuilder.InsertData(table: "RespuestaGrado",
                columns: respuestasGrados,
                values: new object[]
                {
                    "21", "122", "1","61", now, now
                });
            migrationBuilder.InsertData(table: "RespuestaGrado",
                columns: respuestasGrados,
                values: new object[]
                {
                    "22", "122", "2","62", now, now
                });
            migrationBuilder.InsertData(table: "RespuestaGrado",
                columns: respuestasGrados,
                values: new object[]
                {
                    "23", "122", "3","63", now, now
                });
            migrationBuilder.InsertData(table: "RespuestaGrado",
                columns: respuestasGrados,
                values: new object[]
                {
                    "24", "122", "4","64", now, now
                });
            migrationBuilder.InsertData(table: "RespuestaGrado",
                columns: respuestasGrados,
                values: new object[]
                {
                    "25", "123", "1","71", now, now
                });
            migrationBuilder.InsertData(table: "RespuestaGrado",
                columns: respuestasGrados,
                values: new object[]
                {
                    "26", "123", "2","72", now, now
                });
            migrationBuilder.InsertData(table: "RespuestaGrado",
                columns: respuestasGrados,
                values: new object[]
                {
                    "27", "123", "3","73", now, now
                });
            migrationBuilder.InsertData(table: "RespuestaGrado",
                columns: respuestasGrados,
                values: new object[]
                {
                    "28", "123", "4","74", now, now
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
