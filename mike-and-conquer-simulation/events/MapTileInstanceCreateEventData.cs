﻿namespace mike_and_conquer_simulation.events
{
    public class MapTileInstanceCreateEventData
    {


        public int ID { get; set; }


        public int XInWorldMapTileCoordinates { get; }

        public int YInWorldMapTileCoordinates { get; }

        public string TextureKey { get; }

        public byte ImageIndex { get;  }

        public bool IsBlockingTerrain { get; }


        public string Visibility { get; }

        public const string EventName = "MapTileInstanceCreated";

        private MapTileInstanceCreateEventData()
        {
        }

        public MapTileInstanceCreateEventData(int id, int xInWorldMapTileCoordinates, int yInWorldMapTileCoordinates, string textureKey, byte imageIndex, bool isBlockingTerrain,  string visibility)
        {
            ID = id;
            XInWorldMapTileCoordinates = xInWorldMapTileCoordinates;
            YInWorldMapTileCoordinates = yInWorldMapTileCoordinates;
            TextureKey = textureKey;
            ImageIndex = imageIndex;
            IsBlockingTerrain = isBlockingTerrain;
            Visibility = visibility;
        }
    }
}



