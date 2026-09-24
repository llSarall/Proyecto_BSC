using System.Data;
using Dapper;
using Entities;
using Entities.Dtos;

namespace DataAccess.Repositories;

public class ProductoRepository : IProductoRepository
{
    private readonly DbConnectionFactory _connectionFactory;

    public ProductoRepository(DbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<Producto>> ObtenerTodosAsync()
    {
        using var connection = _connectionFactory.CreateConnection();

        return await connection.QueryAsync<Producto>(
            "dbo.sp_obtener_productos",
            commandType: CommandType.StoredProcedure);
    }

    public async Task<int> RegistrarAsync(Producto producto)
    {
        using var connection = _connectionFactory.CreateConnection();

        return await connection.ExecuteScalarAsync<int>(
            "dbo.sp_registrar_producto",
            new
            {
                clave_producto = producto.ClaveProducto,
                nombre_producto = producto.NombreProducto,
                existencia = producto.Existencia,
                id_usuario_registro = producto.IdUsuarioRegistro
            },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<ReporteExistencia>> ObtenerReporteExistenciasAsync()
    {
        using var connection = _connectionFactory.CreateConnection();

        return await connection.QueryAsync<ReporteExistencia>(
            @"SELECT id_producto, clave_producto, nombre_producto, existencia, estatus_existencia
              FROM dbo.vw_reporte_existencias
              ORDER BY nombre_producto;");
    }
}