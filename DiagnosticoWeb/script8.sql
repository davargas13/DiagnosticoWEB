ALTER TABLE [Municipios] ADD [Indice] float NOT NULL DEFAULT 0;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200826184001_AddIndiceMunicipios', N'2.1.0-rtm-30799');

GO

ALTER TABLE [Domicilio] ADD [TipoCalculo] nvarchar(max) NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200831133935_AddCalculoDomicilio', N'2.1.0-rtm-30799');

GO

UPDATE Municipios SET indice=-0.3273146 WHERE id='001';
UPDATE Municipios SET indice=-0.8705792 WHERE id='002';
UPDATE Municipios SET indice=-0.2216494 WHERE id='003';
UPDATE Municipios SET indice=-0.3628738 WHERE id='004';
UPDATE Municipios SET indice=-0.5856001 WHERE id='005';
UPDATE Municipios SET indice=0.9195024 WHERE id='006';
UPDATE Municipios SET indice=-1.247316 WHERE id='007';
UPDATE Municipios SET indice=-0.4264252 WHERE id='008';
UPDATE Municipios SET indice=-0.0607258 WHERE id='009';
UPDATE Municipios SET indice=-0.3867472 WHERE id='010';
UPDATE Municipios SET indice=-0.9200176 WHERE id='011';
UPDATE Municipios SET indice=-0.5502287 WHERE id='012';
UPDATE Municipios SET indice=0.0740071 WHERE id='013';
UPDATE Municipios SET indice=-0.1503135 WHERE id='014';
UPDATE Municipios SET indice=-1.087733 WHERE id='015';
UPDATE Municipios SET indice=-0.658052 WHERE id='016';
UPDATE Municipios SET indice=-1.083139 WHERE id='017';
UPDATE Municipios SET indice=-0.8772981 WHERE id='018';
UPDATE Municipios SET indice=0.1135169 WHERE id='019';
UPDATE Municipios SET indice=-1.225117 WHERE id='020';
UPDATE Municipios SET indice=-1.063325 WHERE id='021';
UPDATE Municipios SET indice=-0.1261756 WHERE id='022';
UPDATE Municipios SET indice=-0.3772973 WHERE id='023';
UPDATE Municipios SET indice=-0.7016186 WHERE id='024';
UPDATE Municipios SET indice=-0.7025726 WHERE id='025';
UPDATE Municipios SET indice=-0.3390784 WHERE id='026';
UPDATE Municipios SET indice=-1.203662 WHERE id='027';
UPDATE Municipios SET indice=-0.7062282 WHERE id='028';
UPDATE Municipios SET indice=0.2635599 WHERE id='029';
UPDATE Municipios SET indice=0.2574427 WHERE id='030';
UPDATE Municipios SET indice=-0.7938851 WHERE id='031';
UPDATE Municipios SET indice=-0.6291258 WHERE id='032';
UPDATE Municipios SET indice=-0.1244905 WHERE id='033';
UPDATE Municipios SET indice=0.5311181 WHERE id='034';
UPDATE Municipios SET indice=-0.3131617 WHERE id='035';
UPDATE Municipios SET indice=-0.6750426 WHERE id='036';
UPDATE Municipios SET indice=-0.553416 WHERE id='037';
UPDATE Municipios SET indice=-0.8142988 WHERE id='038';
UPDATE Municipios SET indice=-0.5627993 WHERE id='039';
UPDATE Municipios SET indice=0.8712348 WHERE id='040';
UPDATE Municipios SET indice=-0.8675064 WHERE id='041';
UPDATE Municipios SET indice=-0.5370325 WHERE id='042';
UPDATE Municipios SET indice=0.6467889 WHERE id='043';
UPDATE Municipios SET indice=-1.045395 WHERE id='044';
UPDATE Municipios SET indice=0.7456042 WHERE id='045';
UPDATE Municipios SET indice=-0.4039261 WHERE id='046';