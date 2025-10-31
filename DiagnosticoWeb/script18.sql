CREATE TABLE [Colonias] (
    [Id] nvarchar(450) NOT NULL,
    [Nombre] nvarchar(256) NOT NULL,
    [CodigoPostal] nvarchar(5) NOT NULL,
    [MunicipioId] nvarchar(450) NOT NULL,
    [CreatedAt] datetime2 NULL,
    [UpdatedAt] datetime2 NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_Colonias] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Colonias_Municipio] FOREIGN KEY ([MunicipioId]) REFERENCES [Municipios] ([Id])
);

GO

CREATE INDEX [IX_Colonias_MunicipioId] ON [Colonias] ([MunicipioId]);

GO

CREATE TABLE [Calles] (
    [Id] nvarchar(450) NOT NULL,
    [Nombre] nvarchar(256) NOT NULL,
    [MunicipioId] nvarchar(450) NOT NULL,
    [LocalidadId] nvarchar(450) NOT NULL,
    [CreatedAt] datetime2 NULL,
    [UpdatedAt] datetime2 NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_Calles] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Calles_Municipio] FOREIGN KEY ([MunicipioId]) REFERENCES [Municipios] ([Id]),
    CONSTRAINT [FK_Calles_Localidad] FOREIGN KEY ([LocalidadId]) REFERENCES [Localidades] ([Id])
);

GO

CREATE INDEX [IX_Calles_MunicipioId] ON [Calles] ([MunicipioId]);

GO

CREATE INDEX [IX_Calles_LocalidadId] ON [Calles] ([LocalidadId]);

GO

ALTER TABLE [Domicilio] ADD [ColoniaId] nvarchar(450) NULL;

GO

ALTER TABLE [Domicilio] ADD [CalleId] nvarchar(450) NULL;

GO

CREATE INDEX [IX_Colonias_DomicilioId] ON [Domicilio] ([ColoniaId]);

GO

CREATE INDEX [IX_Calles_DomicilioId] ON [Domicilio] ([CalleId]);

GO

ALTER TABLE [Domicilio] ADD CONSTRAINT [FK_Colonias_Domicilio_ColoniaId] FOREIGN KEY ([ColoniaId]) REFERENCES [Colonias] ([Id]);

GO

ALTER TABLE [Domicilio] ADD CONSTRAINT [FK_Calles_Domicilio_CalleId] FOREIGN KEY ([CalleId]) REFERENCES [Calles] ([Id]);

GO

EXEC sp_rename N'[Domicilio].[Colonia]', N'ColoniaCalculada', N'COLUMN';

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20201125214736_AddCalleDomicilio', N'2.1.0-rtm-30799');

GO

ALTER TABLE [Domicilio] ADD [Indice] real NULL;

GO

ALTER TABLE [Domicilio] ADD [IndiceDesarrolloHumano] nvarchar(50) NULL;

GO

ALTER TABLE [Domicilio] ADD [MarginacionMunicipio] nvarchar(50) NULL;

GO

ALTER TABLE [Domicilio] ADD [MarginacionLocalidad] nvarchar(50) NULL;

GO

ALTER TABLE [Domicilio] ADD [MarginacionAgeb] nvarchar(50) NULL;

GO

ALTER TABLE [Domicilio] ADD [ClaveMunicipioCalculado] nvarchar(50) NULL;

GO

ALTER TABLE [Domicilio] ADD [ClaveLocalidadCalculada] nvarchar(50) NULL;

GO

ALTER TABLE [Domicilio] ADD [ClaveMunicipioCisCalculado] nvarchar(50) NULL;

GO

ALTER TABLE [Domicilio] ADD [DomicilioCisCalculado] nvarchar(200) NULL;

GO

ALTER TABLE [Domicilio] ADD [ZonaImpulsoCalculada] nvarchar(200) NULL;

GO

ALTER TABLE [Domicilio] ADD [ClaveZonaImpulsoCalculada] nvarchar(50) NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20201127144617_AddIndicesDomicilio', N'2.1.0-rtm-30799');

GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Configuracion]') AND [c].[name] = N'Valor');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Configuracion] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [Configuracion] ALTER COLUMN [Valor] nvarchar(max) NOT NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20201205200147_ChaneValorConfiguracion', N'2.1.0-rtm-30799');

GO

CREATE TABLE [AlgoritmoVersiones] (
    [Id] nvarchar(450) NOT NULL,
    [Version] int NOT NULL DEFAULT 0,
    [FechaAutorizacion] datetime2 NULL,
    [CreatedAt] datetime2 NULL,
    [UpdatedAt] datetime2 NULL,
    [UsuarioId] nvarchar(450) NOT NULL,
    [AutorizadorId] nvarchar(450) NULL,
    CONSTRAINT [PK_AlgoritmoVersiones] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AlgoritmoVersiones_AspNetUsers_UsuarioId] FOREIGN KEY ([UsuarioId]) REFERENCES [AspNetUsers] ([Id]),
    CONSTRAINT [FK_AlgoritmoVersiones_AspNetUsers_AutorizadorId] FOREIGN KEY ([AutorizadorId]) REFERENCES [AspNetUsers] ([Id])
);

GO

CREATE INDEX [IX_AlgoritmoVersiones_UsuarioId] ON [AlgoritmoVersiones] ([UsuarioId]);

GO

CREATE INDEX [IX_AlgoritmoVersiones_AutorizadorId] ON [AlgoritmoVersiones] ([AutorizadorId]);

GO

CREATE TABLE [AlgoritmoResultados] (
    [Id] nvarchar(450) NOT NULL,
    [Educativa] bit NOT NULL,
    [Analfabetismo] bit NOT NULL,
    [Inasistencia] bit NOT NULL,
    [PrimariaIncompleta] bit NOT NULL,
    [SecundariaIncompleta] bit NOT NULL,
    [ServicioSalud] bit NOT NULL,
    [SeguridadSocial] bit NOT NULL,
    [Vivienda] bit NOT NULL,
    [Piso] bit NOT NULL,
    [Techo] bit NOT NULL,
    [Muro] bit NOT NULL,
    [Hacinamiento] bit NOT NULL,
    [Servicios] bit NOT NULL,
    [Agua] bit NOT NULL,
    [Drenaje] bit NOT NULL,
    [Electricidad] bit NOT NULL,
    [Combustible] bit NOT NULL,
    [Alimentaria] bit NOT NULL,
    [GradoAlimentaria] nvarchar(100) NOT NULL,
    [TieneActa] bit NOT NULL,
    [Pea] bit NOT NULL,
    [Ingreso] float NOT NULL,
    [LineaBienestar] int NOT NULL,
    [NivelPobreza] nvarchar(100) NOT NULL,
    [SeguridadPublica] smallint NOT NULL,
    [SeguridadParque] smallint NOT NULL,
    [ConfianzaLiderez] smallint NOT NULL,
    [ConfianzaInstituciones] smallint NOT NULL,
    [Movilidad] smallint NOT NULL,
    [RedesSociales] smallint NOT NULL,
    [Tejido] smallint NOT NULL,
    [Satisfaccion] smallint NOT NULL,
    [PropiedadVivienda] bit NOT NULL,
    [CreatedAt] datetime2 NULL,
    [UpdatedAt] datetime2 NULL,
    [VersionId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AlgoritmoResultados] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AlgoritmoResultados_AlgoritmoVersiones_VersionId] FOREIGN KEY ([VersionId]) REFERENCES [AlgoritmoVersiones] ([Id])
);

GO

CREATE INDEX [IX_AlgoritmoResultados_VersionId] ON [AlgoritmoResultados] ([VersionId]);

GO

CREATE TABLE [AlgoritmoIntegranteResultados] (
    [Id] nvarchar(450) NOT NULL,
    [Educativa] bit NOT NULL,
    [Analfabetismo] bit NOT NULL,
    [Inasistencia] bit NOT NULL,
    [PrimariaIncompleta] bit NOT NULL,
    [SecundariaIncompleta] bit NOT NULL,
    [ServicioSalud] bit NOT NULL,
    [SeguridadSocial] bit NOT NULL,
    [Vivienda] bit NOT NULL,
    [Piso] bit NOT NULL,
    [Techo] bit NOT NULL,
    [Muro] bit NOT NULL,
    [Hacinamiento] bit NOT NULL,
    [Servicios] bit NOT NULL,
    [Agua] bit NOT NULL,
    [Drenaje] bit NOT NULL,
    [Electricidad] bit NOT NULL,
    [Combustible] bit NOT NULL,
    [Alimentaria] bit NOT NULL,
    [GradoAlimentaria] nvarchar(100) NOT NULL,
    [TieneActa] bit NOT NULL,
    [Pea] bit NOT NULL,
    [Ingreso] float NOT NULL,
    [LineaBienestar] int NOT NULL,
    [NivelPobreza] nvarchar(100) NOT NULL,
    [ParentescoId] nvarchar(450) NOT NULL,
    [SexoId] nvarchar(450) NOT NULL,
    [Discapacidad] nvarchar(max) NOT NULL,
    [GradoEducativa] int NOT NULL,
    [Edad] int NOT NULL,
    [NumIntegrante] int NOT NULL,
    [CreatedAt] datetime2 NULL,
    [UpdatedAt] datetime2 NULL,
    [ResultadoId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AlgoritmoIntegranteResultados] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AlgoritmoIntegranteResultados_AlgoritmoResultados_ResultadoId] FOREIGN KEY ([ResultadoId]) REFERENCES [AlgoritmoResultados] ([Id])
);

GO

CREATE INDEX [IX_AlgoritmoIntegranteResultados_ResultadoId] ON [AlgoritmoIntegranteResultados] ([ResultadoId]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20201206143914_CreateAlgoritmo', N'2.1.0-rtm-30799');

GO

