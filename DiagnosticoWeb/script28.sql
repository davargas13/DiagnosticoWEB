ALTER TABLE [Domicilio] ADD [VersionAplicacion] nvarchar(10) NULL;

GO

ALTER TABLE [Beneficiarios] ADD [VersionAplicacion] nvarchar(10) NULL;

GO

ALTER TABLE [Aplicaciones] ADD [VersionAplicacion] nvarchar(10) NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20210323223643_AddVersionEncuesta', N'2.1.0-rtm-30799');

GO

