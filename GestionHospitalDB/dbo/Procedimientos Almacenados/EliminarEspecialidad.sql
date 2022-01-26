CREATE PROCEDURE [dbo].[EliminarEspecialidad]
	@i_id_especialidad			int,
	@i_id_usuario_modificacion	int
AS

UPDATE	Especialidad
SET		Estado = 0,
		IdUsuarioModificacion = @i_id_usuario_modificacion,
		FechaModificacion = GETDATE()
WHERE	IdEspecialidad = @i_id_especialidad

RETURN 0