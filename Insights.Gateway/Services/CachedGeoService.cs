using Insights.Gateway.Model;
using Microsoft.Extensions.Caching.Memory;

namespace Insights.Gateway.Services;

public class CachedGeoService(
    IGeoService inner,
    IMemoryCache cache,
    ILogger<CachedGeoService> logger) : IGeoService
{
  
    public async Task<GeoInfoDto> GetGeoInfoAsync(GeoRequestDto request)
    {
        var key = $"geo:{request.CityName ?? request.IpAddress ?? $"{request.Lat},{request.Lon}"}";
        if (cache.TryGetValue(key, out GeoInfoDto? cached)) 
        {
            logger.LogInformation("Cache {Result} for key {Key}", "HIT", key);
            return cached!; // cache hit
        }

        logger.LogInformation("CachedGeoService - GeoInfoAsync - Asking value to external service");
        var result = await inner.GetGeoInfoAsync(request); // cache miss

        cache.Set(key, result, TimeSpan.FromHours(1));
        logger.LogInformation("Cache {Result} for key {Key}", "MISS", key);
        return result;
    }
}
