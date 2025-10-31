ALTER TABLE [Aplicaciones] ADD [NumeroIntegrantes] int NOT NULL DEFAULT 1;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20210421224007_AddNumIntegrantesAplicaciones', N'2.1.14-servicing-32113');

GO

