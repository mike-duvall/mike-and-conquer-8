using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
// using WeatherForecast = mike_and_conquer_simulation.rest.domain.WeatherForecast;
using Minigunner = mike_and_conquer_simulation.rest.domain.Minigunner;

namespace mike_and_conquer_simulation.rest.controller
{
    [ApiController]
    [Route("[controller]")]
    public class MinigunnersController : ControllerBase
    {





        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<MinigunnersController> _logger;

        public MinigunnersController(ILogger<MinigunnersController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Minigunner> Get()
        {
            _logger.LogInformation("This is some test logging from the simulation.  And, Mike is cool");
            _logger.LogWarning("This is some test logging from the simulation.  And, Mike is cool");
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new Minigunner
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
