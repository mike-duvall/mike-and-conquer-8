using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using mike_and_conquer_simulation;


namespace mike_and_conquer_monogame
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {


            mike_and_conquer_simulation.Program.RunRestServer();
            Program.RunRestServer();

            using (var game = new Game1())
                game.Run();
        }


        public static void RunRestServer()
        {
            // CreateHostBuilder(null).Build().Run();
            var task = CreateHostBuilder(null).Build().RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<MonogameStartup>()
                        .UseUrls("http://*:5010");
                });


    }
}
