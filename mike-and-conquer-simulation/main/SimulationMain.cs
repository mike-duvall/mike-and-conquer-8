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


        private Queue<CreateMinigunnerEvent> createMinigunnerEventQueue;
        private Queue<SimulationStateUpdateEvent> simulationStateUpdateEventQueue;

        public static ILoggerFactory loggerFactory;

        private static ILogger logger;

        public static SimulationMain instance;

        public static ManualResetEvent condition;

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

            condition = new ManualResetEvent(false);
            Thread backgroundThread = new Thread(new ThreadStart(SimulationMain.Main));
            backgroundThread.IsBackground = true;

            // Start thread  
            backgroundThread.Start();
            condition.WaitOne();
        }


        public static void Main()
        {
            SimulationMain simulationMain = new SimulationMain();

            SimulationMain.condition.Set();
            while (true)
            {
                Thread.Sleep(17);
                simulationMain.ProcessCreateMinigunnerEventQueue();
//                logger.LogInformation("DateTime.Now:" + DateTime.Now.Millisecond);

            }

        }

        SimulationMain()
        {
            createMinigunnerEventQueue = new Queue<CreateMinigunnerEvent>();
            simulationStateUpdateEventQueue = new Queue<SimulationStateUpdateEvent>();

            SimulationMain.instance = this;
        }

        private void ProcessCreateMinigunnerEventQueue()
        {
            lock (createMinigunnerEventQueue)
            {
                while (createMinigunnerEventQueue.Count > 0)
                {
                    CreateMinigunnerEvent createMinigunnerEvent =  createMinigunnerEventQueue.Dequeue();
                    this.CreateMinigunnerFromEvent(createMinigunnerEvent);

                }
            }
        }

        public Queue<SimulationStateUpdateEvent>  GetSimulationStateUpdateEventQueue()
        {
            return simulationStateUpdateEventQueue;
        }
        private void CreateMinigunnerFromEvent(CreateMinigunnerEvent createMinigunnerEvent)
        {
            lock (simulationStateUpdateEventQueue)
            {
                SimulationStateUpdateEvent simulationStateUpdateEvent = new SimulationStateUpdateEvent();
                simulationStateUpdateEvent.ID = 1;
                simulationStateUpdateEvent.X = createMinigunnerEvent.X;
                simulationStateUpdateEvent.Y = createMinigunnerEvent.Y;

                simulationStateUpdateEventQueue.Enqueue(simulationStateUpdateEvent);
            }
            
        }

        public void CreateMinigunnerViaEvent(CreateMinigunnerEvent anEvent)
        {
            lock (createMinigunnerEventQueue)
            {
                createMinigunnerEventQueue.Enqueue(anEvent);
            }
        }
    }
}
