using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace mike_and_conquer_monogame.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MonogameWeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<MonogameWeatherForecastController> _logger;

        public MonogameWeatherForecastController(ILogger<MonogameWeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<MonogameWeatherForecast> Get()
        {
            _logger.LogInformation("This is some test logging from monogame.  And, Mike is cool");
            _logger.LogWarning("This is some test logging from monogame.  And, Mike is cool");
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new MonogameWeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
