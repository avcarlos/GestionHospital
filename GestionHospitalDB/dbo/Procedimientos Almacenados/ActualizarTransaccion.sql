CREATE PROCEDURE [dbo].[ActualizarTransaccion]
	@i_id_transaccion			int,
	@i_descripcion				varchar(300) = NULL,
	@i_estado					bit = NULL,
	@i_id_usuario_modificacion	int
AS

UPDATE	dbo.Transaccion
SET		Estado = ISNULL(@i_estado, Estado),
		Descripcion = ISNULL(@i_descripcion, Descripcion),
		IdUsuarioModificacion = @i_id_usuario_modificacion,
		FechaModificacion = GETDATE()
WHERE	IdTransaccion = @i_id_transaccion

RETURN 0