CREATE PROCEDURE [dbo].[EliminarPersona]
	@i_id_persona	int
AS

UPDATE	dbo.Persona
SET		Estado = 0
WHERE	IdPersona = @i_id_persona

RETURN 0