using Insights.CountriesAPI.Model;

namespace Insights.CountriesAPI.Data;

public interface ICountriesData
{
    Task<CountryDto?> GetCountryByCodeAsync(string countryCode);
}