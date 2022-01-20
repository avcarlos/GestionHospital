﻿CREATE PROCEDURE [dbo].[GuardarPersona]
	@i_identificacion		varchar(13),
	@i_id_tipo_identicacion	int,
	@i_nombres				varchar(50),
	@i_apellidos			varchar(50),
	@i_fecha_nacimiento		datetime = NULL,
	@i_telefono				varchar(12) = NULL,
	@i_celular				varchar(12) = NULL,
	@i_email				varchar(30) = NULL,
	@i_id_genero			int = NULL,
	@i_direccion			varchar(300) = NULL,
	@i_id_ciudad			int = NULL
AS

INSERT INTO Persona
(
	Identificacion,
	IdTipoIdenticacion,
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
	@i_id_tipo_identicacion,
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

RETURN 0