using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using mike_and_conquer_simulation.main;
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

        // [HttpPost]
        // [ProducesResponseType(StatusCodes.Status201Created)]
        // [ProducesResponseType(StatusCodes.Status400BadRequest)]
        // public ActionResult<RestMinigunner> PostMinigunner([FromBody] RestMinigunner incomingRestMinigunner)
        // {
        //     try
        //     {
        //
        //         
        //         RestMinigunner createdRestMinigunner = new RestMinigunner();
        //         createdRestMinigunner.X = 100;
        //         createdRestMinigunner.Y = 200;
        //         createdRestMinigunner.ID = 2;
        //
        //         CreateMinigunnerCommand createMinigunnerEvent = new CreateMinigunnerCommand();
        //         createMinigunnerEvent.X = incomingRestMinigunner.X;
        //         createMinigunnerEvent.Y = incomingRestMinigunner.Y;
        //         SimulationMain.instance.CreateMinigunnerViaEvent(createMinigunnerEvent);
        //
        //         return new CreatedResult($"/minigunners/{createdRestMinigunner.ID}", createdRestMinigunner);
        //
        //
        //
        //     }
        //     catch (Exception e)
        //     {
        //         _logger.LogWarning(e, "Unable to POST minigunner.");
        //
        //         return ValidationProblem(e.Message);
        //     }
        // }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<RestMinigunner> PostMinigunner([FromBody] RestMinigunner incomingRestMinigunner)
        {
            try
            {

                Minigunner minigunner = SimulationMain.instance.CreateMinigunnerViaEvent(incomingRestMinigunner.X, incomingRestMinigunner.Y);

                RestMinigunner createdRestMinigunner = new RestMinigunner();
                createdRestMinigunner.X = (int) minigunner.X;
                createdRestMinigunner.Y = (int) minigunner.Y;
                createdRestMinigunner.ID = minigunner.ID;

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
