using Insights.Gateway.Model;
using System.Text.Json;

namespace Insights.Gateway.HttpClients;

public class CitiesHttpClient(HttpClient httpClient, IConfiguration configuration)
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public async Task<CityDto> GetCitiesByNameAsync(string cityName)
    {
        var endpoint = configuration["ApiInfo:Cities:Endpoints:GetCitiesByName"]!
            .Replace("##CITY##", Uri.EscapeDataString(cityName));

        var response = await httpClient.GetAsync(endpoint);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<CityDto>(JsonOptions)
            ?? throw new Exception($"Empty response from CitiesAPI for: {cityName}");
    }

    public async Task<CityDto?> GetCityByCoordinatesAsync(double lat, double lon)
    {
        var endpoint = configuration["ApiInfo:Cities:Endpoints:GetCitiesByCoordinates"]!
            .Replace("##LAT##", lat.ToString())
            .Replace("##LON##", lon.ToString());

        var response = await httpClient.GetAsync(endpoint);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<CityDto?>(JsonOptions);
    }

    public async Task<CityDto?> GetCityByIpAddressAsync(string ipAddress)
    {
        CityDto res;
        var endpoint = configuration["ApiInfo:Cities:Endpoints:GetCityByIPAddress"]!
            .Replace("##IP_ADDRESS##", Uri.EscapeDataString(ipAddress));

        var response = await httpClient.GetAsync(endpoint);
        response.EnsureSuccessStatusCode();
        res = await response.Content.ReadFromJsonAsync<CityDto?>(JsonOptions);
        return res;
    }
}