ALTER TABLE [AlgoritmoVersiones] ADD [DeletedAt] datetime2 NULL;

GO

ALTER TABLE [AlgoritmoResultados] ADD [DeletedAt] datetime2 NULL;

GO

ALTER TABLE [AlgoritmoIntegranteResultados] ADD CONSTRAINT [FK_AlgoritmoIntegranteResultados_Parentescos_ParentescoId] FOREIGN KEY ([ParentescoId]) REFERENCES [Parentescos] ([Id]);

GO

ALTER TABLE [AlgoritmoIntegranteResultados] ADD CONSTRAINT [FK_AlgoritmoIntegranteResultados_Sexos_SexoId] FOREIGN KEY ([SexoId]) REFERENCES [Sexos] ([Id]);

GO

ALTER TABLE [AplicacionCarencias] ADD CONSTRAINT [FK_AplicacionCarencias_Beneficiarios_BeneficiarioId] FOREIGN KEY ([BeneficiarioId]) REFERENCES [Beneficiarios] ([Id]);

GO

ALTER TABLE [AplicacionCarencias] ADD CONSTRAINT [FK_AplicacionCarencias_Parentescos_ParentescoId] FOREIGN KEY ([ParentescoId]) REFERENCES [Parentescos] ([Id]);

GO

ALTER TABLE [AplicacionCarencias] ADD CONSTRAINT [FK_AplicacionCarencias_Sexos_SexoId] FOREIGN KEY ([SexoId]) REFERENCES [Sexos] ([Id]);

GO

ALTER TABLE [BeneficiarioHistorico] ADD CONSTRAINT [FK_BeneficiarioHistorico_Domicilio_DomicilioId] FOREIGN KEY ([DomicilioId]) REFERENCES [Domicilio] ([Id]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20210926220153_Ajustes', N'2.1.0-rtm-30799');

GO

