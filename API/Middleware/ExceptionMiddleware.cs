using System.Net;
using System.Text.Json;
using Application.Core;

namespace API.Middleware
{
    public class ExceptionMiddleware
    {
        public readonly RequestDelegate _next;
        public ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware(
            RequestDelegate next,
            ILogger<ExceptionMiddleware> logger,
            IHostEnvironment env
        )
        {
            _env = env;
            _logger = logger;
            _next = next;
        }

        //This method is the main entry point for the middleware and is responsible for handling exceptions and generating appropriate JSON responses.
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                //When an exception occurs, the middleware logs the exception using the injected logger with _logger.LogError(ex, ex.Message).
                _logger.LogError(ex, ex.Message);
                //The middleware then sets the response content type to JSON and the status code to 500 (Internal Server Error).
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                //Depending on the environment, the middleware creates an AppException object to encapsulate the error details.
                var response = _env.IsDevelopment()
                    ? new AppException(
                        context.Response.StatusCode,
                        ex.Message,
                        ex.StackTrace?.ToString()
                    )
                    : new AppException(context.Response.StatusCode, "Internal Server Error");

                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                var json = JsonSerializer.Serialize(response, options);
                //The serialized JSON response is written to the HTTP response stream
                await context.Response.WriteAsync(json);
            }
        }
    }
}
