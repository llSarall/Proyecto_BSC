using System.Data;
using Dapper;
using Entities;

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
}