using System.Net.Http.Json;
using Entities.Dtos;

namespace web.Services;

public class AuthService
{
    private readonly HttpClient _http;
    private readonly JwtAuthStateProvider _authStateProvider;

    public AuthService(HttpClient http, JwtAuthStateProvider authStateProvider)
    {
        _http = http;
        _authStateProvider = authStateProvider;
    }

    /// <summary>Devuelve null si el login fue exitoso, o el mensaje de error.</summary>
    public async Task<string?> IniciarSesionAsync(LoginRequest request)
    {
        var respuesta = await _http.PostAsJsonAsync("api/auth/login", request);

        if (!respuesta.IsSuccessStatusCode)
        {
            var error = await respuesta.Content.ReadFromJsonAsync<RespuestaError>();
            return error?.Mensaje ?? "No fue posible iniciar sesión.";
        }

        var login = await respuesta.Content.ReadFromJsonAsync<LoginResponse>();
        await _authStateProvider.IniciarSesionAsync(login!.Token);
        return null;
    }

    public Task CerrarSesionAsync() => _authStateProvider.CerrarSesionAsync();
}