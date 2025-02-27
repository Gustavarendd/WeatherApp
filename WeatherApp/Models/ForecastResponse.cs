using System.Text.Json.Serialization;

namespace WeatherApp.Models
{
    public class ForecastResponse
    {
        [JsonPropertyName("list")]
        public List<ForecastItem> List { get; set; } = new();

        [JsonPropertyName("city")]
        public City City { get; set; } = new();
    }

    public class ForecastItem
    {
        [JsonPropertyName("dt")]
        public long Dt { get; set; }

        [JsonPropertyName("main")]
        public Main Main { get; set; } = new();

        [JsonPropertyName("weather")]
        public List<Weather> Weather { get; set; } = new();

        [JsonPropertyName("clouds")]
        public Clouds Clouds { get; set; } = new();

        [JsonPropertyName("wind")]
        public Wind Wind { get; set; } = new();

        [JsonPropertyName("visibility")]
        public int Visibility { get; set; }

        [JsonPropertyName("pop")]
        public double Pop { get; set; } // Probability of precipitation

        [JsonPropertyName("rain")]
        public Rain Rain { get; set; } = new();

        [JsonPropertyName("snow")]
        public Snow Snow { get; set; } = new();

        [JsonPropertyName("dt_txt")]
        public string DtTxt { get; set; } = string.Empty;

        public DateTime DateTime => DateTime.UnixEpoch.AddSeconds(Dt);
    }

    public class Rain
    {
        [JsonPropertyName("3h")]
        public double ThreeHours { get; set; }
    }

    public class City
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("coord")]
        public Coord Coord { get; set; } = new();

        [JsonPropertyName("country")]
        public string Country { get; set; } = string.Empty;

        [JsonPropertyName("population")]
        public int Population { get; set; }

        [JsonPropertyName("timezone")]
        public int Timezone { get; set; }

        [JsonPropertyName("sunrise")]
        public long Sunrise { get; set; }

        [JsonPropertyName("sunset")]
        public long Sunset { get; set; }
    }
}
