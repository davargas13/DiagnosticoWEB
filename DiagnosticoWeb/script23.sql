DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Domicilio]') AND [c].[name] = N'NumInterior');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Domicilio] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [Domicilio] ALTER COLUMN [NumInterior] nvarchar(50) NOT NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20210125225616_ChangeSizeDomicilioInterior', N'2.1.14-servicing-32113');

GO

