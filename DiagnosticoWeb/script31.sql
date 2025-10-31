ALTER TABLE [Aplicaciones] ADD [NumeroEducativas] int NOT NULL DEFAULT 0;

GO

ALTER TABLE [Aplicaciones] ADD [NumeroSalud] int NOT NULL DEFAULT 0;

GO

ALTER TABLE [Aplicaciones] ADD [NumeroSocial] int NOT NULL DEFAULT 0;

GO

ALTER TABLE [Aplicaciones] ADD [NumeroCarencias] int NOT NULL DEFAULT 0;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20210425210737_AddNumCarencias', N'2.1.0-rtm-30799');

GO

