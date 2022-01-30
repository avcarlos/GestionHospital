CREATE TABLE [dbo].[Receta]
(
	[IdReceta]			INT IDENTITY(1,1)	NOT NULL,
	[IdCita]			INT					NOT NULL,
	[Observaciones]		VARCHAR(300)		NULL,
	[IdUsuarioCreacion]	INT					NULL,
	[FechaCreacion]		DATETIME			NULL,
	CONSTRAINT [PK_Receta] PRIMARY KEY (IdReceta),
	CONSTRAINT [FK_Cita_Receta] FOREIGN KEY (IdCita) REFERENCES Cita(IdCita)
)