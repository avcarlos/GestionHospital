CREATE PROCEDURE [dbo].[EliminarDetalleReceta]
	@i_id_detalle_receta	int
AS

DELETE FROM dbo.DetalleReceta
WHERE	IdDetalleReceta = @i_id_detalle_receta

RETURN 0