using Insights.SharedKernel.Middleware;
using Microsoft.AspNetCore.Builder;

namespace Insights.SharedKernel.Extensions;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder app)
        => app.UseMiddleware<ExceptionMiddleware>();
    public static IApplicationBuilder UseScopeMiddleware(this IApplicationBuilder app)
        => app.UseMiddleware<ScopeMiddleware>();
}