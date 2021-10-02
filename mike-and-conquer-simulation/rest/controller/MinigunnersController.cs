using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using mike_and_conquer_simulation.rest.domain;
// using WeatherForecast = mike_and_conquer_simulation.rest.domain.WeatherForecast;

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
        public IEnumerable<RestMinigunner> Get()
        {
            _logger.LogInformation("This is some test logging from the simulation.  And, Mike is cool");
            _logger.LogWarning("This is some test logging from the simulation.  And, Mike is cool");
            var rng = new Random();
            int baseX = 1;
            int baseY = 1;
            return Enumerable.Range(1, 5).Select(index => new RestMinigunner
            {
                X = baseX++,
                Y= baseY++

                // Date = DateTime.Now.AddDays(index),
                // TemperatureC = rng.Next(-20, 55),
                // Summary = Summaries[rng.Next(Summaries.Length)]

            })
            .ToArray();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<RestMinigunner> PostProduct([FromBody] RestMinigunner incomingRestMinigunner)
        {
            try
            {

                // var result = CreatedAtAction(nameof(GetById), new { id = 5 },minigunner);
                // var s = minigunner.ToString();
                // var t = minigunner.GetType();
                // return new CreatedResult($"/products/{5}", minigunner);

                
                RestMinigunner createdRestMinigunner = new RestMinigunner();
                createdRestMinigunner.X = 100;
                createdRestMinigunner.Y = 200;
                createdRestMinigunner.ID = 2;

                return new CreatedResult($"/minigunners/{createdRestMinigunner.ID}", createdRestMinigunner);



            }
            catch (Exception e)
            {
                _logger.LogWarning(e, "Unable to POST minigunner.");

                return ValidationProblem(e.Message);
            }
        }

    }
}
