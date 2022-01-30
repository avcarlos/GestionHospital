CREATE PROCEDURE [dbo].[ActualizarEspecialidad]
	@i_id_especialidad			int,
	@i_nombre					varchar(30) = NULL,
	@i_descripcion				varchar(300) = NULL,
	@i_estado					bit = NULL,
	@i_id_usuario_modificacion	int
AS

UPDATE	Especialidad
SET		Nombre = ISNULL(@i_nombre, Nombre),
		Descripcion = ISNULL(@i_descripcion, Descripcion),
		Estado = ISNULL(@i_estado, Estado),
		IdUsuarioModificacion = @i_id_usuario_modificacion,
		FechaModificacion = GETDATE()
WHERE	IdEspecialidad = @i_id_especialidad

RETURN 0