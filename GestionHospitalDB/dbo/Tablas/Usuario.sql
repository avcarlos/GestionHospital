CREATE TABLE [dbo].[Usuario]
(
	[IdUsuario]			INT IDENTITY(1,1) NOT NULL,
	[LoginUsuario]		VARCHAR(30)		NOT NULL,
	[PasswordUsuario]	VARCHAR(20)		NULL,
	[IdRolSeguridad]	INT				NOT NULL,
	[IdPersona]			INT				NULL,
	[Estado]			BIT				NULL,
	CONSTRAINT [PK_Usuario] PRIMARY KEY (IdUsuario),
	CONSTRAINT [FK_RolSeguridad_Usuario] FOREIGN KEY (IdRolSeguridad) REFERENCES RolSeguridad(IdRolSeguridad),
	CONSTRAINT [FK_Persona_Usuario] FOREIGN KEY (IdPersona) REFERENCES Persona(IdPersona)
)