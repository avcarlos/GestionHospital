CREATE PROCEDURE [dbo].[ActualizarRolSeguridad]
	@i_id_rol_seguridad		int,
	@i_nombre				varchar(30) = NULL,
	@i_descripcion			varchar(300) = NULL,
	@i_estado				bit = NULL,
	@i_id_usuario			int
AS

UPDATE	RolSeguridad
SET		Nombre = ISNULL(@i_nombre, Nombre),
		Descripcion = ISNULL(@i_descripcion, Descripcion),
		Estado = ISNULL(@i_estado, Estado),
		IdUsuarioModificacion = @i_id_usuario,
		FechaModificacion = GETDATE()
WHERE	IdRolSeguridad = @i_id_rol_seguridad

RETURN 0