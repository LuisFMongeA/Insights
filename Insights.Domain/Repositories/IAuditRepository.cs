using Insights.Domain.Models;

namespace Insights.Domain.Repositories;

public interface IAuditRepository
{
    Task AddAsync(AuditEntry entry);
    Task<IEnumerable<AuditEntry>> GetAllAsync();
    Task<AuditEntry?> GetByIdAsync(Guid id);
    Task<IEnumerable<AuditEntry>> GetByDateRangeAsync(DateTime from, DateTime to);
}