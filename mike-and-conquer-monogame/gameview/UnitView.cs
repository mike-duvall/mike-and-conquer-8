using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace mike_and_conquer_monogame.main
{
    public  class UnitView
    {
        public int XInWorldCoordinates { get; set; }
        public int YInWorldCoordinates { get; set; }

        public int ID { get; set; }


        public String type;

        public Color color;
    }
}
