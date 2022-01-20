CREATE PROCEDURE [dbo].[ConsultarTipoPersona]
	@i_id_persona	int
AS

SELECT	*
FROM	dbo.TipoPersona

RETURN 0