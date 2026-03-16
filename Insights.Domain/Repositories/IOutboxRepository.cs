using Insights.Domain.Models;

namespace Insights.Domain.Repositories;

public interface IOutboxRepository
{
    Task AddAsync(OutboxMessage entry);
    Task<IEnumerable<OutboxMessage>> GetPendingAsync(int batchsize=50);
    Task UpdateAsync(OutboxMessage entry);

    Task SaveChangesAsync(CancellationToken ct = default);

}