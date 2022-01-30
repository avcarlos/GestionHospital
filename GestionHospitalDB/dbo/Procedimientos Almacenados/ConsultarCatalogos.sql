CREATE PROCEDURE [dbo].[ConsultarCatalogos]
	@i_id_catalogo		int = NULL
AS

SELECT	*
FROM	Catalogo
WHERE	IdCatalogo = ISNULL(@i_id_catalogo, IdCatalogo)

RETURN 0