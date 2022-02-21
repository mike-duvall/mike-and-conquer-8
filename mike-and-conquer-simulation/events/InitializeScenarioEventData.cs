using System.Collections.Generic;

namespace mike_and_conquer_simulation.events
{
    public class InitializeScenarioEventData
    {
        public int MapWidth { get; set; }
        public int MapHeight { get; set; }
        
        public const string EventName = "InitializeScenario";

        public List<MapTileInstanceCreateEventData> MapTileInstanceCreateEventDataList;
        public List<TerrainItemCreateEventData> TerrainItemCreateEventDataList;

    }
}
