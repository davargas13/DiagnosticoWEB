ALTER TABLE [Domicilio] DROP CONSTRAINT [FK_Domicilio_Localidad];

GO

ALTER TABLE [Domicilio] DROP CONSTRAINT [FK_Caminos_Domicilio_DomicilioId];

GO

ALTER TABLE [Domicilio] DROP CONSTRAINT [FK_Manzanas_Domicilio_DomicilioId];

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20201221172933_DeleteDomicilioForeings', N'2.1.0-rtm-30799');

GO

