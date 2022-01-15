using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mike_and_conquer_simulation.main.events;
using Newtonsoft.Json;

namespace mike_and_conquer_simulation.main
{
    public class Minigunner
    {
        // public float X { get; set; }
        // public float Y { get; set; }


        protected GameWorldLocation gameWorldLocation;

        public GameWorldLocation GameWorldLocation
        {
            get { return gameWorldLocation; }
        }


        public int ID { get; set; }


        public enum State { IDLE, MOVING, ATTACKING, LANDING_AT_MAP_SQUARE };
        public State state;

        public enum Command { NONE, ATTACK_TARGET, FOLLOW_PATH };
        public Command currentCommand;


        private int destinationXInWorldCoordinates;
        private int destinationYInWorldCoordinates;


        double movementDistanceEpsilon;
        private float movementDelta;

        public Minigunner()
        {
            state = State.IDLE;
            currentCommand = Command.NONE;
            this.movementDistanceEpsilon = 0.1f;
            // this.movementDelta = 0.15f;
            // this.movementDelta = 0.30f;
            // this.movementDelta = 0.40f;  // 14530
            // this.movementDelta = 0.50f;  // 11591
            // this.movementDelta = 0.60f; // 9745
            // this.movementDelta = 0.70f; // 8339
            // this.movementDelta = 0.80f; // 7300 ish
            //this.movementDelta = 0.83f;
            float speedFromCncInLeptons = 12;  // 12 leptons, for MCV, MPH_MEDIUM_SLOW = 12
            // float speedFromCncInLeptons = 30;  // 30 leptons, for Jeep, MPH_MEDIUM_FAST = 30


            float pixelsPerSquare = 24;
            float leptonsPerSquare = 256;
            float pixelsPerLepton = 0.09375f;
            float leptonsPerPixel = 10.66666666666667f;


            this.movementDelta = speedFromCncInLeptons * pixelsPerLepton;
            // this.movementDistanceEpsilon = 0.5f;  // worked for MCV
            this.movementDistanceEpsilon = 1.5f;  

            this.gameWorldLocation = GameWorldLocation.CreateFromWorldCoordinates(0, 0);
        }




        public void OrderMoveToDestination(int destinationXInWorldCoordinates, int destinationYInWorldCoordinates)
        {
            currentCommand = Command.FOLLOW_PATH;
            state = State.MOVING;
            this.destinationXInWorldCoordinates = destinationXInWorldCoordinates;
            this.destinationYInWorldCoordinates = destinationYInWorldCoordinates;

        }

        public void Update()
        {
            if (currentCommand == Command.FOLLOW_PATH)
            {
                if (IsAtDestination(destinationXInWorldCoordinates, destinationYInWorldCoordinates))
                {
                    currentCommand = Command.NONE;
                    state = State.IDLE;

                    SimulationStateUpdateEvent simulationStateUpdateEvent = new SimulationStateUpdateEvent();
                    simulationStateUpdateEvent.EventType = "UnitArrivedAtDestination";
                    UnitArrivedAtDestinationEventData eventData = new UnitArrivedAtDestinationEventData();
                    eventData.ID = this.ID;
                    eventData.Timestamp = DateTime.Now.Ticks;


                    eventData.XInWorldCoordinates = (int) Math.Round(this.gameWorldLocation.X, 0);
                    eventData.YInWorldCoordinates = (int) Math.Round(this.gameWorldLocation.Y, 0);

                    simulationStateUpdateEvent.EventData = JsonConvert.SerializeObject(eventData);

                    SimulationMain.instance.PublishEvent(simulationStateUpdateEvent);


                }
                else
                {
                    if (gameWorldLocation.X < destinationXInWorldCoordinates)
                    {
                        gameWorldLocation.X += movementDelta;
                    }
                    else if (gameWorldLocation.X > destinationXInWorldCoordinates)
                    {
                        gameWorldLocation.X -= movementDelta;
                    }

                    if (gameWorldLocation.Y < destinationYInWorldCoordinates)
                    {
                        gameWorldLocation.Y += movementDelta;
                    }
                    else if (gameWorldLocation.Y > destinationYInWorldCoordinates)
                    {
                        gameWorldLocation.Y -= movementDelta;
                    }

                    SimulationStateUpdateEvent simulationStateUpdateEvent = new SimulationStateUpdateEvent();
                    simulationStateUpdateEvent.EventType = "UnitPositionChanged";
                    UnitPositionChangedEventData eventData = new UnitPositionChangedEventData();
                    eventData.ID = this.ID;


                    eventData.XInWorldCoordinates = (int)Math.Round(this.gameWorldLocation.X, 0);
                    eventData.YInWorldCoordinates = (int)Math.Round(this.gameWorldLocation.Y, 0);

                    simulationStateUpdateEvent.EventData = JsonConvert.SerializeObject(eventData);

                    SimulationMain.instance.PublishEvent(simulationStateUpdateEvent);


                }

            }
            


        }

        private bool IsAtDestination(int destinationX, int destinationY)
        {
            return IsAtDestinationX(destinationX) && IsAtDestinationY(destinationY);
        }


        private bool IsAtDestinationY(int destinationY)
        {
            return (
                IsFarEnoughDown(destinationY) &&
                IsFarEnoughUp(destinationY)
            );

        }



        private bool IsAtDestinationX(int destinationX)
        {
            return (
                IsFarEnoughRight(destinationX) &&
                IsFarEnoughLeft(destinationX)
            );

        }

        private bool IsFarEnoughRight(int destinationX)
        {
            // return (GameWorldLocation.WorldCoordinatesAsVector2.X > (destinationX - movementDistanceEpsilon));
            return (this.gameWorldLocation.X > (destinationX - movementDistanceEpsilon));
        }


        private bool IsFarEnoughLeft(int destinationX)
        {
            // return (GameWorldLocation.WorldCoordinatesAsVector2.X < (destinationX + movementDistanceEpsilon));
            return (gameWorldLocation.X < (destinationX + movementDistanceEpsilon));

        }

        private bool IsFarEnoughDown(int destinationY)
        {
            // return (GameWorldLocation.WorldCoordinatesAsVector2.Y > (destinationY - movementDistanceEpsilon));
            return (gameWorldLocation.Y > (destinationY - movementDistanceEpsilon));
        }

        private bool IsFarEnoughUp(int destinationY)
        {
            // return (GameWorldLocation.WorldCoordinatesAsVector2.Y < (destinationY + movementDistanceEpsilon));
            return (gameWorldLocation.Y < (destinationY + movementDistanceEpsilon));
        }




    }
}
