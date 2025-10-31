ALTER TABLE [EncuestaVersiones] ADD [Numero] int NOT NULL DEFAULT 0;

GO

ALTER TABLE [Preguntas] ADD [Activa] bit NOT NULL DEFAULT 1;

GO

ALTER TABLE [Preguntas] ADD [Obligatoria] bit NOT NULL DEFAULT 1;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200910173555_AddNumeroEncuestaVersion', N'2.1.0-rtm-30799');

GO

ALTER TABLE [Preguntas] ADD [Excluir] bit NOT NULL DEFAULT 0;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200924141503_AddExcluirPregunta', N'2.1.0-rtm-30799');

GO

