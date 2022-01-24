CREATE TABLE [dbo].[Horario]
(
	[IdHorario]		INT IDENTITY(1,1)	NOT NULL,
	[Nombre]		VARCHAR(15)		NOT NULL,
	[HoraInicio]	TIME			NOT NULL,
	[HoraFin]		TIME			NOT NULL,
	[Estado]		BIT				NULL,
	CONSTRAINT [PK_Horario] PRIMARY KEY (IdHorario)
)
