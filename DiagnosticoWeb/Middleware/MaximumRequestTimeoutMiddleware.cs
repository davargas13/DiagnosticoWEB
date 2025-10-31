using System.Threading;
using System.Threading.Tasks;
using DiagnosticoWeb.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace DiagnosticoWeb.Middleware
{
    public class MaximumRequestTimeoutMiddleware
    {
        private readonly RequestDelegate _next;

        public MaximumRequestTimeoutMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IOptions<MaximumRequestTimeoutSettings> requestTimeoutSettings)
        {
            using (var timeoutSource = CancellationTokenSource.CreateLinkedTokenSource(context.RequestAborted))
            {
                timeoutSource.CancelAfter(requestTimeoutSettings.Value.Timeout);
                context.RequestAborted = timeoutSource.Token;
                await _next(context);
            }
        }
    }

    public static class MaximumRequestTimeoutMiddlewareExtensions
    {
        public static IApplicationBuilder UseMaximumRequestTimeout(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<MaximumRequestTimeoutMiddleware>();

            return builder;
        }
    }
}