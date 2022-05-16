
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

    }
}
