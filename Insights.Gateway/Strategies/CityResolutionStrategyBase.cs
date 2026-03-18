using Insights.Gateway.HttpClients;
using Insights.Gateway.Model;

namespace Insights.Gateway.Strategies;

public abstract class CityResolutionStrategyBase(CitiesHttpClient citiesClient) : ICityResolutionStrategy
{
    protected CitiesHttpClient CitiesClient => citiesClient;
    public abstract bool CanHandle(GeoRequestDto request);


    public abstract Task<CityDto> ResolveAsync(GeoRequestDto request);
    
}
