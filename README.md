# BSC – Sistema de administración de pedidos

Evaluación técnica Full Stack para BJXIT. Aplicación web para que la empresa BSC administre usuarios, productos, existencias y pedidos, con permisos por perfil.

## Tecnologías

- **Base de datos:** SQL Server 2025
- **Backend:** ASP.NET Core Web API (.NET 10), Dapper, autenticación JWT
- **Frontend:** Blazor WebAssembly (.NET 10), Bootstrap
- **Herramientas:** Visual Studio 2026, SSMS 22, draw.io, Postman

## Arquitectura

Solución en 3 capas:

| Proyecto | Capa | Responsabilidad |
|---|---|---|
| `web` | Presentación (UI) | Blazor WebAssembly: login, menú y pantallas por perfil |
| `Api` | Servicios REST | Controladores, autenticación JWT, CORS y manejo global de errores |
| `BusinessLogic` | Lógica de negocio | Reglas, validaciones y hash de contraseñas |
| `DataAccess` | Acceso a datos | Repositorios con Dapper que ejecutan stored procedures y vistas |
| `Entities` | Compartido | Entidades y DTOs usados por todas las capas |

Flujo de una petición:

```
web (Blazor) → Api (Controlador) → BusinessLogic (Servicio) → DataAccess (Repositorio) → SQL Server
```

Los diagramas (entidad-relación, componentes y clases) están en la carpeta `Diagrama`.

## Base de datos

Los scripts están en la carpeta `Base de datos` y deben ejecutarse en orden sobre una instancia sin la base `database_bsc`:

| # | Script | Contenido |
|---|---|---|
| 1 | `01_tablas.sql` | Base de datos, tablas y restricciones (PK, FK, UNIQUE, CHECK, DEFAULT) |
| 2 | `02_datos_iniciales.sql` | Perfiles y usuarios iniciales (uno por perfil) |
| 3 | `03_stored_procedures.sql` | Procedimientos almacenados, incluido `sp_crear_pedido` con transacción y validación de existencia |
| 4 | `04_triggers.sql` | Trigger que descuenta la existencia al registrar un pedido |
| 5 | `05_vistas.sql` | Vistas para el reporte de existencias y la consulta de pedidos |
| 6 | `06_indices.sql` | Índices sobre llaves foráneas y fechas |
| 7 | `07_datos_prueba.sql` | Productos y pedidos de ejemplo (creados con el stored procedure) |

## Requisitos

- Visual Studio 2026 con la carga de trabajo **Desarrollo de ASP.NET y web**
- SDK de .NET 10
- SQL Server 2025 (o compatible) con autenticación de Windows

## Cómo ejecutar

1. Ejecutar los scripts de la carpeta `Base de datos` en orden (01 al 07).
2. Verificar la cadena de conexión `BSC` en `Api/appsettings.json` (por defecto apunta a `localhost` con autenticación de Windows).
3. Abrir `Proyecto_BSC.slnx` en Visual Studio 2026.
4. Configurar varios proyectos de inicio: clic derecho en la solución → **Configurar proyectos de inicio** → **Iniciar** en `Api` y `web`.
5. Ejecutar con **F5**:
   - API (Swagger): https://localhost:7176/swagger
   - Aplicación web: https://localhost:7051

## Usuarios de prueba

| Usuario | Contraseña | Perfil | Funcionalidad |
|---|---|---|---|
| admin_01 | Admin123 | Administrador | Crear usuarios y asignar perfil |
| almacen01 | Almacen123 | Personal Administrativo | Registrar productos, reporte de existencias y consulta de pedidos |
| vendedor01 | Ventas123 | Vendedor | Colocar pedidos con validación de existencia |

## Seguridad

- Las contraseñas se guardan con hash PBKDF2 (`PasswordHasher` de ASP.NET Core Identity), nunca en texto plano.
- La regla de contraseña (mínimo 8 caracteres, con letras y números) se valida en el frontend, en la API y en la capa de negocio.
- Autorización por perfil en la API (`[Authorize(Roles = ...)]`) y en el frontend (`AuthorizeView` y `[Authorize]` en las páginas).
- El usuario que registra productos y pedidos se obtiene del token JWT, no del cuerpo de la petición.
- La validación de existencia se hace dentro de una transacción con bloqueo de fila (`UPDLOCK`), para evitar que dos vendedores vendan las mismas piezas al mismo tiempo.
- Los errores inesperados se registran en el log y el cliente solo recibe un mensaje genérico.

## Pruebas

La colección de Postman con los endpoints está en Pruebas/BSC_API.postman_collection.json

## Mejoras a futuro

- Pedidos con varios productos (la base de datos ya lo soporta mediante la tabla `detalle_pedido`).
- Guardar la clave del JWT en User Secrets o en un gestor de secretos, en lugar de `appsettings.json`.
- Consulta de pedidos propios para cada vendedor.
- Endpoint de catálogo de perfiles, en lugar de tenerlos fijos en la pantalla de alta de usuarios.
- Pruebas unitarias de la capa de negocio.
