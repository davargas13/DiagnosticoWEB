ALTER TABLE [Beneficiarios] ADD [NumIntegrante] int NOT NULL DEFAULT 0;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200821021600_AddNumIntegranteBeneficiarios', N'2.1.0-rtm-30799');

GO

