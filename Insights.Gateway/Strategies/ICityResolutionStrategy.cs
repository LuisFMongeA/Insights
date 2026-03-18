using Insights.Gateway.Model;

namespace Insights.Gateway.Strategies;

public interface ICityResolutionStrategy
{
    bool CanHandle(GeoRequestDto request);
    Task<CityDto> ResolveAsync(GeoRequestDto request);
}
