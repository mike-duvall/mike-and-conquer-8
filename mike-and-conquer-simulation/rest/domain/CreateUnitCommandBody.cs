using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mike_and_conquer_simulation.rest.domain
{
    class CreateUnitCommandBody
    {
        public int StartLocationXInWorldCoordinates { get; set; }

        public int StartLocationYInWorldCoordinates { get; set; }

    }
}
