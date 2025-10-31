ALTER TABLE [AplicacionCarencias] ADD [SexoId] nvarchar(max) NULL;

GO

ALTER TABLE [AplicacionCarencias] ADD [Discapacidad] bit NOT NULL DEFAULT 0;

GO

ALTER TABLE [AplicacionCarencias] ADD [GradoEducativa] smallint NULL;

GO

ALTER TABLE [Aplicaciones] ADD [Discapacidad] bit NOT NULL DEFAULT 0;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20201021230924_AddSexoIdAplicaciones', N'2.1.0-rtm-30799');

GO

