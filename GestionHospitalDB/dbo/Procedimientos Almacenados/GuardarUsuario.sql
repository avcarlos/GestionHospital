CREATE PROCEDURE [dbo].[GuardarUsuario]
	@i_nombre_usuario		varchar(30),
	@i_password				varchar(30) = NULL,
	@i_id_rol_seguridad		int,
	@i_id_persona			int = NULL,
	@o_id_usuario			int out
AS

INSERT INTO Usuario
(
	LoginUsuario,
	IdRolSeguridad,
	IdPersona,
	Estado
)
VALUES
(
	@i_nombre_usuario,
	@i_id_rol_seguridad,
	@i_id_persona,
	1
)

SET @o_id_usuario = SCOPE_IDENTITY()

IF @i_password IS NOT NULL
BEGIN
	UPDATE	Usuario
	SET		PasswordUsuario = PWDENCRYPT(@i_password)
	WHERE	IdUsuario = @o_id_usuario
END

RETURN 0