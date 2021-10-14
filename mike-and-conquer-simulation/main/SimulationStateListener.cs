using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mike_and_conquer_simulation.main
{
    public abstract class SimulationStateListener
    {
        public abstract void Update(SimulationStateUpdateEvent anEvent);
    }
}
