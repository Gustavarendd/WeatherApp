using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using WeatherApp.Models;

namespace WeatherApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherApiController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _apiKey = "d2d457c1bbbdb21f2061710aeb7f5cdc";

        public WeatherApiController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet("current")]
        public async Task<IActionResult> GetCurrentWeather(double latitude, double longitude)
        {
            var client = _httpClientFactory.CreateClient("OpenWeatherClient");
            var response = await client.GetAsync($"https://api.openweathermap.org/data/2.5/weather?lat={latitude}&lon={longitude}&appid={_apiKey}&units=metric");

            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStringAsync();
                var weather = JsonSerializer.Deserialize<WeatherResponse>(apiResponse);
                return Ok(weather);
            }
            return BadRequest("Error fetching data");
        }

        [HttpGet("temperature")]
        public async Task<IActionResult> GetTemperature(double latitude, double longitude)
        {
            var client = _httpClientFactory.CreateClient("OpenWeatherClient");
            var response = await client.GetAsync($"https://api.openweathermap.org/data/2.5/weather?lat={latitude}&lon={longitude}&appid={_apiKey}&units=metric");

            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStringAsync();
                var weather = JsonSerializer.Deserialize<WeatherResponse>(apiResponse);
                return Ok(new
                {
                    Temperature = weather.Main.Temp,
                    FeelsLike = weather.Main.FeelsLike,
                    TempMin = weather.Main.TempMin,
                    TempMax = weather.Main.TempMax,
                    Unit = "Celsius"
                });
            }
            return BadRequest("Error fetching temperature data");
        }

        [HttpGet("wind")]
        public async Task<IActionResult> GetWind(double latitude, double longitude)
        {
            var client = _httpClientFactory.CreateClient("OpenWeatherClient");
            var response = await client.GetAsync($"https://api.openweathermap.org/data/2.5/weather?lat={latitude}&lon={longitude}&appid={_apiKey}&units=metric");

            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStringAsync();
                var weather = JsonSerializer.Deserialize<WeatherResponse>(apiResponse);
                return Ok(new
                {
                    Speed = weather.Wind.Speed,
                    Direction = weather.Wind.Deg,
                    Unit = "m/s"
                });
            }
            return BadRequest("Error fetching wind data");
        }

        [HttpGet("humidity")]
        public async Task<IActionResult> GetHumidity(double latitude, double longitude)
        {
            var client = _httpClientFactory.CreateClient("OpenWeatherClient");
            var response = await client.GetAsync($"https://api.openweathermap.org/data/2.5/weather?lat={latitude}&lon={longitude}&appid={_apiKey}&units=metric");

            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStringAsync();
                var weather = JsonSerializer.Deserialize<WeatherResponse>(apiResponse);
                return Ok(new
                {
                    Humidity = weather.Main.Humidity,
                    Unit = "%"
                });
            }
            return BadRequest("Error fetching humidity data");
        }

        [HttpGet("forecast")]
        public async Task<IActionResult> GetForecast(double latitude, double longitude, int days = 5)
        {
            var client = _httpClientFactory.CreateClient("OpenWeatherClient");
            var response = await client.GetAsync($"https://api.openweathermap.org/data/2.5/forecast?lat={latitude}&lon={longitude}&appid={_apiKey}&units=metric&cnt={days * 8}");

            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStringAsync();
                return Ok(apiResponse);
            }
            return BadRequest("Error fetching forecast data");
        }

        [HttpGet("by-city")]
        public async Task<IActionResult> GetWeatherByCity(string city)
        {
            var client = _httpClientFactory.CreateClient("OpenWeatherClient");
            var response = await client.GetAsync($"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={_apiKey}&units=metric");

            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStringAsync();
                var weather = JsonSerializer.Deserialize<WeatherResponse>(apiResponse);
                return Ok(weather);
            }
            return BadRequest("Error fetching data for the specified city");
        }
    }
}
