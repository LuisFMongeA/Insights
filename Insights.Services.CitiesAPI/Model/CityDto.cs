using System.Text.Json.Serialization;

namespace Insights.CitiesAPI.Model
{
    public class CityDto
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("latitude")]
        public double Latitude { get; set; }

        [JsonPropertyName("longitude")]
        public double Longitude { get; set; }

        [JsonPropertyName("country")]
        public string Country { get; set; } = string.Empty;

        [JsonPropertyName("population")]
        public long Population { get; set; }

        [JsonPropertyName("is_capital")]
        public bool IsCapital { get; set; }
    }
}
