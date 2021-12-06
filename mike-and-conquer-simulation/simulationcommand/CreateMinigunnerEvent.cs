using mike_and_conquer_simulation.main;

namespace mike_and_conquer_simulation.simulationcommand
{
    public class CreateMinigunnerEvent : AsyncSimulationCommand
    {
        public int X { get; set; }
        public int Y { get; set; }


        protected override void ProcessImpl()
        {
            result = SimulationMain.instance.CreateMinigunner(X, Y);
        }

        public Minigunner GetMinigunner()
        {
            return (Minigunner) GetResult();
        }
    }
}