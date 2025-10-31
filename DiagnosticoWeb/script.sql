[40m[32minfo[39m[22m[49m: Microsoft.EntityFrameworkCore.Infrastructure[10403]
      Entity Framework Core 2.1.11-servicing-32099 initialized 'ApplicationDbContext' using provider 'Microsoft.EntityFrameworkCore.SqlServer' with options: None
Inicial
Preguntas
Respuestas
PreguntaGrado
RespuestaGrado
IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

CREATE TABLE [AspNetRoles] (
    [Id] nvarchar(450) NOT NULL,
    [Name] nvarchar(256) NULL,
    [NormalizedName] nvarchar(256) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [AspNetUsers] (
    [Id] nvarchar(450) NOT NULL,
    [Name] nvarchar(256) NOT NULL,
    [UserName] nvarchar(256) NULL,
    [NormalizedUserName] nvarchar(256) NULL,
    [Email] nvarchar(256) NULL,
    [NormalizedEmail] nvarchar(256) NULL,
    [EmailConfirmed] bit NOT NULL,
    [PasswordHash] nvarchar(max) NULL,
    [SecurityStamp] nvarchar(max) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    [PhoneNumber] nvarchar(max) NULL,
    [PhoneNumberConfirmed] bit NOT NULL,
    [TwoFactorEnabled] bit NOT NULL,
    [LockoutEnd] datetimeoffset NULL,
    [LockoutEnabled] bit NOT NULL,
    [AccessFailedCount] int NOT NULL,
    CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [AspNetRoleClaims] (
    [Id] int NOT NULL IDENTITY,
    [RoleId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id])
);

GO

CREATE TABLE [AspNetUserClaims] (
    [Id] int NOT NULL IDENTITY,
    [UserId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id])
);

GO

CREATE TABLE [AspNetUserLogins] (
    [LoginProvider] nvarchar(450) NOT NULL,
    [ProviderKey] nvarchar(450) NOT NULL,
    [ProviderDisplayName] nvarchar(max) NULL,
    [UserId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
    CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [AspNetUserRoles] (
    [UserId] nvarchar(450) NOT NULL,
    [RoleId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
    CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [AspNetUserTokens] (
    [UserId] nvarchar(450) NOT NULL,
    [LoginProvider] nvarchar(450) NOT NULL,
    [Name] nvarchar(450) NOT NULL,
    [Value] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
    CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [Trabajadores] (
    [Id] nvarchar(450) NOT NULL,
    [Nombre] nvarchar(100) NULL,
    [Username] nvarchar(50) NULL,
    [Password] nvarchar(450) NULL,
    [Email] nvarchar(100) NULL,
    [CreatedAt] datetime2 NULL,
    [UpdatedAt] datetime2 NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_Trabajadores] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [Dependencias] (
    [Id] nvarchar(450) NOT NULL,
    [Nombre] nvarchar(250) NULL,
    [CreatedAt] datetime2 NULL,
    [UpdatedAt] datetime2 NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_Dependencias] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [TrabajadoresDependencias] (
    [Id] nvarchar(450) NOT NULL,
    [DependenciaId] nvarchar(450) NOT NULL,
    [UsuarioId] nvarchar(450) NOT NULL,
    [CreatedAt] datetime2 NULL,
    [UpdatedAt] datetime2 NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_TrabajadoresDependencias] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_TrabajadoresDependencias_Dependencia] FOREIGN KEY ([DependenciaId]) REFERENCES [Dependencias] ([Id]),
    CONSTRAINT [FK_TrabajadoresDependencias_Trabajadores] FOREIGN KEY ([UsuarioId]) REFERENCES [Trabajadores] ([Id])
);

GO

CREATE TABLE [UsuariosDependencias] (
    [Id] nvarchar(450) NOT NULL,
    [DependenciaId] nvarchar(450) NOT NULL,
    [UsuarioId] nvarchar(450) NOT NULL,
    [CreatedAt] datetime2 NULL,
    [UpdatedAt] datetime2 NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_UsuariosDependencias] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_UsuariosDependencias_Dependencia] FOREIGN KEY ([DependenciaId]) REFERENCES [Dependencias] ([Id]),
    CONSTRAINT [FK_UsuariosDependencias_Trabajadores] FOREIGN KEY ([UsuarioId]) REFERENCES [AspNetUsers] ([Id])
);

GO

CREATE TABLE [ProgramasSociales] (
    [Id] nvarchar(450) NOT NULL,
    [Nombre] nvarchar(250) NULL,
    [DependenciaId] nvarchar(450) NOT NULL,
    [CreatedAt] datetime2 NULL,
    [UpdatedAt] datetime2 NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_ProgramasSociales] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ProgramasSociales_Dependencias] FOREIGN KEY ([DependenciaId]) REFERENCES [Dependencias] ([Id])
);

GO

CREATE TABLE [Vertientes] (
    [Id] nvarchar(450) NOT NULL,
    [Nombre] nvarchar(250) NULL,
    [ProgramaSocialId] nvarchar(450) NOT NULL,
    [Inmediato] bit NOT NULL,
    [CreatedAt] datetime2 NULL,
    [UpdatedAt] datetime2 NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_Vertientes] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Vertientes_ProgramasSociales] FOREIGN KEY ([ProgramaSocialId]) REFERENCES [ProgramasSociales] ([Id])
);

GO

CREATE TABLE [Beneficiarios] (
    [Id] nvarchar(450) NOT NULL,
    [ApellidoPaterno] nvarchar(256) NULL,
    [ApellidoMaterno] nvarchar(256) NULL,
    [Nombre] nvarchar(256) NULL,
    [FechaNacimiento] datetime2 NULL,
    [Sexo] nvarchar(10) NULL,
    [EstadoNacimiento] nvarchar(50) NULL,
    [Curp] nvarchar(18) NULL,
    [Rfc] nvarchar(13) NULL,
    [Comentarios] nvarchar(450) NULL,
    [CreatedAt] datetime2 NULL,
    [UpdatedAt] datetime2 NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_Beneficiarios] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [Archivos] (
    [Id] nvarchar(450) NOT NULL,
    [Nombre] nvarchar(450) NULL,
    [Base64] text NOT NULL,
    [CreatedAt] datetime2 NULL,
    [UpdatedAt] datetime2 NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_Archivos] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [Domicilio] (
    [Id] nvarchar(450) NOT NULL,
    [ClaveAGEB] nvarchar(4) NULL,
    [Telefono] nvarchar(13) NULL,
    [TelefonoCasa] nvarchar(13) NULL,
    [Email] nvarchar(max) NULL,
    [DomicilioN] nvarchar(100) NULL,
    [EntreCalle1] nvarchar(100) NULL,
    [EntreCalle2] nvarchar(100) NULL,
    [Latitud] float NULL,
    [Longitud] float NULL,
    [LatitudAtm] float NULL,
    [LongitudAtm] float NULL,
    [CodigoPostal] nvarchar(10) NULL,
    [BeneficiarioId] nvarchar(450) NOT NULL,
    [CreatedAt] datetime2 NULL,
    [UpdatedAt] datetime2 NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_Domicilio] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Beneficiario_Domicilio] FOREIGN KEY ([BeneficiarioId]) REFERENCES [Beneficiarios] ([Id])
);

GO

CREATE TABLE [Configuracion] (
    [Id] nvarchar(450) NOT NULL,
    [MetrosRadio] float NULL,
    [CreatedAt] datetime2 NULL,
    [UpdatedAt] datetime2 NULL,
    CONSTRAINT [PK_Configuracion] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [Encuestas] (
    [Id] nvarchar(450) NOT NULL,
    [Nombre] nvarchar(255) NULL,
    [Vigencia] int NULL,
    [Activo] bit NULL,
    [CreatedAt] datetime2 NULL,
    [UpdatedAt] datetime2 NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_Encuestas] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [EncuestaVersiones] (
    [Id] nvarchar(450) NOT NULL,
    [Activa] bit NULL,
    [EncuestaId] nvarchar(450) NULL,
    [CreatedAt] datetime2 NULL,
    [UpdatedAt] datetime2 NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_EncuestaVersiones] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_EncuestaVersiones_Encuesta] FOREIGN KEY ([EncuestaId]) REFERENCES [Encuestas] ([Id])
);

GO

CREATE TABLE [Preguntas] (
    [Id] nvarchar(450) NOT NULL,
    [Nombre] nvarchar(255) NULL,
    [TipoPregunta] nvarchar(100) NULL,
    [Condicion] nvarchar(450) NULL,
    [CondicionLista] bit NULL,
    [CondicionIterable] nvarchar(450) NULL,
    [Iterable] bit NULL,
    [EncuestaVersionId] nvarchar(450) NULL,
    [Numero] int NULL,
    [Editable] bit NULL,
    [CreatedAt] datetime2 NULL,
    [UpdatedAt] datetime2 NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_Preguntas] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_EncuestaVersion_Pregunta] FOREIGN KEY ([EncuestaVersionId]) REFERENCES [EncuestaVersiones] ([Id])
);

GO

CREATE TABLE [Respuestas] (
    [Id] nvarchar(450) NOT NULL,
    [Nombre] nvarchar(255) NULL,
    [PreguntaId] nvarchar(450) NULL,
    [Numero] int NULL,
    [CreatedAt] datetime2 NULL,
    [UpdatedAt] datetime2 NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_Respuestas] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Preguntas_Respuestas] FOREIGN KEY ([PreguntaId]) REFERENCES [Preguntas] ([Id])
);

GO

CREATE TABLE [Aplicaciones] (
    [Id] nvarchar(450) NOT NULL,
    [Estatus] nvarchar(450) NOT NULL,
    [BeneficiarioId] nvarchar(450) NOT NULL,
    [EncuestaVersionId] nvarchar(450) NULL,
    [CreatedAt] datetime2 NULL,
    [UpdatedAt] datetime2 NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_Aplicaciones] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Aplicaciones_EncuestaVersion] FOREIGN KEY ([EncuestaVersionId]) REFERENCES [EncuestaVersiones] ([Id]),
    CONSTRAINT [FK_Beneficiarios_Aplicaciones] FOREIGN KEY ([BeneficiarioId]) REFERENCES [Beneficiarios] ([Id])
);

GO

CREATE TABLE [AplicacionPreguntas] (
    [Id] nvarchar(450) NOT NULL,
    [Valor] nvarchar(255) NULL,
    [AplicacionId] nvarchar(450) NULL,
    [PreguntaId] nvarchar(450) NULL,
    [RespuestaId] nvarchar(450) NULL,
    [RespuestaValor] float NULL,
    [RespuestaIteracion] nvarchar(2) NULL,
    [CreatedAt] datetime2 NULL,
    [UpdatedAt] datetime2 NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_AplicacionPreguntas] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Aplicaciones_AplicacionPreguntas] FOREIGN KEY ([AplicacionId]) REFERENCES [Aplicaciones] ([Id]),
    CONSTRAINT [FK_Preguntas_AplicacionPreguntas] FOREIGN KEY ([PreguntaId]) REFERENCES [Preguntas] ([Id]),
    CONSTRAINT [FK_Respuesta_AplicacionPreguntas] FOREIGN KEY ([RespuestaId]) REFERENCES [Respuestas] ([Id])
);

GO

CREATE TABLE [Solicitudes] (
    [Id] nvarchar(450) NOT NULL,
    [BeneficiarioId] nvarchar(450) NOT NULL,
    [TrabajadorId] nvarchar(450) NOT NULL,
    [VertienteId] nvarchar(450) NOT NULL,
    [AplicacionId] nvarchar(450) NOT NULL,
    [Estatus] nvarchar(450) NOT NULL,
    [CreatedAt] datetime2 NULL,
    [UpdatedAt] datetime2 NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_Solicitudes] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Beneficiario_Solicitudes] FOREIGN KEY ([BeneficiarioId]) REFERENCES [Beneficiarios] ([Id]),
    CONSTRAINT [FK_Trabajador_Solicitudes] FOREIGN KEY ([TrabajadorId]) REFERENCES [Trabajadores] ([Id]),
    CONSTRAINT [FK_Vertientes_Solicitudes] FOREIGN KEY ([VertienteId]) REFERENCES [Vertientes] ([Id]),
    CONSTRAINT [FK_Aplicacion_Solicitudes] FOREIGN KEY ([AplicacionId]) REFERENCES [Aplicaciones] ([Id])
);

GO

CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);

GO

CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL;

GO

CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);

GO

CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId]);

GO

CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);

GO

CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]);

GO

CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190429182557_InitialMigration', N'2.1.11-servicing-32099');

GO

CREATE TABLE [Estados] (
    [Id] nvarchar(450) NOT NULL,
    [Clave] nvarchar(256) NULL,
    [Nombre] nvarchar(256) NULL,
    [CreatedAt] datetime2 NULL,
    [UpdatedAt] datetime2 NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_Estados] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [Municipios] (
    [Id] nvarchar(450) NOT NULL,
    [Clave] nvarchar(256) NULL,
    [Nombre] nvarchar(256) NULL,
    [EstadoId] nvarchar(450) NULL,
    [CreatedAt] datetime2 NULL,
    [UpdatedAt] datetime2 NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_Municipios] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Municipios_Estados] FOREIGN KEY ([EstadoId]) REFERENCES [Estados] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [Localidades] (
    [Id] nvarchar(450) NOT NULL,
    [Clave] nvarchar(256) NULL,
    [Nombre] nvarchar(256) NULL,
    [Indigena] nvarchar(256) NULL,
    [MunicipioId] nvarchar(450) NULL,
    [EstadoId] nvarchar(450) NULL,
    [CreatedAt] datetime2 NULL,
    [UpdatedAt] datetime2 NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_Localidades] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Localidades_Municipios] FOREIGN KEY ([MunicipioId]) REFERENCES [Municipios] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Localidades_Estados] FOREIGN KEY ([EstadoId]) REFERENCES [Estados] ([Id])
);

GO

CREATE TABLE [Agebs] (
    [Id] nvarchar(450) NOT NULL,
    [Clave] nvarchar(256) NULL,
    [Nombre] nvarchar(256) NULL,
    [LocalidadId] nvarchar(450) NULL,
    [MunicipioId] nvarchar(450) NULL,
    [EstadoId] nvarchar(450) NULL,
    [CreatedAt] datetime2 NULL,
    [UpdatedAt] datetime2 NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_Agebs] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Agebs_Localidades] FOREIGN KEY ([LocalidadId]) REFERENCES [Localidades] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Agebs_Municipios] FOREIGN KEY ([MunicipioId]) REFERENCES [Municipios] ([Id]),
    CONSTRAINT [FK_Agebs_Estados] FOREIGN KEY ([EstadoId]) REFERENCES [Estados] ([Id])
);

GO

CREATE TABLE [ZonasImpulso] (
    [Id] nvarchar(450) NOT NULL,
    [Nombre] nvarchar(256) NULL,
    [MunicipioId] nvarchar(450) NULL,
    [CreatedAt] datetime2 NULL,
    [UpdatedAt] datetime2 NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_ZonasImpulso] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ZonasImpulso_Municipios] FOREIGN KEY ([MunicipioId]) REFERENCES [Municipios] ([Id]) ON DELETE CASCADE
);

GO

ALTER TABLE [Beneficiarios] ADD [LocalidadId] nvarchar(450) NULL;

GO

ALTER TABLE [Beneficiarios] ADD [MunicipioId] nvarchar(450) NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190705163230_Catalogos', N'2.1.11-servicing-32099');

GO

ALTER TABLE [Preguntas] ADD [Catalogo] nvarchar(max) NULL;

GO

CREATE TABLE [Parentescos] (
    [Id] nvarchar(450) NOT NULL,
    [Nombre] nvarchar(256) NULL,
    [CreatedAt] datetime2 NULL,
    [UpdatedAt] datetime2 NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_Parentescos] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [Sexos] (
    [Id] nvarchar(450) NOT NULL,
    [Nombre] nvarchar(256) NULL,
    [CreatedAt] datetime2 NULL,
    [UpdatedAt] datetime2 NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_Sexos] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [Estudios] (
    [Id] nvarchar(450) NOT NULL,
    [Nombre] nvarchar(256) NULL,
    [CreatedAt] datetime2 NULL,
    [UpdatedAt] datetime2 NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_Estudios] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [Discapacidades] (
    [Id] nvarchar(450) NOT NULL,
    [Nombre] nvarchar(256) NULL,
    [CreatedAt] datetime2 NULL,
    [UpdatedAt] datetime2 NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_Discapacidades] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [EstadosCiviles] (
    [Id] nvarchar(450) NOT NULL,
    [Nombre] nvarchar(256) NULL,
    [CreatedAt] datetime2 NULL,
    [UpdatedAt] datetime2 NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_EstadosCiviles] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [Ocupaciones] (
    [Id] nvarchar(450) NOT NULL,
    [Nombre] nvarchar(256) NULL,
    [CreatedAt] datetime2 NULL,
    [UpdatedAt] datetime2 NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_Ocupaciones] PRIMARY KEY ([Id])
);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190708231628_PreguntaCatalogo', N'2.1.11-servicing-32099');

GO

CREATE TABLE [Zonas] (
    [Id] nvarchar(450) NOT NULL,
    [Clave] nvarchar(max) NULL,
    [DependenciaId] nvarchar(450) NOT NULL,
    [CreatedAt] datetime2 NULL,
    [UpdatedAt] datetime2 NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_Zonas] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Zonas_Dependencias] FOREIGN KEY ([DependenciaId]) REFERENCES [Dependencias] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [MunicipiosZonas] (
    [Id] nvarchar(450) NOT NULL,
    [ZonaId] nvarchar(450) NOT NULL,
    [MunicipioId] nvarchar(450) NOT NULL,
    [CreatedAt] datetime2 NULL,
    [UpdatedAt] datetime2 NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_ZonasMunicipios] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ZonasMunicipios_Municipios] FOREIGN KEY ([MunicipioId]) REFERENCES [Municipios] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_ZonasMunicipios_Zonas] FOREIGN KEY ([ZonaId]) REFERENCES [Zonas] ([Id]) ON DELETE CASCADE
);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190709164547_Zonas', N'2.1.11-servicing-32099');

GO

ALTER TABLE [Vertientes] ADD [Vigencia] int NULL;

GO

ALTER TABLE [Vertientes] ADD [TipoEntrega] nvarchar(max) NULL;

GO

CREATE TABLE [VertientesArchivos] (
    [Id] nvarchar(450) NOT NULL,
    [TipoArchivo] nvarchar(max) NULL,
    [VertienteId] nvarchar(450) NOT NULL,
    [CreatedAt] datetime2 NULL,
    [UpdatedAt] datetime2 NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_VertientesArchivos] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_VertientesArchivos] FOREIGN KEY ([VertienteId]) REFERENCES [Vertientes] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [TipoAsentamientos] (
    [Id] nvarchar(450) NOT NULL,
    [Nombre] nvarchar(256) NULL,
    [CreatedAt] datetime2 NULL,
    [UpdatedAt] datetime2 NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_TipoAsentamientos] PRIMARY KEY ([Id])
);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190711182549_ConfVertientes', N'2.1.11-servicing-32099');

GO

EXEC sp_rename N'[Beneficiarios].[EstadoNacimiento]', N'EstadoId', N'COLUMN';

GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Beneficiarios]') AND [c].[name] = N'EstadoId');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Beneficiarios] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [Beneficiarios] ALTER COLUMN [EstadoId] nvarchar(450) NULL;

GO

ALTER TABLE [Beneficiarios] ADD CONSTRAINT [FK_Beneficiario_Estado] FOREIGN KEY ([EstadoId]) REFERENCES [Estados] ([Id]);

GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Beneficiarios]') AND [c].[name] = N'MunicipioId');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Beneficiarios] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [Beneficiarios] DROP COLUMN [MunicipioId];

GO

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Beneficiarios]') AND [c].[name] = N'LocalidadId');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [Beneficiarios] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [Beneficiarios] DROP COLUMN [LocalidadId];

GO

ALTER TABLE [Domicilio] ADD [MunicipioId] nvarchar(450) NULL;

GO

ALTER TABLE [Domicilio] ADD [LocalidadId] nvarchar(450) NULL;

GO

ALTER TABLE [Domicilio] ADD CONSTRAINT [FK_Domicilio_Municipio] FOREIGN KEY ([MunicipioId]) REFERENCES [Municipios] ([Id]);

GO

ALTER TABLE [Domicilio] ADD CONSTRAINT [FK_Domicilio_Localidad] FOREIGN KEY ([LocalidadId]) REFERENCES [Localidades] ([Id]);

GO

EXEC sp_rename N'[Domicilio].[ClaveAGEB]', N'AgebId', N'COLUMN';

GO

DECLARE @var3 sysname;
SELECT @var3 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Domicilio]') AND [c].[name] = N'AgebId');
IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [Domicilio] DROP CONSTRAINT [' + @var3 + '];');
ALTER TABLE [Domicilio] ALTER COLUMN [AgebId] nvarchar(450) NULL;

GO

ALTER TABLE [Domicilio] ADD CONSTRAINT [FK_Domicilio_Ageb] FOREIGN KEY ([AgebId]) REFERENCES [Agebs] ([Id]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190712170828_RenombrarEstado', N'2.1.11-servicing-32099');

GO

CREATE TABLE [Carencias] (
    [Id] nvarchar(450) NOT NULL,
    [Clave] nvarchar(256) NULL,
    [Nombre] nvarchar(256) NULL,
    [CreatedAt] datetime2 NULL,
    [UpdatedAt] datetime2 NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_Carencias] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [VertienteCarencias] (
    [Id] nvarchar(450) NOT NULL,
    [VertienteId] nvarchar(450) NULL,
    [CarenciaId] nvarchar(450) NULL,
    [CreatedAt] datetime2 NULL,
    [UpdatedAt] datetime2 NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_VertienteCarencias] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_VertienteCarencia_Carencias] FOREIGN KEY ([CarenciaId]) REFERENCES [Carencias] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_VertienteCarencia_Vertientes] FOREIGN KEY ([VertienteId]) REFERENCES [Vertientes] ([Id]) ON DELETE CASCADE
);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190717202858_Carencias', N'2.1.11-servicing-32099');

GO

ALTER TABLE [Aplicaciones] ADD [Carencias] nvarchar(450) NULL;

GO

ALTER TABLE [Beneficiarios] ADD [EstudioId] nvarchar(450) NULL;

GO

ALTER TABLE [Beneficiarios] ADD CONSTRAINT [FK_Beneficiario_Estudios] FOREIGN KEY ([EstudioId]) REFERENCES [Estudios] ([Id]);

GO

ALTER TABLE [Beneficiarios] ADD [EstadoCivilId] nvarchar(450) NULL;

GO

ALTER TABLE [Beneficiarios] ADD CONSTRAINT [FK_Beneficiario_EstadoCivil] FOREIGN KEY ([EstadoCivilId]) REFERENCES [EstadosCiviles] ([Id]);

GO

ALTER TABLE [Beneficiarios] ADD [DiscapacidadId] nvarchar(450) NULL;

GO

ALTER TABLE [Beneficiarios] ADD CONSTRAINT [FK_Beneficiario_Discapacidad] FOREIGN KEY ([DiscapacidadId]) REFERENCES [Estudios] ([Id]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190725175002_AddDatosBeneficiario', N'2.1.11-servicing-32099');

GO

CREATE TABLE [Importaciones] (
    [Clave] nvarchar(256) NOT NULL,
    [UsuarioId] nvarchar(450) NULL,
    [ImportedAt] datetime2 NULL,
    CONSTRAINT [PK_Importaciones] PRIMARY KEY ([Clave]),
    CONSTRAINT [FK_Importaciones_Usuarios] FOREIGN KEY ([UsuarioId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190725202703_Exportacion', N'2.1.11-servicing-32099');

GO

ALTER TABLE [AplicacionPreguntas] DROP CONSTRAINT [FK_Respuesta_AplicacionPreguntas];

GO

DECLARE @var4 sysname;
SELECT @var4 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Vertientes]') AND [c].[name] = N'Inmediato');
IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [Vertientes] DROP CONSTRAINT [' + @var4 + '];');
ALTER TABLE [Vertientes] DROP COLUMN [Inmediato];

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190726190321_DropColumns', N'2.1.11-servicing-32099');

GO

EXEC sp_rename N'[Localidades].[Indigena]', N'ZonaIndigenaId', N'COLUMN';

GO

ALTER TABLE [Domicilio] ADD [ZonaImpulsoId] nvarchar(max) NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190727030922_DomicilioZona', N'2.1.11-servicing-32099');

GO

ALTER TABLE [Beneficiarios] ADD [Estatus] bit NULL DEFAULT 0;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190727220959_AddBeneficiarioEstatus', N'2.1.11-servicing-32099');

GO

ALTER TABLE [TrabajadoresDependencias] DROP CONSTRAINT [FK_TrabajadoresDependencias_Trabajadores];

GO

EXEC sp_rename N'[TrabajadoresDependencias].[UsuarioId]', N'TrabajadorId', N'COLUMN';

GO

ALTER TABLE [TrabajadoresDependencias] ADD CONSTRAINT [FK_TrabajadoresDependencias_Trabajadores_TrabajadorId] FOREIGN KEY ([TrabajadorId]) REFERENCES [Trabajadores] ([Id]) ON DELETE NO ACTION;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190728144708_TrabajadorDependencia', N'2.1.11-servicing-32099');

GO

CREATE TABLE [PreguntaGrado] (
    [Id] nvarchar(450) NOT NULL,
    [PreguntaId] nvarchar(450) NULL,
    [Grado] nvarchar(250) NULL,
    [CreatedAt] datetime2 NULL,
    [UpdatedAt] datetime2 NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_PreguntaGrado] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_PreguntaGrado_Preguntas] FOREIGN KEY ([PreguntaId]) REFERENCES [Preguntas] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [RespuestaGrado] (
    [Id] nvarchar(450) NOT NULL,
    [RespuestaId] nvarchar(450) NULL,
    [PreguntaGradoId] nvarchar(450) NULL,
    [Grado] nvarchar(250) NULL,
    [CreatedAt] datetime2 NULL,
    [UpdatedAt] datetime2 NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_RespuestaGrado] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_RespuestaGrado_Respuesta] FOREIGN KEY ([RespuestaId]) REFERENCES [Respuestas] ([Id]),
    CONSTRAINT [FK_RespuestaGrado_PreguntaGrado] FOREIGN KEY ([PreguntaGradoId]) REFERENCES [PreguntaGrado] ([Id])
);

GO

ALTER TABLE [Preguntas] ADD [Gradual] bit NULL;

GO

ALTER TABLE [Respuestas] ADD [Negativa] bit NULL;

GO

ALTER TABLE [AplicacionPreguntas] ADD [Grado] nvarchar(20) NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190730204940_Grados', N'2.1.11-servicing-32099');

GO

DECLARE @var5 sysname;
SELECT @var5 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Beneficiarios]') AND [c].[name] = N'Sexo');
IF @var5 IS NOT NULL EXEC(N'ALTER TABLE [Beneficiarios] DROP CONSTRAINT [' + @var5 + '];');
ALTER TABLE [Beneficiarios] DROP COLUMN [Sexo];

GO

ALTER TABLE [Beneficiarios] ADD [SexoId] nvarchar(450) NULL;

GO

ALTER TABLE [Beneficiarios] ADD CONSTRAINT [FK_Beneficiario_Sexo] FOREIGN KEY ([SexoId]) REFERENCES [Sexos] ([Id]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190731182448_BeneficiarioSexoId', N'2.1.11-servicing-32099');

GO

ALTER TABLE [AspNetUsers] ADD [DependenciaId] nvarchar(450) NULL;

GO

ALTER TABLE [AspNetUsers] ADD CONSTRAINT [FK_AspNetUsers_Dependencias] FOREIGN KEY ([DependenciaId]) REFERENCES [Dependencias] ([Id]);

GO

ALTER TABLE [Preguntas] ADD [CarenciaId] nvarchar(450) NULL;

GO

ALTER TABLE [Preguntas] ADD CONSTRAINT [FK_Preguntas_Carencias] FOREIGN KEY ([CarenciaId]) REFERENCES [Carencias] ([Id]);

GO

ALTER TABLE [Carencias] ADD [Color] nvarchar(max) NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190806202147_CarenciaPregunta', N'2.1.11-servicing-32099');

GO

ALTER TABLE [Beneficiarios] ADD [TrabajadorId] nvarchar(450) NULL;

GO

ALTER TABLE [Beneficiarios] ADD CONSTRAINT [FK_Beneficiario_Trabajador] FOREIGN KEY ([TrabajadorId]) REFERENCES [Trabajadores] ([Id]);

GO

ALTER TABLE [Domicilio] ADD [TrabajadorId] nvarchar(450) NULL;

GO

ALTER TABLE [Domicilio] ADD CONSTRAINT [FK_Domicilio_Trabajador] FOREIGN KEY ([TrabajadorId]) REFERENCES [Trabajadores] ([Id]);

GO

ALTER TABLE [Aplicaciones] ADD [TrabajadorId] nvarchar(450) NULL;

GO

ALTER TABLE [Aplicaciones] ADD CONSTRAINT [FK_Aplicacion_Trabajador] FOREIGN KEY ([TrabajadorId]) REFERENCES [Trabajadores] ([Id]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190807150414_AddTrabajadorId', N'2.1.11-servicing-32099');

GO

CREATE TABLE [BeneficiarioHistorico] (
    [Id] nvarchar(450) NOT NULL,
    [ApellidoPaterno] nvarchar(256) NULL,
    [ApellidoMaterno] nvarchar(256) NULL,
    [Nombre] nvarchar(256) NULL,
    [FechaNacimiento] datetime2 NULL,
    [Curp] nvarchar(18) NULL,
    [Rfc] nvarchar(13) NULL,
    [Comentarios] nvarchar(450) NULL,
    [BeneficiarioId] nvarchar(450) NOT NULL,
    [EstudioId] nvarchar(450) NULL,
    [EstadoId] nvarchar(450) NULL,
    [EstadoCivilId] nvarchar(450) NULL,
    [DiscapacidadId] nvarchar(450) NULL,
    [SexoId] nvarchar(450) NULL,
    [TrabajadorId] nvarchar(450) NULL,
    [Estatus] bit NULL,
    [CreatedAt] datetime2 NULL,
    [UpdatedAt] datetime2 NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_BeneficiarioHistorico] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_HistorialBeneficiario_Beneficiario] FOREIGN KEY ([BeneficiarioId]) REFERENCES [Beneficiarios] ([Id]),
    CONSTRAINT [FK_HistorialBeneficiario_Estudio] FOREIGN KEY ([EstudioId]) REFERENCES [Estudios] ([Id]),
    CONSTRAINT [FK_HistorialBeneficiario_EstadoCivil] FOREIGN KEY ([EstadoCivilId]) REFERENCES [EstadosCiviles] ([Id]),
    CONSTRAINT [FK_HistorialBeneficiario_Discapacidad] FOREIGN KEY ([DiscapacidadId]) REFERENCES [Discapacidades] ([Id]),
    CONSTRAINT [FK_HistorialBeneficiario_Sexo] FOREIGN KEY ([SexoId]) REFERENCES [Sexos] ([Id]),
    CONSTRAINT [FK_HistorialBeneficiario_Trabajador] FOREIGN KEY ([TrabajadorId]) REFERENCES [Trabajadores] ([Id])
);

GO

ALTER TABLE [Domicilio] ADD [Activa] bit NULL DEFAULT 0;

GO

ALTER TABLE [Aplicaciones] ADD [Activa] bit NULL DEFAULT 0;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190810145659_AddHistorial', N'2.1.11-servicing-32099');

GO

ALTER TABLE [Preguntas] ADD [Expresion] nvarchar(450) NULL;

GO

ALTER TABLE [Preguntas] ADD [ExpresionEjemplo] nvarchar(450) NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190814133604_AddExpresionField', N'2.1.11-servicing-32099');

GO

ALTER TABLE [Estados] ADD [Abreviacion] nvarchar(2) NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190814151712_AddAbreviacionEstado', N'2.1.11-servicing-32099');

GO

ALTER TABLE [Beneficiarios] ADD [EstatusInformacion] nvarchar(450) NULL;

GO

ALTER TABLE [Beneficiarios] ADD [EstatusUpdatedAt] datetime2 NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190816140901_AddEstatusBeneficiarios', N'2.1.11-servicing-32099');

GO

ALTER TABLE [Domicilio] ADD [CadenaOCR] nvarchar(450) NULL;

GO

ALTER TABLE [Domicilio] ADD [Porcentaje] nvarchar(450) NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190820205306_AddInformacionOCR', N'2.1.11-servicing-32099');

GO

CREATE TABLE [TrabajadorRegion] (
    [Id] nvarchar(450) NOT NULL,
    [TrabajadorId] nvarchar(450) NULL,
    [ZonaId] nvarchar(450) NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NOT NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_TrabajadorRegion] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_TrabajadorRegion_Trabajadores_TrabajadorId] FOREIGN KEY ([TrabajadorId]) REFERENCES [Trabajadores] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_TrabajadorRegion_Zonas_ZonaId] FOREIGN KEY ([ZonaId]) REFERENCES [Zonas] ([Id]) ON DELETE NO ACTION
);

GO

CREATE TABLE [UsuarioRegion] (
    [Id] nvarchar(450) NOT NULL,
    [UsuarioId] nvarchar(450) NULL,
    [ZonaId] nvarchar(450) NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NOT NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_UsuarioRegion] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_UsuarioRegion_AspNetUsers_UsuarioId] FOREIGN KEY ([UsuarioId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_UsuarioRegion_Zonas_ZonaId] FOREIGN KEY ([ZonaId]) REFERENCES [Zonas] ([Id]) ON DELETE NO ACTION
);

GO

CREATE INDEX [IX_TrabajadorRegion_TrabajadorId] ON [TrabajadorRegion] ([TrabajadorId]);

GO

CREATE INDEX [IX_TrabajadorRegion_ZonaId] ON [TrabajadorRegion] ([ZonaId]);

GO

CREATE INDEX [IX_UsuarioRegion_UsuarioId] ON [UsuarioRegion] ([UsuarioId]);

GO

CREATE INDEX [IX_UsuarioRegion_ZonaId] ON [UsuarioRegion] ([ZonaId]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190826213727_Regiones', N'2.1.11-servicing-32099');

GO

ALTER TABLE [Aplicaciones] ADD [NumeroAplicacion] int NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190829221354_AgregarNumeroAplicacion', N'2.1.11-servicing-32099');

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name', N'NormalizedName') AND [object_id] = OBJECT_ID(N'[AspNetRoles]'))
    SET IDENTITY_INSERT [AspNetRoles] ON;
INSERT INTO [AspNetRoles] ([Id], [Name], [NormalizedName])
VALUES (N'1', N'Administrador', N'Administrador');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name', N'NormalizedName') AND [object_id] = OBJECT_ID(N'[AspNetRoles]'))
    SET IDENTITY_INSERT [AspNetRoles] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name', N'NormalizedName') AND [object_id] = OBJECT_ID(N'[AspNetRoles]'))
    SET IDENTITY_INSERT [AspNetRoles] ON;
INSERT INTO [AspNetRoles] ([Id], [Name], [NormalizedName])
VALUES (N'2', N'Administrador de dependencia', N'Dependencia');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name', N'NormalizedName') AND [object_id] = OBJECT_ID(N'[AspNetRoles]'))
    SET IDENTITY_INSERT [AspNetRoles] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name', N'NormalizedName') AND [object_id] = OBJECT_ID(N'[AspNetRoles]'))
    SET IDENTITY_INSERT [AspNetRoles] ON;
INSERT INTO [AspNetRoles] ([Id], [Name], [NormalizedName])
VALUES (N'3', N'Analista de dependencia', N'Analista');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name', N'NormalizedName') AND [object_id] = OBJECT_ID(N'[AspNetRoles]'))
    SET IDENTITY_INSERT [AspNetRoles] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Dependencias]'))
    SET IDENTITY_INSERT [Dependencias] ON;
INSERT INTO [Dependencias] ([Id], [Nombre], [CreatedAt], [UpdatedAt])
VALUES (N'e587835d-66ec-4cd2-813c-ce9ac91affcc', N'Secretaría de Desarrollo Social y Humano', '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Dependencias]'))
    SET IDENTITY_INSERT [Dependencias] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name', N'UserName', N'NormalizedUserName', N'Email', N'NormalizedEmail', N'EmailConfirmed', N'PasswordHash', N'SecurityStamp', N'AccessFailedCount', N'PhoneNumberConfirmed', N'TwoFactorEnabled', N'LockoutEnabled') AND [object_id] = OBJECT_ID(N'[AspNetUsers]'))
    SET IDENTITY_INSERT [AspNetUsers] ON;
INSERT INTO [AspNetUsers] ([Id], [Name], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [AccessFailedCount], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnabled])
VALUES (N'b7d2de8e-8448-4d84-9e5c-3c20bfff554a', N'Administrador', N'admin', N'admin', N'cnunez@guanajuato.gob.mx', N'cnunez@guanajuato.gob.mx', 1, N'AQAAAAEAACcQAAAAEO9N/RjFrlaYgqrUlwNORJwDQuwiKDHOna613kA+ZmEC3+uRWWsfdoM3qCLIwKydFw==', N'', 0, 0, 0, 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name', N'UserName', N'NormalizedUserName', N'Email', N'NormalizedEmail', N'EmailConfirmed', N'PasswordHash', N'SecurityStamp', N'AccessFailedCount', N'PhoneNumberConfirmed', N'TwoFactorEnabled', N'LockoutEnabled') AND [object_id] = OBJECT_ID(N'[AspNetUsers]'))
    SET IDENTITY_INSERT [AspNetUsers] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'UserId', N'RoleId') AND [object_id] = OBJECT_ID(N'[AspNetUserRoles]'))
    SET IDENTITY_INSERT [AspNetUserRoles] ON;
INSERT INTO [AspNetUserRoles] ([UserId], [RoleId])
VALUES (N'b7d2de8e-8448-4d84-9e5c-3c20bfff554a', N'1');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'UserId', N'RoleId') AND [object_id] = OBJECT_ID(N'[AspNetUserRoles]'))
    SET IDENTITY_INSERT [AspNetUserRoles] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Clave', N'Nombre', N'CreatedAt', N'UpdatedAt', N'Color') AND [object_id] = OBJECT_ID(N'[Carencias]'))
    SET IDENTITY_INSERT [Carencias] ON;
INSERT INTO [Carencias] ([Id], [Clave], [Nombre], [CreatedAt], [UpdatedAt], [Color])
VALUES (N'1', N'educativa', N'Rezago educativo', '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', N'#fff3f3');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Clave', N'Nombre', N'CreatedAt', N'UpdatedAt', N'Color') AND [object_id] = OBJECT_ID(N'[Carencias]'))
    SET IDENTITY_INSERT [Carencias] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Clave', N'Nombre', N'CreatedAt', N'UpdatedAt', N'Color') AND [object_id] = OBJECT_ID(N'[Carencias]'))
    SET IDENTITY_INSERT [Carencias] ON;
INSERT INTO [Carencias] ([Id], [Clave], [Nombre], [CreatedAt], [UpdatedAt], [Color])
VALUES (N'2', N'salud', N'Acceso a los servicios de salud', '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', N'#fffef3');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Clave', N'Nombre', N'CreatedAt', N'UpdatedAt', N'Color') AND [object_id] = OBJECT_ID(N'[Carencias]'))
    SET IDENTITY_INSERT [Carencias] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Clave', N'Nombre', N'CreatedAt', N'UpdatedAt', N'Color') AND [object_id] = OBJECT_ID(N'[Carencias]'))
    SET IDENTITY_INSERT [Carencias] ON;
INSERT INTO [Carencias] ([Id], [Clave], [Nombre], [CreatedAt], [UpdatedAt], [Color])
VALUES (N'3', N'seguridad', N'Acceso a la seguridad social', '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', N'#f6fff3');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Clave', N'Nombre', N'CreatedAt', N'UpdatedAt', N'Color') AND [object_id] = OBJECT_ID(N'[Carencias]'))
    SET IDENTITY_INSERT [Carencias] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Clave', N'Nombre', N'CreatedAt', N'UpdatedAt', N'Color') AND [object_id] = OBJECT_ID(N'[Carencias]'))
    SET IDENTITY_INSERT [Carencias] ON;
INSERT INTO [Carencias] ([Id], [Clave], [Nombre], [CreatedAt], [UpdatedAt], [Color])
VALUES (N'4', N'vivienda', N'Calidad y espacios de la vivienda', '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', N'#f3fffe');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Clave', N'Nombre', N'CreatedAt', N'UpdatedAt', N'Color') AND [object_id] = OBJECT_ID(N'[Carencias]'))
    SET IDENTITY_INSERT [Carencias] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Clave', N'Nombre', N'CreatedAt', N'UpdatedAt', N'Color') AND [object_id] = OBJECT_ID(N'[Carencias]'))
    SET IDENTITY_INSERT [Carencias] ON;
INSERT INTO [Carencias] ([Id], [Clave], [Nombre], [CreatedAt], [UpdatedAt], [Color])
VALUES (N'5', N'infraestructura', N'Acceso a los servicios básicos en la vivienda', '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', N'#f3f7ff');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Clave', N'Nombre', N'CreatedAt', N'UpdatedAt', N'Color') AND [object_id] = OBJECT_ID(N'[Carencias]'))
    SET IDENTITY_INSERT [Carencias] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Clave', N'Nombre', N'CreatedAt', N'UpdatedAt', N'Color') AND [object_id] = OBJECT_ID(N'[Carencias]'))
    SET IDENTITY_INSERT [Carencias] ON;
INSERT INTO [Carencias] ([Id], [Clave], [Nombre], [CreatedAt], [UpdatedAt], [Color])
VALUES (N'6', N'alimentacion', N'Acceso a la alimentación', '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', N'#f9f3ff');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Clave', N'Nombre', N'CreatedAt', N'UpdatedAt', N'Color') AND [object_id] = OBJECT_ID(N'[Carencias]'))
    SET IDENTITY_INSERT [Carencias] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'Vigencia', N'Activo', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Encuestas]'))
    SET IDENTITY_INSERT [Encuestas] ON;
INSERT INTO [Encuestas] ([Id], [Nombre], [Vigencia], [Activo], [CreatedAt], [UpdatedAt])
VALUES (N'1', N'CONEVAL', 24, 1, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'Vigencia', N'Activo', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Encuestas]'))
    SET IDENTITY_INSERT [Encuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Activa', N'EncuestaId', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[EncuestaVersiones]'))
    SET IDENTITY_INSERT [EncuestaVersiones] ON;
INSERT INTO [EncuestaVersiones] ([Id], [Activa], [EncuestaId], [CreatedAt], [UpdatedAt])
VALUES (N'1', 1, N'1', '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Activa', N'EncuestaId', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[EncuestaVersiones]'))
    SET IDENTITY_INSERT [EncuestaVersiones] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'969', N'Municipio vivienda', N'Listado', NULL, 0, NULL, 0, N'1', 1, 0, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', N'Municipios', 0, NULL, NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'975', N'Tipo de asentamiento', N'Listado', NULL, 0, NULL, 0, N'1', 2, 0, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', N'TipoAsentamientos', 0, NULL, NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'984', N'¿Cuántas personas viven normalmente en esta vivienda contando a los niños chiquitos y a los ancianos?', N'Numerica', NULL, 0, NULL, 0, N'1', 3, 0, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', NULL, 0, NULL, NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'987', N'¿Cuántas personas forman parte de este hogar?', N'Numerica', NULL, 0, NULL, 0, N'1', 4, 0, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', NULL, 0, NULL, NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'993', N'Parentesco con el jefe de familia', N'Listado', NULL, 0, N'?987', 1, N'1', 5, 0, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', N'Parentescos', 0, NULL, NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'24', N'¿Actualmente...', N'Listado', NULL, 0, N'?987', 1, N'1', 6, 0, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', NULL, 0, NULL, NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'998', N'Sexo habitante', N'Listado', NULL, 0, N'?987', 1, N'1', 7, 0, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', N'Sexos', 0, NULL, NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'208', N'CURP', N'Abierta', NULL, 0, N'?987', 1, N'1', 8, 1, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', NULL, 0, NULL, N'[A-Z]{1}[AEIOU]{1}[A-Z]{2}[0-9]{2}(0[1-9]|1[0-2])(0[1-9]|1[0-9]|2[0-9]|3[0-1])[HM]{1}(AS|BC|BS|CC|CS|CH|CL|CM|DF|DG|GT|GR|HG|JC|MC|MN|MS|NT|NL|OC|PL|QT|QR|SP|SL|SR|TC|TS|TL|VZ|YN|ZS|NE)[B-DF-HJ-NP-TV-Z]{3}[0-9A-Z]{1}[0-9]{1}$', N'LARS881101HGTZZR05');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'996', N'Fecha de nacimiento', N'Fecha', NULL, 0, N'?987', 1, N'1', 9, 0, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', NULL, 0, N'1', NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'997', N'¿Cuántos años cumplidos tiene?', N'Numerica', NULL, 0, N'?987', 1, N'1', 10, 0, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', NULL, 0, N'1', NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'27', N'¿sabe leer y escribir un recado?', N'Listado', N'?997,>6', 0, N'?987', 1, N'1', 11, 0, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', NULL, 0, N'1', NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'130', N'¿Cuál es el último nivel que aprobó en la escuela?', N'Listado', N'?997,>2', 0, N'?987', 1, N'1', 12, 0, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', NULL, 0, N'1', NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'131', N'¿Cuál es el último grado que aprobó en la escuela? (último año  aprobado del nivel referido en la pregunta antecedente)', N'Listado', N'?997,>6', 0, N'?987', 1, N'1', 13, 0, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', NULL, 0, N'1', NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'29', N'¿Asiste actualmente a la escuela, estancia infantil, CENDI, CADI o guardería?', N'Listado', NULL, 0, N'?987', 1, N'1', 14, 0, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', NULL, 0, N'1', NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'37', N'Tiene derecho a los servicios médicos:', N'Listado', NULL, 0, N'?987', 1, N'1', 15, 0, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', NULL, 0, N'2', NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'82', N'¿Está afiliado o inscrito a la institución por?', N'Listado', N'?37,!=102', 0, N'?987', 1, N'1', 16, 0, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', NULL, 0, N'2', NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'38', N'Cuando tiene problemas de salud, ¿En dónde se atiende?', N'Listado', NULL, 0, N'?987', 1, N'1', 17, 0, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', NULL, 0, N'2', NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'42', N'En su vida diaria, ¿tiene dificultad al realizar las siguientes actividades:', N'Listado', NULL, 0, N'?987', 1, N'1', 18, 0, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', NULL, 1, N'2', NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'33', N'¿El mes pasado…', N'Listado', N'?997,>16', 0, N'?987', 1, N'1', 19, 0, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', NULL, 0, N'3', NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'91', N'En su trabajo principal del mes pasado ¿tuvo un jefe(a) o supervisor?', N'Listado', N'?33,77', 0, N'?987', 1, N'1', 20, 0, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', NULL, 0, N'3', NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'92', N'Entonces en el trabajo principal del mes pasado de ¿se dedicó a un negocio o actividad por su cuenta?', N'Listado', N'?91,!=325', 0, N'?987', 1, N'1', 21, 0, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', NULL, 0, N'3', NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'88', N'En su trabajo principal del mes pasado ¿se desempeñó como…?', N'Listado', N'?33,77', 0, N'?987', 1, N'1', 22, 0, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', N'Ocupaciones', 0, N'3', NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'93', N'En su trabajo principal del mes pasado ¿recibió un pago?', N'Listado', N'?33,77', 0, N'?987', 1, N'1', 23, 0, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', NULL, 0, N'3', NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'34', N'¿En este trabajo le dieron las siguientes prestaciones, aunque no las haya utilizado?', N'Check', N'?33,77/78/79/80', 0, N'?987', 1, N'1', 24, 0, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', NULL, 0, N'3', NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'96', N'¿Tiene contratado voluntariamente…', N'Check', NULL, 0, N'?987', 1, N'1', 25, 0, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', NULL, 0, N'3', NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'99', N'¿Recibe dinero por ...', N'Listado', N'?997,>64', 0, N'?987', 1, N'1', 26, 0, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', NULL, 0, N'3', NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'100', N'¿Realiza regularmente las siguientes actividades? (trabajo secundario)', N'Listado', N'?997,>16', 0, N'?987', 1, N'1', 27, 0, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', NULL, 0, N'3', NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'79', N'¿Alguien en el hogar recibe dinero proveniente de otros países?', N'Listado', NULL, 0, NULL, 0, N'1', 28, 0, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', NULL, 0, N'3', NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'44', N'¿De qué material es la mayor parte del piso de esta vivienda?', N'Listado', NULL, 0, NULL, 0, N'1', 29, 0, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', NULL, 0, N'4', NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'46', N'¿De qué material es la mayor parte de las paredes o muros de esta vivienda?', N'Listado', NULL, 0, NULL, 0, N'1', 30, 0, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', NULL, 0, N'4', NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'48', N'¿De qué material es la mayor parte del techo de esta vivienda?', N'Listado', NULL, 0, NULL, 0, N'1', 31, 0, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', NULL, 0, N'4', NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'141', N'¿Cúantos cuartos tiene en total esta vivienda contando la cocina? (no cuente pasillos ni baños)', N'Numerica', NULL, 0, NULL, 0, N'1', 32, 0, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', NULL, 0, N'4', NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'51', N'¿Cuántos cuartos se usan para dormir sin contar pasillos y cocina? ', N'Numerica', NULL, 0, NULL, 0, N'1', 33, 0, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', NULL, 0, N'4', NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'113', N'¿Qué tipo de baño o escusado tiene su vivienda?', N'Listado', NULL, 0, NULL, 0, N'1', 34, 0, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', NULL, 0, N'4', NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'54', N'¿En esta vivienda tienen...', N'Listado', NULL, 0, NULL, 0, N'1', 35, 0, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', NULL, 0, N'4', NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'56', N'¿Esta vivienda tiene drenaje o desagüe conectado a...', N'Listado', NULL, 0, NULL, 0, N'1', 36, 0, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', NULL, 0, N'5', NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'57', N'¿El combustible que más usan para cocinar es...', N'Listado', NULL, 0, NULL, 0, N'1', 37, 0, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', NULL, 0, N'5', NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'58', N'¿La estufa (fogón) de leña o carbón con la que cocinan tiene chimenea?', N'Listado', N'?57,177/178', 0, NULL, 0, N'1', 38, 0, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', NULL, 0, N'5', NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'117', N'¿En esta vivienda tienen y funciona…', N'Check', NULL, 0, NULL, 0, N'1', 39, 0, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', NULL, 0, N'5', NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'118', N'En su vivienda ¿La luz eléctrica la obtienen…', N'Listado', NULL, 0, NULL, 0, N'1', 40, 0, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', NULL, 0, N'5', NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'119', N'¿La vivienda que habita es…', N'Listado', NULL, 0, NULL, 0, N'1', 41, 0, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', NULL, 0, N'5', NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'63', N'En los últimos tres meses, por falta de dinero o recursos… ¿Alguna vez usted o algún adulto en su hogar tuvo una alimentación basada en muy poca variedad de alimentos?', N'Listado', NULL, 0, NULL, 0, N'1', 42, 0, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', NULL, 0, N'6', NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'64', N'En los últimos tres meses, por falta de dinero o recursos… ¿Alguna vez usted o algún adulto en su hogar dejó de desayunar, comer o cenar?', N'Listado', NULL, 0, NULL, 0, N'1', 43, 0, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', NULL, 0, N'6', NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'65', N'En los últimos tres meses, por falta de dinero o recursos… ¿Alguna vez usted o algún adulto en su hogar comió menos de lo que usted piensa debía comer?', N'Listado', NULL, 0, NULL, 0, N'1', 44, 0, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', NULL, 0, N'6', NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'66', N'En los últimos tres meses, por falta de dinero o recursos… ¿Alguna vez usted o algún adulto en su hogar se quedaron sin comida?', N'Listado', NULL, 0, NULL, 0, N'1', 45, 0, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', NULL, 0, N'6', NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'67', N'En los últimos tres meses, por falta de dinero o recursos… ¿Alguna vez usted o algún adulto de este hogar sintió hambre pero no comió?', N'Listado', NULL, 0, NULL, 0, N'1', 46, 0, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', NULL, 0, N'6', NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'68', N'En los últimos tres meses, por falta de dinero o recursos… ¿Alguna vez usted o algún adulto en su hogar solo comió una vez al día o dejo de comer todo un día?', N'Listado', NULL, 0, NULL, 0, N'1', 47, 0, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', NULL, 0, N'6', NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'69', N'En los últimos tres meses, por falta de dinero o recursos… ¿Alguna vez algún menor de 18 años en su hogar tuvo una alimentación basada en muy poca variedad de alimentos?', N'Listado', N'?997,<18', 1, NULL, 0, N'1', 48, 0, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', NULL, 0, N'6', NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'70', N'En los últimos tres meses, por falta de dinero o recursos… ¿Alguna vez algún menor de 18 años en su hogar comió menos de lo que debía?', N'Listado', N'?997,<18', 1, NULL, 0, N'1', 49, 0, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', NULL, 0, N'6', NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'71', N'En los últimos tres meses, por falta de dinero o recursos, ¿alguna vez tuvieron que disminuir la cantidad servida en las comidas a algún menor de 18 años del hogar?', N'Listado', N'?997,<18', 1, NULL, 0, N'1', 50, 0, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', NULL, 0, N'6', NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'72', N'En los últimos tres meses, por falta de dinero o recursos, ¿alguna vez algún menor de 18 años en su hogar sintió hambre pero no comió?', N'Listado', N'?997,<18', 1, NULL, 0, N'1', 51, 0, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', NULL, 0, N'6', NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'73', N'En los últimos tres meses, por falta de dinero o recursos, ¿alguna vez algún menor de 18 años en su hogar se acostó con hambre?', N'Listado', N'?997,<18', 1, NULL, 0, N'1', 52, 0, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', NULL, 0, N'6', NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'74', N'En los últimos tres meses, por falta de dinero o recursos, ¿alguna vez algún menor de 18 años en su hogar comió una vez al día o dejó de comer todo un día?', N'Listado', N'?997,<18', 1, NULL, 0, N'1', 53, 0, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', NULL, 0, N'6', NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'358', N'Cuidar sin pago y de manera exclusiva a niños, enfermos, adultos mayores o discapacitados', N'100', 1, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'359', N'Trabajo comunitario o voluntario', N'100', 2, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'360', N'Reparaciones a la vivienda, aparatos domésticos o vehículos.', N'100', 3, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'361', N'Realizar el quehacer de su hogar', N'100', 4, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'362', N'Acarrear agua o leña', N'100', 5, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'623', N'Ninguno', N'100', 6, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'404', N'Con conexión de agua/tiene descarga directa de agua', N'113', 1, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'405', N'Le echan agua con cubeta', N'113', 2, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'406', N'Sin admisión de agua (letrina seca o húmeda)', N'113', 3, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'407', N'Pozo u hoyo negro', N'113', 4, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'408', N'No tiene', N'113', 5, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'423', N'Refrigerador', N'117', 1, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'424', N'Lavadora automática', N'117', 2, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'425', N'VHS, DVD, BLU-RAY', N'117', 3, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'426', N'Vehículo (carro, camioneta o camión)', N'117', 4, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'427', N'Teléfono (fijo)', N'117', 5, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'428', N'Horno (microondas o eléctrico)', N'117', 6, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'429', N'Computadora', N'117', 7, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'430', N'Estufa / parrilla de gas', N'117', 8, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'431', N'Calentador de agua/ boiler de gas', N'117', 9, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'432', N'Calentador de agua/ solar', N'117', 10, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'433', N'Calentador de agua/ boiler electrico', N'117', 11, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'434', N'Internet', N'117', 12, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'435', N'Teléfono celular', N'117', 13, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'436', N'Aparato de televisión', N'117', 14, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'437', N'Aparato de televisión digital', N'117', 15, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'438', N'Servicio de televisión de paga (antena parabólica, SKY o TV por cable)', N'117', 16, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'440', N'Tinaco', N'117', 17, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'441', N'Aparato para regular la temperatura (ventilador, enfriador, clima, calefactor)', N'117', 18, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'627', N'Ninguno', N'117', 19, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'618', N'Regadera', N'117', 20, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'443', N'Del servicio público', N'118', 1, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'444', N'De una planta particular', N'118', 2, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'445', N'De panel solar', N'118', 3, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'446', N'De otra fuente', N'118', 4, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'447', N'No tienen luz eléctrica', N'118', 5, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'448', N'Propia y totalmente pagada', N'119', 1, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'449', N'Propia y la está pagando', N'119', 2, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'450', N'Propia y está hipotecada', N'119', 3, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'451', N'Rentada o alquilada', N'119', 4, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'452', N'Prestada o la está cuidando', N'119', 5, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'453', N'Intestada o está en litigio', N'119', 6, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'506', N'Kínder o preescolar', N'130', 1, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'507', N'Primaria', N'130', 2, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'508', N'Secundaria', N'130', 3, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'509', N'Preparatoria o Bachillerato', N'130', 4, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'510', N'Normal básica', N'130', 5, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'511', N'Carrera técnica o comercial con primaria completa', N'130', 6, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'512', N'Carrera técnica o comercial con secundaria completa', N'130', 7, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'513', N'Carrera técnica o comercial con preparatoria completa', N'130', 8, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'514', N'Profesional', N'130', 9, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'515', N'Posgrado (maestría o doctorado)', N'130', 10, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'566', N'Ninguno', N'130', 11, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'567', N'1 años', N'131', 1, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'568', N'2 años', N'131', 2, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'569', N'3 años', N'131', 3, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'570', N'4 años', N'131', 4, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'271', N'5 años', N'131', 5, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'572', N'6 años', N'131', 6, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'3', N'Vive en unión libre?', N'24', 1, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'4', N'Es casada(o)?', N'24', 2, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'5', N'Es separada(o)?', N'24', 3, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'6', N'Es divorciada(o)?', N'24', 4, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'7', N'Está viuda(o)?', N'24', 5, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'8', N'Es soltera(o)?', N'24', 6, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'35', N'No', N'27', 1, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'34', N'Si', N'27', 2, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'573', N'No sabe/No contesta', N'27', 3, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'49', N'Si', N'29', 1, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'50', N'No', N'29', 2, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'77', N'Trabajó (por lo menos una hora)', N'33', 1, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'78', N'Tenía trabajo, pero no trabajó', N'33', 2, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'79', N'Buscó trabajo', N'33', 3, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'80', N'Es pensionada(o) o jubilada(o)', N'33', 4, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'81', N'Es estudiante', N'33', 5, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'82', N'Se dedica a los quehaceres de su hogar', N'33', 6, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'83', N'Tiene alguna limitación física o mental permanente que le impide trabajar', N'33', 7, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'84', N'Estaba en otra situación diferente a las anteriores', N'33', 8, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'85', N'Incapacidad por enfermedad, accidente o maternidad', N'34', 1, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'86', N'SAR o AFORE', N'34', 2, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'87', N'Crédito para vivienda', N'34', 3, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'88', N'Guardería', N'34', 4, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'89', N'Aguinaldo', N'34', 5, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'90', N'Seguro de vida', N'34', 6, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'91', N'No tiene derecho a ninguna de estas prestaciones', N'34', 7, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 1);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'92', N'No sabe / no responde', N'34', 8, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 1);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'95', N'Del Seguro Social IMSS', N'37', 1, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'96', N'Del ISSSTE', N'37', 2, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'97', N'Del ISSSTE estatal', N'37', 3, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'98', N'PEMEX, Defensa o Marina', N'37', 4, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'99', N'Del Seguro Popular para una nueva generación', N'37', 5, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'100', N'De un seguro privado', N'37', 6, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'101', N'En otra institucións', N'37', 7, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'102', N'Entonces ¿no tiene derecho a servicios médicos?', N'37', 8, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'103', N'Seguro Social IMSS', N'38', 1, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'104', N'ISSSTE', N'38', 2, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'105', N'ISSSTE estatal', N'38', 3, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'106', N'PEMEX, Defensa o Marina', N'38', 4, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'107', N'Centro de Salud u Hospital de la SSA (Seguro Popular)', N'38', 5, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'108', N'IMSS-PROSPERA', N'38', 6, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'1082', N'Consultorio de una farmacia', N'38', 7, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'1083', N'Se automedica', N'38', 8, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'109', N'Consultorio, clinica u hospital privado', N'38', 9, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'110', N'Otro lugar', N'38', 10, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'111', N'No se atiende', N'38', 11, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'112', N'En otra institución', N'38', 12, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'113', N'Entonces ¿no tiene derecho a servicios médicos?', N'38', 13, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'117', N'Caminar, moverse, subir o bajar', N'42', 1, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'118', N'Ver, aun usando lentes', N'42', 2, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'119', N'Hablar, comunicarse o conversar', N'42', 3, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'120', N'Oír, aun usando aparato auditivo', N'42', 4, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'121', N'Vestirse, bañarse o comer', N'42', 5, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'122', N'Poner atención o aprender cosas sencillas', N'42', 6, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'123', N'Tiene alguna limitación mental', N'42', 7, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'124', N'Entonces, ¿No tiene dificultad física o mental?', N'42', 8, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 1);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'130', N'Tierra', N'44', 1, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'131', N'Cemento o firme', N'44', 2, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'132', N'Madera, mosaico u otro recubrimiento', N'44', 3, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'135', N'Material de desecho', N'46', 1, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'136', N'Lámina de cartón', N'46', 2, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'137', N'Lámina de asbesto o metálica', N'46', 3, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'138', N'Carrizo, bambú o palma', N'46', 4, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'139', N'Embarro o bajareque', N'46', 5, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'140', N'Madera', N'46', 6, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'141', N'Adobe', N'46', 7, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'142', N'Tabique, ladrillo, block, piedra, cantera, cemento o concreto', N'46', 8, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'145', N'Material de desecho', N'48', 1, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'146', N'Lámina de cartón', N'48', 2, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'147', N'Lámina metálica', N'48', 3, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'148', N'Lámina de asbesto', N'48', 4, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'149', N'Palma o paja', N'48', 5, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'150', N'Madera o tejamanil', N'48', 6, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'151', N'Terrado con viguería', N'48', 7, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'152', N'Teja', N'48', 8, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'153', N'Losa de concreto o viguetas con bovedilla', N'48', 9, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'162', N'Agua entubada dentro de la vivienda', N'54', 1, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'163', N'Agua entubada fuera de la vivienda,pero dentro del terreno', N'54', 2, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'164', N'Agua entubada de llave pública (o hidrante)', N'54', 3, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'165', N'Agua entubada que acarrean de otra vivienda', N'54', 4, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'166', N'Agua de pipa', N'54', 5, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'167', N'Agua de un pozo, río, lago, arroyo u otra', N'54', 6, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'643', N'Agua captada de lluvia u otro medio', N'54', 7, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'170', N'La red pública', N'56', 1, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'171', N'Una fosa séptica', N'56', 2, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'172', N'Una tubería que va a dar a una barranca o grieta', N'56', 3, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'173', N'Una tubería que va a dar a un río, lago o mar', N'56', 4, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'174', N'No tiene drenaje', N'56', 5, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'620', N'Biodigestor', N'56', 6, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'175', N'Gas de cilindro o tanque (estacionario)', N'57', 1, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'176', N'Gas natural o de tubería', N'57', 2, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'177', N'Leña', N'57', 3, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'178', N'Carbón', N'57', 4, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'179', N'Electricidad', N'57', 5, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'180', N'Otro combustible', N'57', 6, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'181', N'Si', N'58', 1, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'182', N'No', N'58', 2, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'206', N'Si', N'63', 1, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'207', N'No', N'63', 2, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'208', N'Si', N'64', 1, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'209', N'No', N'64', 2, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'210', N'Si', N'65', 1, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'211', N'No', N'65', 2, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'212', N'Si', N'66', 1, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'213', N'No', N'66', 2, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'214', N'Si', N'67', 1, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'215', N'No', N'67', 2, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'216', N'Si', N'68', 1, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'217', N'No', N'68', 2, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'218', N'Si', N'69', 1, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'219', N'No', N'69', 2, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'220', N'Si', N'70', 1, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'221', N'No', N'70', 2, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'222', N'Si', N'71', 1, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'223', N'No', N'71', 2, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'224', N'Si', N'72', 1, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'225', N'No', N'72', 2, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'226', N'Si', N'73', 1, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'227', N'No', N'73', 2, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'228', N'Si', N'74', 1, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'229', N'No', N'74', 2, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'247', N'Si', N'79', 1, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'248', N'No', N'79', 2, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'249', N'No sabe', N'79', 3, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'250', N'No contesta', N'79', 4, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'256', N'Prestación en el trabajo', N'82', 1, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'257', N'Jubilación', N'82', 2, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'258', N'Invalidez', N'82', 3, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'259', N'Algún familiar en el hogar', N'82', 4, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'260', N'Muerte del asegurado', N'82', 5, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'261', N'Ser estudiante', N'82', 6, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'262', N'Contratación propia', N'82', 7, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'263', N'Contratación propia', N'82', 8, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'264', N'Apoyo del gobierno', N'82', 9, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'265', N'No sabe/No responde', N'82', 10, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'325', N'Si', N'91', 1, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'326', N'No', N'91', 1, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'327', N'No sabe/No responde', N'91', 1, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'328', N'Si', N'91', 1, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'329', N'No', N'91', 2, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'330', N'No sabe/No responde', N'91', 3, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'331', N'Si', N'93', 1, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'332', N'No', N'93', 2, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'333', N'No sabe/No responde', N'93', 3, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'341', N'SAR, AFORE o fondo de retiro', N'96', 1, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'342', N'Seguro privado de gastos médicos', N'96', 2, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'343', N'Seguro de vida', N'96', 3, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'344', N'Seguro de invalidez', N'96', 4, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'345', N'Otro tipo de seguro', N'96', 5, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'346', N'Ninguno de los anteriores', N'96', 6, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 1);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'347', N'No sabe/No responde', N'96', 7, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 1);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'353', N'Programa Pensión del Programa para Adultos Mayores', N'99', 1, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'354', N'Componente de apoyo del Programa para Adultos Mayores  (PROSPERA)', N'99', 2, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'355', N'Otros Programas para Adultos Mayores (Estatal o Municipal)', N'99', 3, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'356', N'Ninguno', N'99', 4, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'357', N'No sabe/No responde', N'99', 5, '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'PreguntaId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[PreguntaGrado]'))
    SET IDENTITY_INSERT [PreguntaGrado] ON;
INSERT INTO [PreguntaGrado] ([Id], [PreguntaId], [Grado], [CreatedAt], [UpdatedAt])
VALUES (N'1', N'42', N'No puede hacerlo', '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'PreguntaId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[PreguntaGrado]'))
    SET IDENTITY_INSERT [PreguntaGrado] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'PreguntaId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[PreguntaGrado]'))
    SET IDENTITY_INSERT [PreguntaGrado] ON;
INSERT INTO [PreguntaGrado] ([Id], [PreguntaId], [Grado], [CreatedAt], [UpdatedAt])
VALUES (N'2', N'42', N'Lo hace con mucha dificultad', '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'PreguntaId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[PreguntaGrado]'))
    SET IDENTITY_INSERT [PreguntaGrado] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'PreguntaId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[PreguntaGrado]'))
    SET IDENTITY_INSERT [PreguntaGrado] ON;
INSERT INTO [PreguntaGrado] ([Id], [PreguntaId], [Grado], [CreatedAt], [UpdatedAt])
VALUES (N'3', N'42', N'Lo hace con poca dificultad', '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'PreguntaId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[PreguntaGrado]'))
    SET IDENTITY_INSERT [PreguntaGrado] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'PreguntaId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[PreguntaGrado]'))
    SET IDENTITY_INSERT [PreguntaGrado] ON;
INSERT INTO [PreguntaGrado] ([Id], [PreguntaId], [Grado], [CreatedAt], [UpdatedAt])
VALUES (N'4', N'42', N'No tiene dificultad', '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'PreguntaId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[PreguntaGrado]'))
    SET IDENTITY_INSERT [PreguntaGrado] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] ON;
INSERT INTO [RespuestaGrado] ([Id], [RespuestaId], [PreguntaGradoId], [Grado], [CreatedAt], [UpdatedAt])
VALUES (N'1', N'117', N'1', N'11', '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] ON;
INSERT INTO [RespuestaGrado] ([Id], [RespuestaId], [PreguntaGradoId], [Grado], [CreatedAt], [UpdatedAt])
VALUES (N'2', N'117', N'2', N'12', '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] ON;
INSERT INTO [RespuestaGrado] ([Id], [RespuestaId], [PreguntaGradoId], [Grado], [CreatedAt], [UpdatedAt])
VALUES (N'3', N'117', N'3', N'13', '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] ON;
INSERT INTO [RespuestaGrado] ([Id], [RespuestaId], [PreguntaGradoId], [Grado], [CreatedAt], [UpdatedAt])
VALUES (N'4', N'117', N'4', N'14', '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] ON;
INSERT INTO [RespuestaGrado] ([Id], [RespuestaId], [PreguntaGradoId], [Grado], [CreatedAt], [UpdatedAt])
VALUES (N'5', N'118', N'1', N'21', '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] ON;
INSERT INTO [RespuestaGrado] ([Id], [RespuestaId], [PreguntaGradoId], [Grado], [CreatedAt], [UpdatedAt])
VALUES (N'6', N'118', N'2', N'22', '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] ON;
INSERT INTO [RespuestaGrado] ([Id], [RespuestaId], [PreguntaGradoId], [Grado], [CreatedAt], [UpdatedAt])
VALUES (N'7', N'118', N'3', N'23', '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] ON;
INSERT INTO [RespuestaGrado] ([Id], [RespuestaId], [PreguntaGradoId], [Grado], [CreatedAt], [UpdatedAt])
VALUES (N'8', N'118', N'4', N'24', '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] ON;
INSERT INTO [RespuestaGrado] ([Id], [RespuestaId], [PreguntaGradoId], [Grado], [CreatedAt], [UpdatedAt])
VALUES (N'9', N'119', N'1', N'31', '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] ON;
INSERT INTO [RespuestaGrado] ([Id], [RespuestaId], [PreguntaGradoId], [Grado], [CreatedAt], [UpdatedAt])
VALUES (N'10', N'119', N'2', N'32', '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] ON;
INSERT INTO [RespuestaGrado] ([Id], [RespuestaId], [PreguntaGradoId], [Grado], [CreatedAt], [UpdatedAt])
VALUES (N'11', N'119', N'3', N'33', '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] ON;
INSERT INTO [RespuestaGrado] ([Id], [RespuestaId], [PreguntaGradoId], [Grado], [CreatedAt], [UpdatedAt])
VALUES (N'12', N'119', N'4', N'34', '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] ON;
INSERT INTO [RespuestaGrado] ([Id], [RespuestaId], [PreguntaGradoId], [Grado], [CreatedAt], [UpdatedAt])
VALUES (N'13', N'120', N'1', N'41', '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] ON;
INSERT INTO [RespuestaGrado] ([Id], [RespuestaId], [PreguntaGradoId], [Grado], [CreatedAt], [UpdatedAt])
VALUES (N'14', N'120', N'2', N'42', '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] ON;
INSERT INTO [RespuestaGrado] ([Id], [RespuestaId], [PreguntaGradoId], [Grado], [CreatedAt], [UpdatedAt])
VALUES (N'15', N'120', N'3', N'43', '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] ON;
INSERT INTO [RespuestaGrado] ([Id], [RespuestaId], [PreguntaGradoId], [Grado], [CreatedAt], [UpdatedAt])
VALUES (N'16', N'120', N'4', N'44', '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] ON;
INSERT INTO [RespuestaGrado] ([Id], [RespuestaId], [PreguntaGradoId], [Grado], [CreatedAt], [UpdatedAt])
VALUES (N'17', N'121', N'1', N'51', '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] ON;
INSERT INTO [RespuestaGrado] ([Id], [RespuestaId], [PreguntaGradoId], [Grado], [CreatedAt], [UpdatedAt])
VALUES (N'18', N'121', N'2', N'52', '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] ON;
INSERT INTO [RespuestaGrado] ([Id], [RespuestaId], [PreguntaGradoId], [Grado], [CreatedAt], [UpdatedAt])
VALUES (N'19', N'121', N'3', N'53', '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] ON;
INSERT INTO [RespuestaGrado] ([Id], [RespuestaId], [PreguntaGradoId], [Grado], [CreatedAt], [UpdatedAt])
VALUES (N'20', N'121', N'4', N'54', '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] ON;
INSERT INTO [RespuestaGrado] ([Id], [RespuestaId], [PreguntaGradoId], [Grado], [CreatedAt], [UpdatedAt])
VALUES (N'21', N'122', N'1', N'61', '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] ON;
INSERT INTO [RespuestaGrado] ([Id], [RespuestaId], [PreguntaGradoId], [Grado], [CreatedAt], [UpdatedAt])
VALUES (N'22', N'122', N'2', N'62', '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] ON;
INSERT INTO [RespuestaGrado] ([Id], [RespuestaId], [PreguntaGradoId], [Grado], [CreatedAt], [UpdatedAt])
VALUES (N'23', N'122', N'3', N'63', '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] ON;
INSERT INTO [RespuestaGrado] ([Id], [RespuestaId], [PreguntaGradoId], [Grado], [CreatedAt], [UpdatedAt])
VALUES (N'24', N'122', N'4', N'64', '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] ON;
INSERT INTO [RespuestaGrado] ([Id], [RespuestaId], [PreguntaGradoId], [Grado], [CreatedAt], [UpdatedAt])
VALUES (N'25', N'123', N'1', N'71', '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] ON;
INSERT INTO [RespuestaGrado] ([Id], [RespuestaId], [PreguntaGradoId], [Grado], [CreatedAt], [UpdatedAt])
VALUES (N'26', N'123', N'2', N'72', '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] ON;
INSERT INTO [RespuestaGrado] ([Id], [RespuestaId], [PreguntaGradoId], [Grado], [CreatedAt], [UpdatedAt])
VALUES (N'27', N'123', N'3', N'73', '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] ON;
INSERT INTO [RespuestaGrado] ([Id], [RespuestaId], [PreguntaGradoId], [Grado], [CreatedAt], [UpdatedAt])
VALUES (N'28', N'123', N'4', N'74', '2019-09-05T12:29:47.7382069-05:00', '2019-09-05T12:29:47.7382069-05:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] OFF;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190830185112_Seeder', N'2.1.11-servicing-32099');

GO


[40m[32minfo[39m[22m[49m: Microsoft.EntityFrameworkCore.Infrastructure[10403]
      Entity Framework Core 2.1.11-servicing-32099 initialized 'ApplicationDbContext' using provider 'Microsoft.EntityFrameworkCore.SqlServer' with options: None
Inicial
Preguntas
Respuestas
PreguntaGrado
RespuestaGrado
IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

CREATE TABLE [AspNetRoles] (
    [Id] nvarchar(450) NOT NULL,
    [Name] nvarchar(256) NULL,
    [NormalizedName] nvarchar(256) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [AspNetUsers] (
    [Id] nvarchar(450) NOT NULL,
    [Name] nvarchar(256) NOT NULL,
    [UserName] nvarchar(256) NULL,
    [NormalizedUserName] nvarchar(256) NULL,
    [Email] nvarchar(256) NULL,
    [NormalizedEmail] nvarchar(256) NULL,
    [EmailConfirmed] bit NOT NULL,
    [PasswordHash] nvarchar(max) NULL,
    [SecurityStamp] nvarchar(max) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    [PhoneNumber] nvarchar(max) NULL,
    [PhoneNumberConfirmed] bit NOT NULL,
    [TwoFactorEnabled] bit NOT NULL,
    [LockoutEnd] datetimeoffset NULL,
    [LockoutEnabled] bit NOT NULL,
    [AccessFailedCount] int NOT NULL,
    CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [AspNetRoleClaims] (
    [Id] int NOT NULL IDENTITY,
    [RoleId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id])
);

GO

CREATE TABLE [AspNetUserClaims] (
    [Id] int NOT NULL IDENTITY,
    [UserId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id])
);

GO

CREATE TABLE [AspNetUserLogins] (
    [LoginProvider] nvarchar(450) NOT NULL,
    [ProviderKey] nvarchar(450) NOT NULL,
    [ProviderDisplayName] nvarchar(max) NULL,
    [UserId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
    CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [AspNetUserRoles] (
    [UserId] nvarchar(450) NOT NULL,
    [RoleId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
    CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [AspNetUserTokens] (
    [UserId] nvarchar(450) NOT NULL,
    [LoginProvider] nvarchar(450) NOT NULL,
    [Name] nvarchar(450) NOT NULL,
    [Value] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
    CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [Trabajadores] (
    [Id] nvarchar(450) NOT NULL,
    [Nombre] nvarchar(100) NULL,
    [Username] nvarchar(50) NULL,
    [Password] nvarchar(450) NULL,
    [Email] nvarchar(100) NULL,
    [CreatedAt] datetime2 NULL,
    [UpdatedAt] datetime2 NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_Trabajadores] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [Dependencias] (
    [Id] nvarchar(450) NOT NULL,
    [Nombre] nvarchar(250) NULL,
    [CreatedAt] datetime2 NULL,
    [UpdatedAt] datetime2 NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_Dependencias] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [TrabajadoresDependencias] (
    [Id] nvarchar(450) NOT NULL,
    [DependenciaId] nvarchar(450) NOT NULL,
    [UsuarioId] nvarchar(450) NOT NULL,
    [CreatedAt] datetime2 NULL,
    [UpdatedAt] datetime2 NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_TrabajadoresDependencias] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_TrabajadoresDependencias_Dependencia] FOREIGN KEY ([DependenciaId]) REFERENCES [Dependencias] ([Id]),
    CONSTRAINT [FK_TrabajadoresDependencias_Trabajadores] FOREIGN KEY ([UsuarioId]) REFERENCES [Trabajadores] ([Id])
);

GO

CREATE TABLE [UsuariosDependencias] (
    [Id] nvarchar(450) NOT NULL,
    [DependenciaId] nvarchar(450) NOT NULL,
    [UsuarioId] nvarchar(450) NOT NULL,
    [CreatedAt] datetime2 NULL,
    [UpdatedAt] datetime2 NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_UsuariosDependencias] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_UsuariosDependencias_Dependencia] FOREIGN KEY ([DependenciaId]) REFERENCES [Dependencias] ([Id]),
    CONSTRAINT [FK_UsuariosDependencias_Trabajadores] FOREIGN KEY ([UsuarioId]) REFERENCES [AspNetUsers] ([Id])
);

GO

CREATE TABLE [ProgramasSociales] (
    [Id] nvarchar(450) NOT NULL,
    [Nombre] nvarchar(250) NULL,
    [DependenciaId] nvarchar(450) NOT NULL,
    [CreatedAt] datetime2 NULL,
    [UpdatedAt] datetime2 NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_ProgramasSociales] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ProgramasSociales_Dependencias] FOREIGN KEY ([DependenciaId]) REFERENCES [Dependencias] ([Id])
);

GO

CREATE TABLE [Vertientes] (
    [Id] nvarchar(450) NOT NULL,
    [Nombre] nvarchar(250) NULL,
    [ProgramaSocialId] nvarchar(450) NOT NULL,
    [Inmediato] bit NOT NULL,
    [CreatedAt] datetime2 NULL,
    [UpdatedAt] datetime2 NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_Vertientes] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Vertientes_ProgramasSociales] FOREIGN KEY ([ProgramaSocialId]) REFERENCES [ProgramasSociales] ([Id])
);

GO

CREATE TABLE [Beneficiarios] (
    [Id] nvarchar(450) NOT NULL,
    [ApellidoPaterno] nvarchar(256) NULL,
    [ApellidoMaterno] nvarchar(256) NULL,
    [Nombre] nvarchar(256) NULL,
    [FechaNacimiento] datetime2 NULL,
    [Sexo] nvarchar(10) NULL,
    [EstadoNacimiento] nvarchar(50) NULL,
    [Curp] nvarchar(18) NULL,
    [Rfc] nvarchar(13) NULL,
    [Comentarios] nvarchar(450) NULL,
    [CreatedAt] datetime2 NULL,
    [UpdatedAt] datetime2 NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_Beneficiarios] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [Archivos] (
    [Id] nvarchar(450) NOT NULL,
    [Nombre] nvarchar(450) NULL,
    [Base64] text NOT NULL,
    [CreatedAt] datetime2 NULL,
    [UpdatedAt] datetime2 NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_Archivos] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [Domicilio] (
    [Id] nvarchar(450) NOT NULL,
    [ClaveAGEB] nvarchar(4) NULL,
    [Telefono] nvarchar(13) NULL,
    [TelefonoCasa] nvarchar(13) NULL,
    [Email] nvarchar(max) NULL,
    [DomicilioN] nvarchar(100) NULL,
    [EntreCalle1] nvarchar(100) NULL,
    [EntreCalle2] nvarchar(100) NULL,
    [Latitud] float NULL,
    [Longitud] float NULL,
    [LatitudAtm] float NULL,
    [LongitudAtm] float NULL,
    [CodigoPostal] nvarchar(10) NULL,
    [BeneficiarioId] nvarchar(450) NOT NULL,
    [CreatedAt] datetime2 NULL,
    [UpdatedAt] datetime2 NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_Domicilio] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Beneficiario_Domicilio] FOREIGN KEY ([BeneficiarioId]) REFERENCES [Beneficiarios] ([Id])
);

GO

CREATE TABLE [Configuracion] (
    [Id] nvarchar(450) NOT NULL,
    [MetrosRadio] float NULL,
    [CreatedAt] datetime2 NULL,
    [UpdatedAt] datetime2 NULL,
    CONSTRAINT [PK_Configuracion] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [Encuestas] (
    [Id] nvarchar(450) NOT NULL,
    [Nombre] nvarchar(255) NULL,
    [Vigencia] int NULL,
    [Activo] bit NULL,
    [CreatedAt] datetime2 NULL,
    [UpdatedAt] datetime2 NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_Encuestas] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [EncuestaVersiones] (
    [Id] nvarchar(450) NOT NULL,
    [Activa] bit NULL,
    [EncuestaId] nvarchar(450) NULL,
    [CreatedAt] datetime2 NULL,
    [UpdatedAt] datetime2 NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_EncuestaVersiones] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_EncuestaVersiones_Encuesta] FOREIGN KEY ([EncuestaId]) REFERENCES [Encuestas] ([Id])
);

GO

CREATE TABLE [Preguntas] (
    [Id] nvarchar(450) NOT NULL,
    [Nombre] nvarchar(255) NULL,
    [TipoPregunta] nvarchar(100) NULL,
    [Condicion] nvarchar(450) NULL,
    [CondicionLista] bit NULL,
    [CondicionIterable] nvarchar(450) NULL,
    [Iterable] bit NULL,
    [EncuestaVersionId] nvarchar(450) NULL,
    [Numero] int NULL,
    [Editable] bit NULL,
    [CreatedAt] datetime2 NULL,
    [UpdatedAt] datetime2 NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_Preguntas] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_EncuestaVersion_Pregunta] FOREIGN KEY ([EncuestaVersionId]) REFERENCES [EncuestaVersiones] ([Id])
);

GO

CREATE TABLE [Respuestas] (
    [Id] nvarchar(450) NOT NULL,
    [Nombre] nvarchar(255) NULL,
    [PreguntaId] nvarchar(450) NULL,
    [Numero] int NULL,
    [CreatedAt] datetime2 NULL,
    [UpdatedAt] datetime2 NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_Respuestas] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Preguntas_Respuestas] FOREIGN KEY ([PreguntaId]) REFERENCES [Preguntas] ([Id])
);

GO

CREATE TABLE [Aplicaciones] (
    [Id] nvarchar(450) NOT NULL,
    [Estatus] nvarchar(450) NOT NULL,
    [BeneficiarioId] nvarchar(450) NOT NULL,
    [EncuestaVersionId] nvarchar(450) NULL,
    [CreatedAt] datetime2 NULL,
    [UpdatedAt] datetime2 NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_Aplicaciones] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Aplicaciones_EncuestaVersion] FOREIGN KEY ([EncuestaVersionId]) REFERENCES [EncuestaVersiones] ([Id]),
    CONSTRAINT [FK_Beneficiarios_Aplicaciones] FOREIGN KEY ([BeneficiarioId]) REFERENCES [Beneficiarios] ([Id])
);

GO

CREATE TABLE [AplicacionPreguntas] (
    [Id] nvarchar(450) NOT NULL,
    [Valor] nvarchar(255) NULL,
    [AplicacionId] nvarchar(450) NULL,
    [PreguntaId] nvarchar(450) NULL,
    [RespuestaId] nvarchar(450) NULL,
    [RespuestaValor] float NULL,
    [RespuestaIteracion] nvarchar(2) NULL,
    [CreatedAt] datetime2 NULL,
    [UpdatedAt] datetime2 NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_AplicacionPreguntas] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Aplicaciones_AplicacionPreguntas] FOREIGN KEY ([AplicacionId]) REFERENCES [Aplicaciones] ([Id]),
    CONSTRAINT [FK_Preguntas_AplicacionPreguntas] FOREIGN KEY ([PreguntaId]) REFERENCES [Preguntas] ([Id]),
    CONSTRAINT [FK_Respuesta_AplicacionPreguntas] FOREIGN KEY ([RespuestaId]) REFERENCES [Respuestas] ([Id])
);

GO

CREATE TABLE [Solicitudes] (
    [Id] nvarchar(450) NOT NULL,
    [BeneficiarioId] nvarchar(450) NOT NULL,
    [TrabajadorId] nvarchar(450) NOT NULL,
    [VertienteId] nvarchar(450) NOT NULL,
    [AplicacionId] nvarchar(450) NOT NULL,
    [Estatus] nvarchar(450) NOT NULL,
    [CreatedAt] datetime2 NULL,
    [UpdatedAt] datetime2 NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_Solicitudes] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Beneficiario_Solicitudes] FOREIGN KEY ([BeneficiarioId]) REFERENCES [Beneficiarios] ([Id]),
    CONSTRAINT [FK_Trabajador_Solicitudes] FOREIGN KEY ([TrabajadorId]) REFERENCES [Trabajadores] ([Id]),
    CONSTRAINT [FK_Vertientes_Solicitudes] FOREIGN KEY ([VertienteId]) REFERENCES [Vertientes] ([Id]),
    CONSTRAINT [FK_Aplicacion_Solicitudes] FOREIGN KEY ([AplicacionId]) REFERENCES [Aplicaciones] ([Id])
);

GO

CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);

GO

CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL;

GO

CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);

GO

CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId]);

GO

CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);

GO

CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]);

GO

CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190429182557_InitialMigration', N'2.1.11-servicing-32099');

GO

CREATE TABLE [Estados] (
    [Id] nvarchar(450) NOT NULL,
    [Clave] nvarchar(256) NULL,
    [Nombre] nvarchar(256) NULL,
    [CreatedAt] datetime2 NULL,
    [UpdatedAt] datetime2 NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_Estados] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [Municipios] (
    [Id] nvarchar(450) NOT NULL,
    [Clave] nvarchar(256) NULL,
    [Nombre] nvarchar(256) NULL,
    [EstadoId] nvarchar(450) NULL,
    [CreatedAt] datetime2 NULL,
    [UpdatedAt] datetime2 NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_Municipios] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Municipios_Estados] FOREIGN KEY ([EstadoId]) REFERENCES [Estados] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [Localidades] (
    [Id] nvarchar(450) NOT NULL,
    [Clave] nvarchar(256) NULL,
    [Nombre] nvarchar(256) NULL,
    [Indigena] nvarchar(256) NULL,
    [MunicipioId] nvarchar(450) NULL,
    [EstadoId] nvarchar(450) NULL,
    [CreatedAt] datetime2 NULL,
    [UpdatedAt] datetime2 NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_Localidades] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Localidades_Municipios] FOREIGN KEY ([MunicipioId]) REFERENCES [Municipios] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Localidades_Estados] FOREIGN KEY ([EstadoId]) REFERENCES [Estados] ([Id])
);

GO

CREATE TABLE [Agebs] (
    [Id] nvarchar(450) NOT NULL,
    [Clave] nvarchar(256) NULL,
    [Nombre] nvarchar(256) NULL,
    [LocalidadId] nvarchar(450) NULL,
    [MunicipioId] nvarchar(450) NULL,
    [EstadoId] nvarchar(450) NULL,
    [CreatedAt] datetime2 NULL,
    [UpdatedAt] datetime2 NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_Agebs] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Agebs_Localidades] FOREIGN KEY ([LocalidadId]) REFERENCES [Localidades] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Agebs_Municipios] FOREIGN KEY ([MunicipioId]) REFERENCES [Municipios] ([Id]),
    CONSTRAINT [FK_Agebs_Estados] FOREIGN KEY ([EstadoId]) REFERENCES [Estados] ([Id])
);

GO

CREATE TABLE [ZonasImpulso] (
    [Id] nvarchar(450) NOT NULL,
    [Nombre] nvarchar(256) NULL,
    [MunicipioId] nvarchar(450) NULL,
    [CreatedAt] datetime2 NULL,
    [UpdatedAt] datetime2 NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_ZonasImpulso] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ZonasImpulso_Municipios] FOREIGN KEY ([MunicipioId]) REFERENCES [Municipios] ([Id]) ON DELETE CASCADE
);

GO

ALTER TABLE [Beneficiarios] ADD [LocalidadId] nvarchar(450) NULL;

GO

ALTER TABLE [Beneficiarios] ADD [MunicipioId] nvarchar(450) NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190705163230_Catalogos', N'2.1.11-servicing-32099');

GO

ALTER TABLE [Preguntas] ADD [Catalogo] nvarchar(max) NULL;

GO

CREATE TABLE [Parentescos] (
    [Id] nvarchar(450) NOT NULL,
    [Nombre] nvarchar(256) NULL,
    [CreatedAt] datetime2 NULL,
    [UpdatedAt] datetime2 NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_Parentescos] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [Sexos] (
    [Id] nvarchar(450) NOT NULL,
    [Nombre] nvarchar(256) NULL,
    [CreatedAt] datetime2 NULL,
    [UpdatedAt] datetime2 NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_Sexos] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [Estudios] (
    [Id] nvarchar(450) NOT NULL,
    [Nombre] nvarchar(256) NULL,
    [CreatedAt] datetime2 NULL,
    [UpdatedAt] datetime2 NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_Estudios] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [Discapacidades] (
    [Id] nvarchar(450) NOT NULL,
    [Nombre] nvarchar(256) NULL,
    [CreatedAt] datetime2 NULL,
    [UpdatedAt] datetime2 NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_Discapacidades] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [EstadosCiviles] (
    [Id] nvarchar(450) NOT NULL,
    [Nombre] nvarchar(256) NULL,
    [CreatedAt] datetime2 NULL,
    [UpdatedAt] datetime2 NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_EstadosCiviles] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [Ocupaciones] (
    [Id] nvarchar(450) NOT NULL,
    [Nombre] nvarchar(256) NULL,
    [CreatedAt] datetime2 NULL,
    [UpdatedAt] datetime2 NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_Ocupaciones] PRIMARY KEY ([Id])
);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190708231628_PreguntaCatalogo', N'2.1.11-servicing-32099');

GO

CREATE TABLE [Zonas] (
    [Id] nvarchar(450) NOT NULL,
    [Clave] nvarchar(max) NULL,
    [DependenciaId] nvarchar(450) NOT NULL,
    [CreatedAt] datetime2 NULL,
    [UpdatedAt] datetime2 NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_Zonas] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Zonas_Dependencias] FOREIGN KEY ([DependenciaId]) REFERENCES [Dependencias] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [MunicipiosZonas] (
    [Id] nvarchar(450) NOT NULL,
    [ZonaId] nvarchar(450) NOT NULL,
    [MunicipioId] nvarchar(450) NOT NULL,
    [CreatedAt] datetime2 NULL,
    [UpdatedAt] datetime2 NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_ZonasMunicipios] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ZonasMunicipios_Municipios] FOREIGN KEY ([MunicipioId]) REFERENCES [Municipios] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_ZonasMunicipios_Zonas] FOREIGN KEY ([ZonaId]) REFERENCES [Zonas] ([Id]) ON DELETE CASCADE
);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190709164547_Zonas', N'2.1.11-servicing-32099');

GO

ALTER TABLE [Vertientes] ADD [Vigencia] int NULL;

GO

ALTER TABLE [Vertientes] ADD [TipoEntrega] nvarchar(max) NULL;

GO

CREATE TABLE [VertientesArchivos] (
    [Id] nvarchar(450) NOT NULL,
    [TipoArchivo] nvarchar(max) NULL,
    [VertienteId] nvarchar(450) NOT NULL,
    [CreatedAt] datetime2 NULL,
    [UpdatedAt] datetime2 NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_VertientesArchivos] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_VertientesArchivos] FOREIGN KEY ([VertienteId]) REFERENCES [Vertientes] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [TipoAsentamientos] (
    [Id] nvarchar(450) NOT NULL,
    [Nombre] nvarchar(256) NULL,
    [CreatedAt] datetime2 NULL,
    [UpdatedAt] datetime2 NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_TipoAsentamientos] PRIMARY KEY ([Id])
);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190711182549_ConfVertientes', N'2.1.11-servicing-32099');

GO

EXEC sp_rename N'[Beneficiarios].[EstadoNacimiento]', N'EstadoId', N'COLUMN';

GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Beneficiarios]') AND [c].[name] = N'EstadoId');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Beneficiarios] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [Beneficiarios] ALTER COLUMN [EstadoId] nvarchar(450) NULL;

GO

ALTER TABLE [Beneficiarios] ADD CONSTRAINT [FK_Beneficiario_Estado] FOREIGN KEY ([EstadoId]) REFERENCES [Estados] ([Id]);

GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Beneficiarios]') AND [c].[name] = N'MunicipioId');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Beneficiarios] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [Beneficiarios] DROP COLUMN [MunicipioId];

GO

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Beneficiarios]') AND [c].[name] = N'LocalidadId');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [Beneficiarios] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [Beneficiarios] DROP COLUMN [LocalidadId];

GO

ALTER TABLE [Domicilio] ADD [MunicipioId] nvarchar(450) NULL;

GO

ALTER TABLE [Domicilio] ADD [LocalidadId] nvarchar(450) NULL;

GO

ALTER TABLE [Domicilio] ADD CONSTRAINT [FK_Domicilio_Municipio] FOREIGN KEY ([MunicipioId]) REFERENCES [Municipios] ([Id]);

GO

ALTER TABLE [Domicilio] ADD CONSTRAINT [FK_Domicilio_Localidad] FOREIGN KEY ([LocalidadId]) REFERENCES [Localidades] ([Id]);

GO

EXEC sp_rename N'[Domicilio].[ClaveAGEB]', N'AgebId', N'COLUMN';

GO

DECLARE @var3 sysname;
SELECT @var3 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Domicilio]') AND [c].[name] = N'AgebId');
IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [Domicilio] DROP CONSTRAINT [' + @var3 + '];');
ALTER TABLE [Domicilio] ALTER COLUMN [AgebId] nvarchar(450) NULL;

GO

ALTER TABLE [Domicilio] ADD CONSTRAINT [FK_Domicilio_Ageb] FOREIGN KEY ([AgebId]) REFERENCES [Agebs] ([Id]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190712170828_RenombrarEstado', N'2.1.11-servicing-32099');

GO

CREATE TABLE [Carencias] (
    [Id] nvarchar(450) NOT NULL,
    [Clave] nvarchar(256) NULL,
    [Nombre] nvarchar(256) NULL,
    [CreatedAt] datetime2 NULL,
    [UpdatedAt] datetime2 NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_Carencias] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [VertienteCarencias] (
    [Id] nvarchar(450) NOT NULL,
    [VertienteId] nvarchar(450) NULL,
    [CarenciaId] nvarchar(450) NULL,
    [CreatedAt] datetime2 NULL,
    [UpdatedAt] datetime2 NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_VertienteCarencias] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_VertienteCarencia_Carencias] FOREIGN KEY ([CarenciaId]) REFERENCES [Carencias] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_VertienteCarencia_Vertientes] FOREIGN KEY ([VertienteId]) REFERENCES [Vertientes] ([Id]) ON DELETE CASCADE
);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190717202858_Carencias', N'2.1.11-servicing-32099');

GO

ALTER TABLE [Aplicaciones] ADD [Carencias] nvarchar(450) NULL;

GO

ALTER TABLE [Beneficiarios] ADD [EstudioId] nvarchar(450) NULL;

GO

ALTER TABLE [Beneficiarios] ADD CONSTRAINT [FK_Beneficiario_Estudios] FOREIGN KEY ([EstudioId]) REFERENCES [Estudios] ([Id]);

GO

ALTER TABLE [Beneficiarios] ADD [EstadoCivilId] nvarchar(450) NULL;

GO

ALTER TABLE [Beneficiarios] ADD CONSTRAINT [FK_Beneficiario_EstadoCivil] FOREIGN KEY ([EstadoCivilId]) REFERENCES [EstadosCiviles] ([Id]);

GO

ALTER TABLE [Beneficiarios] ADD [DiscapacidadId] nvarchar(450) NULL;

GO

ALTER TABLE [Beneficiarios] ADD CONSTRAINT [FK_Beneficiario_Discapacidad] FOREIGN KEY ([DiscapacidadId]) REFERENCES [Estudios] ([Id]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190725175002_AddDatosBeneficiario', N'2.1.11-servicing-32099');

GO

CREATE TABLE [Importaciones] (
    [Clave] nvarchar(256) NOT NULL,
    [UsuarioId] nvarchar(450) NULL,
    [ImportedAt] datetime2 NULL,
    CONSTRAINT [PK_Importaciones] PRIMARY KEY ([Clave]),
    CONSTRAINT [FK_Importaciones_Usuarios] FOREIGN KEY ([UsuarioId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190725202703_Exportacion', N'2.1.11-servicing-32099');

GO

ALTER TABLE [AplicacionPreguntas] DROP CONSTRAINT [FK_Respuesta_AplicacionPreguntas];

GO

DECLARE @var4 sysname;
SELECT @var4 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Vertientes]') AND [c].[name] = N'Inmediato');
IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [Vertientes] DROP CONSTRAINT [' + @var4 + '];');
ALTER TABLE [Vertientes] DROP COLUMN [Inmediato];

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190726190321_DropColumns', N'2.1.11-servicing-32099');

GO

EXEC sp_rename N'[Localidades].[Indigena]', N'ZonaIndigenaId', N'COLUMN';

GO

ALTER TABLE [Domicilio] ADD [ZonaImpulsoId] nvarchar(max) NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190727030922_DomicilioZona', N'2.1.11-servicing-32099');

GO

ALTER TABLE [Beneficiarios] ADD [Estatus] bit NULL DEFAULT 0;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190727220959_AddBeneficiarioEstatus', N'2.1.11-servicing-32099');

GO

ALTER TABLE [TrabajadoresDependencias] DROP CONSTRAINT [FK_TrabajadoresDependencias_Trabajadores];

GO

EXEC sp_rename N'[TrabajadoresDependencias].[UsuarioId]', N'TrabajadorId', N'COLUMN';

GO

ALTER TABLE [TrabajadoresDependencias] ADD CONSTRAINT [FK_TrabajadoresDependencias_Trabajadores_TrabajadorId] FOREIGN KEY ([TrabajadorId]) REFERENCES [Trabajadores] ([Id]) ON DELETE NO ACTION;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190728144708_TrabajadorDependencia', N'2.1.11-servicing-32099');

GO

CREATE TABLE [PreguntaGrado] (
    [Id] nvarchar(450) NOT NULL,
    [PreguntaId] nvarchar(450) NULL,
    [Grado] nvarchar(250) NULL,
    [CreatedAt] datetime2 NULL,
    [UpdatedAt] datetime2 NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_PreguntaGrado] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_PreguntaGrado_Preguntas] FOREIGN KEY ([PreguntaId]) REFERENCES [Preguntas] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [RespuestaGrado] (
    [Id] nvarchar(450) NOT NULL,
    [RespuestaId] nvarchar(450) NULL,
    [PreguntaGradoId] nvarchar(450) NULL,
    [Grado] nvarchar(250) NULL,
    [CreatedAt] datetime2 NULL,
    [UpdatedAt] datetime2 NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_RespuestaGrado] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_RespuestaGrado_Respuesta] FOREIGN KEY ([RespuestaId]) REFERENCES [Respuestas] ([Id]),
    CONSTRAINT [FK_RespuestaGrado_PreguntaGrado] FOREIGN KEY ([PreguntaGradoId]) REFERENCES [PreguntaGrado] ([Id])
);

GO

ALTER TABLE [Preguntas] ADD [Gradual] bit NULL;

GO

ALTER TABLE [Respuestas] ADD [Negativa] bit NULL;

GO

ALTER TABLE [AplicacionPreguntas] ADD [Grado] nvarchar(20) NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190730204940_Grados', N'2.1.11-servicing-32099');

GO

DECLARE @var5 sysname;
SELECT @var5 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Beneficiarios]') AND [c].[name] = N'Sexo');
IF @var5 IS NOT NULL EXEC(N'ALTER TABLE [Beneficiarios] DROP CONSTRAINT [' + @var5 + '];');
ALTER TABLE [Beneficiarios] DROP COLUMN [Sexo];

GO

ALTER TABLE [Beneficiarios] ADD [SexoId] nvarchar(450) NULL;

GO

ALTER TABLE [Beneficiarios] ADD CONSTRAINT [FK_Beneficiario_Sexo] FOREIGN KEY ([SexoId]) REFERENCES [Sexos] ([Id]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190731182448_BeneficiarioSexoId', N'2.1.11-servicing-32099');

GO

ALTER TABLE [AspNetUsers] ADD [DependenciaId] nvarchar(450) NULL;

GO

ALTER TABLE [AspNetUsers] ADD CONSTRAINT [FK_AspNetUsers_Dependencias] FOREIGN KEY ([DependenciaId]) REFERENCES [Dependencias] ([Id]);

GO

ALTER TABLE [Preguntas] ADD [CarenciaId] nvarchar(450) NULL;

GO

ALTER TABLE [Preguntas] ADD CONSTRAINT [FK_Preguntas_Carencias] FOREIGN KEY ([CarenciaId]) REFERENCES [Carencias] ([Id]);

GO

ALTER TABLE [Carencias] ADD [Color] nvarchar(max) NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190806202147_CarenciaPregunta', N'2.1.11-servicing-32099');

GO

ALTER TABLE [Beneficiarios] ADD [TrabajadorId] nvarchar(450) NULL;

GO

ALTER TABLE [Beneficiarios] ADD CONSTRAINT [FK_Beneficiario_Trabajador] FOREIGN KEY ([TrabajadorId]) REFERENCES [Trabajadores] ([Id]);

GO

ALTER TABLE [Domicilio] ADD [TrabajadorId] nvarchar(450) NULL;

GO

ALTER TABLE [Domicilio] ADD CONSTRAINT [FK_Domicilio_Trabajador] FOREIGN KEY ([TrabajadorId]) REFERENCES [Trabajadores] ([Id]);

GO

ALTER TABLE [Aplicaciones] ADD [TrabajadorId] nvarchar(450) NULL;

GO

ALTER TABLE [Aplicaciones] ADD CONSTRAINT [FK_Aplicacion_Trabajador] FOREIGN KEY ([TrabajadorId]) REFERENCES [Trabajadores] ([Id]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190807150414_AddTrabajadorId', N'2.1.11-servicing-32099');

GO

CREATE TABLE [BeneficiarioHistorico] (
    [Id] nvarchar(450) NOT NULL,
    [ApellidoPaterno] nvarchar(256) NULL,
    [ApellidoMaterno] nvarchar(256) NULL,
    [Nombre] nvarchar(256) NULL,
    [FechaNacimiento] datetime2 NULL,
    [Curp] nvarchar(18) NULL,
    [Rfc] nvarchar(13) NULL,
    [Comentarios] nvarchar(450) NULL,
    [BeneficiarioId] nvarchar(450) NOT NULL,
    [EstudioId] nvarchar(450) NULL,
    [EstadoId] nvarchar(450) NULL,
    [EstadoCivilId] nvarchar(450) NULL,
    [DiscapacidadId] nvarchar(450) NULL,
    [SexoId] nvarchar(450) NULL,
    [TrabajadorId] nvarchar(450) NULL,
    [Estatus] bit NULL,
    [CreatedAt] datetime2 NULL,
    [UpdatedAt] datetime2 NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_BeneficiarioHistorico] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_HistorialBeneficiario_Beneficiario] FOREIGN KEY ([BeneficiarioId]) REFERENCES [Beneficiarios] ([Id]),
    CONSTRAINT [FK_HistorialBeneficiario_Estudio] FOREIGN KEY ([EstudioId]) REFERENCES [Estudios] ([Id]),
    CONSTRAINT [FK_HistorialBeneficiario_EstadoCivil] FOREIGN KEY ([EstadoCivilId]) REFERENCES [EstadosCiviles] ([Id]),
    CONSTRAINT [FK_HistorialBeneficiario_Discapacidad] FOREIGN KEY ([DiscapacidadId]) REFERENCES [Discapacidades] ([Id]),
    CONSTRAINT [FK_HistorialBeneficiario_Sexo] FOREIGN KEY ([SexoId]) REFERENCES [Sexos] ([Id]),
    CONSTRAINT [FK_HistorialBeneficiario_Trabajador] FOREIGN KEY ([TrabajadorId]) REFERENCES [Trabajadores] ([Id])
);

GO

ALTER TABLE [Domicilio] ADD [Activa] bit NULL DEFAULT 0;

GO

ALTER TABLE [Aplicaciones] ADD [Activa] bit NULL DEFAULT 0;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190810145659_AddHistorial', N'2.1.11-servicing-32099');

GO

ALTER TABLE [Preguntas] ADD [Expresion] nvarchar(450) NULL;

GO

ALTER TABLE [Preguntas] ADD [ExpresionEjemplo] nvarchar(450) NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190814133604_AddExpresionField', N'2.1.11-servicing-32099');

GO

ALTER TABLE [Estados] ADD [Abreviacion] nvarchar(2) NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190814151712_AddAbreviacionEstado', N'2.1.11-servicing-32099');

GO

ALTER TABLE [Beneficiarios] ADD [EstatusInformacion] nvarchar(450) NULL;

GO

ALTER TABLE [Beneficiarios] ADD [EstatusUpdatedAt] datetime2 NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190816140901_AddEstatusBeneficiarios', N'2.1.11-servicing-32099');

GO

ALTER TABLE [Domicilio] ADD [CadenaOCR] nvarchar(450) NULL;

GO

ALTER TABLE [Domicilio] ADD [Porcentaje] nvarchar(450) NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190820205306_AddInformacionOCR', N'2.1.11-servicing-32099');

GO

CREATE TABLE [TrabajadorRegion] (
    [Id] nvarchar(450) NOT NULL,
    [TrabajadorId] nvarchar(450) NULL,
    [ZonaId] nvarchar(450) NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NOT NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_TrabajadorRegion] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_TrabajadorRegion_Trabajadores_TrabajadorId] FOREIGN KEY ([TrabajadorId]) REFERENCES [Trabajadores] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_TrabajadorRegion_Zonas_ZonaId] FOREIGN KEY ([ZonaId]) REFERENCES [Zonas] ([Id]) ON DELETE NO ACTION
);

GO

CREATE TABLE [UsuarioRegion] (
    [Id] nvarchar(450) NOT NULL,
    [UsuarioId] nvarchar(450) NULL,
    [ZonaId] nvarchar(450) NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NOT NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_UsuarioRegion] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_UsuarioRegion_AspNetUsers_UsuarioId] FOREIGN KEY ([UsuarioId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_UsuarioRegion_Zonas_ZonaId] FOREIGN KEY ([ZonaId]) REFERENCES [Zonas] ([Id]) ON DELETE NO ACTION
);

GO

CREATE INDEX [IX_TrabajadorRegion_TrabajadorId] ON [TrabajadorRegion] ([TrabajadorId]);

GO

CREATE INDEX [IX_TrabajadorRegion_ZonaId] ON [TrabajadorRegion] ([ZonaId]);

GO

CREATE INDEX [IX_UsuarioRegion_UsuarioId] ON [UsuarioRegion] ([UsuarioId]);

GO

CREATE INDEX [IX_UsuarioRegion_ZonaId] ON [UsuarioRegion] ([ZonaId]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190826213727_Regiones', N'2.1.11-servicing-32099');

GO

ALTER TABLE [Aplicaciones] ADD [NumeroAplicacion] int NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190829221354_AgregarNumeroAplicacion', N'2.1.11-servicing-32099');

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name', N'NormalizedName') AND [object_id] = OBJECT_ID(N'[AspNetRoles]'))
    SET IDENTITY_INSERT [AspNetRoles] ON;
INSERT INTO [AspNetRoles] ([Id], [Name], [NormalizedName])
VALUES (N'1', N'Administrador', N'Administrador');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name', N'NormalizedName') AND [object_id] = OBJECT_ID(N'[AspNetRoles]'))
    SET IDENTITY_INSERT [AspNetRoles] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name', N'NormalizedName') AND [object_id] = OBJECT_ID(N'[AspNetRoles]'))
    SET IDENTITY_INSERT [AspNetRoles] ON;
INSERT INTO [AspNetRoles] ([Id], [Name], [NormalizedName])
VALUES (N'2', N'Administrador de dependencia', N'Dependencia');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name', N'NormalizedName') AND [object_id] = OBJECT_ID(N'[AspNetRoles]'))
    SET IDENTITY_INSERT [AspNetRoles] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name', N'NormalizedName') AND [object_id] = OBJECT_ID(N'[AspNetRoles]'))
    SET IDENTITY_INSERT [AspNetRoles] ON;
INSERT INTO [AspNetRoles] ([Id], [Name], [NormalizedName])
VALUES (N'3', N'Analista de dependencia', N'Analista');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name', N'NormalizedName') AND [object_id] = OBJECT_ID(N'[AspNetRoles]'))
    SET IDENTITY_INSERT [AspNetRoles] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Dependencias]'))
    SET IDENTITY_INSERT [Dependencias] ON;
INSERT INTO [Dependencias] ([Id], [Nombre], [CreatedAt], [UpdatedAt])
VALUES (N'e587835d-66ec-4cd2-813c-ce9ac91affcc', N'Secretaría de Desarrollo Social y Humano', '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Dependencias]'))
    SET IDENTITY_INSERT [Dependencias] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name', N'UserName', N'NormalizedUserName', N'Email', N'NormalizedEmail', N'EmailConfirmed', N'PasswordHash', N'SecurityStamp', N'AccessFailedCount', N'PhoneNumberConfirmed', N'TwoFactorEnabled', N'LockoutEnabled') AND [object_id] = OBJECT_ID(N'[AspNetUsers]'))
    SET IDENTITY_INSERT [AspNetUsers] ON;
INSERT INTO [AspNetUsers] ([Id], [Name], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [AccessFailedCount], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnabled])
VALUES (N'b7d2de8e-8448-4d84-9e5c-3c20bfff554a', N'Administrador', N'admin', N'admin', N'cnunez@guanajuato.gob.mx', N'cnunez@guanajuato.gob.mx', 1, N'AQAAAAEAACcQAAAAENn8JelFkFNl7BMD9NmGDSBWUuKgppgFDanwV5LZwF3BlO7cUTk2FI1AxNy2KZoc9w==', N'', 0, 0, 0, 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name', N'UserName', N'NormalizedUserName', N'Email', N'NormalizedEmail', N'EmailConfirmed', N'PasswordHash', N'SecurityStamp', N'AccessFailedCount', N'PhoneNumberConfirmed', N'TwoFactorEnabled', N'LockoutEnabled') AND [object_id] = OBJECT_ID(N'[AspNetUsers]'))
    SET IDENTITY_INSERT [AspNetUsers] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'UserId', N'RoleId') AND [object_id] = OBJECT_ID(N'[AspNetUserRoles]'))
    SET IDENTITY_INSERT [AspNetUserRoles] ON;
INSERT INTO [AspNetUserRoles] ([UserId], [RoleId])
VALUES (N'b7d2de8e-8448-4d84-9e5c-3c20bfff554a', N'1');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'UserId', N'RoleId') AND [object_id] = OBJECT_ID(N'[AspNetUserRoles]'))
    SET IDENTITY_INSERT [AspNetUserRoles] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Clave', N'Nombre', N'CreatedAt', N'UpdatedAt', N'Color') AND [object_id] = OBJECT_ID(N'[Carencias]'))
    SET IDENTITY_INSERT [Carencias] ON;
INSERT INTO [Carencias] ([Id], [Clave], [Nombre], [CreatedAt], [UpdatedAt], [Color])
VALUES (N'1', N'educativa', N'Rezago educativo', '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', N'#fff3f3');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Clave', N'Nombre', N'CreatedAt', N'UpdatedAt', N'Color') AND [object_id] = OBJECT_ID(N'[Carencias]'))
    SET IDENTITY_INSERT [Carencias] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Clave', N'Nombre', N'CreatedAt', N'UpdatedAt', N'Color') AND [object_id] = OBJECT_ID(N'[Carencias]'))
    SET IDENTITY_INSERT [Carencias] ON;
INSERT INTO [Carencias] ([Id], [Clave], [Nombre], [CreatedAt], [UpdatedAt], [Color])
VALUES (N'2', N'salud', N'Acceso a los servicios de salud', '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', N'#fffef3');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Clave', N'Nombre', N'CreatedAt', N'UpdatedAt', N'Color') AND [object_id] = OBJECT_ID(N'[Carencias]'))
    SET IDENTITY_INSERT [Carencias] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Clave', N'Nombre', N'CreatedAt', N'UpdatedAt', N'Color') AND [object_id] = OBJECT_ID(N'[Carencias]'))
    SET IDENTITY_INSERT [Carencias] ON;
INSERT INTO [Carencias] ([Id], [Clave], [Nombre], [CreatedAt], [UpdatedAt], [Color])
VALUES (N'3', N'seguridad', N'Acceso a la seguridad social', '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', N'#f6fff3');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Clave', N'Nombre', N'CreatedAt', N'UpdatedAt', N'Color') AND [object_id] = OBJECT_ID(N'[Carencias]'))
    SET IDENTITY_INSERT [Carencias] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Clave', N'Nombre', N'CreatedAt', N'UpdatedAt', N'Color') AND [object_id] = OBJECT_ID(N'[Carencias]'))
    SET IDENTITY_INSERT [Carencias] ON;
INSERT INTO [Carencias] ([Id], [Clave], [Nombre], [CreatedAt], [UpdatedAt], [Color])
VALUES (N'4', N'vivienda', N'Calidad y espacios de la vivienda', '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', N'#f3fffe');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Clave', N'Nombre', N'CreatedAt', N'UpdatedAt', N'Color') AND [object_id] = OBJECT_ID(N'[Carencias]'))
    SET IDENTITY_INSERT [Carencias] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Clave', N'Nombre', N'CreatedAt', N'UpdatedAt', N'Color') AND [object_id] = OBJECT_ID(N'[Carencias]'))
    SET IDENTITY_INSERT [Carencias] ON;
INSERT INTO [Carencias] ([Id], [Clave], [Nombre], [CreatedAt], [UpdatedAt], [Color])
VALUES (N'5', N'infraestructura', N'Acceso a los servicios básicos en la vivienda', '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', N'#f3f7ff');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Clave', N'Nombre', N'CreatedAt', N'UpdatedAt', N'Color') AND [object_id] = OBJECT_ID(N'[Carencias]'))
    SET IDENTITY_INSERT [Carencias] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Clave', N'Nombre', N'CreatedAt', N'UpdatedAt', N'Color') AND [object_id] = OBJECT_ID(N'[Carencias]'))
    SET IDENTITY_INSERT [Carencias] ON;
INSERT INTO [Carencias] ([Id], [Clave], [Nombre], [CreatedAt], [UpdatedAt], [Color])
VALUES (N'6', N'alimentacion', N'Acceso a la alimentación', '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', N'#f9f3ff');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Clave', N'Nombre', N'CreatedAt', N'UpdatedAt', N'Color') AND [object_id] = OBJECT_ID(N'[Carencias]'))
    SET IDENTITY_INSERT [Carencias] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'Vigencia', N'Activo', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Encuestas]'))
    SET IDENTITY_INSERT [Encuestas] ON;
INSERT INTO [Encuestas] ([Id], [Nombre], [Vigencia], [Activo], [CreatedAt], [UpdatedAt])
VALUES (N'1', N'CONEVAL', 24, 1, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'Vigencia', N'Activo', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Encuestas]'))
    SET IDENTITY_INSERT [Encuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Activa', N'EncuestaId', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[EncuestaVersiones]'))
    SET IDENTITY_INSERT [EncuestaVersiones] ON;
INSERT INTO [EncuestaVersiones] ([Id], [Activa], [EncuestaId], [CreatedAt], [UpdatedAt])
VALUES (N'1', 1, N'1', '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Activa', N'EncuestaId', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[EncuestaVersiones]'))
    SET IDENTITY_INSERT [EncuestaVersiones] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'969', N'Municipio vivienda', N'Listado', NULL, 0, NULL, 0, N'1', 1, 0, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', N'Municipios', 0, NULL, NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'975', N'Tipo de asentamiento', N'Listado', NULL, 0, NULL, 0, N'1', 2, 0, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', N'TipoAsentamientos', 0, NULL, NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'984', N'¿Cuántas personas viven normalmente en esta vivienda contando a los niños chiquitos y a los ancianos?', N'Numerica', NULL, 0, NULL, 0, N'1', 3, 0, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', NULL, 0, NULL, NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'987', N'¿Cuántas personas forman parte de este hogar?', N'Numerica', NULL, 0, NULL, 0, N'1', 4, 0, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', NULL, 0, NULL, NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'993', N'Parentesco con el jefe de familia', N'Listado', NULL, 0, N'?987', 1, N'1', 5, 0, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', N'Parentescos', 0, NULL, NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'24', N'¿Actualmente...', N'Listado', NULL, 0, N'?987', 1, N'1', 6, 0, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', NULL, 0, NULL, NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'998', N'Sexo habitante', N'Listado', NULL, 0, N'?987', 1, N'1', 7, 0, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', N'Sexos', 0, NULL, NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'208', N'CURP', N'Abierta', NULL, 0, N'?987', 1, N'1', 8, 1, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', NULL, 0, NULL, N'[A-Z]{1}[AEIOU]{1}[A-Z]{2}[0-9]{2}(0[1-9]|1[0-2])(0[1-9]|1[0-9]|2[0-9]|3[0-1])[HM]{1}(AS|BC|BS|CC|CS|CH|CL|CM|DF|DG|GT|GR|HG|JC|MC|MN|MS|NT|NL|OC|PL|QT|QR|SP|SL|SR|TC|TS|TL|VZ|YN|ZS|NE)[B-DF-HJ-NP-TV-Z]{3}[0-9A-Z]{1}[0-9]{1}$', N'LARS881101HGTZZR05');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'996', N'Fecha de nacimiento', N'Fecha', NULL, 0, N'?987', 1, N'1', 9, 0, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', NULL, 0, N'1', NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'997', N'¿Cuántos años cumplidos tiene?', N'Numerica', NULL, 0, N'?987', 1, N'1', 10, 0, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', NULL, 0, N'1', NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'27', N'¿sabe leer y escribir un recado?', N'Listado', N'?997,>6', 0, N'?987', 1, N'1', 11, 0, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', NULL, 0, N'1', NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'130', N'¿Cuál es el último nivel que aprobó en la escuela?', N'Listado', N'?997,>2', 0, N'?987', 1, N'1', 12, 0, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', NULL, 0, N'1', NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'131', N'¿Cuál es el último grado que aprobó en la escuela? (último año  aprobado del nivel referido en la pregunta antecedente)', N'Listado', N'?997,>6', 0, N'?987', 1, N'1', 13, 0, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', NULL, 0, N'1', NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'29', N'¿Asiste actualmente a la escuela, estancia infantil, CENDI, CADI o guardería?', N'Listado', NULL, 0, N'?987', 1, N'1', 14, 0, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', NULL, 0, N'1', NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'37', N'Tiene derecho a los servicios médicos:', N'Listado', NULL, 0, N'?987', 1, N'1', 15, 0, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', NULL, 0, N'2', NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'82', N'¿Está afiliado o inscrito a la institución por?', N'Listado', N'?37,!=102', 0, N'?987', 1, N'1', 16, 0, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', NULL, 0, N'2', NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'38', N'Cuando tiene problemas de salud, ¿En dónde se atiende?', N'Listado', NULL, 0, N'?987', 1, N'1', 17, 0, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', NULL, 0, N'2', NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'42', N'En su vida diaria, ¿tiene dificultad al realizar las siguientes actividades:', N'Listado', NULL, 0, N'?987', 1, N'1', 18, 0, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', NULL, 1, N'2', NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'33', N'¿El mes pasado…', N'Listado', N'?997,>16', 0, N'?987', 1, N'1', 19, 0, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', NULL, 0, N'3', NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'91', N'En su trabajo principal del mes pasado ¿tuvo un jefe(a) o supervisor?', N'Listado', N'?33,77', 0, N'?987', 1, N'1', 20, 0, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', NULL, 0, N'3', NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'92', N'Entonces en el trabajo principal del mes pasado de ¿se dedicó a un negocio o actividad por su cuenta?', N'Listado', N'?91,!=325', 0, N'?987', 1, N'1', 21, 0, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', NULL, 0, N'3', NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'88', N'En su trabajo principal del mes pasado ¿se desempeñó como…?', N'Listado', N'?33,77', 0, N'?987', 1, N'1', 22, 0, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', N'Ocupaciones', 0, N'3', NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'93', N'En su trabajo principal del mes pasado ¿recibió un pago?', N'Listado', N'?33,77', 0, N'?987', 1, N'1', 23, 0, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', NULL, 0, N'3', NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'34', N'¿En este trabajo le dieron las siguientes prestaciones, aunque no las haya utilizado?', N'Check', N'?33,77/78/79/80', 0, N'?987', 1, N'1', 24, 0, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', NULL, 0, N'3', NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'96', N'¿Tiene contratado voluntariamente…', N'Check', NULL, 0, N'?987', 1, N'1', 25, 0, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', NULL, 0, N'3', NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'99', N'¿Recibe dinero por ...', N'Listado', N'?997,>64', 0, N'?987', 1, N'1', 26, 0, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', NULL, 0, N'3', NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'100', N'¿Realiza regularmente las siguientes actividades? (trabajo secundario)', N'Listado', N'?997,>16', 0, N'?987', 1, N'1', 27, 0, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', NULL, 0, N'3', NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'79', N'¿Alguien en el hogar recibe dinero proveniente de otros países?', N'Listado', NULL, 0, NULL, 0, N'1', 28, 0, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', NULL, 0, N'3', NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'44', N'¿De qué material es la mayor parte del piso de esta vivienda?', N'Listado', NULL, 0, NULL, 0, N'1', 29, 0, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', NULL, 0, N'4', NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'46', N'¿De qué material es la mayor parte de las paredes o muros de esta vivienda?', N'Listado', NULL, 0, NULL, 0, N'1', 30, 0, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', NULL, 0, N'4', NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'48', N'¿De qué material es la mayor parte del techo de esta vivienda?', N'Listado', NULL, 0, NULL, 0, N'1', 31, 0, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', NULL, 0, N'4', NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'141', N'¿Cúantos cuartos tiene en total esta vivienda contando la cocina? (no cuente pasillos ni baños)', N'Numerica', NULL, 0, NULL, 0, N'1', 32, 0, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', NULL, 0, N'4', NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'51', N'¿Cuántos cuartos se usan para dormir sin contar pasillos y cocina? ', N'Numerica', NULL, 0, NULL, 0, N'1', 33, 0, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', NULL, 0, N'4', NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'113', N'¿Qué tipo de baño o escusado tiene su vivienda?', N'Listado', NULL, 0, NULL, 0, N'1', 34, 0, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', NULL, 0, N'4', NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'54', N'¿En esta vivienda tienen...', N'Listado', NULL, 0, NULL, 0, N'1', 35, 0, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', NULL, 0, N'4', NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'56', N'¿Esta vivienda tiene drenaje o desagüe conectado a...', N'Listado', NULL, 0, NULL, 0, N'1', 36, 0, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', NULL, 0, N'5', NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'57', N'¿El combustible que más usan para cocinar es...', N'Listado', NULL, 0, NULL, 0, N'1', 37, 0, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', NULL, 0, N'5', NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'58', N'¿La estufa (fogón) de leña o carbón con la que cocinan tiene chimenea?', N'Listado', N'?57,177/178', 0, NULL, 0, N'1', 38, 0, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', NULL, 0, N'5', NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'117', N'¿En esta vivienda tienen y funciona…', N'Check', NULL, 0, NULL, 0, N'1', 39, 0, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', NULL, 0, N'5', NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'118', N'En su vivienda ¿La luz eléctrica la obtienen…', N'Listado', NULL, 0, NULL, 0, N'1', 40, 0, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', NULL, 0, N'5', NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'119', N'¿La vivienda que habita es…', N'Listado', NULL, 0, NULL, 0, N'1', 41, 0, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', NULL, 0, N'5', NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'63', N'En los últimos tres meses, por falta de dinero o recursos… ¿Alguna vez usted o algún adulto en su hogar tuvo una alimentación basada en muy poca variedad de alimentos?', N'Listado', NULL, 0, NULL, 0, N'1', 42, 0, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', NULL, 0, N'6', NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'64', N'En los últimos tres meses, por falta de dinero o recursos… ¿Alguna vez usted o algún adulto en su hogar dejó de desayunar, comer o cenar?', N'Listado', NULL, 0, NULL, 0, N'1', 43, 0, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', NULL, 0, N'6', NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'65', N'En los últimos tres meses, por falta de dinero o recursos… ¿Alguna vez usted o algún adulto en su hogar comió menos de lo que usted piensa debía comer?', N'Listado', NULL, 0, NULL, 0, N'1', 44, 0, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', NULL, 0, N'6', NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'66', N'En los últimos tres meses, por falta de dinero o recursos… ¿Alguna vez usted o algún adulto en su hogar se quedaron sin comida?', N'Listado', NULL, 0, NULL, 0, N'1', 45, 0, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', NULL, 0, N'6', NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'67', N'En los últimos tres meses, por falta de dinero o recursos… ¿Alguna vez usted o algún adulto de este hogar sintió hambre pero no comió?', N'Listado', NULL, 0, NULL, 0, N'1', 46, 0, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', NULL, 0, N'6', NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'68', N'En los últimos tres meses, por falta de dinero o recursos… ¿Alguna vez usted o algún adulto en su hogar solo comió una vez al día o dejo de comer todo un día?', N'Listado', NULL, 0, NULL, 0, N'1', 47, 0, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', NULL, 0, N'6', NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'69', N'En los últimos tres meses, por falta de dinero o recursos… ¿Alguna vez algún menor de 18 años en su hogar tuvo una alimentación basada en muy poca variedad de alimentos?', N'Listado', N'?997,<18', 1, NULL, 0, N'1', 48, 0, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', NULL, 0, N'6', NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'70', N'En los últimos tres meses, por falta de dinero o recursos… ¿Alguna vez algún menor de 18 años en su hogar comió menos de lo que debía?', N'Listado', N'?997,<18', 1, NULL, 0, N'1', 49, 0, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', NULL, 0, N'6', NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'71', N'En los últimos tres meses, por falta de dinero o recursos, ¿alguna vez tuvieron que disminuir la cantidad servida en las comidas a algún menor de 18 años del hogar?', N'Listado', N'?997,<18', 1, NULL, 0, N'1', 50, 0, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', NULL, 0, N'6', NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'72', N'En los últimos tres meses, por falta de dinero o recursos, ¿alguna vez algún menor de 18 años en su hogar sintió hambre pero no comió?', N'Listado', N'?997,<18', 1, NULL, 0, N'1', 51, 0, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', NULL, 0, N'6', NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'73', N'En los últimos tres meses, por falta de dinero o recursos, ¿alguna vez algún menor de 18 años en su hogar se acostó con hambre?', N'Listado', N'?997,<18', 1, NULL, 0, N'1', 52, 0, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', NULL, 0, N'6', NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] ([Id], [Nombre], [TipoPregunta], [Condicion], [CondicionLista], [CondicionIterable], [Iterable], [EncuestaVersionId], [Numero], [Editable], [CreatedAt], [UpdatedAt], [Catalogo], [Gradual], [CarenciaId], [Expresion], [ExpresionEjemplo])
VALUES (N'74', N'En los últimos tres meses, por falta de dinero o recursos, ¿alguna vez algún menor de 18 años en su hogar comió una vez al día o dejó de comer todo un día?', N'Listado', N'?997,<18', 1, NULL, 0, N'1', 53, 0, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', NULL, 0, N'6', NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'TipoPregunta', N'Condicion', N'CondicionLista', N'CondicionIterable', N'Iterable', N'EncuestaVersionId', N'Numero', N'Editable', N'CreatedAt', N'UpdatedAt', N'Catalogo', N'Gradual', N'CarenciaId', N'Expresion', N'ExpresionEjemplo') AND [object_id] = OBJECT_ID(N'[Preguntas]'))
    SET IDENTITY_INSERT [Preguntas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'358', N'Cuidar sin pago y de manera exclusiva a niños, enfermos, adultos mayores o discapacitados', N'100', 1, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'359', N'Trabajo comunitario o voluntario', N'100', 2, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'360', N'Reparaciones a la vivienda, aparatos domésticos o vehículos.', N'100', 3, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'361', N'Realizar el quehacer de su hogar', N'100', 4, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'362', N'Acarrear agua o leña', N'100', 5, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'623', N'Ninguno', N'100', 6, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'404', N'Con conexión de agua/tiene descarga directa de agua', N'113', 1, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'405', N'Le echan agua con cubeta', N'113', 2, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'406', N'Sin admisión de agua (letrina seca o húmeda)', N'113', 3, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'407', N'Pozo u hoyo negro', N'113', 4, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'408', N'No tiene', N'113', 5, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'423', N'Refrigerador', N'117', 1, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'424', N'Lavadora automática', N'117', 2, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'425', N'VHS, DVD, BLU-RAY', N'117', 3, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'426', N'Vehículo (carro, camioneta o camión)', N'117', 4, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'427', N'Teléfono (fijo)', N'117', 5, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'428', N'Horno (microondas o eléctrico)', N'117', 6, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'429', N'Computadora', N'117', 7, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'430', N'Estufa / parrilla de gas', N'117', 8, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'431', N'Calentador de agua/ boiler de gas', N'117', 9, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'432', N'Calentador de agua/ solar', N'117', 10, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'433', N'Calentador de agua/ boiler electrico', N'117', 11, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'434', N'Internet', N'117', 12, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'435', N'Teléfono celular', N'117', 13, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'436', N'Aparato de televisión', N'117', 14, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'437', N'Aparato de televisión digital', N'117', 15, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'438', N'Servicio de televisión de paga (antena parabólica, SKY o TV por cable)', N'117', 16, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'440', N'Tinaco', N'117', 17, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'441', N'Aparato para regular la temperatura (ventilador, enfriador, clima, calefactor)', N'117', 18, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'627', N'Ninguno', N'117', 19, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'618', N'Regadera', N'117', 20, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'443', N'Del servicio público', N'118', 1, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'444', N'De una planta particular', N'118', 2, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'445', N'De panel solar', N'118', 3, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'446', N'De otra fuente', N'118', 4, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'447', N'No tienen luz eléctrica', N'118', 5, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'448', N'Propia y totalmente pagada', N'119', 1, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'449', N'Propia y la está pagando', N'119', 2, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'450', N'Propia y está hipotecada', N'119', 3, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'451', N'Rentada o alquilada', N'119', 4, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'452', N'Prestada o la está cuidando', N'119', 5, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'453', N'Intestada o está en litigio', N'119', 6, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'506', N'Kínder o preescolar', N'130', 1, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'507', N'Primaria', N'130', 2, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'508', N'Secundaria', N'130', 3, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'509', N'Preparatoria o Bachillerato', N'130', 4, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'510', N'Normal básica', N'130', 5, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'511', N'Carrera técnica o comercial con primaria completa', N'130', 6, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'512', N'Carrera técnica o comercial con secundaria completa', N'130', 7, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'513', N'Carrera técnica o comercial con preparatoria completa', N'130', 8, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'514', N'Profesional', N'130', 9, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'515', N'Posgrado (maestría o doctorado)', N'130', 10, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'566', N'Ninguno', N'130', 11, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'567', N'1 años', N'131', 1, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'568', N'2 años', N'131', 2, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'569', N'3 años', N'131', 3, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'570', N'4 años', N'131', 4, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'271', N'5 años', N'131', 5, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'572', N'6 años', N'131', 6, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'3', N'Vive en unión libre?', N'24', 1, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'4', N'Es casada(o)?', N'24', 2, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'5', N'Es separada(o)?', N'24', 3, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'6', N'Es divorciada(o)?', N'24', 4, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'7', N'Está viuda(o)?', N'24', 5, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'8', N'Es soltera(o)?', N'24', 6, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'35', N'No', N'27', 1, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'34', N'Si', N'27', 2, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'573', N'No sabe/No contesta', N'27', 3, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'49', N'Si', N'29', 1, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'50', N'No', N'29', 2, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'77', N'Trabajó (por lo menos una hora)', N'33', 1, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'78', N'Tenía trabajo, pero no trabajó', N'33', 2, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'79', N'Buscó trabajo', N'33', 3, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'80', N'Es pensionada(o) o jubilada(o)', N'33', 4, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'81', N'Es estudiante', N'33', 5, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'82', N'Se dedica a los quehaceres de su hogar', N'33', 6, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'83', N'Tiene alguna limitación física o mental permanente que le impide trabajar', N'33', 7, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'84', N'Estaba en otra situación diferente a las anteriores', N'33', 8, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'85', N'Incapacidad por enfermedad, accidente o maternidad', N'34', 1, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'86', N'SAR o AFORE', N'34', 2, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'87', N'Crédito para vivienda', N'34', 3, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'88', N'Guardería', N'34', 4, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'89', N'Aguinaldo', N'34', 5, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'90', N'Seguro de vida', N'34', 6, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'91', N'No tiene derecho a ninguna de estas prestaciones', N'34', 7, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 1);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'92', N'No sabe / no responde', N'34', 8, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 1);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'95', N'Del Seguro Social IMSS', N'37', 1, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'96', N'Del ISSSTE', N'37', 2, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'97', N'Del ISSSTE estatal', N'37', 3, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'98', N'PEMEX, Defensa o Marina', N'37', 4, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'99', N'Del Seguro Popular para una nueva generación', N'37', 5, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'100', N'De un seguro privado', N'37', 6, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'101', N'En otra institucións', N'37', 7, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'102', N'Entonces ¿no tiene derecho a servicios médicos?', N'37', 8, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'103', N'Seguro Social IMSS', N'38', 1, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'104', N'ISSSTE', N'38', 2, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'105', N'ISSSTE estatal', N'38', 3, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'106', N'PEMEX, Defensa o Marina', N'38', 4, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'107', N'Centro de Salud u Hospital de la SSA (Seguro Popular)', N'38', 5, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'108', N'IMSS-PROSPERA', N'38', 6, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'1082', N'Consultorio de una farmacia', N'38', 7, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'1083', N'Se automedica', N'38', 8, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'109', N'Consultorio, clinica u hospital privado', N'38', 9, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'110', N'Otro lugar', N'38', 10, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'111', N'No se atiende', N'38', 11, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'112', N'En otra institución', N'38', 12, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'113', N'Entonces ¿no tiene derecho a servicios médicos?', N'38', 13, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'117', N'Caminar, moverse, subir o bajar', N'42', 1, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'118', N'Ver, aun usando lentes', N'42', 2, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'119', N'Hablar, comunicarse o conversar', N'42', 3, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'120', N'Oír, aun usando aparato auditivo', N'42', 4, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'121', N'Vestirse, bañarse o comer', N'42', 5, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'122', N'Poner atención o aprender cosas sencillas', N'42', 6, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'123', N'Tiene alguna limitación mental', N'42', 7, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'124', N'Entonces, ¿No tiene dificultad física o mental?', N'42', 8, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 1);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'130', N'Tierra', N'44', 1, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'131', N'Cemento o firme', N'44', 2, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'132', N'Madera, mosaico u otro recubrimiento', N'44', 3, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'135', N'Material de desecho', N'46', 1, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'136', N'Lámina de cartón', N'46', 2, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'137', N'Lámina de asbesto o metálica', N'46', 3, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'138', N'Carrizo, bambú o palma', N'46', 4, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'139', N'Embarro o bajareque', N'46', 5, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'140', N'Madera', N'46', 6, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'141', N'Adobe', N'46', 7, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'142', N'Tabique, ladrillo, block, piedra, cantera, cemento o concreto', N'46', 8, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'145', N'Material de desecho', N'48', 1, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'146', N'Lámina de cartón', N'48', 2, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'147', N'Lámina metálica', N'48', 3, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'148', N'Lámina de asbesto', N'48', 4, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'149', N'Palma o paja', N'48', 5, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'150', N'Madera o tejamanil', N'48', 6, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'151', N'Terrado con viguería', N'48', 7, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'152', N'Teja', N'48', 8, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'153', N'Losa de concreto o viguetas con bovedilla', N'48', 9, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'162', N'Agua entubada dentro de la vivienda', N'54', 1, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'163', N'Agua entubada fuera de la vivienda,pero dentro del terreno', N'54', 2, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'164', N'Agua entubada de llave pública (o hidrante)', N'54', 3, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'165', N'Agua entubada que acarrean de otra vivienda', N'54', 4, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'166', N'Agua de pipa', N'54', 5, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'167', N'Agua de un pozo, río, lago, arroyo u otra', N'54', 6, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'643', N'Agua captada de lluvia u otro medio', N'54', 7, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'170', N'La red pública', N'56', 1, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'171', N'Una fosa séptica', N'56', 2, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'172', N'Una tubería que va a dar a una barranca o grieta', N'56', 3, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'173', N'Una tubería que va a dar a un río, lago o mar', N'56', 4, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'174', N'No tiene drenaje', N'56', 5, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'620', N'Biodigestor', N'56', 6, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'175', N'Gas de cilindro o tanque (estacionario)', N'57', 1, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'176', N'Gas natural o de tubería', N'57', 2, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'177', N'Leña', N'57', 3, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'178', N'Carbón', N'57', 4, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'179', N'Electricidad', N'57', 5, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'180', N'Otro combustible', N'57', 6, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'181', N'Si', N'58', 1, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'182', N'No', N'58', 2, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'206', N'Si', N'63', 1, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'207', N'No', N'63', 2, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'208', N'Si', N'64', 1, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'209', N'No', N'64', 2, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'210', N'Si', N'65', 1, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'211', N'No', N'65', 2, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'212', N'Si', N'66', 1, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'213', N'No', N'66', 2, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'214', N'Si', N'67', 1, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'215', N'No', N'67', 2, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'216', N'Si', N'68', 1, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'217', N'No', N'68', 2, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'218', N'Si', N'69', 1, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'219', N'No', N'69', 2, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'220', N'Si', N'70', 1, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'221', N'No', N'70', 2, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'222', N'Si', N'71', 1, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'223', N'No', N'71', 2, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'224', N'Si', N'72', 1, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'225', N'No', N'72', 2, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'226', N'Si', N'73', 1, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'227', N'No', N'73', 2, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'228', N'Si', N'74', 1, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'229', N'No', N'74', 2, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'247', N'Si', N'79', 1, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'248', N'No', N'79', 2, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'249', N'No sabe', N'79', 3, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'250', N'No contesta', N'79', 4, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'256', N'Prestación en el trabajo', N'82', 1, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'257', N'Jubilación', N'82', 2, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'258', N'Invalidez', N'82', 3, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'259', N'Algún familiar en el hogar', N'82', 4, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'260', N'Muerte del asegurado', N'82', 5, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'261', N'Ser estudiante', N'82', 6, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'262', N'Contratación propia', N'82', 7, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'263', N'Contratación propia', N'82', 8, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'264', N'Apoyo del gobierno', N'82', 9, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'265', N'No sabe/No responde', N'82', 10, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'325', N'Si', N'91', 1, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'326', N'No', N'91', 1, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'327', N'No sabe/No responde', N'91', 1, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'328', N'Si', N'91', 1, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'329', N'No', N'91', 2, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'330', N'No sabe/No responde', N'91', 3, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'331', N'Si', N'93', 1, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'332', N'No', N'93', 2, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'333', N'No sabe/No responde', N'93', 3, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'341', N'SAR, AFORE o fondo de retiro', N'96', 1, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'342', N'Seguro privado de gastos médicos', N'96', 2, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'343', N'Seguro de vida', N'96', 3, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'344', N'Seguro de invalidez', N'96', 4, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'345', N'Otro tipo de seguro', N'96', 5, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'346', N'Ninguno de los anteriores', N'96', 6, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 1);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'347', N'No sabe/No responde', N'96', 7, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 1);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'353', N'Programa Pensión del Programa para Adultos Mayores', N'99', 1, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'354', N'Componente de apoyo del Programa para Adultos Mayores  (PROSPERA)', N'99', 2, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'355', N'Otros Programas para Adultos Mayores (Estatal o Municipal)', N'99', 3, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'356', N'Ninguno', N'99', 4, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] ON;
INSERT INTO [Respuestas] ([Id], [Nombre], [PreguntaId], [Numero], [CreatedAt], [UpdatedAt], [Negativa])
VALUES (N'357', N'No sabe/No responde', N'99', 5, '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'PreguntaId', N'Numero', N'CreatedAt', N'UpdatedAt', N'Negativa') AND [object_id] = OBJECT_ID(N'[Respuestas]'))
    SET IDENTITY_INSERT [Respuestas] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'PreguntaId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[PreguntaGrado]'))
    SET IDENTITY_INSERT [PreguntaGrado] ON;
INSERT INTO [PreguntaGrado] ([Id], [PreguntaId], [Grado], [CreatedAt], [UpdatedAt])
VALUES (N'1', N'42', N'No puede hacerlo', '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'PreguntaId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[PreguntaGrado]'))
    SET IDENTITY_INSERT [PreguntaGrado] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'PreguntaId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[PreguntaGrado]'))
    SET IDENTITY_INSERT [PreguntaGrado] ON;
INSERT INTO [PreguntaGrado] ([Id], [PreguntaId], [Grado], [CreatedAt], [UpdatedAt])
VALUES (N'2', N'42', N'Lo hace con mucha dificultad', '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'PreguntaId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[PreguntaGrado]'))
    SET IDENTITY_INSERT [PreguntaGrado] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'PreguntaId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[PreguntaGrado]'))
    SET IDENTITY_INSERT [PreguntaGrado] ON;
INSERT INTO [PreguntaGrado] ([Id], [PreguntaId], [Grado], [CreatedAt], [UpdatedAt])
VALUES (N'3', N'42', N'Lo hace con poca dificultad', '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'PreguntaId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[PreguntaGrado]'))
    SET IDENTITY_INSERT [PreguntaGrado] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'PreguntaId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[PreguntaGrado]'))
    SET IDENTITY_INSERT [PreguntaGrado] ON;
INSERT INTO [PreguntaGrado] ([Id], [PreguntaId], [Grado], [CreatedAt], [UpdatedAt])
VALUES (N'4', N'42', N'No tiene dificultad', '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'PreguntaId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[PreguntaGrado]'))
    SET IDENTITY_INSERT [PreguntaGrado] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] ON;
INSERT INTO [RespuestaGrado] ([Id], [RespuestaId], [PreguntaGradoId], [Grado], [CreatedAt], [UpdatedAt])
VALUES (N'1', N'117', N'1', N'11', '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] ON;
INSERT INTO [RespuestaGrado] ([Id], [RespuestaId], [PreguntaGradoId], [Grado], [CreatedAt], [UpdatedAt])
VALUES (N'2', N'117', N'2', N'12', '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] ON;
INSERT INTO [RespuestaGrado] ([Id], [RespuestaId], [PreguntaGradoId], [Grado], [CreatedAt], [UpdatedAt])
VALUES (N'3', N'117', N'3', N'13', '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] ON;
INSERT INTO [RespuestaGrado] ([Id], [RespuestaId], [PreguntaGradoId], [Grado], [CreatedAt], [UpdatedAt])
VALUES (N'4', N'117', N'4', N'14', '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] ON;
INSERT INTO [RespuestaGrado] ([Id], [RespuestaId], [PreguntaGradoId], [Grado], [CreatedAt], [UpdatedAt])
VALUES (N'5', N'118', N'1', N'21', '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] ON;
INSERT INTO [RespuestaGrado] ([Id], [RespuestaId], [PreguntaGradoId], [Grado], [CreatedAt], [UpdatedAt])
VALUES (N'6', N'118', N'2', N'22', '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] ON;
INSERT INTO [RespuestaGrado] ([Id], [RespuestaId], [PreguntaGradoId], [Grado], [CreatedAt], [UpdatedAt])
VALUES (N'7', N'118', N'3', N'23', '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] ON;
INSERT INTO [RespuestaGrado] ([Id], [RespuestaId], [PreguntaGradoId], [Grado], [CreatedAt], [UpdatedAt])
VALUES (N'8', N'118', N'4', N'24', '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] ON;
INSERT INTO [RespuestaGrado] ([Id], [RespuestaId], [PreguntaGradoId], [Grado], [CreatedAt], [UpdatedAt])
VALUES (N'9', N'119', N'1', N'31', '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] ON;
INSERT INTO [RespuestaGrado] ([Id], [RespuestaId], [PreguntaGradoId], [Grado], [CreatedAt], [UpdatedAt])
VALUES (N'10', N'119', N'2', N'32', '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] ON;
INSERT INTO [RespuestaGrado] ([Id], [RespuestaId], [PreguntaGradoId], [Grado], [CreatedAt], [UpdatedAt])
VALUES (N'11', N'119', N'3', N'33', '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] ON;
INSERT INTO [RespuestaGrado] ([Id], [RespuestaId], [PreguntaGradoId], [Grado], [CreatedAt], [UpdatedAt])
VALUES (N'12', N'119', N'4', N'34', '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] ON;
INSERT INTO [RespuestaGrado] ([Id], [RespuestaId], [PreguntaGradoId], [Grado], [CreatedAt], [UpdatedAt])
VALUES (N'13', N'120', N'1', N'41', '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] ON;
INSERT INTO [RespuestaGrado] ([Id], [RespuestaId], [PreguntaGradoId], [Grado], [CreatedAt], [UpdatedAt])
VALUES (N'14', N'120', N'2', N'42', '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] ON;
INSERT INTO [RespuestaGrado] ([Id], [RespuestaId], [PreguntaGradoId], [Grado], [CreatedAt], [UpdatedAt])
VALUES (N'15', N'120', N'3', N'43', '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] ON;
INSERT INTO [RespuestaGrado] ([Id], [RespuestaId], [PreguntaGradoId], [Grado], [CreatedAt], [UpdatedAt])
VALUES (N'16', N'120', N'4', N'44', '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] ON;
INSERT INTO [RespuestaGrado] ([Id], [RespuestaId], [PreguntaGradoId], [Grado], [CreatedAt], [UpdatedAt])
VALUES (N'17', N'121', N'1', N'51', '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] ON;
INSERT INTO [RespuestaGrado] ([Id], [RespuestaId], [PreguntaGradoId], [Grado], [CreatedAt], [UpdatedAt])
VALUES (N'18', N'121', N'2', N'52', '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] ON;
INSERT INTO [RespuestaGrado] ([Id], [RespuestaId], [PreguntaGradoId], [Grado], [CreatedAt], [UpdatedAt])
VALUES (N'19', N'121', N'3', N'53', '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] ON;
INSERT INTO [RespuestaGrado] ([Id], [RespuestaId], [PreguntaGradoId], [Grado], [CreatedAt], [UpdatedAt])
VALUES (N'20', N'121', N'4', N'54', '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] ON;
INSERT INTO [RespuestaGrado] ([Id], [RespuestaId], [PreguntaGradoId], [Grado], [CreatedAt], [UpdatedAt])
VALUES (N'21', N'122', N'1', N'61', '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] ON;
INSERT INTO [RespuestaGrado] ([Id], [RespuestaId], [PreguntaGradoId], [Grado], [CreatedAt], [UpdatedAt])
VALUES (N'22', N'122', N'2', N'62', '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] ON;
INSERT INTO [RespuestaGrado] ([Id], [RespuestaId], [PreguntaGradoId], [Grado], [CreatedAt], [UpdatedAt])
VALUES (N'23', N'122', N'3', N'63', '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] ON;
INSERT INTO [RespuestaGrado] ([Id], [RespuestaId], [PreguntaGradoId], [Grado], [CreatedAt], [UpdatedAt])
VALUES (N'24', N'122', N'4', N'64', '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] ON;
INSERT INTO [RespuestaGrado] ([Id], [RespuestaId], [PreguntaGradoId], [Grado], [CreatedAt], [UpdatedAt])
VALUES (N'25', N'123', N'1', N'71', '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] ON;
INSERT INTO [RespuestaGrado] ([Id], [RespuestaId], [PreguntaGradoId], [Grado], [CreatedAt], [UpdatedAt])
VALUES (N'26', N'123', N'2', N'72', '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] ON;
INSERT INTO [RespuestaGrado] ([Id], [RespuestaId], [PreguntaGradoId], [Grado], [CreatedAt], [UpdatedAt])
VALUES (N'27', N'123', N'3', N'73', '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] ON;
INSERT INTO [RespuestaGrado] ([Id], [RespuestaId], [PreguntaGradoId], [Grado], [CreatedAt], [UpdatedAt])
VALUES (N'28', N'123', N'4', N'74', '2019-09-10T16:55:04.8529711-05:00', '2019-09-10T16:55:04.8529711-05:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RespuestaId', N'PreguntaGradoId', N'Grado', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[RespuestaGrado]'))
    SET IDENTITY_INSERT [RespuestaGrado] OFF;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190830185112_Seeder', N'2.1.11-servicing-32099');

GO

ALTER TABLE [Solicitudes] ADD [DomicilioId] nvarchar(450) NULL;

GO

CREATE INDEX [IX_Solicitudes_DomicilioId] ON [Solicitudes] ([DomicilioId]);

GO

ALTER TABLE [Solicitudes] ADD CONSTRAINT [FK_Solicitudes_Domicilio_DomicilioId] FOREIGN KEY ([DomicilioId]) REFERENCES [Domicilio] ([Id]) ON DELETE NO ACTION;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190905203454_AddSolicitudDomicilio', N'2.1.11-servicing-32099');

GO


