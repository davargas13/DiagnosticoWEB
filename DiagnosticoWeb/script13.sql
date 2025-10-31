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

