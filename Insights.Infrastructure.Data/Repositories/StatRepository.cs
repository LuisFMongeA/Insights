using Insights.Domain.Models;
using Insights.Domain.Repositories;
using Insights.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Insights.Infrastructure.Data.Repositories;

public class StatRepository(InsightsDbContext context) : IStatRepository
{
    public async Task AddAsync(StatEntry entry)
        => await context.StatEntries.AddAsync(entry);

    public async Task<IEnumerable<StatEntry>> GetAllAsync()
        => await context.StatEntries
            .OrderByDescending(e => e.RequestedAt)
            .ToListAsync();

    public async Task<IEnumerable<StatEntry>> GetTopCitiesAsync(int count)
        => await context.StatEntries
            .GroupBy(e => e.CityName)
            .OrderByDescending(g => g.Count())
            .Take(count)
            .Select(g => g.First())
            .ToListAsync();

    public async Task<IEnumerable<StatEntry>> GetTopCountriesAsync(int count)
        => await context.StatEntries
            .GroupBy(e => e.CountryCode)
            .OrderByDescending(g => g.Count())
            .Take(count)
            .Select(g => g.First())
            .ToListAsync();
}