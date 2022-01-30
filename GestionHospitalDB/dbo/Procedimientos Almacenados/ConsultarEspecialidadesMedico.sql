CREATE PROCEDURE [dbo].[ConsultarEspecialidadesMedico]
	@i_id_medico	int
AS

SELECT	dbo.EspecialidadMedico.*,
		dbo.Especialidad.Nombre
FROM	dbo.EspecialidadMedico
		JOIN dbo.Especialidad ON dbo.EspecialidadMedico.IdEspecialidad = dbo.Especialidad.IdEspecialidad
WHERE	IdMedico = @i_id_medico
  AND	dbo.EspecialidadMedico.Estado = 1

RETURN 0