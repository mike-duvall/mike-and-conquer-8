﻿using System;
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
        // public MapTileInstance myMapTileInstance;

        // TODO Refactor handling of map shroud masks.  Consider pulling out everything into separate class(es)
        private static Texture2D visibleMask = null;
        private PartiallyVisibileMapTileMask partiallyVisibileMapTileMask;
        private static Vector2 middleOfSpriteInSpriteCoordinates;

        private int imageIndex;
        // private string textureKey;
        private bool isBlockingTerrain;

        private MapTileVisibility visibility;

        private Texture2D mapTileBorder;
        private Texture2D mapTileBlockingTerrainBorder;

        private List<MapTileShroudMapping> mapTileShroudMappingList;

        public enum MapTileVisibility
        {
            NotVisible,
            PartiallyVisible,
            Visible
        }


        private int xInWorldMapTileCoordinates;
        private int yInWorldMapTileCoordinates;

        public MapTileInstanceView(int xInWorldMapTileCoordinates, int yInWorldMapTileCoordinates, int imageIndex, string textureKey, bool isBlockingTerrain, MapTileVisibility mapTileVisibility)
        {

            this.xInWorldMapTileCoordinates = xInWorldMapTileCoordinates;
            this.yInWorldMapTileCoordinates = yInWorldMapTileCoordinates;
            this.imageIndex = imageIndex;
            // this.textureKey = textureKey;
            this.isBlockingTerrain = isBlockingTerrain;
            this.visibility = mapTileVisibility;
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
            // Vector2 worldCoordinatesAsXnaVector2 =
            //     MonogameUtil.ConvertSystemNumericsVector2ToXnaVector2(this.myMapTileInstance.MapTileLocation
            //         .WorldCoordinatesAsVector2);

            Point pointInWorldCoordinates =
                GameWorldView.ConvertMapTileCoordinatesToWorldCoordinates(new Point(xInWorldMapTileCoordinates,
                    yInWorldMapTileCoordinates));

            Vector2 worldCoordinatesAsXnaVector2 = new Vector2(pointInWorldCoordinates.X, pointInWorldCoordinates.Y);

            singleTextureSprite.Draw(
                gameTime,
                spriteBatch,
                worldCoordinatesAsXnaVector2,
                SpriteSortLayers.MAP_SQUARE_DEPTH,
                false,
                Color.White);

            if (GameOptions.instance.DrawTerrainBorder)
            {
                float defaultScale = 1;
                float layerDepth = 0;
                spriteBatch.Draw(mapTileBorder, worldCoordinatesAsXnaVector2, null, Color.White,
                    0f, middleOfSpriteInSpriteCoordinates, defaultScale, SpriteEffects.None, layerDepth);
            }

            if (GameOptions.instance.DrawBlockingTerrainBorder && this.isBlockingTerrain)
            {
                float defaultScale = 1;
                float layerDepth = 0;
                spriteBatch.Draw(mapTileBlockingTerrainBorder, worldCoordinatesAsXnaVector2, null,
                    Color.White, 0f, middleOfSpriteInSpriteCoordinates, defaultScale, SpriteEffects.None, layerDepth);
            }
        }



        // TODO:  Consider pulling map shroud code into seprate class(es)
        private void InitializeMapTileShroudMappingList()
        {
            mapTileShroudMappingList = new List<MapTileShroudMapping>();



            // Original
            // mapTileShroudMappingList.Add(new MapTileShroudMapping(
            //     MapTileInstanceView.MapTileVisibility.PartiallyVisible,
            //     MapTileInstanceView.MapTileVisibility.NotVisible,
            //     MapTileInstanceView.MapTileVisibility.PartiallyVisible,
            //     MapTileInstanceView.MapTileVisibility.Visible,
            //     0));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.Visible,
                MapTileVisibility.Visible,
                0));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.Visible,
                MapTileVisibility.Visible,
                MapTileVisibility.Visible,
                0));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.Visible,
                MapTileVisibility.Visible,
                MapTileVisibility.PartiallyVisible,
                0));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.Visible,
                MapTileVisibility.Visible,
                MapTileVisibility.PartiallyVisible,
                0));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.Visible,
                MapTileVisibility.Visible,
                MapTileVisibility.Visible,
                0));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.Visible,
                MapTileVisibility.Visible,
                0));  // TODO:  No test fails if this mapping is not here
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.Visible,
                MapTileVisibility.Visible,
                MapTileVisibility.Visible,
                0));  // TODO:  No test fails if this mapping is not here




            // Original
            // mapTileShroudMappingList.Add(new MapTileShroudMapping(
            //     MapTileVisibility.NotVisible,
            //     MapTileVisibility.NotVisible,
            //     MapTileVisibility.PartiallyVisible,
            //     MapTileVisibility.Visible,
            //     0));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileVisibility.NotVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.Visible,
                MapTileVisibility.NotVisible,
                0));


            // Original
            // mapTileShroudMappingList.Add(new MapTileShroudMapping(
            //     MapTileVisibility.Visible,
            //     MapTileVisibility.PartiallyVisible,
            //     MapTileVisibility.NotVisible,
            //     MapTileVisibility.PartiallyVisible,
            //     1));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileVisibility.Visible,
                MapTileVisibility.Visible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                1));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileVisibility.Visible,
                MapTileVisibility.Visible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.Visible,
                1));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileVisibility.Visible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.Visible,
                1));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileVisibility.Visible,
                MapTileVisibility.Visible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                1));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileVisibility.Visible,
                MapTileVisibility.Visible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.Visible,
                1));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileVisibility.Visible,
                MapTileVisibility.Visible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.Visible,
                1));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileVisibility.Visible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.Visible,
                1));




            // east: Visible,
            // south: Partial,
            // west: Partial,
            // north: Partial
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileVisibility.Visible,
                MapTileVisibility.Visible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.Visible,
                1));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileVisibility.Visible,
                MapTileVisibility.Visible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                1));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileVisibility.Visible,
                MapTileVisibility.Visible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                1));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileVisibility.Visible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.Visible,
                1));  // TODO:  No test to justify this mapping



            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileVisibility.Visible,
                MapTileVisibility.Visible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.Visible,
                1));


            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileVisibility.Visible,
                MapTileVisibility.Visible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                1));




            // Original
            // mapTileShroudMappingList.Add(new MapTileShroudMapping(
            //     MapTileVisibility.Visible,
            //     MapTileVisibility.PartiallyVisible,
            //     MapTileVisibility.PartiallyVisible,
            //     MapTileVisibility.Visible,
            //     2));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileVisibility.Visible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.Visible,
                MapTileVisibility.Visible,
                2));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileVisibility.Visible,
                MapTileVisibility.Visible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.Visible,
                MapTileVisibility.Visible,
                2));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileVisibility.Visible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.Visible,
                MapTileVisibility.Visible,
                MapTileVisibility.Visible,
                2));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileVisibility.Visible,
                MapTileVisibility.Visible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.Visible,
                MapTileVisibility.Visible,
                MapTileVisibility.Visible,
                2));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileVisibility.Visible,
                MapTileVisibility.Visible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.Visible,
                MapTileVisibility.Visible,
                MapTileVisibility.Visible,
                2));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileVisibility.Visible,
                MapTileVisibility.Visible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.Visible,
                MapTileVisibility.Visible,
                2));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileVisibility.Visible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.Visible,
                MapTileVisibility.Visible,
                MapTileVisibility.Visible,
                2));





            // Original
            // mapTileShroudMappingList.Add(new MapTileShroudMapping(
            //     MapTileVisibility.PartiallyVisible,
            //     MapTileVisibility.Visible,
            //     MapTileVisibility.PartiallyVisible,
            //     MapTileVisibility.NotVisible,
            //     3));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.Visible,
                MapTileVisibility.Visible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.NotVisible,
                3));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.Visible,
                MapTileVisibility.Visible,
                MapTileVisibility.Visible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.NotVisible,
                3));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.Visible,
                MapTileVisibility.Visible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.NotVisible,
                3));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.Visible,
                MapTileVisibility.Visible,
                MapTileVisibility.Visible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.PartiallyVisible,
                3));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.Visible,
                MapTileVisibility.Visible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.PartiallyVisible,
                3));







            // Original
            // mapTileShroudMappingList.Add(new MapTileShroudMapping(
            //     MapTileVisibility.NotVisible,
            //     MapTileVisibility.Visible,
            //     MapTileVisibility.PartiallyVisible,
            //     MapTileVisibility.NotVisible,
            //     3));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileVisibility.NotVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.Visible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.NotVisible,
                3));


            // Original
            // mapTileShroudMappingList.Add(new MapTileShroudMapping(
            //     MapTileVisibility.Visible,
            //     MapTileVisibility.Visible,
            //     MapTileVisibility.PartiallyVisible,
            //     MapTileVisibility.PartiallyVisible,
            //     4));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileVisibility.Visible,
                MapTileVisibility.Visible,
                MapTileVisibility.Visible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                4));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileVisibility.Visible,
                MapTileVisibility.Visible,
                MapTileVisibility.Visible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.Visible,
                4));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileVisibility.Visible,
                MapTileVisibility.Visible,
                MapTileVisibility.Visible,
                MapTileVisibility.Visible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                4));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileVisibility.Visible,
                MapTileVisibility.Visible,
                MapTileVisibility.Visible,
                MapTileVisibility.Visible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.Visible,
                4));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileVisibility.Visible,
                MapTileVisibility.Visible,
                MapTileVisibility.Visible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.Visible,
                4));


            // Original
            // mapTileShroudMappingList.Add(new MapTileShroudMapping(
            //     MapTileVisibility.NotVisible,
            //     MapTileVisibility.PartiallyVisible,
            //     MapTileVisibility.Visible,
            //     MapTileVisibility.PartiallyVisible,
            //     5));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileVisibility.NotVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.Visible,
                MapTileVisibility.Visible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.NotVisible,
                5));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileVisibility.NotVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.Visible,
                MapTileVisibility.Visible,
                MapTileVisibility.Visible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.NotVisible,
                5));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileVisibility.NotVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.Visible,
                MapTileVisibility.Visible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.NotVisible,
                5));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileVisibility.NotVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.Visible,
                MapTileVisibility.Visible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.NotVisible,
                5));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileVisibility.NotVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.Visible,
                MapTileVisibility.Visible,
                MapTileVisibility.Visible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                5));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileVisibility.NotVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.Visible,
                MapTileVisibility.Visible,
                MapTileVisibility.Visible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.NotVisible,
                5));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileVisibility.NotVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.Visible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.NotVisible,
                5));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileVisibility.NotVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.Visible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.NotVisible,
                5));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileVisibility.NotVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.Visible,
                MapTileVisibility.Visible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                5));


            // Original
            // mapTileShroudMappingList.Add(new MapTileShroudMapping(
            //     MapTileVisibility.PartiallyVisible,
            //     MapTileVisibility.PartiallyVisible,
            //     MapTileVisibility.Visible,
            //     MapTileVisibility.PartiallyVisible,
            //     5));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.Visible,
                MapTileVisibility.Visible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                5));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.Visible,
                MapTileVisibility.Visible,
                MapTileVisibility.Visible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                5));




            // Original
            // mapTileShroudMappingList.Add(new MapTileShroudMapping(
            //     MapTileVisibility.PartiallyVisible,
            //     MapTileVisibility.PartiallyVisible,
            //     MapTileVisibility.Visible,
            //     MapTileVisibility.Visible,
            //     6));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.Visible,
                MapTileVisibility.Visible,
                MapTileVisibility.Visible,
                MapTileVisibility.PartiallyVisible,
                6));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.Visible,
                MapTileVisibility.Visible,
                MapTileVisibility.Visible,
                MapTileVisibility.Visible,
                MapTileVisibility.PartiallyVisible,
                6));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.Visible,
                MapTileVisibility.Visible,
                MapTileVisibility.Visible,
                MapTileVisibility.Visible,
                MapTileVisibility.Visible,
                6));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.Visible,
                MapTileVisibility.Visible,
                MapTileVisibility.Visible,
                MapTileVisibility.Visible,
                6));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.Visible,
                MapTileVisibility.Visible,
                MapTileVisibility.Visible,
                MapTileVisibility.Visible,
                MapTileVisibility.Visible,
                6));


            // Original
            // mapTileShroudMappingList.Add(new MapTileShroudMapping(
            //     MapTileVisibility.PartiallyVisible,
            //     MapTileVisibility.Visible,
            //     MapTileVisibility.Visible,
            //     MapTileVisibility.PartiallyVisible,
            //     7));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.Visible,
                MapTileVisibility.Visible,
                MapTileVisibility.Visible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.NotVisible,
                7));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.Visible,
                MapTileVisibility.Visible,
                MapTileVisibility.Visible,
                MapTileVisibility.Visible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.NotVisible,
                7));



            // Original
            // mapTileShroudMappingList.Add(new MapTileShroudMapping(
            //     MapTileVisibility.NotVisible,
            //     MapTileVisibility.Visible,
            //     MapTileVisibility.Visible,
            //     MapTileVisibility.PartiallyVisible,
            //     MapTileVisibility.NotVisible,
            //     MapTileVisibility.NotVisible,
            //     7));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileVisibility.NotVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.Visible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.Visible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.NotVisible,
                7));

            // original
            // mapTileShroudMappingList.Add(new MapTileShroudMapping(
            //     MapTileVisibility.PartiallyVisible,
            //     MapTileVisibility.NotVisible,
            //     MapTileVisibility.NotVisible,
            //     MapTileVisibility.PartiallyVisible,
            //     8));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.Visible,
                8));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.Visible,
                8));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.Visible,
                8));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.Visible,
                8));

            // Original
            // mapTileShroudMappingList.Add(new MapTileShroudMapping(
            //     MapTileVisibility.PartiallyVisible,
            //     MapTileVisibility.PartiallyVisible,
            //     MapTileVisibility.NotVisible,
            //     MapTileVisibility.PartiallyVisible,
            //     MapTileVisibility.Visible,
            //     MapTileVisibility.PartiallyVisible,
            //     8));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.Visible,
                8));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.Visible,
                8));  // TODO:  No test for this one



            // Original
            // mapTileShroudMappingList.Add(new MapTileShroudMapping(
            //     MapTileVisibility.PartiallyVisible,
            //     MapTileVisibility.PartiallyVisible,
            //     MapTileVisibility.PartiallyVisible,
            //     MapTileVisibility.PartiallyVisible,
            //     8));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.Visible,
                8));


            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.Visible,
                8));


            // Original
            // mapTileShroudMappingList.Add(new MapTileShroudMapping(
            //     MapTileVisibility.PartiallyVisible,
            //     MapTileVisibility.PartiallyVisible,
            //     MapTileVisibility.NotVisible,
            //     MapTileVisibility.PartiallyVisible,
            //     MapTileVisibility.PartiallyVisible,
            //     MapTileVisibility.Visible,
            //     9));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.Visible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                9));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.Visible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                9));


            // Original
            // mapTileShroudMappingList.Add(new MapTileShroudMapping(
            //     MapTileVisibility.PartiallyVisible,
            //     MapTileVisibility.PartiallyVisible,
            //     MapTileVisibility.NotVisible,
            //     MapTileVisibility.NotVisible,
            //     9));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.Visible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.NotVisible,
                9));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.Visible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.NotVisible,
                9));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.Visible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.PartiallyVisible,
                9));


            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.Visible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                9));

            // east PartiallyVisible
            // south PartiallyVisible
            // west PartiallyVisible
            // north PartiallyVisible
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.Visible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                9));



            // Original
            // mapTileShroudMappingList.Add(new MapTileShroudMapping(
            //     MapTileVisibility.NotVisible,
            //     MapTileVisibility.PartiallyVisible,
            //     MapTileVisibility.PartiallyVisible,
            //     MapTileVisibility.NotVisible,
            //     10));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileVisibility.NotVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.Visible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.NotVisible,
                10));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileVisibility.NotVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.Visible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.NotVisible,
                10));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileVisibility.NotVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.Visible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.NotVisible,
                10));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileVisibility.NotVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.NotVisible,
                10));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileVisibility.NotVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.Visible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.NotVisible,
                10));



            // Original
            // mapTileShroudMappingList.Add(new MapTileShroudMapping(
            //     MapTileVisibility.NotVisible,
            //     MapTileVisibility.NotVisible,
            //     MapTileVisibility.PartiallyVisible,
            //     MapTileVisibility.PartiallyVisible,
            //     11));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileVisibility.NotVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.Visible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.NotVisible,
                11));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileVisibility.NotVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.Visible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.NotVisible,
                11));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileVisibility.NotVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.Visible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                11));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileVisibility.NotVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.NotVisible,
                11));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileVisibility.NotVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.Visible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                11));




            // Original
            // mapTileShroudMappingList.Add(new MapTileShroudMapping(
            //     MapTileVisibility.PartiallyVisible,
            //     MapTileVisibility.NotVisible,
            //     MapTileVisibility.PartiallyVisible,
            //     MapTileVisibility.PartiallyVisible,
            //     11));
            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.NotVisible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.Visible,
                MapTileVisibility.PartiallyVisible,
                MapTileVisibility.PartiallyVisible,
                11));



        }

        // private bool VisibilityMatches(Nullable<MapTileVisibility> expectedVisibility,
        //     Nullable<MapTileVisibility> actualVisibility)
        // {
        //     if (!expectedVisibility.HasValue)
        //     {
        //         return true;
        //     }
        //
        //     return expectedVisibility == actualVisibility;
        // }




        private int FindMapTileShroudMapping(MapTileVisibility east,
            MapTileVisibility south,
            MapTileVisibility west,
            MapTileVisibility north,
            MapTileVisibility northEast,
            MapTileVisibility southEast,
            MapTileVisibility southWest,
            MapTileVisibility northWest)
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
                    //                              "MapTileVisibility." + east + ", \n" +
                    //                              "MapTileVisibility." + southEast + ", \n" +
                    //                              "MapTileVisibility." + south + ", \n" +
                    //                              "MapTileVisibility." + southWest + ", \n" +
                    //                              "MapTileVisibility." + west + ", \n" +
                    //                              "MapTileVisibility." + northWest + ", \n" +
                    //                              "MapTileVisibility." + north + ", \n" +
                    //                              "MapTileVisibility." + northEast + ", \n" +
                    //                              mapping.shroudTileIndex + "));";
                    //     MikeAndConquerGame.instance.log.Information(matchingMapping);
                    //
                    //
                    //     if (east == MapTileVisibility.NotVisible &&
                    //         south == MapTileVisibility.NotVisible &&
                    //         west == MapTileVisibility.PartiallyVisible &&
                    //         north == MapTileVisibility.Visible)
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
                                     "MapTileVisibility." + east + ", \n" +
                                     "MapTileVisibility." + southEast + ", \n" +
                                     "MapTileVisibility." + south + ", \n" +
                                     "MapTileVisibility." + southWest + ", \n" +
                                     "MapTileVisibility." + west + ", \n" +
                                     "MapTileVisibility." + northWest + ", \n" +
                                     "MapTileVisibility." + north + ", \n" +
                                     "MapTileVisibility." + northEast + ", \n" +
                                     "?));";


            // MikeAndConquerGame.instance.log.Information(missingMapping);
            MikeAndConquerGame.instance.logger.LogInformation(missingMapping);

            // throw new Exception("Didn't find match");
            return PartiallyVisibileMapTileMask.MISSING_MAPPING_INDEX;
        }



        private int DeterminePartiallyVisibleMaskTile()
        {

            // // MapTileInstanceView south = GameWorld.instance.FindMapTileInstanceAllowNull(
            // //     myMapTileInstance.MapTileLocation.CreateAdjacentMapTileLocation(GameWorld.TILE_LOCATION.SOUTH));
            //
            // MapTileInstanceView south = FindMapTileInstanceAllowNull(
            //     myMapTileInstance.MapTileLocation.CreateAdjacentMapTileLocation(GameWorld.TILE_LOCATION.SOUTH));
            //
            //
            //
            // MapTileVisibility southVisibility = MapTileVisibility.NotVisible;
            // if (south != null)
            // {
            //     southVisibility = south.visibility;
            // }
            //
            //
            // MapTileInstance north = GameWorld.instance.FindMapTileInstanceAllowNull(
            //     myMapTileInstance.MapTileLocation.CreateAdjacentMapTileLocation(GameWorld.TILE_LOCATION.NORTH));
            //
            //
            //
            // MapTileVisibility northVisibility = MapTileVisibility.NotVisible;
            // if (north != null)
            // {
            //     northVisibility = north.visibility;
            // }
            //
            // MapTileInstance west = GameWorld.instance.FindMapTileInstanceAllowNull(
            //     myMapTileInstance.MapTileLocation.CreateAdjacentMapTileLocation(GameWorld.TILE_LOCATION.WEST));
            //
            //
            // MapTileVisibility westVisibility = MapTileVisibility.NotVisible;
            // if (west != null)
            // {
            //     westVisibility = west.Visibility;
            // }
            //
            // MapTileInstance east = GameWorld.instance.FindMapTileInstanceAllowNull(
            //     myMapTileInstance.MapTileLocation.CreateAdjacentMapTileLocation(GameWorld.TILE_LOCATION.EAST));
            //
            // MapTileVisibility eastVisibility = MapTileVisibility.NotVisible;
            // if (east != null)
            // {
            //     eastVisibility = east.Visibility;
            // }
            //
            // MapTileInstance northEast = GameWorld.instance.FindMapTileInstanceAllowNull(
            //     myMapTileInstance.MapTileLocation.CreateAdjacentMapTileLocation(GameWorld.TILE_LOCATION.NORTH_EAST));
            // MapTileVisibility northEastVisibility = MapTileVisibility.NotVisible;
            // if (northEast != null)
            // {
            //     northEastVisibility = northEast.Visibility;
            // }
            //
            // MapTileInstance southEast = GameWorld.instance.FindMapTileInstanceAllowNull(
            //     myMapTileInstance.MapTileLocation.CreateAdjacentMapTileLocation(GameWorld.TILE_LOCATION.SOUTH_EAST));
            // MapTileVisibility southEastVisibility = MapTileVisibility.NotVisible;
            // if (southEast != null)
            // {
            //     southEastVisibility = southEast.Visibility;
            // }
            //
            //
            // MapTileInstance southWest = GameWorld.instance.FindMapTileInstanceAllowNull(
            //     myMapTileInstance.MapTileLocation.CreateAdjacentMapTileLocation(GameWorld.TILE_LOCATION.SOUTH_WEST));
            // MapTileVisibility southWestVisibility = MapTileVisibility.NotVisible;
            // if (southEast != null)
            // {
            //     southWestVisibility = southWest.Visibility;
            // }
            //
            // MapTileInstance northWest = GameWorld.instance.FindMapTileInstanceAllowNull(
            //     myMapTileInstance.MapTileLocation.CreateAdjacentMapTileLocation(GameWorld.TILE_LOCATION.NORTH_WEST));
            // MapTileVisibility northWestVisibility = MapTileVisibility.NotVisible;
            // if (southEast != null)
            // {
            //     northWestVisibility = northWest.Visibility;
            // }
            //
            // return FindMapTileShroudMapping(
            //     eastVisibility,
            //     southVisibility,
            //     westVisibility,
            //     northVisibility,
            //     northEastVisibility,
            //     southEastVisibility,
            //     southWestVisibility,
            //     northWestVisibility);
            //
            return 1;


        }



        internal void DrawVisbilityMask(GameTime gameTime, SpriteBatch spriteBatch)
        {
            float defaultScale = 1;
            // Vector2 worldCoordinatesAsXnaVector2 =
            //     MonogameUtil.ConvertSystemNumericsVector2ToXnaVector2(this.myMapTileInstance.MapTileLocation
            //         .WorldCoordinatesAsVector2);


            Point pointInWorldCoordinates =
                GameWorldView.ConvertMapTileCoordinatesToWorldCoordinates(new Point(xInWorldMapTileCoordinates,
                    xInWorldMapTileCoordinates));

            Vector2 worldCoordinatesAsXnaVector2 = new Vector2(pointInWorldCoordinates.X, pointInWorldCoordinates.Y);


            if (this.visibility == MapTileVisibility.Visible)
            {

                spriteBatch.Draw(visibleMask, worldCoordinatesAsXnaVector2, null, Color.White, 0f,
                    middleOfSpriteInSpriteCoordinates, defaultScale, SpriteEffects.None, 1.0f);
            }
            else if (this.visibility == MapTileVisibility.PartiallyVisible)
            {
                int index = DeterminePartiallyVisibleMaskTile();
                spriteBatch.Draw(partiallyVisibileMapTileMask.GetMask(index),
                    worldCoordinatesAsXnaVector2, null, Color.White, 0f,
                    middleOfSpriteInSpriteCoordinates, defaultScale, SpriteEffects.None, 1.0f);
            }
        }


        public bool ContainsPoint(int mouseX, int mouseY)
        {
            int width = GameWorldView.MAP_TILE_WIDTH;
            int height = GameWorldView.MAP_TILE_HEIGHT;


            Point mapTileInstanceViewInWorldCoordinates =
                MapTileInstanceView.ConvertMapTileCoordinatesToWorldCoordinates(new Point(xInWorldMapTileCoordinates,
                    yInWorldMapTileCoordinates));

            int leftX = mapTileInstanceViewInWorldCoordinates.X - (width / 2);
            int topY = mapTileInstanceViewInWorldCoordinates.Y - (height / 2);

            Rectangle boundRectangle = new Rectangle(leftX, topY, width, height);
            return boundRectangle.Contains(new Point(mouseX, mouseY));
        }


        public Point GetCenter()
        {
            return MapTileInstanceView.ConvertMapTileCoordinatesToWorldCoordinates(new Point(xInWorldMapTileCoordinates,
                yInWorldMapTileCoordinates));
        }


        public static Point ConvertMapTileCoordinatesToWorldCoordinates(Point pointInWorldMapSquareCoordinates)
        {

            int xInWorldCoordinates = (pointInWorldMapSquareCoordinates.X * GameWorldView.MAP_TILE_WIDTH) +
                                      (GameWorldView.MAP_TILE_WIDTH / 2);
            int yInWorldCoordinates = pointInWorldMapSquareCoordinates.Y * GameWorldView.MAP_TILE_HEIGHT +
                                      (GameWorldView.MAP_TILE_HEIGHT / 2);

            return new Point(xInWorldCoordinates, yInWorldCoordinates);
        }


    }
}