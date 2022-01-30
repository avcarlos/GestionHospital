CREATE TABLE [dbo].[Catalogo]
(
	[IdCatalogo]	INT IDENTITY(1,1)	NOT NULL,
	[Nombre]		VARCHAR(30)			NOT NULL,
	[Descripcion]	VARCHAR(300)		NULL,
	[Estado]		BIT					NULL,
	[Administrable]	BIT					NULL,
	CONSTRAINT [PK_Catalogo] PRIMARY KEY (IdCatalogo)
)