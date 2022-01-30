CREATE TABLE [dbo].[RolSeguridad]
(
	[IdRolSeguridad]		INT IDENTITY	NOT NULL,
	[Nombre]				VARCHAR(30)		NOT NULL,
	[Descripcion]			VARCHAR(300)	NULL,
	[Estado]				BIT				NULL,
	[IdUsuarioCreacion]		INT				NULL,
	[FechaCreacion]			DATETIME		NULL,
	[IdUsuarioModificacion]	INT				NULL,
	[FechaModificacion]		DATETIME		NULL,
	CONSTRAINT [PK_RolSeguridad] PRIMARY KEY (IdRolSeguridad)
)