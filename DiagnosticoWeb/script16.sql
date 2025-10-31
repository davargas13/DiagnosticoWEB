ALTER TABLE [Domicilio] ADD [NumExterior] nvarchar(20) NULL;

GO

ALTER TABLE [Domicilio] ADD [NumInterior] nvarchar(20) NULL;

GO

ALTER TABLE [Domicilio] ADD [NumFamilia] int NOT NULL DEFAULT 1;

GO

ALTER TABLE [Domicilio] ADD [NumFamiliasRegistradas] int NOT NULL DEFAULT 1;

GO

ALTER TABLE [Beneficiarios] ADD [NumFamilia] int NOT NULL DEFAULT 1;

GO

ALTER TABLE [Beneficiarios] ADD [EstatusVisita] int NULL;

GO

ALTER TABLE [BeneficiarioHistorico] ADD [EstatusVisita] int NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20201106220740_AddNumerosDomicilio', N'2.1.0-rtm-30799');

GO

