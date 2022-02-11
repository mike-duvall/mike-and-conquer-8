using mike_and_conquer.gamesprite;
using mike_and_conquer_simulation.gameworld;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using GameTime = Microsoft.Xna.Framework.GameTime;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;


namespace mike_and_conquer.gameview
{
    public class TerrainView
    {

        private TerrainSprite terrainSprite;

        public static readonly ShpFileColorMapper SHP_FILE_COLOR_MAPPER = new GdiShpFileColorMapper();


        private TerrainItem terrainItem;

        public TerrainView(TerrainItem terrainItem)
        {
            this.terrainItem = terrainItem;
            this.terrainSprite = new TerrainSprite(terrainItem.TerrainItemType, terrainItem.MapTileLocation.WorldMapTileCoordinatesAsPoint);
        }


        public void DrawFull(GameTime gameTime, SpriteBatch spriteBatch)
        {
            DrawNoShadow(gameTime, spriteBatch);
            DrawShadowOnly(gameTime,spriteBatch);
        }

        public void DrawShadowOnly(GameTime gameTime, SpriteBatch spriteBatch)
        {
            this.terrainSprite.DrawShadowOnly(gameTime, spriteBatch, terrainItem.MapTileLocation.WorldCoordinatesAsVector2);
        }

        public void DrawNoShadow(GameTime gameTime, SpriteBatch spriteBatch)
        {
            this.terrainSprite.DrawNoShadow(gameTime, spriteBatch, terrainItem.MapTileLocation.WorldCoordinatesAsVector2, terrainItem.LayerDepthOffset);
        }
    }
}
