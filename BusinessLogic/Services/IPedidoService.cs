using Entities.Dtos;

namespace BusinessLogic.Services;

public interface IPedidoService
{
    Task<int> CrearAsync(CrearPedidoRequest request, int idUsuarioRegistro);
    Task<IEnumerable<PedidoConsulta>> ObtenerTodosAsync();
}