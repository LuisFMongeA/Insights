
using Insights.Contracts.Events;
using Insights.Domain.Models;
using Insights.Domain.Repositories;
using Insights.MessageBus;

namespace Insights.Processor.Handlers;

public class GeoStatsHandler(IServiceScopeFactory scopeFactory, ILogger<GeoStatsHandler> logger) : IMessageHandler<GeoInfoRequestedEvent>, IGeoStatsHandler
{
    public async Task HandleAsync(GeoInfoRequestedEvent message, CancellationToken ct = default)
    {
        using var scope = scopeFactory.CreateScope();
        var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        StatEntry entry = StatEntry.Create(message.Lat, message.Lon, message.ResolvedCityName, message.CountryCode, message.RequestedAt);

        try
        {
            await uow.StatRepository.AddAsync(entry);
            await uow.CommitAsync();
        }
        catch (Exception ex)
        {
            logger.LogError("Error on GeoStatsHandler: {ex}", ex);
            throw;
        }
    }
}
