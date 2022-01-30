CREATE PROCEDURE [dbo].[EliminarRolSeguridad]
	@i_id_rol_seguridad		int,
	@i_id_usuario			int
AS

UPDATE	RolSeguridad
SET		Estado = 0,
		IdUsuarioModificacion = @i_id_usuario,
		FechaModificacion = GETDATE()
WHERE	IdRolSeguridad = @i_id_rol_seguridad

RETURN 0