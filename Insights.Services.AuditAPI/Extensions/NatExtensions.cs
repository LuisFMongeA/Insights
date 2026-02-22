using Insights.AuditAPI.Data;
using Insights.AuditAPI.Messaging;

namespace Insights.AuditAPI.Extensions;

public static class NatsExtensions
{
    public static IServiceCollection AddNatsConsumer(this IServiceCollection services)
    {
        services.AddSingleton<IAuditRepository, InMemoryAuditRepository>();
        services.AddHostedService<NatsConsumer>();
        return services;
    }
}