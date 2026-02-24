using Insights.Gateway.Model;
using System.Globalization;
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
        WeatherDto res;
        var endpoint = configuration["ApiInfo:Weather:Endpoints:GetWeatherByLocation"]!
            .Replace("##LAT##", lat.ToString(CultureInfo.InvariantCulture))
            .Replace("##LON##", lon.ToString(CultureInfo.InvariantCulture));

        var response = await httpClient.GetAsync(endpoint);
        response.EnsureSuccessStatusCode();
        res = await response.Content.ReadFromJsonAsync<WeatherDto?>(JsonOptions);
        return res;
    }
}