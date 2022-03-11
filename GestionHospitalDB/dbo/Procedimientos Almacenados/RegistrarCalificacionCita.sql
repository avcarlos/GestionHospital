CREATE PROCEDURE [dbo].[RegistrarCalificacionCita]
	@i_id_cita			int,
	@i_id_calificacion	int
AS

UPDATE	Cita
SET		IdCalificacion = @i_id_calificacion,
		FechaCalificacion = GETDATE()
WHERE	IdCita = @i_id_cita

RETURN 0