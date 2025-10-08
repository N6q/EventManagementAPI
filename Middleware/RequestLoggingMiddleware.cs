using System.Diagnostics;

namespace EventManagementAPI.Middleware
{
    /// <summary>
    /// Logs each request and its processing time using Serilog.
    /// </summary>
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        // ======================================================
        // 🔹 INVOKE PIPELINE
        // ======================================================
        /// <summary>
        /// Intercepts each request, measures duration, and logs details to Serilog.
        /// </summary>
        public async Task Invoke(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();
            var request = context.Request;

            try
            {
                await _next(context);
            }
            finally
            {
                stopwatch.Stop();

                var response = context.Response;
                _logger.LogInformation(
                    "HTTP {Method} {Path} responded {StatusCode} in {Elapsed:0.0000} ms",
                    request.Method,
                    request.Path,
                    response.StatusCode,
                    stopwatch.Elapsed.TotalMilliseconds
                );
            }
        }
    }
}
