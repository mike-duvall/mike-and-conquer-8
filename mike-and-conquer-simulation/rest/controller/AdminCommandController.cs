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
    [Route("simulation/command/admin")]


    
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
        public ActionResult PostAdminCommand([FromBody] RestAdminCommand incomingAdminCommand)
        {
            try
            {

                if (incomingAdminCommand.CommandType.Equals("CreateMinigunner"))
                {
                    CreateUnitCommandBody createMinigunnerCommandBody = 
                        JsonConvert.DeserializeObject<CreateUnitCommandBody>(incomingAdminCommand.CommandData);


                    SimulationMain.instance.CreateMinigunnerViaCommand(createMinigunnerCommandBody.StartLocationXInWorldCoordinates,
                        createMinigunnerCommandBody.StartLocationYInWorldCoordinates);

                    return new OkObjectResult(new { Message = "Command Accepted" });
                }
                else if (incomingAdminCommand.CommandType.Equals("ResetScenario"))
                {
                    SimulationMain.instance.SubmitResetScenarioCommand();
                    return new OkObjectResult(new { Message = "Command Accepted" });
                }
                else
                {
                    throw new Exception("Unknown CommandType:" + incomingAdminCommand.CommandType);
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
