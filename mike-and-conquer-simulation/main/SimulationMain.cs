﻿
using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using mike_and_conquer_simulation.rest.simulationevent;
using Newtonsoft.Json;

namespace mike_and_conquer_simulation.main
{
    public class SimulationMain
    {

        private Queue<AsyncGameEvent> inputEventQueue;



        private List<SimulationStateUpdateEvent> simulationStateUpdateEventsHistory;

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

        public void AddListener(SimulationStateListener listener)
        {
            listeners.Add(listener);
        }


        public static void Main()
        {
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
            inputEventQueue = new Queue<AsyncGameEvent>();
            simulationStateUpdateEventsHistory = new List<SimulationStateUpdateEvent>();
            listeners = new List<SimulationStateListener>();
            listeners.Add(new SimulationStateHistoryListener(this));

            SimulationMain.instance = this;
        }

        private void ProcessCreateMinigunnerEventQueue()
        {
            lock (inputEventQueue)
            {
                while (inputEventQueue.Count > 0)
                {
                    AsyncGameEvent anEvent = inputEventQueue.Dequeue();
                    anEvent.Process();
                }
            }
        }


        public bool  OrderUnitMoveViaEvent(int unitId, int destinationXInWorldCoordiantes,
            int destinationYInWorldCoordinates)
        {
            // CreateMinigunnerEvent createMinigunnerEvent = new CreateMinigunnerEvent();
            // createMinigunnerEvent.X = x;
            // createMinigunnerEvent.Y = y;
            //
            // lock (inputEventQueue)
            // {
            //     inputEventQueue.Enqueue(createMinigunnerEvent);
            // }

            // Minigunner gdiMinigunner = createMinigunnerEvent.GetMinigunner();

            OrderUnitToMoveEvent anEvent = new OrderUnitToMoveEvent();
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
            CreateMinigunnerEvent createMinigunnerEvent = new CreateMinigunnerEvent();
            createMinigunnerEvent.X = x;
            createMinigunnerEvent.Y = y;

            lock (inputEventQueue)
            {
                inputEventQueue.Enqueue(createMinigunnerEvent);
            }

            Minigunner gdiMinigunner = createMinigunnerEvent.GetMinigunner();
            return gdiMinigunner;

        }


        public List<SimulationStateUpdateEvent> GetCopyOfEventHistoryViaEvent()
        {
            GetCopyOfEventHistoryEvent anEvent = new GetCopyOfEventHistoryEvent();

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
            minigunner.X = minigunnerX;
            minigunner.Y = minigunnerY;
            minigunner.ID = 1;

            SimulationStateUpdateEvent simulationStateUpdateEvent = new SimulationStateUpdateEvent();
            simulationStateUpdateEvent.EventType = "MinigunnerCreated";
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

            //throw new System.NotImplementedException();
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

    }
}
