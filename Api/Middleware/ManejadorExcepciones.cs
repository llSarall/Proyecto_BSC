using BusinessLogic.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Data.SqlClient;

namespace Api.Middleware;

public class ManejadorExcepciones : IExceptionHandler
{
    private readonly ILogger<ManejadorExcepciones> _logger;

    public ManejadorExcepciones(ILogger<ManejadorExcepciones> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var (status, mensaje) = exception switch
        {
            ReglaNegocioException ex => (StatusCodes.Status400BadRequest, ex.Message),
            SqlException { Number: >= 50000 } ex => (StatusCodes.Status400BadRequest, ex.Message),
            SqlException { Number: 2627 or 2601 } => (StatusCodes.Status409Conflict, "El registro ya existe."),
            SqlException { Number: 547 } => (StatusCodes.Status400BadRequest, "Los datos hacen referencia a un registro inexistente."),
            _ => (StatusCodes.Status500InternalServerError, "Ocurrió un error inesperado.")
        };

        if (status == StatusCodes.Status500InternalServerError)
            _logger.LogError(exception, "Error no controlado");

        httpContext.Response.StatusCode = status;
        await httpContext.Response.WriteAsJsonAsync(new { mensaje }, cancellationToken);
        return true;
    }
}