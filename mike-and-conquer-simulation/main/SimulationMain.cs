
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using mike_and_conquer_simulation.commands;
using mike_and_conquer_simulation.events;
using Newtonsoft.Json;

namespace mike_and_conquer_simulation.main
{
    public class SimulationMain
    {

        private SimulationOptions simulationOptions;

        private Queue<AsyncSimulationCommand> inputCommandQueue;

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
            SimulationMain.instance.SetGameSpeed(SimulationOptions.GameSpeed.Normal);

            while (true)
            {


                int sleepTime = (int) SimulationMain.instance.simulationOptions.CurrentGameSpeed;
                TimerHelper.SleepForNoMoreThan(sleepTime);


                SimulationMain.instance.Tick();

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
            inputCommandQueue = new Queue<AsyncSimulationCommand>();
            simulationStateUpdateEventsHistory = new List<SimulationStateUpdateEvent>();
            listeners = new List<SimulationStateListener>();
            listeners.Add(new SimulationStateHistoryListener(this));

            minigunnerList = new List<Minigunner>();

            simulationOptions = new SimulationOptions();

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
            lock (inputCommandQueue)
            {
                while (inputCommandQueue.Count > 0)
                {
                    AsyncSimulationCommand anEvent = inputCommandQueue.Dequeue();
                    anEvent.Process();
                }
            }
        }


        public void  PostOrderUnitMoveCommand(int unitId, int destinationXInWorldCoordiantes,
            int destinationYInWorldCoordinates)
        {
            OrderUnitToMoveCommand anEvent = new OrderUnitToMoveCommand();
            anEvent.UnitId = unitId;
            anEvent.DestinationXInWorldCoordinates = destinationXInWorldCoordiantes;
            anEvent.DestinationYInWorldCoordinates = destinationYInWorldCoordinates;

            lock (inputCommandQueue)
            {
                inputCommandQueue.Enqueue(anEvent);
            }

        }

        public bool SetGameSpeedViaEvent(SimulationOptions.GameSpeed aGameSpeed)
        {
            SetGameSpeedCommand aCommand = new SetGameSpeedCommand();
            aCommand.GameSpeed = aGameSpeed;

            lock (inputCommandQueue)
            {
                inputCommandQueue.Enqueue(aCommand);
            }

            return true;

        }

        public void PostCreateMinigunnerCommand(int x, int y)
        {
            CreateMinigunnerCommand createMinigunnerCommand = new CreateMinigunnerCommand();
            createMinigunnerCommand.X = x;
            createMinigunnerCommand.Y = y;

            lock (inputCommandQueue)
            {
                inputCommandQueue.Enqueue(createMinigunnerCommand);
            }

        }


        public void PostResetScenarioCommand()
        {
            ResetScenarioCommand command = new ResetScenarioCommand();
            lock (inputCommandQueue)
            {
                inputCommandQueue.Enqueue(command);
            }

        }


        public List<SimulationStateUpdateEvent> GetCopyOfEventHistoryViaEvent()
        {
            GetCopyOfEventHistoryCommand anEvent = new GetCopyOfEventHistoryCommand();

            lock (inputCommandQueue)
            {
                inputCommandQueue.Enqueue(anEvent);
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

        public void SetGameSpeed(SimulationOptions.GameSpeed aGameSpeed)
        {
            this.simulationOptions.CurrentGameSpeed = aGameSpeed;
        }


        public void ResetScenario()
        {

            lock (simulationStateUpdateEventsHistory)
            {
                simulationStateUpdateEventsHistory.Clear();
            }

            lock (minigunnerList)
            {
                minigunnerList.Clear();
            }
        }
    }
}
