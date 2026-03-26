using Insights.Domain.ValueObjects;

namespace Insights.Domain.Models;

public class AuditEntry
{
    public Guid Id { get; private set; }
    public DateTime RequestedAt { get; private set; }
    public Coordinates Coordinates { get; private set; }
    public string? CityName { get; private set; }
    public string ResolvedCityName { get; private set; } = string.Empty;
    public CountryCode CountryCode { get; private set; }
    public DateTime ReceivedAt { get; private set; } // ← sin inicializador

    private AuditEntry() { }

    public static AuditEntry Create(double lat, double lon, string cityName, string resolvedCityName, 
        string countryCode, DateTime requestedAt) 
    {
        ArgumentNullException.ThrowIfNullOrEmpty(resolvedCityName, nameof(resolvedCityName));
        ArgumentNullException.ThrowIfNull(countryCode, nameof(countryCode));
        return new AuditEntry
        {
            Id = Guid.NewGuid(),
            RequestedAt = requestedAt,
            Coordinates = Coordinates.Create(lat,lon),
            CityName = cityName,
            ResolvedCityName = resolvedCityName,
            CountryCode = CountryCode.Create(countryCode),
            ReceivedAt = DateTime.UtcNow
        };
    }
}