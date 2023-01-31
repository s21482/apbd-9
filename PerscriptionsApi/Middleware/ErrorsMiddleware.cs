using Microsoft.AspNetCore.Builder;
using PerscriptionsApi.Middlewares;

namespace PerscriptionsApi.Middleware
{
    public static class ErrorsMiddleware
    {
        public static IApplicationBuilder UseErrorsMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorsMiddlewareHandler>();
        }
    }
}
