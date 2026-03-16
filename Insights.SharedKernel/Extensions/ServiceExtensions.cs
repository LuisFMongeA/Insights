
using Insights.SharedKernel.Middleware;
using Microsoft.Extensions.DependencyInjection;

namespace Insights.SharedKernel.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddSharedMiddlewares(this IServiceCollection services)
    {
        services.AddTransient<ExceptionMiddleware>();
        //services.AddTransient<ScopeMiddleware>();
        //services.AddTransient<ApiKeyMiddleware>();
        return services;
    }
}

