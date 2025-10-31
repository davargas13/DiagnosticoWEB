ALTER TABLE [Domicilio] ADD [MunicipioCalculado] nvarchar(max) NULL;

GO

ALTER TABLE [Domicilio] ADD [LocalidadCalculado] nvarchar(max) NULL;

GO

ALTER TABLE [Domicilio] ADD [AgebCalculado] nvarchar(max) NULL;

GO

ALTER TABLE [Domicilio] ADD [ManzanaCalculado] nvarchar(max) NULL;

GO

ALTER TABLE [Domicilio] ADD [ZapCalculado] nvarchar(max) NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200709230053_AddDomicilioCalculados', N'2.1.0-rtm-30799');

GO

ALTER TABLE [AplicacionPreguntas] ADD [ValorNumerico] int NULL;

GO

ALTER TABLE [AplicacionPreguntas] ADD [ValorFecha] datetime2 NULL;

GO

ALTER TABLE [AplicacionPreguntas] ADD [ValorCatalogo] nvarchar(max) NULL;

GO

ALTER TABLE [Domicilio] ADD [TipoZap] nvarchar(100) NULL;

GO

ALTER TABLE [Domicilio] ADD [CodigoPostalCalculado] nvarchar(10) NULL;

GO

ALTER TABLE [Domicilio] ADD [Colonia] nvarchar(max) NULL;

GO

ALTER TABLE [Domicilio] ADD [CisCercano] nvarchar(max) NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200805181228_AddValoresAplicacion', N'2.1.0-rtm-30799');

GO

CREATE INDEX [IX_Agebs_LocalidadId] ON [Agebs] ([LocalidadId]);

GO

CREATE INDEX [IX_Agebs_MunicipioId] ON [Agebs] ([MunicipioId]);

GO

CREATE INDEX [IX_Agebs_EstadoId] ON [Agebs] ([EstadoId]);

GO

CREATE INDEX [IX_Aplicaciones_BeneficiarioId] ON [Aplicaciones] ([BeneficiarioId]);

GO

CREATE INDEX [IX_Aplicaciones_EncuestaVersionId] ON [Aplicaciones] ([EncuestaVersionId]);

GO

CREATE INDEX [IX_Aplicaciones_TrabajadorId] ON [Aplicaciones] ([TrabajadorId]);

GO

ALTER TABLE [AplicacionPreguntas] ADD CONSTRAINT [FK_Respuestas_AplicacionPreguntas] FOREIGN KEY ([RespuestaId]) REFERENCES [Respuestas] ([Id]);

GO

CREATE INDEX [IX_AplicacionPreguntas_AplicacionId] ON [AplicacionPreguntas] ([AplicacionId]);

GO

CREATE INDEX [IX_AplicacionPreguntas_PreguntaId] ON [AplicacionPreguntas] ([PreguntaId]);

GO

CREATE INDEX [IX_AplicacionPreguntas_RespuestaId] ON [AplicacionPreguntas] ([RespuestaId]);

GO

CREATE INDEX [IX_BeneficiarioHistorico_BeneficiarioId] ON [BeneficiarioHistorico] ([BeneficiarioId]);

GO

CREATE INDEX [IX_BeneficiarioHistorico_EstudioId] ON [BeneficiarioHistorico] ([EstudioId]);

GO

CREATE INDEX [IX_BeneficiarioHistorico_EstadoId] ON [BeneficiarioHistorico] ([EstadoId]);

GO

CREATE INDEX [IX_BeneficiarioHistorico_EstadoCivilId] ON [BeneficiarioHistorico] ([EstadoCivilId]);

GO

CREATE INDEX [IX_BeneficiarioHistorico_DiscapacidadId] ON [BeneficiarioHistorico] ([DiscapacidadId]);

GO

CREATE INDEX [IX_BeneficiarioHistorico_SexoId] ON [BeneficiarioHistorico] ([SexoId]);

GO

CREATE INDEX [IX_BeneficiarioHistorico_TrabajadorId] ON [BeneficiarioHistorico] ([TrabajadorId]);

GO

CREATE INDEX [IX_BeneficiarioHistorico_GradoEstudioId] ON [BeneficiarioHistorico] ([GradoEstudioId]);

GO

CREATE INDEX [IX_BeneficiarioHistorico_CausaDiscapacidadId] ON [BeneficiarioHistorico] ([CausaDiscapacidadId]);

GO

CREATE INDEX [IX_BeneficiarioHistorico_DiscapacidadGradoId] ON [BeneficiarioHistorico] ([DiscapacidadGradoId]);

GO

CREATE INDEX [IX_Beneficiarios_EstadoId] ON [Beneficiarios] ([EstadoId]);

GO

CREATE INDEX [IX_Beneficiarios_EstudioId] ON [Beneficiarios] ([EstudioId]);

GO

CREATE INDEX [IX_Beneficiarios_EstadoCivilId] ON [Beneficiarios] ([EstadoCivilId]);

GO

CREATE INDEX [IX_Beneficiarios_DiscapacidadId] ON [Beneficiarios] ([DiscapacidadId]);

GO

CREATE INDEX [IX_Beneficiarios_SexoId] ON [Beneficiarios] ([SexoId]);

GO

CREATE INDEX [IX_Beneficiarios_TrabajadorId] ON [Beneficiarios] ([TrabajadorId]);

GO

CREATE INDEX [IX_Beneficiarios_PadreId] ON [Beneficiarios] ([PadreId]);

GO

CREATE INDEX [IX_Beneficiarios_ParentescoId] ON [Beneficiarios] ([ParentescoId]);

GO

CREATE INDEX [IX_Beneficiarios_GradoEstudioId] ON [Beneficiarios] ([GradoEstudioId]);

GO

CREATE INDEX [IX_Beneficiarios_CausaDiscapacidadId] ON [Beneficiarios] ([CausaDiscapacidadId]);

GO

CREATE INDEX [IX_Beneficiarios_DiscapacidadGradoId] ON [Beneficiarios] ([DiscapacidadGradoId]);

GO

CREATE INDEX [IX_Beneficiarios_HijoId] ON [Beneficiarios] ([HijoId]);

GO

CREATE INDEX [IX_Bitacora_UsuarioId] ON [Bitacora] ([UsuarioId]);

GO

CREATE INDEX [IX_DiscapacidadGrado_DiscapacidadId] ON [DiscapacidadGrado] ([DiscapacidadId]);

GO

CREATE INDEX [IX_DiscapacidadGrado_GradoId] ON [DiscapacidadGrado] ([GradoId]);

GO

CREATE INDEX [IX_Domicilio_AgebId] ON [Domicilio] ([AgebId]);

GO

CREATE INDEX [IX_Domicilio_BeneficiarioId] ON [Domicilio] ([BeneficiarioId]);

GO

CREATE INDEX [IX_Domicilio_MunicipioId] ON [Domicilio] ([MunicipioId]);

GO

CREATE INDEX [IX_Domicilio_LocalidadId] ON [Domicilio] ([LocalidadId]);

GO

CREATE INDEX [IX_Domicilio_TrabajadorId] ON [Domicilio] ([TrabajadorId]);

GO

CREATE INDEX [IX_Domicilio_ManzanaId] ON [Domicilio] ([ManzanaId]);

GO

CREATE INDEX [IX_Domicilio_CarreteraId] ON [Domicilio] ([CarreteraId]);

GO

CREATE INDEX [IX_Domicilio_CaminoId] ON [Domicilio] ([CaminoId]);

GO

CREATE INDEX [IX_Domicilio_TipoAsentamientoId] ON [Domicilio] ([TipoAsentamientoId]);

GO

CREATE INDEX [IX_EncuestaVersiones_EncuestaId] ON [EncuestaVersiones] ([EncuestaId]);

GO

CREATE INDEX [IX_Importaciones_EncuestaId] ON [Importaciones] ([UsuarioId]);

GO

CREATE INDEX [IX_Localidades_ZonaIndigenaId] ON [Localidades] ([ZonaIndigenaId]);

GO

CREATE INDEX [IX_Localidades_MunicipioId] ON [Localidades] ([MunicipioId]);

GO

CREATE INDEX [IX_Localidades_EstadoId] ON [Localidades] ([EstadoId]);

GO

CREATE INDEX [IX_Manzanas_LocalidadId] ON [Manzanas] ([LocalidadId]);

GO

CREATE INDEX [IX_Manzanas_MunicipioId] ON [Manzanas] ([MunicipioId]);

GO

CREATE INDEX [IX_Manzanas_AgebId] ON [Manzanas] ([AgebId]);

GO

CREATE INDEX [IX_Municipios_EstadoId] ON [Municipios] ([EstadoId]);

GO

CREATE INDEX [IX_MunicipiosZonas_ZonaId] ON [MunicipiosZonas] ([ZonaId]);

GO

CREATE INDEX [IX_MunicipiosZonas_MunicipioId] ON [MunicipiosZonas] ([MunicipioId]);

GO

CREATE INDEX [IX_PerfilPermiso_PerfilId] ON [PerfilPermiso] ([PerfilId]);

GO

CREATE INDEX [IX_PerfilPermiso_PermisoId] ON [PerfilPermiso] ([PermisoId]);

GO

CREATE INDEX [IX_PreguntaGrado_PreguntaId] ON [PreguntaGrado] ([PreguntaId]);

GO

CREATE INDEX [IX_Preguntas_EncuestaVersionId] ON [Preguntas] ([EncuestaVersionId]);

GO

CREATE INDEX [IX_Preguntas_CarenciaId] ON [Preguntas] ([CarenciaId]);

GO

CREATE INDEX [IX_RespuestaGrado_RespuestaId] ON [RespuestaGrado] ([RespuestaId]);

GO

CREATE INDEX [IX_RespuestaGrado_PreguntaGradoId] ON [RespuestaGrado] ([PreguntaGradoId]);

GO

CREATE INDEX [IX_Respuestas_PreguntaId] ON [Respuestas] ([PreguntaId]);

GO

CREATE INDEX [IX_TrabajadoresDependencias_DependenciaId] ON [TrabajadoresDependencias] ([DependenciaId]);

GO

CREATE INDEX [IX_TrabajadoresDependencias_TrabajadorId] ON [TrabajadoresDependencias] ([TrabajadorId]);

GO

CREATE INDEX [IX_UsuariosDependencias_DependenciaId] ON [UsuariosDependencias] ([DependenciaId]);

GO

CREATE INDEX [IX_UsuariosDependencias_UsuarioId] ON [UsuariosDependencias] ([UsuarioId]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200805181239_Indices', N'2.1.0-rtm-30799');

GO

ALTER TABLE [Aplicaciones] ADD [FechaInicio] datetime2 NULL;

GO

ALTER TABLE [Aplicaciones] ADD [FechaFin] datetime2 NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200806142551_AddTiempoAplicacion', N'2.1.0-rtm-30799');

GO

ALTER TABLE [Aplicaciones] ADD [FechaSincronizacion] datetime2 NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200806151312_AddFechaSincronizacionAplicacion', N'2.1.0-rtm-30799');

GO

ALTER TABLE [AspNetUsers] ADD [Token] nvarchar(max) NULL;

GO

ALTER TABLE [AspNetUsers] ADD [FechaToken] datetime2 NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200806165850_AddTokenUsers', N'2.1.0-rtm-30799');

GO

