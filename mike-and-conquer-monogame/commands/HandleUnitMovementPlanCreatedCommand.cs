
using mike_and_conquer_monogame.main;

using mike_and_conquer_simulation.events;

namespace mike_and_conquer_monogame.commands
{
    public class HandleUnitMovementPlanCreatedCommand : AsyncViewCommand
    {


        private UnitMovementPlanCreatedEventData unitMovementPlanCreatedEventData;

        public HandleUnitMovementPlanCreatedCommand(UnitMovementPlanCreatedEventData data)
        {
            this.unitMovementPlanCreatedEventData = data;
        }

        protected override void ProcessImpl()
        {
            MikeAndConquerGame.instance.UpdateUnitMovementPlan(unitMovementPlanCreatedEventData);
        }
    }
}
