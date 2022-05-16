
using Microsoft.Xna.Framework.Input;

namespace mike_and_conquer.gameworld.humancontroller
{
    public abstract class HumanControllerState
    {
        public abstract HumanControllerState Update(
            MouseState newMouseState,
            MouseState oldMouseState);

    }
}

