CREATE TABLE [dbo].[DetalleReceta]
(
	[IdDetalleReceta]	INT IDENTITY(1,1)	NOT NULL,
	[IdReceta]			INT					NOT NULL,
	[IdMedicamento]		INT					NULL,
	[Indicaciones]		VARCHAR(150)		NULL,
	CONSTRAINT [PK_DetalleReceta] PRIMARY KEY (IdDetalleReceta),
	CONSTRAINT [FK_Receta_DetalleReceta] FOREIGN KEY (IdReceta) REFERENCES Receta(IdReceta)
)