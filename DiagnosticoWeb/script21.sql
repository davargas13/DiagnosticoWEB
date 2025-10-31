DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Archivos]') AND [c].[name] = N'Base64');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Archivos] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [Archivos] ALTER COLUMN [Base64] text NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20201215225757_ChangeBase64Archivo', N'2.1.0-rtm-30799');

GO

ALTER TABLE [Aplicaciones] ADD [VersionAlgoritmo] int NOT NULL DEFAULT 1;

GO

ALTER TABLE [AplicacionCarencias] ADD [VersionAlgoritmo] int NOT NULL DEFAULT 1;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20201216115214_AddVersionAlgoritmoAplicaciones', N'2.1.0-rtm-30799');

GO

