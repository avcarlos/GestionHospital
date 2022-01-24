CREATE PROCEDURE [dbo].[ConsultarUsuarioPersona]
	@i_id_persona		int
AS

SELECT	*
FROM	dbo.Usuario
WHERE	IdPersona = @i_id_persona

RETURN 0