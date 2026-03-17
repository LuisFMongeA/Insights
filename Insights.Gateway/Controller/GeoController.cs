using Insights.Contracts.Events;
using Insights.Gateway.Filters;
using Insights.Gateway.HttpClients;
using Insights.Gateway.Model;
using Insights.Gateway.Services;
using Insights.MessageBus;
using Insights.SharedKernel.Attributes;
using Insights.SharedKernel.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog.Core;

namespace Insights.Gateway.Controllers;
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class GeoController(IGeoService geoService) : ControllerBase
{
    [HttpGet]
    [ValidateDto<GeoRequestDto>]
    [RequireScope(AuthConstants.Scopes.GeoRead)]
    public async Task<ActionResult<GeoInfoDto>> Get([FromQuery] GeoRequestDto request)
    {
        var result = await geoService.GetGeoInfoAsync(request);
        return Ok(result);
    }
}