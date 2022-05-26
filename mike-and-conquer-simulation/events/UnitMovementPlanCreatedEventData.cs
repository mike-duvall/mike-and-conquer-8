
using System.Collections.Generic;
using System.Diagnostics;

namespace mike_and_conquer_simulation.events
{
    class UnitMovementPlanCreatedEventData
    {

        public int NumSteps { get; set; }

        public List<PathStep> PathSteps { get; set; }

        public const string EventName = "UnitMovementPlanCreated";

    }
}
