CREATE PROCEDURE [dbo].[EliminarEspecialidadMedico]
	@i_id_medico		int,
	@i_id_especialidad	int
AS

UPDATE	dbo.EspecialidadMedico
SET		Estado = 0
WHERE	IdMedico = @i_id_medico
  AND	IdEspecialidad = @i_id_especialidad

RETURN 0