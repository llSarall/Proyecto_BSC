USE database_bsc;
GO

CREATE OR ALTER PROCEDURE dbo.sp_crear_pedido
    @nombre_cliente      VARCHAR(255),
    @id_usuario_registro INT,
    @id_producto         INT,
    @cantidad            INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @existencia INT;
    DECLARE @id_pedido  INT;

    BEGIN TRY
        BEGIN TRANSACTION;

        SELECT @existencia = existencia
        FROM productos WITH (UPDLOCK, ROWLOCK)
        WHERE id_producto = @id_producto;

        IF @existencia IS NULL
            THROW 50001, 'El producto no existe.', 1;

        IF @existencia < @cantidad
            THROW 50002, 'Sin existencia', 1;

        INSERT INTO pedidos (nombre_cliente, id_usuario_registro)
        VALUES (@nombre_cliente, @id_usuario_registro);

        SET @id_pedido = SCOPE_IDENTITY();

        INSERT INTO detalle_pedido (id_pedido, id_producto, cantidad)
        VALUES (@id_pedido, @id_producto, @cantidad);

        COMMIT TRANSACTION;

        SELECT @id_pedido AS id_pedido;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        THROW;
    END CATCH
END
GO


CREATE OR ALTER PROCEDURE dbo.sp_obtener_productos
AS
BEGIN
    SET NOCOUNT ON;

    SELECT id_producto, clave_producto, nombre_producto,
           existencia, id_usuario_registro, fecha_registro_producto
    FROM productos
    ORDER BY nombre_producto;
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_crear_usuario
    @usuario       VARCHAR(50),
    @id_perfil     INT,
    @password_hash VARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO usuarios (usuario, id_perfil, password_hash)
    VALUES (@usuario, @id_perfil, @password_hash);

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS id_usuario;
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_obtener_usuario_por_nombre
    @usuario VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT u.id_usuario,
           u.usuario AS nombre_usuario,
           u.id_perfil,
           p.nombre_perfil,
           u.password_hash,
           u.estatus
    FROM usuarios u
    INNER JOIN perfiles p ON p.id_perfil = u.id_perfil
    WHERE u.usuario = @usuario;
END
GO


CREATE OR ALTER PROCEDURE dbo.sp_registrar_producto
    @clave_producto      VARCHAR(50),
    @nombre_producto     VARCHAR(50),
    @existencia          INT,
    @id_usuario_registro INT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO productos (clave_producto, nombre_producto, existencia, id_usuario_registro)
    VALUES (@clave_producto, @nombre_producto, @existencia, @id_usuario_registro);

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS id_producto;
END
GO