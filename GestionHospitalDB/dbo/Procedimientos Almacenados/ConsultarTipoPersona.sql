CREATE PROCEDURE [dbo].[ConsultarTipoPersona]
	@i_id_persona	int
AS

SELECT	*
FROM	dbo.TipoPersona
WHERE	IdPersona = @i_id_persona

RETURN 0