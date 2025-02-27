using System.Text.Json.Serialization;

namespace WeatherApp.Models
{
    public class WeatherResponse
    {
        [JsonPropertyName("coord")]
        public Coord Coord { get; set; } = new Coord();

        [JsonPropertyName("weather")]
        public List<Weather> WeatherInfo { get; set; } = new List<Weather>();

        [JsonPropertyName("base")]
        public string Base { get; set; } = string.Empty;

        [JsonPropertyName("main")]
        public Main Main { get; set; } = new Main();

        [JsonPropertyName("visibility")]
        public int Visibility { get; set; } = 0;

        [JsonPropertyName("wind")]
        public Wind Wind { get; set; } = new Wind();

        [JsonPropertyName("snow")]
        public Snow Snow { get; set; } = new Snow();

        [JsonPropertyName("clouds")]
        public Clouds Clouds { get; set; } = new Clouds();

        [JsonPropertyName("dt")]
        public long Dt { get; set; } = 0;

        [JsonPropertyName("sys")]
        public Sys Sys { get; set; } = new Sys();

        [JsonPropertyName("timezone")]
        public int Timezone { get; set; } = 0;

        [JsonPropertyName("id")]
        public int Id { get; set; } = 0;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("cod")]
        public int Cod { get; set; } = 0;
    }

    public class Coord
    {
        [JsonPropertyName("lon")]
        public double Lon { get; set; } = 0;

        [JsonPropertyName("lat")]
        public double Lat { get; set; } = 0;
    }

    public class Weather
    {
        [JsonPropertyName("id")]
        public int Id { get; set; } = 0;

        [JsonPropertyName("main")]
        public string Main { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("icon")]
        public string Icon { get; set; } = string.Empty;
    }

    public class Main
    {
        [JsonPropertyName("temp")]
        public double Temp { get; set; } = 0;

        [JsonPropertyName("feels_like")]
        public double FeelsLike { get; set; } = 0;

        [JsonPropertyName("temp_min")]
        public double TempMin { get; set; } = 0;

        [JsonPropertyName("temp_max")]
        public double TempMax { get; set; } = 0;

        [JsonPropertyName("pressure")]
        public int Pressure { get; set; } = 0;

        [JsonPropertyName("humidity")]
        public int Humidity { get; set; } = 0;

        [JsonPropertyName("sea_level")]
        public int? SeaLevel { get; set; } = 0;

        [JsonPropertyName("grnd_level")]
        public int? GrndLevel { get; set; } = 0;
    }

    public class Wind
    {
        [JsonPropertyName("speed")]
        public double Speed { get; set; } = 0;

        [JsonPropertyName("deg")]
        public int Deg { get; set; } = 0;

        [JsonPropertyName("gust")]
        public double? Gust { get; set; } = 0;
    }

   

    public class Clouds
    {
        [JsonPropertyName("all")]
        public int All { get; set; } = 0;
    }

    public class Sys
    {
        [JsonPropertyName("type")]
        public int Type { get; set; } = 0;

        [JsonPropertyName("id")]
        public int Id { get; set; } = 0;

        [JsonPropertyName("country")]
        public string Country { get; set; } = string.Empty;

        [JsonPropertyName("sunrise")]
        public long Sunrise { get; set; } = 0;

        [JsonPropertyName("sunset")]
        public long Sunset { get; set; } = 0;
    }
}
