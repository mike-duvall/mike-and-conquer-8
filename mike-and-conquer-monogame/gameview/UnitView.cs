using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using mike_and_conquer.gameview;
using mike_and_conquer_monogame.gameview;
using mike_and_conquer_simulation.events;

namespace mike_and_conquer_monogame.main
{
    public  class UnitView
    {
        public int XInWorldCoordinates { get; set; }
        public int YInWorldCoordinates { get; set; }

        public int UnitId { get; set; }

        public bool Selected { get; set; }


        public String type;

        public Color color;

        protected UnitSize unitSize;

        protected PlannedPathView plannedPathView;


        public UnitSize UnitSize
        {
            get { return unitSize; }
        }

        protected Point selectionCursorOffset;
        public Point SelectionCursorOffset
        {
            get { return selectionCursorOffset; }
        }

        public void UpdatePlannedPathView(List<PathStep> pathStepList)
        {
            plannedPathView = new PlannedPathView(pathStepList);

        }

        public void UpdatePlannedPathView(UnitArrivedAtPathStepEventData unitArrivedAtPathStepEventData)
        {

            plannedPathView.RemoveFromPlannedPath(unitArrivedAtPathStepEventData.PathStep.X,
                unitArrivedAtPathStepEventData.PathStep.Y);

        }


        
    }
}
