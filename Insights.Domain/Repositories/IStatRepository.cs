using Insights.Domain.Models;

namespace Insights.Domain.Repositories;

public interface IStatRepository
{
    Task AddAsync(StatEntry entry);
    Task<IEnumerable<StatEntry>> GetAllAsync();
    Task<IEnumerable<StatEntry>> GetTopCitiesAsync(int count);
    Task<IEnumerable<StatEntry>> GetTopCountriesAsync(int count);
}