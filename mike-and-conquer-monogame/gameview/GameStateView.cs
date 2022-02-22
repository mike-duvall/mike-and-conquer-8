
using GameTime = Microsoft.Xna.Framework.GameTime;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;

namespace mike_and_conquer.gameview
{
    abstract class GameStateView
    {
        public abstract void Draw(GameTime gameTime);
        public abstract void Update(GameTime gameTime);
    }
}
