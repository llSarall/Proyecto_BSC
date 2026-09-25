/* -----------------------------------------------------------
   Proyecto : BSC - Evaluación técnica BJXIT
   Script   : 06_indices.sql (6 de 7)
   Autor    : Alejandro Mejia Herrera
   Fecha    : 24/09/2026
   Descripción: Este script crea los índices para mejorar el rendimiento de las consultas de la app.
   -----------------------------------------------------------*/

USE database_bsc;
GO

/*
Índice sobre el perfil de los usuarios.
Ayuda al JOIN entre usuarios y perfiles que hacemos en el login (sp_obtener_usuario_por_nombre).
*/
CREATE NONCLUSTERED INDEX IX_usuarios_id_perfil
    ON dbo.usuarios (id_perfil);
GO

/*
Índice sobre el usuario que registró el pedido.
Ayuda al JOIN entre pedidos y usuarios de la vista vw_pedidos, para obtener el nombre del vendedor.
*/
CREATE NONCLUSTERED INDEX IX_pedidos_id_usuario_registro
    ON dbo.pedidos (id_usuario_registro);
GO

/*
Índice sobre la fecha de los pedidos, ordenado de más reciente a más antiguo.
Ayuda a la consulta de pedidos de la app, que los muestra ordenados por fecha descendente.
*/
CREATE NONCLUSTERED INDEX IX_pedidos_fecha
    ON dbo.pedidos (fecha_registro_pedido DESC);
GO

/*
Índice sobre el producto en el detalle de los pedidos.
Ayuda al JOIN entre detalle_pedido y productos de la vista vw_pedidos, y al trigger que descuenta la existencia.
Con INCLUDE (cantidad) es un índice de cobertura: guarda también la cantidad dentro del índice,
así las consultas que solo necesitan el producto y la cantidad no tienen que ir a la tabla.

Nota: no se crea un índice sobre detalle_pedido (id_pedido) porque la restricción
UQ_detalle_pedido_producto (id_pedido, id_producto) ya creó uno que empieza por esa columna,
y SQL Server lo puede usar para las búsquedas por pedido. Crear otro sería redundante.
*/
CREATE NONCLUSTERED INDEX IX_detalle_pedido_id_producto
    ON dbo.detalle_pedido (id_producto)
    INCLUDE (cantidad);
GO