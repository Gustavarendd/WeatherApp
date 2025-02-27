using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WeatherApp.Models
{
    public class IndexModel : PageModel
    {
        public string ApiResponse { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public WeatherResponse Weather { get; set; } = new WeatherResponse();

    
    }
}
