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

            string requestBodyPreview = string.Empty;
            try
            {
                if (request.ContentLength > 0 && request.ContentLength < 8192)
                {
                    request.EnableBuffering();

                    request.Body.Position = 0;
                    using (var reader = new StreamReader(request.Body, Encoding.UTF8, detectEncodingFromByteOrderMarks: false, leaveOpen: true))
                    {
                        requestBodyPreview = await reader.ReadToEndAsync();
                    }
                    request.Body.Position = 0;
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "No se pudo leer request body en LoggingMiddleware");
            }

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

                _logger.LogInformation("HTTP {Method} {Path}{QueryString} responded {StatusCode} in {Elapsed}ms\nRequestPreview: {Req}\nResponsePreview: {Res}",
                    request.Method,
                    request.Path,
                    request.QueryString,
                    context.Response.StatusCode,
                    sw.ElapsedMilliseconds,
                    requestBodyPreview,
                    responseBodyText);

                await responseBody.CopyToAsync(originalBodyStream);
                context.Response.Body = originalBodyStream;
            }
        }

    }

    public static class LoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestResponseLogging(this IApplicationBuilder app)
        {
            return app.UseMiddleware<LoggingMiddleware>();
        }
    }
}
