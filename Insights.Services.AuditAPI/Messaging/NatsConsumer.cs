using Insights.AuditAPI.Data;
using Insights.Contracts.Events;
using Insights.Services.AuditAPI.Model;
using Insights.SharedKernel.Constants;
using NATS.Client.Core;

namespace Insights.AuditAPI.Messaging;

public class NatsConsumer(
    IAuditRepository repository,
    IConfiguration configuration,
    ILogger<NatsConsumer> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        var url = configuration["Nats:Url"] ?? "nats://localhost:4222";

        while (!ct.IsCancellationRequested)
        {
            try
            {
                logger.LogInformation("Connecting to NATS at {Url}...", url);

                await using var connection = new NatsConnection(new NatsOpts { Url = url });
                await connection.ConnectAsync();

                logger.LogInformation("NATS connected. Listening on geo.info.requested");

                await foreach (var msg in connection.SubscribeAsync<GeoInfoRequestedEvent>(
                    NatsSubjects.GeoInfoRequested, cancellationToken: ct))
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
                            CityName = msg.Data.ParamCityName,
                            ResolvedCityName = msg.Data.ResolvedCityName,
                            CountryCode = msg.Data.CountryCode
                        };

                        await repository.SaveAsync(entry);
                        logger.LogInformation("Audit saved for city: {City}", entry.ResolvedCityName);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Error processing message");
                    }
                }
            }
            catch (OperationCanceledException)
            {
                // Apagado limpio, salimos sin error
                break;
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "NATS connection failed. Retrying in 5 seconds...");
                await Task.Delay(TimeSpan.FromSeconds(5), ct);
            }
        }

        logger.LogInformation("NATS consumer stopped");
    }
}