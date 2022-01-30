CREATE PROCEDURE [dbo].[ConsultarPersonaEmail]
	@i_email	varchar(30)
AS

SELECT	*
FROM	dbo.Persona
WHERE	LTRIM(RTRIM(Email)) = LTRIM(RTRIM(@i_email))

RETURN 0