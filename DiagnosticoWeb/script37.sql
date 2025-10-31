ALTER TABLE [Trabajadores] ADD [Tipo] tinyint NOT NULL default 0;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20210810214229_AddTipoTrabajador', N'2.1.0-rtm-30799');

GO