ALTER TABLE [Beneficiarios] ADD [DeviceId] nvarchar(100) NULL;

GO

ALTER TABLE [Domicilio] ADD [DeviceId] nvarchar(100) NULL;

GO

ALTER TABLE [Aplicaciones] ADD [DeviceId] nvarchar(100) NULL;

GO

ALTER TABLE [Incidencias] ADD [DeviceId] nvarchar(100) NULL;

GO

ALTER TABLE [AplicacionPreguntas] ADD [NumIntegrante] int NOT NULL DEFAULT 0;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20210228230730_AddDeviceId', N'2.1.14-servicing-32113');

GO

