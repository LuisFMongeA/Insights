using Insights.WeatherAPI.Model;

namespace Insights.WeatherAPI.Data;

public interface IWeatherData
{
    Task<WeatherDto?> GetWeatherByLocationAsync(double lat, double lon);
}