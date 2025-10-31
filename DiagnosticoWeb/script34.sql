CREATE TABLE [DomicilioHistorico] (
    [Id] nvarchar(450) NOT NULL,
    [DomicilioId] nvarchar(450) NOT NULL,
    [AgebId] nvarchar(450) NULL,
    [Telefono] nvarchar(20) NULL,
    [TelefonoCasa] nvarchar(20) NULL,
    [Email] nvarchar(200) NULL,
    [DomicilioN] nvarchar(100) NOT NULL,
    [EntreCalle1] nvarchar(100) NOT NULL,
    [EntreCalle2] nvarchar(10) NOT NULL,
    [Latitud] nvarchar(100) NOT NULL,
    [Longitud] nvarchar(100) NULL,
    [CodigoPostal] nvarchar(450) NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NOT NULL,
    [DeletedAt] datetime2 NULL,
    [MunicipioId] nvarchar(450) NOT NULL,
    [LocalidadId] nvarchar(450) NOT NULL,
    [ZonaImpulsoId] nvarchar(450) NULL,
    [ManzanaId] nvarchar(450) NULL,
    [CarreteraId] nvarchar(450) NOT NULL,
    [CaminoId] nvarchar(450) NOT NULL,
    [NombreAsentamiento] nvarchar(450) NOT NULL,
    [TipoAsentamientoId] nvarchar(450) NOT NULL,
    [CallePosterior] nvarchar(450) NULL,
    [NumExterior] nvarchar(100) NOT NULL,
    [NumInterior] nvarchar(100) NULL,
    [ColoniaId] nvarchar(450) NULL,
    [CalleId] nvarchar(450) NULL,
    CONSTRAINT [PK_DomicilioHistorico] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_DomicilioHistorico_Domicilio] FOREIGN KEY ([DomicilioId]) REFERENCES [Domicilio] ([Id]),
    CONSTRAINT [FK_DomicilioHistorico_Agebs] FOREIGN KEY ([AgebId]) REFERENCES [Agebs] ([Id]),
    CONSTRAINT [FK_DomicilioHistorico_Municipios] FOREIGN KEY ([MunicipioId]) REFERENCES [Municipios] ([Id]),
    CONSTRAINT [FK_DomicilioHistorico_Localidades] FOREIGN KEY ([LocalidadId]) REFERENCES [Localidades] ([Id]),
    CONSTRAINT [FK_DomicilioHistorico_ZonasImpulso] FOREIGN KEY ([ZonaImpulsoId]) REFERENCES [ZonasImpulso] ([Id]),
    CONSTRAINT [FK_DomicilioHistorico_Manzanas] FOREIGN KEY ([ManzanaId]) REFERENCES [Manzanas] ([Id]),
    CONSTRAINT [FK_DomicilioHistorico_Carreteras] FOREIGN KEY ([CarreteraId]) REFERENCES [Carreteras] ([Id]),
    CONSTRAINT [FK_DomicilioHistorico_Caminos] FOREIGN KEY ([CaminoId]) REFERENCES [Caminos] ([Id]),
    CONSTRAINT [FK_DomicilioHistorico_TipoAsentamientos] FOREIGN KEY ([TipoAsentamientoId]) REFERENCES [TipoAsentamientos] ([Id]),
    CONSTRAINT [FK_DomicilioHistorico_Colonias] FOREIGN KEY ([ColoniaId]) REFERENCES [Colonias] ([Id]),
    CONSTRAINT [FK_DomicilioHistorico_Calles] FOREIGN KEY ([CalleId]) REFERENCES [Calles] ([Id])
);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20210608175847_CreateDomicilioHistorico', N'2.1.0-rtm-30799');

GO

ALTER TABLE [Aplicaciones] ADD [PreocupacionCovid] int NOT NULL DEFAULT 0;

GO

ALTER TABLE [Aplicaciones] ADD [PerdidaEmpleo] bit NOT NULL DEFAULT 0;

GO

ALTER TABLE [AplicacionCarencias] ADD [PerdidaEmpleo] bit NOT NULL DEFAULT 0;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20210609233612_AddCovidIntegrantes', N'2.1.0-rtm-30799');

GO

