using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using mike_and_conquer.gameworld.humancontroller;
using mike_and_conquer_monogame.commands;
using mike_and_conquer_monogame.main;
using mike_and_conquer_simulation.commands;
using mike_and_conquer_simulation.commands.commandbody;
using mike_and_conquer_simulation.main;
using mike_and_conquer_simulation.rest.domain;
using Newtonsoft.Json;

// using WeatherForecast = mike_and_conquer_simulation.rest.domain.WeatherForecast;

namespace mike_and_conquer_simulation.rest.controller
{
    [ApiController]
//    [Route("[controller]")]
//    [Route("api/[controller]")]
    [Route("ui/command")]


    public class AdminCommandController : ControllerBase
    {

        private readonly ILogger<AdminCommandController> _logger;

        public AdminCommandController(ILogger<AdminCommandController> logger)
        {
            _logger = logger;
        }

        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult PostAdminCommand([FromBody] RestRawCommandUI incomingRawCommand)
        {
            try
            {
                RawCommandUI rawCommand = new RawCommandUI();
                rawCommand.CommandType = incomingRawCommand.CommandType;
                rawCommand.CommandData = incomingRawCommand.CommandData;


                // if (rawCommand.CommandType.Equals(SelectUnitCommand.CommandName))
                // {
                //
                //     SelectUnitCommandBody commandBody =
                //         JsonConvert.DeserializeObject<SelectUnitCommandBody>(rawCommand.CommandData);
                //
                //     SelectUnitCommand command = new SelectUnitCommand(commandBody.UnitId);
                //     command.Process();
                //
                // }
                // else
                // {
                    // SimulationMain.instance.PostCommand(rawCommand);
                    MikeAndConquerGame.instance.PostCommand(rawCommand);

                // }


                return new OkObjectResult(new {Message = "Command Accepted"});

            }
            catch (Exception e)
            {
                _logger.LogWarning(e, "Error processing Command");

                return ValidationProblem(e.Message);
            }
        }


        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };


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
