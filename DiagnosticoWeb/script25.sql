
DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Domicilio]') AND [c].[name] = N'Telefono');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Domicilio] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [Domicilio] ALTER COLUMN [Telefono] nvarchar(max) NOT NULL;

GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Domicilio]') AND [c].[name] = N'TelefonoCasa');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Domicilio] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [Domicilio] ALTER COLUMN [TelefonoCasa] nvarchar(max) NOT NULL;

GO

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Domicilio]') AND [c].[name] = N'Latitud');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [Domicilio] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [Domicilio] ALTER COLUMN [Latitud] nvarchar(max) NOT NULL;

GO

DECLARE @var3 sysname;
SELECT @var3 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Domicilio]') AND [c].[name] = N'Longitud');
IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [Domicilio] DROP CONSTRAINT [' + @var3 + '];');
ALTER TABLE [Domicilio] ALTER COLUMN [Longitud] nvarchar(max) NOT NULL;

GO

DECLARE @var4 sysname;
SELECT @var4 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Domicilio]') AND [c].[name] = N'LatitudAtm');
IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [Domicilio] DROP CONSTRAINT [' + @var4 + '];');
ALTER TABLE [Domicilio] ALTER COLUMN [LatitudAtm] nvarchar(max) NOT NULL;

GO

DECLARE @var5 sysname;
SELECT @var5 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Domicilio]') AND [c].[name] = N'LongitudAtm');
IF @var5 IS NOT NULL EXEC(N'ALTER TABLE [Domicilio] DROP CONSTRAINT [' + @var5 + '];');
ALTER TABLE [Domicilio] ALTER COLUMN [LongitudAtm] nvarchar(max) NOT NULL;

GO

DECLARE @var6 sysname;
SELECT @var6 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Domicilio]') AND [c].[name] = N'CodigoPostal');
IF @var6 IS NOT NULL EXEC(N'ALTER TABLE [Domicilio] DROP CONSTRAINT [' + @var6 + '];');
ALTER TABLE [Domicilio] ALTER COLUMN [CodigoPostal] nvarchar(max) NOT NULL;

GO

DECLARE @var7 sysname;
SELECT @var7 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Domicilio]') AND [c].[name] = N'CadenaOCR');
IF @var7 IS NOT NULL EXEC(N'ALTER TABLE [Domicilio] DROP CONSTRAINT [' + @var7 + '];');
ALTER TABLE [Domicilio] ALTER COLUMN [CadenaOCR] nvarchar(max) NOT NULL;

GO

DECLARE @var8 sysname;
SELECT @var8 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Domicilio]') AND [c].[name] = N'Porcentaje');
IF @var8 IS NOT NULL EXEC(N'ALTER TABLE [Domicilio] DROP CONSTRAINT [' + @var8 + '];');
ALTER TABLE [Domicilio] ALTER COLUMN [Porcentaje] nvarchar(max) NOT NULL;

GO

DECLARE @var9 sysname;
SELECT @var9 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Domicilio]') AND [c].[name] = N'TipoZap');
IF @var9 IS NOT NULL EXEC(N'ALTER TABLE [Domicilio] DROP CONSTRAINT [' + @var9 + '];');
ALTER TABLE [Domicilio] ALTER COLUMN [TipoZap] nvarchar(max) NOT NULL;

GO

DECLARE @var10 sysname;
SELECT @var10 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Domicilio]') AND [c].[name] = N'CodigoPostalCalculado');
IF @var10 IS NOT NULL EXEC(N'ALTER TABLE [Domicilio] DROP CONSTRAINT [' + @var10 + '];');
ALTER TABLE [Domicilio] ALTER COLUMN [CodigoPostalCalculado] nvarchar(max) NOT NULL;

GO

DECLARE @var11 sysname;
SELECT @var11 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Domicilio]') AND [c].[name] = N'indicedesarrollohumano');
IF @var11 IS NOT NULL EXEC(N'ALTER TABLE [Domicilio] DROP CONSTRAINT [' + @var11 + '];');
ALTER TABLE [Domicilio] ALTER COLUMN [indicedesarrollohumano] nvarchar(max) NOT NULL;

GO

DECLARE @var12 sysname;
SELECT @var12 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Domicilio]') AND [c].[name] = N'marginacionlocalidad');
IF @var12 IS NOT NULL EXEC(N'ALTER TABLE [Domicilio] DROP CONSTRAINT [' + @var12 + '];');
ALTER TABLE [Domicilio] ALTER COLUMN [marginacionlocalidad] nvarchar(max) NOT NULL;

GO

DECLARE @var13 sysname;
SELECT @var13 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Domicilio]') AND [c].[name] = N'marginacionmunicipio');
IF @var13 IS NOT NULL EXEC(N'ALTER TABLE [Domicilio] DROP CONSTRAINT [' + @var13 + '];');
ALTER TABLE [Domicilio] ALTER COLUMN [marginacionmunicipio] nvarchar(max) NOT NULL;

GO

DECLARE @var14 sysname;
SELECT @var14 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Domicilio]') AND [c].[name] = N'marginacionlocalidad');
IF @var14 IS NOT NULL EXEC(N'ALTER TABLE [Domicilio] DROP CONSTRAINT [' + @var14 + '];');
ALTER TABLE [Domicilio] ALTER COLUMN [marginacionlocalidad] nvarchar(max) NOT NULL;

GO

DECLARE @var15 sysname;
SELECT @var15 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Domicilio]') AND [c].[name] = N'marginacionageb');
IF @var15 IS NOT NULL EXEC(N'ALTER TABLE [Domicilio] DROP CONSTRAINT [' + @var15 + '];');
ALTER TABLE [Domicilio] ALTER COLUMN [marginacionageb] nvarchar(max) NOT NULL;

GO

DECLARE @var16 sysname;
SELECT @var16 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Domicilio]') AND [c].[name] = N'clavemunicipiocalculado');
IF @var16 IS NOT NULL EXEC(N'ALTER TABLE [Domicilio] DROP CONSTRAINT [' + @var16 + '];');
ALTER TABLE [Domicilio] ALTER COLUMN [clavemunicipiocalculado] nvarchar(max) NOT NULL;

GO

DECLARE @var17 sysname;
SELECT @var17 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Domicilio]') AND [c].[name] = N'clavelocalidadcalculada');
IF @var17 IS NOT NULL EXEC(N'ALTER TABLE [Domicilio] DROP CONSTRAINT [' + @var17 + '];');
ALTER TABLE [Domicilio] ALTER COLUMN [clavelocalidadcalculada] nvarchar(max) NOT NULL;

GO

DECLARE @var18 sysname;
SELECT @var18 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Domicilio]') AND [c].[name] = N'clavezonaimpulsocalculada');
IF @var18 IS NOT NULL EXEC(N'ALTER TABLE [Domicilio] DROP CONSTRAINT [' + @var18 + '];');
ALTER TABLE [Domicilio] ALTER COLUMN [clavezonaimpulsocalculada] nvarchar(max) NOT NULL;

GO

ALTER TABLE [Beneficiarios] ADD [Prueba] bit NOT NULL DEFAULT 0;

GO

ALTER TABLE [Aplicaciones] ADD [Prueba] bit NOT NULL DEFAULT 0;

GO

ALTER TABLE [Domicilio] ADD [Prueba] bit NOT NULL DEFAULT 0;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20210212155105_ChangeDomicilioSize', N'2.1.14-servicing-32113');

GO

