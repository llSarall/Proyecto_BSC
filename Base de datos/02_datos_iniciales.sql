/* -----------------------------------------------------------
   Proyecto : BSC - Evaluación técnica BJXIT
   Script   : 02_datos_iniciales.sql (2 de 7)
   Autor    : Alejandro Mejia Herrera
   Fecha    : 24/09/2026
   Descripción: Este Script nos sirve para realizar la carga inicial de información mínima necesaria a la base de datos para que el proyecto funcione 
   -----------------------------------------------------------*/

USE database_bsc;
GO

/*Este insert sirve para cargar los perfiles/roles necesarios y deben insertarse en este orden para que coincidan con los insert de los usuarios*/
INSERT INTO perfiles (nombre_perfil) VALUES 
	('Administrador'),
	('Personal Administrativo'),
	('Vendedor');

				
/*Este es el insert de los usuarios con perfil/rol diferente para poder probar el sistema, el admin_01 es el unico usuario que puede crear mas usuarios, 
la contraseña no se guarda en texto plano sino como un hash generado desde la app, los usuarios y contraseñas reales esta en el README*/
INSERT INTO usuarios (usuario, id_perfil, password_hash) VALUES 
	('admin_01', 1, 'AQAAAAIAAYagAAAAEGEOMLtYgGpuSlXHCrLlXQdDxp/FjDMdGgoAjX9lZI1OqwtZjltig/+NpIpDCZhHKA=='),
	('almacen01', 2, 'AQAAAAIAAYagAAAAEIElE9914hRasBRjOJPMpVgjyu/+20ktXeRlJaOmLBhS+4aLKtyAttsD2uMq7nwgSA=='),
	('vendedor01', 3, 'AQAAAAIAAYagAAAAEBkCdPFPHb6XrORRILo5qXeAYHvbedyQ+/oSI7hxxMHZ9t28089dU0LWGJC4sXo2iA==');
