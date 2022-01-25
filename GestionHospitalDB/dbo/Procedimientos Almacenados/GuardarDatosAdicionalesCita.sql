﻿CREATE PROCEDURE [dbo].[GuardarDatosAdicionalesCita]
	@i_id_cita					int,
	@i_diagnostico				varchar(300) = NULL,
	@i_examenes					varchar(300) = NULL,
	@i_receta					varchar(300) = NULL,
	@i_id_estado				int,
	@i_fecha_proximo_control	datetime = NULL
AS

UPDATE	Cita
SET		Diagnostico = ISNULL(@i_diagnostico, Diagnostico),
		Examenes = ISNULL(@i_examenes, Examenes),
		Receta = ISNULL(@i_receta, Receta),
		IdEstado = ISNULL(@i_id_estado, IdEstado),
		FechaProximoControl = ISNULL(@i_fecha_proximo_control, FechaProximoControl)
WHERE	IdCita = @i_id_cita

RETURN 0