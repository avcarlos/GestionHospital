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

SET IDENTITY_INSERT dbo.Usuario ON

IF NOT EXISTS (SELECT 1 FROM dbo.Usuario WHERE us_id_usuario = 1) INSERT INTO dbo.Usuario (us_id_usuario, us_login) VALUES(1,'1')

SET IDENTITY_INSERT dbo.Usuario OFF