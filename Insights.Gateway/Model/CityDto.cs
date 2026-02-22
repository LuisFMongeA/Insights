using Newtonsoft.Json;

namespace Insights.Gateway.Model
{
    public class CityDto
    {
        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;

        [JsonProperty("latitude")]
        public double Latitude { get; set; }

        [JsonProperty("longitude")]
        public double Longitude { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; } = string.Empty;

        [JsonProperty("population")]
        public long Population { get; set; }

        [JsonProperty("is_capital")]
        public bool IsCapital { get; set; }
    }
}
