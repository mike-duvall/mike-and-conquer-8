using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace mike_and_conquer_simulation.main
{
    public class Minigunner
    {
        public float X { get; set; }
        public float Y { get; set; }

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
            this.movementDelta = 0.05f;
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

                    //                    (int)Math.Round(precise, 0)

                    // eventData.XInWorldCoordinates = (int) this.X;
                    // eventData.YInWorldCoordinates = (int) this.Y;

                    eventData.XInWorldCoordinates = (int) Math.Round(this.X, 0);
                    eventData.YInWorldCoordinates = (int) Math.Round(this.Y, 0);


                    simulationStateUpdateEvent.EventData = JsonConvert.SerializeObject(eventData);

                    SimulationMain.instance.PublishEvent(simulationStateUpdateEvent);
                    // foreach (SimulationStateListener listener in listeners)
                    // {
                    //     listener.Update(simulationStateUpdateEvent);
                    // }


                }
                else
                {
                    if (X < destinationXInWorldCoordinates)
                    {
                        X += movementDelta;
                    }
                    else if (X > destinationXInWorldCoordinates)
                    {
                        X -= movementDelta;
                    }

                    if (Y < destinationYInWorldCoordinates)
                    {
                        Y += movementDelta;
                    }
                    else if (Y > destinationYInWorldCoordinates)
                    {
                        Y -= movementDelta;
                    }
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
            return (this.X > (destinationX - movementDistanceEpsilon));
        }


        private bool IsFarEnoughLeft(int destinationX)
        {
            // return (GameWorldLocation.WorldCoordinatesAsVector2.X < (destinationX + movementDistanceEpsilon));
            return (X < (destinationX + movementDistanceEpsilon));

        }

        private bool IsFarEnoughDown(int destinationY)
        {
            // return (GameWorldLocation.WorldCoordinatesAsVector2.Y > (destinationY - movementDistanceEpsilon));
            return (Y > (destinationY - movementDistanceEpsilon));
        }

        private bool IsFarEnoughUp(int destinationY)
        {
            // return (GameWorldLocation.WorldCoordinatesAsVector2.Y < (destinationY + movementDistanceEpsilon));
            return (Y < (destinationY + movementDistanceEpsilon));
        }




    }
}
