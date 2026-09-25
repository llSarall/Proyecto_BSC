/* -----------------------------------------------------------
   Proyecto : BSC - Evaluación técnica BJXIT
   Script   : 03_stored_procedures.sql (3 de 7)
   Autor    : Alejandro Mejia Herrera
   Fecha    : 24/09/2026
   Descripción: Este Script nos sirve para la creación de algunos procedimientos almacenados que usamos en la app
   -----------------------------------------------------------*/


USE database_bsc;
GO

/*
Descripción: Este procedimiento sirve para crear un pedido y en el hacemos distintas validaciones como que el producto ingresado exista en la
bd, asi como su existencia, en caso de que no haya existencia suficiente se notifica, tambien hace el insert en la tabla de detalle 
de los pedidos
Se utiliza: En la app al momento de que un vendedor realiza un pedido
Parametros necesarios: Nombre del cliente, usuario que lo registró, producto y cantidad del pedido
Otros: Se uso XACT_ABORT para que en caso de que haya algun error al ejecutarse no realice ningun insert a la base de datos 
       Se uso UPDLOCK y ROWLOCK para bloquear la fila del registro para que otro usuario no pueda utilizarla
*/
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
        BEGIN
            DECLARE @mensaje NVARCHAR(200) = CONCAT('Existencia insuficiente. Piezas disponibles: ', @existencia);
            THROW 50002, @mensaje, 1;
        END

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


/*
Descripción: Este procedimiento sirve para obtener el listado de productos creados asi como toda su información como existencia, quien lo creo y cuando
Se utiliza: En la app cuando un vendedor realiza un pedido aparece un catálogo de opciones con los productos en la base de datos
Parametros necesarios: NA
Otros: NA
*/
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

/*
Descripción: Este procedimiento sirve para la creación de usuarios
Se utiliza: En la app al momento de crear un nuevo usuario 
Parametros necesarios: Usuario, el id del perfil y hash de la contraseña
Otros: Retorna el id del usuario que se creó
*/
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


/*
Descripción: Este procedimiento sirve para obtener un usuario con su perfil y estatus
Se utiliza: En la app al momento de hacer login para tener toda la informacion del usuario logueado
Parametros necesarios: Usuario
Otros: Se hace un JOIN con la tabla de los perfiles para tener el detalle de que perfil/rol pertenece
*/
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


/*
Descripción: Este procedimiento sirve para la creación de productos y agregarle existencia
Se utiliza: En la app al momento de crear un producto
Parametros necesarios: clave del producto, nombre del producto, existencia y que usuario realiza el registro
Otros: Retorna el id del producto que se creó
*/
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