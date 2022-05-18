namespace mike_and_conquer_simulation.events
{
    public class UnitMoveOrderEventData
    {
        public int DestinationXInWorldCoordinates { get; set; }
        public int DestinationYInWorldCoordinates { get; set; }
        
        public int UnitId { get; set; }

        public long Timestamp { get; set; }

        public const string EventName = "UnitOrderedToMove";

    }
}
