USE database_bsc;
GO

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