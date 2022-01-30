CREATE PROCEDURE [dbo].[GuardarEspecialidad]
	@i_nombre				varchar(30),
	@i_descripcion			varchar(300),
	@i_id_usuario_registro	int
AS

INSERT INTO Especialidad
(
	Nombre,
	Descripcion,
	Estado,
	IdUsuarioRegistro,
	FechaRegistro
)
VALUES
(
	@i_nombre,
	@i_descripcion,
	1,
	@i_id_usuario_registro,
	GETDATE()
)

RETURN 0