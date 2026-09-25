/* -----------------------------------------------------------
   Proyecto : BSC - Evaluación técnica BJXIT
   Script   : 07_datos_prueba.sql (7 de 7)
   Autor    : Alejandro Mejia Herrera
   Fecha    : 24/09/2026
   Descripción: Este Script nos sirve para cargar productos y pedidos de prueba para poder probar la app.
   -----------------------------------------------------------*/

USE database_bsc;
GO
/* Vamos a obtener los Id's de los usuarios por nombre para no tenerlos fijos*/
DECLARE @id_almacen  INT = (SELECT id_usuario FROM usuarios WHERE usuario = 'almacen01');
DECLARE @id_vendedor INT = (SELECT id_usuario FROM usuarios WHERE usuario = 'vendedor01');


/*Este es el insert de algunos productos de prueba para poder probar la app*/
INSERT INTO productos (clave_producto, nombre_producto, existencia, id_usuario_registro) VALUES
	('TOR01', 'Tornillo', 250, @id_almacen),
	('TUE01', 'Tuerca', 200, @id_almacen),
	('RON01', 'Rondana', 180, @id_almacen),
	('CLA01', 'Clavo', 100, @id_almacen),
	('TAQ01', 'Taquete', 0, @id_almacen);

/* Vamos a obtener los Id's de los productos por clave del producto para no tenerlos fijos*/
DECLARE @id_tornillo INT = (SELECT id_producto FROM productos WHERE clave_producto = 'TOR01');
DECLARE @id_tuerca   INT = (SELECT id_producto FROM productos WHERE clave_producto = 'TUE01');
DECLARE @id_rondana  INT = (SELECT id_producto FROM productos WHERE clave_producto = 'RON01');

/*Para la creación de los pedidos vamos a utilizar un procedimiento almacenado, son los mismos que usamos
en la app, ademas usaremos un trigger que descuenta la existencia de los productos cuando se realiza un pedido*/
EXEC sp_crear_pedido @nombre_cliente = 'Pedro Ramírez',     @id_usuario_registro = @id_vendedor,
                     @id_producto = @id_tornillo, @cantidad = 20;

EXEC sp_crear_pedido @nombre_cliente = 'Ferretería López',  @id_usuario_registro = @id_vendedor,
                     @id_producto = @id_tuerca,   @cantidad = 50;

EXEC sp_crear_pedido @nombre_cliente = 'María González',    @id_usuario_registro = @id_vendedor,
                     @id_producto = @id_rondana,  @cantidad = 30;

/*Vamos a verificar que se haya descontado la existencia de los productos*/
SELECT * FROM vw_reporte_existencias ORDER BY nombre_producto;
SELECT * FROM vw_pedidos ORDER BY fecha_registro_pedido DESC;