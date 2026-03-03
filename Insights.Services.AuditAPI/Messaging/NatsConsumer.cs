using Insights.Contracts.Events;
using Insights.Domain.Models;
using Insights.Domain.Repositories;
using Insights.SharedKernel.Constants;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NATS.Client.Core;

namespace Insights.AuditAPI.Messaging;

public class NatsConsumer(
    IServiceScopeFactory scopeFactory,
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
                await using var connection = new NatsConnection(new NatsOpts { Url = url });
                await connection.ConnectAsync();

                logger.LogInformation("NATS connected. Listening on {Subject}",
                    NatsSubjects.GeoInfoRequested);

                await foreach (var msg in connection.SubscribeAsync<GeoInfoRequestedEvent>(
                    NatsSubjects.GeoInfoRequested, cancellationToken: ct))
                {
                    if (msg.Data is null) continue;

                    try
                    {
                        using var scope = scopeFactory.CreateScope();
                        var unitOfWork = scope.ServiceProvider
                            .GetRequiredService<IUnitOfWork>();

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

                        await unitOfWork.AuditRepository.AddAsync(entry);
                        await unitOfWork.CommitAsync(ct);

                        logger.LogInformation("Audit saved for city: {City}",
                            entry.ResolvedCityName);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Error processing message");
                    }
                }
            }
            catch (OperationCanceledException)
            {
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