CREATE PROCEDURE [dbo].[GuardarExamenCita]
	@i_id_cita			int,
	@i_id_examen		int,
	@i_indicaciones		varchar(150) = NULL
AS

INSERT INTO dbo.ExamenesCita
(
	IdCita,
	IdExamen,
	Indicaciones
)
VALUES
(
	@i_id_cita,
	@i_id_examen,
	@i_indicaciones
)

RETURN 0