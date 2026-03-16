using Insights.Domain.Repositories;
using Insights.Domain.Services;
using Insights.Infrastructure.Data.Context;
using Insights.Infrastructure.Data.Repositories;
using Insights.Infrastructure.Data.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text.RegularExpressions;

namespace Insights.Infrastructure.Data.Extensions;

public static class DatabaseExtensions
{
    public static IServiceCollection AddDatabase(
     this IServiceCollection services,
     IConfiguration configuration)
    {
        services.AddDbContext<InsightsDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("InsightsDb")));

        // Repositorios
        services.AddScoped<IAuditRepository, AuditRepository>();
        services.AddScoped<IStatRepository, StatRepository>();

        // Lazy wrappers
        services.AddScoped<Lazy<IAuditRepository>>(sp =>
            new Lazy<IAuditRepository>(() =>
                sp.GetRequiredService<IAuditRepository>()));
        services.AddScoped<Lazy<IStatRepository>>(sp =>
            new Lazy<IStatRepository>(() =>
                sp.GetRequiredService<IStatRepository>()));

        RegisterRepositories(services);

        // UnitOfWork
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddSingleton<IDatabaseReadyService, DatabaseReadyService>();

        // Las migraciones se aplican automáticamente al arrancar
        services.AddHostedService<MigrationService>();
        services.AddHostedService<OutboxWorker>();

        return services;
    }

    private static void RegisterRepositories(IServiceCollection services) {

        var pattern = @"^(?!I)[A-Z][\w]+Repository$";

        var assembly = typeof(InsightsDbContext).Assembly;
        var repositories = assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && Regex.IsMatch(t.Name,pattern));

        foreach (var repo in repositories)
        {
            var interfaceType = repo.GetInterfaces()
                .FirstOrDefault(i => i.Name == $"I{repo.Name}");
            if (interfaceType == null)  continue;
            
            services.AddScoped(interfaceType, repo);
        }

    }
}