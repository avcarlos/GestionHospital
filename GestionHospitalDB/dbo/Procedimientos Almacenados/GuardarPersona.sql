CREATE PROCEDURE [dbo].[GuardarPersona]
	@i_identificacion			varchar(13),
	@i_id_tipo_identificacion	int,
	@i_nombres					varchar(50),
	@i_apellidos				varchar(50),
	@i_fecha_nacimiento			datetime = NULL,
	@i_telefono					varchar(12) = NULL,
	@i_celular					varchar(12) = NULL,
	@i_email					varchar(30) = NULL,
	@i_id_genero				int = NULL,
	@i_direccion				varchar(300) = NULL,
	@i_id_ciudad				int = NULL,
	@o_id_persona				int out
AS

INSERT INTO Persona
(
	Identificacion,
	IdTipoIdentificacion,
	Nombres,
	Apellidos,
	FechaNacimiento,
	Telefono,
	Celular,
	Email,
	IdGenero,
	Direccion,
	IdCiudad,
	Estado
)
VALUES
(
	@i_identificacion,
	@i_id_tipo_identificacion,
	@i_nombres,
	@i_apellidos,
	@i_fecha_nacimiento,
	@i_telefono,
	@i_celular,
	@i_email,
	@i_id_genero,
	@i_direccion,
	@i_id_ciudad,
	1
)

SET @o_id_persona = SCOPE_IDENTITY()

RETURN 0