using BusinessLogic.Services;
using Entities.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PedidosController : ControllerBase
{
    private readonly IPedidoService _pedidoService;

    public PedidosController(IPedidoService pedidoService)
    {
        _pedidoService = pedidoService;
    }

    [HttpPost]
    [Authorize(Roles = "Vendedor")]
    public async Task<IActionResult> Crear(CrearPedidoRequest request)
    {
        var idUsuario = int.Parse(User.FindFirst("sub")!.Value);
        var idPedido = await _pedidoService.CrearAsync(request, idUsuario);
        return Ok(new { idPedido });
    }

    [HttpGet]
    [Authorize(Roles = "Personal Administrativo")]
    public async Task<ActionResult<IEnumerable<PedidoConsulta>>> ObtenerTodos()
    {
        return Ok(await _pedidoService.ObtenerTodosAsync());
    }
}