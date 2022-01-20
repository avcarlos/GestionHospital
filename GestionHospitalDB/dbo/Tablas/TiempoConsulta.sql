CREATE TABLE [dbo].[TiempoConsulta]
(
	[IdTiempoConsulta]	INT IDENTITY(1,1)	NOT NULL,
	[Nombre]			VARCHAR(15)		NOT NULL,
	[HoraInicio]		TIME			NOT NULL,
	[HoraFin]			TIME			NOT NULL,
	CONSTRAINT [PK_TiempoConsulta] PRIMARY KEY (IdTiempoConsulta)
)
