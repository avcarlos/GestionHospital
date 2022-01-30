CREATE TABLE [dbo].[EspecialidadMedico]
(
	[IdEspecialidadMedico]	INT IDENTITY(1,1)	NOT NULL,
	[IdMedico]				INT					NOT NULL,
	[IdEspecialidad]		INT					NOT NULL,
	[Estado]				BIT					NULL,
	CONSTRAINT [PK_EspecialidadMedico] PRIMARY KEY (IdEspecialidadMedico),
	CONSTRAINT [FK_Persona_EspecialidadMedico] FOREIGN KEY (IdMedico) REFERENCES Persona(IdPersona),
	CONSTRAINT [FK_Especialidad_EspecialidadMedico] FOREIGN KEY (IdEspecialidad) REFERENCES Especialidad(IdEspecialidad)
)