CREATE TABLE [LineasBienestar] (
    [Id] int NOT NULL IDENTITY(1,1),
    [MinimaRural] real NOT NULL DEFAULT 0,
    [Rural] real NOT NULL DEFAULT 0,
    [MinimaUrbana] real NOT NULL DEFAULT 0,
    [Urbana] real NOT NULL DEFAULT 0,
    [CreatedAt] datetime2 NULL,
    [UpdatedAt] datetime2 NULL,
    CONSTRAINT [PK_LineasBienestar] PRIMARY KEY ([Id])
);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20210626234151_CreateLineasBienestar', N'2.1.0-rtm-30799');

GO

