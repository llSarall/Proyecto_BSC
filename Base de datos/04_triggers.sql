/* -----------------------------------------------------------
   Proyecto : BSC - Evaluación técnica BJXIT
   Script   : 04_triggers.sql (4 de 7)
   Autor    : Alejandro Mejia Herrera
   Fecha    : 24/09/2026
   Descripción: Este Script nos sirve para la creación del trigger para descontar existencia cuando se hace un insert en la tabla de detalle_pedido
   -----------------------------------------------------------*/


USE database_bsc;
GO


/*
Este trigger se acciona cada que se crea una fila en la tabla de detalle de pedido, utiliza la tabla virtual inserted para saber con cual producto se hizo
el insert, lo agrupa por producto, suma la cantidad para después ir a la tabla de productos y modificar la existencia disponible (existencia - vendido)
*/
CREATE OR ALTER TRIGGER dbo.trg_detalle_pedido_descontar_existencia
ON dbo.detalle_pedido
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE p
    SET p.existencia = p.existencia - i.total_cantidad
    FROM productos p
    INNER JOIN (
        SELECT id_producto, SUM(cantidad) AS total_cantidad
        FROM inserted
        GROUP BY id_producto
    ) i ON p.id_producto = i.id_producto;
END
GO