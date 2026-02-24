using System.Text.Json.Serialization;

namespace Insights.Gateway.Model;

public class CountryDto
{
    [JsonPropertyName("name")]
    public CountryName? Name { get; set; }

    [JsonPropertyName("tld")]
    public string[]? Tld { get; set; }

    [JsonPropertyName("cca2")]
    public string Cca2 { get; set; } = string.Empty;

    [JsonPropertyName("cca3")]
    public string Cca3 { get; set; } = string.Empty;

    [JsonPropertyName("cioc")]
    public string? Cioc { get; set; }

    [JsonPropertyName("independent")]
    public bool Independent { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("unMember")]
    public bool UnMember { get; set; }

    [JsonPropertyName("capital")]
    public string[]? Capital { get; set; }

    [JsonPropertyName("altSpellings")]
    public string[]? AltSpellings { get; set; }

    [JsonPropertyName("region")]
    public string Region { get; set; } = string.Empty;

    [JsonPropertyName("subregion")]
    public string? Subregion { get; set; }

    [JsonPropertyName("landlocked")]
    public bool Landlocked { get; set; }

    [JsonPropertyName("borders")]
    public string[]? Borders { get; set; }

    [JsonPropertyName("area")]
    public double Area { get; set; }

    [JsonPropertyName("flag")]
    public string Flag { get; set; } = string.Empty;

    [JsonPropertyName("population")]
    public long Population { get; set; }

    [JsonPropertyName("fifa")]
    public string? Fifa { get; set; }

    [JsonPropertyName("timezones")]
    public string[]? Timezones { get; set; }

    [JsonPropertyName("continents")]
    public string[]? Continents { get; set; }

    [JsonPropertyName("flags")]
    public Flags? Flags { get; set; }

    [JsonPropertyName("startOfWeek")]
    public string? StartOfWeek { get; set; }

    [JsonPropertyName("capitalInfo")]
    public CapitalInfo? CapitalInfo { get; set; }
}

public class CountryName
{
    [JsonPropertyName("common")]
    public string Common { get; set; } = string.Empty;

    [JsonPropertyName("official")]
    public string Official { get; set; } = string.Empty;
}

public class Flags
{
    [JsonPropertyName("png")]
    public string? Png { get; set; }

    [JsonPropertyName("svg")]
    public string? Svg { get; set; }

    [JsonPropertyName("alt")]
    public string? Alt { get; set; }
}

public class CapitalInfo
{
    [JsonPropertyName("latlng")]
    public double[]? Latlng { get; set; }
}