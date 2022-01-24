CREATE PROCEDURE [dbo].[GuardarTipoPersona]
	@i_id_persona	int,
	@i_id_tipo		int
AS

IF NOT EXISTS (	SELECT	1
				FROM	dbo.TipoPersona
				WHERE	IdPersona = @i_id_persona
				  AND	IdTipo = @i_id_tipo)
BEGIN
	INSERT INTO dbo.TipoPersona
	(
		IdPersona,
		IdTipo,
		Estado
	)
	VALUES
	(
		@i_id_persona,
		@i_id_tipo,
		1
	)
END
ELSE
BEGIN
	UPDATE	dbo.TipoPersona
	SET		Estado = 1
	WHERE	IdPersona = @i_id_persona
	  AND	IdTipo = @i_id_tipo
END

RETURN 0