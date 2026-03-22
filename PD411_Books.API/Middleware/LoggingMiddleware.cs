using System.Diagnostics;

namespace PD411_Books.API.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();
            var requestTime = DateTime.UtcNow;

            _logger.LogInformation("Request started: {Method} {Path} at {Time}", 
                context.Request.Method, 
                context.Request.Path, 
                requestTime);

            try
            {
                await _next(context);
            }
            finally
            {
                stopwatch.Stop();
                var responseTime = DateTime.UtcNow;
                var duration = stopwatch.ElapsedMilliseconds;

                _logger.LogInformation("Request completed: {Method} {Path} - Status: {StatusCode} - Duration: {Duration}ms - Completed at {Time}", 
                    context.Request.Method, 
                    context.Request.Path, 
                    context.Response.StatusCode, 
                    duration,
                    responseTime);
            }
        }
    }
}
