using Azure.Core;
using Insights.Gateway.HttpClients;
using Insights.Gateway.Model;

namespace Insights.Gateway.Strategies;

public class CityByNameStrategy(CitiesHttpClient citiesClient)
    : CityResolutionStrategyBase(citiesClient)
{
    public override bool CanHandle(GeoRequestDto request) 
        => !string.IsNullOrEmpty(request.CityName);

    public override async Task<CityDto> ResolveAsync(GeoRequestDto request) 
        => await CitiesClient.GetCitiesByNameAsync(request.CityName!);
    
}
