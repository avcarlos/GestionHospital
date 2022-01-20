CREATE PROCEDURE [dbo].[EliminarEspecialidadMedico]
	@i_id_medico		int,
	@i_id_especialidad	int
AS

DELETE
FROM	dbo.EspecialidadMedico
WHERE	IdMedico = @i_id_medico
  AND	IdEspecialidad = @i_id_especialidad

RETURN 0