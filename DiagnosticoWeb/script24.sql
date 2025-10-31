ALTER TABLE [Domicilio] DROP CONSTRAINT [FK_Manzanas_Domicilio_DomicilioId];

GO

ALTER TABLE [Domicilio] DROP CONSTRAINT [FK_Carreteras_Domicilio_DomicilioId];

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20210126231246_DeleteForeignDomicilioManzana', N'2.1.14-servicing-32113');

GO

