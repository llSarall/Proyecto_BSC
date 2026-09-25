/* -----------------------------------------------------------
   Proyecto : BSC - Evaluación técnica BJXIT
   Script   : 01_tablas.sql (1 de 7)
   Autor    : Alejandro Mejia Herrera
   Fecha    : 24/09/2026
   Descripción: Este Script nos permite crear la base de datos con sus respectivas tablas
   -----------------------------------------------------------*/

IF DB_ID('database_bsc') IS NULL CREATE DATABASE database_bsc;

GO
USE database_bsc;
GO

/* La tabla de perfiles nos sirve para tener definidos los distintos roles de los usuarios, es como tener un catálogo de opciones*/
CREATE TABLE perfiles (
    id_perfil INT IDENTITY (1, 1) CONSTRAINT PK_perfiles PRIMARY KEY,
    nombre_perfil VARCHAR (50) NOT NULL CONSTRAINT UQ_perfiles_nombre_perfil UNIQUE
);

/* La tabla de usuarios nos sirve para almacenar la informacion de los usuarios asi como su perfil/rol al que tiene acceso*/
CREATE TABLE usuarios (
    id_usuario INT IDENTITY (1, 1) CONSTRAINT PK_usuarios PRIMARY KEY,
    usuario VARCHAR (50) NOT NULL CONSTRAINT UQ_usuarios_usuario UNIQUE,
    id_perfil INT NOT NULL,
    password_hash VARCHAR (255) NOT NULL,
    estatus BIT CONSTRAINT DF_usuarios_estatus DEFAULT 1 NOT NULL,
    CONSTRAINT FK_usuarios_perfiles FOREIGN KEY (id_perfil) REFERENCES perfiles (id_perfil)
);

/* La tabla de productos nos sirve para almacenar la informacion de los productos/articulos asi como su existencia, ademas agregue un par de columnas que nos sirven
para tener rastreabilidad de quien realizo el registro y la fecha*/
CREATE TABLE productos (
    id_producto INT IDENTITY (1, 1) CONSTRAINT PK_productos PRIMARY KEY,
    clave_producto VARCHAR (50) NOT NULL CONSTRAINT UQ_productos_clave UNIQUE,
    nombre_producto VARCHAR (50) NOT NULL,
    existencia INT NOT NULL CONSTRAINT CK_productos_existencia CHECK (existencia >= 0) CONSTRAINT DF_productos_existencia DEFAULT 0,
    id_usuario_registro INT NOT NULL,
    fecha_registro_producto DATETIME2 NOT NULL CONSTRAINT DF_productos_fecha DEFAULT GETDATE(),
    CONSTRAINT FK_productos_usuarios FOREIGN KEY (id_usuario_registro) REFERENCES usuarios (id_usuario)
);

/* La tabla de pedidos nos sirve para almacenar la informacion principal de los pedidos, ademas agregue un par de columnas que nos sirven para tener rastreabilidad de 
quien realizo el pedido y la fecha*/
CREATE TABLE pedidos (
    id_pedido INT IDENTITY (1, 1) CONSTRAINT PK_pedidos PRIMARY KEY,
    nombre_cliente VARCHAR (255) NOT NULL,
    id_usuario_registro INT NOT NULL,
    fecha_registro_pedido DATETIME2 NOT NULL CONSTRAINT DF_pedidos_fecha DEFAULT GETDATE(),
    CONSTRAINT FK_pedidos_usuarios FOREIGN KEY (id_usuario_registro) REFERENCES usuarios (id_usuario)
);

/* La tabla de detalle_pedido nos sirve para almacenar la informacion de cada articulo de los pedidos, decidi manejar los pedidos en dos tablas ya que si en un pedido hay mas
de un articulo iba a ser complicado manejarlo en una sola tabla asi que por eso opte por esta alternativa, asi en la tabla de pedidos solo se crea un registro y en esta tabla se crean
N numero de registros dependiendo los articulos */
CREATE TABLE detalle_pedido (
    id_detalle_pedido INT IDENTITY (1, 1) CONSTRAINT PK_detalle_pedido PRIMARY KEY,
    id_pedido INT NOT NULL,
    id_producto INT NOT NULL,
    cantidad INT NOT NULL CONSTRAINT CK_detalle_pedido_cantidad CHECK (cantidad > 0),
    CONSTRAINT FK_detalle_pedido_pedidos FOREIGN KEY (id_pedido) REFERENCES pedidos (id_pedido),
    CONSTRAINT FK_detalle_pedido_productos FOREIGN KEY (id_producto) REFERENCES productos (id_producto),
    CONSTRAINT UQ_detalle_pedido_producto UNIQUE (id_pedido, id_producto)
);