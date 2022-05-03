using mike_and_conquer_monogame.main;


namespace mike_and_conquer_monogame.commands
{
    public class AddMinigunnerCommand : AsyncViewCommand
    {


        private int id;
        private int x;
        private int y;

        public AddMinigunnerCommand(int id, int x, int y)
        {
            this.id = id;
            this.x = x;
            this.y = y;
        }

        protected override void ProcessImpl()
        {
            MikeAndConquerGame.instance.AddMinigunner(
                id, x, y);

        }
    }
}
