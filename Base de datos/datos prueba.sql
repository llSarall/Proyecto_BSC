SELECT * FROM perfiles
SELECT * FROM usuarios;   /* 3 */
SELECT * FROM productos;


SELECT * FROM pedidos;
/*DELETE FROM pedidos;*/
SELECT * FROM detalle_pedido;
/*DELETE FROM detalle_pedido;*/

/*
INSERT INTO usuarios (usuario, id_perfil, password_hash, estatus) VALUES ('Alejandro', 2, 'Temporal', 1);
INSERT INTO usuarios (usuario, id_perfil, password_hash, estatus) VALUES ('Alex', 1, 'Temporal2', 1);
INSERT INTO usuarios (usuario, id_perfil, password_hash, estatus) VALUES ('Jandro', 3, 'Temporal3', 1);
*/


/*
INSERT INTO productos (clave_producto, nombre_producto, id_usuario_registro, fecha_registro_producto) VALUES ('TOR01', 'Tornillo chico', 2, GETDATE());
INSERT INTO productos (clave_producto, nombre_producto, id_usuario_registro, fecha_registro_producto) VALUES ('TUE01', 'Tuerca chico', 2, GETDATE());
INSERT INTO productos (clave_producto, nombre_producto, id_usuario_registro, fecha_registro_producto) VALUES ('RON01', 'Rondana chico', 2, GETDATE());
INSERT INTO productos (clave_producto, nombre_producto, id_usuario_registro, fecha_registro_producto) VALUES ('TOR02', 'Tornillo mediano', 2, GETDATE());
INSERT INTO productos (clave_producto, nombre_producto, id_usuario_registro, fecha_registro_producto) VALUES ('TUE02', 'Tuerca mediana', 2, GETDATE());
INSERT INTO productos (clave_producto, nombre_producto, id_usuario_registro, fecha_registro_producto) VALUES ('RON02', 'Rondana mediana', 2, GETDATE());
*/


UPDATE productos SET existencia = 250 where id_producto in (6);


DECLARE @id_pedido INT;
INSERT INTO pedidos (nombre_cliente, id_usuario_registro, fecha_registro_pedido) VALUES ('Pedro', 850, GETDATE());
SET @id_pedido = SCOPE_IDENTITY();
INSERT INTO detalle_pedido (id_pedido, id_producto, cantidad) VALUES (@id_pedido, 2, 10), (@id_pedido, 1, 20), (@id_pedido, 3, 30);


