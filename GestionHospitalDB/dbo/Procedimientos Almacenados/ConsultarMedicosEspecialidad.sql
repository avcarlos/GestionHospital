CREATE PROCEDURE [dbo].[ConsultarMedicosEspecialidad]
	@i_id_especialidad		int
AS

SELECT	Persona.*,
		Persona.Nombres + ' ' + Persona.Apellidos	AS 'NombreMedico'
FROM	EspecialidadMedico
		JOIN Persona ON EspecialidadMedico.IdMedico = Persona.IdPersona
		JOIN TipoPersona ON Persona.IdPersona = TipoPersona.IdPersona
WHERE	EspecialidadMedico.IdEspecialidad = @i_id_especialidad
  AND	EspecialidadMedico.Estado = 1
  AND	TipoPersona.Estado = 1

RETURN 0