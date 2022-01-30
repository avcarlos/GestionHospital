CREATE PROCEDURE [dbo].[GuardarDetalleReceta]
	@i_id_receta		int,
	@i_id_medicamento	int,
	@i_indicaciones		varchar(150) = NULL
AS

INSERT INTO dbo.DetalleReceta
(
	IdReceta,
	IdMedicamento,
	Indicaciones
)
VALUES
(
	@i_id_receta,
	@i_id_medicamento,
	@i_indicaciones
)

RETURN 0