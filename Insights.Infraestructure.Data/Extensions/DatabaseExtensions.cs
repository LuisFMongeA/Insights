using Insights.Domain.Repositories;
using Insights.Infrastructure.Data.Context;
using Insights.Infrastructure.Data.Repositories;
using Insights.Infrastructure.Data.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Insights.Infrastructure.Data.Extensions;

public static class DatabaseExtensions
{
    public static IServiceCollection AddDatabase(
     this IServiceCollection services,
     IConfiguration configuration)
    {
        services.AddDbContext<InsightsDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("InsightsDb")));

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Las migraciones se aplican automáticamente al arrancar
        services.AddHostedService<MigrationService>();

        return services;
    }
}