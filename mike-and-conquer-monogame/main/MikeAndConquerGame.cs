using System;
using System.Collections.Generic;

using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using mike_and_conquer.gamesprite;
using mike_and_conquer.gamestate;
using mike_and_conquer.gameview;
using mike_and_conquer.gameworld;
using mike_and_conquer.openralocal;
using mike_and_conquer_simulation.commands;
using mike_and_conquer_simulation.events;

namespace mike_and_conquer_monogame.main
{
    public class MikeAndConquerGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        public ILogger logger;

        public MonogameSimulationStateListener monogameSimulationStateListener = null;

        private bool hasScenarioBeenInitialized = false;
        private int mapWidth = -10;
        private int mapHeight = -10;

        private List<UnitView> unitViewList;

        private Queue<AsyncViewCommand> inputCommandQueue;

        public static MikeAndConquerGame instance;

        public const string CONTENT_DIRECTORY_PREFIX = "Content\\";

        public SpriteSheet SpriteSheet
        {
            get { return spriteSheet; }
        }

        private SpriteSheet spriteSheet;

        public GameWorld gameWorld;
        private GameWorldView gameWorldView;

        private GameState currentGameState;

        private GameStateView currentGameStateView;

        private RAISpriteFrameManager raiSpriteFrameManager;



        public MikeAndConquerGame()
        {

            logger = MainProgram.loggerFactory.CreateLogger<MikeAndConquerGame>();
            logger.LogInformation("Game1() ctor");

            _graphics = new GraphicsDeviceManager(this);

            unitViewList = new List<UnitView>();
            inputCommandQueue = new Queue<AsyncViewCommand>();


            new GameOptions();

            if (GameOptions.instance.IsFullScreen)
            {
                _graphics.IsFullScreen = true;
                _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            }
            else
            {
                _graphics.IsFullScreen = false;
                // graphics.PreferredBackBufferWidth = 1280;
                // graphics.PreferredBackBufferHeight = 1024;
                _graphics.PreferredBackBufferWidth = 1280;
                _graphics.PreferredBackBufferHeight = 768;

                // graphics.PreferredBackBufferWidth = 1024;
                // graphics.PreferredBackBufferHeight = 768;


            }


            Content.RootDirectory = "Content";
            monogameSimulationStateListener = new MonogameSimulationStateListener(this);
            IsMouseVisible = true;
            // double currentResolution = TimerHelper.GetCurrentResolution();
            gameWorld = new GameWorld();
            gameWorldView = new GameWorldView();

            raiSpriteFrameManager = new RAISpriteFrameManager();
            spriteSheet = new SpriteSheet();
            currentGameState = new PlayingGameState();


            MikeAndConquerGame.instance = this;
        }


        public void PostCommand(AsyncViewCommand command)
        {
            lock (inputCommandQueue)
            {
                inputCommandQueue.Enqueue(command);
            }
        }

        public void ResetScenario()
        {
            unitViewList.Clear();
            hasScenarioBeenInitialized = false;
            mapWidth = -10;
            mapHeight = -10;
        }
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {

            logger.LogInformation("Game1::LoadContent()");
            logger.LogWarning("Game1::LoadContent()");

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            gameWorld.InitializeDefaultMap();

            LoadTextures();


            // gameWorld.InitializeNavigationGraph();
            gameWorldView.LoadContent();




            // TODO: use this.Content to load your game content here
        }


        private void LoadTextures()
        {
            LoadMapTextures();
            LoadSingleTextures();
            LoadShpFileTextures();
            LoadTemFiles();
            LoadBarracksPlacementTexture();
        }


        private void LoadBarracksPlacementTexture()
        {
            // LoadTmpFile(BarracksPlacementIndicatorView.FILE_NAME);
            // MapBlackMapTileFramePixelsToToTransparent(BarracksPlacementIndicatorView.FILE_NAME);
        }



        private void LoadMapTextures()
        {
            LoadTmpFile(GameMap.CLEAR1_SHP);
            LoadTmpFile(GameMap.D04_TEM);
            LoadTmpFile(GameMap.D09_TEM);
            LoadTmpFile(GameMap.D13_TEM);
            LoadTmpFile(GameMap.D15_TEM);
            LoadTmpFile(GameMap.D20_TEM);
            LoadTmpFile(GameMap.D21_TEM);
            LoadTmpFile(GameMap.D23_TEM);

            LoadTmpFile(GameMap.P07_TEM);
            LoadTmpFile(GameMap.P08_TEM);

            LoadTmpFile(GameMap.S09_TEM);
            LoadTmpFile(GameMap.S10_TEM);
            LoadTmpFile(GameMap.S11_TEM);
            LoadTmpFile(GameMap.S12_TEM);
            LoadTmpFile(GameMap.S14_TEM);
            LoadTmpFile(GameMap.S22_TEM);
            LoadTmpFile(GameMap.S29_TEM);
            LoadTmpFile(GameMap.S32_TEM);
            LoadTmpFile(GameMap.S34_TEM);
            LoadTmpFile(GameMap.S35_TEM);

            LoadTmpFile(GameMap.SH1_TEM);
            LoadTmpFile(GameMap.SH2_TEM);
            LoadTmpFile(GameMap.SH3_TEM);
            LoadTmpFile(GameMap.SH4_TEM);
            LoadTmpFile(GameMap.SH5_TEM);
            LoadTmpFile(GameMap.SH6_TEM);
            LoadTmpFile(GameMap.SH9_TEM);
            LoadTmpFile(GameMap.SH10_TEM);
            LoadTmpFile(GameMap.SH17_TEM);
            LoadTmpFile(GameMap.SH18_TEM);

            LoadTmpFile(GameMap.W1_TEM);
            LoadTmpFile(GameMap.W2_TEM);
        }

        private void LoadSingleTextures()
        {
            // spriteSheet.LoadSingleTextureFromFile(MissionAccomplishedMessage.MISSION_SPRITE_KEY, "Mission");
            // spriteSheet.LoadSingleTextureFromFile(MissionAccomplishedMessage.ACCOMPLISHED_SPRITE_KEY, "Accomplished");
            // spriteSheet.LoadSingleTextureFromFile(MissionFailedMessage.FAILED_SPRITE_KEY, "Failed");
            // spriteSheet.LoadSingleTextureFromFile(DestinationSquare.SPRITE_KEY, DestinationSquare.SPRITE_KEY);
            // spriteSheet.LoadSingleTextureFromFile(ReadyOverlay.SPRITE_KEY, ReadyOverlay.SPRITE_KEY);

        }


        private void LoadShpFileTextures()
        {
            // raiSpriteFrameManager.LoadAllTexturesFromShpFile(GdiMinigunnerView.SHP_FILE_NAME);
            // spriteSheet.LoadUnitFramesFromSpriteFrames(
            //     GdiMinigunnerView.SPRITE_KEY,
            //     raiSpriteFrameManager.GetSpriteFramesForUnit(GdiMinigunnerView.SHP_FILE_NAME),
            //     GdiMinigunnerView.SHP_FILE_COLOR_MAPPER);
            // spriteSheet.LoadUnitFramesFromSpriteFrames(
            //     NodMinigunnerView.SPRITE_KEY,
            //     raiSpriteFrameManager.GetSpriteFramesForUnit(NodMinigunnerView.SHP_FILE_NAME),
            //     NodMinigunnerView.SHP_FILE_COLOR_MAPPER);
            //
            //
            // raiSpriteFrameManager.LoadAllTexturesFromShpFile(MCVView.SHP_FILE_NAME);
            // spriteSheet.LoadUnitFramesFromSpriteFrames(
            //     MCVView.SPRITE_KEY,
            //     raiSpriteFrameManager.GetSpriteFramesForUnit(MCVView.SHP_FILE_NAME),
            //     MCVView.SHP_FILE_COLOR_MAPPER);
            //
            // raiSpriteFrameManager.LoadAllTexturesFromShpFile(SandbagView.SHP_FILE_NAME);
            // spriteSheet.LoadUnitFramesFromSpriteFrames(
            //     SandbagView.SPRITE_KEY,
            //     raiSpriteFrameManager.GetSpriteFramesForUnit(SandbagView.SHP_FILE_NAME),
            //     SandbagView.SHP_FILE_COLOR_MAPPER);
            //
            // raiSpriteFrameManager.LoadAllTexturesFromShpFile(NodTurretView.SHP_FILE_NAME);
            // spriteSheet.LoadUnitFramesFromSpriteFrames(
            //     NodTurretView.SPRITE_KEY,
            //     raiSpriteFrameManager.GetSpriteFramesForUnit(NodTurretView.SHP_FILE_NAME),
            //     NodTurretView.SHP_FILE_COLOR_MAPPER);
            //
            //
            // raiSpriteFrameManager.LoadAllTexturesFromShpFile(Projectile120mmView.SHP_FILE_NAME);
            // spriteSheet.LoadUnitFramesFromSpriteFrames(
            //     Projectile120mmView.SPRITE_KEY,
            //     raiSpriteFrameManager.GetSpriteFramesForUnit(Projectile120mmView.SHP_FILE_NAME),
            //     Projectile120mmView.SHP_FILE_COLOR_MAPPER);
            //
            //
            // raiSpriteFrameManager.LoadAllTexturesFromShpFile(MinigunnerSidebarIconView.SHP_FILE_NAME);
            // spriteSheet.LoadUnitFramesFromSpriteFrames(
            //     MinigunnerSidebarIconView.SPRITE_KEY,
            //     raiSpriteFrameManager.GetSpriteFramesForUnit(MinigunnerSidebarIconView.SHP_FILE_NAME),
            //     MinigunnerSidebarIconView.SHP_FILE_COLOR_MAPPER);
            //
            // raiSpriteFrameManager.LoadAllTexturesFromShpFile(BarracksSidebarIconView.SHP_FILE_NAME);
            // spriteSheet.LoadUnitFramesFromSpriteFrames(
            //     BarracksSidebarIconView.SPRITE_KEY,
            //     raiSpriteFrameManager.GetSpriteFramesForUnit(BarracksSidebarIconView.SHP_FILE_NAME),
            //     BarracksSidebarIconView.SHP_FILE_COLOR_MAPPER);
            //
            //
            //
            //
            // raiSpriteFrameManager.LoadAllTexturesFromShpFile(GDIBarracksView.SHP_FILE_NAME);
            // spriteSheet.LoadUnitFramesFromSpriteFrames(
            //     GDIBarracksView.SPRITE_KEY,
            //     raiSpriteFrameManager.GetSpriteFramesForUnit(GDIBarracksView.SHP_FILE_NAME),
            //     GDIBarracksView.SHP_FILE_COLOR_MAPPER);
            //
            // raiSpriteFrameManager.LoadAllTexturesFromShpFile(GDIConstructionYardView.SHP_FILE_NAME);
            // spriteSheet.LoadUnitFramesFromSpriteFrames(
            //     GDIConstructionYardView.SPRITE_KEY,
            //     raiSpriteFrameManager.GetSpriteFramesForUnit(GDIConstructionYardView.SHP_FILE_NAME),
            //     GDIConstructionYardView.SHP_FILE_COLOR_MAPPER);
            //
            raiSpriteFrameManager.LoadAllTexturesFromShpFile(PartiallyVisibileMapTileMask.SHP_FILE_NAME);
            spriteSheet.LoadUnitFramesFromSpriteFrames(PartiallyVisibileMapTileMask.SPRITE_KEY,
                raiSpriteFrameManager.GetSpriteFramesForUnit(PartiallyVisibileMapTileMask.SHP_FILE_NAME),
                PartiallyVisibileMapTileMask.SHP_FILE_COLOR_MAPPER);

        }

        private void LoadTemFiles()
        {
            LoadTerrainTexture("T01.tem");
            LoadTerrainTexture("T02.tem");
            LoadTerrainTexture("T05.tem");
            LoadTerrainTexture("T06.tem");
            LoadTerrainTexture("T07.tem");
            LoadTerrainTexture("T16.tem");
            LoadTerrainTexture("T17.tem");
            LoadTerrainTexture("TC01.tem");
            LoadTerrainTexture("TC02.tem");
            LoadTerrainTexture("TC04.tem");
            LoadTerrainTexture("TC05.tem");

        }


        private void LoadTerrainTexture(String filename)
        {
            raiSpriteFrameManager.LoadAllTexturesFromShpFile(filename);
            spriteSheet.LoadUnitFramesFromSpriteFrames(
                filename,
                raiSpriteFrameManager.GetSpriteFramesForUnit(filename),
                TerrainView.SHP_FILE_COLOR_MAPPER);

        }





        private void LoadTmpFile(string tmpFileName)
        {
            raiSpriteFrameManager.LoadAllTexturesFromTmpFile(tmpFileName);
            spriteSheet.LoadMapTileFramesFromSpriteFrames(
                tmpFileName,
                raiSpriteFrameManager.GetSpriteFramesForMapTile(tmpFileName));

        }


        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            

            base.Update(gameTime);

            // lock (inputCommandQueue)
            // {
            //     foreach (AsyncViewCommand command in inputCommandQueue)
            //     {
            //         command.Process();
            //     }
            // }

            KeyboardState newKeyboardState = Keyboard.GetState();


            gameWorldView.Update(gameTime, newKeyboardState);

            currentGameState = this.currentGameState.Update(gameTime);
            this.currentGameStateView.Update(gameTime);


        }



        public void AddMinigunner(int id, int x, int y)
        {
            UnitView unitView = new UnitView();
            unitView.ID = id;
            unitView.XInWorldCoordinates = x;
            unitView.YInWorldCoordinates = y;
            unitView.type = "Minigunner";
            unitView.color = Color.Chocolate;
            unitViewList.Add(unitView);
        }

        public void AddJeep(int id, int x, int y)
        {
            UnitView unitView = new UnitView();
            unitView.ID = id;
            unitView.XInWorldCoordinates = x;
            unitView.YInWorldCoordinates = y;
            unitView.type = "Jeep";
            unitView.color = Color.Blue;
            unitViewList.Add(unitView);
        }

        public void AddMCV(int id, int x, int y)
        {
            // hasJeepBeenCreated = true;
            // jeepX = x;
            // jeepY = y;
            UnitView unitView = new UnitView();
            unitView.ID = id;
            unitView.XInWorldCoordinates = x;
            unitView.YInWorldCoordinates = y;
            unitView.type = "MCV";
            unitView.color = Color.Yellow;
            unitViewList.Add(unitView);

        }


        protected override void Draw(GameTime gameTime)
        {
            Viewport originalViewport = GraphicsDevice.Viewport;

            GraphicsDevice.Clear(Color.Crimson);

            currentGameStateView.Draw(gameTime);
            //
            // DrawMap(gameTime);
            // DrawSidebar(gameTime);
            // DrawGameCursor(gameTime);

            // GraphicsDevice.Viewport = defaultViewport;
            GraphicsDevice.Viewport = originalViewport;
            base.Draw(gameTime);


        }


        // protected override void Draw(GameTime gameTime)
        // {
        //     GraphicsDevice.Clear(Color.CornflowerBlue);
        //     _spriteBatch.Begin();
        //
        //
        //     if (hasScenarioBeenInitialized)
        //     {
        //         DrawMap();
        //     }
        //
        //
        //
        //     foreach (UnitView unitView in unitViewList)
        //     {
        //         DrawRectangleAtCoordinate(unitView.XInWorldCoordinates, unitView.YInWorldCoordinates, unitView.color );
        //     }
        //
        //     // TODO: Add your drawing code here
        //
        //     _spriteBatch.End();
        //     base.Draw(gameTime);
        // }

        private void DrawMap()
        {

            for(int column = 0; column < this.mapWidth; column++) 
                for(int row = 0; row < this.mapHeight; row++)
                    DrawUnfilledRectangleAtCoordinate(column * 24, row * 24, 24, 24, Color.Red);

        }

        void DrawRectangleAtCoordinate(int x, int y, Color aColor)
        {
            int width = 10;
            int height = 10;
            Texture2D rect = new Texture2D(GraphicsDevice, width, height);

            Color[] data = new Color[width * height];
            for (int i = 0; i < data.Length; ++i) data[i] = aColor;
            rect.SetData(data);

            Vector2 coor = new Vector2(x, y);
            _spriteBatch.Draw(rect, coor, Color.White);
        }

        private Texture2D mapRect = null;
        void DrawUnfilledRectangleAtCoordinate(int x, int y, int width, int height, Color color)
        {
            if (mapRect == null)
            {
                mapRect = new Texture2D(GraphicsDevice, width, height);
                Color[] data = new Color[width * height];
                // Draw top line
                for (int i = 0; i < width; ++i) data[i] = color;

                // Draw left line
                for (int i = 0; i < width * height; i++)
                {
                    if (i % width == 0)
                    {
                        data[i] = color;
                    }
                }

                // Draw right line
                for (int i = 0; i < width * height; i++)
                {
                    if ((i + 1) % width == 0)
                    {
                        data[i] = color;
                    }
                }


                // Draw bottom line
                for (int i = width * height - width; i < width * height; i++) data[i] = color;

                // 012
                // 345        
                // 678

                mapRect.SetData(data);

            }

            Vector2 coor = new Vector2(x, y);
            _spriteBatch.Draw(mapRect, coor, Color.White);
        }



        public void UpdateUnitPosition(UnitPositionChangedEventData unitPositionChangedEventData)
        {
            UnitView unitView = FindUnitViewById(unitPositionChangedEventData.ID);
            unitView.XInWorldCoordinates = unitPositionChangedEventData.XInWorldCoordinates;
            unitView.YInWorldCoordinates = unitPositionChangedEventData.YInWorldCoordinates;
        }

        private UnitView FindUnitViewById(int id)
        {
            foreach (UnitView unitView in unitViewList)
            {
                if (unitView.ID == id)
                {
                    return unitView;
                }
            }

            return null;
        }

        public void InitializeScenario(InitializeScenarioEventData initializeScenarioEventData)
        {
            this.mapWidth = initializeScenarioEventData.MapWidth;
            this.mapHeight = initializeScenarioEventData.MapHeight;
            hasScenarioBeenInitialized = true;
        }

        // public void SwitchToNewGameStateViewIfNeeded()
        // {
        //     GameState currentGameState = this.GetCurrentGameState();
        //     if (currentGameState.GetType().Equals(typeof(PlayingGameState)))
        //     {
        //         HandleSwitchToPlayingGameStateView();
        //     }
        //     else if (currentGameState.GetType().Equals(typeof(MissionAccomplishedGameState)))
        //     {
        //         HandleSwitchToMissionAccomplishedGameStateView();
        //     }
        //     else if (currentGameState.GetType().Equals(typeof(MissionFailedGameState)))
        //     {
        //         HandleSwitchToMissionFailedGameStateView();
        //     }
        // }

        public void SwitchToNewGameStateViewIfNeeded()
        {
            HandleSwitchToPlayingGameStateView();
        }

        private void HandleSwitchToPlayingGameStateView()
        {
            if (currentGameStateView == null || !currentGameStateView.GetType().Equals(typeof(PlayingGameStateView)))
            {
                currentGameStateView = new PlayingGameStateView();
            }
        }

        // private void HandleSwitchToMissionAccomplishedGameStateView()
        // {
        //     if (currentGameStateView == null || !currentGameStateView.GetType().Equals(typeof(MissionAccomplishedGameStateView)))
        //     {
        //         currentGameStateView = new MissionAccomplishedGameStateView();
        //     }
        // }
        //
        // private void HandleSwitchToMissionFailedGameStateView()
        // {
        //     if (currentGameStateView == null || !currentGameStateView.GetType().Equals(typeof(MissionFailedGameStateView)))
        //     {
        //         currentGameStateView = new MissionFailedGameStateView();
        //     }
        // }




    }
}
