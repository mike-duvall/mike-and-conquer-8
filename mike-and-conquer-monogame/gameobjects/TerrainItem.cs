
using System;
using System.Collections.Generic;
using mike_and_conquer.gameview;
using mike_and_conquer.gameworld;
// using mike_and_conquer.main;
using Point = Microsoft.Xna.Framework.Point;

namespace mike_and_conquer.gameobjects
{ 

    public class TerrainItem
    {


        private MapTileLocation mapTileLocation;

        public MapTileLocation MapTileLocation
        {
            get { return mapTileLocation; }
        }

        public String TerrainItemType
        {
            get { return terrainItemDescriptor.TerrainItemType; }
        }

        private TerrainItemDescriptor terrainItemDescriptor;

        private float layerDepthOffset;

        public float LayerDepthOffset
        {
            get { return layerDepthOffset; }
        }


        protected TerrainItem()
        {
        }


        public TerrainItem(int x, int y, TerrainItemDescriptor terrainItemDescriptor, float layerDepthOffset)
        {
            mapTileLocation = MapTileLocation.CreateFromWorldCoordinates(x, y);
            this.terrainItemDescriptor = terrainItemDescriptor;
            this.layerDepthOffset = layerDepthOffset;
        }


        public List<MapTileInstance> GetBlockedMapTileInstances()
        {
            List<MapTileInstance> blockMapTileInstances = new List<MapTileInstance>();
            List<Point> blockMapTileRelativeCoordinates = terrainItemDescriptor.GetBlockMapTileRelativeCoordinates();

            foreach (Point point in blockMapTileRelativeCoordinates)
            {
                MapTileLocation relativeMapTileLocation = MapTileLocation
                    .Clone()
                    .IncrementWorldMapTileX(point.X)
                    .IncrementWorldMapTileY(point.Y);

                MapTileInstance blockedMapTile = GameWorld.instance.FindMapTileInstance(
                    relativeMapTileLocation);
                blockMapTileInstances.Add(blockedMapTile);
            }

            return blockMapTileInstances;

        }

    }


}
