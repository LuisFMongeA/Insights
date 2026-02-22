namespace Insights.Gateway.Model
{

    public class GeoInfoDto
    {
        public string CityName { get; set; } = string.Empty;
        public long CityPopulation { get; set; }
        public string CountryCode { get; set; } = string.Empty;
        public string CountryName { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
        public string Subregion { get; set; } = string.Empty;
        public string Flag { get; set; } = string.Empty;
        public string WeatherDescription { get; set; } = string.Empty;
        public float Temperature { get; set; }
        public float MaxTemperature { get; set; }
        public float MinTemperature { get; set; }
        public float FeelLike { get; set; }
        public float Humidity { get; set; }
        public float WindSpeed { get; set; }
        public DateTime Sunrise { get; set; }
        public DateTime Sunset { get; set; }
    }
}
