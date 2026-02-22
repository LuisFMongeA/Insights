namespace Insights.Gateway.Model
{
    using Newtonsoft.Json;
    public class WeatherDto
    {

            [JsonProperty("coord")]
            public Coord Coord { get; set; }

            [JsonProperty("weather")]
            public Weather[] Weather { get; set; }

            [JsonProperty("base")]
            public string Base { get; set; }

            [JsonProperty("main")]
            public Main Main { get; set; }

            [JsonProperty("visibility")]
            public long Visibility { get; set; }

            [JsonProperty("wind")]
            public Wind Wind { get; set; }

            [JsonProperty("clouds")]
            public Clouds Clouds { get; set; }

            [JsonProperty("dt")]
            public long Dt { get; set; }

            [JsonProperty("sys")]
            public Sys Sys { get; set; }

            [JsonProperty("timezone")]
            public long Timezone { get; set; }

            [JsonProperty("id")]
            public long Id { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("cod")]
            public long Cod { get; set; }
        }

        public partial class Clouds
        {
            [JsonProperty("all")]
            public long All { get; set; }
        }

        public partial class Coord
        {
            [JsonProperty("lon")]
            public double Lon { get; set; }

            [JsonProperty("lat")]
            public double Lat { get; set; }
        }

        public partial class Main
        {
            [JsonProperty("temp")]
            public float Temp { get; set; }

            [JsonProperty("feels_like")]
            public float FeelsLike { get; set; }

            [JsonProperty("temp_min")]
            public float TempMin { get; set; }

            [JsonProperty("temp_max")]
            public float TempMax { get; set; }

            [JsonProperty("pressure")]
            public long Pressure { get; set; }

            [JsonProperty("humidity")]
            public long Humidity { get; set; }

            [JsonProperty("sea_level")]
            public long SeaLevel { get; set; }

            [JsonProperty("grnd_level")]
            public long GrndLevel { get; set; }
        }

        public partial class Sys
        {
            [JsonProperty("type")]
            public long Type { get; set; }

            [JsonProperty("id")]
            public long Id { get; set; }

            [JsonProperty("country")]
            public string Country { get; set; }

            [JsonProperty("sunrise")]
            public long Sunrise { get; set; }

            [JsonProperty("sunset")]
            public long Sunset { get; set; }
        }

        public partial class Weather
        {
            [JsonProperty("id")]
            public long Id { get; set; }

            [JsonProperty("main")]
            public string Main { get; set; }

            [JsonProperty("description")]
            public string Description { get; set; }

            [JsonProperty("icon")]
            public string Icon { get; set; }
        }

        public partial class Wind
        {
            [JsonProperty("speed")]
            public float Speed { get; set; }

            [JsonProperty("deg")]
            public long Deg { get; set; }

            [JsonProperty("gust")]
            public double Gust { get; set; }
        }
    
}
