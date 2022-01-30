CREATE PROCEDURE [dbo].[ConsultarTransacciones]
	@i_id_transaccion		int = NULL
AS
	
SELECT	*
FROM	dbo.Transaccion
WHERE	IdTransaccion = ISNULL(@i_id_transaccion, IdTransaccion)

RETURN 0