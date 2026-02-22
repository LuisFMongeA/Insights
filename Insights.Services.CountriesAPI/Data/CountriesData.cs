using Insights.CountriesAPI.Model;
using System.Text.Json;

namespace Insights.CountriesAPI.Data;

public class CountriesData(IHttpClientFactory httpClientFactory, IConfiguration configuration) : ICountriesData
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public async Task<CountryDto?> GetCountryByCodeAsync(string countryCode)
    {
        var client = httpClientFactory.CreateClient("RestCountries");

        var endpoint = configuration["RestCountries:Endpoints:GetCountryByCode"]!
            .Replace("##COUNTRY##", Uri.EscapeDataString(countryCode));

        var response = await client.GetAsync(endpoint);

        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            return null;

        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();

        // RestCountries devuelve siempre un array, cogemos el primero
        var countries = JsonSerializer.Deserialize<List<CountryDto>>(content, JsonOptions);
        return countries?.FirstOrDefault();
    }
}