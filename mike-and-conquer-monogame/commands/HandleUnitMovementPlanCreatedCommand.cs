
using System.Collections.Generic;
using mike_and_conquer_monogame.main;

using mike_and_conquer_simulation.events;

namespace mike_and_conquer_monogame.commands
{
    public class HandleUnitMovementPlanCreatedCommand : AsyncViewCommand
    {


        private int unitId;
        private List<PathStep> pathStepList;

        public HandleUnitMovementPlanCreatedCommand(int unitId, List<PathStep> pathStepList)
        {
            this.unitId = unitId;
            this.pathStepList = pathStepList;
        }

        protected override void ProcessImpl()
        {
            MikeAndConquerGame.instance.HandleUnitMovementPlanCreated(unitId, pathStepList);
        }
    }
}
