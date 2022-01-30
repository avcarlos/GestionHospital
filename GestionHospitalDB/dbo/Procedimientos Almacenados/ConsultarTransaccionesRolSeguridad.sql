CREATE PROCEDURE [dbo].[ConsultarTransaccionesRolSeguridad]
	@i_id_rol_seguridad		int
AS

SELECT	Transaccion.*,
		TransaccionRolSeguridad.IdTransaccionRolSeguridad
FROM	TransaccionRolSeguridad
		JOIN Transaccion ON TransaccionRolSeguridad.IdTransaccion = Transaccion.IdTransaccion
WHERE	TransaccionRolSeguridad.IdRolSeguridad = @i_id_rol_seguridad
  AND	Transaccion.Estado = 1
ORDER BY Transaccion.IdTransaccion

RETURN 0