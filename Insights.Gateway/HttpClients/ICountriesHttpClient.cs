using Insights.Gateway.Model;

namespace Insights.Gateway.HttpClients;

public interface ICountriesHttpClient
{
    Task<CountryDto?> GetCountryByCodeAsync(string countryCode);
}
