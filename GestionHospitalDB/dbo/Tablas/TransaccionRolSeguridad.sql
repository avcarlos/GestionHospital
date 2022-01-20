CREATE TABLE [dbo].[TransaccionRolSeguridad]
(
	[IdTransaccionRolSeguridad]		INT Identity(1,1) NOT NULL,
	[IdRolSeguridad]				INT NOT NULL,
	[IdTransaccion]					INT NOT NULL,
	CONSTRAINT [PK_TransaccionRolSeguridad] PRIMARY KEY (IdTransaccionRolSeguridad),
	CONSTRAINT [FK_RolSeguridad_TransaccionRolSeguridad] FOREIGN KEY (IdRolSeguridad) REFERENCES RolSeguridad(IdRolSeguridad),
	CONSTRAINT [FK_Transaccuin_RolSeguridad] FOREIGN KEY (IdTransaccion) REFERENCES Transaccion(IdTransaccion)
)
