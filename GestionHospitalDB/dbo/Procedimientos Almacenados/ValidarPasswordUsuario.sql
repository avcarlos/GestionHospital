CREATE PROCEDURE [dbo].[ValidarPasswordUsuario]
	@i_id_usuario			int,
	@i_password				varchar(300),
	@o_valido				bit out
AS

DECLARE	@w_password_usuario	varbinary(MAX)

SET	@o_valido = 0

SELECT	@w_password_usuario = PasswordUsuario
FROM	Usuario
WHERE	IdUsuario = @i_id_usuario

IF @w_password_usuario IS NOT NULL
BEGIN
	SET	@o_valido = PWDCOMPARE(@i_password, @w_password_usuario)
END

RETURN 0