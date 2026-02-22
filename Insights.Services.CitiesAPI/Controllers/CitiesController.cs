using Insights.CitiesAPI.Data;
using Insights.CitiesAPI.Model;
using Microsoft.AspNetCore.Mvc;

namespace Insights.CitiesAPI.Controllers
{
    [ApiController]
    [Route("api/cities")]
    public class CitiesController : ControllerBase
    {
        private readonly ILogger<CitiesController> _logger;
        private readonly ICitiesData _citiesData;
        public CitiesController(ILogger<CitiesController> logger, ICitiesData citiesData)
        {
            _logger = logger;
            _citiesData = citiesData;
        }

        [HttpGet("{name}")]
        public ResponseDto Get(string name) => _citiesData.GetCityInfoByNameAsync(name).Result;

    }
}
