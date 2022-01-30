CREATE PROCEDURE [dbo].[ConsultarTransaccionesUsuario]
	@i_nombre_usuario		varchar(30)
AS

SELECT	Transaccion.*
FROM	Usuario
		JOIN RolSeguridad ON Usuario.IdRolSeguridad = RolSeguridad.IdRolSeguridad
		JOIN TransaccionRolSeguridad ON RolSeguridad.IdRolSeguridad = TransaccionRolSeguridad.IdRolSeguridad
		JOIN Transaccion ON TransaccionRolSeguridad.IdTransaccion = Transaccion.IdTransaccion
WHERE	Usuario.LoginUsuario = @i_nombre_usuario
  AND	Transaccion.Estado = 1
ORDER BY Transaccion.IdTransaccion

RETURN 0