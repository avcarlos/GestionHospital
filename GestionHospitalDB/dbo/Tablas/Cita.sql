CREATE TABLE [dbo].[Cita]
(
	[IdCita]				INT IDENTITY(1,1)	NOT NULL,
	[IdPaciente]			INT					NOT NULL,
	[IdEspecialidadMedico]	INT					NOT NULL,
	[Fecha]					DATETIME			NOT NULL,
	[HoraInicio]			TIME				NOT NULL,
	[HoraFin]				TIME				NOT NULL,
	[Motivo]				VARCHAR(300)		NULL,
	[Observaciones]			VARCHAR(300)		NULL,
	[Examenes]				VARCHAR(300)		NULL,
	[Receta]				VARCHAR(300)		NULL,
	[FechaProximoControl]	DATETIME			NULL,
	[Estado]				INT					NULL,
	CONSTRAINT [PK_Cita] PRIMARY KEY (IdCita),
	CONSTRAINT [FK_IdPersona] FOREIGN KEY (IdPaciente) REFERENCES Persona(IdPersona)
)
