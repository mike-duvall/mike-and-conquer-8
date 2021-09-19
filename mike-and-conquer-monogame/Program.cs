using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using mike_and_conquer_simulation;


namespace mike_and_conquer_monogame
{
    public class Program
    {


        public static ILoggerFactory loggerFactory;

        [STAThread]
        static void Main()
        {


            loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                    .AddFilter("Microsoft", LogLevel.Warning)
                    .AddFilter("System", LogLevel.Warning)
                    .AddFilter("mike_and_conquer_monogame.Program", LogLevel.Warning)
                    .AddDebug()
                    .AddConsole();
            });

            ILogger logger = loggerFactory.CreateLogger<Program>();
            logger.LogInformation("************************Mike is cool");


            mike_and_conquer_simulation.Program.RunRestServer();
            Program.RunRestServer();

            using (var game = new Game1())
                game.Run();
        }


        public static void RunRestServer()
        {
            var task = CreateHostBuilder(null).Build().RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((builderContext, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: false);
                })

                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<MonogameStartup>()
                        .UseUrls("http://*:5010");
                });


    }
}
