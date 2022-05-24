
namespace mike_and_conquer_simulation.events
{
    class UnitMovementPlanCreatedEventData
    {

        // public int XInWorldCoordinates { get; set; }
        // public int YInWorldCoordinates { get; set; }
        //
        // public int UnitId { get; set; }
        //
        // public long Timestamp { get; set; }

        public int NumSteps { get; set; }

        public const string EventName = "UnitMovementPlanCreated";


    }
}
