using Insights.Gateway.Model;

namespace Insights.Gateway.HttpClients;

public interface IWeatherHttpClient
{
    Task<WeatherDto?> GetWeatherByLocationAsync(double lat, double lon);
}
