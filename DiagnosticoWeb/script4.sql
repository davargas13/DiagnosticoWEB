CREATE TABLE [Grados] (
    [Id] nvarchar(450) NOT NULL,
    [Nombre] nvarchar(256) NULL,
    [CreatedAt] datetime2 NULL,
    [UpdatedAt] datetime2 NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_GradosDiscapacidad] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [DiscapacidadGrado] (
    [Id] nvarchar(450) NOT NULL,
    [DiscapacidadId] nvarchar(450) NULL,
    [GradoId] nvarchar(450) NULL,
    [Grado] nvarchar(10) NULL,
    [CreatedAt] datetime2 NULL,
    [UpdatedAt] datetime2 NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_DiscapacidadGrado] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_DiscapacidadGrado_Discapacidades] FOREIGN KEY ([DiscapacidadId]) REFERENCES [Discapacidades] ([Id]),
    CONSTRAINT [FK_DiscapacidadGrado_Grado] FOREIGN KEY ([GradoId]) REFERENCES [Grados] ([Id])
);

GO

CREATE TABLE [CausaDiscapacidad] (
    [Id] nvarchar(450) NOT NULL,
    [Nombre] nvarchar(256) NULL,
    [CreatedAt] datetime2 NULL,
    [UpdatedAt] datetime2 NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_Grados] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [GradosEstudio] (
    [Id] nvarchar(450) NOT NULL,
    [Nombre] nvarchar(256) NULL,
    [CreatedAt] datetime2 NULL,
    [UpdatedAt] datetime2 NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_GradosEstudio] PRIMARY KEY ([Id])
);

GO

ALTER TABLE [Beneficiarios] ADD [PadreId] nvarchar(450) NULL;

GO

ALTER TABLE [Beneficiarios] ADD CONSTRAINT [FK_Beneficiarios_Padre] FOREIGN KEY ([PadreId]) REFERENCES [Beneficiarios] ([Id]);

GO

ALTER TABLE [Beneficiarios] ADD [MismoDomicilio] bit NULL DEFAULT 0;

GO

ALTER TABLE [Beneficiarios] ADD [ParentescoId] nvarchar(450) NULL;

GO

ALTER TABLE [Beneficiarios] ADD CONSTRAINT [FK_Beneficiario_Parentesco] FOREIGN KEY ([ParentescoId]) REFERENCES [Parentescos] ([Id]);

GO

ALTER TABLE [Beneficiarios] ADD [GradoEstudioId] nvarchar(450) NULL;

GO

ALTER TABLE [Beneficiarios] ADD CONSTRAINT [FK_Beneficiario_GradosEstudio] FOREIGN KEY ([GradoEstudioId]) REFERENCES [GradosEstudio] ([Id]);

GO

ALTER TABLE [Beneficiarios] ADD [CausaDiscapacidadId] nvarchar(450) NULL;

GO

ALTER TABLE [Beneficiarios] ADD CONSTRAINT [FK_Beneficiario_CausaDiscapacidad] FOREIGN KEY ([CausaDiscapacidadId]) REFERENCES [CausaDiscapacidad] ([Id]);

GO

ALTER TABLE [Beneficiarios] ADD [DiscapacidadGradoId] nvarchar(450) NULL;

GO

ALTER TABLE [Beneficiarios] ADD CONSTRAINT [FK_Beneficiario_DiscapacidadGrado] FOREIGN KEY ([DiscapacidadGradoId]) REFERENCES [DiscapacidadGrado] ([Id]);

GO

update vertientes set costo=1;
go

update unidades set CreatedAt='2019-11-01', UpdatedAt='2019-11-01';
go

update Beneficiarios set MismoDomicilio=0;
go

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20191003161342_EnrolamientoHijos', N'2.1.11-servicing-32099');

GO
