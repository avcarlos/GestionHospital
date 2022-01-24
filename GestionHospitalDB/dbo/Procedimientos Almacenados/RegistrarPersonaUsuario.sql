CREATE PROCEDURE [dbo].[RegistrarPersonaUsuario]
	@i_id_usuario		int,
	@i_id_persona		int
AS

UPDATE	dbo.Usuario
SET		IdPersona = @i_id_persona
WHERE	IdUsuario = @i_id_usuario

RETURN 0