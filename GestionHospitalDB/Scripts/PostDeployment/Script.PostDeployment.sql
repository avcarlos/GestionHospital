/*
Plantilla de script posterior a la implementación							
--------------------------------------------------------------------------------------
 Este archivo contiene instrucciones de SQL que se anexarán al script de compilación.		
 Use la sintaxis de SQLCMD para incluir un archivo en el script posterior a la implementación.			
 Ejemplo:      :r .\miArchivo.sql								
 Use la sintaxis de SQLCMD para hacer referencia a una variable en el script posterior a la implementación.		
 Ejemplo:      :setvar TableName miTabla							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

DELETE FROM dbo.TransaccionRolSeguridad
DELETE FROM dbo.Transaccion

DELETE FROM dbo.DetalleCatalogo
DELETE FROM dbo.Catalogo

-- Transacciones
SET IDENTITY_INSERT dbo.Transaccion ON

IF NOT EXISTS (SELECT 1 FROM dbo.Transaccion WHERE IdTransaccion =   1) INSERT INTO dbo.Transaccion (IdTransaccion, Nombre, Estado, Menu) VALUES (  1, 'Personal Médico', 1, 'Administracion\PersonalMedico')
IF NOT EXISTS (SELECT 1 FROM dbo.Transaccion WHERE IdTransaccion =  11) INSERT INTO dbo.Transaccion (IdTransaccion, Nombre, Estado, Menu) VALUES ( 11, 'Especialidades', 1, 'Administracion\Especialidades')
IF NOT EXISTS (SELECT 1 FROM dbo.Transaccion WHERE IdTransaccion =  12) INSERT INTO dbo.Transaccion (IdTransaccion, Nombre, Estado, Menu) VALUES ( 12, 'Catálogos', 1, 'Administracion\Catalogos')
IF NOT EXISTS (SELECT 1 FROM dbo.Transaccion WHERE IdTransaccion =  21) INSERT INTO dbo.Transaccion (IdTransaccion, Nombre, Estado, Menu) VALUES ( 21, 'Pacientes', 1, 'Administracion\Paciente')
IF NOT EXISTS (SELECT 1 FROM dbo.Transaccion WHERE IdTransaccion =  31) INSERT INTO dbo.Transaccion (IdTransaccion, Nombre, Estado, Menu) VALUES ( 31, 'Perfil Paciente', 1, 'Administracion\PacienteLinea')
IF NOT EXISTS (SELECT 1 FROM dbo.Transaccion WHERE IdTransaccion = 101) INSERT INTO dbo.Transaccion (IdTransaccion, Nombre, Estado, Menu) VALUES (101, 'Agenda Citas', 1, 'Procesos\AgendamientoCita')
IF NOT EXISTS (SELECT 1 FROM dbo.Transaccion WHERE IdTransaccion = 111) INSERT INTO dbo.Transaccion (IdTransaccion, Nombre, Estado, Menu) VALUES (111, 'Gestión Citas', 1, 'Procesos\AgendaCitas')
IF NOT EXISTS (SELECT 1 FROM dbo.Transaccion WHERE IdTransaccion = 201) INSERT INTO dbo.Transaccion (IdTransaccion, Nombre, Estado, Menu) VALUES (201, 'Estadísticas', 1, 'Consultas\Estadistica')
IF NOT EXISTS (SELECT 1 FROM dbo.Transaccion WHERE IdTransaccion = 901) INSERT INTO dbo.Transaccion (IdTransaccion, Nombre, Estado, Menu) VALUES (901, 'Roles', 1, 'Seguridad\Roles')
IF NOT EXISTS (SELECT 1 FROM dbo.Transaccion WHERE IdTransaccion = 911) INSERT INTO dbo.Transaccion (IdTransaccion, Nombre, Estado, Menu) VALUES (911, 'Permisos', 1, 'Seguridad\Permisos')
IF NOT EXISTS (SELECT 1 FROM dbo.Transaccion WHERE IdTransaccion = 921) INSERT INTO dbo.Transaccion (IdTransaccion, Nombre, Estado, Menu) VALUES (921, 'Usuarios', 1, 'Seguridad\Usuarios')

SET IDENTITY_INSERT dbo.Transaccion OFF

-- Roles Seguridad
SET IDENTITY_INSERT dbo.RolSeguridad ON

IF NOT EXISTS (SELECT 1 FROM dbo.RolSeguridad WHERE IdRolSeguridad = 1) INSERT INTO dbo.RolSeguridad (IdRolSeguridad, Nombre, Estado) VALUES (1, 'Administrador', 1)
IF NOT EXISTS (SELECT 1 FROM dbo.RolSeguridad WHERE IdRolSeguridad = 2) INSERT INTO dbo.RolSeguridad (IdRolSeguridad, Nombre, Estado) VALUES (2, 'Paciente', 1)
IF NOT EXISTS (SELECT 1 FROM dbo.RolSeguridad WHERE IdRolSeguridad = 3) INSERT INTO dbo.RolSeguridad (IdRolSeguridad, Nombre, Estado) VALUES (3, 'Tecnico1', 1)
IF NOT EXISTS (SELECT 1 FROM dbo.RolSeguridad WHERE IdRolSeguridad = 4) INSERT INTO dbo.RolSeguridad (IdRolSeguridad, Nombre, Estado) VALUES (4, 'Tecnico2', 1)
IF NOT EXISTS (SELECT 1 FROM dbo.RolSeguridad WHERE IdRolSeguridad = 5) INSERT INTO dbo.RolSeguridad (IdRolSeguridad, Nombre, Estado) VALUES (5, 'Medico', 1)
IF NOT EXISTS (SELECT 1 FROM dbo.RolSeguridad WHERE IdRolSeguridad = 6) INSERT INTO dbo.RolSeguridad (IdRolSeguridad, Nombre, Estado) VALUES (6, 'Auditor', 1)

SET IDENTITY_INSERT dbo.RolSeguridad OFF

-- Transacciones RolSeguridad

SET IDENTITY_INSERT dbo.TransaccionRolSeguridad ON

IF NOT EXISTS (SELECT 1 FROM dbo.TransaccionRolSeguridad WHERE IdRolSeguridad = 1 AND IdTransaccion =   1) INSERT INTO dbo.TransaccionRolSeguridad (IdTransaccionRolSeguridad, IdRolSeguridad, IdTransaccion) VALUES ( 1, 1,   1)
IF NOT EXISTS (SELECT 1 FROM dbo.TransaccionRolSeguridad WHERE IdRolSeguridad = 1 AND IdTransaccion =  11) INSERT INTO dbo.TransaccionRolSeguridad (IdTransaccionRolSeguridad, IdRolSeguridad, IdTransaccion) VALUES ( 2, 1,  11)
IF NOT EXISTS (SELECT 1 FROM dbo.TransaccionRolSeguridad WHERE IdRolSeguridad = 1 AND IdTransaccion =  12) INSERT INTO dbo.TransaccionRolSeguridad (IdTransaccionRolSeguridad, IdRolSeguridad, IdTransaccion) VALUES ( 3, 1,  12)
IF NOT EXISTS (SELECT 1 FROM dbo.TransaccionRolSeguridad WHERE IdRolSeguridad = 1 AND IdTransaccion =  21) INSERT INTO dbo.TransaccionRolSeguridad (IdTransaccionRolSeguridad, IdRolSeguridad, IdTransaccion) VALUES ( 4, 1,  21)
IF NOT EXISTS (SELECT 1 FROM dbo.TransaccionRolSeguridad WHERE IdRolSeguridad = 1 AND IdTransaccion =  31) INSERT INTO dbo.TransaccionRolSeguridad (IdTransaccionRolSeguridad, IdRolSeguridad, IdTransaccion) VALUES ( 5, 1,  31)
IF NOT EXISTS (SELECT 1 FROM dbo.TransaccionRolSeguridad WHERE IdRolSeguridad = 1 AND IdTransaccion = 101) INSERT INTO dbo.TransaccionRolSeguridad (IdTransaccionRolSeguridad, IdRolSeguridad, IdTransaccion) VALUES ( 6, 1, 101)
IF NOT EXISTS (SELECT 1 FROM dbo.TransaccionRolSeguridad WHERE IdRolSeguridad = 1 AND IdTransaccion = 111) INSERT INTO dbo.TransaccionRolSeguridad (IdTransaccionRolSeguridad, IdRolSeguridad, IdTransaccion) VALUES ( 7, 1, 111)
IF NOT EXISTS (SELECT 1 FROM dbo.TransaccionRolSeguridad WHERE IdRolSeguridad = 1 AND IdTransaccion = 201) INSERT INTO dbo.TransaccionRolSeguridad (IdTransaccionRolSeguridad, IdRolSeguridad, IdTransaccion) VALUES ( 8, 1, 201)
IF NOT EXISTS (SELECT 1 FROM dbo.TransaccionRolSeguridad WHERE IdRolSeguridad = 1 AND IdTransaccion = 901) INSERT INTO dbo.TransaccionRolSeguridad (IdTransaccionRolSeguridad, IdRolSeguridad, IdTransaccion) VALUES ( 9, 1, 901)
IF NOT EXISTS (SELECT 1 FROM dbo.TransaccionRolSeguridad WHERE IdRolSeguridad = 1 AND IdTransaccion = 911) INSERT INTO dbo.TransaccionRolSeguridad (IdTransaccionRolSeguridad, IdRolSeguridad, IdTransaccion) VALUES (10, 1, 911)
IF NOT EXISTS (SELECT 1 FROM dbo.TransaccionRolSeguridad WHERE IdRolSeguridad = 1 AND IdTransaccion = 921) INSERT INTO dbo.TransaccionRolSeguridad (IdTransaccionRolSeguridad, IdRolSeguridad, IdTransaccion) VALUES (11, 1, 921)

IF NOT EXISTS (SELECT 1 FROM dbo.TransaccionRolSeguridad WHERE IdRolSeguridad = 2 AND IdTransaccion =  31) INSERT INTO dbo.TransaccionRolSeguridad (IdTransaccionRolSeguridad, IdRolSeguridad, IdTransaccion) VALUES (12, 2,  31)
IF NOT EXISTS (SELECT 1 FROM dbo.TransaccionRolSeguridad WHERE IdRolSeguridad = 2 AND IdTransaccion = 101) INSERT INTO dbo.TransaccionRolSeguridad (IdTransaccionRolSeguridad, IdRolSeguridad, IdTransaccion) VALUES (13, 2, 101)

IF NOT EXISTS (SELECT 1 FROM dbo.TransaccionRolSeguridad WHERE IdRolSeguridad = 3 AND IdTransaccion = 101) INSERT INTO dbo.TransaccionRolSeguridad (IdTransaccionRolSeguridad, IdRolSeguridad, IdTransaccion) VALUES (14, 3, 101)

IF NOT EXISTS (SELECT 1 FROM dbo.TransaccionRolSeguridad WHERE IdRolSeguridad = 4 AND IdTransaccion = 101) INSERT INTO dbo.TransaccionRolSeguridad (IdTransaccionRolSeguridad, IdRolSeguridad, IdTransaccion) VALUES (15, 4, 101)

IF NOT EXISTS (SELECT 1 FROM dbo.TransaccionRolSeguridad WHERE IdRolSeguridad = 5 AND IdTransaccion = 111) INSERT INTO dbo.TransaccionRolSeguridad (IdTransaccionRolSeguridad, IdRolSeguridad, IdTransaccion) VALUES (16, 5, 111)

IF NOT EXISTS (SELECT 1 FROM dbo.TransaccionRolSeguridad WHERE IdRolSeguridad = 6 AND IdTransaccion =   1) INSERT INTO dbo.TransaccionRolSeguridad (IdTransaccionRolSeguridad, IdRolSeguridad, IdTransaccion) VALUES (17, 6,   1)
IF NOT EXISTS (SELECT 1 FROM dbo.TransaccionRolSeguridad WHERE IdRolSeguridad = 6 AND IdTransaccion =  11) INSERT INTO dbo.TransaccionRolSeguridad (IdTransaccionRolSeguridad, IdRolSeguridad, IdTransaccion) VALUES (18, 6,  11)
IF NOT EXISTS (SELECT 1 FROM dbo.TransaccionRolSeguridad WHERE IdRolSeguridad = 6 AND IdTransaccion =  12) INSERT INTO dbo.TransaccionRolSeguridad (IdTransaccionRolSeguridad, IdRolSeguridad, IdTransaccion) VALUES (19, 6,  12)
IF NOT EXISTS (SELECT 1 FROM dbo.TransaccionRolSeguridad WHERE IdRolSeguridad = 6 AND IdTransaccion =  21) INSERT INTO dbo.TransaccionRolSeguridad (IdTransaccionRolSeguridad, IdRolSeguridad, IdTransaccion) VALUES (20, 6,  21)
IF NOT EXISTS (SELECT 1 FROM dbo.TransaccionRolSeguridad WHERE IdRolSeguridad = 6 AND IdTransaccion =  31) INSERT INTO dbo.TransaccionRolSeguridad (IdTransaccionRolSeguridad, IdRolSeguridad, IdTransaccion) VALUES (21, 6,  31)
IF NOT EXISTS (SELECT 1 FROM dbo.TransaccionRolSeguridad WHERE IdRolSeguridad = 6 AND IdTransaccion = 101) INSERT INTO dbo.TransaccionRolSeguridad (IdTransaccionRolSeguridad, IdRolSeguridad, IdTransaccion) VALUES (22, 6, 101)
IF NOT EXISTS (SELECT 1 FROM dbo.TransaccionRolSeguridad WHERE IdRolSeguridad = 6 AND IdTransaccion = 111) INSERT INTO dbo.TransaccionRolSeguridad (IdTransaccionRolSeguridad, IdRolSeguridad, IdTransaccion) VALUES (23, 6, 111)
IF NOT EXISTS (SELECT 1 FROM dbo.TransaccionRolSeguridad WHERE IdRolSeguridad = 6 AND IdTransaccion = 201) INSERT INTO dbo.TransaccionRolSeguridad (IdTransaccionRolSeguridad, IdRolSeguridad, IdTransaccion) VALUES (24, 6, 201)
IF NOT EXISTS (SELECT 1 FROM dbo.TransaccionRolSeguridad WHERE IdRolSeguridad = 6 AND IdTransaccion = 901) INSERT INTO dbo.TransaccionRolSeguridad (IdTransaccionRolSeguridad, IdRolSeguridad, IdTransaccion) VALUES (25, 6, 901)
IF NOT EXISTS (SELECT 1 FROM dbo.TransaccionRolSeguridad WHERE IdRolSeguridad = 6 AND IdTransaccion = 911) INSERT INTO dbo.TransaccionRolSeguridad (IdTransaccionRolSeguridad, IdRolSeguridad, IdTransaccion) VALUES (26, 6, 911)
IF NOT EXISTS (SELECT 1 FROM dbo.TransaccionRolSeguridad WHERE IdRolSeguridad = 6 AND IdTransaccion = 921) INSERT INTO dbo.TransaccionRolSeguridad (IdTransaccionRolSeguridad, IdRolSeguridad, IdTransaccion) VALUES (27, 6, 921)

SET IDENTITY_INSERT dbo.TransaccionRolSeguridad OFF

-- Usuarios
SET IDENTITY_INSERT dbo.Usuario ON

IF NOT EXISTS (SELECT 1 FROM dbo.Usuario WHERE IdUsuario = 1) INSERT INTO dbo.Usuario (IdUsuario, LoginUsuario, IdRolSeguridad, Estado) VALUES (1, 'administrador@pruebas.com', 1, 1)

SET IDENTITY_INSERT dbo.Usuario OFF

-- Catálogos

SET IDENTITY_INSERT dbo.Catalogo ON
   
IF NOT EXISTS (SELECT 1 FROM dbo.Catalogo WHERE IdCatalogo = 1) INSERT INTO dbo.Catalogo (IdCatalogo, Nombre, Estado) VALUES (1, 'Género', 1)
IF NOT EXISTS (SELECT 1 FROM dbo.Catalogo WHERE IdCatalogo = 2) INSERT INTO dbo.Catalogo (IdCatalogo, Nombre, Estado) VALUES (2, 'Tipo Persona', 1)
IF NOT EXISTS (SELECT 1 FROM dbo.Catalogo WHERE IdCatalogo = 3) INSERT INTO dbo.Catalogo (IdCatalogo, Nombre, Estado) VALUES (3, 'Tipo Identificación', 1)
IF NOT EXISTS (SELECT 1 FROM dbo.Catalogo WHERE IdCatalogo = 4) INSERT INTO dbo.Catalogo (IdCatalogo, Nombre, Estado) VALUES (4, 'Estado Cita', 1)
IF NOT EXISTS (SELECT 1 FROM dbo.Catalogo WHERE IdCatalogo = 5) INSERT INTO dbo.Catalogo (IdCatalogo, Nombre, Estado) VALUES (5, 'Ciudad', 1)
IF NOT EXISTS (SELECT 1 FROM dbo.Catalogo WHERE IdCatalogo = 6) INSERT INTO dbo.Catalogo (IdCatalogo, Nombre, Estado) VALUES (6, 'Tipo Horario', 1)
IF NOT EXISTS (SELECT 1 FROM dbo.Catalogo WHERE IdCatalogo = 101) INSERT INTO dbo.Catalogo (IdCatalogo, Nombre, Estado, Administrable) VALUES (101, 'Medicamentos', 1, 1)

SET IDENTITY_INSERT dbo.Catalogo OFF

SET IDENTITY_INSERT dbo.DetalleCatalogo ON

-- Género
IF NOT EXISTS (SELECT 1 FROM dbo.DetalleCatalogo WHERE IdDetalleCatalogo = 1) INSERT INTO dbo.DetalleCatalogo (IdDetalleCatalogo, IdCatalogo, Nombre, Codigo, Estado) VALUES (1, 1, 'Masculino', 'M', 1)
IF NOT EXISTS (SELECT 1 FROM dbo.DetalleCatalogo WHERE IdDetalleCatalogo = 2) INSERT INTO dbo.DetalleCatalogo (IdDetalleCatalogo, IdCatalogo, Nombre, Codigo, Estado) VALUES (2, 1, 'Femenino', 'F', 1)

-- Tipo Persona
IF NOT EXISTS (SELECT 1 FROM dbo.DetalleCatalogo WHERE IdDetalleCatalogo = 5) INSERT INTO dbo.DetalleCatalogo (IdDetalleCatalogo, IdCatalogo, Nombre, Codigo, Estado) VALUES (5, 2, 'Medico', 'MED', 1)
IF NOT EXISTS (SELECT 1 FROM dbo.DetalleCatalogo WHERE IdDetalleCatalogo = 6) INSERT INTO dbo.DetalleCatalogo (IdDetalleCatalogo, IdCatalogo, Nombre, Codigo, Estado) VALUES (6, 2, 'Paciente', 'PAC', 1)

-- Tipo Identificación
IF NOT EXISTS (SELECT 1 FROM dbo.DetalleCatalogo WHERE IdDetalleCatalogo = 11) INSERT INTO dbo.DetalleCatalogo (IdDetalleCatalogo, IdCatalogo, Nombre, Codigo, Estado, Parametro1) VALUES (11, 3, 'Cédula', 'C', 1, 1)
IF NOT EXISTS (SELECT 1 FROM dbo.DetalleCatalogo WHERE IdDetalleCatalogo = 12) INSERT INTO dbo.DetalleCatalogo (IdDetalleCatalogo, IdCatalogo, Nombre, Codigo, Estado, Parametro1) VALUES (12, 3, 'Pasaporte', 'P', 1, 1)
IF NOT EXISTS (SELECT 1 FROM dbo.DetalleCatalogo WHERE IdDetalleCatalogo = 13) INSERT INTO dbo.DetalleCatalogo (IdDetalleCatalogo, IdCatalogo, Nombre, Codigo, Estado, Parametro1) VALUES (13, 3, 'RUC', 'R', 1, 0)

-- Estado Cita
IF NOT EXISTS (SELECT 1 FROM dbo.DetalleCatalogo WHERE IdDetalleCatalogo = 15) INSERT INTO dbo.DetalleCatalogo (IdDetalleCatalogo, IdCatalogo, Nombre, Codigo, Estado) VALUES (15, 4, 'Pendiente', 'PE', 1)
IF NOT EXISTS (SELECT 1 FROM dbo.DetalleCatalogo WHERE IdDetalleCatalogo = 16) INSERT INTO dbo.DetalleCatalogo (IdDetalleCatalogo, IdCatalogo, Nombre, Codigo, Estado) VALUES (16, 4, 'Atendido', 'AT', 1)
IF NOT EXISTS (SELECT 1 FROM dbo.DetalleCatalogo WHERE IdDetalleCatalogo = 17) INSERT INTO dbo.DetalleCatalogo (IdDetalleCatalogo, IdCatalogo, Nombre, Codigo, Estado) VALUES (17, 4, 'Cancelado', 'CA', 1)

-- Ciudad
IF NOT EXISTS (SELECT 1 FROM dbo.DetalleCatalogo WHERE IdDetalleCatalogo = 21) INSERT INTO dbo.DetalleCatalogo (IdDetalleCatalogo, IdCatalogo, Nombre, Codigo, Estado) VALUES (21, 5, 'Quito', 'UIO', 1)
IF NOT EXISTS (SELECT 1 FROM dbo.DetalleCatalogo WHERE IdDetalleCatalogo = 22) INSERT INTO dbo.DetalleCatalogo (IdDetalleCatalogo, IdCatalogo, Nombre, Codigo, Estado) VALUES (22, 5, 'Guayaquil', 'GYE', 1)
IF NOT EXISTS (SELECT 1 FROM dbo.DetalleCatalogo WHERE IdDetalleCatalogo = 23) INSERT INTO dbo.DetalleCatalogo (IdDetalleCatalogo, IdCatalogo, Nombre, Codigo, Estado) VALUES (23, 5, 'Cuenca', 'CUE', 1)
IF NOT EXISTS (SELECT 1 FROM dbo.DetalleCatalogo WHERE IdDetalleCatalogo = 24) INSERT INTO dbo.DetalleCatalogo (IdDetalleCatalogo, IdCatalogo, Nombre, Codigo, Estado) VALUES (24, 5, 'Ambato', 'AMB', 1)
IF NOT EXISTS (SELECT 1 FROM dbo.DetalleCatalogo WHERE IdDetalleCatalogo = 25) INSERT INTO dbo.DetalleCatalogo (IdDetalleCatalogo, IdCatalogo, Nombre, Codigo, Estado) VALUES (25, 5, 'Manta', 'MAN', 1)

-- Tipo Horario
IF NOT EXISTS (SELECT 1 FROM dbo.DetalleCatalogo WHERE IdDetalleCatalogo = 101) INSERT INTO dbo.DetalleCatalogo (IdDetalleCatalogo, IdCatalogo, Nombre, Codigo, Estado) VALUES (101, 6, 'Medico', 'MED', 1)
IF NOT EXISTS (SELECT 1 FROM dbo.DetalleCatalogo WHERE IdDetalleCatalogo = 102) INSERT INTO dbo.DetalleCatalogo (IdDetalleCatalogo, IdCatalogo, Nombre, Codigo, Estado) VALUES (102, 6, 'Cita', 'CIT', 1)

-- Medicamentos
IF NOT EXISTS (SELECT 1 FROM dbo.DetalleCatalogo WHERE IdDetalleCatalogo = 1001) INSERT INTO dbo.DetalleCatalogo (IdDetalleCatalogo, IdCatalogo, Nombre, Codigo, Estado) VALUES (1001, 101, 'Medicamento 1', 'ME1', 1)
IF NOT EXISTS (SELECT 1 FROM dbo.DetalleCatalogo WHERE IdDetalleCatalogo = 1002) INSERT INTO dbo.DetalleCatalogo (IdDetalleCatalogo, IdCatalogo, Nombre, Codigo, Estado) VALUES (1002, 101, 'Medicamento 2', 'ME2', 1)

SET IDENTITY_INSERT dbo.DetalleCatalogo OFF

-- Especialidades

SET IDENTITY_INSERT dbo.Especialidad ON

IF NOT EXISTS (SELECT 1 FROM dbo.Especialidad WHERE IdEspecialidad = 1) INSERT INTO dbo.Especialidad (IdEspecialidad, Nombre, Estado, IdUsuarioRegistro, FechaRegistro) VALUES (1, 'Medicina Interna', 1, 1, '2022-01-01')
IF NOT EXISTS (SELECT 1 FROM dbo.Especialidad WHERE IdEspecialidad = 2) INSERT INTO dbo.Especialidad (IdEspecialidad, Nombre, Estado, IdUsuarioRegistro, FechaRegistro) VALUES (2, 'Pediatría', 1, 1, '2022-01-01')
IF NOT EXISTS (SELECT 1 FROM dbo.Especialidad WHERE IdEspecialidad = 3) INSERT INTO dbo.Especialidad (IdEspecialidad, Nombre, Estado, IdUsuarioRegistro, FechaRegistro) VALUES (3, 'Endocrinología', 1, 1, '2022-01-01')
IF NOT EXISTS (SELECT 1 FROM dbo.Especialidad WHERE IdEspecialidad = 4) INSERT INTO dbo.Especialidad (IdEspecialidad, Nombre, Estado, IdUsuarioRegistro, FechaRegistro) VALUES (4, 'Ginecología', 1, 1, '2022-01-01')

SET IDENTITY_INSERT dbo.Especialidad OFF

-- Horarios

SET IDENTITY_INSERT dbo.Horario ON

IF NOT EXISTS (SELECT 1 FROM dbo.Horario WHERE IdHorario = 1) INSERT INTO dbo.Horario (IdHorario, Nombre, HoraInicio, HoraFin, IdTipoHorario, Estado) VALUES (1, '9:00 - 12:00', '09:00:00', '12:00:00', 101, 1)
IF NOT EXISTS (SELECT 1 FROM dbo.Horario WHERE IdHorario = 2) INSERT INTO dbo.Horario (IdHorario, Nombre, HoraInicio, HoraFin, IdTipoHorario, Estado) VALUES (2, '16:00 - 18:00', '16:00:00', '18:00:00', 101, 1)

IF NOT EXISTS (SELECT 1 FROM dbo.Horario WHERE IdHorario = 3) INSERT INTO dbo.Horario (IdHorario, Nombre, HoraInicio, HoraFin, IdTipoHorario, Estado) VALUES (3, '9:00 - 9:30', '09:00:00', '09:30:00', 102, 1)
IF NOT EXISTS (SELECT 1 FROM dbo.Horario WHERE IdHorario = 4) INSERT INTO dbo.Horario (IdHorario, Nombre, HoraInicio, HoraFin, IdTipoHorario, Estado) VALUES (4, '9:30 - 10:00', '09:30:00', '10:00:00', 102, 1)
IF NOT EXISTS (SELECT 1 FROM dbo.Horario WHERE IdHorario = 5) INSERT INTO dbo.Horario (IdHorario, Nombre, HoraInicio, HoraFin, IdTipoHorario, Estado) VALUES (5, '10:00 - 10:30', '10:00:00', '10:30:00', 102, 1)
IF NOT EXISTS (SELECT 1 FROM dbo.Horario WHERE IdHorario = 6) INSERT INTO dbo.Horario (IdHorario, Nombre, HoraInicio, HoraFin, IdTipoHorario, Estado) VALUES (6, '10:30 - 11:00', '10:30:00', '11:00:00', 102, 1)
IF NOT EXISTS (SELECT 1 FROM dbo.Horario WHERE IdHorario = 7) INSERT INTO dbo.Horario (IdHorario, Nombre, HoraInicio, HoraFin, IdTipoHorario, Estado) VALUES (7, '11:00 - 11:30', '11:00:00', '11:30:00', 102, 1)
IF NOT EXISTS (SELECT 1 FROM dbo.Horario WHERE IdHorario = 8) INSERT INTO dbo.Horario (IdHorario, Nombre, HoraInicio, HoraFin, IdTipoHorario, Estado) VALUES (8, '11:30 - 12:00', '11:30:00', '12:00:00', 102, 1)

IF NOT EXISTS (SELECT 1 FROM dbo.Horario WHERE IdHorario = 9) INSERT INTO dbo.Horario (IdHorario, Nombre, HoraInicio, HoraFin, IdTipoHorario, Estado) VALUES (9, '16:00 - 16:30', '16:00:00', '16:30:00', 102, 1)
IF NOT EXISTS (SELECT 1 FROM dbo.Horario WHERE IdHorario = 10) INSERT INTO dbo.Horario (IdHorario, Nombre, HoraInicio, HoraFin, IdTipoHorario, Estado) VALUES (10, '16:30 - 17:00', '16:30:00', '17:00:00', 102, 1)
IF NOT EXISTS (SELECT 1 FROM dbo.Horario WHERE IdHorario = 11) INSERT INTO dbo.Horario (IdHorario, Nombre, HoraInicio, HoraFin, IdTipoHorario, Estado) VALUES (11, '17:00 - 17:30', '17:00:00', '17:30:00', 102, 1)
IF NOT EXISTS (SELECT 1 FROM dbo.Horario WHERE IdHorario = 12) INSERT INTO dbo.Horario (IdHorario, Nombre, HoraInicio, HoraFin, IdTipoHorario, Estado) VALUES (12, '17:30 - 18:00', '17:30:00', '18:00:00', 102, 1)

SET IDENTITY_INSERT dbo.Horario OFF