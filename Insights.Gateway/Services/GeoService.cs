using Insights.Contracts.Events;
using Insights.Contracts.RabbitMq;
using Insights.Domain.Models;
using Insights.Domain.Repositories;
using Insights.Gateway.HttpClients;
using Insights.Gateway.Model;
using Insights.SharedKernel.Constants;
using System.Text.Json;

namespace Insights.Gateway.Services;

public class GeoService(
    CitiesHttpClient citiesClient,
    CountriesHttpClient countriesClient,
    WeatherHttpClient weatherClient,
    IOutboxRepository  outboxRepository,
    ILogger<GeoService> logger) : IGeoService
{
    public async Task<GeoInfoDto> GetGeoInfoAsync(GeoRequestDto request)
    {

        var city = request switch
        {
            { CityName: not null and not "" } =>
                await citiesClient.GetCitiesByNameAsync(request.CityName),
            { Lat: not null, Lon: not null } =>
                await citiesClient.GetCityByCoordinatesAsync(request.Lat.Value, request.Lon.Value),
            _ =>
                await citiesClient.GetCityByIpAddressAsync(request.IpAddress!)
        };

        var countryTask = countriesClient.GetCountryByCodeAsync(city.CountryCode);
        var weatherTask = weatherClient.GetWeatherByLocationAsync(city.Latitude, city.Longitude);

        await Task.WhenAll(countryTask, weatherTask);

        var country = await countryTask;
        var weather = await weatherTask;

        // 3. Convierte unix timestamps a DateTime
        var sunrise = DateTimeOffset.FromUnixTimeSeconds(weather?.Sys?.Sunrise ?? 0).UtcDateTime;
        var sunset = DateTimeOffset.FromUnixTimeSeconds(weather?.Sys?.Sunset ?? 0).UtcDateTime;

        // 4. Agrega y devuelve
        var geoInfo =new GeoInfoDto
        {
            CityName = city.Name,
            CityPopulation = city.Population,
            CountryCode = city.CountryCode,
            CountryName = country?.Name?.Common ?? string.Empty,
            Region = country?.Region ?? string.Empty,
            Subregion = country?.Subregion ?? string.Empty,
            Flag = country?.Flag ?? string.Empty,
            WeatherDescription = weather?.Weather?.FirstOrDefault()?.Description ?? string.Empty,
            Temperature = weather?.Main?.Temp ?? 0,
            MaxTemperature = weather?.Main?.TempMax ?? 0,
            MinTemperature = weather?.Main?.TempMin ?? 0,
            FeelLike = weather?.Main?.FeelsLike ?? 0,
            Humidity = weather?.Main?.Humidity ?? 0,
            WindSpeed = weather?.Wind?.Speed ?? 0,
            Sunrise = sunrise,
            Sunset = sunset
        };


        await outboxRepository.AddAsync(new OutboxMessage
        {
            Id = Guid.NewGuid(),
            Type = RabbitMqRoutingKeys.GeoRequested,
            Payload = JsonSerializer.Serialize(new GeoInfoRequestedEvent
            {
                Id= Guid.NewGuid(),
                RequestedAt = DateTime.UtcNow,
                Lat = city.Latitude,
                Lon = city.Longitude,
                ParamCityName = request.CityName,
                ResolvedCityName = city.Name,
                CountryCode = city.Country
            }),
            CreatedAt = DateTime.UtcNow
        });

        await outboxRepository.SaveChangesAsync();

        return geoInfo;
    }
}