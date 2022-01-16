

using System;


namespace mike_and_conquer_monogame.main

{
    public class SimulationOptions
    {

        public enum GameSpeed
        {
            Slowest = 252,  // verified
            Slower = 125,
            Slow = 85, 
            Moderate = 63,
            Normal = 42,  // verified
            Fast = 30,
            Faster = 25,
            Fastest = 23  // verified
        }


        public GameSpeed CurrentGameSpeed = GameSpeed.Moderate;

    }
}

