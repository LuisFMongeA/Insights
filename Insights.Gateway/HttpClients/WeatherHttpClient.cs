using Insights.Gateway.Model;
using System.Text.Json;

namespace Insights.Gateway.HttpClients;

public class WeatherHttpClient(HttpClient httpClient, IConfiguration configuration)
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public async Task<WeatherDto?> GetWeatherByLocationAsync(double lat, double lon)
    {
        var endpoint = configuration["ApiInfo:Weather:Endpoints:GetWeatherByLocation"]!
            .Replace("##LAT##", lat.ToString())
            .Replace("##LON##", lon.ToString());

        var response = await httpClient.GetAsync(endpoint);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<WeatherDto?>(JsonOptions);
    }
}