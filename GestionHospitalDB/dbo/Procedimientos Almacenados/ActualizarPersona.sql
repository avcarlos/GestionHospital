CREATE PROCEDURE [dbo].[ActualizarPersona]
	@i_id_persona			int,
	@i_fecha_nacimiento		datetime = NULL,
	@i_telefono				varchar(12) = NULL,
	@i_celular				varchar(12) = NULL,
	@i_email				varchar(30) = NULL,
	@i_id_genero			int = NULL,
	@i_direccion			varchar(300) = NULL,
	@i_id_ciudad			int = NULL
AS

UPDATE	dbo.Persona
SET		FechaNacimiento = ISNULL(@i_fecha_nacimiento, FechaNacimiento),
		Telefono = ISNULL(@i_telefono, Telefono),
		Celular = ISNULL(@i_celular, Celular),
		Email = ISNULL(@i_email, Email),
		IdGenero = ISNULL(@i_id_genero, IdGenero),
		Direccion = ISNULL(@i_direccion, Direccion),
		IdCiudad = ISNULL(@i_id_ciudad, IdCiudad)
WHERE	IdPersona = @i_id_persona

RETURN 0