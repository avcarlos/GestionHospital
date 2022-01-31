CREATE PROCEDURE [dbo].[ConsultarExamenesCita]
	@i_id_cita		int
AS

SELECT	*
FROM	ExamenesCita
WHERE	IdCita = @i_id_cita

RETURN 0