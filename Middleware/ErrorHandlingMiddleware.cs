using System.Net;
using System.Text.Json;

namespace EventManagementAPI.Middleware
{
    // ======================================================
    // 🔹 GLOBAL ERROR HANDLING MIDDLEWARE
    // ======================================================
    // Intercepts all unhandled exceptions in the pipeline.
    // Logs them and returns a standardized JSON response.
    // ======================================================
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        // ======================================================
        // 🔹 INVOKE PIPELINE
        // ======================================================
        /// <summary>
        /// Executes the next middleware in the pipeline and catches exceptions globally.
        /// </summary>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        // ======================================================
        // 🔹 EXCEPTION HANDLER
        // ======================================================
        /// <summary>
        /// Logs exception details and sends a consistent JSON response.
        /// </summary>
        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            // Log the exception details with stack trace
            _logger.LogError(ex, "❌ An unhandled exception occurred during request processing.");

            // Determine proper status code
            var statusCode = ex switch
            {
                KeyNotFoundException => HttpStatusCode.NotFound,
                UnauthorizedAccessException => HttpStatusCode.Unauthorized,
                InvalidOperationException => HttpStatusCode.BadRequest,
                ArgumentException => HttpStatusCode.BadRequest,
                _ => HttpStatusCode.InternalServerError
            };

            // Build JSON response body
            var errorResponse = new
            {
                success = false,
                message = ex.Message,
                statusCode = (int)statusCode,
                traceId = context.TraceIdentifier
            };

            // Serialize with indented JSON for readability
            var json = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            });

            // If headers already sent, just abort safely
            if (context.Response.HasStarted)
            {
                _logger.LogWarning("⚠️ Response already started — cannot modify headers for error handling.");
                return;
            }

            // Set response details
            context.Response.Clear();
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            // Send standardized response
            await context.Response.WriteAsync(json);
        }
    }
}
