using System;
using System.Collections.Generic;
using System.Linq;
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

                SimulationMain.instance.OrderUnitMoveViaEvent(
                    commandBody.UnitId,
                    commandBody.DestinationLocationXInWorldCoordinates,
                    commandBody.DestinationLocationYInWorldCoordinates
                );


                // Minigunner minigunner =
                //     SimulationMain.instance.CreateMinigunnerViaEvent(commandBody.DestinationLocationXInWorldCoordinates,
                //         commandBody.DestinationLocationYInWorldCoordinates);
                //
                //
                //
                //     RestMinigunner createdRestMinigunner = new RestMinigunner();
                // createdRestMinigunner.X = minigunner.X;
                // createdRestMinigunner.Y = minigunner.Y;
                // createdRestMinigunner.ID = minigunner.ID;
    
//                return new CreatedResult($"/minigunners/{createdRestMinigunner.ID}", createdRestMinigunner);
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
    
    }


}
