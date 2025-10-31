CREATE TABLE [BeneficiarioDiscapacidades] (
    [Id] nvarchar(450) NOT NULL,
    [BeneficiarioId] nvarchar(450) NULL,
    [DiscapacidadId] nvarchar(450) NULL,
    [GradoId] nvarchar(450) NULL,
    [CausaId] nvarchar(450) NULL,
    [CreatedAt] datetime2 NULL,
    [UpdatedAt] datetime2 NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_BeneficiarioDiscapacidades] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_BeneficiarioDiscapacidades_Respuestas_DiscapacidadId] FOREIGN KEY ([DiscapacidadId]) REFERENCES [Respuestas] ([Id]),
    CONSTRAINT [FK_BeneficiarioDiscapacidades_RespuestaGrado_GradoId] FOREIGN KEY ([GradoId]) REFERENCES [RespuestaGrado] ([Id]),
    CONSTRAINT [FK_BeneficiarioDiscapacidades_CausaDiscapacidad_CausaId] FOREIGN KEY ([CausaId]) REFERENCES [CausaDiscapacidad] ([Id]),
    CONSTRAINT [FK_BeneficiarioDiscapacidades_Beneficiarios_BeneficiarioId] FOREIGN KEY ([BeneficiarioId]) REFERENCES [Beneficiarios] ([Id])
);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20201007221329_AddBeneficiariosDiscapacidades', N'2.1.0-rtm-30799');

GO

ALTER TABLE [Preguntas] ADD [Maximo] int NOT NULL DEFAULT 0;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20201008180129_AddPreguntasMaximo', N'2.1.0-rtm-30799');

GO

ALTER TABLE [Aplicaciones] ADD [SeguridadPublica] smallint NOT NULL DEFAULT 0;

GO

ALTER TABLE [Aplicaciones] ADD [SeguridadParque] smallint NOT NULL DEFAULT 0;

GO

ALTER TABLE [Aplicaciones] ADD [ConfianzaLiderez] smallint NOT NULL DEFAULT 0;

GO

ALTER TABLE [Aplicaciones] ADD [ConfianzaInstituciones] smallint NOT NULL DEFAULT 0;

GO

ALTER TABLE [Aplicaciones] ADD [Movilidad] smallint NOT NULL DEFAULT 0;

GO

ALTER TABLE [Aplicaciones] ADD [RedesSociales] smallint NOT NULL DEFAULT 0;

GO

ALTER TABLE [Aplicaciones] ADD [Tejido] smallint NOT NULL DEFAULT 0;

GO

ALTER TABLE [Aplicaciones] ADD [Satisfaccion] smallint NOT NULL DEFAULT 0;

GO

ALTER TABLE [Aplicaciones] ADD [PropiedadVivienda] bit NOT NULL DEFAULT 0;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20201009183918_AddIndicadoresIntegrantes', N'2.1.0-rtm-30799');

GO

