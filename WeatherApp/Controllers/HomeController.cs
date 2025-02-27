using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using WeatherApp.Models;

namespace WeatherApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HomeController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> Index(
            double latitude = 55.6761,
            double longitude = 12.5683,
            int forecastDays = 5,
            string units = "metric",
            string sortBy = "",
            string weatherType = "",
            bool latestSunset = false)
        {
            // Create client with the application's base URL
            var client = _httpClientFactory.CreateClient();

            var model = new IndexModel
            {
                Latitude = latitude,
                Longitude = longitude,
                ForecastDays = forecastDays,
                Units = units,
                SortBy = sortBy,
                WeatherType = weatherType,
                LatestSunset = latestSunset
            };

            // Get current weather
            var weatherResponse = await client.GetAsync(
                $"{Request.Scheme}://{Request.Host}/api/WeatherApi/current?latitude={latitude}&longitude={longitude}&units={units}");

            if (weatherResponse.IsSuccessStatusCode)
            {
                var weatherApiResponse = await weatherResponse.Content.ReadAsStringAsync();
                model.Weather = JsonSerializer.Deserialize<WeatherResponse>(weatherApiResponse);
            }

            // Build forecast URL with all parameters
            string forecastUrl = $"{Request.Scheme}://{Request.Host}/api/WeatherApi/forecast" +
                $"?latitude={latitude}&longitude={longitude}&days={forecastDays}&units={units}";

            if (!string.IsNullOrEmpty(sortBy))
            {
                forecastUrl += $"&sortBy={sortBy}";
            }

            if (!string.IsNullOrEmpty(weatherType))
            {
                forecastUrl += $"&weatherType={weatherType}";
            }

            if (latestSunset)
            {
                forecastUrl += "&latestSunset=true";
            }

            // Get forecast
            var forecastResponse = await client.GetAsync(forecastUrl);

            if (forecastResponse.IsSuccessStatusCode)
            {
                var forecastApiResponse = await forecastResponse.Content.ReadAsStringAsync();

                if (latestSunset)
                {
                    // For latestSunset, the response format is different
                    var combinedResponse = JsonSerializer.Deserialize<JsonElement>(forecastApiResponse);
                    if (combinedResponse.TryGetProperty("allForecasts", out var allForecasts) &&
                        combinedResponse.TryGetProperty("latestSunsetDay", out var latestSunsetDay))
                    {
                        model.Forecast = JsonSerializer.Deserialize<ForecastResponse>(allForecasts.GetRawText());
                        model.LatestSunsetDay = JsonSerializer.Deserialize<DailyForecastItem>(latestSunsetDay.GetRawText());
                    }
                }
                else
                {
                    model.Forecast = JsonSerializer.Deserialize<ForecastResponse>(forecastApiResponse);
                }
            }

            ViewData["Title"] = "Home Page";
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ByCity(
            string city,
            int forecastDays = 5,
            string units = "metric",
            string sortBy = "",
            string weatherType = "",
            bool latestSunset = false)
        {
            if (string.IsNullOrEmpty(city))
            {
                return RedirectToAction("Index");
            }

            var client = _httpClientFactory.CreateClient();

            // Get weather by city to get coordinates
            var cityResponse = await client.GetAsync($"{Request.Scheme}://{Request.Host}/api/WeatherApi/by-city?city={city}&units={units}");

            if (cityResponse.IsSuccessStatusCode)
            {
                var cityApiResponse = await cityResponse.Content.ReadAsStringAsync();
                var weather = JsonSerializer.Deserialize<WeatherResponse>(cityApiResponse);

                if (weather != null)
                {
                    // Redirect to the Index action with the coordinates
                    return RedirectToAction("Index", new
                    {
                        latitude = weather.Coord.Lat,
                        longitude = weather.Coord.Lon,
                        forecastDays,
                        units,
                        sortBy,
                        weatherType,
                        latestSunset
                    });
                }
            }

            // If city not found, redirect to Index
            return RedirectToAction("Index");
        }
    }
}
