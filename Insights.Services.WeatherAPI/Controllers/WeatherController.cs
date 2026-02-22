using Insights.WeatherAPI.Data;
using Insights.WeatherAPI.Model;
using Microsoft.AspNetCore.Mvc;

namespace Insights.WeatherAPI.Controllers;

[ApiController]
[Route("api/weather")]
public class WeatherController(IWeatherData weatherData) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<WeatherDto>> Get([FromQuery] double lat, [FromQuery] double lon)
    {
        var weather = await weatherData.GetWeatherByLocationAsync(lat, lon);

        if (weather is null)
            return NotFound($"Weather not found for coordinates: {lat}, {lon}");

        return Ok(weather);
    }
}