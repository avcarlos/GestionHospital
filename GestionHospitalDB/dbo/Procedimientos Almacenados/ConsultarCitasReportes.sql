CREATE PROCEDURE [dbo].[ConsultarCitasReportes]
	@i_id_estado			int = NULL,
	@i_fecha_desde			datetime,
	@i_fecha_hasta			datetime
AS

SELECT	Cita.*,
		EspecialidadMedico.IdEspecialidad,
		EspecialidadMedico.IdMedico
FROM	Cita
		JOIN EspecialidadMedico ON Cita.IdEspecialidadMedico = EspecialidadMedico.IdEspecialidadMedico
WHERE	Cita.IdEstado = ISNULL(@i_id_estado, Cita.IdEstado)
  AND	Cita.Fecha >= @i_fecha_desde
  AND	Cita.Fecha <= @i_fecha_hasta

RETURN 0