using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mike_and_conquer_simulation.rest.domain
{
    class RestOrderUnitMoveCommandBody
    {
        public int DestinationLocationXInWorldCoordinates { get; set; }

        public int DestinationLocationYInWorldCoordinates { get; set; }

        public int UnitId { get; set; }

    }
}
