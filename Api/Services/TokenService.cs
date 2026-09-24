using System.Security.Claims;
using System.Text;
using Entities;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Api.Services;

public class TokenService
{
    private readonly IConfiguration _configuration;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public (string Token, DateTime Expiracion) GenerarToken(Usuario usuario)
    {
        var jwt = _configuration.GetSection("Jwt");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!));
        var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiracion = DateTime.UtcNow.AddMinutes(int.Parse(jwt["ExpiracionMinutos"]!));

        var claims = new[]
        {
            new Claim("sub",  usuario.IdUsuario.ToString()),
            new Claim("name", usuario.NombreUsuario),
            new Claim("role", usuario.NombrePerfil)
        };

        var descriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = expiracion,
            Issuer = jwt["Issuer"],
            Audience = jwt["Audience"],
            SigningCredentials = credenciales
        };

        var token = new JsonWebTokenHandler().CreateToken(descriptor);
        return (token, expiracion);
    }
}