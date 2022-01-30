CREATE PROCEDURE [dbo].[ConsultarReceta]
	@i_id_receta		int = NULL,
	@i_id_cita			int = NULL
AS

SELECT	*
FROM	Receta
WHERE	IdReceta = ISNULL(@i_id_receta, IdReceta)
  AND	IdCita = ISNULL(@i_id_cita, IdCita)

RETURN 0