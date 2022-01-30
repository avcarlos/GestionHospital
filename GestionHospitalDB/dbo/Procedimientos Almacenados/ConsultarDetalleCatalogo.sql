CREATE PROCEDURE [dbo].[ConsultarDetalleCatalogo]
	@i_id_catalogo			int = NULL,
	@i_id_detalle_catalogo	int = NULL
AS

SELECT	*
FROM	DetalleCatalogo
WHERE	IdCatalogo = ISNULL(@i_id_catalogo, IdCatalogo)
  AND	IdDetalleCatalogo = ISNULL(@i_id_detalle_catalogo, IdDetalleCatalogo)

RETURN 0