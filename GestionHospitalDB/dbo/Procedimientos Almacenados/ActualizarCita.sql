CREATE PROCEDURE [dbo].[ActualizarCita]
	@i_id_cita			int,
	@i_id_especialidad	int,
	@i_id_medico		int,
	@i_fecha			datetime,
	@i_id_horario		int,
	@i_motivo			varchar(300),
	@i_id_estado		int
AS

DECLARE	@w_id_especialidad_medico	int

SELECT	@w_id_especialidad_medico = IdEspecialidadMedico
FROM	EspecialidadMedico
WHERE	IdEspecialidad = @i_id_especialidad
  AND	IdMedico = @i_id_medico

UPDATE	Cita
SET		IdEspecialidadMedico = @w_id_especialidad_medico,
		Fecha = @i_fecha,
		IdHorario = @i_id_horario,
		Motivo = @i_motivo,
		Reagendada = 1
WHERE	IdCita = @i_id_cita

RETURN 0