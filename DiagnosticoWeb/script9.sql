CREATE TABLE [AplicacionCarencias] (
    [Id] nvarchar(450) NOT NULL,
    [Educativa] bit NOT NULL,
    [Analbatesimo] bit NOT NULL,
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
    [Pea] bit NOT NULL,
    [ParentescoId] nvarchar(max) NOT NULL,
    [Edad] int NOT NULL,
    [Ingreso] float NOT NULL,
    [LineaBienestar] int NOT NULL,
    [NumIntegrante] int NOT NULL,
    [NivelPobreza] nvarchar(100) NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NOT NULL,
    [DeletedAt] datetime2 NULL,
    [AplicacionId] nvarchar(450) NOT NULL,
    [BeneficiarioId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AplicacionCarencias] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AplicacionCarencias_Aplicacion] FOREIGN KEY ([AplicacionId]) REFERENCES [Aplicaciones] ([Id]),
    CONSTRAINT [FK_AplicacionCarencias_Beneficiario] FOREIGN KEY ([BeneficiarioId]) REFERENCES [Beneficiarios] ([Id])
);

GO

ALTER TABLE [Aplicaciones] ADD [Educativa] bit NOT NULL DEFAULT 0;

GO

ALTER TABLE [Aplicaciones] ADD [Analbatesimo] bit NOT NULL DEFAULT 0;

GO

ALTER TABLE [Aplicaciones] ADD [Inasistencia] bit NOT NULL DEFAULT 0;

GO

ALTER TABLE [Aplicaciones] ADD [PrimariaIncompleta] bit NOT NULL DEFAULT 0;

GO

ALTER TABLE [Aplicaciones] ADD [SecundariaIncompleta] bit NOT NULL DEFAULT 0;

GO

ALTER TABLE [Aplicaciones] ADD [ServicioSalud] bit NOT NULL DEFAULT 0;

GO

ALTER TABLE [Aplicaciones] ADD [SeguridadSocial] bit NOT NULL DEFAULT 0;

GO

ALTER TABLE [Aplicaciones] ADD [Vivienda] bit NOT NULL DEFAULT 0;

GO

ALTER TABLE [Aplicaciones] ADD [Piso] bit NOT NULL DEFAULT 0;

GO

ALTER TABLE [Aplicaciones] ADD [Techo] bit NOT NULL DEFAULT 0;

GO

ALTER TABLE [Aplicaciones] ADD [Muro] bit NOT NULL DEFAULT 0;

GO

ALTER TABLE [Aplicaciones] ADD [Hacinamiento] bit NOT NULL DEFAULT 0;

GO

ALTER TABLE [Aplicaciones] ADD [Servicios] bit NOT NULL DEFAULT 0;

GO

ALTER TABLE [Aplicaciones] ADD [Agua] bit NOT NULL DEFAULT 0;

GO

ALTER TABLE [Aplicaciones] ADD [Drenaje] bit NOT NULL DEFAULT 0;

GO

ALTER TABLE [Aplicaciones] ADD [Electricidad] bit NOT NULL DEFAULT 0;

GO

ALTER TABLE [Aplicaciones] ADD [Combustible] bit NOT NULL DEFAULT 0;

GO

ALTER TABLE [Aplicaciones] ADD [Alimentaria] bit NOT NULL DEFAULT 0;

GO

ALTER TABLE [Aplicaciones] ADD [GradoAlimentaria] nvarchar(100) NULL;

GO

ALTER TABLE [Aplicaciones] ADD [Pea] bit NOT NULL DEFAULT 0;

GO

ALTER TABLE [Aplicaciones] ADD [Ingreso] float NOT NULL DEFAULT 0;

GO

ALTER TABLE [Aplicaciones] ADD [LineaBienestar] int NOT NULL DEFAULT 0;

GO

ALTER TABLE [Aplicaciones] ADD [NivelPobreza] nvarchar(100) NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200902031136_AddAplicacionCarencias', N'2.1.0-rtm-30799');

GO

ALTER TABLE [Aplicaciones] ADD [Resultado] nvarchar(max) NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200905174108_AddResultadoCarencias', N'2.1.0-rtm-30799');

GO

