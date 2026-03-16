using Insights.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Insights.Infrastructure.Data.Context;

public class InsightsDbContext(DbContextOptions<InsightsDbContext> options)
    : DbContext(options)
{
    public DbSet<AuditEntry> AuditEntries => Set<AuditEntry>();
    public DbSet<StatEntry> StatEntries => Set<StatEntry>();
    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
        typeof(InsightsDbContext).Assembly);

    }
}