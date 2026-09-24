using Api.Services;
using BusinessLogic.Services;
using Entities.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUsuarioService _usuarioService;
    private readonly TokenService _tokenService;

    public AuthController(IUsuarioService usuarioService, TokenService tokenService)
    {
        _usuarioService = usuarioService;
        _tokenService = tokenService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
    {
        var usuario = await _usuarioService.ValidarCredencialesAsync(request.Usuario, request.Password);

        if (usuario is null)
            return Unauthorized(new { mensaje = "Usuario o contraseña incorrectos." });

        var (token, expiracion) = _tokenService.GenerarToken(usuario);

        return Ok(new LoginResponse
        {
            Token = token,
            Expiracion = expiracion,
            IdUsuario = usuario.IdUsuario,
            NombreUsuario = usuario.NombreUsuario,
            Perfil = usuario.NombrePerfil
        });
    }
}