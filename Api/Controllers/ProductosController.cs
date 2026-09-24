using BusinessLogic.Services;
using Entities;
using Entities.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductosController : ControllerBase
{
    private readonly IProductoService _productoService;

    public ProductosController(IProductoService productoService)
    {
        _productoService = productoService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Producto>>> ObtenerTodos()
    {
        return Ok(await _productoService.ObtenerTodosAsync());
    }

    [HttpPost]
    [Authorize(Roles = "Personal Administrativo")]
    public async Task<IActionResult> Registrar(RegistrarProductoRequest request)
    {
        var idUsuario = int.Parse(User.FindFirst("sub")!.Value);
        var idProducto = await _productoService.RegistrarAsync(request, idUsuario);
        return Ok(new { idProducto });
    }

    [HttpGet("existencias")]
    [Authorize(Roles = "Personal Administrativo")]
    public async Task<ActionResult<IEnumerable<ReporteExistencia>>> ReporteExistencias()
    {
        return Ok(await _productoService.ObtenerReporteExistenciasAsync());
    }
}