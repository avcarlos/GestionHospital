CREATE TABLE [dbo].[Usuario]
(
	[IdUsuario]		INT IDENTITY(1,1) NOT NULL,
	[Login]			VARCHAR(20)		NOT NULL,
	[Password]		VARCHAR(20)		NULL,
	CONSTRAINT [PK_Usuario] PRIMARY KEY ([IdUsuario])
)