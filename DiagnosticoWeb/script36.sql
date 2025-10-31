ALTER TABLE [AlgoritmoResultados] ADD [BeneficiarioId] nvarchar(450) NOT NULL;

GO

ALTER TABLE [AlgoritmoResultados] ADD CONSTRAINT [FK_AlgoritmoResultados_Beneficiario] FOREIGN KEY ([BeneficiarioId]) REFERENCES [Beneficiarios] ([Id]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20210630223354_AddBeneficiarioResultado', N'2.1.0-rtm-30799');

GO

