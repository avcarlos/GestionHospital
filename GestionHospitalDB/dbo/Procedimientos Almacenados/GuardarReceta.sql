CREATE PROCEDURE [dbo].[GuardarReceta]
	@i_id_receta		int = NULL,
	@i_id_cita			int,
	@i_observaciones	varchar(300),
	@i_usuario			int,
	@o_id_receta		int out
AS

IF @i_id_receta IS NULL
BEGIN
	INSERT INTO dbo.Receta
	(
		IdCita,
		Observaciones,
		IdUsuarioCreacion,
		FechaCreacion
	)
	VALUES
	(
		@i_id_cita,
		@i_observaciones,
		@i_usuario,
		GETDATE()
	)
	
	SET @o_id_receta = SCOPE_IDENTITY()
END
ELSE
BEGIN
	UPDATE	dbo.Receta
	SET		Observaciones = ISNULL(@i_observaciones, Observaciones)
	WHERE	IdReceta = @i_id_receta

	SET @o_id_receta = @i_id_cita
END	

RETURN 0