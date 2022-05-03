using mike_and_conquer_monogame.main;


namespace mike_and_conquer_monogame.commands
{
    public class AddMCVCommand : AsyncViewCommand
    {


        private int id;
        private int x;
        private int y;

        public AddMCVCommand(int id, int x, int y)
        {
            this.id = id;
            this.x = x;
            this.y = y;
        }

        protected override void ProcessImpl()
        {
            MikeAndConquerGame.instance.AddMCV(
                id, x, y);

        }
    }
}
