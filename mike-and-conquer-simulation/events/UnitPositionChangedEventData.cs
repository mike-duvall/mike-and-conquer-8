namespace mike_and_conquer_simulation.events
{
    public class UnitPositionChangedEventData
    {
        public int XInWorldCoordinates { get; set; }
        public int YInWorldCoordinates { get; set; }
        
        public int UnitId { get; set; }

        public const string EventName = "UnitPositionChanged";

    }
}
