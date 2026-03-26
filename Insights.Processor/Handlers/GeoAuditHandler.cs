using Insights.Contracts.Events;
using Insights.Domain.Models;
using Insights.Domain.Repositories;
using Insights.MessageBus;

namespace Insights.Processor.Handlers;

public class GeoAuditHandler(IServiceScopeFactory scopeFactory, ILogger<GeoAuditHandler> logger) : IMessageHandler<GeoInfoRequestedEvent>, IGeoAuditHandler
{
    public async Task HandleAsync(GeoInfoRequestedEvent message, CancellationToken ct = default)
    {
        using var scope = scopeFactory.CreateScope();
        var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        AuditEntry entry = AuditEntry.Create(message.Lat, message.Lon, message.ParamCityName ?? "", message.ResolvedCityName,message.CountryCode, message.RequestedAt);

        try
        {
            await uow.AuditRepository.AddAsync(entry);
            await uow.CommitAsync();
            
        }
        catch (Exception ex)
        {
            logger.LogError("Error on GeoAuditHandler {Ex}", ex);
            throw;
        }
        
    }
}
