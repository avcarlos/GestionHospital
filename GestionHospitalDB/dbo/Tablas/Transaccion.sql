CREATE TABLE [dbo].[Transaccion]
(
	[IdTransaccion]		INT Identity(1,1)	NOT NULL,
	[Nombre]			VARCHAR(50)			NOT NULL,
	[Descripcion]		VARCHAR(300)		NULL,
	[Estado]			BIT					NULL,
	[Menu]				VARCHAR(300)		NULL,
	CONSTRAINT [PK_Transaccion] PRIMARY KEY (IdTransaccion)
)