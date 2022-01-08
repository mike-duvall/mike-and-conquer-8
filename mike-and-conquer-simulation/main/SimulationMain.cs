
using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using mike_and_conquer_simulation.main.events;
using mike_and_conquer_simulation.simulationcommand;
using Newtonsoft.Json;

namespace mike_and_conquer_simulation.main
{
    public class SimulationMain
    {

        private Queue<AsyncSimulationCommand> inputEventQueue;

        private List<SimulationStateUpdateEvent> simulationStateUpdateEventsHistory;

        private List<SimulationStateListener> listeners;

        public static ILoggerFactory loggerFactory;

        private static ILogger logger;


        public static SimulationMain instance;

        public static ManualResetEvent condition;

        private List<Minigunner> minigunnerList;

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

            EmitInitializeScenarioEvent(27,23);


        }

        private static void EmitInitializeScenarioEvent(int mapWidth, int mapHeight)
        {
            SimulationStateUpdateEvent simulationStateUpdateEvent = new SimulationStateUpdateEvent();
            simulationStateUpdateEvent.EventType = InitializeScenarioEventData.EventName;
            InitializeScenarioEventData eventData = new InitializeScenarioEventData();

            eventData.MapWidth = mapWidth;
            eventData.MapHeight = mapHeight;

            simulationStateUpdateEvent.EventData = JsonConvert.SerializeObject(eventData);
            SimulationMain.instance.PublishEvent(simulationStateUpdateEvent);

        }


        public void AddListener(SimulationStateListener listener)
        {
            listeners.Add(listener);
        }


        public static void Main()
        {
            SimulationMain.condition.Set();
            long previousTicks = 0;
            while (true)
            {
                // int sleepTime = 23; // 7025  // Seems to be correct time for Fastest



                // int sleepTime = 252; // 75700, 75731 // Seems to be correct time for Slowest


                int sleepTime = 42; // 12733, 12703 // Seems to best correct time for Normal




                //Thread.Sleep(17);
                //Thread.Sleep(1);
                TimerHelper.SleepForNoMoreThan(sleepTime);
                 // TimerHelper.SleepForNoMoreThan(2);

                SimulationMain.instance.Tick();


                // SimulationMain.instance.ProcessInputEventQueue();
                
                // long currentTicks = DateTime.Now.Ticks;
                // // logger.LogInformation("DateTime.Now:" + DateTime.Now.Millisecond);
                // long delta = (currentTicks - previousTicks) / TimeSpan.TicksPerMillisecond;
                // previousTicks = currentTicks;
                // logger.LogInformation("delta=" + delta);


                bool doneWaiting = false;
                long delta = -1;
                long currentTicks = -1;

                while (!doneWaiting)
                {
                
                     currentTicks = DateTime.Now.Ticks;
                     delta = (currentTicks - previousTicks) / TimeSpan.TicksPerMillisecond;
                     if (delta >= sleepTime)
                     {
                         doneWaiting = true;
                     }


                }
                // logger.LogInformation("delta=" + delta);
                previousTicks = currentTicks;
            }

        }


        SimulationMain()
        {
            inputEventQueue = new Queue<AsyncSimulationCommand>();
            simulationStateUpdateEventsHistory = new List<SimulationStateUpdateEvent>();
            listeners = new List<SimulationStateListener>();
            listeners.Add(new SimulationStateHistoryListener(this));

            minigunnerList = new List<Minigunner>();

            SimulationMain.instance = this;
        }

        private void Tick()
        {
            Update();
            ProcessInputEventQueue();
        }

        private void Update()
        {
            foreach(Minigunner minigunner in minigunnerList)
            {
                minigunner.Update();
            }
        }

        private void ProcessInputEventQueue()
        {
            lock (inputEventQueue)
            {
                while (inputEventQueue.Count > 0)
                {
                    AsyncSimulationCommand anEvent = inputEventQueue.Dequeue();
                    anEvent.Process();
                }
            }
        }


        public bool  OrderUnitMoveViaEvent(int unitId, int destinationXInWorldCoordiantes,
            int destinationYInWorldCoordinates)
        {
            OrderUnitToMoveCommand anEvent = new OrderUnitToMoveCommand();
            anEvent.UnitId = unitId;
            anEvent.DestinationXInWorldCoordinates = destinationXInWorldCoordiantes;
            anEvent.DestinationYInWorldCoordinates = destinationYInWorldCoordinates;

            lock (inputEventQueue)
            {
                inputEventQueue.Enqueue(anEvent);
            }


            return true;

        }

        public Minigunner CreateMinigunnerViaEvent(int x, int y)
        {
            CreateMinigunnerCommand createMinigunnerCommand = new CreateMinigunnerCommand();
            createMinigunnerCommand.X = x;
            createMinigunnerCommand.Y = y;

            lock (inputEventQueue)
            {
                inputEventQueue.Enqueue(createMinigunnerCommand);
            }

            Minigunner gdiMinigunner = createMinigunnerCommand.GetMinigunner();
            return gdiMinigunner;

        }


        public List<SimulationStateUpdateEvent> GetCopyOfEventHistoryViaEvent()
        {
            GetCopyOfEventHistoryCommand anEvent = new GetCopyOfEventHistoryCommand();

            lock (inputEventQueue)
            {
                inputEventQueue.Enqueue(anEvent);
            }

            List<SimulationStateUpdateEvent> list = anEvent.GetCopyOfEventHistory();
            return list;
        }


        public Minigunner CreateMinigunner(int minigunnerX, int minigunnerY)
        {

            Minigunner minigunner = new Minigunner();
            minigunner.GameWorldLocation.X = minigunnerX;
            minigunner.GameWorldLocation.Y = minigunnerY;
            minigunner.ID = 1;

            minigunnerList.Add(minigunner);

            SimulationStateUpdateEvent simulationStateUpdateEvent = new SimulationStateUpdateEvent();
            simulationStateUpdateEvent.EventType = MinigunnerCreateEventData.EventName;
            MinigunnerCreateEventData eventData = new MinigunnerCreateEventData();
            eventData.ID = minigunner.ID;
            eventData.X = minigunnerX;
            eventData.Y = minigunnerY;

            simulationStateUpdateEvent.EventData = JsonConvert.SerializeObject(eventData);

            foreach (SimulationStateListener listener in listeners)
            {
                listener.Update(simulationStateUpdateEvent);
            }

            return minigunner;
        }

        public void OrderUnitToMove(int unitId, int destinationXInWorldCoordinates, int destinationYInWorldCoordinates)
        {

            Minigunner foundMinigunner = FindMinigunnerWithUnitId(unitId);

            foundMinigunner.OrderMoveToDestination(destinationXInWorldCoordinates, destinationYInWorldCoordinates);

            SimulationStateUpdateEvent simulationStateUpdateEvent = new SimulationStateUpdateEvent();
            simulationStateUpdateEvent.EventType = "UnitOrderedToMove";
            UnitMoveOrderEventData eventData = new UnitMoveOrderEventData();
            eventData.ID = unitId;
            eventData.DestinationXInWorldCoordinates = destinationXInWorldCoordinates;
            eventData.DestinationYInWorldCoordinates = destinationYInWorldCoordinates;
            eventData.Timestamp = DateTime.Now.Ticks;

            simulationStateUpdateEvent.EventData = JsonConvert.SerializeObject(eventData);

            foreach (SimulationStateListener listener in listeners)
            {
                listener.Update(simulationStateUpdateEvent);
            }

        }
        private Minigunner FindMinigunnerWithUnitId(int unitId)
        {
            Minigunner foundMinigunner = null;

            foreach (Minigunner minigunner in minigunnerList)
            {
                if (minigunner.ID == unitId)
                {
                    foundMinigunner = minigunner;
                }
            }

            return foundMinigunner;
        }

        public List<SimulationStateUpdateEvent> GetCopyOfEventHistory()
        {
            List<SimulationStateUpdateEvent> copyList = new List<SimulationStateUpdateEvent>();

            foreach (SimulationStateUpdateEvent simulationStateUpdateEvent in simulationStateUpdateEventsHistory)
            {
                SimulationStateUpdateEvent copyEvent = new SimulationStateUpdateEvent();
                copyEvent.EventType = simulationStateUpdateEvent.EventType;
                // MinigunnerCreateEventData copyEventData = new MinigunnerCreateEventData();
                //
                // copyEventData.ID = simulationStateUpdateEvent.ID;
                // copyEventData.X = simulationStateUpdateEvent.X;
                // copyEventData.Y = simulationStateUpdateEvent.Y;
                String copyEventData = new string(simulationStateUpdateEvent.EventData);

                copyEvent.EventData = copyEventData;
                copyList.Add(copyEvent);
            }

            return copyList;
        }


        public void AddHistoryEvent(SimulationStateUpdateEvent anEvent)
        {
            simulationStateUpdateEventsHistory.Add(anEvent); 
        }

        public void PublishEvent(SimulationStateUpdateEvent simulationStateUpdateEvent)
        {
            foreach (SimulationStateListener listener in listeners)
            {
                listener.Update(simulationStateUpdateEvent);
            }
        }
    }
}
