/* -----------------------------------------------------------
   Proyecto : BSC - Evaluación técnica BJXIT
   Script   : 05_vistas.sql (5 de 7)
   Autor    : Alejandro Mejia Herrera
   Fecha    : 24/09/2026
   Descripción: Este Script nos sirve para la creación de las vistas que usamos en la app para los reportes (existencia y pedidos)
   -----------------------------------------------------------*/


USE database_bsc;
GO


/*
Esta vista nos sirve para obtener el listado de productos con su existencia además agregué un estatus para que sea un poco más facil identificar cuáles ya no tiene existencia
*/
CREATE OR ALTER VIEW dbo.vw_reporte_existencias
AS
SELECT
    p.id_producto,
    p.clave_producto,
    p.nombre_producto,
    p.existencia,
    CASE
        WHEN p.existencia = 0 THEN 'Sin existencia'
        ELSE 'Disponible'
    END AS estatus_existencia
FROM productos p;
GO

/*
Esta vista nos sirve para obtener el listado de pedidos con su respectiva información como fecha, quien lo creo, cliente, el producto, su cantidad, etc
*/
CREATE OR ALTER VIEW dbo.vw_pedidos
AS
SELECT
    pe.id_pedido,
    pe.fecha_registro_pedido,
    pe.nombre_cliente,
    u.usuario            AS vendedor,
    pr.clave_producto,
    pr.nombre_producto,
    dp.cantidad
FROM pedidos pe
INNER JOIN usuarios u        ON u.id_usuario   = pe.id_usuario_registro
INNER JOIN detalle_pedido dp ON dp.id_pedido   = pe.id_pedido
INNER JOIN productos pr      ON pr.id_producto = dp.id_producto;
GO