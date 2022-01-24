CREATE PROCEDURE [dbo].[ConsultarPersona]
	@i_id_tipo_identificacion	int,
	@i_identificacion			varchar(13),
	@i_id_tipo_persona			int = NULL
AS

SELECT	Persona.*, 
		TipoPersona.IdTipo,
		TipoPersona.Estado AS 'EstadoTipo'
FROM	Persona
		LEFT JOIN TipoPersona ON Persona.IdPersona = TipoPersona.IdPersona
WHERE	Persona.IdTipoIdentificacion = @i_id_tipo_identificacion
  AND	Persona.Identificacion = @i_identificacion
  AND	ISNULL(TipoPersona.IdTipo, 0) = ISNULL(@i_id_tipo_persona, ISNULL(TipoPersona.IdTipo, 0))

RETURN 0