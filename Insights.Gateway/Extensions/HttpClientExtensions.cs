using Insights.Gateway.HttpClients;

namespace Insights.Gateway.Extensions;

public static class HttpClientExtensions
{
    public static IServiceCollection AddHttpClients(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var internalApiKey = configuration["InternalApiKey"]
            ?? throw new InvalidOperationException("InternalApiKey not configured");

        services.AddHttpClient<CitiesHttpClient>(client =>
        {
            client.BaseAddress = new Uri(configuration["ApiInfo:Cities:BaseUrl"]!);
            client.DefaultRequestHeaders.Add("X-Api-Key", internalApiKey);
        });

        services.AddHttpClient<CountriesHttpClient>(client =>
        {
            client.BaseAddress = new Uri(configuration["ApiInfo:Countries:BaseUrl"]!);
            client.DefaultRequestHeaders.Add("X-Api-Key", internalApiKey);
        });

        services.AddHttpClient<WeatherHttpClient>(client =>
        {
            client.BaseAddress = new Uri(configuration["ApiInfo:Weather:BaseUrl"]!);
            client.DefaultRequestHeaders.Add("X-Api-Key", internalApiKey);
        });

        return services;
    }
}