using Insights.Gateway.HttpClients;
using Insights.Gateway.Model;

namespace Insights.Gateway.Strategies;

public class CityByCoordinatesStrategy(CitiesHttpClient citiesHttpClient) : CityResolutionStrategyBase(citiesHttpClient)
{
    public override bool CanHandle(GeoRequestDto request)
        => request.Lat is not null && request.Lon is not null;

    public override async Task<CityDto> ResolveAsync(GeoRequestDto request)
        => (await CitiesClient.GetCityByCoordinatesAsync(request.Lat!.Value!, request.Lon!.Value!))!;
}
