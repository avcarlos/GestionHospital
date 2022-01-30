CREATE PROCEDURE [dbo].[ConsultarRolesSeguridad]
	@i_id_rol_seguridad		int = NULL
AS
	
SELECT	*
FROM	dbo.RolSeguridad
WHERE	IdRolSeguridad = ISNULL(@i_id_rol_seguridad, IdRolSeguridad)

RETURN 0