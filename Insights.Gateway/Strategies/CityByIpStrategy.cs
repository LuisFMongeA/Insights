using Insights.Gateway.HttpClients;
using Insights.Gateway.Model;

namespace Insights.Gateway.Strategies;

public class CityByIpStrategy(CitiesHttpClient citiesClient)
    : CityResolutionStrategyBase(citiesClient)
{
    public override bool CanHandle(GeoRequestDto request)
        => !string.IsNullOrEmpty(request.IpAddress);

    public override async Task<CityDto> ResolveAsync(GeoRequestDto request)
        => (await CitiesClient.GetCityByIpAddressAsync(request.IpAddress!))!;
}
