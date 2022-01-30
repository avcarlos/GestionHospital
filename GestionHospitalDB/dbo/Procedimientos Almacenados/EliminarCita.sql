CREATE PROCEDURE [dbo].[EliminarCita]
	@i_id_cita		int
AS

UPDATE	Cita
SET		IdEstado = 17
WHERE	IdCita = @i_id_cita

RETURN 0