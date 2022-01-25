CREATE PROCEDURE [dbo].[ConsultarCitas]
	@i_id_cita					int = NULL,
	@i_id_paciente				int = NULL,
	@i_id_medico				int = NULL,
	@i_id_estado				int = NULL,
	@i_id_especialidad			int = NULL,
	@i_fecha					datetime = NULL,
	@i_fecha_proximo_control	datetime = NULL
AS

SELECT	Cita.*,
		EspecialidadMedico.IdEspecialidad,
		EspecialidadMedico.IdMedico
FROM	Cita
		JOIN EspecialidadMedico ON Cita.IdEspecialidadMedico = EspecialidadMedico.IdEspecialidadMedico
WHERE	Cita.IdPaciente = ISNULL(@i_id_paciente, Cita.IdPaciente)
  AND	Cita.IdEstado = ISNULL(@i_id_estado, Cita.IdEstado)
  AND	Cita.Fecha = ISNULL(@i_fecha, Cita.Fecha)
  AND	ISNULL(Cita.FechaProximoControl, 0) = ISNULL(@i_fecha_proximo_control, ISNULL(Cita.FechaProximoControl, 0))
  AND	EspecialidadMedico.IdMedico = ISNULL(@i_id_medico, EspecialidadMedico.IdMedico)
  AND	EspecialidadMedico.IdEspecialidad = ISNULL(@i_id_especialidad, EspecialidadMedico.IdEspecialidad)

RETURN 0