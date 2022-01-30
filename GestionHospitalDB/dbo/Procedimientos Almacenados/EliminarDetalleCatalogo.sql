CREATE PROCEDURE [dbo].[EliminarDetalleCatalogo]
	@i_id_detalle_catalogo		int,
	@i_id_usuario				int
AS

UPDATE	dbo.DetalleCatalogo
SET		Estado = 0,
		IdUsuarioModificacion = @i_id_usuario,
		FechaModificacion = GETDATE()
WHERE	IdDetalleCatalogo = @i_id_detalle_catalogo

RETURN 0