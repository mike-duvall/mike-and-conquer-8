


namespace mike_and_conquer_simulation.main
{
    internal class GameWorldLocation
    {
        private float xInWorldCoordinates;
        private float yInWorldCoordinates;


        public float X
        {
            get => xInWorldCoordinates;
            set => xInWorldCoordinates = value;
        }

        public float Y
        {
            get => yInWorldCoordinates;
            set => yInWorldCoordinates = value;
        }




        private GameWorldLocation(float x, float y)
        {
            this.xInWorldCoordinates = x;
            this.yInWorldCoordinates = y;
        }


        public static GameWorldLocation CreateFromWorldCoordinates(float x, float y)
        {
            return new GameWorldLocation(x, y);
        }

        // public Vector2 WorldCoordinatesAsVector2
        // {
        //     get { return new Vector2(xInWorldCoordinates, yInWorldCoordinates); }
        // }


    }
}
