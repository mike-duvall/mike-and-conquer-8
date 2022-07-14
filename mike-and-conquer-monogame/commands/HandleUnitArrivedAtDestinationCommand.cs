
using System;
using mike_and_conquer_monogame.main;

using mike_and_conquer_simulation.events;

namespace mike_and_conquer_monogame.commands
{
    public class HandleUnitArrivedAtDestinationCommand : AsyncViewCommand
    {


        private UnitArrivedAtDestinationEventData unitArrivedAtDestinationEventData;

        public HandleUnitArrivedAtDestinationCommand(UnitArrivedAtDestinationEventData data)
        {
            this.unitArrivedAtDestinationEventData = data;
        }

        protected override void ProcessImpl()
        {

            // MikeAndConquerGame.instance.UpdateUnitMovementPlan(unitArrivedAtDestinationEventData);
            throw new Exception("Not implemented");

        }
    }
}
