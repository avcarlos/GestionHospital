CREATE PROCEDURE [dbo].[GuardarDetalleCatalogo]
	@i_id_catalogo		int,
	@i_nombre			varchar(50) = NULL,
	@i_codigo			varchar(10) = NULL,
	@i_id_usuario		int
AS

INSERT INTO DetalleCatalogo
(
	IdCatalogo,
	Nombre,
	Codigo,
	Estado,
	IdUsuarioCreacion,
	FechaCreacion
)
VALUES
(
	@i_id_catalogo,
	@i_nombre,
	@i_codigo,
	1,
	@i_id_usuario,
	GETDATE()
)

RETURN 0