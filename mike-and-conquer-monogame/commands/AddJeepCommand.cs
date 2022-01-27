using mike_and_conquer_monogame.main;
using mike_and_conquer_simulation.commands;

namespace mike_and_conquer_monogame.commands
{
    public class AddJeepCommand : AsyncViewCommand
    {


        private int id;
        private int x;
        private int y;

        public AddJeepCommand(int id, int x, int y)
        {
            this.id = id;
            this.x = x;
            this.y = y;
        }

        protected override void ProcessImpl()
        {
            MikeAndConquerGame.instance.AddJeep(
                id, x, y);

        }
    }
}
