CREATE PROCEDURE [dbo].[EliminarTipoPersona]
	@i_id_persona	int,
	@i_id_tipo		int
AS

UPDATE	dbo.TipoPersona
SET		Estado = 0
WHERE	IdPersona = @i_id_persona
  AND	IdTipo = @i_id_tipo

RETURN 0