CREATE PROCEDURE [dbo].[GuardarDatosAdicionalesCita]
	@i_id_cita					int,
	@i_diagnostico				varchar(300) = NULL,
	@i_id_estado				int,
	@i_fecha_proximo_control	datetime = NULL,
	@i_id_usuario				int
AS

UPDATE	Cita
SET		Diagnostico = ISNULL(@i_diagnostico, Diagnostico),
		IdEstado = ISNULL(@i_id_estado, IdEstado),
		FechaProximoControl = ISNULL(@i_fecha_proximo_control, FechaProximoControl),
		IdUsuarioModificacion = @i_id_usuario,
		FechaModificacion = GETDATE()
WHERE	IdCita = @i_id_cita

RETURN 0