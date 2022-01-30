CREATE PROCEDURE [dbo].[GuardarTransaccionRolSeguridad]
	@i_id_rol_seguridad		int,
	@i_id_transaccion		int
AS

INSERT INTO TransaccionRolSeguridad
(
	IdRolSeguridad,
	IdTransaccion
)
VALUES
(
	@i_id_rol_seguridad,
	@i_id_transaccion
)

RETURN 0