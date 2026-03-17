using Insights.SharedKernel.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Insights.SharedKernel.Extensions;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder app)
        => app.UseMiddleware<ExceptionMiddleware>();
    public static IApplicationBuilder UseScopeMiddleware(this IApplicationBuilder app)
        => app.UseMiddleware<ScopeMiddleware>();
    public static IApplicationBuilder UseApiKeyMiddleware(this IApplicationBuilder app)
        => app.UseMiddleware<ApiKeyMiddleware>();
    public static IApplicationBuilder UseSecurityHeadersMiddleware(this IApplicationBuilder app)
        => app.UseMiddleware<SecurityHeadersMiddleware>();
}
