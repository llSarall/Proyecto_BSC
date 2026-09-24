using Entities.Dtos;

namespace DataAccess.Repositories;

public interface IPedidoRepository
{
    Task<int> CrearAsync(string nombreCliente, int idUsuarioRegistro, int idProducto, int cantidad);
    Task<IEnumerable<PedidoConsulta>> ObtenerTodosAsync();
}