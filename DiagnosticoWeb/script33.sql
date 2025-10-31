ALTER TABLE [Preguntas] ADD [SeleccionarRespuestas] bit NOT NULL DEFAULT 0;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20210520230318_AddSeleccionarTodasPreguntas', N'2.1.14-servicing-32113');

GO

