CREATE PROCEDURE [dbo].[GuardarCita]
	@i_id_paciente		int,
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

INSERT INTO Cita
(
	IdPaciente,
	IdEspecialidadMedico,
	Fecha,
	IdHorario,
	Motivo,
	IdEstado,
	Reagendada
)
VALUES
(
	@i_id_paciente,
	@w_id_especialidad_medico,
	@i_fecha,
	@i_id_horario,
	@i_motivo,
	@i_id_estado,
	0
)

RETURN 0