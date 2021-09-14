using System;


namespace mike_and_conquer_monogame
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            mike_and_conquer_simulation.Class1 class1 = new mike_and_conquer_simulation.Class1();
            using (var game = new Game1())
                game.Run();
        }
    }
}
