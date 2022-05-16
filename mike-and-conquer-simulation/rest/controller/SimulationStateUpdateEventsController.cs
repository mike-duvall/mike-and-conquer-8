using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using mike_and_conquer_simulation.events;
using mike_and_conquer_simulation.main;
using mike_and_conquer_simulation.rest.domain;
// using WeatherForecast = mike_and_conquer_simulation.rest.domain.WeatherForecast;

namespace mike_and_conquer_simulation.rest.controller
{
    [ApiController]
    [Route("[controller]")]
    public class SimulationStateUpdateEventsController : ControllerBase
    {


        private readonly ILogger<SimulationStateUpdateEventsController> _logger;

        public SimulationStateUpdateEventsController(ILogger<SimulationStateUpdateEventsController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<RestSimulationStateUpdateEvent> Get()
        {
            List<RestSimulationStateUpdateEvent> restReturnList = new List<RestSimulationStateUpdateEvent>();

            List<SimulationStateUpdateEvent> simulationStateUpdateList = 
                SimulationMain.instance.GetCopyOfEventHistoryViaEvent();

            foreach (SimulationStateUpdateEvent simulationStateUpdateEvent in simulationStateUpdateList)
            {
                RestSimulationStateUpdateEvent anEvent = new RestSimulationStateUpdateEvent();
                anEvent.EventType = simulationStateUpdateEvent.EventType;
                anEvent.EventData = simulationStateUpdateEvent.EventData;
                // anEvent.X = simulationStateUpdateEvent.X;
                // anEvent.Y = simulationStateUpdateEvent.Y;
                // anEvent.UnitId = simulationStateUpdateEvent.UnitId;
                restReturnList.Add(anEvent);
            }


            return restReturnList;
        }


    }
}
