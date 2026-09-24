using BusinessLogic.Exceptions;
using DataAccess.Repositories;
using Entities.Dtos;

namespace BusinessLogic.Services;

public class PedidoService : IPedidoService
{
    private readonly IPedidoRepository _pedidoRepository;

    public PedidoService(IPedidoRepository pedidoRepository)
    {
        _pedidoRepository = pedidoRepository;
    }

    public Task<int> CrearAsync(CrearPedidoRequest request, int idUsuarioRegistro)
    {
        var cliente = request.NombreCliente.Trim();

        if (string.IsNullOrEmpty(cliente))
            throw new ReglaNegocioException("El nombre del cliente es obligatorio.");

        if (request.Cantidad <= 0)
            throw new ReglaNegocioException("La cantidad debe ser mayor a cero.");

        // La validación de existencia vive en sp_crear_pedido, dentro de la
        // transacción y con bloqueo de fila, para evitar condiciones de carrera.
        return _pedidoRepository.CrearAsync(cliente, idUsuarioRegistro, request.IdProducto, request.Cantidad);
    }

    public Task<IEnumerable<PedidoConsulta>> ObtenerTodosAsync()
    {
        return _pedidoRepository.ObtenerTodosAsync();
    }
}