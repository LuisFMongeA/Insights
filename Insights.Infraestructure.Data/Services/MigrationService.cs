using Insights.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Insights.Infrastructure.Data.Services;

public class MigrationService(
    IServiceScopeFactory scopeFactory,
    ILogger<MigrationService> logger) : IHostedService
{
    public async Task StartAsync(CancellationToken ct)
    {
        logger.LogInformation("Applying database migrations...");

        using var scope = scopeFactory.CreateScope();
        var context = scope.ServiceProvider
            .GetRequiredService<InsightsDbContext>();

        await context.Database.MigrateAsync(ct);

        logger.LogInformation("Database migrations applied successfully");
    }

    public Task StopAsync(CancellationToken ct) => Task.CompletedTask;
}