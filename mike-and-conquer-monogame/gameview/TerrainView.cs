using Microsoft.Xna.Framework;
using mike_and_conquer.gamesprite;
using mike_and_conquer.util;
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


        // private TerrainItem terrainItem;
        //
        // public TerrainView(TerrainItem terrainItem)
        // {
        //     this.terrainItem = terrainItem;
        //     Point xnaPoint = new Point(terrainItem.MapTileLocation.WorldMapTileCoordinatesAsPoint.X,
        //         terrainItem.MapTileLocation.WorldMapTileCoordinatesAsPoint.Y);
        //     this.terrainSprite = new TerrainSprite(terrainItem.TerrainItemType, xnaPoint);
        // }


        // public void DrawFull(GameTime gameTime, SpriteBatch spriteBatch)
        // {
        //     DrawNoShadow(gameTime, spriteBatch);
        //     DrawShadowOnly(gameTime,spriteBatch);
        // }
        //
        // public void DrawShadowOnly(GameTime gameTime, SpriteBatch spriteBatch)
        // {
        //     
        //     // this.terrainSprite.DrawShadowOnly(gameTime, spriteBatch, terrainItem.MapTileLocation.WorldCoordinatesAsVector2);
        //     this.terrainSprite.DrawShadowOnly(
        //         gameTime,
        //         spriteBatch,
        //         MonogameUtil.ConvertSystemNumericsVector2ToXnaVector2(terrainItem.MapTileLocation.WorldCoordinatesAsVector2)
        //         );
        // }
        //
        //
        //
        // public void DrawNoShadow(GameTime gameTime, SpriteBatch spriteBatch)
        // {
        //     this.terrainSprite.DrawNoShadow(
        //         gameTime,
        //         spriteBatch, 
        //         MonogameUtil.ConvertSystemNumericsVector2ToXnaVector2(terrainItem.MapTileLocation.WorldCoordinatesAsVector2),
        //         terrainItem.LayerDepthOffset);
        // }
    }
}
