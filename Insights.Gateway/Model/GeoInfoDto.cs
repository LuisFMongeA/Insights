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
        public double Temperature { get; set; }
        public double MaxTemperature { get; set; }
        public double MinTemperature { get; set; }
        public double FeelLike { get; set; }
        public double Humidity { get; set; }
        public double WindSpeed { get; set; }
        public DateTime Sunrise { get; set; }
        public DateTime Sunset { get; set; }
    }
}
