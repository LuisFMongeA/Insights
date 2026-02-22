namespace Insights.Services.AuditAPI.Model;

public class AuditEntry
{
    public Guid Id { get; set; }
    public DateTime RequestedAt { get; set; }
    public double Lat { get; set; }
    public double Lon { get; set; }
    public string? CityName { get; set; }
    public string ResolvedCityName { get; set; } = string.Empty;
    public string CountryCode { get; set; } = string.Empty;
    public DateTime ReceivedAt { get; set; } = DateTime.UtcNow;
}