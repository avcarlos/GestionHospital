CREATE PROCEDURE [dbo].[ConsultarUsuario]
	@i_nombre_usuario	varchar(30) = NULL
AS
	
SELECT	*
FROM	Usuario
WHERE	LoginUsuario = ISNULL(@i_nombre_usuario, LoginUsuario)
  AND	Estado = 1

RETURN 0