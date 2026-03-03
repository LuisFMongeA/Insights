using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Insights.Domain.Models;

[Table("stat_entries")]
public class StatEntry
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("city_name")]
    public string CityName { get; set; } = string.Empty;

    [Column("country_code")]
    public string CountryCode { get; set; } = string.Empty;

    [Column("lat")]
    public double Lat { get; set; }

    [Column("lon")]
    public double Lon { get; set; }

    [Column("requested_at")]
    public DateTime RequestedAt { get; set; }
}