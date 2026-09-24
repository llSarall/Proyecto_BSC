using System.Data;
using Dapper;
using Entities.Dtos;

namespace DataAccess.Repositories;

public class PedidoRepository : IPedidoRepository
{
    private readonly DbConnectionFactory _connectionFactory;

    public PedidoRepository(DbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<int> CrearAsync(string nombreCliente, int idUsuarioRegistro, int idProducto, int cantidad)
    {
        using var connection = _connectionFactory.CreateConnection();

        return await connection.ExecuteScalarAsync<int>(
            "dbo.sp_crear_pedido",
            new
            {
                nombre_cliente = nombreCliente,
                id_usuario_registro = idUsuarioRegistro,
                id_producto = idProducto,
                cantidad
            },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<PedidoConsulta>> ObtenerTodosAsync()
    {
        using var connection = _connectionFactory.CreateConnection();

        return await connection.QueryAsync<PedidoConsulta>(
            @"SELECT id_pedido, fecha_registro_pedido, nombre_cliente, vendedor,
                     clave_producto, nombre_producto, cantidad
              FROM dbo.vw_pedidos
              ORDER BY fecha_registro_pedido DESC;");
    }
}