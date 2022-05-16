namespace mike_and_conquer_simulation.events
{
    public class TerrainItemCreateEventData
    {


        //     public int UnitId { get; set; }
        //
        //
        public int XInWorldMapTileCoordinates { get; }
        
        public int YInWorldMapTileCoordinates { get; }


        public string TerrainItemType { get; }


        //     public string TextureKey { get; }
        //
        //     public byte ImageIndex { get;  }
        //
        //     public bool IsBlockingTerrain { get; }
        //
        //
        //     public string Visibility { get; }
        //
        //     public const string EventName = "MapTileInstanceCreated";
        //
        //     private TerrainItemCreateEventData()
        //     {
        //     }
        //
        //     public TerrainItemCreateEventData(int id, int xInWorldMapTileCoordinates, int yInWorldMapTileCoordinates, string textureKey, byte imageIndex, bool isBlockingTerrain,  string visibility)
        //     {
        //         UnitId = id;
        //         XInWorldMapTileCoordinates = xInWorldMapTileCoordinates;
        //         YInWorldMapTileCoordinates = yInWorldMapTileCoordinates;
        //         TextureKey = textureKey;
        //         ImageIndex = imageIndex;
        //         IsBlockingTerrain = isBlockingTerrain;
        //         Visibility = visibility;
        //     }

        public TerrainItemCreateEventData(int xInWorldMapTileCoordinates, int yInWorldMapTileCoordinates, string terrainItemType)
        {
            this.XInWorldMapTileCoordinates = xInWorldMapTileCoordinates;
            this.YInWorldMapTileCoordinates = yInWorldMapTileCoordinates;
            this.TerrainItemType = terrainItemType;
        }


    }
}



