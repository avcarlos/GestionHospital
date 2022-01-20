CREATE PROCEDURE [dbo].[GuardarEspecialidadMedico]
	@i_id_medico		int,
	@i_id_especialidad	int
AS

IF NOT EXISTS (	SELECT	1
				FROM	dbo.EspecialidadMedico
				WHERE	IdMedico = @i_id_medico
				  AND	IdEspecialidad = @i_id_especialidad)
BEGIN
	INSERT INTO dbo.EspecialidadMedico
	(
		IdMedico,
		IdEspecialidad
	)
	VALUES
	(
		@i_id_medico,
		@i_id_especialidad
	)
END

RETURN 0