CREATE PROCEDURE [dbo].[ConsultarPersonaId]
	@i_id_persona	int
AS

SELECT	*
FROM	Persona
WHERE	Persona.IdPersona = @i_id_persona

RETURN 0