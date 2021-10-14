using System.Collections.Generic;
using mike_and_conquer_simulation.main;

namespace mike_and_conquer_simulation.rest.simulationevent
{
    public class GetCopyOfEventHistoryEvent : AsyncGameEvent
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
