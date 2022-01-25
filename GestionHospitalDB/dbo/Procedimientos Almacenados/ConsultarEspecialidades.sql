CREATE PROCEDURE [dbo].[ConsultarEspecialidades]
	@i_id_especialidad		int = NULL
AS
	
SELECT	*
FROM	dbo.Especialidad
WHERE	IdEspecialidad = ISNULL(@i_id_especialidad, IdEspecialidad)

RETURN 0