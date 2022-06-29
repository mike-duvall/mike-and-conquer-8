
using mike_and_conquer_monogame.main;

using mike_and_conquer_simulation.events;

namespace mike_and_conquer_monogame.commands
{
    public class HighlightPathCommand : AsyncViewCommand
    {


        private UnitMovementPlanCreatedEventData unitMovementPlanCreatedEventData;

        public HighlightPathCommand(UnitMovementPlanCreatedEventData data)
        {
            this.unitMovementPlanCreatedEventData = data;
        }

        protected override void ProcessImpl()
        {

            // MikeAndConquerGame.instance.UpdateUnitPosition(unitPositionChangedEventData);
            MikeAndConquerGame.instance.HighlightPath(unitMovementPlanCreatedEventData.PathSteps);

        }
    }
}
