using Insights.CitiesAPI.Model;
using System.Text.Json;

namespace Insights.CitiesAPI.Data;

public class CitiesData(IHttpClientFactory httpClientFactory, IConfiguration configuration) : ICitiesData
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public async Task<List<CityDto>> GetCitiesByNameAsync(string cityName)
    {
        var client = httpClientFactory.CreateClient("ApiNinjas");

        var endpoint = configuration["ApiNinjas:Endpoints:GetCitiesByName"]!
            .Replace("##CITY##", Uri.EscapeDataString(cityName));

        using var message = new HttpRequestMessage(HttpMethod.Get, endpoint);
        message.Headers.Add("Accept", "application/json");

        var response = await client.SendAsync(message);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<CityDto>>(content, JsonOptions) ?? [];
    }
}