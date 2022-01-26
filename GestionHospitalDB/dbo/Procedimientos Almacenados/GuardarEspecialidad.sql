CREATE PROCEDURE [dbo].[GuardarEspecialidad]
	@i_nombre				varchar(30),
	@i_descripcion			varchar(300),
	@i_id_usuario_registro	int
AS

INSERT INTO Especialidad
(
	Nombre,
	Descripcion,
	IdUsuarioRegistro,
	FechaRegistro
)
VALUES
(
	@i_nombre,
	@i_descripcion,
	@i_id_usuario_registro,
	GETDATE()
)

RETURN 0