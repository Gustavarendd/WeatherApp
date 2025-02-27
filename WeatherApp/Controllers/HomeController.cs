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
        public async Task<IActionResult> Index(double latitude = 50, double longitude = 11)
        {
            // Create client with the application's base URL
            var client = _httpClientFactory.CreateClient();
            // Use absolute URL including the host
            var response = await client.GetAsync($"{Request.Scheme}://{Request.Host}/api/WeatherApi/current?latitude={latitude}&longitude={longitude}");
            Console.WriteLine(Request.Scheme, Request.Host);
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStringAsync();
                var weather = JsonSerializer.Deserialize<WeatherResponse>(apiResponse);
                var model = new IndexModel
                {
                    Latitude = latitude,
                    Longitude = longitude,
                    Weather = weather,
                };
                ViewData["Title"] = "Home Page";
                return View(model);
            }
            return View(new IndexModel { ApiResponse = "Error fetching data" });
        }

        [HttpGet]
        public async Task<IActionResult> ByCity(string city)
        {
            if (string.IsNullOrEmpty(city))
            {
                return RedirectToAction("Index");
            }

            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"{Request.Scheme}://{Request.Host}/api/WeatherApi/by-city?city={city}");

            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStringAsync();
                var weather = JsonSerializer.Deserialize<WeatherResponse>(apiResponse);
                var model = new IndexModel
                {
                    Weather = weather,
                };
                ViewData["Title"] = "Home Page";
                return View("Index", model);
            }
            return View("Index", new IndexModel { ApiResponse = $"Error fetching weather data for {city}" });
        }
    }
}
