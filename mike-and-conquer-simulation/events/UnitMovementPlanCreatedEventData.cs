
using System;
using System.Collections.Generic;


namespace mike_and_conquer_simulation.events
{
    class UnitMovementPlanCreatedEventData
    {

        public const string EventType = "UnitMovementPlanCreated";

        public int UnitId { get; }
        public long Timestamp { get; }
        public List<PathStep> PathSteps { get;  }


        public UnitMovementPlanCreatedEventData(int unitId, List<PathStep> listOfPathSteps)
        {
            this.Timestamp = DateTime.Now.Ticks;
            this.UnitId = unitId;
            this.PathSteps = listOfPathSteps;

        }

    }
}
