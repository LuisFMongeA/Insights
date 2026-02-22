using Insights.Services.AuditAPI.Model;
using System.Collections.Concurrent;

namespace Insights.AuditAPI.Data;

public class InMemoryAuditRepository : IAuditRepository
{
    private readonly ConcurrentDictionary<Guid, AuditEntry> _entries = new();

    public Task SaveAsync(AuditEntry entry)
    {
        _entries[entry.Id] = entry;
        return Task.CompletedTask;
    }

    public Task<IEnumerable<AuditEntry>> GetAllAsync()
    {
        return Task.FromResult(_entries.Values.AsEnumerable());
    }

    public Task<AuditEntry?> GetByIdAsync(Guid id)
    {
        _entries.TryGetValue(id, out var entry);
        return Task.FromResult(entry);
    }
}