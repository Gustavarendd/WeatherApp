using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using WeatherApp.Models;
using Microsoft.Extensions.Caching.Memory;
using System.Linq;

namespace WeatherApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherApiController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMemoryCache _cache;
        private readonly string _apiKey = "d2d457c1bbbdb21f2061710aeb7f5cdc";

        public WeatherApiController(IHttpClientFactory httpClientFactory, IMemoryCache cache)
        {
            _httpClientFactory = httpClientFactory;
            _cache = cache;
        }

        [HttpGet("current")]
        public async Task<IActionResult> GetCurrentWeather(double latitude, double longitude, string units = "metric")
        {
            var cacheKey = $"current_{latitude}_{longitude}_{units}";

            if (!_cache.TryGetValue(cacheKey, out WeatherResponse weather))
            {
                var client = _httpClientFactory.CreateClient("OpenWeatherClient");
                var response = await client.GetAsync($"https://api.openweathermap.org/data/2.5/weather?lat={latitude}&lon={longitude}&appid={_apiKey}&units={units}");

                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    weather = JsonSerializer.Deserialize<WeatherResponse>(apiResponse);

                    // Cache for 10 minutes
                    _cache.Set(cacheKey, weather, TimeSpan.FromMinutes(10));
                }
                else
                {
                    return BadRequest("Error fetching data");
                }
            }

            return Ok(weather);
        }

        [HttpGet("temperature")]
        public async Task<IActionResult> GetTemperature(double latitude, double longitude, string units = "metric")
        {
            var cacheKey = $"current_{latitude}_{longitude}_{units}";
            WeatherResponse weather;

            if (!_cache.TryGetValue(cacheKey, out weather))
            {
                var client = _httpClientFactory.CreateClient("OpenWeatherClient");
                var response = await client.GetAsync($"https://api.openweathermap.org/data/2.5/weather?lat={latitude}&lon={longitude}&appid={_apiKey}&units={units}");

                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    weather = JsonSerializer.Deserialize<WeatherResponse>(apiResponse);

                    // Cache for 10 minutes
                    _cache.Set(cacheKey, weather, TimeSpan.FromMinutes(10));
                }
                else
                {
                    return BadRequest("Error fetching temperature data");
                }
            }

            string tempUnit = units == "imperial" ? "°F" : "°C";
            return Ok(new
            {
                Temperature = weather.Main.Temp,
                FeelsLike = weather.Main.FeelsLike,
                TempMin = weather.Main.TempMin,
                TempMax = weather.Main.TempMax,
                Unit = tempUnit
            });
        }

        [HttpGet("wind")]
        public async Task<IActionResult> GetWind(double latitude, double longitude, string units = "metric")
        {
            var cacheKey = $"current_{latitude}_{longitude}_{units}";
            WeatherResponse weather;

            if (!_cache.TryGetValue(cacheKey, out weather))
            {
                var client = _httpClientFactory.CreateClient("OpenWeatherClient");
                var response = await client.GetAsync($"https://api.openweathermap.org/data/2.5/weather?lat={latitude}&lon={longitude}&appid={_apiKey}&units={units}");

                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    weather = JsonSerializer.Deserialize<WeatherResponse>(apiResponse);

                    // Cache for 10 minutes
                    _cache.Set(cacheKey, weather, TimeSpan.FromMinutes(10));
                }
                else
                {
                    return BadRequest("Error fetching wind data");
                }
            }

            string speedUnit = units == "imperial" ? "mph" : "m/s";
            return Ok(new
            {
                Speed = weather.Wind.Speed,
                Direction = weather.Wind.Deg,
                Unit = speedUnit
            });
        }

        [HttpGet("humidity")]
        public async Task<IActionResult> GetHumidity(double latitude, double longitude, string units = "metric")
        {
            var cacheKey = $"current_{latitude}_{longitude}_{units}";
            WeatherResponse weather;

            if (!_cache.TryGetValue(cacheKey, out weather))
            {
                var client = _httpClientFactory.CreateClient("OpenWeatherClient");
                var response = await client.GetAsync($"https://api.openweathermap.org/data/2.5/weather?lat={latitude}&lon={longitude}&appid={_apiKey}&units={units}");

                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    weather = JsonSerializer.Deserialize<WeatherResponse>(apiResponse);

                    // Cache for 10 minutes
                    _cache.Set(cacheKey, weather, TimeSpan.FromMinutes(10));
                }
                else
                {
                    return BadRequest("Error fetching humidity data");
                }
            }

            return Ok(new
            {
                Humidity = weather.Main.Humidity,
                Unit = "%"
            });
        }

        [HttpGet("forecast")]
        public async Task<IActionResult> GetForecast(
            double latitude,
            double longitude,
            int days = 5,
            string units = "metric",
            string sortBy = "",
            string weatherType = "",
            bool latestSunset = false)
        {
            if (days < 1 || days > 7)
            {
                return BadRequest("Days must be between 1 and 7");
            }

            var cacheKey = $"forecast_{latitude}_{longitude}_{days}_{units}";

            if (!_cache.TryGetValue(cacheKey, out ForecastResponse forecast))
            {
                var client = _httpClientFactory.CreateClient("OpenWeatherClient");
                var response = await client.GetAsync($"https://api.openweathermap.org/data/2.5/forecast?lat={latitude}&lon={longitude}&appid={_apiKey}&units={units}&cnt={days * 8}");

                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    forecast = JsonSerializer.Deserialize<ForecastResponse>(apiResponse);

                    // Cache for 10 minutes
                    _cache.Set(cacheKey, forecast, TimeSpan.FromMinutes(10));

                    // If we need to find latest sunset day
                    if (latestSunset)
                    {
                        // Get additional daily forecast to access sunset times
                        var dailyResponse = await client.GetAsync($"https://api.openweathermap.org/data/2.5/forecast/daily?lat={latitude}&lon={longitude}&appid={_apiKey}&units={units}&cnt={days}");
                        if (dailyResponse.IsSuccessStatusCode)
                        {
                            var dailyApiResponse = await dailyResponse.Content.ReadAsStringAsync();
                            var dailyForecast = JsonSerializer.Deserialize<DailyForecastResponse>(dailyApiResponse);

                            if (dailyForecast != null && dailyForecast.List.Any())
                            {
                                // Find the day with the latest sunset
                                var latestSunsetDay = dailyForecast.List
                                    .OrderByDescending(d => d.Sunset)
                                    .First();

                                return Ok(new
                                {
                                    LatestSunsetDay = latestSunsetDay,
                                    AllForecasts = forecast
                                });
                            }
                        }
                    }
                }
                else
                {
                    return BadRequest("Error fetching forecast data");
                }
            }

            // Apply filtering for specific weather type if requested
            if (!string.IsNullOrEmpty(weatherType))
            {
                forecast.List = forecast.List
                    .Where(item => item.Weather.Any(w => w.Main.ToLower() == weatherType.ToLower()))
                    .ToList();
            }

            // Sort by precipitation amount if requested
            if (sortBy == "precipitation")
            {
                forecast.List = forecast.List
                    .OrderByDescending(item => item.Rain != null ? item.Rain.ThreeHours : 0)
                    .ToList();
            }

            return Ok(forecast);
        }

        [HttpGet("by-city")]
        public async Task<IActionResult> GetWeatherByCity(string city, string units = "metric")
        {
            var cacheKey = $"city_{city}_{units}";

            if (!_cache.TryGetValue(cacheKey, out WeatherResponse weather))
            {
                var client = _httpClientFactory.CreateClient("OpenWeatherClient");
                var response = await client.GetAsync($"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={_apiKey}&units={units}");

                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    weather = JsonSerializer.Deserialize<WeatherResponse>(apiResponse);

                    // Cache for 10 minutes
                    _cache.Set(cacheKey, weather, TimeSpan.FromMinutes(10));
                }
                else
                {
                    return BadRequest("Error fetching data for the specified city");
                }
            }

            return Ok(weather);
        }
    }
}
