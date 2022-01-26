CREATE TABLE [dbo].[Especialidad]
(
	[IdEspecialidad]		INT IDENTITY(1,1)	NOT NULL,
	[Nombre]				VARCHAR(30)			NOT NULL,
	[Descripcion]			VARCHAR(300)		NULL,
	[Estado]				BIT					NULL,
	[IdUsuarioRegistro]		INT					NULL,
	[FechaRegistro]			DATETIME			NULL,
	[IdUsuarioModificacion]	INT					NULL,
	[FechaModificacion]		DATETIME			NULL,
	CONSTRAINT [PK_Especialidad] PRIMARY KEY (IdEspecialidad),
	CONSTRAINT [FK_UsuarioRegistro_Especialidad] FOREIGN KEY (IdUsuarioRegistro) REFERENCES Usuario(IdUsuario),
	CONSTRAINT [FK_UsuarioModificacion_Especialidad] FOREIGN KEY (IdUsuarioModificacion) REFERENCES Usuario(IdUsuario)
)