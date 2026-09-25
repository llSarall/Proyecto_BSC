using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace web.Services;

public class JwtAuthStateProvider : AuthenticationStateProvider
{
    private const string ClaveToken = "bsc_token";

    private static readonly AuthenticationState Anonimo =
        new(new ClaimsPrincipal(new ClaimsIdentity()));

    private readonly IJSRuntime _js;
    private readonly HttpClient _http;

    public JwtAuthStateProvider(IJSRuntime js, HttpClient http)
    {
        _js = js;
        _http = http;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await _js.InvokeAsync<string?>("localStorage.getItem", ClaveToken);

        if (string.IsNullOrWhiteSpace(token))
            return Anonimo;

        try
        {
            var claims = LeerClaims(token);

            if (TokenExpirado(claims))
            {
                await _js.InvokeVoidAsync("localStorage.removeItem", ClaveToken);
                return Anonimo;
            }

            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var identidad = new ClaimsIdentity(claims, "jwt", "name", "role");
            return new AuthenticationState(new ClaimsPrincipal(identidad));
        }
        catch
        {
            await _js.InvokeVoidAsync("localStorage.removeItem", ClaveToken);
            return Anonimo;
        }
    }

    public async Task IniciarSesionAsync(string token)
    {
        await _js.InvokeVoidAsync("localStorage.setItem", ClaveToken, token);
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public async Task CerrarSesionAsync()
    {
        await _js.InvokeVoidAsync("localStorage.removeItem", ClaveToken);
        _http.DefaultRequestHeaders.Authorization = null;
        NotifyAuthenticationStateChanged(Task.FromResult(Anonimo));
    }

    private static List<Claim> LeerClaims(string token)
    {
        var payload = token.Split('.')[1]
            .Replace('-', '+')
            .Replace('_', '/');

        payload = payload.PadRight(payload.Length + (4 - payload.Length % 4) % 4, '=');

        var json = Convert.FromBase64String(payload);
        var valores = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(json)!;

        return valores.Select(v => new Claim(v.Key, v.Value.ToString())).ToList();
    }

    private static bool TokenExpirado(List<Claim> claims)
    {
        var exp = claims.FirstOrDefault(c => c.Type == "exp")?.Value;

        if (exp is null || !long.TryParse(exp, out var segundos))
            return true;

        return DateTimeOffset.FromUnixTimeSeconds(segundos) <= DateTimeOffset.UtcNow;
    }
}