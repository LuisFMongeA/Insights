using Insights.AuditAPI.Data;
using Insights.Contracts.Events;
using Insights.Services.AuditAPI.Model;
using NATS.Client.Core;

namespace Insights.AuditAPI.Messaging;

public class NatsConsumer(IAuditRepository repository, IConfiguration configuration, ILogger<NatsConsumer> logger)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        var url = configuration["Nats:Url"] ?? "nats://localhost:4222";
        await using var connection = new NatsConnection(new NatsOpts { Url = url });

        logger.LogInformation("NATS consumer started, listening on geo.info.requested");

        await foreach (var msg in connection.SubscribeAsync<GeoInfoRequestedEvent>("geo.info.requested", cancellationToken: ct))
        {
            if (msg.Data is null) continue;

            try
            {
                var entry = new AuditEntry
                {
                    Id = msg.Data.Id,
                    RequestedAt = msg.Data.RequestedAt,
                    Lat = msg.Data.Lat,
                    Lon = msg.Data.Lon,
                    CityName = msg.Data.CityName,
                    ResolvedCityName = msg.Data.ResolvedCityName,
                    CountryCode = msg.Data.CountryCode
                };

                await repository.SaveAsync(entry);
                logger.LogInformation("Audit saved for city: {City}", entry.ResolvedCityName);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error processing NATS message");
            }
        }
    }
}