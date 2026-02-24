using Insights.CitiesAPI.Data;
using Insights.CitiesAPI.Model;
using Microsoft.AspNetCore.Mvc;

namespace Insights.CitiesAPI.Controllers;

[ApiController]
[Route("api/cities")]
public class CitiesController(ICitiesData citiesData, ILogger<CitiesController> logger) : ControllerBase
{
    [HttpGet("{name}")]
    public async Task<ActionResult<List<CityDto>>> Get(string name)
    {
        var cities = await citiesData.GetCitiesByNameAsync(name);

        if (cities.Count == 0)
            return NotFound($"No cities found for: {name}");

        return Ok(cities);
    }

    [HttpGet("coordinates")]
    public async Task<ActionResult<CityDto>> GetCityByCoord(double lat, double lon)
    { 
        var city = await citiesData.GetCityByCoordAsync(lat, lon);
        if (city == null) return NotFound($"No city found for lat: {lat} and lon: {lon}");
        return Ok(city);
    }

    [HttpGet("iplookup")]
    public async Task<ActionResult<CityDto>> GetCityByIpAddress(string ipAddress)
    {
        var city = await citiesData.GetCityByIpAddressAsync(ipAddress);
        if (city == null) return NotFound($"No city found for ip address {ipAddress}");
        return Ok(city);
    }
}