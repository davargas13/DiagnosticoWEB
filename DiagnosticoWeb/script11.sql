ALTER TABLE [Aplicaciones] ADD [TieneActa] bit NOT NULL DEFAULT 0;

GO

ALTER TABLE [AplicacionCarencias] ADD [TieneActa] int NOT NULL DEFAULT 0;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200928221006_AddTieneActaAplicaciones', N'2.1.0-rtm-30799');

GO

