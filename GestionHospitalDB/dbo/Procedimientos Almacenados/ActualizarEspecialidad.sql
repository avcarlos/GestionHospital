CREATE PROCEDURE [dbo].[ActualizarEspecialidad]
	@i_id_especialidad			int,
	@i_nombre					varchar(30),
	@i_descripcion				varchar(300),
	@i_id_usuario_modificacion	int
AS

UPDATE	Especialidad
SET		Nombre = @i_nombre,
		Descripcion = @i_descripcion,
		IdUsuarioModificacion = @i_id_usuario_modificacion,
		FechaModificacion = GETDATE()
WHERE	IdEspecialidad = @i_id_especialidad

RETURN 0