using Insights.Gateway.HttpClients;

namespace Insights.Gateway.Extensions;

public static class HttpClientExtensions
{
    public static IServiceCollection AddHttpClients(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddHttpClient<CitiesHttpClient>(client =>
            client.BaseAddress = new Uri(configuration["ApiInfo:Cities:BaseUrl"]!));

        services.AddHttpClient<CountriesHttpClient>(client =>
            client.BaseAddress = new Uri(configuration["ApiInfo:Countries:BaseUrl"]!));

        services.AddHttpClient<WeatherHttpClient>(client =>
            client.BaseAddress = new Uri(configuration["ApiInfo:Weather:BaseUrl"]!));

        return services;
    }
}