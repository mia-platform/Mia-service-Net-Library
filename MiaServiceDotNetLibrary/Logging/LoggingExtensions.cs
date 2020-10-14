using Microsoft.AspNetCore.Builder;

namespace MiaServiceDotNetLibrary.Logging
{
    public static class LoggingExtensions
    {
        public static IApplicationBuilder UseRequestResponseLoggingMiddleware(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestResponseLoggingMiddleware>();
        }
    }
}