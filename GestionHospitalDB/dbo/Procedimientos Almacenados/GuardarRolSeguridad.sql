CREATE PROCEDURE [dbo].[GuardarRolSeguridad]
	@i_nombre			varchar(30),
	@i_descripcion		varchar(300),
	@i_id_usuario		int,
	@o_id_rol_seguridad	int out
AS

INSERT INTO RolSeguridad
(
	Nombre,
	Descripcion,
	Estado,
	IdUsuarioCreacion,
	FechaCreacion
)
VALUES
(
	@i_nombre,
	@i_descripcion,
	1,
	@i_id_usuario,
	GETDATE()
)

SET @o_id_rol_seguridad = SCOPE_IDENTITY()

RETURN 0