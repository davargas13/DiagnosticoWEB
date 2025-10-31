CREATE TABLE [TipoIncidencias] (
    [Id] nvarchar(450) NOT NULL,
    [Nombre] nvarchar(100) NOT NULL,
    [CreatedAt] datetime2 NULL,
    [UpdatedAt] datetime2 NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_TipoIncidencias] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [Incidencias] (
    [Id] nvarchar(450) NOT NULL,
    [Observaciones] nvarchar(1000) NOT NULL,
    [CreatedAt] datetime2 NULL,
    [UpdatedAt] datetime2 NULL,
    [DeletedAt] datetime2 NULL,
    [TrabajadorId] nvarchar(450) NOT NULL,
    [TipoIncidenciaId] nvarchar(450) NOT NULL,
    [BeneficiarioId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_Incidencias] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Incidencias_TipoIncidencias_TipoIncidenciaId] FOREIGN KEY ([TipoIncidenciaId]) REFERENCES [TipoIncidencias] ([Id]),
    CONSTRAINT [FK_Incidencias_Trabajadores_TrabajadorId] FOREIGN KEY ([TrabajadorId]) REFERENCES [Trabajadores] ([Id]),
    CONSTRAINT [FK_Incidencias_Beneficiarios_BeneficiarioId] FOREIGN KEY ([BeneficiarioId]) REFERENCES [Beneficiarios] ([Id])
);

GO

CREATE INDEX [IX_Incidencias_TrabajadorId] ON [Incidencias] ([TrabajadorId]);

GO

CREATE INDEX [IX_Incidencias_TipoIncidenciaId] ON [Incidencias] ([TipoIncidenciaId]);

GO

CREATE INDEX [IX_Incidencias_BeneficiarioId] ON [Incidencias] ([BeneficiarioId]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20201210175207_CreateIncidencias', N'2.1.0-rtm-30799');

GO

ALTER TABLE [Carencias] ADD [PadreId] nvarchar(450) NULL;

GO

ALTER TABLE [Carencias] ADD CONSTRAINT [FK_Carencias_Carencias_PadreId] FOREIGN KEY ([PadreId]) REFERENCES [Carencias] ([Id]);

GO

CREATE INDEX [IX_Carencias_PadreId] ON [Carencias] ([PadreId]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20201213232221_AddTemaCategoria', N'2.1.0-rtm-30799');

GO

