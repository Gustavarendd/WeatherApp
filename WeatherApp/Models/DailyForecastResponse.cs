using System.Text.Json.Serialization;

namespace WeatherApp.Models
{
    public class DailyForecastResponse
    {
        [JsonPropertyName("list")]
        public List<DailyForecastItem> List { get; set; } = new();

        [JsonPropertyName("city")]
        public City City { get; set; } = new();
    }

    public class DailyForecastItem
    {
        [JsonPropertyName("dt")]
        public long Dt { get; set; }

        [JsonPropertyName("sunrise")]
        public long Sunrise { get; set; }

        [JsonPropertyName("sunset")]
        public long Sunset { get; set; }

        [JsonPropertyName("temp")]
        public Temperature Temp { get; set; } = new();

        [JsonPropertyName("feels_like")]
        public FeelsLike FeelsLike { get; set; } = new();

        [JsonPropertyName("pressure")]
        public int Pressure { get; set; }

        [JsonPropertyName("humidity")]
        public int Humidity { get; set; }

        [JsonPropertyName("weather")]
        public List<Weather> Weather { get; set; } = new();

        [JsonPropertyName("speed")]
        public double WindSpeed { get; set; }

        [JsonPropertyName("deg")]
        public int WindDirection { get; set; }

        [JsonPropertyName("clouds")]
        public int Clouds { get; set; }

        [JsonPropertyName("pop")]
        public double Pop { get; set; } // Probability of precipitation

        [JsonPropertyName("rain")]
        public double? Rain { get; set; }

        [JsonPropertyName("snow")]
        public double? Snow { get; set; }

        public DateTime Date => DateTime.UnixEpoch.AddSeconds(Dt);
        public DateTime SunriseTime => DateTime.UnixEpoch.AddSeconds(Sunrise);
        public DateTime SunsetTime => DateTime.UnixEpoch.AddSeconds(Sunset);
    }
    public class Temperature
    {
        [JsonPropertyName("day")]
        public double Day { get; set; }

        [JsonPropertyName("min")]
        public double Min { get; set; }

        [JsonPropertyName("max")]
        public double Max { get; set; }

        [JsonPropertyName("night")]
        public double Night { get; set; }

        [JsonPropertyName("eve")]
        public double Eve { get; set; }

        [JsonPropertyName("morn")]
        public double Morn { get; set; }
    }

    public class FeelsLike
    {
        [JsonPropertyName("day")]
        public double Day { get; set; }

        [JsonPropertyName("night")]
        public double Night { get; set; }

        [JsonPropertyName("eve")]
        public double Eve { get; set; }

        [JsonPropertyName("morn")]
        public double Morn { get; set; }
    }
}