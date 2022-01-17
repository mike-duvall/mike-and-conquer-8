using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using mike_and_conquer_simulation.main;
using mike_and_conquer_simulation.rest.domain;
using Newtonsoft.Json;

// using WeatherForecast = mike_and_conquer_simulation.rest.domain.WeatherForecast;

namespace mike_and_conquer_simulation.rest.controller
{
    [ApiController]
//    [Route("[controller]")]
//    [Route("api/[controller]")]
    [Route("simulation/command/user")]


    
    public class UserCommandController : ControllerBase
    {


        private readonly ILogger<UserCommandController> _logger;

        public UserCommandController(ILogger<UserCommandController> logger)
        {
            _logger = logger;
        }


    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<RestMinigunner> PostUserCommand([FromBody] RestAdminCommand incomingCommand)
    {
        try
        {
    
            if (incomingCommand.CommandType.Equals("OrderUnitMove"))
            {

                RestOrderUnitMoveCommandBody commandBody =
                    JsonConvert.DeserializeObject<RestOrderUnitMoveCommandBody>(incomingCommand.CommandData);

                SimulationMain.instance.PostOrderUnitMoveCommand(
                    commandBody.UnitId,
                    commandBody.DestinationLocationXInWorldCoordinates,
                    commandBody.DestinationLocationYInWorldCoordinates
                );


                return new OkObjectResult(new { Message = "Command Accepted" });
            }
            else if (incomingCommand.CommandType.Equals("SetOptions"))
            {
                RestSetSimulationOptions commandBody =
                    JsonConvert.DeserializeObject<RestSetSimulationOptions>(incomingCommand.CommandData);

                SimulationOptions.GameSpeed inputGameSpeed = ConvertGameSpeedStringToEnum(commandBody.GameSpeed);
                SimulationMain.instance.PostSetGameSpeedCommand(inputGameSpeed);

                return new OkObjectResult(new { Message = "Command Accepted" });

            }
            else
            {
                throw new Exception("Unknown CommandType:" + incomingCommand.CommandType);
            }
    
        }
        catch (Exception e)
        {
            _logger.LogWarning(e, "Unable to POST minigunner.");
    
            return ValidationProblem(e.Message);
        }
    }


    private SimulationOptions.GameSpeed ConvertGameSpeedStringToEnum(String gameSpeedAsString)
    {
        if (gameSpeedAsString == "Slowest") return SimulationOptions.GameSpeed.Slowest;
        if (gameSpeedAsString == "Slower") return SimulationOptions.GameSpeed.Slower;
        if (gameSpeedAsString == "Slow") return SimulationOptions.GameSpeed.Slow;
        if (gameSpeedAsString == "Moderate") return SimulationOptions.GameSpeed.Moderate;
        if (gameSpeedAsString == "Normal") return SimulationOptions.GameSpeed.Normal;
        if (gameSpeedAsString == "Fast") return SimulationOptions.GameSpeed.Fast;
        if (gameSpeedAsString == "Faster") return SimulationOptions.GameSpeed.Faster;
        if (gameSpeedAsString == "Fastest") return SimulationOptions.GameSpeed.Fastest;

        throw new Exception("Could not map game speed string of:" + gameSpeedAsString);
    }


    }


}
