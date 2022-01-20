CREATE TABLE [dbo].[Factura]
(
	[IdFactura]	INT IDENTITY(1,1)	NOT NULL,
	[Numero]	VARCHAR(20)			NULL,
	[Total]		MONEY				NOT NULL,
	CONSTRAINT [PK_Factura] PRIMARY KEY (IdFactura)
)