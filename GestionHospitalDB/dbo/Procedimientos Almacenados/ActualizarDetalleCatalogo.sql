CREATE PROCEDURE [dbo].[ActualizarDetalleCatalogo]
	@i_id_detalle_catalogo		int,
	@i_nombre					varchar(50) = NULL,
	@i_codigo					varchar(10) = NULL,
	@i_estado					bit = NULL,
	@i_id_usuario				int
AS

UPDATE	DetalleCatalogo
SET		Nombre = ISNULL(@i_nombre, Nombre),
		Codigo = ISNULL(@i_codigo, Codigo),
		Estado = ISNULL(@i_estado, Estado),
		IdUsuarioModificacion = @i_id_usuario,
		FechaModificacion = GETDATE()
WHERE	IdDetalleCatalogo = @i_id_detalle_catalogo

RETURN 0