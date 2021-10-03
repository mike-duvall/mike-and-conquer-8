using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using mike_and_conquer_monogame.init;
using mike_and_conquer_simulation;
using mike_and_conquer_simulation.main;
using mike_and_conquer_simulation.rest.init;


namespace mike_and_conquer_monogame.main
{
    public class MainProgram
    {


        public static ILoggerFactory loggerFactory;



        [STAThread]
        static void Main()
        {

            var configuration = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json", optional: true, reloadOnChange: false)
                .Build();

            var loggingConfig = configuration.GetSection("Logging");

            loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                    .AddDebug()
                    .AddConsole()
                    .AddConfiguration(loggingConfig)
                    ;
            });

            
            ILogger logger = loggerFactory.CreateLogger<MainProgram>();
            logger.LogInformation("************************Mike is cool");
            logger.LogWarning("************************Mike is cool");


            SimulationRestInitializer.RunRestServer();
            // SimulationRestProgram.RunRestServer();
            SimulationMain.StartSimulation();

            using (var game = new MikeAndConquerGame())
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
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                    logging.AddDebug();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<MonogameRestStartup>()
                        .UseUrls("http://*:5010");
                });


    }
}
