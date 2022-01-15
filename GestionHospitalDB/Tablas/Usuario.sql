CREATE TABLE [dbo].[Usuario]
(
	[us_id_usuario]		INT IDENTITY(1,1) NOT NULL,
	[us_login]			VARCHAR(20)		NOT NULL,
	[us_password]		VARCHAR(20)		NULL,
	CONSTRAINT [PK_Usuario] PRIMARY KEY ([us_id_usuario])
)