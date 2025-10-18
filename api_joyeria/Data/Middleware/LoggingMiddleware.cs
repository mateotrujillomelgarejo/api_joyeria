using System.Diagnostics;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace api_joyeria.Data.Middleware
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
            var sw = Stopwatch.StartNew();
            var request = context.Request;

            // Leer body de request (si aplica) - solo pequeño preview
            string requestBodyPreview = string.Empty;
            if (request.ContentLength > 0 && request.ContentLength < 8192 && request.Body.CanSeek)
            {
                request.Body.Seek(0, SeekOrigin.Begin);
                using (var reader = new StreamReader(request.Body, Encoding.UTF8, detectEncodingFromByteOrderMarks: false, leaveOpen: true))
                {
                    requestBodyPreview = await reader.ReadToEndAsync();
                }
                request.Body.Seek(0, SeekOrigin.Begin);
            }

            // Intercept response
            var originalBodyStream = context.Response.Body;
            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            try
            {
                await _next(context);
            }
            finally
            {
                sw.Stop();

                context.Response.Body.Seek(0, SeekOrigin.Begin);
                string responseBodyText = string.Empty;
                if (context.Response.Body.Length < 8192)
                {
                    using (var reader = new StreamReader(context.Response.Body, Encoding.UTF8, detectEncodingFromByteOrderMarks: false, leaveOpen: true))
                    {
                        responseBodyText = await reader.ReadToEndAsync();
                    }
                    context.Response.Body.Seek(0, SeekOrigin.Begin);
                }

                _logger.LogInformation("HTTP {Method} {Path}{QueryString} responded {StatusCode} in {Elapsed}ms",
                    request.Method,
                    request.Path,
                    request.QueryString,
                    context.Response.StatusCode,
                    sw.ElapsedMilliseconds);

                // debug small previews
                if (!string.IsNullOrWhiteSpace(requestBodyPreview))
                    _logger.LogDebug("Request Body Preview: {RequestPreview}", requestBodyPreview);

                if (!string.IsNullOrWhiteSpace(responseBodyText))
                    _logger.LogDebug("Response Body Preview: {ResponsePreview}", responseBodyText);

                // copy back
                await responseBody.CopyToAsync(originalBodyStream);
                context.Response.Body = originalBodyStream;
            }
        }
    }

    // extensión para registrar el middleware de forma cómoda
    public static class LoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestResponseLogging(this IApplicationBuilder app)
        {
            return app.UseMiddleware<LoggingMiddleware>();
        }
    }
}
