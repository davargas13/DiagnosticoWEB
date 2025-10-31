ALTER TABLE [AspNetUsers] ADD [LastLoginDate] datetime2 NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20191029153859_LastLoginDate', N'2.1.11-servicing-32099');

GO

CREATE TABLE [Bitacora] (
    [Id] nvarchar(450) NOT NULL,
    [UsuarioId] nvarchar(450) NULL,
    [Accion] nvarchar(450) NULL,
    [Mensaje] nvarchar(450) NULL,
    [CreatedAt] datetime2 NULL,
    [UpdatedAt] datetime2 NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_Bitacora] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Bitacora_Usuarios] FOREIGN KEY ([UsuarioId]) REFERENCES [AspNetUsers] ([Id])
);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20191029211533_CrearBitacora', N'2.1.11-servicing-32099');

GO

CREATE TABLE [Permiso] (
    [Id] nvarchar(450) NOT NULL,
    [Clave] nvarchar(450) NULL,
    [Nombre] nvarchar(450) NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NOT NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_Permiso] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [Perfil] (
    [Id] nvarchar(450) NOT NULL,
    [Nombre] nvarchar(450) NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NOT NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_Perfil] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [PerfilPermiso] (
    [Id] nvarchar(450) NOT NULL,
    [PerfilId] nvarchar(450) NULL,
    [PermisoId] nvarchar(450) NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NOT NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_PerfilPermiso] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_PerfilPermiso_Perfil] FOREIGN KEY ([PerfilId]) REFERENCES [Perfil] ([Id]),
    CONSTRAINT [FK_PerfilPermiso_Permiso] FOREIGN KEY ([PermisoId]) REFERENCES [Permiso] ([Id])
);

GO

ALTER TABLE [AspNetUsers] ADD [PerfilId] nvarchar(450) NULL;

GO

ALTER TABLE [AspNetUsers] ADD CONSTRAINT [FK_Perfil_Usuarios] FOREIGN KEY ([PerfilId]) REFERENCES [Perfil] ([Id]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20191030211955_crear_perfil_permiso', N'2.1.11-servicing-32099');

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Clave', N'Nombre', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Permiso]'))
    SET IDENTITY_INSERT [Permiso] ON;
INSERT INTO [Permiso] ([Id], [Clave], [Nombre], [CreatedAt], [UpdatedAt])
VALUES (N'1', N'catalogos.ver', N'Ver catálogos iniciales', '2019-11-22T10:55:54.1921594-06:00', '2019-11-22T10:55:54.1921594-06:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Clave', N'Nombre', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Permiso]'))
    SET IDENTITY_INSERT [Permiso] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Clave', N'Nombre', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Permiso]'))
    SET IDENTITY_INSERT [Permiso] ON;
INSERT INTO [Permiso] ([Id], [Clave], [Nombre], [CreatedAt], [UpdatedAt])
VALUES (N'2', N'catalogos.exportar', N'Exportar catálogos iniciales', '2019-11-22T10:55:54.1921594-06:00', '2019-11-22T10:55:54.1921594-06:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Clave', N'Nombre', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Permiso]'))
    SET IDENTITY_INSERT [Permiso] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Clave', N'Nombre', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Permiso]'))
    SET IDENTITY_INSERT [Permiso] ON;
INSERT INTO [Permiso] ([Id], [Clave], [Nombre], [CreatedAt], [UpdatedAt])
VALUES (N'3', N'catalogos.importar', N'Importar catálogos iniciales', '2019-11-22T10:55:54.1921594-06:00', '2019-11-22T10:55:54.1921594-06:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Clave', N'Nombre', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Permiso]'))
    SET IDENTITY_INSERT [Permiso] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Clave', N'Nombre', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Permiso]'))
    SET IDENTITY_INSERT [Permiso] ON;
INSERT INTO [Permiso] ([Id], [Clave], [Nombre], [CreatedAt], [UpdatedAt])
VALUES (N'4', N'catalogos.crear', N'Crear catálogos iniciales', '2019-11-22T10:55:54.1921594-06:00', '2019-11-22T10:55:54.1921594-06:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Clave', N'Nombre', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Permiso]'))
    SET IDENTITY_INSERT [Permiso] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Clave', N'Nombre', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Permiso]'))
    SET IDENTITY_INSERT [Permiso] ON;
INSERT INTO [Permiso] ([Id], [Clave], [Nombre], [CreatedAt], [UpdatedAt])
VALUES (N'5', N'catalogos.editar', N'Editar catálogos iniciales', '2019-11-22T10:55:54.1921594-06:00', '2019-11-22T10:55:54.1921594-06:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Clave', N'Nombre', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Permiso]'))
    SET IDENTITY_INSERT [Permiso] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Clave', N'Nombre', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Permiso]'))
    SET IDENTITY_INSERT [Permiso] ON;
INSERT INTO [Permiso] ([Id], [Clave], [Nombre], [CreatedAt], [UpdatedAt])
VALUES (N'6', N'trabajadorcampo.crear', N'Crear trabajador de campo', '2019-11-22T10:55:54.1921594-06:00', '2019-11-22T10:55:54.1921594-06:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Clave', N'Nombre', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Permiso]'))
    SET IDENTITY_INSERT [Permiso] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Clave', N'Nombre', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Permiso]'))
    SET IDENTITY_INSERT [Permiso] ON;
INSERT INTO [Permiso] ([Id], [Clave], [Nombre], [CreatedAt], [UpdatedAt])
VALUES (N'7', N'trabajadorcampo.editar', N'Editar trabajador de campo', '2019-11-22T10:55:54.1921594-06:00', '2019-11-22T10:55:54.1921594-06:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Clave', N'Nombre', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Permiso]'))
    SET IDENTITY_INSERT [Permiso] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Clave', N'Nombre', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Permiso]'))
    SET IDENTITY_INSERT [Permiso] ON;
INSERT INTO [Permiso] ([Id], [Clave], [Nombre], [CreatedAt], [UpdatedAt])
VALUES (N'8', N'trabajadorcampo.ver', N'Ver trabajador de campo', '2019-11-22T10:55:54.1921594-06:00', '2019-11-22T10:55:54.1921594-06:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Clave', N'Nombre', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Permiso]'))
    SET IDENTITY_INSERT [Permiso] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Clave', N'Nombre', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Permiso]'))
    SET IDENTITY_INSERT [Permiso] ON;
INSERT INTO [Permiso] ([Id], [Clave], [Nombre], [CreatedAt], [UpdatedAt])
VALUES (N'9', N'trabajadorcampo.eliminar', N'Eliminar trabajador de campo', '2019-11-22T10:55:54.1921594-06:00', '2019-11-22T10:55:54.1921594-06:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Clave', N'Nombre', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Permiso]'))
    SET IDENTITY_INSERT [Permiso] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Clave', N'Nombre', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Permiso]'))
    SET IDENTITY_INSERT [Permiso] ON;
INSERT INTO [Permiso] ([Id], [Clave], [Nombre], [CreatedAt], [UpdatedAt])
VALUES (N'10', N'dependencias.ver', N'Ver catálogo de dependencias', '2019-11-22T10:55:54.1921594-06:00', '2019-11-22T10:55:54.1921594-06:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Clave', N'Nombre', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Permiso]'))
    SET IDENTITY_INSERT [Permiso] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Clave', N'Nombre', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Permiso]'))
    SET IDENTITY_INSERT [Permiso] ON;
INSERT INTO [Permiso] ([Id], [Clave], [Nombre], [CreatedAt], [UpdatedAt])
VALUES (N'11', N'dependencias.crear', N'Crear dependencia', '2019-11-22T10:55:54.1921594-06:00', '2019-11-22T10:55:54.1921594-06:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Clave', N'Nombre', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Permiso]'))
    SET IDENTITY_INSERT [Permiso] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Clave', N'Nombre', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Permiso]'))
    SET IDENTITY_INSERT [Permiso] ON;
INSERT INTO [Permiso] ([Id], [Clave], [Nombre], [CreatedAt], [UpdatedAt])
VALUES (N'12', N'dependencias.editar', N'Editar dependencia', '2019-11-22T10:55:54.1921594-06:00', '2019-11-22T10:55:54.1921594-06:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Clave', N'Nombre', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Permiso]'))
    SET IDENTITY_INSERT [Permiso] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Clave', N'Nombre', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Permiso]'))
    SET IDENTITY_INSERT [Permiso] ON;
INSERT INTO [Permiso] ([Id], [Clave], [Nombre], [CreatedAt], [UpdatedAt])
VALUES (N'13', N'programasocial.ver', N'Ver catálogo de programas sociales', '2019-11-22T10:55:54.1921594-06:00', '2019-11-22T10:55:54.1921594-06:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Clave', N'Nombre', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Permiso]'))
    SET IDENTITY_INSERT [Permiso] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Clave', N'Nombre', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Permiso]'))
    SET IDENTITY_INSERT [Permiso] ON;
INSERT INTO [Permiso] ([Id], [Clave], [Nombre], [CreatedAt], [UpdatedAt])
VALUES (N'14', N'programasocial.editar', N'Editar programa social', '2019-11-22T10:55:54.1921594-06:00', '2019-11-22T10:55:54.1921594-06:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Clave', N'Nombre', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Permiso]'))
    SET IDENTITY_INSERT [Permiso] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Clave', N'Nombre', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Permiso]'))
    SET IDENTITY_INSERT [Permiso] ON;
INSERT INTO [Permiso] ([Id], [Clave], [Nombre], [CreatedAt], [UpdatedAt])
VALUES (N'15', N'programasocial.crear', N'Crear programa social', '2019-11-22T10:55:54.1921594-06:00', '2019-11-22T10:55:54.1921594-06:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Clave', N'Nombre', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Permiso]'))
    SET IDENTITY_INSERT [Permiso] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Clave', N'Nombre', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Permiso]'))
    SET IDENTITY_INSERT [Permiso] ON;
INSERT INTO [Permiso] ([Id], [Clave], [Nombre], [CreatedAt], [UpdatedAt])
VALUES (N'16', N'usuarios.ver', N'Ver catálogo de usuarios', '2019-11-22T10:55:54.1921594-06:00', '2019-11-22T10:55:54.1921594-06:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Clave', N'Nombre', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Permiso]'))
    SET IDENTITY_INSERT [Permiso] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Clave', N'Nombre', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Permiso]'))
    SET IDENTITY_INSERT [Permiso] ON;
INSERT INTO [Permiso] ([Id], [Clave], [Nombre], [CreatedAt], [UpdatedAt])
VALUES (N'17', N'usuarios.crear', N'Crear usuario', '2019-11-22T10:55:54.1921594-06:00', '2019-11-22T10:55:54.1921594-06:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Clave', N'Nombre', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Permiso]'))
    SET IDENTITY_INSERT [Permiso] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Clave', N'Nombre', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Permiso]'))
    SET IDENTITY_INSERT [Permiso] ON;
INSERT INTO [Permiso] ([Id], [Clave], [Nombre], [CreatedAt], [UpdatedAt])
VALUES (N'18', N'usuarios.editar', N'Editar usuario', '2019-11-22T10:55:54.1921594-06:00', '2019-11-22T10:55:54.1921594-06:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Clave', N'Nombre', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Permiso]'))
    SET IDENTITY_INSERT [Permiso] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Clave', N'Nombre', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Permiso]'))
    SET IDENTITY_INSERT [Permiso] ON;
INSERT INTO [Permiso] ([Id], [Clave], [Nombre], [CreatedAt], [UpdatedAt])
VALUES (N'19', N'encuesta.ver', N'Ver encuesta', '2019-11-22T10:55:54.1921594-06:00', '2019-11-22T10:55:54.1921594-06:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Clave', N'Nombre', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Permiso]'))
    SET IDENTITY_INSERT [Permiso] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Clave', N'Nombre', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Permiso]'))
    SET IDENTITY_INSERT [Permiso] ON;
INSERT INTO [Permiso] ([Id], [Clave], [Nombre], [CreatedAt], [UpdatedAt])
VALUES (N'20', N'encuesta.editar', N'Editar encuesta', '2019-11-22T10:55:54.1921594-06:00', '2019-11-22T10:55:54.1921594-06:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Clave', N'Nombre', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Permiso]'))
    SET IDENTITY_INSERT [Permiso] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Clave', N'Nombre', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Permiso]'))
    SET IDENTITY_INSERT [Permiso] ON;
INSERT INTO [Permiso] ([Id], [Clave], [Nombre], [CreatedAt], [UpdatedAt])
VALUES (N'21', N'beneficiarios.ver', N'Ver catálogo de beneficiarios', '2019-11-22T10:55:54.1921594-06:00', '2019-11-22T10:55:54.1921594-06:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Clave', N'Nombre', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Permiso]'))
    SET IDENTITY_INSERT [Permiso] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Clave', N'Nombre', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Permiso]'))
    SET IDENTITY_INSERT [Permiso] ON;
INSERT INTO [Permiso] ([Id], [Clave], [Nombre], [CreatedAt], [UpdatedAt])
VALUES (N'22', N'beneficiarios.editar', N'Editar beneficiario', '2019-11-22T10:55:54.1921594-06:00', '2019-11-22T10:55:54.1921594-06:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Clave', N'Nombre', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Permiso]'))
    SET IDENTITY_INSERT [Permiso] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Clave', N'Nombre', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Permiso]'))
    SET IDENTITY_INSERT [Permiso] ON;
INSERT INTO [Permiso] ([Id], [Clave], [Nombre], [CreatedAt], [UpdatedAt])
VALUES (N'23', N'beneficiarios.exportar', N'Exportar catálogo de beneficiarios', '2019-11-22T10:55:54.1921594-06:00', '2019-11-22T10:55:54.1921594-06:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Clave', N'Nombre', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Permiso]'))
    SET IDENTITY_INSERT [Permiso] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Clave', N'Nombre', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Permiso]'))
    SET IDENTITY_INSERT [Permiso] ON;
INSERT INTO [Permiso] ([Id], [Clave], [Nombre], [CreatedAt], [UpdatedAt])
VALUES (N'24', N'solicitudes.ver', N'Ver solicitudes', '2019-11-22T10:55:54.1921594-06:00', '2019-11-22T10:55:54.1921594-06:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Clave', N'Nombre', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Permiso]'))
    SET IDENTITY_INSERT [Permiso] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Clave', N'Nombre', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Permiso]'))
    SET IDENTITY_INSERT [Permiso] ON;
INSERT INTO [Permiso] ([Id], [Clave], [Nombre], [CreatedAt], [UpdatedAt])
VALUES (N'25', N'solicitudes.editar', N'Dictaminar solicitudes', '2019-11-22T10:55:54.1921594-06:00', '2019-11-22T10:55:54.1921594-06:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Clave', N'Nombre', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Permiso]'))
    SET IDENTITY_INSERT [Permiso] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Clave', N'Nombre', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Permiso]'))
    SET IDENTITY_INSERT [Permiso] ON;
INSERT INTO [Permiso] ([Id], [Clave], [Nombre], [CreatedAt], [UpdatedAt])
VALUES (N'26', N'solicitudes.exportar', N'Exportar catálogo de solicitudes', '2019-11-22T10:55:54.1921594-06:00', '2019-11-22T10:55:54.1921594-06:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Clave', N'Nombre', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Permiso]'))
    SET IDENTITY_INSERT [Permiso] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Clave', N'Nombre', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Permiso]'))
    SET IDENTITY_INSERT [Permiso] ON;
INSERT INTO [Permiso] ([Id], [Clave], [Nombre], [CreatedAt], [UpdatedAt])
VALUES (N'27', N'perfil.ver', N'Ver catálogo de perfiles', '2019-11-22T10:55:54.1921594-06:00', '2019-11-22T10:55:54.1921594-06:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Clave', N'Nombre', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Permiso]'))
    SET IDENTITY_INSERT [Permiso] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Clave', N'Nombre', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Permiso]'))
    SET IDENTITY_INSERT [Permiso] ON;
INSERT INTO [Permiso] ([Id], [Clave], [Nombre], [CreatedAt], [UpdatedAt])
VALUES (N'28', N'perfil.crear', N'Crear perfil', '2019-11-22T10:55:54.1921594-06:00', '2019-11-22T10:55:54.1921594-06:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Clave', N'Nombre', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Permiso]'))
    SET IDENTITY_INSERT [Permiso] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Clave', N'Nombre', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Permiso]'))
    SET IDENTITY_INSERT [Permiso] ON;
INSERT INTO [Permiso] ([Id], [Clave], [Nombre], [CreatedAt], [UpdatedAt])
VALUES (N'29', N'perfil.editar', N'Editar perfil', '2019-11-22T10:55:54.1921594-06:00', '2019-11-22T10:55:54.1921594-06:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Clave', N'Nombre', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Permiso]'))
    SET IDENTITY_INSERT [Permiso] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Perfil]'))
    SET IDENTITY_INSERT [Perfil] ON;
INSERT INTO [Perfil] ([Id], [Nombre], [CreatedAt], [UpdatedAt])
VALUES (N'1', N'Administrador', '2019-11-22T10:55:54.1921594-06:00', '2019-11-22T10:55:54.1921594-06:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nombre', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Perfil]'))
    SET IDENTITY_INSERT [Perfil] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'PerfilId', N'PermisoId', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[PerfilPermiso]'))
    SET IDENTITY_INSERT [PerfilPermiso] ON;
INSERT INTO [PerfilPermiso] ([Id], [PerfilId], [PermisoId], [CreatedAt], [UpdatedAt])
VALUES (N'1', N'1', N'1', '2019-11-22T10:55:54.1921594-06:00', '2019-11-22T10:55:54.1921594-06:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'PerfilId', N'PermisoId', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[PerfilPermiso]'))
    SET IDENTITY_INSERT [PerfilPermiso] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'PerfilId', N'PermisoId', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[PerfilPermiso]'))
    SET IDENTITY_INSERT [PerfilPermiso] ON;
INSERT INTO [PerfilPermiso] ([Id], [PerfilId], [PermisoId], [CreatedAt], [UpdatedAt])
VALUES (N'2', N'1', N'2', '2019-11-22T10:55:54.1921594-06:00', '2019-11-22T10:55:54.1921594-06:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'PerfilId', N'PermisoId', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[PerfilPermiso]'))
    SET IDENTITY_INSERT [PerfilPermiso] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'PerfilId', N'PermisoId', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[PerfilPermiso]'))
    SET IDENTITY_INSERT [PerfilPermiso] ON;
INSERT INTO [PerfilPermiso] ([Id], [PerfilId], [PermisoId], [CreatedAt], [UpdatedAt])
VALUES (N'3', N'1', N'3', '2019-11-22T10:55:54.1921594-06:00', '2019-11-22T10:55:54.1921594-06:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'PerfilId', N'PermisoId', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[PerfilPermiso]'))
    SET IDENTITY_INSERT [PerfilPermiso] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'PerfilId', N'PermisoId', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[PerfilPermiso]'))
    SET IDENTITY_INSERT [PerfilPermiso] ON;
INSERT INTO [PerfilPermiso] ([Id], [PerfilId], [PermisoId], [CreatedAt], [UpdatedAt])
VALUES (N'4', N'1', N'4', '2019-11-22T10:55:54.1921594-06:00', '2019-11-22T10:55:54.1921594-06:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'PerfilId', N'PermisoId', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[PerfilPermiso]'))
    SET IDENTITY_INSERT [PerfilPermiso] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'PerfilId', N'PermisoId', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[PerfilPermiso]'))
    SET IDENTITY_INSERT [PerfilPermiso] ON;
INSERT INTO [PerfilPermiso] ([Id], [PerfilId], [PermisoId], [CreatedAt], [UpdatedAt])
VALUES (N'5', N'1', N'5', '2019-11-22T10:55:54.1921594-06:00', '2019-11-22T10:55:54.1921594-06:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'PerfilId', N'PermisoId', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[PerfilPermiso]'))
    SET IDENTITY_INSERT [PerfilPermiso] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'PerfilId', N'PermisoId', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[PerfilPermiso]'))
    SET IDENTITY_INSERT [PerfilPermiso] ON;
INSERT INTO [PerfilPermiso] ([Id], [PerfilId], [PermisoId], [CreatedAt], [UpdatedAt])
VALUES (N'6', N'1', N'6', '2019-11-22T10:55:54.1921594-06:00', '2019-11-22T10:55:54.1921594-06:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'PerfilId', N'PermisoId', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[PerfilPermiso]'))
    SET IDENTITY_INSERT [PerfilPermiso] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'PerfilId', N'PermisoId', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[PerfilPermiso]'))
    SET IDENTITY_INSERT [PerfilPermiso] ON;
INSERT INTO [PerfilPermiso] ([Id], [PerfilId], [PermisoId], [CreatedAt], [UpdatedAt])
VALUES (N'7', N'1', N'7', '2019-11-22T10:55:54.1921594-06:00', '2019-11-22T10:55:54.1921594-06:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'PerfilId', N'PermisoId', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[PerfilPermiso]'))
    SET IDENTITY_INSERT [PerfilPermiso] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'PerfilId', N'PermisoId', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[PerfilPermiso]'))
    SET IDENTITY_INSERT [PerfilPermiso] ON;
INSERT INTO [PerfilPermiso] ([Id], [PerfilId], [PermisoId], [CreatedAt], [UpdatedAt])
VALUES (N'8', N'1', N'8', '2019-11-22T10:55:54.1921594-06:00', '2019-11-22T10:55:54.1921594-06:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'PerfilId', N'PermisoId', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[PerfilPermiso]'))
    SET IDENTITY_INSERT [PerfilPermiso] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'PerfilId', N'PermisoId', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[PerfilPermiso]'))
    SET IDENTITY_INSERT [PerfilPermiso] ON;
INSERT INTO [PerfilPermiso] ([Id], [PerfilId], [PermisoId], [CreatedAt], [UpdatedAt])
VALUES (N'9', N'1', N'9', '2019-11-22T10:55:54.1921594-06:00', '2019-11-22T10:55:54.1921594-06:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'PerfilId', N'PermisoId', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[PerfilPermiso]'))
    SET IDENTITY_INSERT [PerfilPermiso] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'PerfilId', N'PermisoId', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[PerfilPermiso]'))
    SET IDENTITY_INSERT [PerfilPermiso] ON;
INSERT INTO [PerfilPermiso] ([Id], [PerfilId], [PermisoId], [CreatedAt], [UpdatedAt])
VALUES (N'10', N'1', N'10', '2019-11-22T10:55:54.1921594-06:00', '2019-11-22T10:55:54.1921594-06:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'PerfilId', N'PermisoId', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[PerfilPermiso]'))
    SET IDENTITY_INSERT [PerfilPermiso] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'PerfilId', N'PermisoId', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[PerfilPermiso]'))
    SET IDENTITY_INSERT [PerfilPermiso] ON;
INSERT INTO [PerfilPermiso] ([Id], [PerfilId], [PermisoId], [CreatedAt], [UpdatedAt])
VALUES (N'11', N'1', N'11', '2019-11-22T10:55:54.1921594-06:00', '2019-11-22T10:55:54.1921594-06:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'PerfilId', N'PermisoId', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[PerfilPermiso]'))
    SET IDENTITY_INSERT [PerfilPermiso] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'PerfilId', N'PermisoId', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[PerfilPermiso]'))
    SET IDENTITY_INSERT [PerfilPermiso] ON;
INSERT INTO [PerfilPermiso] ([Id], [PerfilId], [PermisoId], [CreatedAt], [UpdatedAt])
VALUES (N'12', N'1', N'12', '2019-11-22T10:55:54.1921594-06:00', '2019-11-22T10:55:54.1921594-06:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'PerfilId', N'PermisoId', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[PerfilPermiso]'))
    SET IDENTITY_INSERT [PerfilPermiso] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'PerfilId', N'PermisoId', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[PerfilPermiso]'))
    SET IDENTITY_INSERT [PerfilPermiso] ON;
INSERT INTO [PerfilPermiso] ([Id], [PerfilId], [PermisoId], [CreatedAt], [UpdatedAt])
VALUES (N'13', N'1', N'13', '2019-11-22T10:55:54.1921594-06:00', '2019-11-22T10:55:54.1921594-06:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'PerfilId', N'PermisoId', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[PerfilPermiso]'))
    SET IDENTITY_INSERT [PerfilPermiso] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'PerfilId', N'PermisoId', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[PerfilPermiso]'))
    SET IDENTITY_INSERT [PerfilPermiso] ON;
INSERT INTO [PerfilPermiso] ([Id], [PerfilId], [PermisoId], [CreatedAt], [UpdatedAt])
VALUES (N'14', N'1', N'14', '2019-11-22T10:55:54.1921594-06:00', '2019-11-22T10:55:54.1921594-06:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'PerfilId', N'PermisoId', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[PerfilPermiso]'))
    SET IDENTITY_INSERT [PerfilPermiso] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'PerfilId', N'PermisoId', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[PerfilPermiso]'))
    SET IDENTITY_INSERT [PerfilPermiso] ON;
INSERT INTO [PerfilPermiso] ([Id], [PerfilId], [PermisoId], [CreatedAt], [UpdatedAt])
VALUES (N'15', N'1', N'15', '2019-11-22T10:55:54.1921594-06:00', '2019-11-22T10:55:54.1921594-06:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'PerfilId', N'PermisoId', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[PerfilPermiso]'))
    SET IDENTITY_INSERT [PerfilPermiso] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'PerfilId', N'PermisoId', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[PerfilPermiso]'))
    SET IDENTITY_INSERT [PerfilPermiso] ON;
INSERT INTO [PerfilPermiso] ([Id], [PerfilId], [PermisoId], [CreatedAt], [UpdatedAt])
VALUES (N'16', N'1', N'16', '2019-11-22T10:55:54.1921594-06:00', '2019-11-22T10:55:54.1921594-06:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'PerfilId', N'PermisoId', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[PerfilPermiso]'))
    SET IDENTITY_INSERT [PerfilPermiso] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'PerfilId', N'PermisoId', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[PerfilPermiso]'))
    SET IDENTITY_INSERT [PerfilPermiso] ON;
INSERT INTO [PerfilPermiso] ([Id], [PerfilId], [PermisoId], [CreatedAt], [UpdatedAt])
VALUES (N'17', N'1', N'17', '2019-11-22T10:55:54.1921594-06:00', '2019-11-22T10:55:54.1921594-06:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'PerfilId', N'PermisoId', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[PerfilPermiso]'))
    SET IDENTITY_INSERT [PerfilPermiso] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'PerfilId', N'PermisoId', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[PerfilPermiso]'))
    SET IDENTITY_INSERT [PerfilPermiso] ON;
INSERT INTO [PerfilPermiso] ([Id], [PerfilId], [PermisoId], [CreatedAt], [UpdatedAt])
VALUES (N'18', N'1', N'18', '2019-11-22T10:55:54.1921594-06:00', '2019-11-22T10:55:54.1921594-06:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'PerfilId', N'PermisoId', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[PerfilPermiso]'))
    SET IDENTITY_INSERT [PerfilPermiso] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'PerfilId', N'PermisoId', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[PerfilPermiso]'))
    SET IDENTITY_INSERT [PerfilPermiso] ON;
INSERT INTO [PerfilPermiso] ([Id], [PerfilId], [PermisoId], [CreatedAt], [UpdatedAt])
VALUES (N'19', N'1', N'19', '2019-11-22T10:55:54.1921594-06:00', '2019-11-22T10:55:54.1921594-06:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'PerfilId', N'PermisoId', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[PerfilPermiso]'))
    SET IDENTITY_INSERT [PerfilPermiso] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'PerfilId', N'PermisoId', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[PerfilPermiso]'))
    SET IDENTITY_INSERT [PerfilPermiso] ON;
INSERT INTO [PerfilPermiso] ([Id], [PerfilId], [PermisoId], [CreatedAt], [UpdatedAt])
VALUES (N'20', N'1', N'20', '2019-11-22T10:55:54.1921594-06:00', '2019-11-22T10:55:54.1921594-06:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'PerfilId', N'PermisoId', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[PerfilPermiso]'))
    SET IDENTITY_INSERT [PerfilPermiso] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'PerfilId', N'PermisoId', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[PerfilPermiso]'))
    SET IDENTITY_INSERT [PerfilPermiso] ON;
INSERT INTO [PerfilPermiso] ([Id], [PerfilId], [PermisoId], [CreatedAt], [UpdatedAt])
VALUES (N'21', N'1', N'21', '2019-11-22T10:55:54.1921594-06:00', '2019-11-22T10:55:54.1921594-06:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'PerfilId', N'PermisoId', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[PerfilPermiso]'))
    SET IDENTITY_INSERT [PerfilPermiso] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'PerfilId', N'PermisoId', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[PerfilPermiso]'))
    SET IDENTITY_INSERT [PerfilPermiso] ON;
INSERT INTO [PerfilPermiso] ([Id], [PerfilId], [PermisoId], [CreatedAt], [UpdatedAt])
VALUES (N'22', N'1', N'22', '2019-11-22T10:55:54.1921594-06:00', '2019-11-22T10:55:54.1921594-06:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'PerfilId', N'PermisoId', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[PerfilPermiso]'))
    SET IDENTITY_INSERT [PerfilPermiso] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'PerfilId', N'PermisoId', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[PerfilPermiso]'))
    SET IDENTITY_INSERT [PerfilPermiso] ON;
INSERT INTO [PerfilPermiso] ([Id], [PerfilId], [PermisoId], [CreatedAt], [UpdatedAt])
VALUES (N'23', N'1', N'23', '2019-11-22T10:55:54.1921594-06:00', '2019-11-22T10:55:54.1921594-06:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'PerfilId', N'PermisoId', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[PerfilPermiso]'))
    SET IDENTITY_INSERT [PerfilPermiso] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'PerfilId', N'PermisoId', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[PerfilPermiso]'))
    SET IDENTITY_INSERT [PerfilPermiso] ON;
INSERT INTO [PerfilPermiso] ([Id], [PerfilId], [PermisoId], [CreatedAt], [UpdatedAt])
VALUES (N'24', N'1', N'24', '2019-11-22T10:55:54.1921594-06:00', '2019-11-22T10:55:54.1921594-06:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'PerfilId', N'PermisoId', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[PerfilPermiso]'))
    SET IDENTITY_INSERT [PerfilPermiso] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'PerfilId', N'PermisoId', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[PerfilPermiso]'))
    SET IDENTITY_INSERT [PerfilPermiso] ON;
INSERT INTO [PerfilPermiso] ([Id], [PerfilId], [PermisoId], [CreatedAt], [UpdatedAt])
VALUES (N'25', N'1', N'25', '2019-11-22T10:55:54.1921594-06:00', '2019-11-22T10:55:54.1921594-06:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'PerfilId', N'PermisoId', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[PerfilPermiso]'))
    SET IDENTITY_INSERT [PerfilPermiso] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'PerfilId', N'PermisoId', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[PerfilPermiso]'))
    SET IDENTITY_INSERT [PerfilPermiso] ON;
INSERT INTO [PerfilPermiso] ([Id], [PerfilId], [PermisoId], [CreatedAt], [UpdatedAt])
VALUES (N'26', N'1', N'26', '2019-11-22T10:55:54.1921594-06:00', '2019-11-22T10:55:54.1921594-06:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'PerfilId', N'PermisoId', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[PerfilPermiso]'))
    SET IDENTITY_INSERT [PerfilPermiso] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'PerfilId', N'PermisoId', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[PerfilPermiso]'))
    SET IDENTITY_INSERT [PerfilPermiso] ON;
INSERT INTO [PerfilPermiso] ([Id], [PerfilId], [PermisoId], [CreatedAt], [UpdatedAt])
VALUES (N'27', N'1', N'27', '2019-11-22T10:55:54.1921594-06:00', '2019-11-22T10:55:54.1921594-06:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'PerfilId', N'PermisoId', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[PerfilPermiso]'))
    SET IDENTITY_INSERT [PerfilPermiso] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'PerfilId', N'PermisoId', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[PerfilPermiso]'))
    SET IDENTITY_INSERT [PerfilPermiso] ON;
INSERT INTO [PerfilPermiso] ([Id], [PerfilId], [PermisoId], [CreatedAt], [UpdatedAt])
VALUES (N'28', N'1', N'28', '2019-11-22T10:55:54.1921594-06:00', '2019-11-22T10:55:54.1921594-06:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'PerfilId', N'PermisoId', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[PerfilPermiso]'))
    SET IDENTITY_INSERT [PerfilPermiso] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'PerfilId', N'PermisoId', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[PerfilPermiso]'))
    SET IDENTITY_INSERT [PerfilPermiso] ON;
INSERT INTO [PerfilPermiso] ([Id], [PerfilId], [PermisoId], [CreatedAt], [UpdatedAt])
VALUES (N'29', N'1', N'29', '2019-11-22T10:55:54.1921594-06:00', '2019-11-22T10:55:54.1921594-06:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'PerfilId', N'PermisoId', N'CreatedAt', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[PerfilPermiso]'))
    SET IDENTITY_INSERT [PerfilPermiso] OFF;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20191031171259_CrearPermisoSeeder', N'2.1.11-servicing-32099');

GO

ALTER TABLE [BeneficiarioHistorico] ADD [EstatusInformacion] nvarchar(450) NULL;

GO

ALTER TABLE [BeneficiarioHistorico] ADD [EstatusUpdatedAt] datetime2 NULL;

GO

ALTER TABLE [BeneficiarioHistorico] ADD [GradoEstudioId] nvarchar(450) NULL;

GO

ALTER TABLE [BeneficiarioHistorico] ADD CONSTRAINT [FK_BeneficiarioHistorico_GradosEstudio] FOREIGN KEY ([GradoEstudioId]) REFERENCES [GradosEstudio] ([Id]);

GO

ALTER TABLE [BeneficiarioHistorico] ADD [CausaDiscapacidadId] nvarchar(450) NULL;

GO

ALTER TABLE [BeneficiarioHistorico] ADD CONSTRAINT [FK_BeneficiarioHistorico_CausaDiscapacidad] FOREIGN KEY ([CausaDiscapacidadId]) REFERENCES [CausaDiscapacidad] ([Id]);

GO

ALTER TABLE [BeneficiarioHistorico] ADD [DiscapacidadGradoId] nvarchar(450) NULL;

GO

ALTER TABLE [BeneficiarioHistorico] ADD CONSTRAINT [FK_BeneficiarioHistorico_DiscapacidadGrado] FOREIGN KEY ([DiscapacidadGradoId]) REFERENCES [DiscapacidadGrado] ([Id]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20191106221649_AddCamposHistorico', N'2.1.11-servicing-32099');

GO

