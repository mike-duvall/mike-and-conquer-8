
using System.Collections.Generic;
using System.Threading;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using mike_and_conquer_simulation.simulationevent;

namespace mike_and_conquer_simulation.main
{
    public class SimulationMain
    {


        private Queue<CreateMinigunnerEvent> createMinigunnerEventQueue;
        public List<SimulationStateUpdateEvent> publishedSimulationStateUpdateEvents;

        private List<SimulationStateListener> listeners;


        public static ILoggerFactory loggerFactory;

        private static ILogger logger;

        public static SimulationMain instance;

        public static ManualResetEvent condition;

        public static void StartSimulation(SimulationStateListener listener)
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


            new SimulationMain();
            SimulationMain.instance.AddListener(listener);

            condition = new ManualResetEvent(false);
            Thread backgroundThread = new Thread(new ThreadStart(SimulationMain.Main));
            backgroundThread.IsBackground = true;

            // Start thread  
            backgroundThread.Start();
            condition.WaitOne();
        }

        private void AddListener(SimulationStateListener listener)
        {
            listeners.Add(listener);
        }


        public static void Main()
        {
//            SimulationMain simulationMain = new SimulationMain();

            SimulationMain.condition.Set();
            while (true)
            {
                Thread.Sleep(17);
                SimulationMain.instance.ProcessCreateMinigunnerEventQueue();
//                logger.LogInformation("DateTime.Now:" + DateTime.Now.Millisecond);

            }

        }

        SimulationMain()
        {
            createMinigunnerEventQueue = new Queue<CreateMinigunnerEvent>();
            publishedSimulationStateUpdateEvents = new List<SimulationStateUpdateEvent>();
            listeners = new List<SimulationStateListener>();

            SimulationMain.instance = this;
        }

        private void ProcessCreateMinigunnerEventQueue()
        {
            lock (createMinigunnerEventQueue)
            {
                while (createMinigunnerEventQueue.Count > 0)
                {
                    CreateMinigunnerEvent createMinigunnerEvent =  createMinigunnerEventQueue.Dequeue();
                    createMinigunnerEvent.Process();
                }
            }
        }


        public Minigunner CreateMinigunnerViaEvent(int x, int y)
        {
            CreateMinigunnerEvent createMinigunnerEvent = new CreateMinigunnerEvent();
            createMinigunnerEvent.X = x;
            createMinigunnerEvent.Y = y;

            lock (createMinigunnerEventQueue)
            {
                createMinigunnerEventQueue.Enqueue(createMinigunnerEvent);
            }

            Minigunner gdiMinigunner = createMinigunnerEvent.GetMinigunner();
            return gdiMinigunner;

        }

        public Minigunner CreateMinigunner(int minigunnerX, int minigunnerY)
        {

            Minigunner minigunner = new Minigunner();
            minigunner.X = minigunnerX;
            minigunner.Y = minigunnerY;
            minigunner.ID = 1;

            SimulationStateUpdateEvent simulationStateUpdateEvent = new SimulationStateUpdateEvent();
            simulationStateUpdateEvent.ID = minigunner.ID;
            simulationStateUpdateEvent.X = minigunnerX;
            simulationStateUpdateEvent.Y = minigunnerY;

            foreach (SimulationStateListener listener in listeners)
            {
                listener.Update(simulationStateUpdateEvent);
            }

            lock (publishedSimulationStateUpdateEvents)
            {
                publishedSimulationStateUpdateEvents.Add(simulationStateUpdateEvent);
            }

            return minigunner;
        }
    }
}
