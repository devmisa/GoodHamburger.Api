using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace GoodHamburger.Api.Middleware
{
    public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            logger.LogError(exception, "Erro inesperado: {Message}", exception.Message);

            ProblemDetails problem = new()
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Erro interno do servidor.",
                Detail = "Ocorreu um erro inesperado. Tente novamente mais tarde."
            };

            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await httpContext.Response.WriteAsJsonAsync(problem, cancellationToken);

            return true;
        }
    }
}
