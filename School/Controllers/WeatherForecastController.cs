using Microsoft.AspNetCore.Mvc;

namespace Schoole.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[] { "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching" };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            }).ToArray();
        }

        [HttpPost(Name = "GetWeatherForecast")]
        public User Post([FromBody] User user)
        {
            return user;
        }

        [HttpGet("main500/{count}")]
        public IEnumerable<WeatherForecast> Main(int count, [FromQuery] int id)
        {
            return Enumerable.Range(1, count).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            }).ToArray();
        }

        [HttpGet("test")]
        public IActionResult Test()
        {
            return RedirectToAction("Main");
        }
    }

    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class WeatherForecast
    {
        public DateTime Date { get; internal set; }
        public int TemperatureC { get; internal set; }
        public string Summary { get; internal set; }
    }
}