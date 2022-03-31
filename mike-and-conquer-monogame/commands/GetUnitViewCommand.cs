using System.Collections.Generic;
using mike_and_conquer_monogame.main;
using mike_and_conquer_simulation.events;
using mike_and_conquer_simulation.main;

namespace mike_and_conquer_simulation.commands
{
    internal class GetUnitViewCommand : AsyncViewCommand
    {

        private int unitId;



        public GetUnitViewCommand(int unitId)
        {
            this.unitId = unitId;
        }

        protected override void ProcessImpl()
        {
            result = MikeAndConquerGame.instance.GetUnitViewById(unitId);

        }

        public UnitView GetUnitView()
        {
            return (UnitView) GetResult();
        }


    }
}
