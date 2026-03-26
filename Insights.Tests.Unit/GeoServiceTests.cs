using FluentAssertions;
using Insights.Domain.Models;
using Insights.Domain.Repositories;
using Insights.Gateway.HttpClients;
using Insights.Gateway.Model;
using Insights.Gateway.Services;
using Insights.Gateway.Strategies;
using Microsoft.Extensions.Logging;
using Moq;

namespace Insights.Tests.Unit.GeoService;

public class GeoServiceTests
{
    private readonly Mock<ICityResolutionStrategy> _mockStrategy;
    private readonly Mock<ICountriesHttpClient> _mockCountriesClient;
    private readonly Mock<IWeatherHttpClient> _mockWeatherClient;
    private readonly Mock<IOutboxRepository> _mockOutboxRepository;
    private readonly Mock<ILogger<Insights.Gateway.Services.GeoService>> _mockLogger;
    private readonly Insights.Gateway.Services.GeoService _sut;

    public GeoServiceTests()
    {
        _mockStrategy = new Mock<ICityResolutionStrategy>();
        _mockCountriesClient = new Mock<ICountriesHttpClient>();
        _mockWeatherClient = new Mock<IWeatherHttpClient>();
        _mockOutboxRepository = new Mock<IOutboxRepository>();
        _mockLogger = new Mock<ILogger<Insights.Gateway.Services.GeoService>>();

        // Ciudad por defecto
        _mockStrategy
            .Setup(s => s.CanHandle(It.IsAny<GeoRequestDto>()))
            .Returns(true);

        _mockStrategy
            .Setup(s => s.ResolveAsync(It.IsAny<GeoRequestDto>()))
            .ReturnsAsync(new CityDto
            {
                Name = "Madrid",
                CountryCode = "ES",
                Latitude = 40.4168,
                Longitude = -3.7038,
                Population = 3223000
            });

        // País por defecto
        _mockCountriesClient
            .Setup(c => c.GetCountryByCodeAsync(It.IsAny<string>()))
            .ReturnsAsync(new CountryDto
            {
                Name = new CountryName { Common = "Spain" },
                Region = "Europe",
                Subregion = "Southern Europe",
                Flag = "🇪🇸"
            });

        // Tiempo por defecto
        _mockWeatherClient
            .Setup(w => w.GetWeatherByLocationAsync(It.IsAny<double>(), It.IsAny<double>()))
            .ReturnsAsync(new WeatherDto
            {
                Main = new Main { Temp = 22, TempMax = 25, TempMin = 18, FeelsLike = 21, Humidity = 60 },
                Wind = new Wind { Speed = 10 },
                Weather = new[] { new WeatherCondition { Description = "Sunny" } },
                Sys = new Sys { Sunrise = 1710000000, Sunset = 1710040000 }
            });

        // OutboxRepository por defecto
        _mockOutboxRepository
            .Setup(r => r.AddAsync(It.IsAny<OutboxMessage>()))
            .Returns(Task.CompletedTask);

        _mockOutboxRepository
            .Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Creamos el SUT
        _sut = new Insights.Gateway.Services.GeoService(
            new[] { _mockStrategy.Object },
            _mockCountriesClient.Object,
            _mockWeatherClient.Object,
            _mockOutboxRepository.Object,
            _mockLogger.Object);
    }

    // ✅ Happy path
    [Fact]
    public async Task GetGeoInfoAsync_ValidRequest_ReturnsGeoInfoDto()
    {
        // Arrange
        var request = new GeoRequestDto { CityName = "Madrid" };

        // Act
        var result = await _sut.GetGeoInfoAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.CityName.Should().Be("Madrid");
        result.CountryName.Should().Be("Spain");
        result.Temperature.Should().Be(22);
    }

    // ✅ Verifica que se guardó el OutboxMessage
    [Fact]
    public async Task GetGeoInfoAsync_ValidRequest_SavesOutboxMessage()
    {
        // Arrange
        var request = new GeoRequestDto { CityName = "Madrid" };

        // Act
        await _sut.GetGeoInfoAsync(request);

        // Assert — verifica que se llamó a AddAsync exactamente una vez
        _mockOutboxRepository.Verify(
            r => r.AddAsync(It.IsAny<OutboxMessage>()),
            Times.Once);

        _mockOutboxRepository.Verify(
            r => r.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once);
    }

    // ❌ Sin estrategia válida
    [Fact]
    public async Task GetGeoInfoAsync_NoStrategyFound_ThrowsInvalidOperationException()
    {
        // Arrange — estrategia que no puede manejar nada
        _mockStrategy
            .Setup(s => s.CanHandle(It.IsAny<GeoRequestDto>()))
            .Returns(false);

        var request = new GeoRequestDto { CityName = "Madrid" };

        // Act
        var act = async () => await _sut.GetGeoInfoAsync(request);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    // ❌ CountriesAPI falla
    [Fact]
    public async Task GetGeoInfoAsync_CountriesApiFails_ThrowsHttpRequestException()
    {
        // Arrange — sobreescribe el setup del constructor
        _mockCountriesClient
            .Setup(c => c.GetCountryByCodeAsync(It.IsAny<string>()))
            .ThrowsAsync(new HttpRequestException("CountriesAPI unavailable"));

        var request = new GeoRequestDto { CityName = "Madrid" };

        // Act
        var act = async () => await _sut.GetGeoInfoAsync(request);

        // Assert
        await act.Should().ThrowAsync<HttpRequestException>()
            .WithMessage("CountriesAPI unavailable");
    }

    // ❌ WeatherAPI falla
    [Fact]
    public async Task GetGeoInfoAsync_WeatherApiFails_ThrowsHttpRequestException()
    {
        // Arrange
        _mockWeatherClient
            .Setup(w => w.GetWeatherByLocationAsync(It.IsAny<double>(), It.IsAny<double>()))
            .ThrowsAsync(new HttpRequestException("WeatherAPI unavailable"));

        var request = new GeoRequestDto { CityName = "Madrid" };

        // Act
        var act = async () => await _sut.GetGeoInfoAsync(request);

        // Assert
        await act.Should().ThrowAsync<HttpRequestException>()
            .WithMessage("WeatherAPI unavailable");
    }

    // ⚠️ Country devuelve null — no debe petar
    [Fact]
    public async Task GetGeoInfoAsync_CountryIsNull_ReturnsEmptyCountryFields()
    {
        // Arrange
        _mockCountriesClient
            .Setup(c => c.GetCountryByCodeAsync(It.IsAny<string>()))
            .ReturnsAsync((CountryDto?)null);

        var request = new GeoRequestDto { CityName = "Madrid" };

        // Act
        var result = await _sut.GetGeoInfoAsync(request);

        // Assert — no peta y devuelve valores por defecto
        result.Should().NotBeNull();
        result.CountryName.Should().BeEmpty();
        result.Region.Should().BeEmpty();
    }

    // ⚠️ Weather devuelve null — no debe petar
    [Fact]
    public async Task GetGeoInfoAsync_WeatherIsNull_ReturnsZeroWeatherFields()
    {
        // Arrange
        _mockWeatherClient
            .Setup(w => w.GetWeatherByLocationAsync(It.IsAny<double>(), It.IsAny<double>()))
            .ReturnsAsync((WeatherDto?)null);

        var request = new GeoRequestDto { CityName = "Madrid" };

        // Act
        var result = await _sut.GetGeoInfoAsync(request);

        // Assert — no peta y devuelve ceros
        result.Should().NotBeNull();
        result.Temperature.Should().Be(0);
        result.WindSpeed.Should().Be(0);
        result.Humidity.Should().Be(0);
    }
}