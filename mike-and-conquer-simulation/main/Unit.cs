
using System;
using mike_and_conquer_simulation.events;
using Newtonsoft.Json;

namespace mike_and_conquer_simulation.main
{
    internal abstract class Unit
    {

        protected GameWorldLocation gameWorldLocation;

        public GameWorldLocation GameWorldLocation
        {
            get { return gameWorldLocation; }
        }

        public int UnitId { get; set; }

        public abstract void Update();

        public abstract void OrderMoveToDestination(int destinationXInWorldCoordinates,
            int destinationYInWorldCoordinates);

        protected void PublishUnitArrivedAtDestinationEvent()
        {
            UnitArrivedAtDestinationEventData eventData = new UnitArrivedAtDestinationEventData();
            eventData.UnitId = this.UnitId;
            eventData.Timestamp = DateTime.Now.Ticks;


            eventData.XInWorldCoordinates = (int)Math.Round(this.gameWorldLocation.X, 0);
            eventData.YInWorldCoordinates = (int)Math.Round(this.gameWorldLocation.Y, 0);

            string serializedEventData = JsonConvert.SerializeObject(eventData);
            SimulationStateUpdateEvent simulationStateUpdateEvent =
                new SimulationStateUpdateEvent(
                    UnitArrivedAtDestinationEventData.EventName,
                    serializedEventData);


            SimulationMain.instance.PublishEvent(simulationStateUpdateEvent);
        }


        protected void PublishUnitPositionChangedEventData()
        {
            UnitPositionChangedEventData eventData = new UnitPositionChangedEventData();
            eventData.UnitId = this.UnitId;


            eventData.XInWorldCoordinates = (int)Math.Round(this.gameWorldLocation.X, 0);
            eventData.YInWorldCoordinates = (int)Math.Round(this.gameWorldLocation.Y, 0);

            string serializedEventData = JsonConvert.SerializeObject(eventData);

            SimulationStateUpdateEvent simulationStateUpdateEvent = 
                new SimulationStateUpdateEvent(
                    UnitPositionChangedEventData.EventName,
                    serializedEventData);

            SimulationMain.instance.PublishEvent(simulationStateUpdateEvent);
        }


    }
}
