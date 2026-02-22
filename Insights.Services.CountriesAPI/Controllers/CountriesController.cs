using Insights.CountriesAPI.Data;
using Insights.CountriesAPI.Model;
using Microsoft.AspNetCore.Mvc;

namespace Insights.CountriesAPI.Controllers;

[ApiController]
[Route("api/countries")]
public class CountriesController(ICountriesData countriesData) : ControllerBase
{
    [HttpGet("{code}")]
    public async Task<ActionResult<CountryDto>> Get(string code)
    {
        var country = await countriesData.GetCountryByCodeAsync(code);

        if (country is null)
            return NotFound($"Country not found for code: {code}");

        return Ok(country);
    }
}