
namespace mike_and_conquer_simulation.events
{
    public class UnitArrivedAtDestinationEventData
    {
        public int XInWorldCoordinates { get; set; }
        public int YInWorldCoordinates { get; set; }
        
        public int ID { get; set; }

        public long Timestamp { get; set; }

        public const string EventName = "UnitArrivedAtDestination";
    }
}
