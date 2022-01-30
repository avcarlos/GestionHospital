CREATE PROCEDURE [dbo].[EliminarTransaccion]
	@i_id_transaccion			int,
	@i_id_usuario_modificacion	int
AS

UPDATE	dbo.Transaccion
SET		Estado = 0,
		IdUsuarioModificacion = @i_id_usuario_modificacion,
		FechaModificacion = GETDATE()
WHERE	IdTransaccion = @i_id_transaccion

RETURN 0