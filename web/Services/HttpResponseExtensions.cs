using System.Net;
using System.Net.Http.Json;

namespace web.Services;

public static class HttpResponseExtensions
{
    public static async Task<string> LeerErrorAsync(this HttpResponseMessage respuesta)
    {
        if (respuesta.StatusCode == HttpStatusCode.Unauthorized)
            return "Tu sesión expiró. Vuelve a iniciar sesión.";

        if (respuesta.StatusCode == HttpStatusCode.Forbidden)
            return "No tienes permiso para realizar esta acción.";

        try
        {
            var error = await respuesta.Content.ReadFromJsonAsync<RespuestaError>();
            if (!string.IsNullOrWhiteSpace(error?.Mensaje))
                return error.Mensaje;
        }
        catch
        {
            // La respuesta no traía el formato { mensaje }
        }

        return "Ocurrió un error al procesar la solicitud.";
    }
}
