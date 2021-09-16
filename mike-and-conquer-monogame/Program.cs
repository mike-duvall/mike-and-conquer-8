using System;


namespace mike_and_conquer_monogame
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {


            mike_and_conquer_simulation.Program.RunRestServer();
            using (var game = new Game1())
                game.Run();
        }
    }
}
