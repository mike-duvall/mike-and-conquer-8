using System.Collections.Generic;
using mike_and_conquer_simulation.main;
using mike_and_conquer_simulation.main.events;

namespace mike_and_conquer_simulation.simulationcommand
{
    public class GetCopyOfEventHistoryCommand : AsyncSimulationCommand
    {


        protected override void ProcessImpl()
        {
            result = SimulationMain.instance.GetCopyOfEventHistory();

        }

        public List<SimulationStateUpdateEvent> GetCopyOfEventHistory()
        {
            return (List<SimulationStateUpdateEvent>)GetResult();

        }

    }
}
