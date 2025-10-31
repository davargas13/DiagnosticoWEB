ALTER TABLE [Beneficiarios] ADD [HijoId] nvarchar(450) NULL;

GO

ALTER TABLE [Beneficiarios] ADD CONSTRAINT [FK_Beneficiarios_Hijo] FOREIGN KEY ([HijoId]) REFERENCES [Beneficiarios] ([Id]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20191016154239_AddHijoId', N'2.1.11-servicing-32099');

GO

ALTER TABLE [Beneficiarios] DROP CONSTRAINT [FK_Beneficiario_Discapacidad];

GO

ALTER TABLE [Beneficiarios] ADD CONSTRAINT [FK_Beneficiario_Discapacidad] FOREIGN KEY ([DiscapacidadId]) REFERENCES [Discapacidades] ([Id]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20191017164623_CorrectFK', N'2.1.11-servicing-32099');

GO

CREATE TABLE [Unidades] (
    [Id] nvarchar(450) NOT NULL,
    [Nombre] nvarchar(256) NULL,
    [CreatedAt] datetime2 NULL,
    [UpdatedAt] datetime2 NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_Unidades] PRIMARY KEY ([Id])
);

GO

ALTER TABLE [Vertientes] ADD [UnidadId] nvarchar(450) NULL;

GO

ALTER TABLE [Vertientes] ADD CONSTRAINT [FK_Vertiente_Unidad] FOREIGN KEY ([UnidadId]) REFERENCES [Unidades] ([Id]);

GO

ALTER TABLE [Vertientes] ADD [Costo] float NULL;

GO

ALTER TABLE [Solicitudes] ADD [Cantidad] float NULL;

GO

ALTER TABLE [Solicitudes] ADD [Costo] float NULL;

GO

ALTER TABLE [Solicitudes] ADD [Economico] float NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20191017171808_add_datos_vertientes', N'2.1.11-servicing-32099');

GO

ALTER TABLE [ProgramasSociales] ADD [Proyecto] nvarchar(450) NULL;

GO

ALTER TABLE [Vertientes] ADD [Ciclo] nvarchar(450) NULL;

GO

ALTER TABLE [Vertientes] ADD [Ejercicio] nvarchar(450) NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20191024205419_AddEjercicioPrograma', N'2.1.11-servicing-32099');

GO

