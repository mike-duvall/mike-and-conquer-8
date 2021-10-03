using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace mike_and_conquer_simulation.main
{
    public class SimulationMain
    {

        public static ILoggerFactory loggerFactory;

        private static ILogger logger;

        public static void StartSimulation()
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


            logger = loggerFactory.CreateLogger<SimulationMain>();
            logger.LogInformation("************************Simulation Mike is cool");
            logger.LogWarning("************************Simulation Mike is cool");


            Thread backgroundThread = new Thread(new ThreadStart(SimulationMain.Main));

            // Start thread  
            backgroundThread.Start();
        }


        public static void Main()
        {

            while (true)
            {
                Thread.Sleep(17);
//                logger.LogInformation("DateTime.Now:" + DateTime.Now.Millisecond);
            }

        }

    }
}
