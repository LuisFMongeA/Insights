using Insights.Domain.Models;
using Insights.Domain.Repositories;
using Insights.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Insights.Infrastructure.Data.Repositories;

public class OutboxRepository(InsightsDbContext context) : IOutboxRepository
{
    public async Task AddAsync(OutboxMessage entry) => await context.OutboxMessages.AddAsync(entry);

    public async Task<IEnumerable<OutboxMessage>> GetPendingAsync(int batchsize = 50)
    => await context.OutboxMessages.Where(e => e.ProcessedAt == null)
        .OrderBy(e => e.CreatedAt)
        .Take(batchsize).ToListAsync();

    public Task UpdateAsync(OutboxMessage entry)
    {
        context.OutboxMessages.Update(entry);
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync(CancellationToken ct = default) => context.SaveChangesAsync(ct);
    
}
