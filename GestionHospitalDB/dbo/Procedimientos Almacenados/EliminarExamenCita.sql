CREATE PROCEDURE [dbo].[EliminarExamenCita]
	@i_id_examen_cita	int
AS

DELETE FROM dbo.ExamenesCita
WHERE	IdExamenesCita = @i_id_examen_cita

RETURN 0