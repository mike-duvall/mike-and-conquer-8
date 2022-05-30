
using System;
using System.Collections.Generic;


namespace mike_and_conquer_simulation.events
{
    class UnitMovementPlanCreatedEventData
    {

        public List<PathStep> PathSteps { get;  }

        public int UnitId { get;  }

        public long Timestamp { get;  }


        public const string EventName = "UnitMovementPlanCreated";

        public UnitMovementPlanCreatedEventData(int unitId, List<PathStep> listOfPathSteps)
        {
            this.Timestamp = DateTime.Now.Ticks;
            this.UnitId = unitId;
            this.PathSteps = listOfPathSteps;

        }

    }
}
