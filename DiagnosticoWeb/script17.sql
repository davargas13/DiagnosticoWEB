ALTER TABLE [Domicilio] ADD [Indice] real NULL;

GO

ALTER TABLE [Domicilio] ADD [IndiceDesarrolloHumano] nvarchar(50) NULL;

GO

ALTER TABLE [Domicilio] ADD [MarginacionMunicipio] nvarchar(50) NULL;

GO

ALTER TABLE [Domicilio] ADD [MarginacionLocalidad] nvarchar(50) NULL;

GO

ALTER TABLE [Domicilio] ADD [MarginacionAgeb] nvarchar(50) NULL;

GO

ALTER TABLE [Domicilio] ADD [ClaveMunicipioCalculado] nvarchar(50) NULL;

GO

ALTER TABLE [Domicilio] ADD [ClaveLocalidadCalculada] nvarchar(50) NULL;

GO

ALTER TABLE [Domicilio] ADD [ClaveMunicipioCisCalculado] nvarchar(50) NULL;

GO

ALTER TABLE [Domicilio] ADD [DomicilioCisCalculado] nvarchar(200) NULL;

GO

ALTER TABLE [Domicilio] ADD [ZonaImpulsoCalculada] nvarchar(200) NULL;

GO

ALTER TABLE [Domicilio] ADD [ClaveZonaImpulsoCalculada] nvarchar(50) NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20201127144617_AddIndicesDomicilio', N'2.1.0-rtm-30799');

GO

