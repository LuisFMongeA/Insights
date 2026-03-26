using Insights.Domain.ValueObjects;

namespace Insights.Domain.Models;

public class StatEntry
{
    public Guid Id { get; private set; }
    public Coordinates Coordinates { get; private set; }
    public CountryCode CountryCode { get; private set; }
    public string CityName { get; private set; }    
    public DateTime RequestedAt { get; private set; }
    private StatEntry() { }
    public static StatEntry Create(double lat, double lon, string cityName, string countryCode, DateTime requestedAt) 
    {
        ArgumentNullException.ThrowIfNullOrEmpty(cityName, nameof(cityName));
        return new StatEntry
        {
            Id = Guid.NewGuid(),
            Coordinates = Coordinates.Create(lat, lon),
            CountryCode = CountryCode.Create(countryCode),
            CityName = cityName,
            RequestedAt = requestedAt
        };
    }
}