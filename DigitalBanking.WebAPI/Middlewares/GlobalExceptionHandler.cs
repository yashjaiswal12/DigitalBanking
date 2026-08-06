using DigitalBanking.Domain.Exceptions;
using DigitalBanking.WebAPI.Common;
using Microsoft.AspNetCore.Diagnostics;

namespace DigitalBanking.WebAPI.Middlewares
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError(exception, $"Unhandled exception occured while executing request {context.Request.Method} {context.Request.Path}");

            var response = new ApiErrorResponse();

            var (statuscode, message) = exception switch
            {
                CustomerNotFoundException ex => (StatusCodes.Status404NotFound, ex.Message),
                CustomerAlreadyExistsException ex => (StatusCodes.Status409Conflict, ex.Message),
                InvalidTokenException ex => (StatusCodes.Status401Unauthorized, ex.Message),
                InvalidCustomerException ex => (StatusCodes.Status400BadRequest, ex.Message),
                DomainException ex => (StatusCodes.Status400BadRequest, ex.Message),
                _ => (StatusCodes.Status500InternalServerError, exception.Message)
            };

            context.Response.StatusCode = statuscode;

            await context.Response.WriteAsJsonAsync(response = new ApiErrorResponse 
            {
                StatusCode = statuscode,
                Message = message,
            }, cancellationToken);

            return true;
        }
    }
}
