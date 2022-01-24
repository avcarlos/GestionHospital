CREATE PROCEDURE [dbo].[GuardarUsuario]
	@i_nombre_usuario		varchar(30),
	@i_contrasena			varchar(20) = NULL,
	@i_id_rol_seguridad		int,
	@i_id_persona			int = NULL,
	@o_id_usuario			int out
AS

INSERT INTO Usuario
(
	LoginUsuario,
	PasswordUsuario,
	IdRolSeguridad,
	IdPersona,
	Estado
)
VALUES
(
	@i_nombre_usuario,
	@i_contrasena,
	@i_id_rol_seguridad,
	@i_id_persona,
	1
)

SET @o_id_usuario = SCOPE_IDENTITY()

RETURN 0