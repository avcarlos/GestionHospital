CREATE TABLE [dbo].[DetalleCatalogo]
(
	[IdDetalleCatalogo]		INT IDENTITY(1,1)	NOT NULL,
	[IdCatalogo]			INT					NOT NULL,
	[Nombre]				VARCHAR(50)			NOT NULL,
	[Codigo]				VARCHAR(10)			NOT NULL,
	CONSTRAINT [PK_DetalleCatalogo] PRIMARY KEY (IdDetalleCatalogo),
	CONSTRAINT [FK_Catalogo_DetalleCatalogo] FOREIGN KEY (IdCatalogo) REFERENCES Catalogo(IdCatalogo)
)