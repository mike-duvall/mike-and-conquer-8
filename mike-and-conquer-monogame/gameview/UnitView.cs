using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using mike_and_conquer.gameobjects;

namespace mike_and_conquer_monogame.main
{
    public  class UnitView
    {
        public int XInWorldCoordinates { get; set; }
        public int YInWorldCoordinates { get; set; }

        public int ID { get; set; }

        public bool Selected { get; set; }


        public String type;

        public Color color;

        protected UnitSize unitSize;

        public UnitSize UnitSize
        {
            get { return unitSize; }
        }

        protected Point selectionCursorOffset;
        public Point SelectionCursorOffset
        {
            get { return selectionCursorOffset; }
        }



    }
}
