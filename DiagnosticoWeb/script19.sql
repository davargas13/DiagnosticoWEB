DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Domicilio]') AND [c].[name] = N'Latitud');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Domicilio] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [Domicilio] ALTER COLUMN [Latitud] nvarchar(15) NULL;

GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Domicilio]') AND [c].[name] = N'Longitud');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Domicilio] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [Domicilio] ALTER COLUMN [Longitud] nvarchar(15) NULL;

GO

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Domicilio]') AND [c].[name] = N'LatitudAtm');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [Domicilio] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [Domicilio] ALTER COLUMN [LatitudAtm] nvarchar(15) NULL;

GO

DECLARE @var3 sysname;
SELECT @var3 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Domicilio]') AND [c].[name] = N'LongitudAtm');
IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [Domicilio] DROP CONSTRAINT [' + @var3 + '];');
ALTER TABLE [Domicilio] ALTER COLUMN [LongitudAtm] nvarchar(15) NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20201208230140_ChaneCoordendasDomciilio', N'2.1.0-rtm-30799');

GO

