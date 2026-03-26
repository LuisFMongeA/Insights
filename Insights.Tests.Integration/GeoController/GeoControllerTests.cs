using Insights.Gateway.Model;
using Insights.Tests.Integration.Infraestrcture;
using Moq;
using System.Net;
using System.Net.Http.Headers;

namespace Insights.Tests.Integration.GeoController;

public class GeoControllerTests : IClassFixture<GatewayWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly GatewayWebApplicationFactory _factory;

    public GeoControllerTests(GatewayWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Get_SinToken_Devuelve401()
    {
        // Arrange — sin Authorization header
        _client.DefaultRequestHeaders.Authorization = null;
        // Act
        var response = await _client.GetAsync("/api/geo?CityName=Madrid");

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Get_SinParametros_Devuelve400()
    {
        // Arrange — añade el token fake
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Test", "fake-token");
        // Act
        var response = await _client.GetAsync("/api/geo");

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }   

    [Fact]
    public async Task Get_LatSinLon_Devuelve400()
    {
 
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Test", "fake-token");

        var response = await _client.GetAsync("/api/geo?Lat=40.4");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
  
    }

    [Fact]
    public async Task Get_CityNameValido_Devuelve200()
    {
        // Arrange — configura el mock de IGeoService
        _factory.MockGeoService
            .Setup(s => s.GetGeoInfoAsync(It.IsAny<GeoRequestDto>()))
            .ReturnsAsync(new GeoInfoDto { CityName = "Madrid" });
        
        _client.DefaultRequestHeaders.Authorization =
          new AuthenticationHeaderValue("Test", "fake-token");

        var response = await _client.GetAsync("/api/geo?CityName=Madrid");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
