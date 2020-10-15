using Microsoft.AspNetCore.Builder;

namespace MiaServiceDotNetLibrary.Logging
{
    public static class LoggingExtensions
    {
        public static IApplicationBuilder UseRequestResponseLoggingMiddleware(
            this IApplicationBuilder builder, MiddlewareOptions options = default)
        {
            options = options ?? new MiddlewareOptions();
            return builder.UseMiddleware<RequestResponseLoggingMiddleware>(options);
        }
    }
}
