using Insights.Domain.Repositories;
using Insights.Domain.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Insights.Infrastructure.Data.Services
{
    public class OutboxWorker(IServiceScopeFactory scopeFactory, 
        ILogger<OutboxWorker> logger,
        IDatabaseReadyService databaseReadyService) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken ct)
        {
            while (!databaseReadyService.IsReady && !ct.IsCancellationRequested)
                await Task.Delay(500, ct);
            while (!ct.IsCancellationRequested) {
                using var scope = scopeFactory.CreateScope();
                var repo = scope.ServiceProvider.GetRequiredService<IOutboxRepository>();

                try {
                    var pendingMessages = await repo.GetPendingAsync();
                    var tasks= pendingMessages.Select(async message => 
                    {
                        
                        using var msgScope = scopeFactory.CreateScope();
                        var msgRepo = msgScope.ServiceProvider.GetRequiredService<IOutboxRepository>();
                        var publisher = msgScope.ServiceProvider.GetRequiredService<IMessagePublisher>();

                        try
                        {
                            logger.LogInformation("About to publish {Type} with payload {Payload}", message.Type, message.Payload);
                            await publisher.PublishAsync(message.Type, message.Payload);
                            message.ProcessedAt = DateTime.UtcNow;

                        }
                        catch (Exception ex)
                        {
                            message.RetryCount++;
                            message.Error = ex.Message;

                            logger.LogError("OutboxWorker - ExecuteAsync: {Message}", ex);

                        }
                        finally {
                            await msgRepo.UpdateAsync(message);
                            await msgRepo.SaveChangesAsync();
                        }
                    });

                    await Task.WhenAll(tasks);

                } catch (Exception ex) {
                    
                    logger.LogCritical("OutboxWorker - ExecuteAsync: {Message}", ex.Message);
                }
                await Task.Delay(5000,ct);
            }
            

        }
    }
}
