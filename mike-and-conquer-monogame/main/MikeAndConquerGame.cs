using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using mike_and_conquer_simulation.main;
using MonoGame.Framework.Utilities;

namespace mike_and_conquer_monogame.main
{
    public class MikeAndConquerGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private ILogger logger;

        public MonogameSimulationStateListener monogameSimulationStateListener = null;

        public MikeAndConquerGame()
        {

            logger = MainProgram.loggerFactory.CreateLogger<MikeAndConquerGame>();
            logger.LogInformation("Game1() ctor");

            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            monogameSimulationStateListener = new MonogameSimulationStateListener(this);
            IsMouseVisible = true;
            // double currentResolution = TimerHelper.GetCurrentResolution();
            int x = 3;
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

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            

            base.Update(gameTime);
        }


        private bool hasMinigunnerBeenCreated = false;
        private int minigunnerX = -10;
        private int minigunnerY = -10;

        public void AddMinigunner(int x, int y)
        {
            hasMinigunnerBeenCreated = true;
            minigunnerX = x;
            minigunnerY = y;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();



            // Queue<SimulationStateUpdateEvent> simulationStateUpdateEventQueue =  SimulationMain.instance.GetSimulationStateUpdateEventQueue();
            // lock (simulationStateUpdateEventQueue)
            // {
            //     while (simulationStateUpdateEventQueue.Count > 0)
            //     {
            //         SimulationStateUpdateEvent anEvent =  simulationStateUpdateEventQueue.Dequeue();
            //         hasMinigunnerBeenCreated = true;
            //         minigunnerX = anEvent.X;
            //         minigunnerY = anEvent.Y;
            //
            //     }
            // }

            if (hasMinigunnerBeenCreated)
            {
                DrawRectangleAtCoordinate(minigunnerX, minigunnerY);
            }


            // TODO: Add your drawing code here

            _spriteBatch.End();
            base.Draw(gameTime);
        }

        void DrawRectangleAtCoordinate(int x, int y)
        {
            int width = 10;
            int height = 10;
            Texture2D rect = new Texture2D(GraphicsDevice, width, height);

            Color[] data = new Color[width * height];
            for (int i = 0; i < data.Length; ++i) data[i] = Color.Chocolate;
            rect.SetData(data);

            Vector2 coor = new Vector2(x, y);
            _spriteBatch.Draw(rect, coor, Color.White);


        }

        public void UpdateMinigunnerPosition(UnitPositionChangedEventData unitPositionChangedEventData)
        {
            minigunnerX = unitPositionChangedEventData.XInWorldCoordinates;
            minigunnerY = unitPositionChangedEventData.YInWorldCoordinates;
        }
    }
}
