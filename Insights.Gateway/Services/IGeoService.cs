using Insights.Gateway.Model;

namespace Insights.Gateway.Services;

public interface IGeoService
{
    Task<GeoInfoDto> GetGeoInfoAsync(GeoRequestDto request);
}