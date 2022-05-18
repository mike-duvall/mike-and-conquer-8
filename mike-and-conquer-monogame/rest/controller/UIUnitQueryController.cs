using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using mike.and.conquer.monogame.rest.domain;

using mike_and_conquer_monogame.main;



namespace mike_and_conquer_simulation.rest.controller
{
    [ApiController]
    //    [Route("[controller]")]

    [Route("ui/query/unit")]


    public class UIUnitQueryController : ControllerBase
    {

        private readonly ILogger<AdminCommandController> _logger;

        public UIUnitQueryController(ILogger<AdminCommandController> logger)
        {
            _logger = logger;
        }




        [HttpGet]
        public ActionResult Get([FromQuery] int unitId)
        {

            UnitView unitView = MikeAndConquerGame.instance.GetUnitViewByIdByEvent(unitId);


            RestUnit restUnit = new RestUnit();

            restUnit.UnitId = unitView.UnitId;
            restUnit.Selected = unitView.Selected;

            return new OkObjectResult(restUnit);
        }


    }
}
