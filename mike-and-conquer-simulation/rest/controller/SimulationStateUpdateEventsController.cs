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
    public class SimulationStateUpdateEventsController : ControllerBase
    {


        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<SimulationStateUpdateEventsController> _logger;

        public SimulationStateUpdateEventsController(ILogger<SimulationStateUpdateEventsController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<RestSimulationStateUpdateEvent> Get()
        {
            List<RestSimulationStateUpdateEvent> list = new List<RestSimulationStateUpdateEvent>();

            lock (SimulationMain.instance.publishedSimulationStateUpdateEvents)
            {

                foreach (SimulationStateUpdateEvent simulationStateUpdateEvent in SimulationMain.instance.publishedSimulationStateUpdateEvents)
                {

                    RestSimulationStateUpdateEvent anEvent = new RestSimulationStateUpdateEvent();
                    anEvent.X = simulationStateUpdateEvent.X;
                    anEvent.Y = simulationStateUpdateEvent.Y;
                    anEvent.ID = simulationStateUpdateEvent.ID;
                    list.Add(anEvent);
                }
            }

            return list;
        }


    }
}
