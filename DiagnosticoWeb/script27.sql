ALTER TABLE [Aplicaciones] ADD [Tiempo] int NULL DEFAULT 0;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20210304233359_AddDuracionAplicacion', N'2.1.0-rtm-30799');

GO

