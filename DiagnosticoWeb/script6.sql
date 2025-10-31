DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AplicacionPreguntas]') AND [c].[name] = N'RespuestaIteracion');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [AplicacionPreguntas] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [AplicacionPreguntas] ALTER COLUMN [RespuestaIteracion] int NOT NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200811154934_ChangeRespuestaIteracion', N'2.1.0-rtm-30799');

GO

