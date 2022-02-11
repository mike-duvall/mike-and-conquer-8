
using System;


using MapTileInstance= mike_and_conquer_simulation.gameworld.MapTileInstance;

namespace mike_and_conquer.gameview
{
    public class MapTileShroudMapping
    {



        public MapTileInstance.MapTileVisibility east;
        public MapTileInstance.MapTileVisibility southEast;
        public MapTileInstance.MapTileVisibility south;
        public MapTileInstance.MapTileVisibility southWest;
        public MapTileInstance.MapTileVisibility west;
        public MapTileInstance.MapTileVisibility northWest;
        public MapTileInstance.MapTileVisibility north;
        public MapTileInstance.MapTileVisibility northEast;

        public int shroudTileIndex;
        

        public MapTileShroudMapping(
            MapTileInstance.MapTileVisibility east,
            MapTileInstance.MapTileVisibility southEast,
            MapTileInstance.MapTileVisibility south,
            MapTileInstance.MapTileVisibility southWest,
            MapTileInstance.MapTileVisibility west,
            MapTileInstance.MapTileVisibility northWest,
            MapTileInstance.MapTileVisibility north,
            MapTileInstance.MapTileVisibility northEast,

            int shroudTileIndex)
        {
            this.east = east;
            this.south = south;
            this.west = west;
            this.north = north;
            this.northEast = northEast;
            this.southEast = southEast;
            this.southWest = southWest;
            this.northWest = northWest;
            this.shroudTileIndex = shroudTileIndex;
        }





    }
}