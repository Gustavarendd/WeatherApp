using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WeatherApp.Models
{
    public class IndexModel : PageModel
    {
        public string ApiResponse { get; set; } = string.Empty;
        public double Latitude { get; set; } = 55.6761; // Copenhagen latitude
        public double Longitude { get; set; } = 12.5683; // Copenhagen longitude

        [Range(1, 7, ErrorMessage = "Please select between 1 and 7 days")]
        public int ForecastDays { get; set; } = 5;

        public string Units { get; set; } = "metric";

        public string SortBy { get; set; } = string.Empty;

        public string WeatherType { get; set; } = string.Empty;

        public bool LatestSunset { get; set; } = false;

        public WeatherResponse Weather { get; set; } = new();
        public ForecastResponse Forecast { get; set; } = new();
        public DailyForecastItem LatestSunsetDay { get; set; }

        public string GetTemperatureUnit() => Units == "imperial" ? "°F" : "°C";
        public string GetSpeedUnit() => Units == "imperial" ? "mph" : "m/s";

        public List<string> WeatherTypes => new List<string>
        {
            "Clear", "Clouds", "Rain", "Snow", "Thunderstorm", "Drizzle", "Mist", "Smoke", "Haze", "Dust", "Fog"
        };
    }
}
