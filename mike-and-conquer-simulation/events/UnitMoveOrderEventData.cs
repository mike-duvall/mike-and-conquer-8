using System;

namespace mike_and_conquer_simulation.events
{
    public class UnitMoveOrderEventData
    {
        public int DestinationXInWorldCoordinates { get;  }
        public int DestinationYInWorldCoordinates { get;  }
        
        public int UnitId { get;  }

        public long Timestamp { get;  }

        public const string EventName = "UnitOrderedToMove";


        public UnitMoveOrderEventData(int unitId, int destinationXInWorldCoordinates, int destinationYInWorldCoordinates)
        {
            this.Timestamp = DateTime.Now.Ticks;
            this.UnitId = unitId;
            this.DestinationXInWorldCoordinates = destinationXInWorldCoordinates;
            this.DestinationYInWorldCoordinates = destinationYInWorldCoordinates;
        }

    }
}
