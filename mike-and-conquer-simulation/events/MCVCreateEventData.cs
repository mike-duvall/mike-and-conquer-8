﻿namespace mike_and_conquer_simulation.events
{
    public class MCVCreateEventData : UnitCreateEventData
    {
        public const string EventName = "MCVCreated";

        public MCVCreateEventData(int unitId, int x, int y) : base(unitId, x, y)
        {
        }
    }
}



