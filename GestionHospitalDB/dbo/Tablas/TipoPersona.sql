CREATE TABLE [dbo].[TipoPersona]
(
	[IdTipoPersona]		INT IDENTITY(1,1)	NOT NULL,
	[IdPersona]			INT					NOT NULL,
	[IdTipo]			INT					NOT NULL,
	CONSTRAINT [PK_TipoPersona] PRIMARY KEY (IdTipoPersona),
	CONSTRAINT [FK_Persona_TipoPersona] FOREIGN KEY (IdPersona) REFERENCES Persona(IdPersona)
)
