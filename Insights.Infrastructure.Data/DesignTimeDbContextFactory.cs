using Insights.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Insights.Infrastructure.Data;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<InsightsDbContext>
{
    public InsightsDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true)
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<InsightsDbContext>();
        optionsBuilder.UseNpgsql(
            configuration.GetConnectionString("InsightsDb")
            ?? "Host=localhost;Database=insights;Username=postgres;Password=postgres");

        return new InsightsDbContext(optionsBuilder.Options);
    }
}