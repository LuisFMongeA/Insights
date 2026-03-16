namespace Insights.Contracts.Events;

public class GeoInfoRequestedEvent
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
    public double Lat { get; set; }
    public double Lon { get; set; }
    public string? ParamCityName { get; set; } = string.Empty;
    public string ResolvedCityName { get; set; } = string.Empty;
    public string CountryCode { get; set; } = string.Empty;
}
