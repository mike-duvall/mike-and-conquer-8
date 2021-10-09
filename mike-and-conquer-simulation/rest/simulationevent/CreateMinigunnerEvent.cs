using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mike_and_conquer_simulation.main;
using mike_and_conquer_simulation.rest.simulationevent;

namespace mike_and_conquer_simulation.simulationevent
{
    public class CreateMinigunnerEvent : AsyncGameEvent
    {
        public int X { get; set; }
        public int Y { get; set; }


        protected override void ProcessImpl()
        {
            result = SimulationMain.instance.CreateMinigunner(X, Y);
        }

        public Minigunner GetMinigunner()
        {
            return (Minigunner)GetResult();

        }

    }
}
