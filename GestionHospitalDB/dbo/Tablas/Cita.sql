CREATE TABLE [dbo].[Cita]
(
	[IdCita]				INT IDENTITY(1,1)	NOT NULL,
	[IdPaciente]			INT					NOT NULL,
	[IdEspecialidadMedico]	INT					NOT NULL,
	[Fecha]					DATETIME			NOT NULL,
	[IdHorario]				INT					NOT NULL,
	[Motivo]				VARCHAR(300)		NULL,
	[Diagnostico]			VARCHAR(300)		NULL,
	[Observaciones]			VARCHAR(300)		NULL,
	[Examenes]				VARCHAR(300)		NULL,
	[Receta]				VARCHAR(300)		NULL,
	[FechaProximoControl]	DATETIME			NULL,
	[IdEstado]				INT					NULL,
	[Reagendada]			BIT					NULL,
	CONSTRAINT [PK_Cita] PRIMARY KEY (IdCita),
	CONSTRAINT [FK_IdPersona] FOREIGN KEY (IdPaciente) REFERENCES Persona(IdPersona)
)