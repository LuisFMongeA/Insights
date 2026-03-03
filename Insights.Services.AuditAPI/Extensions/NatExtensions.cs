using Insights.AuditAPI.Messaging;

namespace Insights.AuditAPI.Extensions;

public static class NatsExtensions
{
    public static IServiceCollection AddNatsConsumer(this IServiceCollection services)
    {
        services.AddHostedService<NatsConsumer>();
        return services;
    }
}