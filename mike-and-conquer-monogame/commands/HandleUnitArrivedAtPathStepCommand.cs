
using mike_and_conquer_monogame.main;

using mike_and_conquer_simulation.events;

namespace mike_and_conquer_monogame.commands
{
    public class HandleUnitArrivedAtPathStepCommand : AsyncViewCommand
    {


        private UnitArrivedAtPathStepEventData unitArrivedAtPathStepEventData;

        public HandleUnitArrivedAtPathStepCommand(UnitArrivedAtPathStepEventData data)
        {
            this.unitArrivedAtPathStepEventData = data;
        }

        protected override void ProcessImpl()
        {

            MikeAndConquerGame.instance.UpdateUnitMovementPlan(unitArrivedAtPathStepEventData);

        }
    }
}
