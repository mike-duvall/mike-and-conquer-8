﻿namespace mike_and_conquer_simulation.events
{
    public class MinigunnerCreateEventData
    {
        public int X { get; set; }
        public int Y { get; set; }
        
        public int UnitId { get; set; }

        public const string EventName = "MinigunnerCreated";


    }
}



