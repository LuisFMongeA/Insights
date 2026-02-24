using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Insights.Gateway.Model
{
    public class CityDto
    {
        [JsonPropertyName("city")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("lat")]
        public double Latitude { get; set; }

        [JsonPropertyName("lon")]
        public double Longitude { get; set; }

        [JsonPropertyName("country")]
        public string Country { get; set; } = string.Empty;

        [JsonPropertyName("countryCode")]
        public string CountryCode { get; set; } = string.Empty;

        [JsonPropertyName("population")]
        public long Population { get; set; }

        [JsonPropertyName("is_capital")]
        public bool IsCapital { get; set; }
    }
}
