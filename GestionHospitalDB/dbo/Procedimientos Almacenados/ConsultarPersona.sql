CREATE PROCEDURE [dbo].[ConsultarPersona]
	@i_id_tipo_identificacion	int,
	@i_identificacion			varchar(13),
	@i_id_tipo_persona			int = NULL
AS

SELECT	Persona.*, 
		TipoPersona.IdTipoPersona
FROM	Persona
		JOIN TipoPersona ON Persona.IdPersona = TipoPersona.IdPersona
WHERE	Persona.IdTipoIdenticacion = @i_id_tipo_identificacion
  AND	Persona.Identificacion = @i_identificacion
  AND	TipoPersona.IdTipoPersona = ISNULL(@i_id_tipo_persona, TipoPersona.IdTipoPersona)

RETURN 0