using System.Text.Json.Serialization;

namespace Insights.WeatherAPI.Model;

public class WeatherDto
{
    [JsonPropertyName("coord")]
    public Coord? Coord { get; set; }

    [JsonPropertyName("weather")]
    public WeatherCondition[]? Weather { get; set; }

    [JsonPropertyName("base")]
    public string Base { get; set; } = string.Empty;

    [JsonPropertyName("main")]
    public Main? Main { get; set; }

    [JsonPropertyName("visibility")]
    public long Visibility { get; set; }

    [JsonPropertyName("wind")]
    public Wind? Wind { get; set; }

    [JsonPropertyName("clouds")]
    public Clouds? Clouds { get; set; }

    [JsonPropertyName("dt")]
    public long Dt { get; set; }

    [JsonPropertyName("sys")]
    public Sys? Sys { get; set; }

    [JsonPropertyName("timezone")]
    public long Timezone { get; set; }

    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("cod")]
    public long Cod { get; set; }
}

public class Coord
{
    [JsonPropertyName("lon")]
    public double Lon { get; set; }

    [JsonPropertyName("lat")]
    public double Lat { get; set; }
}

public class Main
{
    [JsonPropertyName("temp")]
    public double Temp { get; set; }

    [JsonPropertyName("feels_like")]
    public double FeelsLike { get; set; }

    [JsonPropertyName("temp_min")]
    public double TempMin { get; set; }

    [JsonPropertyName("temp_max")]
    public double TempMax { get; set; }

    [JsonPropertyName("pressure")]
    public long Pressure { get; set; }

    [JsonPropertyName("humidity")]
    public long Humidity { get; set; }

    [JsonPropertyName("sea_level")]
    public long SeaLevel { get; set; }

    [JsonPropertyName("grnd_level")]
    public long GrndLevel { get; set; }
}

public class Sys
{
    [JsonPropertyName("country")]
    public string Country { get; set; } = string.Empty;

    [JsonPropertyName("sunrise")]
    public long Sunrise { get; set; }

    [JsonPropertyName("sunset")]
    public long Sunset { get; set; }
}

public class WeatherCondition
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("main")]
    public string Main { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("icon")]
    public string Icon { get; set; } = string.Empty;
}

public class Wind
{
    [JsonPropertyName("speed")]
    public double Speed { get; set; }

    [JsonPropertyName("deg")]
    public long Deg { get; set; }

    [JsonPropertyName("gust")]
    public double Gust { get; set; }
}

public class Clouds
{
    [JsonPropertyName("all")]
    public long All { get; set; }
}