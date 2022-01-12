

using System;


namespace mike_and_conquer_monogame.main

{
    public class SimulationOptions
    {

        public enum GameSpeed
        {
            Slowest = 250,
            Slower = 125,
            Slow = 85, 
            Moderate = 63,
            Normal = 40,
            Fast = 30,
            Faster = 25,
            Fastest = 23
        }


        // public GameSpeed CurrentGameSpeed = GameSpeed.Moderate;
        public GameSpeed CurrentGameSpeed = GameSpeed.Slowest;
        //
        //
        // public static SimulationOptions instance;
        //
        // public SimulationOptions()
        // {
        //     SimulationOptions.instance = this;
        // }
        //

    }
}

