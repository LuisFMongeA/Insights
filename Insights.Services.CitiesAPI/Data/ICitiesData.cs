using Insights.CitiesAPI.Model;

namespace Insights.CitiesAPI.Data
{
    public interface ICitiesData
    {
        Task<List<CityDto>> GetCitiesByNameAsync(string cityName);
    }
}
