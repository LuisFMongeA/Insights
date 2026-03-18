using Insights.CitiesAPI.Model;
using System.Globalization;
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

    public async Task<CityDto> GetCityByCoordAsync(double lat, double lon) 
    {
        var client = httpClientFactory.CreateClient("ApiNinjas");

        var endpoint = configuration["ApiNinjas:Endpoints:GetCitiesByCoordinates"]!
            .Replace("##LAT##", lat.ToString(CultureInfo.InvariantCulture))
            .Replace("##LON##", lon.ToString(CultureInfo.InvariantCulture));

        using var message = new HttpRequestMessage(HttpMethod.Get, endpoint);
        message.Headers.Add("Accept", "application/json");

        var response = await client.SendAsync(message);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        try 
        {
            CityDto res = JsonSerializer.Deserialize<CityDto>(content, JsonOptions);
            res.Latitude = lat;
            res.Longitude = lon;

            return res;
        } catch (Exception ex) 
        {
            return null;
        }
        
    }

    public async Task<CityDto> GetCityByIpAddressAsync(string ipAddress)
    {
        var client = httpClientFactory.CreateClient("IpApi");

        var response = await client.GetAsync(ipAddress);
        response.EnsureSuccessStatusCode();
        

        var content = await response.Content.ReadAsStringAsync();
        try
        {
            return JsonSerializer.Deserialize<CityDto>(content, JsonOptions);
        }
        catch (Exception ex)
        {
            return null;
        }

    }
}