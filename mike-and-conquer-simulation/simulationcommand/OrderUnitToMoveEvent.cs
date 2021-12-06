using mike_and_conquer_simulation.main;

namespace mike_and_conquer_simulation.simulationcommand
{
    public class OrderUnitToMoveEvent : AsyncSimulationCommand
    {
        public int DestinationXInWorldCoordinates { get; set; }
        public int DestinationYInWorldCoordinates { get; set; }

        public int UnitId { get; set; }


        protected override void ProcessImpl()
        {
//            result = SimulationMain.instance.CreateMinigunner(X, Y);
            SimulationMain.instance.OrderUnitToMove(UnitId, DestinationXInWorldCoordinates,
                DestinationYInWorldCoordinates);

            result = true;

        }

        // public Minigunner GetMinigunner()
        // {
        //     return (Minigunner)GetResult();
        //
        // }

    }
}
