using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Insights.Domain.Models;

[Table("audit_entries")]
public class AuditEntry
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("requested_at")]
    public DateTime RequestedAt { get; set; }

    [Column("lat")]
    public double Lat { get; set; }

    [Column("lon")]
    public double Lon { get; set; }

    [Column("city_name")]
    public string? CityName { get; set; }

    [Column("resolved_city_name")]
    public string ResolvedCityName { get; set; } = string.Empty;

    [Column("country_code")]
    public string CountryCode { get; set; } = string.Empty;

    [Column("received_at")]
    public DateTime ReceivedAt { get; set; } = DateTime.UtcNow;
}