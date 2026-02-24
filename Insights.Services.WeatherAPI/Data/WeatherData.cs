using Insights.WeatherAPI.Model;
using System.Text.Json;

namespace Insights.WeatherAPI.Data;

public class WeatherData(IHttpClientFactory httpClientFactory, IConfiguration configuration) : IWeatherData
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public async Task<WeatherDto?> GetWeatherByLocationAsync(double lat, double lon)
    {
        WeatherDto res;
        var client = httpClientFactory.CreateClient("OpenWeather");

        var endpoint = configuration["OpenWeather:Endpoints:GetWeatherByLocation"]!
            .Replace("##LAT##", lat.ToString(System.Globalization.CultureInfo.InvariantCulture))
            .Replace("##LON##", lon.ToString(System.Globalization.CultureInfo.InvariantCulture))
            .Replace("##APIKEY##", configuration["OpenWeather:ApiKey"]);

        var response = await client.GetAsync(endpoint);
        response.EnsureSuccessStatusCode();

        res = await response.Content.ReadFromJsonAsync<WeatherDto>(JsonOptions);
        return res;
    }
}