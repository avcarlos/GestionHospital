CREATE PROCEDURE [dbo].[ConsultarHorarios]
	@i_id_horario		int = NULL,
	@i_id_tipo_horario	int = NULL
AS

SELECT	*
FROM	Horario
WHERE	IdHorario = ISNULL(@i_id_horario, IdHorario)
  AND	IdTipoHorario = ISNULL(@i_id_tipo_horario, IdTipoHorario)

RETURN 0