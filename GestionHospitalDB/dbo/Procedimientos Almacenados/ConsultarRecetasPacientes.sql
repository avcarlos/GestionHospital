CREATE PROCEDURE [dbo].[ConsultarRecetasPacientes]
	@i_id_paciente		int = NULL,
	@i_id_receta		int = NULL
AS

SELECT	Receta.*,
		medico.Nombres + ' ' + medico.Apellidos		AS 'NombreMedico',
		paciente.Nombres + ' ' + paciente.Apellidos	AS 'NombrePaciente',
		Cita.Fecha,
		Usuario.LoginUsuario,
		Cita.FechaModificacion,
		Especialidad.Nombre							AS 'NombreEspecialidad'
FROM	Receta
		JOIN Cita ON Receta.IdCita = Cita.IdCita
		JOIN EspecialidadMedico ON Cita.IdEspecialidadMedico = EspecialidadMedico.IdEspecialidadMedico
		JOIN Persona medico ON EspecialidadMedico.IdMedico = medico.IdPersona
		JOIN Persona paciente ON Cita.IdPaciente = paciente.IdPersona
		JOIN Usuario ON Cita.IdUsuarioModificacion = Usuario.IdUsuario
		JOIN Especialidad ON EspecialidadMedico.IdEspecialidad = Especialidad.IdEspecialidad
WHERE	Receta.IdReceta = ISNULL(@i_id_receta, Receta.IdReceta)
  AND	Cita.IdPaciente = ISNULL(@i_id_paciente, Cita.IdPaciente)

RETURN 0