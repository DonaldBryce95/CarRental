using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Api.Exceptions
{
  public class GlobalExceptionHandler : IExceptionHandler
  {
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
      _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
      _logger.LogError(exception, "An unhandled exception occurred");

      var statusCode = exception switch
      {
        ArgumentException => StatusCodes.Status400BadRequest,
        InvalidOperationException => StatusCodes.Status409Conflict, _ => StatusCodes.Status500InternalServerError
      };

      var problemDetails = new ProblemDetails
      {
        Status = statusCode,
        Title = statusCode == StatusCodes.Status500InternalServerError
          ? "An unhandled error occurred."
          : "Request failed",
        Detail = statusCode == StatusCodes.Status500InternalServerError
        ? "An unexpected error occurred."
        : exception.Message
      };

      httpContext.Response.StatusCode = statusCode;

      await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

      return true;
    }
  }
}