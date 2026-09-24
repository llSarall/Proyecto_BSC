USE database_bsc;
GO

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