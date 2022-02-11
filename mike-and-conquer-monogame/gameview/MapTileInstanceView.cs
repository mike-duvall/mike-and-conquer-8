using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using mike_and_conquer.gamesprite;
using mike_and_conquer.main;
using mike_and_conquer.util;
using mike_and_conquer_monogame.main;
using mike_and_conquer_simulation.gameworld;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;

namespace mike_and_conquer.gameview
{
    public class MapTileInstanceView
    {
        public SingleTextureSprite singleTextureSprite;
        public MapTileInstance myMapTileInstance;

        // TODO Refactor handling of map shroud masks.  Consider pulling out everything into separate class(es)
        private static Texture2D visibleMask = null;
        private PartiallyVisibileMapTileMask partiallyVisibileMapTileMask;
        private static Vector2 middleOfSpriteInSpriteCoordinates;

        private int imageIndex;
        private string textureKey;

        private Texture2D mapTileBorder;
        private Texture2D mapTileBlockingTerrainBorder;

        private List<MapTileShroudMapping> mapTileShroudMappingList;


        public MapTileInstanceView(MapTileInstance aMapTileInstance)
        {
            this.myMapTileInstance = aMapTileInstance;
            imageIndex = myMapTileInstance.ImageIndex;
            textureKey = myMapTileInstance.TextureKey;
            List<MapTileFrame> mapTileFrameList =
                MikeAndConquerGame.instance.SpriteSheet.GetMapTileFrameForTmpFile(textureKey);
            this.singleTextureSprite = new SingleTextureSprite(mapTileFrameList[imageIndex].Texture);


            InitializeVisibleMask(mapTileFrameList);

            partiallyVisibileMapTileMask = new PartiallyVisibileMapTileMask();

            mapTileBorder = TextureUtil.CreateSpriteBorderRectangleTexture(
                Color.White,
                this.singleTextureSprite.Width,
                this.singleTextureSprite.Height);

            mapTileBlockingTerrainBorder = TextureUtil.CreateSpriteBorderRectangleTexture(
                new Color(127, 255, 255, 255),
                this.singleTextureSprite.Width,
                this.singleTextureSprite.Height);


            InitializeMapTileShroudMappingList();

        }

        private void InitializeVisibleMask(List<MapTileFrame> mapTileFrameList)
        {
            if (MapTileInstanceView.visibleMask == null)
            {
                visibleMask = new Texture2D(MikeAndConquerGame.instance.GraphicsDevice,
                    mapTileFrameList[imageIndex].Texture.Width, mapTileFrameList[imageIndex].Texture.Height);

                int numPixels = mapTileFrameList[imageIndex].Texture.Width *
                                mapTileFrameList[imageIndex].Texture.Height;

                Color[] textureData = new Color[numPixels];
                visibleMask.GetData(textureData);

                for (int i = 0; i < numPixels; i++)
                {
                    Color xnaColor = Color.Transparent;
                    textureData[i] = xnaColor;
                }

                visibleMask.SetData(textureData);
                middleOfSpriteInSpriteCoordinates = new Vector2();

                middleOfSpriteInSpriteCoordinates.X = visibleMask.Width / 2;
                middleOfSpriteInSpriteCoordinates.Y = visibleMask.Height / 2;
            }
        }


        //        internal int GetPaletteIndexOfCoordinate(int x, int y)
        //        {
        //            List<MapTileFrame> mapTileFrameList =
        //                MikeAndConquerGame.instance.SpriteSheet.GetMapTileFrameForTmpFile(textureKey);
        //            MapTileFrame mapTileFrame = mapTileFrameList[imageIndex];
        //            byte[] frameData = mapTileFrame.FrameData;
        //
        //            int frameDataIndex = y * canPlaceBuildingSprite.Width + x;
        //            return frameData[frameDataIndex];
        //        }


        internal void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            singleTextureSprite.Draw(
                gameTime,
                spriteBatch,
                this.myMapTileInstance.MapTileLocation.WorldCoordinatesAsVector2,
                SpriteSortLayers.MAP_SQUARE_DEPTH,
                false,
                Color.White);

            if (GameOptions.instance.DrawTerrainBorder)
            {
                float defaultScale = 1;
                float layerDepth = 0;
                spriteBatch.Draw(mapTileBorder, this.myMapTileInstance.MapTileLocation.WorldCoordinatesAsVector2, null, Color.White,
                    0f, middleOfSpriteInSpriteCoordinates, defaultScale, SpriteEffects.None, layerDepth);
            }

            if (GameOptions.instance.DrawBlockingTerrainBorder && myMapTileInstance.IsBlockingTerrain)
            {
                float defaultScale = 1;
                float layerDepth = 0;
                spriteBatch.Draw(mapTileBlockingTerrainBorder, this.myMapTileInstance.MapTileLocation.WorldCoordinatesAsVector2, null,
                    Color.White, 0f, middleOfSpriteInSpriteCoordinates, defaultScale, SpriteEffects.None, layerDepth);
            }
        }



        // TODO:  Consider pulling map shroud code into seprate class(es)
        private void InitializeMapTileShroudMappingList()
        {
            mapTileShroudMappingList = new List<MapTileShroudMapping>();



            // Original
            // mapTileShroudMappingList.Add(new MapTileShroudMapping(
            //     MapTileInstance.MapTileVisibility.PartiallyVisible,
            //     MapTileInstance.MapTileVisibility.NotVisible,
            //     MapTileInstance.MapTileVisibility.PartiallyVisible,
            //     MapTileInstance.MapTileVisibility.Visible,
            //     0));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.Visible,
                0));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.Visible,
                0));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                0));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                0));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.Visible,
                0));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.Visible,
                0));  // TODO:  No test fails if this mapping is not here
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.Visible,
                0));  // TODO:  No test fails if this mapping is not here




            // Original
            // mapTileShroudMappingList.Add(new MapTileShroudMapping(
            //     MapTileInstance.MapTileVisibility.NotVisible,
            //     MapTileInstance.MapTileVisibility.NotVisible,
            //     MapTileInstance.MapTileVisibility.PartiallyVisible,
            //     MapTileInstance.MapTileVisibility.Visible,
            //     0));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.NotVisible,
                0));


            // Original
            // mapTileShroudMappingList.Add(new MapTileShroudMapping(
            //     MapTileInstance.MapTileVisibility.Visible,
            //     MapTileInstance.MapTileVisibility.PartiallyVisible,
            //     MapTileInstance.MapTileVisibility.NotVisible,
            //     MapTileInstance.MapTileVisibility.PartiallyVisible,
            //     1));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                1));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.Visible,
                1));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.Visible,
                1));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                1));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.Visible,
                1));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.Visible,
                1));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.Visible,
                1));




            // east: Visible,
            // south: Partial,
            // west: Partial,
            // north: Partial
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.Visible,
                1));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                1));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                1));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.Visible,
                1));  // TODO:  No test to justify this mapping



            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.Visible,
                1));


            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                1));




            // Original
            // mapTileShroudMappingList.Add(new MapTileShroudMapping(
            //     MapTileInstance.MapTileVisibility.Visible,
            //     MapTileInstance.MapTileVisibility.PartiallyVisible,
            //     MapTileInstance.MapTileVisibility.PartiallyVisible,
            //     MapTileInstance.MapTileVisibility.Visible,
            //     2));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.Visible,
                2));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.Visible,
                2));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.Visible,
                2));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.Visible,
                2));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.Visible,
                2));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.Visible,
                2));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.Visible,
                2));





            // Original
            // mapTileShroudMappingList.Add(new MapTileShroudMapping(
            //     MapTileInstance.MapTileVisibility.PartiallyVisible,
            //     MapTileInstance.MapTileVisibility.Visible,
            //     MapTileInstance.MapTileVisibility.PartiallyVisible,
            //     MapTileInstance.MapTileVisibility.NotVisible,
            //     3));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                3));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                3));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                3));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                3));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                3));







            // Original
            // mapTileShroudMappingList.Add(new MapTileShroudMapping(
            //     MapTileInstance.MapTileVisibility.NotVisible,
            //     MapTileInstance.MapTileVisibility.Visible,
            //     MapTileInstance.MapTileVisibility.PartiallyVisible,
            //     MapTileInstance.MapTileVisibility.NotVisible,
            //     3));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                3));


            // Original
            // mapTileShroudMappingList.Add(new MapTileShroudMapping(
            //     MapTileInstance.MapTileVisibility.Visible,
            //     MapTileInstance.MapTileVisibility.Visible,
            //     MapTileInstance.MapTileVisibility.PartiallyVisible,
            //     MapTileInstance.MapTileVisibility.PartiallyVisible,
            //     4));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                4));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.Visible,
                4));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                4));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.Visible,
                4));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.Visible,
                4));


            // Original
            // mapTileShroudMappingList.Add(new MapTileShroudMapping(
            //     MapTileInstance.MapTileVisibility.NotVisible,
            //     MapTileInstance.MapTileVisibility.PartiallyVisible,
            //     MapTileInstance.MapTileVisibility.Visible,
            //     MapTileInstance.MapTileVisibility.PartiallyVisible,
            //     5));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                5));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                5));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                5));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                5));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                5));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                5));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                5));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                5));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                5));


            // Original
            // mapTileShroudMappingList.Add(new MapTileShroudMapping(
            //     MapTileInstance.MapTileVisibility.PartiallyVisible,
            //     MapTileInstance.MapTileVisibility.PartiallyVisible,
            //     MapTileInstance.MapTileVisibility.Visible,
            //     MapTileInstance.MapTileVisibility.PartiallyVisible,
            //     5));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                5));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                5));




            // Original
            // mapTileShroudMappingList.Add(new MapTileShroudMapping(
            //     MapTileInstance.MapTileVisibility.PartiallyVisible,
            //     MapTileInstance.MapTileVisibility.PartiallyVisible,
            //     MapTileInstance.MapTileVisibility.Visible,
            //     MapTileInstance.MapTileVisibility.Visible,
            //     6));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                6));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                6));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.Visible,
                6));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.Visible,
                6));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.Visible,
                6));


            // Original
            // mapTileShroudMappingList.Add(new MapTileShroudMapping(
            //     MapTileInstance.MapTileVisibility.PartiallyVisible,
            //     MapTileInstance.MapTileVisibility.Visible,
            //     MapTileInstance.MapTileVisibility.Visible,
            //     MapTileInstance.MapTileVisibility.PartiallyVisible,
            //     7));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                7));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                7));



            // Original
            // mapTileShroudMappingList.Add(new MapTileShroudMapping(
            //     MapTileInstance.MapTileVisibility.NotVisible,
            //     MapTileInstance.MapTileVisibility.Visible,
            //     MapTileInstance.MapTileVisibility.Visible,
            //     MapTileInstance.MapTileVisibility.PartiallyVisible,
            //     MapTileInstance.MapTileVisibility.NotVisible,
            //     MapTileInstance.MapTileVisibility.NotVisible,
            //     7));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                7));

            // original
            // mapTileShroudMappingList.Add(new MapTileShroudMapping(
            //     MapTileInstance.MapTileVisibility.PartiallyVisible,
            //     MapTileInstance.MapTileVisibility.NotVisible,
            //     MapTileInstance.MapTileVisibility.NotVisible,
            //     MapTileInstance.MapTileVisibility.PartiallyVisible,
            //     8));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.Visible,
                8));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.Visible,
                8));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.Visible,
                8));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.Visible,
                8));

            // Original
            // mapTileShroudMappingList.Add(new MapTileShroudMapping(
            //     MapTileInstance.MapTileVisibility.PartiallyVisible,
            //     MapTileInstance.MapTileVisibility.PartiallyVisible,
            //     MapTileInstance.MapTileVisibility.NotVisible,
            //     MapTileInstance.MapTileVisibility.PartiallyVisible,
            //     MapTileInstance.MapTileVisibility.Visible,
            //     MapTileInstance.MapTileVisibility.PartiallyVisible,
            //     8));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.Visible,
                8));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.Visible,
                8));  // TODO:  No test for this one



            // Original
            // mapTileShroudMappingList.Add(new MapTileShroudMapping(
            //     MapTileInstance.MapTileVisibility.PartiallyVisible,
            //     MapTileInstance.MapTileVisibility.PartiallyVisible,
            //     MapTileInstance.MapTileVisibility.PartiallyVisible,
            //     MapTileInstance.MapTileVisibility.PartiallyVisible,
            //     8));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.Visible,
                8));


            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.Visible,
                8));


            // Original
            // mapTileShroudMappingList.Add(new MapTileShroudMapping(
            //     MapTileInstance.MapTileVisibility.PartiallyVisible,
            //     MapTileInstance.MapTileVisibility.PartiallyVisible,
            //     MapTileInstance.MapTileVisibility.NotVisible,
            //     MapTileInstance.MapTileVisibility.PartiallyVisible,
            //     MapTileInstance.MapTileVisibility.PartiallyVisible,
            //     MapTileInstance.MapTileVisibility.Visible,
            //     9));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                9));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                9));


            // Original
            // mapTileShroudMappingList.Add(new MapTileShroudMapping(
            //     MapTileInstance.MapTileVisibility.PartiallyVisible,
            //     MapTileInstance.MapTileVisibility.PartiallyVisible,
            //     MapTileInstance.MapTileVisibility.NotVisible,
            //     MapTileInstance.MapTileVisibility.NotVisible,
            //     9));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                9));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                9));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                9));


            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                9));

            // east PartiallyVisible
            // south PartiallyVisible
            // west PartiallyVisible
            // north PartiallyVisible
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                9));



            // Original
            // mapTileShroudMappingList.Add(new MapTileShroudMapping(
            //     MapTileInstance.MapTileVisibility.NotVisible,
            //     MapTileInstance.MapTileVisibility.PartiallyVisible,
            //     MapTileInstance.MapTileVisibility.PartiallyVisible,
            //     MapTileInstance.MapTileVisibility.NotVisible,
            //     10));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                10));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                10));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                10));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                10));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                10));



            // Original
            // mapTileShroudMappingList.Add(new MapTileShroudMapping(
            //     MapTileInstance.MapTileVisibility.NotVisible,
            //     MapTileInstance.MapTileVisibility.NotVisible,
            //     MapTileInstance.MapTileVisibility.PartiallyVisible,
            //     MapTileInstance.MapTileVisibility.PartiallyVisible,
            //     11));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                11));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                11));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                11));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                11));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                11));




            // Original
            // mapTileShroudMappingList.Add(new MapTileShroudMapping(
            //     MapTileInstance.MapTileVisibility.PartiallyVisible,
            //     MapTileInstance.MapTileVisibility.NotVisible,
            //     MapTileInstance.MapTileVisibility.PartiallyVisible,
            //     MapTileInstance.MapTileVisibility.PartiallyVisible,
            //     11));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                11));



        }

        // private bool VisibilityMatches(Nullable<MapTileInstance.MapTileVisibility> expectedVisibility,
        //     Nullable<MapTileInstance.MapTileVisibility> actualVisibility)
        // {
        //     if (!expectedVisibility.HasValue)
        //     {
        //         return true;
        //     }
        //
        //     return expectedVisibility == actualVisibility;
        // }




        private int FindMapTileShroudMapping(MapTileInstance.MapTileVisibility east,
            MapTileInstance.MapTileVisibility south,
            MapTileInstance.MapTileVisibility west,
            MapTileInstance.MapTileVisibility north,
            MapTileInstance.MapTileVisibility northEast,
            MapTileInstance.MapTileVisibility southEast,
            MapTileInstance.MapTileVisibility southWest,
            MapTileInstance.MapTileVisibility northWest)
        {
            {

            }
            foreach (MapTileShroudMapping mapping in mapTileShroudMappingList)
            {

                if (mapping.east == east &&
                    mapping.south == south &&
                    mapping.west == west &&
                    mapping.north == north &&
                    // VisibilityMatches(mapping.northEast, northEast) &&
                    // VisibilityMatches(mapping.southEast, southEast) &&
                    // VisibilityMatches(mapping.southWest, southWest) &&
                    // VisibilityMatches(mapping.northWest, northWest))
                    mapping.northEast == northEast &&
                    mapping.southEast == southEast &&
                    mapping.southWest == southWest &&
                    mapping.northWest == northWest)

                {


                    // if (!mapping.northEast.HasValue || !mapping.southEast.HasValue || !mapping.southWest.HasValue ||
                    //     !mapping.northWest.HasValue)
                    // {
                    //
                    //
                    //     String nullEntryMessages = "\nMapping had null entries: \neast:" + mapping.east + "\n" +
                    //                         "south:" + mapping.south + "\n" +
                    //                         "west:" + mapping.west + "\n" +
                    //                         "north:" + mapping.north + "\n" +
                    //                         "shroudTileIndex=" + mapping.shroudTileIndex;
                    //
                    //     MikeAndConquerGame.instance.log.Information(nullEntryMessages);
                    //
                    //
                    //     String matchingMapping = "\n\nmapTileShroudMappingList.Add(new MapTileShroudMapping( \n" +
                    //                              "MapTileInstance.MapTileVisibility." + east + ", \n" +
                    //                              "MapTileInstance.MapTileVisibility." + southEast + ", \n" +
                    //                              "MapTileInstance.MapTileVisibility." + south + ", \n" +
                    //                              "MapTileInstance.MapTileVisibility." + southWest + ", \n" +
                    //                              "MapTileInstance.MapTileVisibility." + west + ", \n" +
                    //                              "MapTileInstance.MapTileVisibility." + northWest + ", \n" +
                    //                              "MapTileInstance.MapTileVisibility." + north + ", \n" +
                    //                              "MapTileInstance.MapTileVisibility." + northEast + ", \n" +
                    //                              mapping.shroudTileIndex + "));";
                    //     MikeAndConquerGame.instance.log.Information(matchingMapping);
                    //
                    //
                    //     if (east == MapTileInstance.MapTileVisibility.NotVisible &&
                    //         south == MapTileInstance.MapTileVisibility.NotVisible &&
                    //         west == MapTileInstance.MapTileVisibility.PartiallyVisible &&
                    //         north == MapTileInstance.MapTileVisibility.Visible)
                    //     {
                    //         int xx = 3;
                    //     }
                    //
                    //
                    //
                    // }


                    return mapping.shroudTileIndex;
                }
            }


            //            throw new Exception("Didn't find match");
            String missingMapping = "\nMissing mapping:\nmapTileShroudMappingList.Add(new MapTileShroudMapping( \n" +
                                     "MapTileInstance.MapTileVisibility." + east + ", \n" +
                                     "MapTileInstance.MapTileVisibility." + southEast + ", \n" +
                                     "MapTileInstance.MapTileVisibility." + south + ", \n" +
                                     "MapTileInstance.MapTileVisibility." + southWest + ", \n" +
                                     "MapTileInstance.MapTileVisibility." + west + ", \n" +
                                     "MapTileInstance.MapTileVisibility." + northWest + ", \n" +
                                     "MapTileInstance.MapTileVisibility." + north + ", \n" +
                                     "MapTileInstance.MapTileVisibility." + northEast + ", \n" +
                                     "?));";


            // MikeAndConquerGame.instance.log.Information(missingMapping);
            MikeAndConquerGame.instance.logger.LogInformation(missingMapping);

            // throw new Exception("Didn't find match");
            return PartiallyVisibileMapTileMask.MISSING_MAPPING_INDEX;
        }


        private int DeterminePartiallyVisibleMaskTile()
        {

            MapTileInstance south = GameWorld.instance.FindMapTileInstanceAllowNull(
                myMapTileInstance.MapTileLocation.CreateAdjacentMapTileLocation(GameWorld.TILE_LOCATION.SOUTH));


            MapTileInstance.MapTileVisibility southVisibility = MapTileInstance.MapTileVisibility.NotVisible;
            if (south != null)
            {
                southVisibility = south.Visibility;
            }


            MapTileInstance north = GameWorld.instance.FindMapTileInstanceAllowNull(
                myMapTileInstance.MapTileLocation.CreateAdjacentMapTileLocation(GameWorld.TILE_LOCATION.NORTH));



            MapTileInstance.MapTileVisibility northVisibility = MapTileInstance.MapTileVisibility.NotVisible;
            if (north != null)
            {
                northVisibility = north.Visibility;
            }

            MapTileInstance west = GameWorld.instance.FindMapTileInstanceAllowNull(
                myMapTileInstance.MapTileLocation.CreateAdjacentMapTileLocation(GameWorld.TILE_LOCATION.WEST));


            MapTileInstance.MapTileVisibility westVisibility = MapTileInstance.MapTileVisibility.NotVisible;
            if (west != null)
            {
                westVisibility = west.Visibility;
            }

            MapTileInstance east = GameWorld.instance.FindMapTileInstanceAllowNull(
                myMapTileInstance.MapTileLocation.CreateAdjacentMapTileLocation(GameWorld.TILE_LOCATION.EAST));

            MapTileInstance.MapTileVisibility eastVisibility = MapTileInstance.MapTileVisibility.NotVisible;
            if (east != null)
            {
                eastVisibility = east.Visibility;
            }

            MapTileInstance northEast = GameWorld.instance.FindMapTileInstanceAllowNull(
                myMapTileInstance.MapTileLocation.CreateAdjacentMapTileLocation(GameWorld.TILE_LOCATION.NORTH_EAST));
            MapTileInstance.MapTileVisibility northEastVisibility = MapTileInstance.MapTileVisibility.NotVisible;
            if (northEast != null)
            {
                northEastVisibility = northEast.Visibility;
            }

            MapTileInstance southEast = GameWorld.instance.FindMapTileInstanceAllowNull(
                myMapTileInstance.MapTileLocation.CreateAdjacentMapTileLocation(GameWorld.TILE_LOCATION.SOUTH_EAST));
            MapTileInstance.MapTileVisibility southEastVisibility = MapTileInstance.MapTileVisibility.NotVisible;
            if (southEast != null)
            {
                southEastVisibility = southEast.Visibility;
            }


            MapTileInstance southWest = GameWorld.instance.FindMapTileInstanceAllowNull(
                myMapTileInstance.MapTileLocation.CreateAdjacentMapTileLocation(GameWorld.TILE_LOCATION.SOUTH_WEST));
            MapTileInstance.MapTileVisibility southWestVisibility = MapTileInstance.MapTileVisibility.NotVisible;
            if (southEast != null)
            {
                southWestVisibility = southWest.Visibility;
            }

            MapTileInstance northWest = GameWorld.instance.FindMapTileInstanceAllowNull(
                myMapTileInstance.MapTileLocation.CreateAdjacentMapTileLocation(GameWorld.TILE_LOCATION.NORTH_WEST));
            MapTileInstance.MapTileVisibility northWestVisibility = MapTileInstance.MapTileVisibility.NotVisible;
            if (southEast != null)
            {
                northWestVisibility = northWest.Visibility;
            }

            return FindMapTileShroudMapping(
                eastVisibility,
                southVisibility,
                westVisibility,
                northVisibility,
                northEastVisibility,
                southEastVisibility,
                southWestVisibility,
                northWestVisibility);

        }



        internal void DrawVisbilityMask(GameTime gameTime, SpriteBatch spriteBatch)
        {
            float defaultScale = 1;

            if (this.myMapTileInstance.Visibility == MapTileInstance.MapTileVisibility.Visible)
            {
                spriteBatch.Draw(visibleMask, this.myMapTileInstance.MapTileLocation.WorldCoordinatesAsVector2, null, Color.White, 0f,
                    middleOfSpriteInSpriteCoordinates, defaultScale, SpriteEffects.None, 1.0f);
            }
            else if (this.myMapTileInstance.Visibility == MapTileInstance.MapTileVisibility.PartiallyVisible)
            {
                int index = DeterminePartiallyVisibleMaskTile();
                spriteBatch.Draw(partiallyVisibileMapTileMask.GetMask(index),
                    this.myMapTileInstance.MapTileLocation.WorldCoordinatesAsVector2, null, Color.White, 0f,
                    middleOfSpriteInSpriteCoordinates, defaultScale, SpriteEffects.None, 1.0f);
            }
        }
    }
}