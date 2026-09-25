using BusinessLogic.Exceptions;
using BusinessLogic.Services;
using Entities.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Administrador")]

public class UsuariosController : ControllerBase
{
    private readonly IUsuarioService _usuarioService;

    public UsuariosController(IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    [HttpPost]
    public async Task<IActionResult> Crear(CrearUsuarioRequest request)
    {
        var idUsuario = await _usuarioService.CrearUsuarioAsync(request.Usuario, request.Password, request.IdPerfil);
        return Ok(new { idUsuario });
    }
}