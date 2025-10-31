ALTER TABLE [Domicilio] ADD [AutorizaFotos] bit NOT NULL DEFAULT 0;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20210512152015_AddAutorizaFirmasDomicilio', N'2.1.0-rtm-30799');

GO

