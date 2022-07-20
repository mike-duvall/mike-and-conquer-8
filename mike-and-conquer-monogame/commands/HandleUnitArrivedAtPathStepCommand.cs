﻿
using mike_and_conquer.gameview;
using mike_and_conquer_monogame.main;

using mike_and_conquer_simulation.events;

namespace mike_and_conquer_monogame.commands
{
    public class HandleUnitArrivedAtPathStepCommand : AsyncViewCommand
    {


        // private UnitArrivedAtPathStepEventData unitArrivedAtPathStepEventData;
        private int unitId;
        private PathStep pathStep;

        public HandleUnitArrivedAtPathStepCommand(int unitId, PathStep pathStep)
        {
            this.unitId = unitId;
            this.pathStep = pathStep;
        }

        protected override void ProcessImpl()
        {

            MikeAndConquerGame.instance.HandleUnitArrivedAtPathStep(unitId, pathStep);

        }
    }
}
