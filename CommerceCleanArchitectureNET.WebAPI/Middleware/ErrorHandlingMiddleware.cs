using CommerceCleanArchitectureNET.Domain.Exceptions;
using CommerceCleanArchitectureNET.WebAPI.Models;

namespace CommerceCleanArchitectureNET.WebAPI.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(
            RequestDelegate next,
            ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (DomainException ex)
            {
                _logger.LogWarning(ex, "Domain exception occurred");
                await HandleExceptionAsync(context, ex, StatusCodes.Status400BadRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred");
                await HandleExceptionAsync(
                    context,
                    ex,
                    StatusCodes.Status500InternalServerError);
            }
        }

        private static Task HandleExceptionAsync(
            HttpContext context,
            Exception exception,
            int statusCode)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var response = new ErrorResponse(exception.Message);
            return context.Response.WriteAsJsonAsync(response);
        }
    }
}
