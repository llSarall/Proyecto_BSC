CREATE DATABASE database_bsc;


GO
USE database_bsc;
GO

CREATE TABLE perfiles (
    id_perfil INT IDENTITY (1, 1) PRIMARY KEY,
    nombre_perfil VARCHAR (50) NOT NULL CONSTRAINT UQ_perfiles_nombre_perfil UNIQUE
);

CREATE TABLE usuarios (
    id_usuario INT IDENTITY (1, 1) PRIMARY KEY,
    usuario VARCHAR (50) NOT NULL CONSTRAINT UQ_usuarios_usuario UNIQUE,
    id_perfil INT NOT NULL,
    password_hash VARCHAR (255) NOT NULL,
    estatus BIT CONSTRAINT DF_usuarios_estatus DEFAULT 1 NOT NULL,
    CONSTRAINT FK_usuarios_perfiles FOREIGN KEY (id_perfil) REFERENCES perfiles (id_perfil)
);

CREATE TABLE productos (
    id_producto INT IDENTITY (1, 1) PRIMARY KEY,
    clave_producto VARCHAR (50) NOT NULL CONSTRAINT UQ_productos_clave UNIQUE,
    nombre_producto VARCHAR (50) NOT NULL,
    existencia INT NOT NULL CONSTRAINT CK_Productos_Existencia CHECK (existencia >= 0) CONSTRAINT DF_productos_existencia DEFAULT 0,
    id_usuario_registro INT NOT NULL,
    fecha_registro_producto DATETIME2 NOT NULL CONSTRAINT DF_productos_fecha DEFAULT GETDATE(),
    CONSTRAINT FK_productos_usuarios FOREIGN KEY (id_usuario_registro) REFERENCES usuarios (id_usuario)
);

CREATE TABLE pedidos (
    id_pedido INT IDENTITY (1, 1) PRIMARY KEY,
    nombre_cliente VARCHAR (255) NOT NULL,
    id_usuario_registro INT NOT NULL,
    fecha_registro_pedido DATETIME2 NOT NULL CONSTRAINT DF_pedidos_fecha DEFAULT GETDATE(),
    CONSTRAINT FK_pedidos_usuarios FOREIGN KEY (id_usuario_registro) REFERENCES usuarios (id_usuario)
);

CREATE TABLE detalle_pedido (
    id_detalle_pedido INT IDENTITY (1, 1) PRIMARY KEY,
    id_pedido INT NOT NULL,
    id_producto INT NOT NULL,
    cantidad INT NOT NULL CONSTRAINT CK_detalle_pedido_cantidad CHECK (cantidad > 0),
    CONSTRAINT FK_detalle_pedido_pedidos FOREIGN KEY (id_pedido) REFERENCES pedidos (id_pedido),
    CONSTRAINT FK_detalle_pedido_productos FOREIGN KEY (id_producto) REFERENCES productos (id_producto),
    CONSTRAINT UQ_detalle_pedido_producto UNIQUE (id_pedido, id_producto)
);