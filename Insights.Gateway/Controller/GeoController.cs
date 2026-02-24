using Insights.Contracts.Events;
using Insights.Gateway.HttpClients;
using Insights.Gateway.Model;
using Insights.MessageBus;
using Microsoft.AspNetCore.Mvc;

namespace Insights.Gateway.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GeoController(
    CitiesHttpClient citiesClient,
    CountriesHttpClient countriesClient,
    WeatherHttpClient weatherClient,
    IMessageBus messageBus) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<GeoInfoDto>> Get([FromQuery] GeoRequestDto request)
    {
        CityDto city;
        if (!string.IsNullOrEmpty(request.IpAddress))
        {
            city = await citiesClient.GetCityByIpAddressAsync(request.IpAddress);
        }
        else if (!string.IsNullOrEmpty(request.CityName))
        {
            city = await citiesClient.GetCitiesByNameAsync(request.CityName);
        }
        else if(request.Lat is not null && request.Lon is not null){ 
            city = await citiesClient.GetCityByCoordinatesAsync(request.Lat.Value, request.Lon.Value);
        }
        else
        {
            return NotFound("City not found");
        }            

        // 2. Llama a países y weather en paralelo
        var countryTask = countriesClient.GetCountryByCodeAsync(city.CountryCode);
        var weatherTask = weatherClient.GetWeatherByLocationAsync(city.Latitude, city.Longitude);

        await Task.WhenAll(countryTask, weatherTask);

        var country = await countryTask;
        var weather = await weatherTask;

        _ = messageBus.PublishAsync("geo.info.requested", new GeoInfoRequestedEvent
        {
            Lat = city.Latitude,
            Lon = city.Longitude,
            ParamCityName = request.CityName,
            ResolvedCityName = city.Name,
            CountryCode = city.CountryCode
        });

        // 3. Convierte unix timestamps a DateTime
        var sunrise = DateTimeOffset.FromUnixTimeSeconds(weather?.Sys?.Sunrise ?? 0).UtcDateTime;
        var sunset = DateTimeOffset.FromUnixTimeSeconds(weather?.Sys?.Sunset ?? 0).UtcDateTime;

        // 4. Agrega y devuelve
        return Ok(new GeoInfoDto
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
        });
    }
}