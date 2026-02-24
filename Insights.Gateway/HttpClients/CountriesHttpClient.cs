
using Insights.Gateway.Model;
using System.Text.Json;

namespace Insights.Gateway.HttpClients;

public class CountriesHttpClient(HttpClient httpClient, IConfiguration configuration)
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public async Task<CountryDto?> GetCountryByCodeAsync(string countryCode)
    {
        var endpoint = configuration["ApiInfo:Countries:Endpoints:GetCountryByCode"]!
            .Replace("##COUNTRY##", Uri.EscapeDataString(countryCode));

        var response = await httpClient.GetAsync(endpoint);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<CountryDto?>(JsonOptions);
    }
}