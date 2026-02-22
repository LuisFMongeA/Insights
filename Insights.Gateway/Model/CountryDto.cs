namespace Insights.Gateway.Model
{
    using System;
    using Newtonsoft.Json;

    public partial class CountryDto
    {
        [JsonProperty("name")]
        public Name? Name { get; set; }

        [JsonProperty("tld")]
        public string[]? Tld { get; set; }

        [JsonProperty("cca2")]
        public string Cca2 { get; set; }

        [JsonProperty("cca3")]
        public string Cca3 { get; set; }

        [JsonProperty("cioc")]
        public string Cioc { get; set; }

        [JsonProperty("independent")]
        public bool Independent { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("unMember")]
        public bool UnMember { get; set; }

        [JsonProperty("capital")]
        public string[] Capital { get; set; }

        [JsonProperty("altSpellings")]
        public string[] AltSpellings { get; set; }

        [JsonProperty("region")]
        public string Region { get; set; }

        [JsonProperty("subregion")]
        public string Subregion { get; set; }

        [JsonProperty("latlng")]
        public long[] Latlng { get; set; }

        [JsonProperty("landlocked")]
        public bool Landlocked { get; set; }

        [JsonProperty("borders")]
        public string[] Borders { get; set; }

        [JsonProperty("area")]
        public long Area { get; set; }

        [JsonProperty("flag")]
        public string Flag { get; set; }

        [JsonProperty("population")]
        public long Population { get; set; }

        [JsonProperty("fifa")]
        public string Fifa { get; set; }

        [JsonProperty("car")]
        public Car Car { get; set; }

        [JsonProperty("timezones")]
        public string[] Timezones { get; set; }

        [JsonProperty("continents")]
        public string[] Continents { get; set; }

        [JsonProperty("flags")]
        public Flags Flags { get; set; }

        [JsonProperty("coatOfArms")]
        public CoatOfArms CoatOfArms { get; set; }

        [JsonProperty("startOfWeek")]
        public string StartOfWeek { get; set; }

        [JsonProperty("capitalInfo")]
        public CapitalInfo CapitalInfo { get; set; }
    }

    public partial class CapitalInfo
    {
        [JsonProperty("latlng")]
        public double[] Latlng { get; set; }
    }

    public partial class Car
    {
        [JsonProperty("signs")]
        public string[] Signs { get; set; }

        [JsonProperty("side")]
        public string Side { get; set; }
    }

    public partial class CoatOfArms
    {
        [JsonProperty("png")]
        public Uri Png { get; set; }

        [JsonProperty("svg")]
        public Uri Svg { get; set; }
    }

    public partial class Flags
    {
        [JsonProperty("png")]
        public Uri Png { get; set; }

        [JsonProperty("svg")]
        public Uri Svg { get; set; }

        [JsonProperty("alt")]
        public string Alt { get; set; }
    }

    public partial class Name
    {
        [JsonProperty("common")]
        public string Common { get; set; }

        [JsonProperty("official")]
        public string Official { get; set; }

    }
}

