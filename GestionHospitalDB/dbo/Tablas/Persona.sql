CREATE TABLE [dbo].[Persona]
(
	[IdPersona]				INT IDENTITY(1,1)	NOT NULL,
	[Identificacion]		VARCHAR(13)		NOT NULL,
	[IdTipoIdenticacion]	INT				NOT NULL,
	[Nombres]				VARCHAR(50)		NOT NULL,
	[Apellidos]				VARCHAR(50)		NOT NULL,
	[FechaNacimiento]		DATETIME		NULL,
	[Telefono]				VARCHAR(12)		NULL,
	[Celular]				VARCHAR(12)		NULL,
	[Email]					VARCHAR(30)		NULL,
	[IdGenero]				INT				NULL,
	[Direccion]				VARCHAR(300)	NULL,
	[IdCiudad]				INT				NULL,
	[Estado]				BIT				NULL,
	CONSTRAINT [PK_Persona] PRIMARY KEY (IdPersona)
)