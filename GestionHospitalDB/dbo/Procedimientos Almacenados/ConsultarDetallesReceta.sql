CREATE PROCEDURE [dbo].[ConsultarDetallesReceta]
	@i_id_receta		int
AS

SELECT	DetalleReceta.*,
		DetalleCatalogo.Nombre	AS 'Medicamento'
FROM	dbo.DetalleReceta
		JOIN dbo.DetalleCatalogo ON DetalleReceta.IdMedicamento = DetalleCatalogo.IdDetalleCatalogo
WHERE	IdReceta = @i_id_receta

RETURN 0